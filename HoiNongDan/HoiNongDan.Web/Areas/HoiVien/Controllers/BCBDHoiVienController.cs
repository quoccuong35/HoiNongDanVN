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
        const int startIndex = 7;
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
        public IActionResult _Search(string? MaQuanHuyen, Guid? MaDiaBanHoiVien, int Thang, int Nam)
        {
            return ExecuteSearch(() => {
                var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoiVien: MaDiaBanHoiVien, Thang: Thang, Nam: Nam);
                return PartialView(model);
            });
        }
        public IActionResult ExportEdit(String? MaQuanHuyen, Guid? MaDiaBanHoatDong, int Thang, int Nam)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\BCBDHoiVien.xlsx");
            var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoiVien: MaDiaBanHoatDong, Thang: Thang, Nam: Nam)
                .Select((it,index)=>new BCBDHoiVienExcel{ 
                    STT = index+1,
                    Ten = it.Ten,
                    SL = it.SL,
                    ThemMoi = it.ThemMoi,
                    Giam = it.Giam,
                    ChoDuyet = it.ChoDuyet,
                })
                .OrderBy(it=>it.SL).ToList();

            byte[] filecontent = ClassExportExcel.ExportExcel(model, startIndex, url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "BCBDHoiVien");

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
      
        private List<BCBDHoiVien> LoadData(string? MaQuanHuyen, Guid? MaDiaBanHoiVien, int Thang, int Nam)
        {
            try
            {
                DateTime dateNow = new DateTime(Nam, Thang, DateTime.DaysInMonth(Nam, Thang));

                var model = _context.CanBos.Where(it => it.IsHoiVien == true).Include(it => it.DiaBanHoatDong).ThenInclude(it => it.QuanHuyen).AsQueryable();
                if (!String.IsNullOrEmpty(MaQuanHuyen))
                {
                    model = model.Where(it => it.DiaBanHoatDong.MaQuanHuyen == MaQuanHuyen);
                }
                if (MaDiaBanHoiVien != null)
                {
                    model = model.Where(it => it.MaDiaBanHoatDong == MaDiaBanHoiVien);
                }
                var modeltemp = model.Select(it => new {
                    it.IDCanBo,
                    it.HoVaTen,
                    it.DiaBanHoatDong!.TenDiaBanHoatDong,
                    it.DiaBanHoatDong.Id,
                    it.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                    it.DiaBanHoatDong.MaQuanHuyen,
                    NgayVaoHoi = Function.ConvertStringToDate(it.NgayVaoHoi!),
                    it.NgayRoiHoi,
                    it.HoiVienDuyet,
                    it.CreatedTime,
                    it.TuChoi,
                    it.NgayDuyet,
                    it.isRoiHoi,
                    it.Actived,
                }).ToList();
                List<BCBDHoiVien> data = new List<BCBDHoiVien>();
                if (String.IsNullOrWhiteSpace(MaQuanHuyen) && MaDiaBanHoiVien == null)
                {
                     data = modeltemp.GroupBy(it => new { it.MaQuanHuyen, it.TenQuanHuyen }).Select(p => new BCBDHoiVien
                    {
                        Ma = p.Key.MaQuanHuyen,
                        Ten = p.Key.TenQuanHuyen,
                        Giam = p.Where(lg => lg.isRoiHoi == true && lg.NgayRoiHoi != null && lg.NgayRoiHoi!.Value.Month == dateNow.Month && lg.NgayRoiHoi.Value.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count(),
                        ThemMoi = p.Where(lg => lg.NgayVaoHoi.Month == dateNow.Month && lg.NgayVaoHoi.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count(),
                        SL = p.Where(lg => lg.isRoiHoi != true && lg.HoiVienDuyet == true && lg.Actived == true && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count(),
                        ChoDuyet = p.Where(lg => lg.HoiVienDuyet == false && lg.TuChoi == false && lg.MaQuanHuyen == p.Key.MaQuanHuyen).Count()
                    }).ToList();
                }
                else
                {
                     data = modeltemp.GroupBy(it => new { it.Id, it.TenDiaBanHoatDong }).Select(p => new BCBDHoiVien
                    {
                        Ma = p.Key.Id.ToString(),
                        Ten = p.Key.TenDiaBanHoatDong,
                        Giam = p.Where(lg => lg.isRoiHoi == true && lg.NgayRoiHoi != null && lg.NgayRoiHoi!.Value.Month == dateNow.Month && lg.NgayRoiHoi.Value.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.Id == p.Key.Id).Count(),
                        ThemMoi = p.Where(lg => lg.NgayVaoHoi.Month == dateNow.Month && lg.NgayVaoHoi.Year == dateNow.Year && lg.HoiVienDuyet == true && lg.Id == p.Key.Id).Count(),
                        SL = p.Where(lg => lg.isRoiHoi != true && lg.HoiVienDuyet == true && lg.Actived == true && lg.Id == p.Key.Id).Count(),
                        ChoDuyet = p.Where(lg => lg.HoiVienDuyet == false && lg.TuChoi == false && lg.Id == p.Key.Id).Count()
                    }).ToList();
                   
                }
                if (data.Count() > 0)
                {
                    BCBDHoiVien addTong = new BCBDHoiVien();
                    addTong.Ma = "";
                    addTong.Ten = "Tổng cộng";
                    addTong.SL = data.Sum(it => it.SL);
                    addTong.Giam = data.Sum(it => it.Giam);
                    addTong.ThemMoi = data.Sum(it => it.ThemMoi);
                    addTong.ChoDuyet = data.Sum(it => it.ChoDuyet);
                    data.Add(addTong);
                }
                return data.OrderBy(it => it.SL).ToList();
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
