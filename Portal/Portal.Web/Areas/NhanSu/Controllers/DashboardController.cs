using Microsoft.AspNetCore.Mvc;
using Portal.Constant;
using Portal.DataAccess;
using Portal.Extensions;

namespace Portal.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class DashboardController : BaseController
    {
        public DashboardController(AppDbContext context) : base(context) { }
        public IActionResult Index()
        {
            return View();
        }
    }
}
