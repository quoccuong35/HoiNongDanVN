using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    public class NgheNghiepController : BaseController
    {
        public NgheNghiepController(AppDbContext context):base(context) { }
        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion Index
    }
}
