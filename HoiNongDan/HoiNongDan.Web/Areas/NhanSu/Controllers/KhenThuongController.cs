using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys.NhanSu;
using HoiNongDan.Resources;

using Microsoft.EntityFrameworkCore;
namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class KhenThuongController : BaseController
    {
        public KhenThuongController(AppDbContext context) : base(context) { }
        [HoiNongDanAuthorization]
        #region Index
        public IActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(KhenThuongSearchVN search) {
            return ExecuteSearch(() =>
            {
                var model = _context.QuaTrinhKhenThuongs.Where(it=>it.IsCanBo == true && it.Loai =="02").AsQueryable();
                if (!String.IsNullOrEmpty(search.MaDanhHieuKhenThuong) && !String.IsNullOrWhiteSpace(search.MaDanhHieuKhenThuong))
                {
                    model = model.Where(it => it.MaDanhHieuKhenThuong == search.MaDanhHieuKhenThuong);
                }
                model = model.Include(it => it.CanBo).Include(it => it.DanhHieuKhenThuong).Include(it => it.HinhThucKhenThuong);
                if (!String.IsNullOrEmpty(search.SoCCCD) && !String.IsNullOrWhiteSpace(search.SoCCCD))
                {
                    model = model.Where(it => it.CanBo.SoCCCD == search.SoCCCD);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen) && !String.IsNullOrWhiteSpace(search.HoVaTen))
                {
                    model = model.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
                }
                var data = model.Select(it => new KhenThuongDetailVM
                {
                    IDQuaTrinhKhenThuong = it.IDQuaTrinhKhenThuong,
                    MaCanBo = it.CanBo!.MaCanBo,
                    HoVaTen = it.CanBo.HoVaTen,
                    SoQuyetDinh = it.SoQuyetDinh,
                    NgayQuyetDinh = it.NgayQuyetDinh,
                    NoiDung = it.NoiDung,
                    NguoiKy = it.NguoiKy,
                    GhiChu = it.GhiChu,
                    Nam = it.Nam,
                    TenDanhHieuKhenThuong = it.DanhHieuKhenThuong.TenDanhHieuKhenThuong,
                    TenHinhThucKhenThuong = it.HinhThucKhenThuong.TenHinhThucKhenThuong,
                }).ToList();
                return PartialView(data);
            });
        }
        #endregion Index
        #region Cretae
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create()
        {
            KhenThuongVM khenThuong = new KhenThuongVM();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu.CanBo = true;
            khenThuong.NhanSu = nhanSu;
            CreateViewBag();
            return View(khenThuong);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(KhenThuongVMMT obj) {
            CheckError(obj);
            return ExecuteContainer(() => {
                QuaTrinhKhenThuong add = new QuaTrinhKhenThuong();
                add = obj.GetKhenThuong(add);
                add.IDQuaTrinhKhenThuong = Guid.NewGuid();
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                add.Loai = "02";
                add.IsCanBo = true;
                _context.Attach(add).State = EntityState.Modified;
                _context.QuaTrinhKhenThuongs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.KhenThuong.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id)
        {
            var item = _context.QuaTrinhKhenThuongs.SingleOrDefault(it => it.IDQuaTrinhKhenThuong == id && it.IsCanBo == true);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            KhenThuongVM obj = new KhenThuongVM();
           
            var canBo = _context.CanBos.Include(it => it.CoSo).Include(it => it.Department)
                        .Include(it => it.PhanHe).Include(it => it.TinhTrang).Where(it => it.IDCanBo == item.IDCanBo && it.IsCanBo == true).SingleOrDefault();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu = nhanSu.GeThongTin(canBo!);
            nhanSu.CanBo = true;
            nhanSu.IdCanbo = canBo.IDCanBo;
            nhanSu.HoVaTen = canBo.HoVaTen;
            nhanSu.MaCanBo = canBo.MaCanBo;
            nhanSu.TenTinhTrang = canBo.TinhTrang!.TenTinhTrang;

            nhanSu.TenDonVi = canBo.Department!.Name;
            nhanSu.Edit = false;
            obj.IDQuaTrinhKhenThuong = item.IDQuaTrinhKhenThuong;
            obj.MaHinhThucKhenThuong = item.MaHinhThucKhenThuong!;
            obj.MaDanhHieuKhenThuong = item.MaDanhHieuKhenThuong;
            obj.SoQuyetDinh = item.SoQuyetDinh;
            obj.NgayQuyetDinh = item.NgayQuyetDinh;
            obj.NguoiKy = item.NguoiKy;
            obj.NoiDung = item.NoiDung;
            obj.Nam = item.Nam;
            obj.GhiChu = item.GhiChu;
            obj.NhanSu = nhanSu;
            CreateViewBag(item.MaHinhThucKhenThuong,item.MaDanhHieuKhenThuong);
            return View(obj);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(KhenThuongVMMT obj) {
            CheckError(obj);
            return ExecuteContainer(() => {
                var edit = _context.QuaTrinhKhenThuongs.SingleOrDefault(it => it.IDQuaTrinhKhenThuong == obj.IDQuaTrinhKhenThuong);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.KhenThuong.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetKhenThuong(edit);
                    edit.LastModifiedTime = DateTime.Now;
                    edit.LastModifiedAccountId = AccountId();
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.KhenThuong.ToLower())
                    });
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
                var del = _context.QuaTrinhKhenThuongs.FirstOrDefault(p => p.IDQuaTrinhKhenThuong == id);


                if (del != null)
                {
                    //_context.Entry(accountInRoleModels).State = EntityState.Deleted;
                    //_context.Entry(account).State = EntityState.Deleted;
                    _context.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.KhenThuong.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.KhenThuong.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CheckError(KhenThuongVMMT obj) {
            if (obj.NhanSu.IdCanbo == null)
            {
                ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
            }
        }
        private void CreateViewBag(String? MaHinhThucKhenThuong = null,String? MaDanhHieuKhenThuong = null) {
            var hinhThucKhenThuong = _context.HinhThucKhenThuongs.Select(it => new { MaHinhThucKhenThuong = it.MaHinhThucKhenThuong, TenHinhThucKhenThuong = it.TenHinhThucKhenThuong }).ToList();
            ViewBag.MaHinhThucKhenThuong = new SelectList(hinhThucKhenThuong, "MaHinhThucKhenThuong", "TenHinhThucKhenThuong", MaHinhThucKhenThuong);

            var MenuList = _context.DanhHieuKhenThuongs.Where(it=>it.IsCanBo==true).Select(it => new { MaDanhHieuKhenThuong = it.MaDanhHieuKhenThuong, TenDanhHieuKhenThuong = it.TenDanhHieuKhenThuong }).ToList();
            ViewBag.MaDanhHieuKhenThuong = new SelectList(MenuList, "MaDanhHieuKhenThuong", "TenDanhHieuKhenThuong", MaDanhHieuKhenThuong);
        }
        #endregion Helper
    }
}
