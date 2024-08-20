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
            
            var accountID = db.Accounts.SingleOrDefault(it => it.UserName!.Equals(User!.Identity.Name)).AccountId;
            return db.HoiVienLichSuDuyets.Where(it => it.AccountID == accountID && it.TrangThaiDuyet == false).Count();
            //var phamVis = db.PhamVis.Where(it => it.AccountId == accountID).Select(it => it.MaDiabanHoatDong).ToList();
            //return  db!.CanBos.Join(db!.PhamVis.Where(it => it.AccountId == accountID),
            //        hv => hv.MaDiaBanHoatDong,
            //        pv => pv.MaDiabanHoatDong,
            //        (hv, pv) => new { hv }
            //        ).Where(it => it.hv.IsHoiVien == true && it.hv.HoiVienDuyet == false && it.hv.TuChoi != true && phamVis.Contains(it.hv.MaDiaBanHoatDong!.Value)).Count();
        }
    }
}
