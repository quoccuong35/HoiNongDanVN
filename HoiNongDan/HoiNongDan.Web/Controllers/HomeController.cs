using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Models;
using HoiNongDan.DataAccess;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using HoiNongDan.Extensions;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Controllers
{

   [Authorize]
    public class HomeController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;
        public HomeController(AppDbContext context, ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContext) :base(context)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public IActionResult Index()
        {
            string menu = HttpContext.Session!.GetString("Menu");
            return View("SoDo");
        }

        public IActionResult Dashboard()
        {
            return View("Dashboard");
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Print()
        {
            string mintype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\reports\\Report1.rdlc";
            Dictionary<String, string> parameters = new Dictionary<string, string>();
            parameters.Add("pr1", "Chào mừng bạn đến với report ");
            LocalReport localReport = new LocalReport(path);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mintype);
            return File(result.MainStream, "application/pdf");
        }
        public JsonResult _Dashboard() {
            var data = _context.CanBos.Include(it=>it.DiaBanHoatDong).ThenInclude(it=>it.QuanHuyen!).Where(it => it.Actived == true && ((it.IsCanBo == true && it.MaTinhTrang == "01") || (it.IsHoiVien == true))).ToList();
            var HoiVienTheoQH = data.Where(it=>it.IsHoiVien==true).GroupBy(it => it.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen).Select(it => new {
                TenQuanHuyen = it.Key,
                TongNam = it.Where(p=>p.GioiTinh==GioiTinh.Nam).Count(),
                TongNu = it.Where(p=>p.GioiTinh==GioiTinh.Nữ).Count(),
            });
            return Json(HoiVienTheoQH);
        }
    }
    public class TongSo { 
        public string Name { get; set; }
        public string TrongSo { get; set; }
    }
    public class TongSoHoiVienQuanHuyen
    {
        public string Name { get; set; }
        public string TrongSoNam { get; set; }
        public string TrongSoNu { get; set; }
    }

    public class Dashboard1 {
        public List<TongSo> CanBoHoiVien { get; set; } 
        public TongSoHoiVienQuanHuyen HoiVien { get; set; }
        public List<TongSo> NamNu { get; set; }
    }
}