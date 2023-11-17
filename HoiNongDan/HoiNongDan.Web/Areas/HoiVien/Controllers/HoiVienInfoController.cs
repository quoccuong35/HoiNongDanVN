using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
   
    public class HoiVienInfoController : Controller
    {
        protected AppDbContext _context;
        public IActionResult Index()
        {
            return View();
        }
        public HoiVienInfoController(AppDbContext context) {
            _context = context;
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
        #region View
        [HttpGet]
        public IActionResult XemThongTin()
        {
            CreateViewBagSearch();
            ViewBag.HTTP = "HttpGet";
            return View(new HoiVienDetailVM());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XemThongTin(String MaHoiVien, Guid MaDiaBanHoatDong)
        {
            HoiVienDetailVM hoivien = new HoiVienDetailVM();
            try
            {
                hoivien = _context.CanBos.Where(it => it.MaCanBo == MaHoiVien && it.HoiVienDuyet == true && it.Actived == true && it.IsHoiVien == true && it.MaDiaBanHoatDong == MaDiaBanHoatDong).Include(it => it.TinhTrang)
                   .Include(it => it.DiaBanHoatDong)
                   .Include(it => it.DanToc)
                   .Include(it => it.TonGiao)
                   .Include(it => it.TrinhDoHocVan)
                   .Include(it => it.TrinhDoChuyenMon)
                   .Include(it => it.TrinhDoChinhTri)
                   .Include(it => it.CoSo).Select(it => new HoiVienDetailVM
                   {
                       IDCanBo = it.IDCanBo,
                       MaCanBo = it.MaCanBo,
                       HoVaTen = it.HoVaTen,
                       NgaySinh = it.NgaySinh,
                       GioiTinh = (GioiTinh)it.GioiTinh,
                       SoCCCD = it.SoCCCD!,
                       NgayCapCCCD = it.NgayCapCCCD!,
                       HoKhauThuongTru = it.HoKhauThuongTru,
                       ChoOHienNay = it.ChoOHienNay!,
                       SoDienThoai = it.SoDienThoai,
                       NgayVaoDangChinhThuc = it.NgayVaoDangChinhThuc,
                       TenDiaBanHoatDong = it.DiaBanHoatDong!.TenDiaBanHoatDong,
                       DanToc = it.DanToc!.TenDanToc,
                       TonGiao = it.TonGiao!.TenTonGiao,
                       TrinhDoHocvan = it.TrinhDoHocVan.TenTrinhDoHocVan,
                       MaTrinhDoChuyenMon = it.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                       MaTrinhDoChinhTri = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                       NgayVaoHoi = it.NgayVaoHoi,
                       NgayThamGiaCapUyDang = it.NgayThamGiaCapUyDang,
                       NgayThamGiaHDND = it.NgayThamGiaHDND,
                       HoNgheo = it.HoNgheo == null ? false : it.HoNgheo.Value,
                       CanNgheo = it.CanNgheo == null ? false : it.CanNgheo.Value,
                       GiaDinhChinhSach = it.GiaDinhChinhSach == null ? false : it.GiaDinhChinhSach.Value,
                       GiaDinhThuocDienKhac = it.GiaDinhThuocDienKhac,
                       HinhAnh = it.HinhAnh!,
                       VaiTro = it.VaiTro == "01" ? "Chủ hộ" : "Quan hệ chủ hộ",
                       VaiTroKhac = it.VaiTroKhac,
                       NgheNghiepHienNay = it.NgheNghiep!.TenNgheNghiep,
                       Loai_DV_SX_ChN = it.Loai_DV_SX_ChN,
                       DienTich_QuyMo = it.DienTich_QuyMo,
                       LoaiHoiVien = it.LoaiHoiVien,
                       DangVien = it.DangVien == null ? false : it.DangVien.Value,
                       KKAnToanThucPham = it.KKAnToanThucPham,
                       DKMauNguoiNongDanMoi = it.DKMauNguoiNongDanMoi,
                       HoiVienUuTu = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "14").Select(it => it.GhiChu).ToList()),
                       NDSXKDG = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "15").Select(it => it.GhiChu).ToList()),
                       NDTieuBieu = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "16").Select(it => it.GhiChu).ToList()),
                       NDVietnamXS = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "22").Select(it => it.GhiChu).ToList()),
                       ThamGia_SH_DoanThe_HoiDoanKhac = it.ThamGia_SH_DoanThe_HoiDoanKhac,
                       ThamGia_CLB_DN_MH_HTX_THT = it.ThamGia_CLB_DN_MH_HTX_THT,
                       ThamGia_THNN_CHNN = it.ThamGia_THNN_CHNN

                   }).First();
                var lisCauHoi = _context.HoiVienHoiDaps.Include(it => it.HoiVien).Where(it => it.IDHoivien == hoivien.IDCanBo && it.TraLoi != true).OrderBy(it => it.Ngay).Select(it => new HoiVienHoiDapDetail
                {
                    ID = it.ID,
                    HoVaTen = it.HoiVien.HoVaTen,
                    NoiDung = it.NoiDung,
                    TraLoi = it.TraLoi,
                    Ngay = it.Ngay,
                    IdParent = it.IdParent
                }).ToList();
                if (lisCauHoi.Count() > 0)
                {
                    hoivien.HoiDaps.AddRange(lisCauHoi);//
                                                        // add cau tra loi
                    var listraloi = _context.HoiVienHoiDaps.Include(it => it.Account).Where(it => it.IdParent != null && lisCauHoi.Select(it => it.ID).ToList().Contains(it.IdParent.Value)).Select(it => new HoiVienHoiDapDetail
                    {
                        ID = it.ID,
                        HoVaTen = it.Account!.FullName,
                        NoiDung = it.NoiDung,
                        TraLoi = it.TraLoi,
                        Ngay = it.Ngay,
                        IdParent = it.IdParent
                    }).ToList();
                    hoivien.HoiDaps.AddRange(listraloi);
                }
            }
            catch
            {

            }
            ViewBag.HTTP = "HttpPost";
            CreateViewBagSearch();
            return View(hoivien);
        }
#endregion View
        private void CreateViewBagSearch()
        {
            var diaBanHoatDong = _context.DiaBanHoatDongs.Where(it => it.Actived == true).Select(it => new { MaDiaBanHoatDong = it.Id, Name = it.TenDiaBanHoatDong }).Distinct().ToList();
            ViewBag.MaDiaBanHoatDong = new SelectList(diaBanHoatDong, "MaDiaBanHoatDong", "Name");

        }
    }
}
