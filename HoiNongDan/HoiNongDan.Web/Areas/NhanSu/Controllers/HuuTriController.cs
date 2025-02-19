using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.ViewModels.Masterdata;
using HoiNongDan.Resources;
using HoiNongDan.Web.Areas.NhanSu.Models;
using System.Diagnostics;
using System.IO;
using System.Transactions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using HoiNongDan.Models.Entitys.MasterData;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Authorize]
    [Area(ConstArea.NhanSu)]
    public class HuuTriController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public HuuTriController(AppDbContext context, IWebHostEnvironment hostEnvironment) : base(context) {
            _hostEnvironment = hostEnvironment;
        }
        #region Index
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Index()
        {
            return View(new HuuTriSearchVM());
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(HuuTriSearchVM search) {
            return ExecuteSearch(() => {
                var data = _context.HuuTris.AsQueryable();
                if (!String.IsNullOrEmpty(search.SoQuyetDinh) && !String.IsNullOrWhiteSpace(search.SoQuyetDinh))
                {
                    data = data.Where(it => it.SoQuyetDinh == search.SoQuyetDinh);
                }
                data = data.Include(it => it.CanBo);
                if (!String.IsNullOrEmpty(search.MaCanBo) && !String.IsNullOrWhiteSpace(search.MaCanBo))
                {
                    data = data.Where(it => it.CanBo!.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen) && !String.IsNullOrWhiteSpace(search.HoVaTen))
                {
                    data = data.Where(it => it.CanBo!.HoVaTen.Contains(search.HoVaTen));
                }
                var model = data.Select(it => new HuuTriDetail {
                    Id = it.Id,
                    IDCanBo = it.IDCanBo,
                    HoVaTen = it.CanBo!.HoVaTen,
                    MaCanBo = it.CanBo!.MaCanBo,
                    SoQuyetDinh = it.SoQuyetDinh,
                    NgayQuyetDinh = it.NgayQuyetDinh,
                    NguoiKy = it.NguoiKy,
                    GhiChu = it.GhiChu,
                    TenCoSo = it.CanBo.CoSo.TenCoSo,
                    TenChucVu = it.CanBo.ChucVu.TenChucVu,
                    TenDonVi = it.CanBo.Department.Name
                });
                return PartialView(model);
            });
        }
        #endregion Idex
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create()
        {
            HuuTriVM huuTri = new HuuTriVM();

            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu.CanBo = true;
            huuTri.NhanSu = nhanSu;
            return View(huuTri);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HuuTriMTVM obj, IFormFile? fileInbox) {
            CheckError(obj);
            var add = new HuuTri();
            add = obj.GetHuuChi(add);
            add.Id = Guid.NewGuid();

            FileDinhKemModel addFile = new FileDinhKemModel();
            if (fileInbox != null)
            {
                addFile.Id = add.Id;
                addFile.IdCanBo = add.IDCanBo;
                addFile.IDLoaiDinhKem = "03";
                FunctionFile.CopyFile(_hostEnvironment, fileInbox, addFile);
                if (!String.IsNullOrEmpty(addFile.Error) && !String.IsNullOrWhiteSpace(addFile.Error))
                {
                    ModelState.AddModelError("fileInbox", "Lỗi không cập nhật được file đính kèm");
                }
            }
            return ExecuteContainer(() => {
                add.Actived = true;
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                //var canBo = _context.CanBos.SingleOrDefault(it => it.IDCanBo == add.IDCanBo);
                if (!String.IsNullOrEmpty(addFile.Url) && !String.IsNullOrWhiteSpace(addFile.Url))
                {
                    FileDinhKem fileDinhKem = addFile.GetFileDinhKem();
                    _context.FileDinhKems.Add(fileDinhKem);
                }
                if (obj.CapNhatTinhTrangCanBo)
                {
                    CapNhatNhanSu(obj.NhanSu.IdCanbo!.Value);
                }
                _context.HuuTris.Add(add);

                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.HuuTri.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit 
        public IActionResult Edit(Guid id) {
            var item = _context.HuuTris.SingleOrDefault(it => it.Id == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            HuuTriVM obj = new HuuTriVM();
            obj.CapNhatTinhTrangCanBo = false;
            var file = _context.FileDinhKems.SingleOrDefault(it => it.Id == id);

            var canBo = _context.CanBos.Include(it => it.CoSo).Include(it => it.Department)
                        .Include(it => it.PhanHe).Include(it => it.TinhTrang).Where(it => it.IDCanBo == item.IDCanBo).SingleOrDefault();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu = nhanSu.GeThongTin(canBo!);
            nhanSu.CanBo = true;
            nhanSu.IdCanbo = canBo!.IDCanBo;
            nhanSu.HoVaTen = canBo.HoVaTen;
            nhanSu.MaCanBo = canBo.MaCanBo;
            nhanSu.TenTinhTrang = canBo.TinhTrang.TenTinhTrang;
            //nhanSu.TenCoSo = canBo.CoSo.TenCoSo;
            nhanSu.TenDonVi = canBo.Department.Name;
            //nhanSu.TenPhanHe = canBo.PhanHe.TenPhanHe;
            nhanSu.Edit = false;


            obj.NgayQuyetDinh = item.NgayQuyetDinh;
            obj.SoQuyetDinh = item.SoQuyetDinh;
            obj.NguoiKy = item.NguoiKy;
            obj.GhiChu = item.GhiChu;
            obj.NhanSu = nhanSu;
            obj.Id = item.Id;
            obj.FileDinhKem = file;
            return View(obj);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HuuTriMTVM obj, IFormFile? fileInbox) {
            CheckError(obj);
            return ExecuteContainer(() => {
                var edit = _context.HuuTris.SingleOrDefault(it => it.Id == obj.Id!.Value);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.HuuTri.ToLower())
                    });
                }
                else
                {
                    using (TransactionScope tran = new TransactionScope())
                    {
                        edit = obj.GetHuuChi(edit);
                        edit.Actived = obj.Actived;
                        edit.LastModifiedTime = DateTime.Now;
                        edit.LastModifiedAccountId = AccountId();
                        FileDinhKemModel addFile = new FileDinhKemModel();
                        if (fileInbox != null)
                        {
                            var existFile = _context.FileDinhKems.SingleOrDefault(it => it.Id == edit.Id);
                            if (existFile != null)
                            {
                                FunctionFile.Delete(_hostEnvironment, existFile.Url);
                                _context.FileDinhKems.Remove(existFile);
                            }
                            addFile.Id = edit.Id;
                            addFile.IdCanBo = edit.IDCanBo;
                            addFile.IDLoaiDinhKem = "03";
                            FunctionFile.CopyFile(_hostEnvironment, fileInbox, addFile);
                            if (!String.IsNullOrEmpty(addFile.Error) && !String.IsNullOrWhiteSpace(addFile.Error))
                            {
                                ModelState.AddModelError("fileInbox", "Lỗi không cập nhật được file đính kèm");
                            }
                        }
                        if (!String.IsNullOrEmpty(addFile.Url) && !String.IsNullOrWhiteSpace(addFile.Url))
                        {
                            FileDinhKem fileDinhKem = addFile.GetFileDinhKem();
                            _context.FileDinhKems.Add(fileDinhKem);
                        }
                        if (obj.CapNhatTinhTrangCanBo)
                        {
                            CapNhatNhanSu(edit.IDCanBo);
                        }
                        _context.Entry(edit).State = EntityState.Modified;
                        _context.SaveChanges();
                        tran.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.HuuTri.ToLower())
                        });
                    }
                }
            });
        }


        #endregion Edit
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                using (TransactionScope tran = new TransactionScope())
                {
                    var del = _context.HuuTris.FirstOrDefault(p => p.Id == id);
                    if (del != null)
                    {
                        var existFile = _context.FileDinhKems.SingleOrDefault(it => it.Id == id);
                        if (existFile != null)
                        {
                            FunctionFile.Delete(_hostEnvironment, existFile.Url);
                            _context.FileDinhKems.Remove(existFile);
                        }
                        _context.Remove(del);
                        _context.SaveChanges();
                        tran.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.HuuTri.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.HuuTri.ToLower())
                        });
                    }
                }

            });
        }
        #endregion Delete
        public IActionResult XetNghiHuu()
        {
            CreateViewBag();
            XetNghiHuuSearchVM search = new XetNghiHuuSearchVM();
            search.TuNgay = DateTime.Now;
            search.Nam_Tuoi = 60;
            search.Nam_Thang = 9;
            search.Nu_Tuoi = 56;
            search.Nu_Thang = 0;
            return View(search);
        }
        public IActionResult _DSDenTuoiNghiHuu(XetNghiHuuSearchVM search) {
            return ExecuteSearch(() => {
                //var model = _context.CanBos.FromSqlRaw<CanBo>(@"EXECUTE dbo.GetDanhSachDenTuoiNghiHuu {0},{1},{2},{3},{4},{5},{6}",
                //    search.TuNgay, search.Nam_Tuoi!, search.Nam_Thang!, search.Nu_Tuoi!, search.Nu_Thang!, search.IdCoSo!, search.IdDepartment!)
                //.ToList();
                //var coSo = _context.CoSos.ToList();
                //var donVi = _context.Departments.ToList();
                //var phanHe = _context.PhanHes.ToList();
                //var chucVu = _context.ChucVus.ToList();
                //var bacLuong = _context.BacLuongs.ToList();
                //var dataView = new List<CanBoDenTuoiNghiHuuVM>();
                //foreach (var item in model)
                //{
                //    var temp = coSo.SingleOrDefault(it => it.IdCoSo == item.IdCoSo);
                //    var temp1 = donVi.SingleOrDefault(it => it.Id == item.IdDepartment);
                //    var temp2 = phanHe.SingleOrDefault(it => it.MaPhanHe == item.MaPhanHe);
                //    var temp3 = chucVu.SingleOrDefault(it => it.MaChucVu == item.MaChucVu);
                //    var temp4 = bacLuong.SingleOrDefault(it => it.MaBacLuong == item.MaBacLuong);

                //    dataView.Add(new CanBoDenTuoiNghiHuuVM
                //    { 
                //        MaCanBo = item.MaCanBo,
                //        HoVaTen = item.HoVaTen,
                //        HeSo = item.HeSoLuong,
                //        TenNgachLuong = item.MaNgachLuong!,
                //        TenPhanHe = temp2!= null? temp2.TenPhanHe:"",
                //        TenCoSo = temp!= null? temp.TenCoSo:"",
                //        TenDonVi = temp1 != null? temp1.Name:"",
                //        TenBacLuong = temp4!= null?temp4.TenBacLuong:"",
                //        TenChucVu = temp3 != null? temp3.TenChucVu:"",
                //        Tuoi = GetMonth(item.NgaySinh,search.TuNgay)/12,
                //        Thang = GetMonth(item.NgaySinh, search.TuNgay) % 12
                //    });
                //}
                var model = _context.XetNghiHuu(search);
                return PartialView(model);
            });
        }
        public void CreateViewBag(Guid? IDCoSo = null) {
            var MenuListCoSo = _context.CoSos.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { IdCoSo = it.IdCoSo, TenCoSo = it.TenCoSo }).ToList();
            ViewBag.IdCoSo = new SelectList(MenuListCoSo, "IdCoSo", "TenCoSo");

            var DonVi = _context.Departments.Where(it => it.Actived == true && it.IDCoSo == IDCoSo).Include(it => it.CoSo).OrderBy(p => p.OrderIndex).Select(it => new { IdDepartment = it.Id, Name = it.Name + " " + it.CoSo.TenCoSo }).ToList();
            ViewBag.IdDepartment = new SelectList(DonVi, "IdDepartment", "Name");
        }
        #region Helper
        private void CheckError(HuuTriMTVM obj)
        {
            if (obj.NhanSu.IdCanbo == null)
            {
                ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
            }
            if (obj.Id == null)
            {
                var exist = _context.HuuTris.SingleOrDefault(it => it.IDCanBo == obj.NhanSu.IdCanbo!.Value);
                if (exist != null)
                {
                    ModelState.AddModelError("SoQuyetDinh", "Đã tồn tại thông tin nghỉ hưu");
                }
            }
            
        }
        private void CapNhatNhanSu(Guid IDCanBo) {
            var canBo = _context.CanBos.SingleOrDefault(it => it.IDCanBo == IDCanBo);
            if (canBo != null) {
                canBo.MaTinhTrang = "09";
                HistoryModelRepository history = new HistoryModelRepository(_context);
                history.SaveUpdateHistory(canBo.IDCanBo.ToString(), AccountId()!.Value, canBo);
            }
        }
        //private int GetAge(DateTime birthDate,DateTime approveDate) { 
        //    var age =   (new DateTime(approveDate.Subtract(approveDate).Ticks)).Year-1;
        //    return age;
        //}
        private int GetMonth(DateTime birthDate, DateTime approveDate) { 
            int month = (new DateTime(approveDate.Subtract(approveDate).Ticks)).Month - 1;
            return month;
        }
        #endregion Helper
    }
}
