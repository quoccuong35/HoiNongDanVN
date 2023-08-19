using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class DuyetHoiVienController : BaseController
    {
        public DuyetHoiVienController(AppDbContext context) : base(context) { }

        #region index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        public IActionResult _Search(DuyetHoiVienSearchVM search)
        {
            return ExecuteSearch(() => { 
                var data = _context.CanBos.Where(it=>it.HoiVienDuyet != true && it.IsHoiVien == true).AsQueryable();
                if (!String.IsNullOrWhiteSpace(search.MaCanBo))
                {
                    data = data.Where(it => it.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrWhiteSpace(search.HoVaTen))
                {
                    data = data.Where(it => it.HoVaTen.Contains(search.HoVaTen));
                }
                if (search.MaDiaBanHoatDong != null)
                {
                    data = data.Where(it => it.MaDiaBanHoatDong == search.MaDiaBanHoatDong);
                }
                var model = data.Include(it => it.TinhTrang)
                   .Include(it => it.ChucVu)
                   .Include(it => it.NgheNghiep)
                   .Include(it => it.GiaDinhThuocDien)
                   .Include(it => it.DiaBanHoatDong)
                   .Include(it => it.DanToc)
                   .Include(it => it.TonGiao)
                   .Include(it => it.TrinhDoHocVan)
                   .Include(it => it.CoSo).Select(it => new HoiVienDetailVM
                   {
                       IDCanBo = it.IDCanBo,
                       MaCanBo = it.MaCanBo,
                       HoVaTen = it.HoVaTen,
                       TenDiaBanHoatDong = it.DiaBanHoatDong!.TenDiaBanHoatDong,
                       DanToc = it.DanToc!.TenDanToc,
                       TonGiao = it.TonGiao!.TenTonGiao,
                       TrinhDoHocvan = it.TrinhDoHocVan.TenTrinhDoHocVan,
                       TenChucVu = it.ChucVu.TenChucVu,
                       HinhAnh = it.HinhAnh!,
                       VaiTro = it.VaiTro == "01" ? "Chủ hộ" : "Quan hệ chủ hộ: " + it.VaiTroKhac,
                       NgheNghiepHienNay = it.NgheNghiep!.TenNgheNghiep,
                       GiaDinhThuocDien = it.GiaDinhThuocDien!.TenGiaDinhThuocDien,

                   }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Helper
        private void CreateViewBag()
        {
            var diaBanHoatDong = _context.DiaBanHoatDongs.Where(it => it.Actived == true).Select(it => new { MaDiaBanHoatDong = it.Id, Name = it.TenDiaBanHoatDong }).ToList();
            ViewBag.MaDiaBanHoatDong = new SelectList(diaBanHoatDong, "MaDiaBanHoatDong", "Name");
        }
        #endregion Helper
    }
}
