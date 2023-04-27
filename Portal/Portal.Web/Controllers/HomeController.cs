using Microsoft.AspNetCore.Mvc;
using Portal.Models;
using Portal.DataAccess;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Portal.Extensions;

namespace Portal.Web.Controllers
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
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}