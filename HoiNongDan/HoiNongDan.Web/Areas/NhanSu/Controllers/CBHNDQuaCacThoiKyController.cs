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
                var model = _context.QuaTrinhCongTacs.Include(it=>it.CanBo).ThenInclude(it=>it.TinhTrang).Include(it=>it.ChucVu).Where(it => it.IsBanChapHanh == true).AsQueryable();
                if (!String.IsNullOrEmpty(search.MaCanBo))
                {
                    model = model.Where(it => it.CanBo.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen))
                {
                    model = model.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
                }
                if (!String.IsNullOrEmpty(search.MaTinhTrang))
                {
                    model = model.Where(it => it.CanBo.MaTinhTrang == search.MaTinhTrang);
                }
                if (!String.IsNullOrEmpty(search.MaPhanHe))
                {
                    model = model.Where(it => it.CanBo.MaPhanHe == search.MaPhanHe);
                }
                model = model.Where(it => it.CanBo.IdDepartment == Guid.Parse("FE9D6794-102A-488B-9F95-55A34D593530"));
                //var data = model.Select(it => new CanBoDetailVM
                //    {
                //        MaCanBo = it.CanBo.MaCanBo,
                //        HoVaTen = it.CanBo.HoVaTen,
                //        TenTinhTrang = it.TinhTrang.TenTinhTrang,
                //        TenPhanHe = it.PhanHe.TenPhanHe,
                //        TenCoSo = it.CoSo.TenCoSo,
                //        TenDonVi = it.Department.Name,
                //        TenChucVu = it.ChucVu.TenChucVu,
                //        TenBacLuong = it.BacLuong.TenBacLuong,
                //        TenNgachLuong = it.MaNgachLuong!,
                //        HeSo = it.HeSoLuong,
                //        IDCanBo = it.IDCanBo,
                //        HinhAnh = it.HinhAnh,
                //        GhiChu = it.GhiChu,
                //        SoDienThoai = it.SoDienThoai,
                //        ChoOHienNay = it.ChoOHienNay,
                //        NoiSinh = it.MaChucVu != null ? it.ChucVu.TenChucVu : it.TinhTrang.TenTinhTrang
                //    }).OrderBy(it => it.HoVaTen).ToList();
                var data = model.Select(it => new CanBoBanChapHanhQuanCacThoiKy
                {
                    ID = it.IDQuaTrinhCongTac,
                    MaCanBo = it.CanBo.MaCanBo,
                    HoVaTen = it.CanBo.HoVaTen,
                    TenChucVu = it.ChucVu.TenChucVu,
                    NhiemKy = it.NhiemKy!,
                    NoiLamViec = it.NoiLamViec!,
                    ChoOHienNay = it.CanBo.ChoOHienNay!,
                    SoDienThoai = it.CanBo.SoDienThoai!,
                    ChucVuHienNay = it.CanBo.MaChucVu != null ? it.CanBo.ChucVu.TenChucVu : it.CanBo.TinhTrang.TenTinhTrang
                }).OrderBy(it => it.HoVaTen).ToList();
                return PartialView(data);
            });
        }
    }
}
