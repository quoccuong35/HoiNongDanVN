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
            CreateViewBagSearch();
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(string? MaQuanHuyen, Guid? MaDiaBanHoatDong, DateTime? TuNgay, DateTime? DenNgay)
        {
            return ExecuteSearch(() => {
                var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoatDong, TuNgay: TuNgay, DenNgay: DenNgay);
                return PartialView(model);
            });
        }
        #endregion Index
        #region Update duyệt hội viên
        [HoiNongDanAuthorization]
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
        public IActionResult Edit(List<Guid> lid) {
            return ExecuteContainer(() => {
                const TransactionScopeOption opt = new TransactionScopeOption();

                TimeSpan span = new TimeSpan(0, 0, 30, 30);
                using (TransactionScope ts = new TransactionScope(opt, span)) {
                    List<string> error = new List<string>();
                    DateTime dateTime = DateTime.Now;
                    foreach (Guid item in lid) 
                    {
                        var edit = _context.CanBos.SingleOrDefault(it => it.IDCanBo == item);
                        if (edit != null)
                        {
                            edit.HoiVienDuyet = true;
                            edit.NguoiDuyet = AccountId();
                            edit.NgayDuyet = dateTime;
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
        #region Tu chối duyệt hội viên
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HoiNongDanAuthorization]
        public IActionResult TuChoi(List<Guid> lid, string lyDo) {
            const TransactionScopeOption opt = new TransactionScopeOption();

            TimeSpan span = new TimeSpan(0, 0, 30, 30);
            using (TransactionScope ts = new TransactionScope(opt, span))
            {
                List<string> error = new List<string>();
                DateTime dateTime = DateTime.Now;
                foreach (Guid item in lid)
                {
                    var edit = _context.CanBos.SingleOrDefault(it => it.IDCanBo == item);
                    if (edit != null)
                    {
                        edit.TuChoi = true;
                        edit.AccountIdTuChoi = AccountId();
                        edit.NgayTuChoi = dateTime;
                        edit.LyDoTuChoi = lyDo;
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
                        Data = "Từ chối thành công " + lid.Count().ToString() + " hội viên"
                    });
                }
            }
        }
        #endregion Tu chối duyệt hội viên
        #region Helper

        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }

        private List<BCHVPhatTrienMoiVM> LoadData(string? MaQuanHuyen, Guid? MaDiaBanHoatDong, DateTime? TuNgay, DateTime? DenNgay)
        {
            var data = _context.CanBos
                .Include(it => it.DanToc).Include(it => it.NgheNghiep)
                .Include(it => it.TonGiao).Include(it => it.TrinhDoHocVan)
                .Include(it => it.TrinhDoChuyenMon).Include(it => it.TrinhDoChinhTri)
                .Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen)
                .Join(_context.PhamVis.Where(it => it.AccountId == AccountId()),
                    hv => hv.MaDiaBanHoatDong,
                    pv => pv.MaDiabanHoatDong,
                    (hv, pv) => new { hv }
                    ).Where(
                        it => it.hv.IsHoiVien == true
                        && it.hv.HoiVienDuyet == false
                        && it.hv.TuChoi == false
                        && it.hv.isRoiHoi == false
                    ).AsQueryable();

            if (TuNgay != null)
            {
                data = data.Where(it => it.hv.NgayDangKy >= TuNgay.Value.Date);
            }
            if (DenNgay != null)
            {
                data = data.Where(it => it.hv.NgayDangKy <= DenNgay.Value.Date);
            }
            if (!String.IsNullOrWhiteSpace(MaQuanHuyen))
            {
                data = data.Where(it => it.hv.DiaBanHoatDong!.MaQuanHuyen == MaQuanHuyen);
            }
            if (MaDiaBanHoatDong != null)
            {
                data = data.Where(it => it.hv.MaDiaBanHoatDong == MaDiaBanHoatDong);
            }
            var model = data.Select(it => new BCHVPhatTrienMoiVM
            {
                ID = it.hv.IDCanBo,
                HoVaTen = it.hv.HoVaTen,
                Nam = (int)it.hv.GioiTinh == 1 ? it.hv.NgaySinh : "",
                Nu = (int)it.hv.GioiTinh == 0 ? it.hv.NgaySinh : "",
                SoCCCD = it.hv.SoCCCD,
                NgayCapSoCCCD = it.hv.NgayCapCCCD,
                HoKhauThuongTru = it.hv.HoKhauThuongTru,
                NoiOHiennay = it.hv.ChoOHienNay,
                SoDienThoai = it.hv.SoDienThoai,
                DangVien = "",
                DanToc = it.hv.DanToc!.TenDanToc,
                TonGiao = it.hv.TonGiao!.TenTonGiao,
                TrinhDoHocVan = it.hv.TrinhDoHocVan.TenTrinhDoHocVan,
                TrinhDoChuyenMon = it.hv.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                ChinhTri = it.hv.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                NgayThangVaoHoi = it.hv.NgayVaoHoi,
                NgheNghiep = it.hv.NgheNghiep!.TenNgheNghiep,
                DiaBanDanCu = it.hv.HoiVienDanCu == true ? "X" : "",
                NganhNghe = it.hv.HoiVienNganhNghe == true ? "X" : "",
                //SoThe = it.hv.MaCanBo,
                //NgayCapThe = ""
            }).ToList().Select((it, index) => new BCHVPhatTrienMoiVM
            {
                ID = it.ID,
                STT = index + 1,
                HoVaTen = it.HoVaTen,
                Nam = it.Nam,
                Nu = it.Nu,
                SoCCCD = it.SoCCCD,
                NgayCapSoCCCD = it.NgayCapSoCCCD,
                HoKhauThuongTru = it.HoKhauThuongTru,
                NoiOHiennay = it.NoiOHiennay,
                SoDienThoai = it.SoDienThoai,
                DangVien = "",
                DanToc = it.DanToc,
                TonGiao = it.TonGiao,
                TrinhDoHocVan = it.TrinhDoHocVan,
                TrinhDoChuyenMon = it.TrinhDoChuyenMon,
                ChinhTri = it.ChinhTri,
                NgayThangVaoHoi = it.NgayThangVaoHoi,
                NgheNghiep = it.NgheNghiep,
                DiaBanDanCu = it.DiaBanDanCu,
                NganhNghe = it.NganhNghe,
                SoThe = it.SoThe,
                NgayCapThe = ""

            }).ToList();
            return model;

        }
        #endregion Helper
    }
}
