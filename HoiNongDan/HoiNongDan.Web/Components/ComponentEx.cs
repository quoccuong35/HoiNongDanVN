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
            var accountID = db.Accounts.SingleOrDefault(it => it.UserName.Equals(User!.Identity.Name)).AccountId;
            var phamVis = db.PhamVis.Where(it => it.AccountId == accountID).Select(it => it.MaDiabanHoatDong).ToList();
            return  db!.CanBos.Where(it => it.IsHoiVien == true && it.HoiVienDuyet == false && phamVis.Contains(it.MaDiaBanHoatDong!.Value)).Count();
        }
    }
}
