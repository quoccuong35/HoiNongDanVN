using Microsoft.AspNetCore.Mvc;

namespace Portal.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult ErrorNotFound(string data) { 
            ViewBag.Data = data;
            return View();
        }
    }
}
