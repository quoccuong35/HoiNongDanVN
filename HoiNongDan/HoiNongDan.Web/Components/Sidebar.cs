using Microsoft.AspNetCore.Mvc;

namespace HoiNongDan.Web.Components
{
    [ViewComponent(Name = "Sidebar")]
    public class Sidebar :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string menu = HttpContext.Session!.GetString("Menu");
            return View("Default", menu);
        }
    }
}
