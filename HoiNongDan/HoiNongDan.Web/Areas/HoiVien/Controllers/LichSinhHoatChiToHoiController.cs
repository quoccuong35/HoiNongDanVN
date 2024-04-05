using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.ViewModels.Masterdata;
using HoiNongDan.Resources;
using HoiNongDan.Web.Areas.NhanSu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using System.Data.Entity;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class LichSinhHoatChiToHoiController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private string _tenThuMuc = "LSHChiToHoi";
        private string _loaiFileDinhKem = "80";
        public LichSinhHoatChiToHoiController(AppDbContext context, IWebHostEnvironment hostEnvironment ) :base(context) {
            _hostEnvironment = hostEnvironment;
        }
        #region Index
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBagSearch();
            LichSinhHoatChiToHoiSearchVM model = new LichSinhHoatChiToHoiSearchVM();
            return View(model);
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(LichSinhHoatChiToHoiSearchVM sr) {
            return ExecuteSearch(() => {
                var model = _context.LichSinhHoatChiToHois.Include(it => it.DiaBanHoatDong).AsQueryable();
                if (!String.IsNullOrWhiteSpace(sr.TenNoiDung)) {
                    model = model.Where(it => it.TenNoiDungSinhHoat.Contains(sr.TenNoiDung));
                }
                if (sr.TuNgay != null)
                {
                    model = model.Where(it => it.Ngay.Date >= sr.TuNgay);
                }
                if (sr.DenNgay != null)
                {
                    model = model.Where(it => it.Ngay.Date <= sr.DenNgay);
                }
                if (sr.MaDiaBanHoi != null)
                {
                    model = model.Where(it => it.IDDiaBanHoiVien  == sr.MaDiaBanHoi);
                }
                if (!String.IsNullOrWhiteSpace(sr.MaQuanHuyen)) {
                    model = model.Where(it => it.DiaBanHoatDong.MaQuanHuyen == sr.MaQuanHuyen);
                }
                if (sr.Actived != null)
                {
                    model = model.Where(it => it.Actived == sr.Actived);
                }

                var data = model.Select(it => new LichSinhHoatChiToHoiDetailVM
                {
                    ID = it.ID,
                    TenNoiDungSinhHoat = it.TenNoiDungSinhHoat,
                    //NoiDungSinhHoat = it.NoiDungSinhHoat,
                    Ngay = it.Ngay,
                    TenDiaBanHoi = _context.DiaBanHoatDongs.SingleOrDefault(p=>p.Id == it.IDDiaBanHoiVien)!.TenDiaBanHoatDong,
                    SoLuongNguoiThanGia = it.SoLuongNguoiThanGia,

                }).ToList();
                
                return PartialView(data);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create() {
            LichSinhHoatChiToHoiVM model = new LichSinhHoatChiToHoiVM();
            CreateViewBag();
            return View(model);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        public IActionResult Create(LichSinhHoatChiToHoiVM model, IFormFile?[] fileDinhKems) {
            CheakError(model);
            return ExecuteContainer(() => {
                var lichSinhHoat = new LichSinhHoatChiToHoi();
                lichSinhHoat.ID = Guid.NewGuid();
                lichSinhHoat.NoiDungSinhHoat = model.NoiDungSinhHoat;
                lichSinhHoat.TenNoiDungSinhHoat = model.TenNoiDungSinhHoat;
                lichSinhHoat.IDDiaBanHoiVien = model.MaDiaBanHoiVien;
                lichSinhHoat.Ngay = model.Ngay.Value;
                lichSinhHoat.IDMaChiToHoi = model.IDMaChiToHoi;
                lichSinhHoat.SoLuongNguoiThanGia = model.SoLuongNguoiThanGia;
                lichSinhHoat.Actived = true;
                lichSinhHoat.CreatedAccountId = AccountId();
                lichSinhHoat.CreatedTime = DateTime.Now;
                if (fileDinhKems.Count() > 0)
                {
                    var dataFile = AdFiles(fileDinhKems, lichSinhHoat);
                    _context.FileDinhKems.AddRange(dataFile);
                }
               
                _context.LichSinhHoatChiToHois.Add(lichSinhHoat);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.LichSinhHoatChiToHoi.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit 
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
            var data = _context.LichSinhHoatChiToHois.SingleOrDefault(it => it.ID == id);
            if (data == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            if(data.CreatedAccountId != AccountId()) {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            LichSinhHoatChiToHoiVM model = new LichSinhHoatChiToHoiVM();
            model.ID = id;
            model.Ngay = data.Ngay;
            model.TenNoiDungSinhHoat = data.TenNoiDungSinhHoat;
            model.NoiDungSinhHoat = data.NoiDungSinhHoat;
            model.IDMaChiToHoi = data.IDMaChiToHoi;
            model.SoLuongNguoiThanGia = data.SoLuongNguoiThanGia;
            model.Actived = data.Actived;
            model.MaDiaBanHoiVien = data.IDDiaBanHoiVien;
            model.FileDinhKems = _context.FileDinhKems.Where(it => it.Id == model.ID).ToList();
            CreateViewBag(model.MaDiaBanHoiVien, model.IDMaChiToHoi);
            return View(model); 
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        public IActionResult Edit(LichSinhHoatChiToHoiVM model, IFormFile?[] fileDinhKems) {
            var lichSinhHoat = _context.LichSinhHoatChiToHois.SingleOrDefault(it => it.ID == model.ID);
            if (lichSinhHoat == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + model.ID.ToString());
            }
            if (lichSinhHoat.CreatedAccountId != AccountId())
            {
                 return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                    Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.LichSinhHoatChiToHoi.ToLower())
                });
            }
            CheakError(model);
            return ExecuteContainer(() => {
                lichSinhHoat.NoiDungSinhHoat = model.NoiDungSinhHoat;
                lichSinhHoat.TenNoiDungSinhHoat = model.TenNoiDungSinhHoat;
                lichSinhHoat.IDDiaBanHoiVien = model.MaDiaBanHoiVien;
                lichSinhHoat.Ngay = model.Ngay.Value;
                lichSinhHoat.SoLuongNguoiThanGia = model.SoLuongNguoiThanGia;
                lichSinhHoat.Actived = model.Actived;
                lichSinhHoat.IDMaChiToHoi = model.IDMaChiToHoi;
                lichSinhHoat.LastModifiedAccountId = AccountId();
                lichSinhHoat.LastModifiedTime = DateTime.Now;
                if (fileDinhKems.Count() > 0)
                {
                    var dataFile = AdFiles(fileDinhKems, lichSinhHoat);
                    _context.FileDinhKems.AddRange(dataFile);
                }
                _context.LichSinhHoatChiToHois.Update(lichSinhHoat);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.LichSinhHoatChiToHoi.ToLower())
                });
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
                var del = _context.LichSinhHoatChiToHois.FirstOrDefault(p => p.ID == id);
                if (del != null)
                {
                    if (del.CreatedAccountId != AccountId())
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.LichSinhHoatChiToHoi.ToLower())
                        });
                    }
                    var deleteAllFile = _context.FileDinhKems.Where(it => it.Id == del.ID);
                    if (deleteAllFile.Count() > 0)
                    {
                        foreach (var item in deleteAllFile)
                        {
                            FunctionFile.Delete(_hostEnvironment, item.Url);
                        }
                        _context.RemoveRange(deleteAllFile);
                    }

                    _context.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.HoiVien.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.HoiVien.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        [NonAction]
        private List<FileDinhKemModel> AdFiles(IFormFile?[] filesDinhKem, LichSinhHoatChiToHoi lichSinhHoat)
        {
            List<FileDinhKemModel> addFiles = new List<FileDinhKemModel>();
            foreach (var file in filesDinhKem)
            {
                FileDinhKemModel addFile = new FileDinhKemModel();
                addFile.Id = lichSinhHoat.ID;
                addFile.Key = Guid.NewGuid();
                addFile.IDLoaiDinhKem = _loaiFileDinhKem;
                FunctionFile.CopyFile(_hostEnvironment, file!, addFile,TenThuMuc:_tenThuMuc+@"\"+lichSinhHoat.ID.ToString());
                if (!String.IsNullOrEmpty(addFile.Error) && !String.IsNullOrWhiteSpace(addFile.Error))
                {
                    ModelState.AddModelError("fileInbox", addFile.Error);
                    break;
                }
                addFiles.Add(addFile);
            }
            return addFiles;
        }
        [NonAction]
        private void CheakError(LichSinhHoatChiToHoiVM model) { 
            //if(model.LichSinhHoatChiToHoi_NguoiThamGias.Count() == 0) {
            //    ModelState.AddModelError("Eroor", "Chưa nhâp thông tin thành phần tham gia");
            //}
            if (String.IsNullOrWhiteSpace(model.NoiDungSinhHoat))
            {
                ModelState.AddModelError("NoiDungSinhHoat", "Chưa nhập nội dung sinh hoạt");
            }
        }
        [NonAction]
        private void CreateViewBag(Guid? MaDiaBanHoi = null, Guid? IDMaChiToHoi = null) {

            FnViewBag fnViewBag = new FnViewBag(_context);
        
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId(),value: MaDiaBanHoi);

            ViewBag.IDMaChiToHoi = fnViewBag.ChiToHoi(value: IDMaChiToHoi);
        }
        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
         
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.IDMaChiToHoi = fnViewBag.ChiToHoi();

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }
        #endregion Helper
    }
}
