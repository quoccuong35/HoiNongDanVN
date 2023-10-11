using HoiNongDan.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HoiNongDan.Extensions;
using HoiNongDan.DataAccess;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HoTroController : BaseController
    {
        public HoTroController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            HVHoTroSearchVM model = new HVHoTroSearchVM();
            CreateViewBag();
            return View(model);
        }
        public IActionResult _Search(HVHoTroSearchVM search)
        {
            return ExecuteSearch(() =>
            {
                var data = _context.HoiVienHoTros.Include(it => it.HoiVien).Include(it=>it.NguonVon).Include(it=>it.HinhThucHoTro).AsQueryable();
                if (!String.IsNullOrEmpty(search.MaHV) && !String.IsNullOrWhiteSpace(search.MaHV))
                {
                    data = data.Where(it => it.HoiVien.MaCanBo == search.MaHV);
                }
                if (!String.IsNullOrEmpty(search.TenHV) && !String.IsNullOrWhiteSpace(search.TenHV))
                {
                    data = data.Where(it => it.HoiVien.HoVaTen.Contains(search.TenHV));
                }
                if (search.Actived != null)
                {
                    data = data.Where(it => it.Actived == search.Actived);
                }
                if (search.NamVayVon != null)
                {
                    data = data.Where(it => it.TuNgay!.Value.Year == search.NamVayVon);
                }
                var model = data.Select(it => new HoiVienHoTroDetailVM
                {
                    ID = it.ID,
                    MaHV = it.HoiVien.MaCanBo!,
                    TenHV = it.HoiVien.HoVaTen,
                    SoTienVay = it.SoTienVay,
                    ThoiHangChoVay = it.ThoiHangChoVay,
                    LaiSuatVay = it.LaiSuatVay,
                    TuNgay = it.TuNgay,
                    DenNgay = it.DenNgay,
                    NgayTraNoCuoiCung = it.NgayTraNoCuoiCung,
                    NoiDung = it.NoiDung,
                    TraXong = it.TraXong,
                    HinhThucHoTro = it.HinhThucHoTro.TenHinhThuc,
                    NguonVon = it.NguonVon.TenNguonVon,
                    TienVay = it.SoTienVay !=null? it.SoTienVay.Value.ToString("N0"):""
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
            HoiVienHoTroVM item = new HoiVienHoTroVM();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();

            nhanSu.CanBo = false;
            item.NhanSu = nhanSu;
            CreateViewBag();
            return View(item);
        }
        public JsonResult Create(HoiVienHoTroMTVM obj)
        {
            CheckError(obj);
            return ExecuteContainer(() =>
            {
                var add = new HoiVienHoTro();
                add = obj.GetHoTro(add);
                add.ID = Guid.NewGuid();
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                add.TraXong = false;
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.HoiVienHoTros.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.HoiVienVayVon.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id)
        {
            var item = _context.HoiVienHoTros.SingleOrDefault(it => it.ID == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            HoiVienHoTroVM obj = new HoiVienHoTroVM();


            var canBo = _context.CanBos.Include(it => it.CoSo).Include(it => it.DiaBanHoatDong)
                        .Include(it => it.PhanHe).Include(it => it.TinhTrang).Where(it => it.IDCanBo == item.IDHoiVien).SingleOrDefault();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu = nhanSu.GetHoiVien(canBo);
            nhanSu.Edit = false;

            obj.ID = item.ID;
            obj.SoTienVay = item.SoTienVay != null? item.SoTienVay!.Value.ToString("N0"):null;
            obj.LaiSuatVay = item.LaiSuatVay;
            obj.ThoiHangChoVay = item.ThoiHangChoVay;
            obj.TuNgay = item.TuNgay;
            obj.DenNgay = item.DenNgay;
            obj.NgayTraNoCuoiCung = item.NgayTraNoCuoiCung;
            obj.NoiDung = item.NoiDung;
            obj.GhiChu = item.GhiChu;
            obj.NhanSu = nhanSu;
            obj.TraXong = item.TraXong! == null ? false : item.TraXong;
            obj.MaNguonVon = item.MaNguonVon;
            obj.MaHinhThucHoTro = item.MaHinhThucHoTro;
            CreateViewBag(item.MaNguonVon,item.MaHinhThucHoTro);
            return View(obj);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        public JsonResult Edit(HoiVienHoTroMTVM obj)
        {
            CheckError(obj);
            return ExecuteContainer(() =>
            {
                var edit = _context.HoiVienHoTros.SingleOrDefault(it => it.ID == obj.ID);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.HoiVienVayVon.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetHoTro(edit);
                    edit.TraXong = obj.TraXong;
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.HoiVienVayVon.ToLower())
                    });
                }
            });
        }
        #endregion Edit
        #region Del
        [HttpDelete]
        [HoiNongDanAuthorization]
        //[ValidateAntiForgeryToken]
        public JsonResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.HoiVienHoTros.FirstOrDefault(p => p.ID == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.HoiVienVayVon.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.HoiVienVayVon.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Report
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Report()
        {
            return View(new VayVonQuaHanSearchVM { Ngay = DateTime.Now, SoThang = 3 });
        }
        [HttpGet]
        public IActionResult _ReportData(VayVonQuaHanSearchVM search)
        {
            return ExecuteSearch(() =>
            {
                var data = _context.HoiVienHoTros.Where(it => it.TraXong != true && it.MaHinhThucHoTro == Guid.Parse("945C96EF-EA48-404D-9166-F41342EA48E6")).Include(it => it.ID).AsQueryable();
                var model = data.Where(it => it.NgayTraNoCuoiCung!.Value.AddMonths(search.SoThang) < search.Ngay).Select(it => new HoiVienHoTroDetailVM
                {
                    ID = it.ID,
                    MaHV = it.HoiVien.MaCanBo!,
                    TenHV = it.HoiVien.HoVaTen,
                    SoTienVay = it.SoTienVay,
                    ThoiHangChoVay = it.ThoiHangChoVay,
                    LaiSuatVay = it.LaiSuatVay,
                    TuNgay = it.TuNgay,
                    DenNgay = it.DenNgay,
                    NgayTraNoCuoiCung = it.NgayTraNoCuoiCung,
                    NoiDung = it.NoiDung,
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Report
        #region Helper
        private void CreateViewBag(Guid? MaNguonVon = null, Guid? MaHinhThucHoTro = null) { 
            var nguonVon = _context.NguonVons.Select(it => new { MaNguonVon =it.MaNguonVon, TenNguonVon= it.TenNguonVon}).ToList();
            ViewBag.MaNguonVon = new SelectList(nguonVon, "MaNguonVon", "TenNguonVon", MaNguonVon);

            var hinhThucHoTros = _context.HinhThucHoTros.Select(it => new { MaHinhThucHoTro = it.MaHinhThucHoTro, TenHinhThuc = it.TenHinhThuc }).ToList();
            ViewBag.MaHinhThucHoTro = new SelectList(hinhThucHoTros, "MaHinhThucHoTro", "TenHinhThuc", MaHinhThucHoTro);
        }
        private void CheckError(HoiVienHoTroMTVM obj)
        {
            if (obj.NhanSu.IdCanbo == null)
            {
                ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
            }
            if (obj.DenNgay < obj.TuNgay)
            {
                ModelState.AddModelError("DenNgay", "Từ ngày đến ngày không hợp lệ");
            }
            if (obj.ThoiHangChoVay < 0)
            {
                ModelState.AddModelError("ThoiHangChoVay", "Số tháng cho vay không hợp lệ");
            }
        }
        #endregion Helper
    }
}
