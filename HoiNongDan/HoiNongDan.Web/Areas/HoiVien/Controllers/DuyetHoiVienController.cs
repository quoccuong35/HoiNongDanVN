using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

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

                   }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Update duyệt hội viên
        public IActionResult View(Guid id)
        {
            var hoivien = _context.CanBos.Where(it => it.IDCanBo == id).Include(it => it.TinhTrang)
                    .Include(it => it.DiaBanHoatDong)
                    .Include(it => it.DanToc)
                    .Include(it => it.TonGiao)
                    .Include(it => it.TrinhDoHocVan)
                    .Include(it => it.TrinhDoChuyenMon)
                    .Include(it => it.TrinhDoChinhTri)
                    .Include(it => it.CoSo).Select(it => new HoiVienDetailVM
                    {
                        MaCanBo = it.MaCanBo,
                        IDCanBo = it.IDCanBo,
                        HoVaTen = it.HoVaTen,
                        NgaySinh = it.NgaySinh,
                        GioiTinh = (GioiTinh)it.GioiTinh,
                        SoCCCD = it.SoCCCD!,
                        HoKhauThuongTru = it.HoKhauThuongTru,
                        ChoOHienNay = it.ChoOHienNay!,
                        SoDienThoai = it.SoDienThoai,
                        NgayvaoDangDuBi = it.NgayvaoDangDuBi,
                        NgayVaoDangChinhThuc = it.NgayVaoDangChinhThuc,
                        DanToc = it.DanToc!.TenDanToc,
                        TonGiao = it.TonGiao!.TenTonGiao,
                        TrinhDoHocvan = it.TrinhDoHocVan.TenTrinhDoHocVan,
                        TrinhDoChuyenMon = it.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                        TrinhDoChinhChi = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                        TenDiaBanHoatDong = it.DiaBanHoatDong.TenDiaBanHoatDong,
                        NgayVaoHoi = it.NgayVaoHoi,
                        NgayThamGiaCapUyDang = it.NgayThamGiaCapUyDang,
                        NgayThamGiaHDND = it.NgayThamGiaHDND,
                        VaiTro = it.VaiTro,
                        VaiTroKhac = it.VaiTroKhac,
                        GiaDinhThuocDienKhac = it.GiaDinhThuocDienKhac,
                        NgheNghiepHienNay = it.MaNgheNghiep,
                        Loai_DV_SX_ChN = it.Loai_DV_SX_ChN,
                        DienTich_QuyMo = it.DienTich_QuyMo,
                        ThamGia_SH_DoanThe_HoiDoanKhac = it.ThamGia_SH_DoanThe_HoiDoanKhac,
                        ThamGia_CLB_DN_MH_HTX_THT = it.ThamGia_CLB_DN_MH_HTX_THT,
                        ThamGia_THNN_CHNN = it.ThamGia_THNN_CHNN

                    }).First();


            return View(hoivien);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Edit(List<Guid> lid) {
            return ExecuteContainer(() => {
                const TransactionScopeOption opt = new TransactionScopeOption();

                TimeSpan span = new TimeSpan(0, 0, 30, 30);
                using (TransactionScope ts = new TransactionScope(opt, span)) {
                    List<string> error = new List<string>();
                    foreach (Guid item in lid) 
                    {
                        var edit = _context.CanBos.SingleOrDefault(it => it.IDCanBo == item);
                        if (edit != null)
                        {
                            edit.HoiVienDuyet = true;
                            edit.NguoiDuyet = AccountId();
                            edit.NgayDuyet = DateTime.Now;
                        }
                        else
                        {
                            error.Add("Không tìm thấy cán bộ có mã" + item);
                        }
                    }
                    if (error.Count > 0)
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = String.Join(", ", error)
                        });
                    }
                    else
                    {
                        _context.SaveChanges();
                        ts.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = "Duyệt thành công " + lid.Count().ToString() + " hội viên"
                        });
                    }
                }
            });
        }
        #endregion Update duyệt hội viên

        #region Helper
        private void CreateViewBag()
        {
            var diaBanHoatDong = _context.DiaBanHoatDongs.Where(it => it.Actived == true).Select(it => new { MaDiaBanHoatDong = it.Id, Name = it.TenDiaBanHoatDong }).ToList();
            ViewBag.MaDiaBanHoatDong = new SelectList(diaBanHoatDong, "MaDiaBanHoatDong", "Name");
        }
        #endregion Helper
    }
}
