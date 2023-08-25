using HoiNongDan.DataAccess;
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
            var model = db!.HoiVienHoiDaps.Include(it => it.HoiVien).Where(it => it.TraLoi != true && it.TrangThai == "01").OrderBy(it => it.Ngay).Select(it => new HoiVienHoiDapDetail
            {
                ID = it.ID,
                HoVaTen = it.HoiVien.HoVaTen,
                NoiDung = it.NoiDung,
                TraLoi = it.TraLoi,
                Ngay = it.Ngay,
                IdParent = it.IdParent
            }).ToList();
            return model;
        }
    }
}
