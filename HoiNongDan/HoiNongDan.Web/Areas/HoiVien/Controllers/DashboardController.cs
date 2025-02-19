using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2010.Excel;
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
using System.Reflection;
using System.Runtime.InteropServices;
namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class DashboardController : BaseController
    {
        public DashboardController(AppDbContext context) : base(context) { }
        public IActionResult Index(string? maQuanHuyen= null)
        {
            var phamVis = _context.PhamVis
                        .Join(_context.DiaBanHoatDongs,
                        db => db.MaDiabanHoatDong,
                        pv => pv.Id,
                        (db, pv) => new { pv, db }).Where(it => it.db.AccountId == AccountId()).Select(it => new { it.pv.MaQuanHuyen, it.pv.Id }).ToList();
            if (!String.IsNullOrWhiteSpace(maQuanHuyen)) {
                phamVis = phamVis.Where(it => it.MaQuanHuyen == maQuanHuyen).ToList();
            }

            var data = _context.CanBos.Where(it=>it.IsCanBo == true).AsQueryable().Include(it=>it.DiaBanHoatDong).Include(it=>it.ChiHoi).Include(it=>it.ToHoi)
                .Select(it => new { it.IDCanBo, it.HoVaTen, it.isRoiHoi, it.IsHoiVien,it.HoiVienDanCu,it.HoiVienNganhNghe,
                    it.IsCanBo,it.Actived,it.MaTinhTrang,it.HoiVienDuyet,it.DiaBanHoatDong!.MaQuanHuyen,
                    MaQuanHuyenCB =it.MaQuanHuyen ,
                    it.MaDiaBanHoatDong,it.MaChiHoi,it.MaToHoi,
                    LoaiChiHoi = it.ChiHoi!.Loai,LoaiToHoi = it.ToHoi!.Loai,it.NgayVaoHoi,it.NgayRoiHoi});

            var nhomquyens = _context.AccountInRoleModels
                  .Join(_context.RolesModels,
                      role => role.RolesId,
                      ac => ac.RolesId,
                      (ac, role) => new { ac, role })
                  .Where(it => it.ac.AccountId == AccountId()).Select(it => it.role.RolesCode.ToLower()).ToList();
            if (nhomquyens.Contains("admin") && String.IsNullOrWhiteSpace(maQuanHuyen))
            {
                ViewBag.CanBo = data.Where(it => it.IsCanBo == true && (it.MaTinhTrang == "01" || it.MaTinhTrang == null)).Count().ToString("N0");
            }
            else if (nhomquyens.Contains("quanly_quanhuyen") || ((nhomquyens.Contains("admin") && !String.IsNullOrWhiteSpace(maQuanHuyen))))
            {
                ViewBag.CanBo = data.Where(it =>  it.IsCanBo == true && (it.MaTinhTrang == "01" || it.MaTinhTrang == null) && phamVis.Select(it => it.MaQuanHuyen).Distinct().ToList().Contains(it.MaQuanHuyenCB!)).Count().ToString("N0");
            }
            else 
            {
                ViewBag.CanBo = data.Where(it => it.IsCanBo == true && (it.MaTinhTrang == "01" || it.MaTinhTrang == null) && phamVis.Select(it => it.Id).ToList().Contains(it.MaDiaBanHoatDong!.Value)).Count().ToString("N0");
            }


            DateTime dateNow = DateTime.Now.Date;
            DateTime firstDateOfYear = new DateTime( dateNow.Year, 01, 01).AddDays(-46).Date;
            string id = "";
            maQuanHuyen = String.IsNullOrWhiteSpace(maQuanHuyen) ? "" : maQuanHuyen;
            if (!String.IsNullOrWhiteSpace(maQuanHuyen))
            {
                try
                {
                    Guid temp = Guid.Parse(maQuanHuyen);
                    id = temp.ToString();
                    maQuanHuyen = "";
                }
                catch (Exception)
                {
                }
            }
           
           
            var model = _context.DashboardHoiViens.FromSqlRaw("EXEC dbo.[DashboardHoiVien] @tungay = '" + firstDateOfYear.ToString("yyyy-MM-dd") + "',@denngay = '" + dateNow.ToString("yyyy-MM-dd") + "',@AccountId='" + AccountId().ToString() + "',@maQuanHuyen='" + maQuanHuyen+ "',@id='" + id+"'").ToList().First();
            ViewBag.Tong = model.Tong.ToString("N0");
            ViewBag.TongNN = model.TongNN.ToString("N0");
            ViewBag.TongDC = model.TongDC.ToString("N0");

            //Dang hoạt động tới hiện tại
            ViewBag.HoiVien = model.HoiVien.ToString("N0");
            ViewBag.HoiVienNN = model.HoiVienNN.ToString("N0");
            ViewBag.HoiVienDC = model.HoiVienDC.ToString("N0");

            // hội viên phát triển

            ViewBag.PhatTrien = model.PhatTrien.ToString("N0");

            ViewBag.PhatTrienNN = model.PhatTrienNN.ToString("N0");
            ViewBag.PhatTrienDC = model.PhatTrienDC.ToString("N0");


            // hội viên giảm trong nam

            ViewBag.Giam = model.Giam.ToString("N0");
            
            ViewBag.GiamNN = model.GiamNN.ToString("N0");
            ViewBag.GiamDC = model.GiamDC.ToString("N0");

            // CHi hoi

            ViewBag.chiHoi = model.ChiHoi.ToString("N0");
            ViewBag.chiHoiDC = model.ChiHoiDC.ToString("N0");
            ViewBag.chiHoiNN = model.ChiHoiNN.ToString("N0");
           

            ViewBag.toHoiDC = model.ToHoiDC.ToString("N0");
            ViewBag.toHoiNN = model.ToHoiNN.ToString("N0");
            ViewBag.toHoi = model.ToHoi.ToString("N0"); ;
            return View();
        }
        public JsonResult TongSoHoiVien(string? maQuanHuyen = null) {
            try
            {
                List<Guid> phamVis = _context.PhamVis.Where(it => it.AccountId == AccountId()).Distinct().Select(it => it.MaDiabanHoatDong).ToList();
                DateTime dateNow = DateTime.Now;
                DateTime firstDateOfMonth = new DateTime(dateNow.Year, dateNow.Month, 01);
                var model = _context.CanBos.Where(it => it.IsHoiVien == true && phamVis.Contains(it.MaDiaBanHoatDong!.Value) && ((it.NgayVaoHoi>= firstDateOfMonth && it.NgayVaoHoi <= dateNow) || (it.NgayRoiHoi >= firstDateOfMonth && it.NgayRoiHoi <= dateNow)))
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
                    Actived = it.Actived != null ? it.Actived : false,
                    TuChoi = it.TuChoi != null ? it.TuChoi.Value : false
                }).ToList();
                var nhomquyens = _context.AccountInRoleModels
                .Join(_context.RolesModels,
                    role => role.RolesId,
                    ac => ac.RolesId,
                    (ac, role) => new { ac, role })
                .Where(it => it.ac.AccountId == AccountId()).Select(it => it.role.RolesCode.ToLower()).ToList();
                if (String.IsNullOrWhiteSpace(maQuanHuyen) && nhomquyens.Contains("admin"))
                {
                    List<Guid> pass = new List<Guid>();
                    pass.Add(Guid.Parse("662ac072-fece-41e2-9a5e-e47c362d10cb"));
                    pass.Add(Guid.Parse("bf7024f4-6bef-442a-9d6b-ce4538b1a084"));
                    pass.Add(Guid.Parse("40a7400d-1981-45e8-b4a6-412af186dc5d"));

                    var data = Tam.Where(it=>!pass.Contains(it.Id)).GroupBy(it => new { it.MaQuanHuyen, it.TenQuanHuyen }).Select(p => new
                    {
                        Ma = p.Key.MaQuanHuyen,
                        Ten = p.Key.TenQuanHuyen.ToUpper(),
                        Giam = p.Where(lg => lg.isRoiHoi == true  && lg.HoiVienDuyet == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count(),
                        ThemMoi = p.Where(lg => lg.NgayVaoHoi != null && lg.HoiVienDuyet == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count(),
                        SL = p.Where(lg => lg.isRoiHoi != true && lg.HoiVienDuyet == true && lg.Actived == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count(),
                        ChoDuyet = p.Where(lg => lg.HoiVienDuyet == false && lg.TuChoi == false && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count()
                    }).ToList();
                    var data1 = Tam.Where(it => pass.Contains(it.Id)).GroupBy(it => new { it.MaPhuongXa, it.TenPhuongXa,it.TenQuanHuyen }).Select(p => new
                    {
                        Ma = p.Key.MaPhuongXa,
                        Ten = p.Key.TenPhuongXa.ToUpper() + " "+ p.Key.TenQuanHuyen,
                        Giam = p.Where(lg => lg.isRoiHoi == true && lg.HoiVienDuyet == true && lg.MaPhuongXa == p.Key.MaPhuongXa).Count(),
                        ThemMoi = p.Where(lg => lg.NgayVaoHoi != null && lg.HoiVienDuyet == true && lg.MaPhuongXa == p.Key.MaPhuongXa).Count(),
                        SL = p.Where(lg => lg.isRoiHoi != true && lg.HoiVienDuyet == true && lg.Actived == true && lg.MaPhuongXa == p.Key.MaPhuongXa).Count(),
                        ChoDuyet = p.Where(lg => lg.HoiVienDuyet == false && lg.TuChoi == false && lg.MaPhuongXa == p.Key.MaPhuongXa).Count()
                    }).ToList();
                    data.AddRange(data1);
                    data = data.Where(it => it.ThemMoi > 0 || it.Giam > 0 || it.ChoDuyet > 0).ToList();
                    return Json(data.OrderByDescending(it => it.SL).ToList());
                }
                else
                {
                    var data = Tam.GroupBy(it => new { it.MaPhuongXa, it.TenPhuongXa }).Select(p => new
                    {
                        Ma = p.Key.MaPhuongXa,
                        Ten = p.Key.TenPhuongXa.ToUpper(),
                        Giam = p.Where(lg => lg.isRoiHoi == true && lg.HoiVienDuyet == true && lg.MaPhuongXa == p.Key.MaPhuongXa).Count(),
                        ThemMoi = p.Where(lg => lg.NgayVaoHoi != null && lg.HoiVienDuyet == true && lg.MaPhuongXa == p.Key.MaPhuongXa).Count(),
                        SL = p.Where(lg => lg.isRoiHoi != true && lg.HoiVienDuyet == true && lg.Actived == true && lg.MaPhuongXa == p.Key.MaPhuongXa).Count(),
                        ChoDuyet = p.Where(lg => lg.HoiVienDuyet == false && lg.TuChoi == false && lg.MaPhuongXa == p.Key.MaPhuongXa).Count()
                    }).ToList();
                    data = data.Where(it => it.ThemMoi > 0 || it.Giam > 0 || it.ChoDuyet > 0).ToList();
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
