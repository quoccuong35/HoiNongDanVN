using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class BCBDHoiVienController : BaseController
    {
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 8;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        public BCBDHoiVienController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context)
        {
            _hostEnvironment = hostEnvironment;
            DateFomat = config.GetSection("SiteSettings:DateFormat").Value.ToString().Split(',');
        }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBagSearch();
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(string? MaQuanHuyen, Guid? MaDiaBanHoatDong, int Thang, int Nam)
        {
            return ExecuteSearch(() => {
                var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoatDong, Thang: Thang, Nam: Nam);
                return PartialView(model);
            });
        }
        public IActionResult ExportEdit(String? MaQuanHuyen, Guid? MaDiaBanHoatDong, int Thang, int Nam)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\BCBDHoiVien.xlsx");
            var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoatDong, Thang: Thang, Nam: Nam);

            byte[] filecontent = ClassExportExcel.ExportExcel(model, startIndex, url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "BCBDHoiVien");

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Index
        #region Helper
        private void CreateViewBagSearch()
        {
            var data = (from hv in _context.CanBos
                        join diaban in _context.DiaBanHoatDongs on hv.MaDiaBanHoatDong equals diaban.Id
                        join quanhuyen in _context.QuanHuyens on diaban.MaQuanHuyen equals quanhuyen.MaQuanHuyen
                        where hv.IsHoiVien == true
                        && GetPhamVi().Contains(diaban.Id)
                        select new
                        {
                            MaDiaBanHoatDong = diaban.Id,
                            Name = diaban.TenDiaBanHoatDong,
                            MaQuanHuyen = quanhuyen.MaQuanHuyen,
                            TenQuanHuyen = quanhuyen.TenQuanHuyen
                        }
                                  ).Distinct().ToList();

            var diaBanHoatDong = data.Select(it => new { MaDiaBanHoatDong = it.MaDiaBanHoatDong, Name = it.Name }).Distinct().ToList();
            ViewBag.MaDiaBanHoatDong = new SelectList(diaBanHoatDong, "MaDiaBanHoatDong", "Name");

            var quanHuyen = data.Select(it => new { MaQuanHuyen = it.MaQuanHuyen, TenQuanHuyen = it.TenQuanHuyen }).Distinct().ToList();
            ViewBag.MaQuanHuyen = new SelectList(quanHuyen, "MaQuanHuyen", "TenQuanHuyen");

        }
        public JsonResult loadDiaBanHoatDong(string? maQuanHuyen)
        {
            if (!String.IsNullOrWhiteSpace(maQuanHuyen))
            {
                var diaBanHoatDong = _context.DiaBanHoatDongs.Where(it => it.Actived == true && it.MaQuanHuyen == maQuanHuyen).Select(it => new { MaDiaBanHoatDong = it.Id, Name = it.TenDiaBanHoatDong }).Distinct().ToList();
                return Json(diaBanHoatDong);
            }
            else
            {
                var data = (from hv in _context.CanBos
                            join diaban in _context.DiaBanHoatDongs on hv.MaDiaBanHoatDong equals diaban.Id
                            where hv.IsHoiVien == true && diaban.Actived == true
                              && GetPhamVi().Contains(diaban.Id)
                            select new
                            {
                                MaDiaBanHoatDong = diaban.Id,
                                Name = diaban.TenDiaBanHoatDong,
                            }
                                 ).Distinct().ToList();
                return Json(data);
            }

        }
        private List<BCBDHoiVien> LoadData(string? MaQuanHuyen, Guid? MaDiaBanHoatDong, int Thang, int Nam)
        {
            DateTime dateNow = new DateTime(Nam,Thang,DateTime.DaysInMonth(Nam,Thang));

            var model = _context.CanBos.Where(it => it.IsHoiVien == true).Include(it => it.DiaBanHoatDong).ThenInclude(it => it.QuanHuyen).AsQueryable();
            if (!String.IsNullOrEmpty(MaQuanHuyen)) {
                model = model.Where(it => it.DiaBanHoatDong.MaQuanHuyen == MaQuanHuyen);
            }
            if (MaDiaBanHoatDong != null)
            {
                model = model.Where(it => it.MaDiaBanHoatDong == MaDiaBanHoatDong);
            }
            var modeltemp = model.Select(it => new {
                it.IDCanBo,
                it.HoVaTen,
                it.DiaBanHoatDong!.TenDiaBanHoatDong,
                it.DiaBanHoatDong.Id,
                it.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                it.DiaBanHoatDong.MaQuanHuyen,
                NgayVaoHoi = Function.ConvertStringToDate(it.NgayVaoHoi),
                it.NgayRoiHoi,
                it.HoiVienDuyet,
                it.CreatedTime,
                it.NgayDuyet,
                it.isRoiHoi,
                it.Actived,
            }).ToList();
            var data = modeltemp.GroupBy(it => new { it.MaQuanHuyen, it.TenQuanHuyen }).Select(p => new BCBDHoiVien
            {
                Ma = p.Key.MaQuanHuyen,
                Ten = p.Key.TenQuanHuyen,
                Giam = p.Where(lg => lg.isRoiHoi == true && lg.NgayRoiHoi!.Value.Month == dateNow.Month && lg.NgayRoiHoi.Value.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count(),
                ThemMoi = p.Where(lg => lg.NgayVaoHoi.Month == dateNow.Month && lg.NgayVaoHoi.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count(),
                SL = p.Where(lg => lg.isRoiHoi != true && lg.HoiVienDuyet == true && lg.Actived == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen && lg.MaQuanHuyen == p.Key.MaQuanHuyen && lg.NgayVaoHoi <= dateNow).Count(),
                ChoDuyet = p.Where(lg => lg.HoiVienDuyet == false && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count()
            }).ToList();
            return data;
        }

        #endregion Helper
    }
}
