using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class DashboardController : BaseController
    {
        public DashboardController(AppDbContext context) : base(context) { }
        public IActionResult Index(string? maQuanHuyen= null)
        {
            List<Guid> phamVis = _context.PhamVis.Where(it => it.AccountId == AccountId()).Distinct().Select(it => it.MaDiabanHoatDong ).ToList();
            var data = _context.CanBos.AsQueryable().Include(it=>it.DiaBanHoatDong).Select(it => new { it.IDCanBo, it.HoVaTen, it.isRoiHoi, it.IsHoiVien, it.IsCanBo,it.Actived,it.MaTinhTrang,it.HoiVienDuyet,it.DiaBanHoatDong!.MaQuanHuyen,it.MaDiaBanHoatDong });
            ViewBag.CanBo = data.Where(it => it.IsCanBo == true && it.Actived == true && it.MaTinhTrang == "01").Count().ToString("N0");

            data = data.Where(it => it.HoiVienDuyet == true && it.IsHoiVien == true
                                    && it.Actived == true && phamVis.Contains(it.MaDiaBanHoatDong!.Value));
            Guid? idDiaBan = null;
            try
            {
                idDiaBan = Guid.Parse(maQuanHuyen);
            }
            catch
            {
            }

            if (idDiaBan != null)
            {

                data = data.Where(it => it.MaDiaBanHoatDong == idDiaBan);
            }
            else if (idDiaBan == null  && !String.IsNullOrWhiteSpace(maQuanHuyen)) {
                data = data.Where(it => it.MaQuanHuyen == maQuanHuyen);
            }
            ViewBag.HoiVien = data.Where(
                  lg => lg.isRoiHoi != true
                  ).Count().ToString("N0");
            ViewBag.Tong = data.Where(it => it.HoiVienDuyet == true && it.IsHoiVien == true
                                && it.Actived == true).Count().ToString("N0");
            ViewBag.Giam = data.Where(lg => lg.isRoiHoi == true).Count().ToString("N0");
            return View();
        }
        public JsonResult TongSoHoiVien(string? maQuanHuyen = null) {
            try
            {
                List<Guid> phamVis = _context.PhamVis.Where(it => it.AccountId == AccountId()).Distinct().Select(it => it.MaDiabanHoatDong).ToList();
                DateTime dateNow = DateTime.Now;
                var model = _context.CanBos.Where(it => it.IsHoiVien == true && phamVis.Contains(it.MaDiaBanHoatDong!.Value))
                    .Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen).ThenInclude(it => it.PhuongXas).AsQueryable();
                Guid? idDiaBan = null;
                try
                {
                    idDiaBan = Guid.Parse(maQuanHuyen!);
                }
                catch
                {
                }

                if (idDiaBan != null)
                {

                    model = model.Where(it => it.MaDiaBanHoatDong == idDiaBan.Value);
                }
                else if (idDiaBan == null && !String.IsNullOrWhiteSpace(maQuanHuyen))
                {
                    model = model.Where(it => it.DiaBanHoatDong.MaQuanHuyen == maQuanHuyen);
                }
                var Tam = model.Select(it => new {
                    it.IDCanBo,
                    it.HoVaTen,
                    it.DiaBanHoatDong!.TenDiaBanHoatDong,
                    it.DiaBanHoatDong.Id,
                    it.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                    it.DiaBanHoatDong.MaQuanHuyen,
                    it.DiaBanHoatDong.PhuongXa.TenPhuongXa,
                    it.DiaBanHoatDong.MaPhuongXa,
                    NgayVaoHoi = it.NgayVaoHoi,
                    it.NgayRoiHoi,
                    it.HoiVienDuyet,
                    it.CreatedTime,
                    it.NgayDuyet,
                    isRoiHoi = it.isRoiHoi != null ? it.isRoiHoi.Value : false,
                    Actived = it.Actived! != null ? it.Actived : false,
                    TuChoi = it.TuChoi != null ? it.TuChoi.Value : false
                }).ToList();
                if (String.IsNullOrWhiteSpace(maQuanHuyen))
                {
                    List<Guid> pass = new List<Guid>();
                    pass.Add(Guid.Parse("662ac072-fece-41e2-9a5e-e47c362d10cb"));
                    pass.Add(Guid.Parse("bf7024f4-6bef-442a-9d6b-ce4538b1a084"));
                    pass.Add(Guid.Parse("40a7400d-1981-45e8-b4a6-412af186dc5d"));

                    var data = Tam.Where(it=>!pass.Contains(it.Id)).GroupBy(it => new { it.MaQuanHuyen, it.TenQuanHuyen }).Select(p => new
                    {
                        Ma = p.Key.MaQuanHuyen,
                        Ten = p.Key.TenQuanHuyen.ToUpper(),
                        Giam = p.Where(lg => lg.isRoiHoi == true && lg.NgayRoiHoi != null && lg.NgayRoiHoi!.Value.Month == dateNow.Month && lg.NgayRoiHoi.Value.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count(),
                        ThemMoi = p.Where(lg => lg.NgayVaoHoi != null && lg.NgayVaoHoi.Value.Month == dateNow.Month && lg.NgayVaoHoi.Value.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count(),
                        SL = p.Where(lg => lg.isRoiHoi != true && lg.HoiVienDuyet == true && lg.Actived == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count(),
                        ChoDuyet = p.Where(lg => lg.HoiVienDuyet == false && lg.TuChoi == false && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count()
                    }).ToList();
                    var data1 = Tam.Where(it => pass.Contains(it.Id)).GroupBy(it => new { it.MaPhuongXa, it.TenPhuongXa,it.TenQuanHuyen }).Select(p => new
                    {
                        Ma = p.Key.MaPhuongXa,
                        Ten = p.Key.TenPhuongXa.ToUpper() + " "+ p.Key.TenQuanHuyen,
                        Giam = p.Where(lg => lg.isRoiHoi == true && lg.NgayRoiHoi != null && lg.NgayRoiHoi!.Value.Month == dateNow.Month && lg.NgayRoiHoi.Value.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.MaPhuongXa == p.Key.MaPhuongXa).Count(),
                        ThemMoi = p.Where(lg => lg.NgayVaoHoi != null &&  lg.NgayVaoHoi.Value.Month == dateNow.Month && lg.NgayVaoHoi.Value.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.MaPhuongXa == p.Key.MaPhuongXa).Count(),
                        SL = p.Where(lg => lg.isRoiHoi != true && lg.HoiVienDuyet == true && lg.Actived == true && lg.MaPhuongXa == p.Key.MaPhuongXa).Count(),
                        ChoDuyet = p.Where(lg => lg.HoiVienDuyet == false && lg.TuChoi == false && lg.MaPhuongXa == p.Key.MaPhuongXa).Count()
                    }).ToList();
                    data.AddRange(data1);
                    return Json(data.OrderByDescending(it => it.SL).ToList());
                }
                else
                {
                    var data = Tam.GroupBy(it => new { it.MaPhuongXa, it.TenPhuongXa }).Select(p => new
                    {
                        Ma = p.Key.MaPhuongXa,
                        Ten = p.Key.TenPhuongXa.ToUpper(),
                        Giam = p.Where(lg => lg.isRoiHoi == true && lg.NgayRoiHoi != null && lg.NgayRoiHoi!.Value.Month == dateNow.Month && lg.NgayRoiHoi.Value.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.MaPhuongXa == p.Key.MaPhuongXa).Count(),
                        ThemMoi = p.Where(lg => lg.NgayVaoHoi != null && lg.NgayVaoHoi.Value.Month == dateNow.Month && lg.NgayVaoHoi.Value.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.MaPhuongXa == p.Key.MaPhuongXa).Count(),
                        SL = p.Where(lg => lg.isRoiHoi != true && lg.HoiVienDuyet == true && lg.Actived == true && lg.MaPhuongXa == p.Key.MaPhuongXa).Count(),
                        ChoDuyet = p.Where(lg => lg.HoiVienDuyet == false && lg.TuChoi == false && lg.MaPhuongXa == p.Key.MaPhuongXa).Count()
                    }).ToList();
                    return Json(data.OrderByDescending(it => it.SL).ToList());
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
                throw;
            }
            
            
        }
    }
}
