using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
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
