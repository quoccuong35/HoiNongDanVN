using Microsoft.AspNetCore.Mvc;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models.ViewModels.Masterdata;
using HoiNongDan.Models;
using HoiNongDan.Web.Areas.NhanSu.Models;
using Microsoft.EntityFrameworkCore;
using HoiNongDan.Resources;
using System.Transactions;
using Microsoft.AspNetCore.Mvc.Rendering;
using HoiNongDan.Constant;
using Microsoft.AspNetCore.Authorization;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    [Authorize]
    public class MienNhiemController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public MienNhiemController(AppDbContext context, IWebHostEnvironment hostEnvironment) : base(context)
        {
            _hostEnvironment = hostEnvironment;
        }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult _Search(MienNhiemSearchVM search)
        {
            return ExecuteSearch(() => {
                var data = _context.QuaTrinhMienNhiems.AsQueryable();
                if (!String.IsNullOrEmpty(search.SoQuyetDinh) && !String.IsNullOrWhiteSpace(search.SoQuyetDinh))
                {
                    data = data.Where(it => it.SoQuyetDinh == search.SoQuyetDinh);
                }
                data = data.Include(it => it.CanBo).Include(it => it.ChucVu).Include(it => it.CoSo).Include(it => it.Department);
                if (!String.IsNullOrEmpty(search.MaCanBo) && !String.IsNullOrWhiteSpace(search.MaCanBo))
                {
                    data = data.Where(it => it.CanBo.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen) && !String.IsNullOrWhiteSpace(search.HoVaTen))
                {
                    data = data.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
                }
                var model = data.Select(it => new MienNhiemDetail
                {
                    IDQuaTrinhMienNhiem = it.IDQuaTrinhMienNhiem,
                    MaCanBo = it.CanBo.MaCanBo,
                    HoVaTen = it.CanBo.HoVaTen,
                    SoQuyetDinh = it.SoQuyetDinh,
                    NgayQuyetDinh = it.NgayQuyetDinh,
                    NguoiKy = it.NguoiKy,
                    HeSoChucVu = it.HeSoChucVu,
                    GhiChu = it.GhiChu,
                    TenChucVu = it.ChucVu.TenChucVu,
                    TenDonVi = it.Department.Name,
                    TenCoSo = it.CoSo.TenCoSo
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create()
        {
            MienNhiemVM mienNhiem = new MienNhiemVM();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu.CanBo = true;
            mienNhiem.NhanSu = nhanSu;
            CreateViewBag();
            return View(mienNhiem);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        public JsonResult Create(MienNhiemMTVM obj, IFormFile? fileInbox)
        {
            CheckError(obj);
            var add = new QuaTrinhMienNhiem();
            add = obj.GetMienNhiem(add);
            add.IDQuaTrinhMienNhiem = Guid.NewGuid();
            FileDinhKemModel addFile = new FileDinhKemModel();
            if (fileInbox != null)
            {
                addFile.Id = add.IDQuaTrinhMienNhiem;
                addFile.IdCanBo = add.IDCanBo;
                addFile.IDLoaiDinhKem = "02";
                FunctionFile.CopyFile(_hostEnvironment, fileInbox, addFile);
                if (!String.IsNullOrEmpty(addFile.Error) && !String.IsNullOrWhiteSpace(addFile.Error))
                {
                    ModelState.AddModelError("fileInbox", "Lỗi không cập nhật được file đính kèm");
                }
            }
            return ExecuteContainer(() => {
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                var canBo = _context.CanBos.SingleOrDefault(it => it.IDCanBo == add.IDCanBo);
                //_context.Attach(add).State = EntityState.Modified;
                if (!String.IsNullOrEmpty(addFile.Url) && !String.IsNullOrWhiteSpace(addFile.Url))
                {
                    FileDinhKem fileDinhKem = addFile.GetFileDinhKem();
                    _context.FileDinhKems.Add(fileDinhKem);
                }
                _context.QuaTrinhMienNhiems.Add(add);

                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MienNhiem.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit 
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id)
        {
            var item = _context.QuaTrinhMienNhiems.SingleOrDefault(it => it.IDQuaTrinhMienNhiem == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            MienNhiemVM obj = new MienNhiemVM();
            var file = _context.FileDinhKems.SingleOrDefault(it => it.Id == id);

            var canBo = _context.CanBos.Include(it => it.CoSo).Include(it => it.Department)
                        .Include(it => it.PhanHe).Include(it => it.TinhTrang).Where(it => it.IDCanBo == item.IDCanBo && it.IsCanBo == true).SingleOrDefault();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu = nhanSu.GeThongTin(canBo);
            nhanSu.CanBo = true;
            nhanSu.IdCanbo = canBo!.IDCanBo;
            nhanSu.HoVaTen = canBo.HoVaTen;
            nhanSu.MaCanBo = canBo.MaCanBo;
            nhanSu.TenTinhTrang = canBo.TinhTrang.TenTinhTrang;
            nhanSu.TenCoSo = canBo.CoSo.TenCoSo;
            nhanSu.TenDonVi = canBo.Department.Name;
            nhanSu.TenPhanHe = canBo.PhanHe.TenPhanHe;
            nhanSu.Edit = false;

            obj.IdCoSo = item.IdCoSo;
            obj.IdDepartment = item.IdDepartment;
            obj.MaChucVu = item.MaChucVu;
            obj.NgayQuyetDinh = item.NgayQuyetDinh;
            obj.SoQuyetDinh = item.SoQuyetDinh;
            obj.HeSoChucVu = item.HeSoChucVu;
            obj.NguoiKy = item.NguoiKy;
            obj.GhiChu = item.GhiChu;
            obj.NhanSu = nhanSu;
            obj.IDQuaTrinhMienNhiem = item.IDQuaTrinhMienNhiem;
            obj.FileDinhKem = file;



            CreateViewBag(item.MaChucVu, item.IdCoSo, item.IdDepartment);
            return View(obj);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Edit(MienNhiemMTVM obj, IFormFile? fileInbox)
        {
            CheckError(obj);
            return ExecuteContainer(() => {
                var edit = _context.QuaTrinhMienNhiems.SingleOrDefault(it => it.IDQuaTrinhMienNhiem == obj.IDQuaTrinhMienNhiem);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.MienNhiem.ToLower())
                    });
                }
                else
                {
                    using (TransactionScope tran = new TransactionScope())
                    {
                        edit = obj.GetMienNhiem(edit);
                        edit.LastModifiedTime = DateTime.Now;
                        edit.LastModifiedAccountId = AccountId();
                        FileDinhKemModel addFile = new FileDinhKemModel();
                        if (fileInbox != null)
                        {
                            var existFile = _context.FileDinhKems.SingleOrDefault(it => it.Id == edit.IDQuaTrinhMienNhiem);
                            if (existFile != null)
                            {
                                FunctionFile.Delete(_hostEnvironment, existFile.Url);
                                _context.FileDinhKems.Remove(existFile);
                            }
                            addFile.Id = edit.IDQuaTrinhMienNhiem;
                            addFile.IdCanBo = edit.IDCanBo;
                            addFile.IDLoaiDinhKem = "02";
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
                        _context.Entry(edit).State = EntityState.Modified;
                        _context.SaveChanges();
                        tran.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MienNhiem.ToLower())
                        });
                    }
                }
            });
        }
        #endregion Edit
        #region Delete
        [HttpDelete]
        public JsonResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                using (TransactionScope tran = new TransactionScope()) {
                    var del = _context.QuaTrinhMienNhiems.FirstOrDefault(p => p.IDQuaTrinhMienNhiem == id);
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
                            Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.MienNhiem.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.MienNhiem.ToLower())
                        });
                    }
                }
                
            });
        }
        #endregion Delete
        #region Helper
        private void CheckError(MienNhiemMTVM obj)
        {
            if (obj.NhanSu.IdCanbo == null)
            {
                ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
            }
            var exist = _context.Departments.SingleOrDefault(it => it.Id == obj.IdDepartment && it.IDCoSo == obj.IdCoSo);
            if (exist == null)
            {
                ModelState.AddModelError("IdDepartment", "Đơn vị không đúng với cơ sở");
            }
        }
        private void CreateViewBag(Guid? MaChucVu = null, Guid? IdCoSo = null, Guid? IdDepartment = null)
        {
            var MenuList1 = _context.ChucVus.Where(it => it.Actived == true).Select(it => new { MaChucVu = it.MaChucVu, TenChucVu = it.TenChucVu }).ToList();
            ViewBag.MaChucVu = new SelectList(MenuList1, "MaChucVu", "TenChucVu", MaChucVu);

            var MenuList2 = _context.CoSos.Where(it => it.Actived == true).Select(it => new { IdCoSo = it.IdCoSo, TenCoSo = it.TenCoSo }).ToList();
            ViewBag.IdCoSo = new SelectList(MenuList2, "IdCoSo", "TenCoSo", IdCoSo);

            var MenuList3 = _context.Departments.Where(it => it.Actived == true && (it.IDCoSo == IdCoSo || IdCoSo == null)).Select(it => new { IdDepartment = it.Id, TenDonVi = it.Name }).ToList();
            ViewBag.IdDepartment = new SelectList(MenuList3, "IdDepartment", "TenDonVi", IdDepartment);


        }
        #endregion Helper
    }
}
