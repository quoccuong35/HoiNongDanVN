using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class CBBanChapHanhController : BaseController
    {
        public CBBanChapHanhController(AppDbContext context):base(context) { }
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult _Search(CanBoSearchVM search)
        {
            return ExecuteSearch(() => {
                var model = _context.CanBos.Where(it => it.IsBanChapHanh == true).AsQueryable();
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
                if (search.IdCoSo != null)
                {
                    model = model.Where(it => it.IdCoSo == search.IdCoSo);
                }
                if (search.IdDepartment != null)
                {
                    model = model.Where(it => it.IdDepartment == search.IdDepartment);
                }
                if (search.MaChucVu != null)
                {
                    model = model.Where(it => it.MaChucVu == search.MaChucVu);
                }
                if (search.Actived != null)
                {
                    model = model.Where(it => it.Actived == search.Actived);
                }
                model = model.Where(it => it.IsBanChapHanh == true && it.IsCanBo == true);
                var data = model.Include(it => it.TinhTrang)
                    .Include(it => it.DanToc)
                    .Include(it => it.TonGiao)
                    .Include(it => it.TrinhDoChinhTri)
                    .Include(it => it.TrinhDoChinhTri)
                    .Include(it => it.CoSo).Select(it => new CanBoDetailVM
                    {
                        HoVaTen = it.HoVaTen,
                        TenChucVu = it.ChucVu!.TenChucVu,
                        GioiTinh = (GioiTinh)it.GioiTinh,
                        MaDanToc = it.DanToc!.TenDanToc,
                        MaTonGiao = it.TonGiao!.TenTonGiao,
                        ChoOHienNay = it.ChoOHienNay,
                        ChuyenNganh = it.ChuyenNganh,
                        MaTrinhDoChinhTri = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                        SoDienThoai = it.SoDienThoai,
                        NgayvaoDangDuBi = it.NgayvaoDangDuBi,
                        NgayVaoDangChinhThuc = it.NgayVaoDangChinhThuc,
                        SoBHXH = it.SoDienThoai,

                    }).ToList();
                return PartialView(data);
            });
        }
    }
}
