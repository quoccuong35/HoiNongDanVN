using HoiNongDan.Constant;
using Microsoft.AspNetCore.Mvc;

namespace HoiNongDan.Web.Components
{
    [ViewComponent(Name = "Sidebar")]
    public class Sidebar :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string menu = HttpContext.Session!.GetString(User.Identity!.Name + ConstExcelController.SessionMenu);
            return View("Default", menu);
        }
    }
}
