using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;


namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class DashboardController : BaseController
    {
        public DashboardController(AppDbContext context) : base(context) { }
        public IActionResult Index(string? maQuanHuyen= null)
        {
            var data = _context.CanBos.Where(it => it.Actived == true).Include(it=>it.DiaBanHoatDong).Select(it => new { it.IDCanBo, it.HoVaTen, it.isRoiHoi, it.IsHoiVien, it.IsCanBo,it.Actived,it.MaTinhTrang,it.HoiVienDuyet,it.DiaBanHoatDong!.MaQuanHuyen });
            ViewBag.CanBo = data.Where(it => it.IsCanBo == true && it.Actived == true && it.MaTinhTrang == "01").Count().ToString("N0");
            if (String.IsNullOrWhiteSpace(maQuanHuyen))
            {
                ViewBag.HoiVien = data.Where(lg => lg.isRoiHoi != true && lg.HoiVienDuyet == true && lg.Actived == true).Count().ToString("N0");
            }
            else
            {
                ViewBag.HoiVien = data.Where(lg => lg.MaQuanHuyen == maQuanHuyen && lg.isRoiHoi != true && lg.HoiVienDuyet == true && lg.Actived == true).Count().ToString("N0");
            }
            return View();
        }
        public JsonResult TongSoHoiVien(string? maQuanHuyen = null) {
            try
            {
                DateTime dateNow = DateTime.Now;
                var model = _context.CanBos.Where(it => it.IsHoiVien == true).Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen).ThenInclude(it=>it.PhuongXas).Select(it => new {
                    it.IDCanBo,
                    it.HoVaTen,
                    it.DiaBanHoatDong!.TenDiaBanHoatDong,
                    it.DiaBanHoatDong.Id,
                    it.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                    it.DiaBanHoatDong.MaQuanHuyen,
                    it.DiaBanHoatDong.PhuongXa.TenPhuongXa,
                    NgayVaoHoi = Function.ConvertStringToDate(it.NgayVaoHoi!),
                    it.NgayRoiHoi,
                    it.HoiVienDuyet,
                    it.CreatedTime,
                    it.NgayDuyet,
                    it.isRoiHoi,
                    it.Actived,
                }).ToList();
                if (String.IsNullOrWhiteSpace(maQuanHuyen))
                {

                    var data = model.GroupBy(it => new { it.MaQuanHuyen, it.TenQuanHuyen }).Select(p => new
                    {
                        Ma = p.Key.MaQuanHuyen,
                        Ten = p.Key.TenQuanHuyen.ToUpper(),
                        Giam = p.Where(lg => lg.isRoiHoi == true && lg.NgayRoiHoi!.Value.Month == dateNow.Month && lg.NgayRoiHoi.Value.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count(),
                        ThemMoi = p.Where(lg => lg.NgayVaoHoi.Month == dateNow.Month && lg.NgayVaoHoi.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count(),
                        SL = p.Where(lg => lg.isRoiHoi != true && lg.HoiVienDuyet == true && lg.Actived == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count(),
                        ChoDuyet = p.Where(lg => lg.HoiVienDuyet == false && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count()
                    }).ToList();
                    return Json(data.OrderBy(it => it.Ten).ToList());
                }
                else
                {
                    var data = model.Where(it => it.MaQuanHuyen == maQuanHuyen).GroupBy(it => new { it.Id, it.TenPhuongXa }).Select(p => new
                    {
                        Ma = p.Key.Id,
                        Ten = p.Key.TenPhuongXa.ToUpper(),
                        Giam = p.Where(lg => lg.isRoiHoi == true && lg.NgayRoiHoi!.Value.Month == dateNow.Month && lg.NgayRoiHoi.Value.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.Id == p.Key.Id).Count(),
                        ThemMoi = p.Where(lg => lg.NgayVaoHoi.Month == dateNow.Month && lg.NgayVaoHoi.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.Id == p.Key.Id).Count(),
                        SL = p.Where(lg => lg.isRoiHoi != true && lg.HoiVienDuyet == true && lg.Actived == true && lg.Id == p.Key.Id).Count(),
                        ChoDuyet = p.Where(lg => lg.HoiVienDuyet == false && lg.Id == p.Key.Id).Count()
                    }).ToList();
                    return Json(data.OrderBy(it => it.Ten).ToList());
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
                throw;
            }
            
            
        }
    }
}
