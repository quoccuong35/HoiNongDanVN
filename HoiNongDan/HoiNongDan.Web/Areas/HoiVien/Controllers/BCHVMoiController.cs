using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class BCHVMoiController : BaseController
    {
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 11;
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
            //byte[] filecontent;
            //using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(url), false)) {
            //    ExcelWorksheet workSheet = package.Workbook.Worksheets["Data"];
            //    int startIndex = 11;
            //    workSheet.Cells["A" + startIndex].LoadFromCollection(model, false);
            //    using (ExcelRange r = workSheet.Cells["A11:U" + startIndex.ToString()])
            //    {
            //        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            //        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            //        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            //        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            //        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
            //        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
            //        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
            //        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
            //    }
            //    filecontent = package.GetAsByteArray();
            //}
            if (model.Count == 0)
            {
                BCHVPhatTrienMoiExcelVM add = new BCHVPhatTrienMoiExcelVM();
                model.Add(add);
            }
               byte[] filecontent = ClassExportExcel.ExportExcel(model, startIndex, url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "BCHVMoi");

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Index
        #region Helper
        private void CreateViewBagSearch()
        {
            var phamVis = Function.GetPhamVi(AccountId: AccountId()!.Value, _context: _context);
            var data = (from hv in _context.CanBos
                        join diaban in _context.DiaBanHoatDongs on hv.MaDiaBanHoatDong equals diaban.Id
                        join quanhuyen in _context.QuanHuyens on diaban.MaQuanHuyen equals quanhuyen.MaQuanHuyen
                        where hv.IsHoiVien == true
                        && phamVis.Contains(diaban.Id)
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
                var phamVis = Function.GetPhamVi(AccountId: AccountId()!.Value, _context: _context);
                var data = (from hv in _context.CanBos
                            join diaban in _context.DiaBanHoatDongs on hv.MaDiaBanHoatDong equals diaban.Id
                            where hv.IsHoiVien == true && diaban.Actived == true
                              && phamVis.Contains(diaban.Id)
                            select new
                            {
                                MaDiaBanHoatDong = diaban.Id,
                                Name = diaban.TenDiaBanHoatDong,
                            }
                                 ).Distinct().ToList();
                return Json(data);
            }

        }
        private List<BCHVPhatTrienMoiExcelVM> LoadData(string? MaQuanHuyen, Guid? MaDiaBanHoatDong, DateTime? TuNgay, DateTime? DenNgay) {
            //var data = _context.CanBos.Where(it => it.IsHoiVien == true && it.isRoiHoi != true).Include(it => it.DanToc).Include(it => it.NgheNghiep).
            //    Include(it => it.TonGiao).Include(it => it.TrinhDoHocVan).Include(it => it.TrinhDoChuyenMon)
            //    .Include(it => it.TrinhDoChinhTri).Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen).AsQueryable();
            var data = (from cb in _context.CanBos
                        join pv in _context.PhamVis on cb.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                        where pv.AccountId == AccountId()
                        && cb.IsHoiVien == true
                        && cb.HoiVienDuyet == true
                        select cb).Where(it => it.IsHoiVien == true && it.isRoiHoi != true).Include(it => it.DanToc).Include(it => it.NgheNghiep).
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
                NgayCapThe = it.NgayCapThe != null? it.NgayCapThe.Value.ToString("dd/MM/yyyy") : ""
            }).ToList().Where(it=> it.NgayThangVaoHoi != null && it.NgayThangVaoHoi.Value >= TuNgay && it.NgayThangVaoHoi <= DenNgay).Select((it, index) => new BCHVPhatTrienMoiExcelVM
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
                NgayThangVaoHoi = it.NgayThangVaoHoi != null?it.NgayThangVaoHoi.Value.ToString("dd/MM/yyyy"):"",
                NgheNghiep = it.NgheNghiep,
                DiaBanDanCu = "",
                NganhNghe = it.NgheNghiep,
                SoThe = it.SoThe,
                NgayCapThe = it.NgayCapThe

            }).ToList();
            return model;
        }

        #endregion Helper
    }
}
