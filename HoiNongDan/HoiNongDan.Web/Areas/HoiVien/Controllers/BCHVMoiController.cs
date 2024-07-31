using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class BCHVMoiController : BaseController
    {
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 12;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        public BCHVMoiController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context) {
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
        public IActionResult _Search(string? MaQuanHuyen, Guid? MaDiaBanHoatDong, DateTime? TuNgay, DateTime? DenNgay) {
            return ExecuteSearch(() => { 
                var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoatDong, TuNgay: TuNgay, DenNgay: DenNgay);
                return PartialView(model);
            });
        }
        public IActionResult ExportEdit(String? MaQuanHuyen, Guid? MaDiaBanHoatDong, DateTime? TuNgay, DateTime? DenNgay)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\BCHVMoi.xlsx");
            var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoatDong, TuNgay: TuNgay, DenNgay: DenNgay);

            byte[] filecontent = ClassExportExcel.ExportExcel(model, startIndex, url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "BCHVMoi");

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
        private List<BCHVPhatTrienMoiVM> LoadData(string? MaQuanHuyen, Guid? MaDiaBanHoatDong, DateTime? TuNgay, DateTime? DenNgay) {
            var data = _context.CanBos.Where(it => it.IsHoiVien == true && it.isRoiHoi != true).Include(it => it.DanToc).Include(it => it.NgheNghiep).
                Include(it => it.TonGiao).Include(it => it.TrinhDoHocVan).Include(it => it.TrinhDoChuyenMon)
                .Include(it => it.TrinhDoChinhTri).Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen).AsQueryable();
            if (TuNgay == null)
            { TuNgay = DateTime.Now; }
            if (DenNgay == null)
            { DenNgay = DateTime.Now; }
            if (!String.IsNullOrWhiteSpace(MaQuanHuyen))
            {
                data = data.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == MaQuanHuyen);
            }
            if (MaDiaBanHoatDong != null)
            {
                data = data.Where(it => it.MaDiaBanHoatDong == MaDiaBanHoatDong);
            }
            var model = data.Select(it => new BCHVPhatTrienMoiVM
            {
                HoVaTen = it.HoVaTen,
                Nam = (int)it.GioiTinh == 1 ? "Nam" : null,
                Nu = (int)it.GioiTinh == 0 ? "Nữ" : null,
                SoCCCD = it.SoCCCD,
                NgayCapSoCCCD = it.NgayCapCCCD,
                HoKhauThuongTru = it.HoKhauThuongTru,
                NoiOHiennay = it.ChoOHienNay,
                SoDienThoai = it.SoDienThoai,
                DangVien = "",
                DanToc = it.DanToc!.TenDanToc,
                TonGiao = it.TonGiao!.TenTonGiao,
                TrinhDoHocVan = it.TrinhDoHocVan.TenTrinhDoHocVan,
                TrinhDoChuyenMon = it.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                ChinhTri = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                NgayThangVaoHoi = it.NgayVaoHoi,
                NgheNghiep = it.NgheNghiep!.TenNgheNghiep,
                DiaBanDanCu = "",
                NganhNghe = "",
                SoThe = it.MaCanBo,
                NgayCapThe = ""
            }).ToList().Where(it=> Function.ConvertStringToDate(it.NgayThangVaoHoi!) >= TuNgay && Function.ConvertStringToDate(it.NgayThangVaoHoi!) <= DenNgay).Select((it, index) => new BCHVPhatTrienMoiVM
            {
                STT = index +1,
                HoVaTen = it.HoVaTen,
                Nam = it.Nam,
                Nu = it.Nu,
                SoCCCD = it.SoCCCD,
                NgayCapSoCCCD = it.NgayCapSoCCCD,
                HoKhauThuongTru = it.HoKhauThuongTru,
                NoiOHiennay = it.NoiOHiennay,
                SoDienThoai = it.SoDienThoai,
                DangVien = "",
                DanToc = it.DanToc,
                TonGiao = it.TonGiao,
                TrinhDoHocVan = it.TrinhDoHocVan,
                TrinhDoChuyenMon = it.TrinhDoChuyenMon,
                ChinhTri = it.ChinhTri,
                NgayThangVaoHoi = it.NgayThangVaoHoi,
                NgheNghiep = it.NgheNghiep,
                DiaBanDanCu = "",
                NganhNghe = it.NgheNghiep,
                SoThe = it.SoThe,
                NgayCapThe = ""

            }).ToList();
            return model;
        }

        #endregion Helper
    }
}
