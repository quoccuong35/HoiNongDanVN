using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class BCSLMau1Controller : BaseController
    {
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 8;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        public BCSLMau1Controller(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context)
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
        public IActionResult _Search(string? MaQuanHuyen, Guid? MaDiaBanHoiVien, Guid? MaChiHoi = null)
        {
            return ExecuteSearch(() => {
                var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoiVien: MaDiaBanHoiVien, MaChiHoi: MaChiHoi).OrderBy(it => it.TongSL).ToList();
                return PartialView(model);
            });
        }
        public IActionResult ExportEdit(String? MaQuanHuyen, Guid? MaDiaBanHoiVien, Guid? MaChiHoi = null)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\ThongKeSoLuongHVDangHoatDong.xlsx");
            var data = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoiVien: MaDiaBanHoiVien, MaChiHoi: MaChiHoi)
                .Select((it,index)=>new BCSLMau1Excel { 
                    Stt = index +1,
                    Ten = it.Ten,
                    TongSL = it.TongSL,
                    TongNu = it.TongNu,
                    DoTuoi40  = it.DoTuoi40,
                    DoTuoi60 = it.DoTuoi60,
                    DoTuoiTren60 = it.DoTuoiTren60,
                    TongDanToc = it.TongDanToc,
                    ChiHoiDanCu = it.ChiHoiDanCu,
                    ChiHoiNganhNghe = it.ChiHoiNganhNghe,
                    DangVien = it.DangVien,
                    UuTu = it.UuTu,
                    HVNongCot = it.HVNongCot,
                })
                .OrderBy(it=>it.TongSL).ToList();
            

            byte[] filecontent = ClassExportExcel.ExportExcel(data, startIndex, url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "ThongKeSoLuongHVDangHoatDong");

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Index
        #region Helper
        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId() );

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
            var chiHoi = _context.ToHoiNganhNghe_ChiHoiNganhNghes.Where(it=>it.Loai =="01" && it.Actived == true).Select(it=>new {
                MaChiHoi= it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe,
                Ten = it.Ten
            }).ToList();
            ViewBag.MaChiHoi = new SelectList(chiHoi, "MaChiHoi", "Ten");

        }
     
        private List<BCSLMau1> LoadData(string? MaQuanHuyen, Guid? MaDiaBanHoiVien, Guid? MaChiHoi)
        {
            try
            {
                DateTime dateNow = DateTime.Now;

                var model = _context.CanBos.Where(it => it.IsHoiVien == true && it.isRoiHoi != true && it.Actived == true && it.HoiVienDuyet == true).Include(it => it.DiaBanHoatDong).ThenInclude(it => it.QuanHuyen).Include(it=>it.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens).ThenInclude(it=>it.ToHoiNganhNghe_ChiHoiNganhNghe).AsQueryable();
                if (!String.IsNullOrEmpty(MaQuanHuyen))
                {
                    model = model.Where(it => it.DiaBanHoatDong.MaQuanHuyen == MaQuanHuyen);
                }
                if (MaDiaBanHoiVien != null)
                {
                    model = model.Where(it => it.MaDiaBanHoatDong == MaDiaBanHoiVien);
                }
                if (MaChiHoi != null)
                {
                    model = model.Where(it => it.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Any(p=>p.Ma_ToHoiNganhNghe_ChiHoiNganhNghe == MaChiHoi));
                }
                var modeltemp = model.Select(it => new {
                    it.IDCanBo,
                    it.HoVaTen,
                    it.DiaBanHoatDong!.TenDiaBanHoatDong,
                    it.DiaBanHoatDong.Id,
                    it.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                    it.DiaBanHoatDong.MaQuanHuyen,
                    NgaySinh = Function.ConvertStringToDate(it.NgaySinh!),
                    it.NgayRoiHoi,
                    it.HoiVienDuyet,
                    it.CreatedTime,
                    it.MaDanToc,
                    it.TuChoi,
                    it.NgayDuyet,
                    it.GioiTinh,
                    it.HoiVienNongCot,
                    it.NgayVaoDangChinhThuc
                }).ToList();
                var uuTu = _context.QuaTrinhKhenThuongs.Where(it => it.IsHoiVien == true && it.MaDanhHieuKhenThuong == "14").Select(it => it.IDCanBo).Distinct().ToList();
                var nganhNghe = _context.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Include(it=>it.ToHoiNganhNghe_ChiHoiNganhNghe).Where(it => it.ToHoiNganhNghe_ChiHoiNganhNghe.Loai == "01").Select(it => it.IDHoiVien).Distinct().ToList();
                List<BCSLMau1> data = new List<BCSLMau1>();
                if (String.IsNullOrWhiteSpace(MaQuanHuyen) && MaDiaBanHoiVien == null)
                {
                     data = modeltemp.GroupBy(it => new { it.MaQuanHuyen, it.TenQuanHuyen }).Select(p => new BCSLMau1
                    {
                        Ma = p.Key.MaQuanHuyen,
                        Ten = p.Key.TenQuanHuyen,
                        TongSL = p.Count(),
                        TongNu = p.Where(it => it.GioiTinh == GioiTinh.Nữ).Count(),
                        DoTuoi40 = p.Where(it => (dateNow.Year - it.NgaySinh.Year + 1) < 40).Count(),
                        DoTuoi60 = p.Where(it => (((dateNow.Year - it.NgaySinh.Year + 1) > 39) && (dateNow.Year - it.NgaySinh.Year + 1) < 60)).Count(),
                        DoTuoiTren60 = p.Where(it => (dateNow.Year - it.NgaySinh.Year + 1) > 60).Count(),
                        TongDanToc = p.Where(it => it.MaDanToc != null && it.MaDanToc != "KH" && it.MaDanToc != "KINH").Count(),
                        ChiHoiDanCu = (p.Count() - p.Where(it => nganhNghe.Contains(it.IDCanBo)).Count()),
                        ChiHoiNganhNghe = p.Where(it => nganhNghe.Contains(it.IDCanBo)).Count(),
                        DangVien = p.Where(it => !String.IsNullOrWhiteSpace(it.NgayVaoDangChinhThuc)).Count(),
                        UuTu = p.Where(it => uuTu.Contains(it.IDCanBo)).Count(),
                        HVNongCot = p.Where(it => it.HoiVienNongCot == true).Count(),
                    }).OrderBy(it=>it.TongSL).ToList();
                   
                }
                else
                {
                     data = modeltemp.GroupBy(it => new { it.Id, it.TenDiaBanHoatDong }).Select(p => new BCSLMau1
                    {
                        Ma = p.Key.Id.ToString(),
                        Ten = p.Key.TenDiaBanHoatDong,
                        TongSL = p.Count(),
                        TongNu = p.Where(it => it.GioiTinh == GioiTinh.Nữ).Count(),
                        DoTuoi40 = p.Where(it => (dateNow.Year - it.NgaySinh.Year + 1) < 40).Count(),
                        DoTuoi60 = p.Where(it => (((dateNow.Year - it.NgaySinh.Year + 1) > 39) && (dateNow.Year - it.NgaySinh.Year + 1) < 60)).Count(),
                        DoTuoiTren60 = p.Where(it => (dateNow.Year - it.NgaySinh.Year + 1) > 60).Count(),
                        TongDanToc = p.Where(it => it.MaDanToc != null && it.MaDanToc != "KH" && it.MaDanToc != "KINH").Count(),
                        ChiHoiDanCu = (p.Count() - p.Where(it => nganhNghe.Contains(it.IDCanBo)).Count()),
                        ChiHoiNganhNghe = p.Where(it => nganhNghe.Contains(it.IDCanBo)).Count(),
                        DangVien = p.Where(it => !String.IsNullOrWhiteSpace(it.NgayVaoDangChinhThuc)).Count(),
                        UuTu = p.Where(it => uuTu.Contains(it.IDCanBo)).Count(),
                        HVNongCot = p.Where(it => it.HoiVienNongCot == true).Count(),
                    }).OrderBy(it => it.TongSL).ToList();
                }
                if (data.Count > 0)
                {
                    BCSLMau1 tong = new BCSLMau1 { 
                        Ma = "",
                        Ten = "Tổng cộng",
                        TongSL = data.Sum(it=>it.TongSL),
                        TongNu = data.Sum(it=>it.TongNu),
                        DoTuoi40 = data.Sum(it=>it.DoTuoi40),
                        DoTuoi60 = data.Sum(it=>it.DoTuoi60),
                        DoTuoiTren60 = data.Sum(it => it.DoTuoiTren60),
                        TongDanToc = data.Sum(it => it.TongDanToc),
                        ChiHoiDanCu = data.Sum(it => it.ChiHoiDanCu),
                        ChiHoiNganhNghe = data.Sum(it => it.ChiHoiNganhNghe),
                        DangVien = data.Sum(it => it.DangVien),
                        UuTu = data.Sum(it => it.UuTu),
                        HVNongCot = data.Sum(it => it.HVNongCot),
                    };
                    data.Add(tong);
                }
                return data;
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
                throw;
            }


        }

        #endregion Helper
    }
}
