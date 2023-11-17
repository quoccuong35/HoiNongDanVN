using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Authorize]
    [Area(ConstArea.NhanSu)]
    public class CBHNDQuaCacThoiKyController : BaseController
    {
        public CBHNDQuaCacThoiKyController(AppDbContext context):base(context) { }
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(CanBoSearchVM search)
        {
            return ExecuteSearch(() => {
                var model = _context.CanBos.Where(it => it.IsCanBo == true).AsQueryable();
                if (!String.IsNullOrEmpty(search.MaCanBo))
                {
                    model = model.Where(it => it.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen))
                {
                    model = model.Where(it => it.HoVaTen.Contains(search.HoVaTen));
                }
                if (!String.IsNullOrEmpty(search.MaTinhTrang))
                {
                    model = model.Where(it => it.MaTinhTrang == search.MaTinhTrang);
                }
                if (!String.IsNullOrEmpty(search.MaPhanHe))
                {
                    model = model.Where(it => it.MaPhanHe == search.MaPhanHe);
                }
                model = model.Where(it => it.IdDepartment == Guid.Parse("FE9D6794-102A-488B-9F95-55A34D593530"));
                var data = model.Include(it => it.TinhTrang)
                    .Include(it => it.Department)
                    .Include(it => it.BacLuong)
                    .Include(it => it.ChucVu)
                    .Include(it => it.TinhTrang)
                    .Include(it => it.CoSo).Select(it => new CanBoDetailVM
                    {
                        MaCanBo = it.MaCanBo,
                        HoVaTen = it.HoVaTen,
                        TenTinhTrang = it.TinhTrang.TenTinhTrang,
                        TenPhanHe = it.PhanHe.TenPhanHe,
                        TenCoSo = it.CoSo.TenCoSo,
                        TenDonVi = it.Department.Name,
                        TenChucVu = it.ChucVu.TenChucVu,
                        TenBacLuong = it.BacLuong.TenBacLuong,
                        TenNgachLuong = it.MaNgachLuong!,
                        HeSo = it.HeSoLuong,
                        IDCanBo = it.IDCanBo,
                        HinhAnh = it.HinhAnh,
                        GhiChu = it.GhiChu,
                        SoDienThoai = it.SoDienThoai,
                        ChoOHienNay = it.ChoOHienNay,
                        NoiSinh = it.MaChucVu != null ? it.ChucVu.TenChucVu : it.TinhTrang.TenTinhTrang
                    }).OrderBy(it => it.HoVaTen).ToList();
                return PartialView(data);
            });
        }
    }
}
