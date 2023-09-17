using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Models;
using HoiNongDan.DataAccess;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using HoiNongDan.Extensions;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;

namespace HoiNongDan.Web.Controllers
{

   [Authorize]
    public class HomeController : BaseOfWorkController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) :base(unitOfWork)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public IActionResult Index()
        {
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
    }
}