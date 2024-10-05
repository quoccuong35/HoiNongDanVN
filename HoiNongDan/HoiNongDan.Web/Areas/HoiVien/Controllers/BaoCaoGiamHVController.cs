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
        public IActionResult _Search(String? MaQuanHuyen, Guid? MaDiaBanHoiVien, DateTime? TuNgay, DateTime? DenNgay)
        {
            return ExecuteSearch(() => {
              var model=   LoatData(MaQuanHuyen: MaQuanHuyen, MaDiaban: MaDiaBanHoiVien, TuNgay: TuNgay, DenNgay: DenNgay);
                return PartialView(model);
            });
        }

        public IActionResult ExportEdit(String? MaQuanHuyen, Guid? MaDiaBanHoiVien, DateTime? TuNgay, DateTime? DenNgay)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\BaoCaoHoiVienGiam.xlsx");
            var model = LoatData(MaQuanHuyen: MaQuanHuyen, MaDiaban: MaDiaBanHoiVien, TuNgay: TuNgay, DenNgay: DenNgay);
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

        }
       
        [NonAction]
        private List<BaoCaoGiamHVVM> LoatData(String? MaQuanHuyen, Guid? MaDiaban, DateTime? TuNgay, DateTime? DenNgay) {
            try
            {
                //var data = _context.CanBos.Include(it => it.ChiHoi).Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen).Where(it => it.IsHoiVien == true && it.isRoiHoi == true);


                var data = (from cb in _context.CanBos
                             join pv in _context.PhamVis on cb.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                             where pv.AccountId == AccountId()
                             && cb.IsHoiVien == true
                             && cb.HoiVienDuyet == true
                            select cb).Include(it => it.ChiHoi).Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen).Where(it => it.IsHoiVien == true && it.isRoiHoi == true);
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
                    QuanHuyen = it.DiaBanHoatDong!.QuanHuyen!.TenQuanHuyen,
                    TenHoi= it.DiaBanHoatDong!.TenDiaBanHoatDong,
                    DiaChi = it.ChoOHienNay,
                    ChiHoi = it.ChiHoi!.TenChiHoi,
                    LyDoGiam = it.LyDoRoiHoi,
                    NamVaoHoi = it.NgayVaoHoi == null?null:it.NgayVaoHoi.Value.ToString("dd/MM/yyyy")

                }).ToList().Select((it, index) => new BaoCaoGiamHVVM
                {
                    STT = index +1,
                    HoVaTen = it.HoVaTen,
                    Nam = it.Nam,
                    Nu = it.Nu,
                    QuanHuyen = it.QuanHuyen,
                    TenHoi = it.TenHoi,
                    DiaChi = it.DiaChi == null ? "" : it.DiaChi,
                    ChiHoi = it.ChiHoi ==null?"": it.ChiHoi,
                    LyDoGiam = it.LyDoGiam == null ? "" : it.LyDoGiam,
                    NamVaoHoi = it.NamVaoHoi == null ? null : it.NamVaoHoi,

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
