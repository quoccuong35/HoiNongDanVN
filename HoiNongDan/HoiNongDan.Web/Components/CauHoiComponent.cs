using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Components
{
    [ViewComponent(Name = "CauHoi")]
    public class CauHoiComponent : ViewComponent
    {
        private readonly AppDbContext db;

        public CauHoiComponent(AppDbContext context) => db = context;
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View("Default", GetSoNguoiHoiVienChuaDuyet());
        }
        private List<HoiVienHoiDapDetail> GetSoNguoiHoiVienChuaDuyet()
        {
            var accountID = db.Accounts.SingleOrDefault(it => it.UserName!.Equals(User!.Identity.Name)).AccountId;
            var phamVis = Function.GetPhamVi(accountID, db);
            var model = db!.HoiVienHoiDaps
                .Join(
                    db.CanBos.Where(it=>phamVis.Contains(it.MaDiaBanHoatDong!.Value)) ,
                    hvhd=>hvhd.IDHoivien,
                    hv=>hv.IDCanBo,
                    (hvhd, hv)=>new { hvhd, hv }
                )
            .Where(it => it.hvhd.TraLoi != true && it.hvhd.TrangThai == "01").OrderBy(it => it.hvhd.Ngay).Select(it => new HoiVienHoiDapDetail
            {
                ID = it.hvhd.ID,
                HoVaTen = it.hv.HoVaTen,
                NoiDung = it.hvhd.NoiDung,
                TraLoi = it.hvhd.TraLoi,
                Ngay = it.hvhd.Ngay,
                IdParent = it.hvhd.IdParent
            }).ToList();
            return model;
        }
    }
}
