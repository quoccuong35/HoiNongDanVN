using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HoiNongDan.Models.ViewModels;
using HoiNongDan.Constant;
using HoiNongDan.Models.Entitys.NhanSu;
using MessagePack.Formatters;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class DeXuatKhenThuongController : BaseController
    {
        public DeXuatKhenThuongController(AppDbContext context) : base(context) { }

        #region Index
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Index()
        {
            HoiVienDeXuatKhenThuongSearchVM model = new HoiVienDeXuatKhenThuongSearchVM();
            CreateViewbag();
            return View(model);
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(HoiVienDeXuatKhenThuongSearchVM sr)
        {
            return ExecuteSearch(() =>
            {

                var data = (from kt in _context.QuaTrinhKhenThuongs
                            join hv in _context.CanBos on kt.IDCanBo equals hv.IDCanBo
                            join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                            where kt.Loai == "01" && hv.isRoiHoi != true &&
                            kt.IsHoiVien == true
                            && pv.AccountId == AccountId()
                            select kt).Include(it => it.CanBo)
                    .Include(it => it.CanBo.DanToc)
                    .Include(it => it.CanBo.TonGiao)
                    .Include(it => it.CanBo.TrinhDoHocVan)
                    .Include(it => it.CanBo.TrinhDoChuyenMon)
                    .Include(it => it.CanBo.TrinhDoChinhTri)
                    .Include(it => it.CanBo.ChiHoi)
                    .Include(it => it.CanBo.ChucVu)
                    .Include(it => it.CanBo.DiaBanHoatDong).AsQueryable(); ;

                //var data = _context.QuaTrinhKhenThuongs.Where(it => it.IsHoiVien == true && it.Loai =="01").Include(it => it.DanhHieuKhenThuong)
                //.Include(it => it.CanBo)
                //    .Include(it=>it.CanBo.DanToc)
                //    .Include(it => it.CanBo.TonGiao)
                //    .Include(it => it.CanBo.TrinhDoHocVan)
                //    .Include(it => it.CanBo.TrinhDoChuyenMon)
                //    .Include(it => it.CanBo.TrinhDoChinhTri)
                //    .Include(it => it.CanBo.ChiHoi)
                //    .Include(it => it.CanBo.ChucVu)
                //    .Include(it => it.CanBo.DiaBanHoatDong).AsQueryable();
                if (sr.Nam != null)
                {
                    data = data.Where(it => it.Nam == sr.Nam);
                }
                if (sr.Quy != null)
                {
                    data = data.Where(it => it.Quy == sr.Quy);
                }
                if (sr.MaDanhHieuKhenThuong != null)
                {
                    data = data.Where(it => it.MaDanhHieuKhenThuong == sr.MaDanhHieuKhenThuong);
                }
                if (!String.IsNullOrWhiteSpace(sr.SoCCCD))
                {
                    data = data.Where(it => it.CanBo.SoCCCD == sr.SoCCCD);
                }
                if (sr.MaDiaBanHoiVien != null)
                {
                    data = data.Where(it => it.CanBo.MaDiaBanHoatDong == sr.MaDiaBanHoiVien);
                }
                if (!String.IsNullOrWhiteSpace(sr.MaQuanHuyen))
                {
                    data = data.Where(it => it.CanBo.DiaBanHoatDong!.MaQuanHuyen == sr.MaQuanHuyen);
                }
                if (!String.IsNullOrWhiteSpace(sr.HoVaTen))
                {
                    data = data.Where(it => it.CanBo.HoVaTen.Contains(sr.HoVaTen));
                }
                var model = data.Select(it => new HoiVienDeXuatKhenThuongDetailVM
                {
                    IDQuaTrinhKhenThuong = it.IDQuaTrinhKhenThuong,
                    HoVaTen = it.CanBo.HoVaTen,
                    Nam = (int)it.CanBo.GioiTinh == 1 ? "Nam" : null,
                    Nu = (int)it.CanBo.GioiTinh == 0 ? "Nữ" : null,
                    SoCCCD = it.CanBo.SoCCCD,
                    NgayCap = it.CanBo.NgayCapCCCD,
                    DiaChiThuongTru = it.CanBo.ChoOHienNay,
                    DanToc = it.CanBo.DanToc!.TenDanToc,
                    TonGiao = it.CanBo.TonGiao!.TenTonGiao,
                    HocVan = it.CanBo.TrinhDoHocVan.TenTrinhDoHocVan,
                    ChuyenMon = it.CanBo.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                    ChinhTri = it.CanBo.TrinhDoChinhTri!.TenTrinhDoChinhTri!,
                    NgayVaoHoi = it.CanBo.NgayVaoHoi != null? it.CanBo.NgayVaoHoi.Value.ToString("dd/MM/yyyy") + " " + it.CanBo.ChucVu!.TenChucVu: it.CanBo.ChucVu!.TenChucVu,
                    DaHocLopDang = "",
                    DangHocLopDang = "",
                    ChuaHocLopDang = "",
                    DiaBanHND = it.CanBo.DiaBanHoatDong!.TenDiaBanHoatDong,
                    ChiHoiNongDan = it.CanBo.ChiHoi!.TenChiHoi,
                    NamDX = it.Nam
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create()
        {
            HoiVienDeXuatKhenThuongVM model = new HoiVienDeXuatKhenThuongVM();
            HoiVienInfo hoiVien = new HoiVienInfo();
            hoiVien.Edit = true;
            model.HoiVien = hoiVien;
            CreateViewBag1();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HoiNongDanAuthorization]
        public IActionResult Create(HoiVienDeXuatKhenThuongVM model)
        {
            CheckError(model);
            return ExecuteContainer(() =>
            {
                QuaTrinhKhenThuong add = new QuaTrinhKhenThuong
                {
                    IDQuaTrinhKhenThuong = Guid.NewGuid(),
                    IDCanBo = model.HoiVien.IdCanbo.Value,
                    Quy = model.Quy,
                    Nam = model.Nam,
                    MaDanhHieuKhenThuong = model.MaDanhHieuKhenThuong,
                    MaHinhThucKhenThuong = model.MaHinhThucKhenThuong,
                    NoiDung = model.NoiDung,
                    GhiChu = model.GhiChu,
                    IsHoiVien = true,
                    Loai = "01",
                    CreatedAccountId = AccountId(),
                    CreatedTime = DateTime.Now,
                };
                _context.QuaTrinhKhenThuongs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.DeXuatKhenThuong.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit 
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var khenThuong = _context.QuaTrinhKhenThuongs.SingleOrDefault(it => it.IDQuaTrinhKhenThuong == id && it.Loai == "01" && it.IsHoiVien == true);
            if (khenThuong == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            HoiVienDeXuatKhenThuongVM model = new HoiVienDeXuatKhenThuongVM
            {
                ID = id,
                Nam = khenThuong.Nam!.Value,
                Quy = khenThuong.Quy!.Value,
                MaHinhThucKhenThuong = khenThuong.MaHinhThucKhenThuong!,
                MaDanhHieuKhenThuong = khenThuong.MaDanhHieuKhenThuong,
                NoiDung = khenThuong.NoiDung,
                Loai = khenThuong.Loai,
                HoiVien = Function.GetThongTinHoiVien(khenThuong.IDCanBo, _context),
                GhiChu = khenThuong.GhiChu,
            };
            CreateViewBag1(khenThuong.MaHinhThucKhenThuong, khenThuong.MaDanhHieuKhenThuong);
            return View(model);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public IActionResult Edit(HoiVienDeXuatKhenThuongVM model)
        {
            CheckError(model);
            return ExecuteContainer(() =>
            {
                var edit = _context.QuaTrinhKhenThuongs.SingleOrDefault(it => it.IDQuaTrinhKhenThuong == model.ID.Value);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.DeXuatKhenThuong.ToLower())
                    });
                }
                edit.Nam = model.Nam;
                edit.Quy = model.Quy;
                edit.NoiDung = model.NoiDung;
                edit.GhiChu = model.GhiChu;
                edit.MaDanhHieuKhenThuong = model.MaDanhHieuKhenThuong;
                edit.MaHinhThucKhenThuong = model.MaHinhThucKhenThuong;
                edit.Loai = model.Loai;
                edit.LastModifiedAccountId = AccountId();
                edit.LastModifiedTime = DateTime.Now;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.DeXuatKhenThuong.ToLower())
                });

            });
        }
        #endregion Edit
        #region Del
        [HttpDelete]
        [ValidateAntiForgeryToken]
        [HoiNongDanAuthorization]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.QuaTrinhKhenThuongs.SingleOrDefault(it => it.IDQuaTrinhKhenThuong == id && it.Loai == "01");
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.DeXuatKhenThuong.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.DeXuatKhenThuong.ToLower())
                    });
                }
            });
        }
        #endregion Del
        #region Helper
        private void CreateViewbag()
        {
            FnViewBag viewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoiVien = viewBag.DiaBanHoiVien(acID: AccountId());


            ViewBag.MaDanhHieuKhenThuong = viewBag.DanhHieuKhenThuong();

            ViewBag.MaQuanHuyen = viewBag.QuanHuyen(idAc: AccountId());
        }
        private void CreateViewBag1(String? MaHinhThucKhenThuong = null, String? MaDanhHieuKhenThuong = null)
        {
            FnViewBag viewBag = new FnViewBag(_context);

            ViewBag.MaHinhThucKhenThuong = viewBag.HinhThucKhenThuong(MaHinhThucKhenThuong);

            ViewBag.MaDanhHieuKhenThuong = viewBag.DanhHieuKhenThuong(MaDanhHieuKhenThuong); ;

        }
        private void CheckError(HoiVienDeXuatKhenThuongVM model)
        {
            if (model.HoiVien == null || model.HoiVien.IdCanbo == null)
            {
                ModelState.AddModelError("HoiVien", "Chưa chọn hội viên");
            }
            if (model.Loai == "02")
            {
                if (String.IsNullOrWhiteSpace(model.SoQuyetDinh))
                {
                    ModelState.AddModelError("SoQuyetDinh", "Chưa nhập số quyết định");
                }
                if (model.NgayQuyetDinh == null)
                {
                    ModelState.AddModelError("NgayQuyetDinh", "Chưa nhập ngày quyết định");
                }
            }
        }
        #endregion Helper
    }
}
