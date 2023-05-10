using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portal.Constant;
using Portal.DataAccess;
using Portal.Extensions;
using Portal.Models;
using System.Data.Entity;

namespace Portal.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class DaoTaoController : BaseController
    {
        public DaoTaoController(AppDbContext context) : base(context) { }
        #region Index
        [PortalAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult _Search(DaoTaoSearchVM search) {
            return ExecuteSearch(() => {
                var data = _context.QuaTrinhDaoTaos.AsQueryable();
                if (!String.IsNullOrEmpty(search.MaLoaiBangCap) && !String.IsNullOrWhiteSpace(search.MaLoaiBangCap))
                {
                    data = data.Where(it => it.MaLoaiBangCap == search.MaLoaiBangCap);
                }
                if (!String.IsNullOrEmpty(search.MaHinhThucDaoTao) && !String.IsNullOrWhiteSpace(search.MaHinhThucDaoTao))
                {
                    data = data.Where(it => it.MaHinhThucDaoTao == search.MaHinhThucDaoTao);
                }
                if (!String.IsNullOrEmpty(search.MaChuyenNganh) && !String.IsNullOrWhiteSpace(search.MaChuyenNganh))
                {
                    data = data.Where(it => it.MaChuyenNganh == search.MaChuyenNganh);
                }
                data = data.Include(it=>it.CanBo).Include(it=>it.ChuyenNganh).Include(it=>it.LoaiBangCap).Include(it=>it.HinhThucDaoTao);
                if (!String.IsNullOrEmpty(search.MaCanBo) && !String.IsNullOrWhiteSpace(search.MaCanBo))
                {
                    data = data.Where(it => it.CanBo.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen) && !String.IsNullOrWhiteSpace(search.HoVaTen))
                {
                    data = data.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
                }
                var model = data.Select(it => new DaoTaoDetailVM {
                    IDQuaTrinhDaoTao = it.IDQuaTrinhDaoTao,
                    TenChuyenNganh = it.ChuyenNganh.TenChuyenNganh,
                    TenHinhThucDaoTao = it.HinhThucDaoTao.TenHinhThucDaoTao,
                    TenLoaiBangCap = it.LoaiBangCap.TenLoaiBangCap,
                    CoSoDaoTao = it.CoSoDaoTao,
                    NgayTotNghiep = it.NgayTotNghiep,
                    QuocGia =it.QuocGia,
                    GhiChu = it.GhiChu,
                    LuanAnTN = it.LuanAnTN,
                    FileDinhKem = it.FileDinhKem

                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Helper
        private void CreateViewBag(String? MaChuyenNganh = null,String? MaLoaiBangCap = null, String? MaHinhThucDaoTao = null) {
            var MenuList = _context.ChuyenNganhs.Select(it => new { MaChuyenNganh = it.MaChuyenNganh, TenChuyenNganh = it.TenChuyenNganh }).ToList();
            ViewBag.MaChuyenNganh = new SelectList(MenuList, "MaChuyenNganh", "TenChuyenNganh", MaChuyenNganh);

            var MenuList1 = _context.HinhThucDaoTaos.Select(it => new { MaHinhThucDaoTao = it.MaHinhThucDaoTao, TenHinhThucDaoTao = it.TenHinhThucDaoTao }).ToList();
            ViewBag.MaHinhThucDaoTao = new SelectList(MenuList1, "MaHinhThucDaoTao", "TenHinhThucDaoTao", MaHinhThucDaoTao);

            var MenuList2 = _context.LoaiBangCaps.Select(it => new { MaLoaiBangCap = it.MaLoaiBangCap, TenLoaiBangCap = it.TenLoaiBangCap }).ToList();
            ViewBag.MaLoaiBangCap = new SelectList(MenuList2, "MaLoaiBangCap", "TenLoaiBangCap", MaLoaiBangCap);
        }
        #endregion Helper
    }
}
