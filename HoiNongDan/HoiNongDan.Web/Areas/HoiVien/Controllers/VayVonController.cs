using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class VayVonController : BaseController
    {
        public VayVonController(AppDbContext context) :base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            VayVonSearchVM model = new VayVonSearchVM();
            CreateViewBagSearch();
            return View(model);
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(VayVonSearchVM search)
        {
            return ExecuteSearch(() =>
            {
                var data = (from hvht in _context.VayVons
                            join hv in _context.CanBos on hvht.IDHoiVien equals hv.IDCanBo
                            join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                            where pv.AccountId == AccountId()
                            select hvht).Distinct().Include(it => it.HoiVien).ThenInclude(it => it.DiaBanHoatDong).ThenInclude(it=>it.QuanHuyen).ThenInclude(it=>it.PhuongXas).AsQueryable();

                if (!String.IsNullOrWhiteSpace(search.MaHV))
                {
                    data = data.Where(it => it.HoiVien.MaCanBo == search.MaHV);
                }
                if (!String.IsNullOrEmpty(search.TenHV) && !String.IsNullOrWhiteSpace(search.TenHV))
                {
                    data = data.Where(it => it.HoiVien.HoVaTen.Contains(search.TenHV));
                }
                if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen))
                {
                    data = data.Where(it => it.HoiVien.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
                }
                if (search.MaNguonVon != null)
                {
                    data = data.Where(it => it.MaNguonVon == search.MaNguonVon);
                }
                if (search.Actived != null)
                {
                    data = data.Where(it => it.Actived == search.Actived);
                }
                if (search.MaDiaBanHoiVien != null)
                {
                    data = data.Where(it => it.HoiVien.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
                }
                if (search.NamVayVon != null)
                {
                    data = data.Where(it => it.TuNgay!.Value.Year == search.NamVayVon);
                }
                var model = data.Select(it => new VayVonDetailVM
                {
                    ID = it.IDVayVon,
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
                    TienVay = it.SoTienVay != null ? it.SoTienVay.Value.ToString("N0") : "",
                    PhuongXa = it.HoiVien.DiaBanHoatDong!.PhuongXa.TenPhuongXa,
                    QuanHuyen = it.HoiVien.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen,
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
            VayVonVM item = new VayVonVM();
            HoiVienInfo nhanSu = new HoiVienInfo();

            item.HoiVien = nhanSu;
            CreateViewBag();
            return View(item);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VayVonVM obj)
        {
            CheckError(obj);
            return ExecuteContainer(() =>
            {
                var add = new VayVon { 
                    IDVayVon = Guid.NewGuid(),
                    IDHoiVien = obj.HoiVien.IdCanbo!.Value,
                    SoTienVay = obj.SoTienVay != null ? long.Parse(obj.SoTienVay.Replace(",", "")) : null,
                    ThoiHangChoVay = obj.ThoiHangChoVay,
                    LaiSuatVay = obj.LaiSuatVay,
                    TuNgay = obj.TuNgay,
                    DenNgay = obj.DenNgay,
                    NoiDung = obj.NoiDung,
                    GhiChu =  obj.GhiChu,
                    Actived = true,
                    TraXong = false,
                    MaNguonVon = obj.MaNguonVon,
                    CreatedAccountId = AccountId(),
                    CreatedTime = DateTime.Now
                };
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.VayVons.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.VayVon.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id)
        {
            var item = _context.VayVons.SingleOrDefault(it => it.IDVayVon == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            VayVonVM obj = new VayVonVM();


            obj.ID = item.IDVayVon;
            obj.SoTienVay = item.SoTienVay != null ? item.SoTienVay!.Value.ToString("N0") : null;
            obj.LaiSuatVay = item.LaiSuatVay;
            obj.ThoiHangChoVay = item.ThoiHangChoVay;
            obj.TuNgay = item.TuNgay;
            obj.DenNgay = item.DenNgay;
            obj.NgayTraNoCuoiCung = item.NgayTraNoCuoiCung;
            obj.NoiDung = item.NoiDung;
            obj.GhiChu = item.GhiChu;
            obj.HoiVien = Function.GetThongTinHoiVien(item.IDHoiVien,_context);
            obj.TraXong = item.TraXong! == null ? false : item.TraXong;

            return View(obj);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(VayVonVM obj)
        {
            CheckError(obj);
            return ExecuteContainer(() =>
            {
                var edit = _context.VayVons.SingleOrDefault(it => it.IDVayVon == obj.ID);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.VayVon.ToLower())
                    });
                }
                else
                {
                    edit.NoiDung = obj.NoiDung;
                    edit.GhiChu = obj.GhiChu;
                    edit.TraXong = obj.TraXong;
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.VayVon.ToLower())
                    });
                }
            });
        }
        #endregion Edit
        #region Del
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.VayVons.SingleOrDefault(p => p.IDVayVon == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.VayVon.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.VayVon.ToLower())
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
                var data = _context.VayVons.Where(it => it.TraXong != true).Include(it => it.NguonVon).AsQueryable();
                var model = data.Where(it => it.NgayTraNoCuoiCung!.Value.AddMonths(search.SoThang) < search.Ngay).Select(it => new VayVonDetailVM
                {
                    ID = it.IDVayVon,
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
        private void CreateViewBag(Guid? MaNguonVon = null, Guid? MaHinhThucHoTro = null)
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaNguonVon = fnViewBag.NguonVon(value: MaNguonVon);
        }
        [NonAction]
        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaHinhThucHoTro = fnViewBag.HinhThucHoTro();

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
            ViewBag.MaNguonVon = fnViewBag.NguonVon();
        }
        private void CheckError(VayVonVM obj)
        {
            if (obj.HoiVien.IdCanbo == null)
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
