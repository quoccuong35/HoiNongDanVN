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

            BCHVPhatTrienMoiSearchVM searchVM = new BCHVPhatTrienMoiSearchVM();

            searchVM.TuNgay = StartOfQuarter();
            searchVM.DenNgay = EndOfQuarter();
            CreateViewBagSearch();
            return View(searchVM);
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(BCHVPhatTrienMoiSearchVM searchVM)
        {
            return ExecuteSearch(() => {
              var model=   LoatData(searchVM);
                return PartialView(model);
            });
        }

        public IActionResult ExportEdit(BCHVPhatTrienMoiSearchVM searchVM)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\BaoCaoHoiVienGiam.xlsx");
            var model = LoatData(searchVM);
            if (model.Count == 0)
            {
                BaoCaoGiamHVVM add = new BaoCaoGiamHVVM();
                model.Add(add);
            }
            byte[] filecontent = ClassExportExcel.ExportExcel(model, startIndex, url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "BaoCaoHoiVienGiam");

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Index

        #region Helper
        private void CreateViewBagSearch()
        {

            FnViewBag fnViewBag = new FnViewBag(_context);


            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());

            var chucVus = _context.ChucVus.Where(it => it.HoiVien == true).Select(it => new { it.MaChucVu, it.TenChucVu }).ToList();
            ViewBag.MaChucVu = new SelectList(chucVus, "MaChucVu", "TenChucVu");

        }
       
        [NonAction]
        private List<BaoCaoGiamHVVM> LoatData(BCHVPhatTrienMoiSearchVM searchVM) {
            try
            {
                //var data = _context.CanBos.Include(it => it.ChiHoi).Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen).Where(it => it.IsHoiVien == true && it.isRoiHoi == true);

                searchVM.TuNgay = searchVM.TuNgay != null?searchVM.TuNgay : StartOfQuarter();
                searchVM.DenNgay = searchVM.DenNgay != null ? searchVM.DenNgay : EndOfQuarter();
                var data = (from cb in _context.CanBos
                             join pv in _context.PhamVis on cb.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                             where pv.AccountId == AccountId()
                             && cb.IsHoiVien == true
                             && cb.HoiVienDuyet == true
                            select cb).Include(it => it.ChiHoi).Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen).Where(it => it.IsHoiVien == true && it.isRoiHoi == true);
                if (searchVM.MaDiaBanHoiVien != null)
                {
                    data = data.Where(it => it.MaDiaBanHoatDong == searchVM.MaDiaBanHoiVien);
                }
                if (!String.IsNullOrWhiteSpace(searchVM.MaQuanHuyen))
                {
                    data = data.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == searchVM.MaQuanHuyen);
                }
                if (searchVM.TuNgay != null)
                {
                    data = data.Where(it => it.NgayRoiHoi >= searchVM.TuNgay); ;
                }
                if (searchVM.DenNgay != null)
                {
                    data = data.Where(it => it.NgayRoiHoi <= searchVM.DenNgay);
                }
                var model = data.Select(it => new BaoCaoGiamHVVM
                {
                    HoVaTen = it.HoVaTen,
                    Nam = (int)it.GioiTinh == 1 ? it.NgaySinh : "",
                    Nu = (int)it.GioiTinh != 1 ? it.NgaySinh : "",
                    QuanHuyen = it.DiaBanHoatDong!.QuanHuyen!.TenQuanHuyen,
                    TenHoi = it.DiaBanHoatDong!.TenDiaBanHoatDong,
                    DiaChi = it.ChoOHienNay,
                    SoCCCD = it.SoCCCD,
                    NgayGiam = it.NgayRoiHoi.Value.ToString("dd/MM/yyyy"),
                    ChiHoi = it.ChiHoi!.TenChiHoi,
                    LyDoGiam = it.LyDoRoiHoi,
                    NamVaoHoi = it.NgayVaoHoi == null ? null : it.NgayVaoHoi.Value.ToString("dd/MM/yyyy")

                }).ToList().Select((it, index) => new BaoCaoGiamHVVM
                {
                    STT = index +1,
                    HoVaTen = it.HoVaTen,
                    Nam = it.Nam,
                    Nu = it.Nu,
                    QuanHuyen = it.QuanHuyen,
                    TenHoi = it.TenHoi,
                    SoCCCD = it.SoCCCD,
                    NgayGiam = it.NgayGiam,
                    DiaChi = it.DiaChi == null ? "" : it.DiaChi,
                    ChiHoi = it.ChiHoi ==null?"": it.ChiHoi,
                    LyDoGiam = it.LyDoGiam == null ? "" : it.LyDoGiam,
                    NamVaoHoi = it.NamVaoHoi == null ? null : it.NamVaoHoi,

                }).OrderBy(it=>it.QuanHuyen).ThenBy(it=>it.TenHoi).ToList();
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
