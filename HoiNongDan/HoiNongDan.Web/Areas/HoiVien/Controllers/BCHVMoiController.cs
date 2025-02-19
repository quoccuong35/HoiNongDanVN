using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
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
            BCHVPhatTrienMoiSearchVM searchVM = new BCHVPhatTrienMoiSearchVM();

            searchVM.TuNgay = StartOfQuarter();
            searchVM.DenNgay = EndOfQuarter();

            CreateViewBagSearch();
            return View(searchVM);
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(BCHVPhatTrienMoiSearchVM searchVM) {
            return ExecuteSearch(() => { 
                var model = LoadData(searchVM);
                return PartialView(model);
            });
        }
        public IActionResult ExportEdit(BCHVPhatTrienMoiSearchVM searchVM)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\BCHVMoi.xlsx");
            var model = LoadData(searchVM);
            
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
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());

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
        private List<BCHVPhatTrienMoiExcelVM> LoadData(BCHVPhatTrienMoiSearchVM searchVM) {

            var data = (from cb in _context.CanBos
                        join pv in _context.PhamVis on cb.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                        where pv.AccountId == AccountId()
                        && cb.IsHoiVien == true
                        && cb.HoiVienDuyet == true
                        select cb).Where(it => it.IsHoiVien == true && it.isRoiHoi != true).Include(it => it.DanToc).Include(it => it.NgheNghiep).
                Include(it => it.TonGiao).Include(it => it.TrinhDoHocVan).Include(it => it.TrinhDoChuyenMon).Include(it=>it.ChiHoi).Include(it=>it.ToHoi)
                .Include(it => it.TrinhDoChinhTri).Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen).AsQueryable();
            //Lấy ngày hiện tại

            if (searchVM.TuNgay == null)
            { searchVM.TuNgay = StartOfQuarter(); }
            if (searchVM.DenNgay == null)
            { searchVM.DenNgay = EndOfQuarter(); }
            if (!String.IsNullOrWhiteSpace(searchVM.MaQuanHuyen))
            {
                data = data.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == searchVM.MaQuanHuyen);
            }
            if (searchVM.MaDiaBanHoiVien != null)
            {
                data = data.Where(it => it.MaDiaBanHoatDong == searchVM.MaDiaBanHoiVien);
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
                TenQuanHuyen = it.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                TenHoi = it.DiaBanHoatDong.TenDiaBanHoatDong,
                TrinhDoHocVan = it.TrinhDoHocVan.TenTrinhDoHocVan,
                TrinhDoChuyenMon = it.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                ChinhTri = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                NgayThangVaoHoi = it.NgayVaoHoi,
                NgheNghiep = it.NgheNghiep!.TenNgheNghiep,
                DiaBanDanCu = it.HoiVienDanCu == true? "X":"",
                NganhNghe = it.HoiVienNganhNghe == true  ? "X" : "",
                SoThe = it.MaCanBo,
                NgayCapThe = it.NgayCapThe != null? it.NgayCapThe.Value.ToString("dd/MM/yyyy") : ""
            }).ToList().Where(it=> it.NgayThangVaoHoi != null && it.NgayThangVaoHoi.Value >= searchVM.TuNgay && it.NgayThangVaoHoi <= searchVM. DenNgay).OrderBy(it => it.TenQuanHuyen).ThenBy(it => it.TenHoi).Select((it, index) => new BCHVPhatTrienMoiExcelVM
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
                TenQuanHuyen = it.TenQuanHuyen,
                TenHoi = it.TenHoi,
                ChinhTri = it.ChinhTri,
                NgayThangVaoHoi = it.NgayThangVaoHoi != null?it.NgayThangVaoHoi.Value.ToString("dd/MM/yyyy"):"",
                NgheNghiep = it.NgheNghiep,
                DiaBanDanCu = it.DiaBanDanCu,
                NganhNghe = it.NganhNghe,
                SoThe = it.SoThe,
                NgayCapThe = it.NgayCapThe

            }).OrderBy(it=>it.TenQuanHuyen).ThenBy(it=>it.TenHoi).ToList();
            return model;
        }

        #endregion Helper
    }
}
