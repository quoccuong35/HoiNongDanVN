using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Models;
using HoiNongDan.DataAccess;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using HoiNongDan.Extensions;

namespace HoiNongDan.Web.Controllers
{

   [Authorize]
    public class HomeController : BaseOfWorkController
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork):base(unitOfWork)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("SoDoToChuc");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}