using Microsoft.AspNetCore.Mvc;

namespace HoiNongDan.Web.Controllers
{
    public class ReportController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Print() {
            return View();
        }
    }
}
