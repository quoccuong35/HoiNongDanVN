using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;


namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class BaoCaoGiamHVController : BaseController
    {
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 9;
        private readonly IWebHostEnvironment _hostEnvironment;
        private  string[] DateFomat;
        public BaoCaoGiamHVController(AppDbContext context,IWebHostEnvironment hostEnvironment, IConfiguration config) :base(context) {
            _hostEnvironment = hostEnvironment;
            DateFomat = config.GetSection("SiteSettings:DateFormat").Value.ToString().Split(',');
        }

        #region Index
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Index()
        {
            CreateViewBagSearch();
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(String? MaQuanHuyen, Guid? MaDiaBanHoatDong, DateTime? TuNgay, DateTime? DenNgay)
        {
            return ExecuteSearch(() => {
              var model=   LoatData(MaQuanHuyen: MaQuanHuyen, MaDiaban: MaDiaBanHoatDong, TuNgay: TuNgay, DenNgay: DenNgay);
                return PartialView(model);
            });
        }

        public IActionResult ExportEdit(String? MaQuanHuyen, Guid? MaDiaBanHoatDong, DateTime? TuNgay, DateTime? DenNgay)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\BaoCaoHoiVienGiam.xlsx");
            var model = LoatData(MaQuanHuyen: MaQuanHuyen, MaDiaban: MaDiaBanHoatDong, TuNgay: TuNgay, DenNgay: DenNgay);
            
            byte[] filecontent = ClassExportExcel.ExportExcel(model, startIndex, url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "BaoCaoHoiVienGiam");

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
        [NonAction]
        private List<BaoCaoGiamHVVM> LoatData(String? MaQuanHuyen, Guid? MaDiaban, DateTime? TuNgay, DateTime? DenNgay) {
            try
            {
                var data = _context.CanBos.Include(it => it.ChiHoi).Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen).Where(it => it.IsHoiVien == true && it.isRoiHoi == true);
                if (MaDiaban != null)
                {
                    data = data.Where(it => it.MaDiaBanHoatDong == MaDiaban);
                }
                if (!String.IsNullOrWhiteSpace(MaQuanHuyen))
                {
                    data = data.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == MaQuanHuyen);
                }
                if (TuNgay != null)
                {
                    data = data.Where(it => it.NgayRoiHoi >= TuNgay); ;
                }
                if (DenNgay != null)
                {
                    data = data.Where(it => it.NgayRoiHoi <= DenNgay);
                }
                var model = data.Select(it => new BaoCaoGiamHVVM
                {
                    HoVaTen = it.HoVaTen,
                    Nam = (int)it.GioiTinh == 1 ? it.NgaySinh : "",
                    Nu = (int)it.GioiTinh != 1 ? it.NgaySinh : "",
                    DiaChi = it.ChoOHienNay,
                    ChiHoi = it.ChiHoi!.TenChiHoi,
                    LyDoGiam = it.LyDoRoiHoi,
                    NamVaoHoi = it.NgayVaoHoi

                }).ToList().Select((it, index) => new BaoCaoGiamHVVM
                {
                    STT = index +1,
                    HoVaTen = it.HoVaTen,
                    Nam = it.Nam,
                    Nu = it.Nu,
                    DiaChi = it.DiaChi == null ? "" : it.DiaChi,
                    ChiHoi = it.ChiHoi ==null?"": it.ChiHoi,
                    LyDoGiam = it.LyDoGiam == null ? "" : it.LyDoGiam,
                    NamVaoHoi = it.NamVaoHoi == null ? "" : it.NamVaoHoi,

                }).ToList();
                return model;
            }
            catch
            {
                return null!;
            }
        }
        #endregion Helper

    }
}
