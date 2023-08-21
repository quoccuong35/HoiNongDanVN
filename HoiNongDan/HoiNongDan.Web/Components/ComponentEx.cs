using HoiNongDan.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace HoiNongDan.Web.Components
{
    [ViewComponent(Name = "MyComponent")]
    public class ComponentEx :ViewComponent
    {
        private readonly AppDbContext db;

        public ComponentEx(AppDbContext context) => db = context;
        public async Task<IViewComponentResult> InvokeAsync() {
            
            return View("Default", GetSoNguoiHoiVienChuaDuyet());
        }
        private int GetSoNguoiHoiVienChuaDuyet() {
            return  db!.CanBos.Where(it => it.IsHoiVien == true && it.HoiVienDuyet == false).Count();
        }
    }
}
