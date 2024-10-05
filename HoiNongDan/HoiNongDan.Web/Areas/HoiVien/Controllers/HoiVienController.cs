using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Hosting;
using NuGet.Packaging.Signing;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.Entitys.MasterData;
using HoiNongDan.Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Mapping;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text;
using System.Transactions;
using NuGet.Packaging;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using HoiNongDan.Models.Entitys.NhanSu;
using AspNetCore.Reporting;
using System.Xml.Linq;
using System.Diagnostics;
using HoiNongDan.Models.ViewModels.Masterdata;
using HoiNongDan.Web.Areas.NhanSu.Models;
using System.IO;
using NuGet.Protocol.Plugins;
using HoiNongDan.Models.ViewModels.HoiVien;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HoiVienController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        private string[] DateFomat;
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 5;
        private const string url_excel = @"upload\filemau\HoiVienNew.xlsx";
        private string img_url = "";
        public HoiVienController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config, IHttpContextAccessor httpContext) : base(context)
        {
            _hostEnvironment = hostEnvironment;
            DateFomat = config.GetSection("SiteSettings:DateFormat").Value.ToString().Split(',');
            img_url = config.GetSection("SiteSettings:URLImage").Value.ToString();
            _httpContext = httpContext;
        }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index(HoiVienSearchVM? searchVM)
        {
            //HoiVienSearchVM searchVM = new HoiVienSearchVM();
            CreateViewBagSearch();
            return View(searchVM);
        }
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult _Search(HoiVienSearchVM search,int? Skip =0)
        {
            var model = (from cb in _context.CanBos
                          join pv in _context.PhamVis on cb.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                          where pv.AccountId == AccountId()
                          && cb.IsHoiVien == true
                          select cb).Include(it => it.NgheNghiep)
                .Include(it => it.DiaBanHoatDong)
                .ThenInclude(it=>it!.PhuongXa)
                .ThenInclude(it=>it!.QuanHuyen)
                .Include(it => it.DanToc)
                .Include(it => it.TonGiao)
                .Include(it => it.TrinhDoHocVan)
                .Include(it => it.TrinhDoChuyenMon)
                .Include(it => it.TrinhDoChinhTri)
                .Include(it => it.QuaTrinhKhenThuongs)
                .Include(it => it.GiaDinhThuocDien)
                .Include(it => it.ChiHoi)
                .Include(it => it.ToHoi)
                .Include(it => it.CoSo).AsQueryable();
           
            if (!String.IsNullOrEmpty(search.HoVaTen))
            {
                model = model.Where(it => it.HoVaTen.Contains(search.HoVaTen));
            }

            if (search.MaDiaBanHoiVien != null)
            {
                model = model.Where(it => it.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
            }
            if (search.MaChiHoi != null)
            {
                model = model.Where(it => it.MaChiHoi == search.MaChiHoi);
            }


            if (!String.IsNullOrEmpty(search.MaCanBo))
            {
                model = model.Where(it => it.MaCanBo == search.MaCanBo);
            }
            if (search.MaQuanHuyen != null)
            {
                model = model.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
            }
            if (!String.IsNullOrEmpty(search.TenPhuongXa))
            {
                model = model.Where(it => it.DiaBanHoatDong!.PhuongXa!.TenPhuongXa.ToUpper() == search.TenPhuongXa || it.DiaBanHoatDong!.QuanHuyen!.TenQuanHuyen.ToUpper() == search.TenPhuongXa);
            }
            if (search.IsRoiHoi != null && search.IsRoiHoi == true)
            {
                model = model.Where(it => it.isRoiHoi == search.IsRoiHoi);
                if (search.RoiTuNam != null) {
                    model = model.Where(it => it.NgayRoiHoi != null && it.NgayRoiHoi.Value.Year >=search.RoiTuNam);
                }
                if (search.RoiDenNam != null)
                {
                    model = model.Where(it => it.NgayRoiHoi != null && it.NgayRoiHoi.Value.Year <= search.RoiDenNam);
                }
            }
            else if(search.IsRoiHoi != null && search.IsRoiHoi == false)
            {
                model = model.Where(it => it.isRoiHoi == null || it.isRoiHoi == search.IsRoiHoi);
            }
            var total = model.Count();
            //model = model.Take(1000);
            var data = model
                .Select(it => new 
                {
                   
                    IDCanBo = it.IDCanBo,
                    MaCanBo = it.MaCanBo == null?"":it.MaCanBo,
                    HoVaTen = it.HoVaTen,
                    NgaySinh = it.NgaySinh,
                    GioiTinh = (int)it.GioiTinh ==1?"Nam":"Nữ",
                    SoCCCD = it.SoCCCD!,
                    NgayCapCCCD = it.NgayCapCCCD!,
                    HoKhauThuongTru = it.HoKhauThuongTru,
                    ChoOHienNay = it.ChoOHienNay!,
                    SoDienThoai = it.SoDienThoai,
                    DangVien = it.DangVien == null ? "" :"X",
                    NgayvaoDangDuBi = it.NgayvaoDangDuBi,
                    NgayVaoDangChinhThuc = it.NgayVaoDangChinhThuc,

                    TenDiaBanHoatDong = it.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen + " " +  it.DiaBanHoatDong!.TenDiaBanHoatDong,
                    DanToc = it.DanToc!.TenDanToc,
                    TonGiao = it.TonGiao!.TenTonGiao,
                    TrinhDoHocvan = it.TrinhDoHocVan.TenTrinhDoHocVan,
                    MaTrinhDoChuyenMon = it.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                    MaTrinhDoChinhTri = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                    NgayVaoHoi = it.NgayVaoHoi != null?it.NgayVaoHoi.Value.ToString("dd/MM/yyyy"):"",
                    NgayThamGiaCapUyDang = it.NgayThamGiaCapUyDang,
                    NgayThamGiaHDND = it.NgayThamGiaHDND,
                    VaiTro = it.VaiTro == "1" ? "Chủ hộ" : "Quan hệ chủ hộ",
                    VaiTroKhac = it.VaiTroKhac,
                    GiaDinhThuocDien = it.GiaDinhThuocDien!.TenGiaDinhThuocDien,
                    GiaDinhThuocDienKhac = it.GiaDinhThuocDienKhac,
                    NgheNghiepHienNay = it.NgheNghiep!.TenNgheNghiep,
                    Loai_DV_SX_ChN = it.Loai_DV_SX_ChN,
                    DienTich_QuyMo = it.DienTich_QuyMo,
                    SoLuong = it.SoLuong,
                    LoaiHoiVien = it.LoaiHoiVien,
                    ThamGia_SH_DoanThe_HoiDoanKhac = it.ThamGia_SH_DoanThe_HoiDoanKhac,
                    ThamGia_CLB_DN_MH_HTX_THT = it.ThamGia_CLB_DN_MH_HTX_THT,
                    ThamGia_THNN_CHNN = it.ThamGia_THNN_CHNN,
                    HoiVienNongCot = it.HoiVienNongCot == true ? "X" : "",
                    HoiVienUuTuNam = it.HoiVienUuTuNam,
                    HoiVienDanhDu = it.HoiVienDanhDu == true ? "X" : "",

                    NDSXKDG = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "15").Select(it => it.GhiChu).ToList()),
                    NDTieuBieu = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "16").Select(it => it.GhiChu).ToList()),
                    NDVietnamXS = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "22").Select(it => it.GhiChu).ToList()),
                    KNCGCND = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "17").Select(it => it.GhiChu).ToList()),
                    CanBoHoiCoSoGioi = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "18").Select(it => it.GhiChu).ToList()),
                    SangTaoNhaNong = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "19").Select(it => it.GhiChu).ToList()),
                    GuongDiemHinh = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "13").Select(it => it.GhiChu).ToList()),
                    GuongDanVanKheo = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "20").Select(it => it.GhiChu).ToList()),
                    GuongDiemHinhHocTapLamTheoBac = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "21").Select(it => it.GhiChu).ToList()),
                    SanPhamNongNghiepTieuBieu_OCOP = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "23").Select(it => it.GhiChu).ToList()),
                    HoTrovayVon = it.HoTrovayVon,
                    HoTroKhac = it.HoTroKhac,
                    HoTroDaoTaoNghe = it.HoTroDaoTaoNghe,

                    KKAnToanThucPham = it.KKAnToanThucPham,
                    DKMauNguoiNongDanMoi = it.DKMauNguoiNongDanMoi,
                    ChiHoi = it.ChiHoi!.TenChiHoi,
                    ToHoi = it.ToHoi!.TenToHoi,
                    GhiChu = it.GhiChu,
                }).OrderBy(it=>it.TenDiaBanHoatDong).Skip(Skip!.Value).Take(1000).ToList();
            //var json = Json(data);

            return Json(new
            {
                Code = System.Net.HttpStatusCode.OK,
                Success = true,
                Data = data,
                total = total
            });
        }

        public IActionResult LoadHoiVienOrg(HoiVienSearchVM searchVM) {
            Guid? id = null;string? maQuanHuyen = null;
            try
            {
                id = Guid.Parse(searchVM.MaQuanHuyen);
            }
            catch (Exception)
            {
                maQuanHuyen = searchVM.MaQuanHuyen;
            }
            searchVM.TenPhuongXa = searchVM.TenPhuongXa.Replace(" Q. Bình Tân", "").Replace(" Q. Bình Thạnh","");
            CreateViewBagSearch(maQuanHuyen:maQuanHuyen,maDiaBan:id);
            return View("Index", searchVM);
        }
        #endregion Index
        #region Create
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Create()
        {
            CreateViewBag();
            HoiVienVM obj = new HoiVienVM();
            
            obj.HinhAnh = img_url;
            return View(obj);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HoiVienMTVM insert, IFormFile? avtFileInbox, IFormFile?[] filesDinhKem)
        {
            CheckError(insert);
            return ExecuteContainer(() => {
                CanBo add = new CanBo();
                insert.GetHoiVien(add);
                add.IDCanBo = Guid.NewGuid();
                add.Actived = true;
                add.isRoiHoi = false;
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                add.HoiVienDuyet = true;
                if (avtFileInbox != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = add.MaCanBo!;
                    var uploads = Path.Combine(wwwRootPath, @"images\canbo");

                    bool folderExists = System.IO.Directory.Exists(uploads);
                    if (!folderExists)
                        System.IO.Directory.CreateDirectory(uploads);

                    var extension = Path.GetExtension(avtFileInbox.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        avtFileInbox.CopyTo(fileStream);
                    }
                    add.HinhAnh = @"\images\canbo\" + fileName + extension;
                }
                if (filesDinhKem.Count() > 0)
                {
                    var dataFile = AdFiles(filesDinhKem, add);
                    _context.FileDinhKems.AddRange(dataFile);
                }
                if (add.DoanTheChinhTri_HoiDoan_HoiViens.Count() > 0)
                {
                    add.DoanTheChinhTri_HoiDoan_HoiViens.ToList().ForEach(it => {
                        it.IDHoiVien = add.IDCanBo;
                        it.CreatedAccountId = AccountId();
                    });
                }
                if (add.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Count() > 0)
                {
                    add.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.ToList().ForEach(it => {
                        it.IDHoiVien = add.IDCanBo;
                        it.CreatedAccountId = AccountId();
                    });
                }
                if (add.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Count() > 0)
                {
                    add.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.ToList().ForEach(it => {
                        it.IDHoiVien = add.IDCanBo;
                        it.CreatedAccountId = AccountId();
                    });
                }
                _context.Attach(add).State = EntityState.Modified;
                _context.CanBos.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.HoiVien.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id)
        {
            var phamVis = Function.GetPhamVi(AccountId: AccountId()!.Value, _context: _context);
            var item = _context.CanBos.Include(it=>it.DoanTheChinhTri_HoiDoan_HoiViens).Include(it=>it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens)
                .Include(it => it.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens).SingleOrDefault(it => it.IDCanBo == id && phamVis.Contains(it.MaDiaBanHoatDong!.Value));
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            HoiVienVM edit = HoiVienMTVM.SetHoiVien(item);
            var file = _context.FileDinhKems.Where(it => it.Id == id).ToList();
            var khenThuongs = _context.QuaTrinhKhenThuongs.Where(it => it.IDCanBo == item.IDCanBo).Include(it => it.DanhHieuKhenThuong).Select(it => new HVKhenThuong {
                ID = it.IDQuaTrinhKhenThuong,
                TenDanhHieu = it.DanhHieuKhenThuong.TenDanhHieuKhenThuong,
                Nam = it.Nam == null ? it.GhiChu : it.Nam.Value.ToString(),
                GhiChu = it.Loai =="01"?"Đang đề xuất":""
            }).ToList();
            edit.FileDinhKems!.AddRange(file);
            edit.khenThuongs = khenThuongs;
            CreateViewBag(IdCoSo: item.IdCoSo, IdDepartment:item.IdDepartment, maChucVu:item.MaChucVu,maTrinhDoHocVan:item.MaTrinhDoHocVan,
                maTrinhDoChinhTri:item.MaTrinhDoChinhTri,maDanToc:item.MaDanToc,maTonGiao:item.MaTonGiao,maTrinhDoChuyenMon:item.MaTrinhDoChuyenMon,
                maDiaBanHoatDong:item.MaDiaBanHoatDong,maHocVi:item.MaHocVi,
                MaDoanTheChinhTri_HoiDoan:item.DoanTheChinhTri_HoiDoan_HoiViens.Select(it=>it.MaDoanTheChinhTri_HoiDoan).ToList(),
                Id_CLB_DN_MH_HTX_THT:item.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Select(it => it.Id_CLB_DN_MH_HTX_THT).ToList(),
                Ma_ToHoiNganhNghe_ChiHoiNganhNghe:item.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Select(it=>it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe).ToList(),MaGiaDinhThuocDien:item.MaGiaDinhThuocDien);
            return View(edit);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HoiVienMTVM obj, IFormFile? avtFileInbox, IFormFile?[] filesDinhKem)
        {
            CheckError(obj);
            return ExecuteContainer(() => {
                var phamVis = Function.GetPhamVi(AccountId: AccountId()!.Value, _context: _context);
                var edit = _context.CanBos.Include(it=> it.DoanTheChinhTri_HoiDoan_HoiViens)
                .Include(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens)
                .Include(it => it.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens)
                .SingleOrDefault(it => it.IDCanBo == obj.IDCanBo && phamVis.Contains(it.MaDiaBanHoatDong!.Value));
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.HoiVien.ToLower())
                    });
                }
                obj.GetHoiVien(edit);
                edit.Actived = obj.Actived!.Value;
                edit.isRoiHoi = obj.IsRoiHoi;
                edit.NgayRoiHoi = obj.NgayRoiHoi;
                edit.LyDoRoiHoi = obj.LyDoRoiHoi;
                edit.LastModifiedTime = DateTime.Now;
                edit.LastModifiedAccountId = AccountId();
                if (edit.DoanTheChinhTri_HoiDoan_HoiViens.Count() > 0)
                {
                    var dels = _context.DoanTheChinhTri_HoiDoan_HoiViens.Where(it => it.IDHoiVien == edit.IDCanBo).ToList();
                    if (dels.Count > 0)
                    {
                        _context.RemoveRange(dels);
                    }
                    edit.DoanTheChinhTri_HoiDoan_HoiViens.ToList().ForEach(it => {
                        it.IDHoiVien = edit.IDCanBo;
                        it.CreatedAccountId = AccountId();
                    });
                }
                if (edit.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Count() > 0)
                {
                    var dels = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Where(it => it.IDHoiVien == edit.IDCanBo).ToList();
                    if (dels.Count > 0)
                    {
                        _context.RemoveRange(dels);
                    }
                    edit.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.ToList().ForEach(it => {
                        it.IDHoiVien = edit.IDCanBo;
                        it.CreatedAccountId = AccountId();
                    });
                }
                if (edit.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Count() > 0)
                {
                    var dels = _context.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Where(it => it.IDHoiVien == edit.IDCanBo).ToList();
                    if (dels.Count > 0)
                    {
                        _context.RemoveRange(dels);
                    }
                    edit.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.ToList().ForEach(it => {
                        it.IDHoiVien = edit.IDCanBo;
                        it.CreatedAccountId = AccountId();
                    });
                }
                if (avtFileInbox != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = edit.MaCanBo!;
                    var uploads = Path.Combine(wwwRootPath, @"Images\canbo");

                    bool folderExists = System.IO.Directory.Exists(uploads);
                    if (!folderExists)
                        System.IO.Directory.CreateDirectory(uploads);

                    var extension = Path.GetExtension(avtFileInbox.FileName);
                    if (obj.HinhAnh != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, edit.HinhAnh!.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        avtFileInbox.CopyTo(fileStream);
                    }
                    edit.HinhAnh = @"\Images\canbo\" + fileName + extension;
                }
                if (filesDinhKem.Count() > 0)
                {
                   var dataFile= AdFiles(filesDinhKem, edit);
                    _context.FileDinhKems.AddRange(dataFile);
                }
                HistoryModelRepository history = new HistoryModelRepository(_context);
                history.SaveUpdateHistory(edit.IDCanBo.ToString(), AccountId()!.Value, edit);
                _context.CanBos.Update(edit);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.HoiVien.ToLower())
                });
            });
        }
        #endregion Edit
        #region View
        [HttpGet]
        public IActionResult XemThongTin()
        {
            CreateViewBagSearch();
            ViewBag.HTTP = "HttpGet";
            return View(new HoiVienDetailVM()) ;
        }
        [HttpPost]
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
                       MaGiaDinhThuocDien = it.MaGiaDinhThuocDien,

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
                var lisCauHoi = _context.HoiVienHoiDaps.Include(it=>it.HoiVien).Where(it => it.IDHoivien == hoivien.IDCanBo && it.TraLoi != true).OrderBy(it=>it.Ngay).Select(it=>new HoiVienHoiDapDetail { 
                    ID = it.ID,
                    HoVaTen = it.HoiVien.HoVaTen,
                    NoiDung= it.NoiDung,
                    TraLoi= it.TraLoi,
                    Ngay = it.Ngay,
                    IdParent = it.IdParent
                }).ToList();
                if (lisCauHoi.Count() >0)
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
            CreateViewBagImport();
            return View(hoivien);
        }
        #endregion View
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.CanBos.FirstOrDefault(p => p.IDCanBo == id);


                if (del != null)
                {
                    //_context.Entry(accountInRoleModels).State = EntityState.Deleted;
                    //_context.Entry(account).State = EntityState.Deleted;
                    if (del.HinhAnh != null && del.HinhAnh != "")
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        var oldImagePath = Path.Combine(wwwRootPath, del.HinhAnh!.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                       
                    }
                    var deleteAllFile = _context.FileDinhKems.Where(it => it.Id == del.IDCanBo);
                    if (deleteAllFile.Count() > 0)
                    {
                        foreach (var item in deleteAllFile)
                        {
                            if (FunctionFile.Delete(_hostEnvironment, item.Url))
                            {
                                _context.Remove(deleteAllFile);
                            }
                        }
                    }
                    _context.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.HoiVien.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.HoiVien.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Import Excel 
        [HoiNongDanAuthorization]
        public IActionResult _Import()
        {
            CreateViewBagImport1();
            return PartialView();
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import(Guid? MaDiaBanHoiVien)
        {
            if (MaDiaBanHoiVien == null || MaDiaBanHoiVien.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = false,
                    Data = "Chưa chọn hội địa bàn hội nông dân muốn import"
                });
            }
            DataSet ds = GetDataSetFromExcel();
            List<string> errorList = new List<string>();
            var chiHois = _context.ChiHois.ToList();
            var toHois = _context.ToHois.ToList();
            int iCapNhat = 0;
            return ExcuteImportExcel(() => {
                if (ds.Tables.Count > 0)
                {
                    const TransactionScopeOption opt = new TransactionScopeOption();

                    TimeSpan span = new TimeSpan(0, 0, 30, 30);
                    using (TransactionScope ts = new TransactionScope(opt, span))
                    {
                        
                        foreach (DataTable dt in ds.Tables)
                        {
                            string contCode = dt.Columns[0].ColumnName.ToString();
                            //if (contCode == controllerCode)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    //string aa = dr.ItemArray[0].ToString();
                                    if (dt.Rows.IndexOf(dr) >= startIndex-1)
                                    {
                                        if (!string.IsNullOrEmpty(dr.ItemArray[0]!.ToString()))
                                        {
                                            var data = CheckTemplateHoiVien(dr.ItemArray!, chiHois, toHois);
                                            if (!string.IsNullOrEmpty(data.Error))
                                            {
                                                errorList.Add(data.Error);
                                            }
                                            else
                                            {
                                                // Tiến hành cập nhật
                                                data.CanBo.MaDiaBanHoatDong = MaDiaBanHoiVien;
                                                data.CanBo.HoiVienDuyet = true;

                                                data.CanBo.IsHoiVien = true;
                                                string result = ExecuteImportExcelHoiVien(data, MaDiaBanHoiVien.Value);
                                                if (result != LanguageResource.ImportSuccess)
                                                {
                                                    errorList.Add(result);
                                                }
                                                else
                                                {
                                                    iCapNhat++;
                                                }
                                            }
                                        }
                                        else
                                            break;
                                        //Check correct template

                                    }
                                }
                            }
                           
                        }
                        if (errorList != null && errorList.Count > 0)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = false,
                                Data = String.Join("<br/>", errorList)
                            }); ;
                        }
                        ts.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = LanguageResource.ImportSuccess + iCapNhat.ToString()
                        });

                    }
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = false,
                        Data = LanguageResource.Validation_ImportExcelFile
                    });
                }
            });

        }
        protected DataSet GetDataSetFromExcel()
        {
            DataSet ds = new DataSet();
            try
            {
                var file = Request.Form.Files[0];
                if (file != null && file.Length > 0)
                {
                    //Check file is excel
                    //Notes: Châu bổ sung .xlsb
                    if (file.FileName.Contains("xls") || file.FileName.Contains("xlsx") || file.FileName.Contains("xlsb"))
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        var fileName = Path.GetFileName(file.FileName);
                        var mapPath = Path.Combine(wwwRootPath, @"upload\excel");
                        if (!Directory.Exists(mapPath))
                        {
                            Directory.CreateDirectory(mapPath);
                        }
                        var path = Path.Combine(mapPath, fileName);
                        using (var fileStream = new FileStream(Path.Combine(mapPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        using (ClassImportExcel excelHelper = new ClassImportExcel(path))
                        {
                            excelHelper.Hdr = "YES";
                            excelHelper.Imex = "1";
                            ds = excelHelper.ReadDataSet();
                        }
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }
                return ds;
            }
            catch
            {
                return null!;
            }
        }
        #endregion Import Excel
        #region Export Data
        [HoiNongDanAuthorization]
        public IActionResult ExportCreate()
        {
            //List<HoiVienExcelVM> data = new List<HoiVienExcelVM>();
            //return Export(data);
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, url_excel);
            List<HoiVienExcelNewVM> data = new List<HoiVienExcelNewVM>();
            return Export(data, url);
        }
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit(HoiVienSearchVM search)
        {
            var model = (from cb in _context.CanBos
                         join pv in _context.PhamVis on cb.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                         where pv.AccountId == AccountId()
                         && cb.IsHoiVien == true
                         select cb).Include(it => it.NgheNghiep)
             .Include(it => it.DiaBanHoatDong)
             .ThenInclude(it => it!.PhuongXa)
             .ThenInclude(it => it!.QuanHuyen)
             .Include(it => it.DanToc)
             .Include(it => it.TonGiao)
             .Include(it => it.TrinhDoHocVan)
             .Include(it => it.TrinhDoChuyenMon)
             .Include(it => it.TrinhDoChinhTri)
             .Include(it => it.QuaTrinhKhenThuongs)
             .Include(it => it.GiaDinhThuocDien)
             .Include(it => it.ChiHoi)
             .Include(it => it.ToHoi)
             .Include(it => it.CoSo).AsQueryable();
            if (!String.IsNullOrEmpty(search.MaCanBo))
            {
                model = model.Where(it => it.MaCanBo == search.MaCanBo);
            }
            if (!String.IsNullOrEmpty(search.HoVaTen))
            {
                model = model.Where(it => it.HoVaTen.Contains(search.HoVaTen));
            }

            if (search.MaDiaBanHoiVien != null)
            {
                model = model.Where(it => it.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
            }
            if (search.MaChucVu != null)
            {
                model = model.Where(it => it.MaChucVu == search.MaChucVu);
            }
            if (search.Actived != null)
            {
                model = model.Where(it => it.Actived == search.Actived);
            }

            var data = model.Include(it => it.TrinhDoHocVan)
                .Include(it => it.TrinhDoChuyenMon)
                .Include(it => it.TrinhDoChinhTri)
                .Include(it => it.DanToc)
                .Include(it => it.TonGiao)
                .Include(it => it.ChiHoi)
                .Include(it => it.ToHoi)
                .Include(it => it.DiaBanHoatDong)
                    .ThenInclude(it => it.QuanHuyen)
                .Include(it => it.DiaBanHoatDong)
                    .ThenInclude(it => it.PhuongXa)
                .Select(item => new HoiVienExcelNewVM
                {
                    IDCanBo = item.IDCanBo,

                    HoVaTen = item.HoVaTen,
                    NgaySinh = item.NgaySinh,
                    GioiTinh = item.GioiTinh == GioiTinh.Nam ? true : false,
                    SoCCCD = item.SoCCCD!,
                    NgayCapCCCD = item.NgayCapCCCD,
                    MaCanBo = item.MaCanBo,
                    NgayCapThe = item.NgayCapThe != null? item.NgayCapThe.Value.ToString("dd/MM/yyyy"):"",
                    TenToHoi = item.ToHoi.TenToHoi,
                    TenChiHoi = item.ChiHoi.TenChiHoi,

                    HoKhauThuongTru = item.HoKhauThuongTru,
                    HoKhauThuongTru_XaPhuong = item.DiaBanHoatDong!.PhuongXa.TenPhuongXa,
                    HoKhauThuongTru_QuanHuyen = item.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen,
                    ChoOHienNay = item.ChoOHienNay!,
                    ChoOHienNay_XaPhuong = item.DiaBanHoatDong!.PhuongXa.TenPhuongXa,
                    ChoOHienNay_QuanHuyen = item.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen,
                    SoDienThoai = item.SoDienThoai,
                    NgayvaoDangDuBi = item.NgayvaoDangDuBi,
                    NgayVaoDangChinhThuc = item.NgayVaoDangChinhThuc,
                    MaDanToc = item.DanToc.TenDanToc,
                    MaTonGiao = item.TonGiao.TenTonGiao,
                    MaTrinhDoHocVan = item.TrinhDoHocVan.TenTrinhDoHocVan,
                    MaTrinhDoChuyenMon = item.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                    MaTrinhDoChinhTri = item.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                    NgayVaoHoi = item.NgayVaoHoi != null ? item.NgayVaoHoi.Value.ToString("dd/MM/yyyy") : "",

                    NgayThamGiaCapUyDang = item.NgayThamGiaCapUyDang,
                    NgayThamGiaHDND = item.NgayThamGiaHDND,
                    VaiTro = item.VaiTro == "1" ? "X" : "",
                    VaiTroKhac = item.VaiTroKhac,
                    HoNgheo = item.MaGiaDinhThuocDien == "01"? "X" : "",
                    CanNgheo = item.MaGiaDinhThuocDien == "02" ? "X" : "",
                    GiaDinhChinhSach = item.MaGiaDinhThuocDien == "03" ? "X" : "",
                    GiaDinhThanhPhanKhac = item.GiaDinhThuocDienKhac,
                    NongDan = item.MaNgheNghiep == "01" ? "X" : "",
                    CongNhan = item.MaNgheNghiep == "02" ? "X" : "",
                    CV_VC = item.MaNgheNghiep == "03" ? "X" : "",
                    HuuTri = item.MaNgheNghiep == "04" ? "X" : "",
                    DoanhNghiep = item.MaNgheNghiep == "05" ? "X" : "",
                    LaoDongTuDo = item.MaNgheNghiep == "06" ? "X" : "",
                    HS_SV = item.MaNgheNghiep == "07" ? "X" : "",
                    SX_ChN = item.Loai_DV_SX_ChN,
                    DienTich_QuyMo = item.DienTich_QuyMo,
                    SoLuong = item.SoLuong,
                    SinhHoatDoanTheChinhTri = item.ThamGia_SH_DoanThe_HoiDoanKhac,
                    ThamGia_CLB_DN_HTX = item.ThamGia_CLB_DN_MH_HTX_THT,
                    ThamGia_THNN_CHNN = item.ThamGia_THNN_CHNN,
                    HV_NongCot = item.HoiVienNongCot == true ? "X" : "",
                    HV_UuTuNam = item.HoiVienUuTuNam,
                    HV_DanhDu = item.HoiVienDanhDu == true ? "X" : "",
                    NDSXKDG = String.Join(',', item.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == item.IDCanBo && p.MaDanhHieuKhenThuong == "15").Select(it => it.GhiChu).ToList()),
                    NoDanTieuBieu = String.Join(',', item.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == item.IDCanBo && p.MaDanhHieuKhenThuong == "16").Select(it => it.GhiChu).ToList()),
                    NDXuatSac = String.Join(',', item.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == item.IDCanBo && p.MaDanhHieuKhenThuong == "22").Select(it => it.GhiChu).ToList()),
                    KNCGCND = String.Join(',', item.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == item.IDCanBo && p.MaDanhHieuKhenThuong == "17").Select(it => it.GhiChu).ToList()),
                    CanBoHoiCoSoGioi = String.Join(',', item.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == item.IDCanBo && p.MaDanhHieuKhenThuong == "18").Select(it => it.GhiChu).ToList()),
                    SangTaoNhaNong = String.Join(',', item.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == item.IDCanBo && p.MaDanhHieuKhenThuong == "19").Select(it => it.GhiChu).ToList()),
                    GuongDiemHinh = String.Join(',', item.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == item.IDCanBo && p.MaDanhHieuKhenThuong == "13").Select(it => it.GhiChu).ToList()),
                    GuongDanVanKheo = String.Join(',', item.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == item.IDCanBo && p.MaDanhHieuKhenThuong == "20").Select(it => it.GhiChu).ToList()),
                    GuongDiemHinhHocTapLamTheoBac = String.Join(',', item.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == item.IDCanBo && p.MaDanhHieuKhenThuong == "21").Select(it => it.GhiChu).ToList()),
                    HoTrovayVon = item.HoTrovayVon,
                    HoTroKhac = item.HoTroKhac,
                    HoTroDaoTaoNghe = item.HoTroDaoTaoNghe,
                    GhiChu = item.GhiChu,

                    ChiHoiDanCu_CHT = item.ChiHoiDanCu_CHT,
                    ChiHoiDanCu_CHP = item.ChiHoiDanCu_CHP,
                    ChiHoiNgheNghiep_CHT = item.ChiHoiNgheNghiep_CHT,
                    ChiHoiNgheNghiep_CHP = item.ChiHoiNgheNghiep_CHP,

                    NgayRoiHoi =  item.NgayRoiHoi != null ? item.NgayRoiHoi.Value.ToString("dd/MM/yyyy") : "",
                    LyDoRoiHoi = item.LyDoRoiHoi
                }).ToList();
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, url_excel);

            return Export(data, url);
        }
        [HoiNongDanAuthorization]
        public FileContentResult Export(List<HoiVienExcelNewVM> data, string url)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "IDCanBo", isAllowedToEdit = false, isText = true });

            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinh", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GioiTinh", isBoolean = true, isComment = true, strComment = "Nam để chữ X" });
            columns.Add(new ExcelTemplate() { ColumnName = "SoCCCD", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayCapCCCD", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "MaCanBo", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayCapThe", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "TenToHoi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "TenChiHoi", isAllowedToEdit = true, isText = true });
           
            columns.Add(new ExcelTemplate() { ColumnName = "HoKhauThuongTru", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoKhauThuongTru_XaPhuong", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoKhauThuongTru_QuanHuyen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay_XaPhuong", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay_QuanHuyen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoDienThoai", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayvaoDangDuBi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoDangChinhThuc", isAllowedToEdit = true, isText = true });

            var danToc = _context.DanTocs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaDanToc, Name = x.TenDanToc }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaDanToc", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = danToc, TypeId = ConstExcelController.StringId,Title = "Dân Tộc" });

            var tonGiao = _context.TonGiaos.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTonGiao, Name = x.TenTonGiao }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTonGiao", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tonGiao, TypeId = ConstExcelController.StringId, Title="Tôn Giáo" });

            var hocVan = _context.TrinhDoHocVans.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoHocVan, Name = x.TenTrinhDoHocVan }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoHocVan", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = hocVan, TypeId = ConstExcelController.StringId ,Title = "Trình Độ  Học Vấn"});

            var chuyenNganh = _context.TrinhDoChuyenMons.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChuyenMon, Name = x.TenTrinhDoChuyenMon }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChuyenMon", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chuyenNganh, TypeId = ConstExcelController.StringId , Title = "Chuyên Môn"});

            var chinhTri = _context.TrinhDoChinhTris.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChinhTri, Name = x.TenTrinhDoChinhTri }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChinhTri", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chinhTri, TypeId = ConstExcelController.StringId, Title = "Trình Độ Chính Trị" });

            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoHoi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayThamGiaCapUyDang", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayThamGiaHDND", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "VaiTro", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "VaiTroKhac", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoNgheo", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "CanNgheo", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GiaDinhChinhSach", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GiaDinhThanhPhanKhac", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NongDan", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "CongNhan", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "CV_VC", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HuuTri", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DoanhNghiep", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "LaoDongTuDo", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HS_SV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SX_ChN", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoLuong", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DienTich_QuyMo", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SinhHoatDoanTheChinhTri", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ThamGia_CLB_DN_HTX", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ThamGia_THNN_CHNN", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HV_NongCot", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HV_UuTuNam", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HV_DanhDu", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NDSXKDG", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NoDanTieuBieu", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NDXuatSac", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "KNCGCND", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "CanBoHoiCoSoGioi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SangTaoNhaNong", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GuongDiemHinh", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GuongDanVanKheo", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GuongDiemHinhHocTapLamTheoBac", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoTrovayVon", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoTroKhac", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoTroDaoTaoNghe", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GhiChu", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChiHoiDanCu_CHT", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChiHoiDanCu_CHP", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChiHoiNgheNghiep_CHT", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChiHoiNgheNghiep_CHP", isAllowedToEdit = true, isText = true });
  
            columns.Add(new ExcelTemplate() { ColumnName = "NgayRoiHoi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "LyDoRoiHoi", isAllowedToEdit = true, isText = true });
            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.HoiVien);
            try
            {


                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = controllerCode,
                    RowsToIgnore = 1,
                    isWarning = false,
                    isCode = true
                });
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = fileheader.ToUpper(),
                    RowsToIgnore = 1,
                    isWarning = false,
                    isCode = false
                });
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = LanguageResource.Export_ExcelWarning1 + "-" + LanguageResource.Export_ExcelWarning2,
                    RowsToIgnore = 1,
                    isWarning = true,
                    isCode = false
                });
            }
            catch (Exception ex)
            {
                string ss = ex.Message;

                throw;
            }

            //Header
            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(url, data, columns, heading, true, startIndex);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export Data
        #region Print the hoi vien
        [HttpPost]
        [HoiNongDanAuthorization]
        public IActionResult Print(List<Guid> lid) {

            var parameter = Guid.NewGuid();
            List<HoiNongDan.Models.Parameter> add = new List<Models.Parameter>();

            foreach (var id in lid)
            {
                add.Add(new HoiNongDan.Models.Parameter
                {
                    ID = Guid.NewGuid(),
                    Value = id.ToString(),
                    AccountID = AccountId()!.Value,
                    Parameter1 = parameter

                });
            }
            _context.Parameters.AddRange(add);
            if (_context.SaveChanges() > 0)
            {
                _httpContext.HttpContext!.Session.SetString("InTheHoiVien", parameter.ToString());
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = "Lưu parameter thành công"
                });
                
            }
            else
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                    Data = "Lỗi hệ thống không hiển thị report được"
                });
            }
        }
        public IActionResult ShowInTheHoiVien() {
            string mintype = "";
            int extension = 1;
            var path = $"{this._hostEnvironment.WebRootPath}\\reports\\TheHoiVien.rdlc";
            Dictionary<String, string> parameters = new Dictionary<string, string>();
            var keyPara = _httpContext.HttpContext!.Session.GetString("InTheHoiVien") ;
            List<Guid> idHoiVien = new List<Guid>();
            if (keyPara != null)
            {
                idHoiVien = _context.Parameters.Where(it=>it.Parameter1 == Guid.Parse(keyPara)).Select(it=> Guid.Parse(it.Value)).ToList();
            }
            var dataHoiVien = _context.CanBos.Where(it => it.IsHoiVien == true && idHoiVien.Contains(it.IDCanBo))
                .Include(it => it.DiaBanHoatDong).Select(it => new HoiVienTheTen
                {
                    HoVaTen = it.HoVaTen,
                    NgaySinh = it.NgaySinh!,
                    NoiCuTru = it.ChoOHienNay!.Replace("/n","")!,
                    NgayVaoHoi = it.NgayVaoHoi != null ? it.NgayVaoHoi.ToString():"",
                    NoiCapThe = it.DiaBanHoatDong!.TenDiaBanHoatDong

                }).Take(10).ToList();
            foreach (var item in dataHoiVien)
            {
                item.Ngay = DateTime.Now.Day.ToString().PadLeft(2, '0');
                item.Thang = DateTime.Now.Month.ToString().PadLeft(2, '0');
                item.Nam = DateTime.Now.Year.ToString();
                string[] array = item.NgaySinh.Replace(@"\", "/").Split("/");
                item.NgaySinh = "";
                if (array.Length == 3)
                {
                    item.NgaySinh = array[0].PadLeft(2, '0');
                    item.ThangSinh = array[1].PadLeft(2, '0');
                    item.NamSinh = array[2];
                }
                else if (array.Length == 2)
                {
                    item.ThangSinh = array[0].PadLeft(2, '0');
                    item.NamSinh = array[1];
                }
                else if (array.Length == 1)
                {
                    item.NamSinh = array[0];
                }
                //array = item.Replace(@"\", "/").Split("/");
                item.NgayVaoHoi = "";
                if (array.Length == 3)
                {
                    item.NgayVaoHoi = array[0].PadLeft(2, '0');
                    item.ThangVaoHoi = array[1].PadLeft(2, '0');
                    item.NamVaoHoi = array[2];
                }
                else if (array.Length == 2)
                {
                    item.ThangVaoHoi = array[0].PadLeft(2, '0');
                    item.NamVaoHoi = array[1];
                }
                else if (array.Length == 1)
                {
                    item.NamVaoHoi = array[0];
                }
            }
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("HoiVienTheTen", dataHoiVien);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mintype);
            var dels = _context.Parameters.Where(it => it.Parameter1 == Guid.Parse(keyPara!));
            if (dels != null && dels.Count() > 0)
            {
                _context.Parameters.RemoveRange(dels);
                _httpContext.HttpContext.Session.Remove("InTheHoiVien");
                _context.SaveChanges();
            }
            return File(result.MainStream, "application/pdf");
        }
        #endregion Print the hoi vien
        #region Helper
        private void CheckError(HoiVienMTVM insert)
        {
            var checkExistMaCB = _context.CanBos.Where(it => it.MaCanBo == insert.MaCanBo).ToList();
            if (checkExistMaCB.Count > 0 && insert.IDCanBo == null)
            {
                ModelState.AddModelError("MaCanBo", "Mã cán bộ tồn tại không thể thêm");
            }
            
            if (insert.VaiTro == "2" && (String.IsNullOrEmpty(insert.VaiTroKhac) || String.IsNullOrWhiteSpace(insert.VaiTroKhac)))
            {
                ModelState.AddModelError("VaiTroKhac", "Chưa nhập quan hệ với chủ hộ");
            }
            if (insert.IsRoiHoi == true)
            {
                if ((String.IsNullOrEmpty(insert.LyDoRoiHoi) || String.IsNullOrWhiteSpace(insert.LyDoRoiHoi)))
                {
                    ModelState.AddModelError("LyDoRoiHoi", "Lý do rời hội chưa nhập");
                }
                if (insert.NgayRoiHoi == null)
                {
                    ModelState.AddModelError("NgayRoiHoi", "Ngày rời hội chưa nhập");
                }
            }
            //if (!String.IsNullOrWhiteSpace(insert.NgayVaoHoi))
            //{
            //    DateTime dt = Function.ConvertStringToDate(insert.NgayVaoHoi);
            //    if (dt.Year == 1900 || dt.Year > DateTime.Now.Year)
            //    {
            //        ModelState.AddModelError("NgayVaoHoi", "Ngày vào hội không hợp lệ");
            //    }
                
            //}
            // Kiểm tra mã định danh
            if (!String.IsNullOrWhiteSpace(insert.MaDinhDanh))
            {
                var checkMaDinhDanh = _context.CanBos.Where(it => it.MaDinhDanh == insert.MaDinhDanh);
                if (insert.IDCanBo != null)
                {
                    if (checkMaDinhDanh.Where(it => it.IDCanBo != insert.IDCanBo).Count() > 0)
                    {
                        ModelState.AddModelError("MaDinhDanh", "Mã định danh đã tồn tại");
                    }
                }
                else if (insert.IDCanBo == null && checkMaDinhDanh.Count() > 0)
                {
                    // trường hợp thêm mới
                    ModelState.AddModelError("MaDinhDanh", "Mã định danh đã tồn tại");
                }
            }
            if (!String.IsNullOrWhiteSpace(insert.SoCCCD))
            {
                var checkSoCCCD = _context.CanBos.Where(it => it.SoCCCD == insert.SoCCCD);
                if (insert.IDCanBo != null)
                {
                    if (checkSoCCCD.Where(it => it.IDCanBo != insert.IDCanBo).Count() > 0)
                    {
                        ModelState.AddModelError("SoCCCD", "Số CCCD đã tồn tại");
                    }
                }
                else if (insert.IDCanBo == null && checkSoCCCD.Count() > 0)
                {
                    // trường hợp thêm mới
                    ModelState.AddModelError("SoCCCD", "Số CCCD đã tồn tại");
                }
            }
            if (insert.MaGiaDinhThuocDien == "04" && String.IsNullOrWhiteSpace(insert.GiaDinhThuocDienKhac)) {
                ModelState.AddModelError("GiaDinhThuocDienKhac", "Chưa nhập thông tin thành phần gia đình khác");
            }

        }
      
        public JsonResult LoadDonVi(Guid idCoSo)
        {
            var data = _context.Departments.Where(it => it.Actived == true && it.IDCoSo == idCoSo).OrderBy(p => p.OrderIndex).Select(it => new { IdDepartment = it.Id, Name = it.Name }).ToList();
            return Json(data);
        }
        private void CreateViewBag( Guid? IdCoSo = null, Guid? IdDepartment = null,
            Guid? maChucVu = null,
            String? maTrinhDoHocVan = null, String? maTrinhDoChinhTri = null,
            String? maDanToc = null, String? maTonGiao = null,string? maTrinhDoChuyenMon = null,Guid? maDiaBanHoatDong = null,string? maHocVi = null,Guid? MaChiHoi = null, Guid? MaToHoi = null,
            List<Guid>? MaDoanTheChinhTri_HoiDoan = null, List<Guid>? Id_CLB_DN_MH_HTX_THT = null,List<Guid>? Ma_ToHoiNganhNghe_ChiHoiNganhNghe = null,string? MaNgheNghiep = null, string? MaGiaDinhThuocDien = null)
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            
            ViewBag.MaChucVu = fnViewBag.ChucVu(value:maChucVu);
            
            ViewBag.MaTrinhDoHocVan = fnViewBag.TrinhDoHocVan(value: maTrinhDoHocVan);

            ViewBag.MaTrinhDoChuyenMon = fnViewBag.TrinhDoChuyenMon(value: maTrinhDoChuyenMon);

            ViewBag.MaTrinhDoChinhTri = fnViewBag.TrinhDoChinhTri(value: maTrinhDoChinhTri);

            ViewBag.MaDanToc = fnViewBag.DanToc(value: maDanToc);

            ViewBag.MaTonGiao = fnViewBag.TonGiao(value: maTonGiao);

            ViewBag.MaDiaBanHoatDong = fnViewBag.DiaBanHoatDong(maDiaBanHoatDong,AccountId());

            ViewBag.MaHocVi = fnViewBag.HocVi(value:maHocVi);

            ViewBag.MaChiHoi = fnViewBag.ChiHoi(value: MaChiHoi);

            ViewBag.MaToHoi = fnViewBag.ToHoi(value: MaToHoi);

            ViewBag.MaDoanTheChinhTri_HoiDoan = fnViewBag.DoanTheChinhTri_HoiDoan(value:MaDoanTheChinhTri_HoiDoan);

            ViewBag.Id_CLB_DN_MH_HTX_THT = fnViewBag.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac(value: Id_CLB_DN_MH_HTX_THT);

            ViewBag.Ma_ToHoiNganhNghe_ChiHoiNganhNghe = fnViewBag.ToHoiNganhNghe_ChiHoiNganhNghe(value: Ma_ToHoiNganhNghe_ChiHoiNganhNghe);

            ViewBag.MaNgheNghiep = fnViewBag.NgheNghiep(value: MaNgheNghiep);
            ViewBag.MaGiaDinhThuocDien = fnViewBag.GiaDinhThuocDien(value: MaGiaDinhThuocDien);
        }
        private void CreateViewBagSearch(string? maQuanHuyen = null,Guid? maDiaBan = null, Guid? maChiHoi = null)
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID:AccountId(),value: maDiaBan);

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen( idAc:AccountId(),value: maQuanHuyen);

            ViewBag.MaChiHoi = fnViewBag.ChiHoi(value: maChiHoi);

        }
        private void CreateViewBagImport() {
          
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoatDong = fnViewBag.DiaBanHoatDong(acID:AccountId());
        }
        public JsonResult loadDiaBanHoatDong(string? maQuanHuyen)
        {
            if (!String.IsNullOrWhiteSpace(maQuanHuyen))
            {
                var diaBanHoatDong = _context.DiaBanHoatDongs.Where(it => it.Actived == true && it.MaQuanHuyen == maQuanHuyen).Select(it => new { MaDiaBanHoatDong = it.Id, Name = it.TenDiaBanHoatDong }).Distinct().ToList();
                return Json(diaBanHoatDong);
            }
            else
            {
                var phamVis = Function.GetPhamVi(AccountId: AccountId()!.Value, _context: _context);
                var data = (from hv in _context.CanBos
                            join diaban in _context.DiaBanHoatDongs on hv.MaDiaBanHoatDong equals diaban.Id
                            where hv.IsHoiVien == true && diaban.Actived == true
                              && phamVis.Contains(diaban.Id)
                            select new
                            {
                                MaDiaBanHoatDong = diaban.Id,
                                Name = diaban.TenDiaBanHoatDong,
                            }
                                 ).Distinct().ToList();
                return Json(data);
            }
           
        }

        public JsonResult loadChiHoi(Guid? ma)
        {
            if (ma != null)
            {
                var data = _context.CanBos.Include(it => it.ChiHoi).Where(it => it.MaDiaBanHoatDong == ma && it.MaChiHoi != null).
                    Select(it => new { MaChiHoi = it.MaChiHoi, name = it.ChiHoi.TenChiHoi }).Distinct().ToList(); ;
                return Json(data);
            }
            else
            {
                var data = _context.ChiHois.Where(it => it.Actived == true).Select(it => new { MaChiHoi = it.MaChiHoi, name = it.TenChiHoi }).ToList();
                return Json(data);
            }

        }
        #endregion Helper
        #region Insert/Update data from excel file

        private string ExecuteImportExcelHoiVien(HoiVienImportExcelNew HoiVienExcel,Guid MaDiaBanHoiVien)
        {
            //Check:
            //1. If MenuId == "" then => Insert
            //2. Else then => Update
            CanBo canbo = new CanBo(); ;
            if (HoiVienExcel.isNullValueId == true)
            {
                try
                {
                    canbo = HoiVienExcel.CanBo;
                    canbo.HoiVienDuyet = true;
                    canbo.Actived = true;
                    canbo.Level = "90";
                    canbo.isRoiHoi = false;
                    canbo.IsHoiVien = true;
                    canbo.TuChoi = false;
                    canbo.CreatedTime = DateTime.Now;
                    if (HoiVienExcel.ListKhenThuong.Count() > 0)
                    {

                        _context.QuaTrinhKhenThuongs.AddRange(HoiVienExcel.ListKhenThuong);
                    }
                    if (HoiVienExcel.chiHois.Count() > 0)
                    {

                        _context.ChiHois.AddRange(HoiVienExcel.chiHois);
                    }
                    if (HoiVienExcel.toHois.Count() > 0)
                    {

                        _context.ToHois.AddRange(HoiVienExcel.toHois);
                    }
                    _context.Entry(canbo).State = EntityState.Added;
                }
                catch
                {

                }
            }
            else
            {
                var editHoiVien = _context.CanBos.Where(p => p.IDCanBo == HoiVienExcel.CanBo.IDCanBo && p.MaDiaBanHoatDong == MaDiaBanHoiVien).FirstOrDefault();
                if (editHoiVien != null)
                {
                    //canbo = HoiVienExcel.GetHoiVien(canbo);
                    editHoiVien.isRoiHoi = HoiVienExcel.CanBo.isRoiHoi;

                    editHoiVien.HoVaTen = HoiVienExcel.CanBo.HoVaTen;
                    editHoiVien.NgaySinh = HoiVienExcel.CanBo.NgaySinh;
                    editHoiVien.GioiTinh = HoiVienExcel.CanBo.GioiTinh;

                    editHoiVien.SoCCCD = HoiVienExcel.CanBo.SoCCCD;
                    editHoiVien.NgayCapCCCD = HoiVienExcel.CanBo.NgayCapCCCD;
                    editHoiVien.MaToHoi = HoiVienExcel.CanBo.MaToHoi;
                    editHoiVien.MaChiHoi = HoiVienExcel.CanBo.MaChiHoi;
                    editHoiVien.HoKhauThuongTru = HoiVienExcel.CanBo.HoKhauThuongTru;
                    editHoiVien.ChoOHienNay = HoiVienExcel.CanBo.ChoOHienNay;
                    editHoiVien.SoDienThoai = HoiVienExcel.CanBo.SoDienThoai;
                    editHoiVien.NgayvaoDangDuBi = HoiVienExcel.CanBo.NgayvaoDangDuBi;
                    editHoiVien.NgayVaoDangChinhThuc = HoiVienExcel.CanBo.NgayVaoDangChinhThuc;
                    editHoiVien.MaDanToc = HoiVienExcel.CanBo.MaDanToc;
                    editHoiVien.MaTonGiao = HoiVienExcel.CanBo.MaTonGiao;
                    editHoiVien.MaTrinhDoHocVan = HoiVienExcel.CanBo.MaTrinhDoHocVan;
                    editHoiVien.MaTrinhDoChuyenMon = HoiVienExcel.CanBo.MaTrinhDoChuyenMon;
                    editHoiVien.MaTrinhDoChinhTri = HoiVienExcel.CanBo.MaTrinhDoChinhTri;
                    editHoiVien.NgayVaoHoi = HoiVienExcel.CanBo.NgayVaoHoi;
                    editHoiVien.NgayThamGiaCapUyDang = HoiVienExcel.CanBo.NgayThamGiaCapUyDang;
                    editHoiVien.NgayThamGiaHDND = HoiVienExcel.CanBo.NgayThamGiaHDND;
                    editHoiVien.VaiTro = HoiVienExcel.CanBo.VaiTro;
                    editHoiVien.VaiTroKhac = HoiVienExcel.CanBo.VaiTroKhac;
                    editHoiVien.MaGiaDinhThuocDien = HoiVienExcel.CanBo.MaGiaDinhThuocDien;
                    editHoiVien.GiaDinhThuocDienKhac = HoiVienExcel.CanBo.GiaDinhThuocDienKhac;
                    editHoiVien.MaNgheNghiep = HoiVienExcel.CanBo.MaNgheNghiep;
                    editHoiVien.Loai_DV_SX_ChN = HoiVienExcel.CanBo.Loai_DV_SX_ChN;
                    editHoiVien.SoLuong = HoiVienExcel.CanBo.SoLuong;
                    editHoiVien.DienTich_QuyMo = HoiVienExcel.CanBo.DienTich_QuyMo;
                    editHoiVien.HoiVienNongCot = HoiVienExcel.CanBo.HoiVienNongCot;
                    editHoiVien.HoiVienUuTuNam = HoiVienExcel.CanBo.HoiVienUuTuNam;
                    editHoiVien.HoiVienDanhDu = HoiVienExcel.CanBo.HoiVienDanhDu;
                    editHoiVien.ChiHoiDanCu_CHT = HoiVienExcel.CanBo.ChiHoiDanCu_CHT;
                    editHoiVien.ChiHoiDanCu_CHP = HoiVienExcel.CanBo.ChiHoiDanCu_CHP;
                    editHoiVien.ChiHoiNgheNghiep_CHT = HoiVienExcel.CanBo.ChiHoiNgheNghiep_CHT;
                    editHoiVien.ChiHoiNgheNghiep_CHP = HoiVienExcel.CanBo.ChiHoiNgheNghiep_CHP;
                    editHoiVien.GhiChu = HoiVienExcel.CanBo.GhiChu;
                    editHoiVien.isRoiHoi = HoiVienExcel.CanBo.isRoiHoi;
                    editHoiVien.NgayRoiHoi = HoiVienExcel.CanBo.NgayRoiHoi;
                    editHoiVien.LyDoRoiHoi = HoiVienExcel.CanBo.LyDoRoiHoi;
                    editHoiVien.NgayCapThe = HoiVienExcel.CanBo.NgayCapThe;

                    

                    HistoryModelRepository history = new HistoryModelRepository(_context);
                    history.SaveUpdateHistory(editHoiVien.IDCanBo.ToString(), AccountId()!.Value, canbo);
                }
                else
                {
                    return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                            LanguageResource.HoiVien, HoiVienExcel.IDCanBo,
                                            string.Format(LanguageResource.Export_ExcelHeader,
                                            LanguageResource.HoiVien));
                }
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.InnerException!.Message + " " + canbo.HoVaTen;
            }
            return LanguageResource.ImportSuccess;
        }
        #endregion Insert/Update data from excel file
        #region Check data type 
        private HoiVienImportExcelNew CheckTemplateHoiVien(object[] row, List<ChiHoi> chiHois, List<ToHoi> toHois)
        {
            HoiVienImportExcelNew import = new HoiVienImportExcelNew();
            CanBo data = new CanBo();
            data.MaChucVu = Guid.Parse("D710D930-8342-474B-90A4-A1170A7A5691");
            List<QuaTrinhKhenThuong> listDanhHieu = new List<QuaTrinhKhenThuong>();
            string? value;
            int index = 0;
            for (int i = 0; i < row.Length; i++)
            {
                value = row[i] == null ? "" : row[i].ToString().Trim().Replace(System.Environment.NewLine, string.Empty).Trim();
                try
                {
                    switch (i)
                    {
                        case 0:
                            //Row Index
                            import.RowIndex = index = int.Parse(row[i].ToString());
                            break;
                        case 1:
                            // IDCanBo
                            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                            {
                                import.isNullValueId = true;
                                data.IDCanBo = Guid.NewGuid();
                                data.CreatedAccountId = AccountId();
                            }
                            else
                            {
                                data.IDCanBo = Guid.Parse(value);
                                import.isNullValueId = false;
                            }
                            break;

                        case 2:
                            // ho và tên
                            if (string.IsNullOrEmpty(value))
                            {
                                import.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.FullName), index);
                            }
                            else
                            {
                                data.HoVaTen = value;
                            }
                            break;
                        case 3:
                            //Năm sinh
                            try
                            {
                                //data.NgaySinh = DateTime.ParseExact(ngaySinh, DateFomat, new CultureInfo("en-US"));
                                data.NgaySinh = value;
                            }
                            catch (Exception)
                            {

                                import.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgaySinh, value, index);
                            }
                            break;
                        case 4:
                            // giới tính
                            if (string.IsNullOrEmpty(value))
                            {
                                data.GioiTinh = GioiTinh.Nữ;
                            }
                            else
                            {
                                data.GioiTinh = GioiTinh.Nam;
                            }
                            break;
                        case 5:
                            //  SoCCCD (*)
                            data.SoCCCD = value; data.SoCCCD = value;
                            break;
                        case 6:
                            // Ngày cấp số CCCD
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                try
                                {
                                    data.NgayCapCCCD = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US")).ToString("dd/MM/yyyy") ;
                                }
                                catch (Exception)
                                {

                                    import.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayCapCCCD, value, index);
                                }
                            }
                            break;
                        case 7:
                            // Số thể hội viên
                            data.MaCanBo = value;
                            break;

                        case 8:
                            // Ngày cấp thẻ
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                try
                                {
                                    value = value.Replace("y", "");
                                    data.NgayCapThe = DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {

                                    import.Error += string.Format("Ngày cấp thẻ dòng {0} không hợp lệ", index);
                                }
                            }
                            break;
                        case 9:
                            //  Tổ hội
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                var exist = toHois.Where(it => it.TenToHoi == value);
                                if (exist.Count() > 0)
                                {
                                    data.MaToHoi = exist.First().MaToHoi;
                                }
                                else
                                {
                                    ToHoi toHoi = new ToHoi { MaToHoi = Guid.NewGuid(), TenToHoi = value };
                                    data.MaToHoi = toHoi.MaToHoi;
                                    toHois.Add(toHoi);
                                    import.toHois.Add(toHoi);
                                }

                            }
                            break;
                        case 10:
                            //  Chi hội
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                var exist = chiHois.Where(it => it.TenChiHoi == value);
                                if (exist.Count() > 0)
                                {
                                    data.MaChiHoi = exist.First().MaChiHoi;
                                }
                                else
                                {
                                    ChiHoi chiHoi = new ChiHoi { MaChiHoi = Guid.NewGuid(), TenChiHoi = value };
                                    data.MaChiHoi = chiHoi.MaChiHoi;
                                    import.chiHois.Add(chiHoi);
                                    chiHois.Add(chiHoi);
                                }

                            }
                            break;
                        
                        case 11:
                            //Hộ khẩu thường trú
                            data.HoKhauThuongTru = value;
                            break;
                        case 14:
                            // Nơi ở hiện nay
                            data.ChoOHienNay = value;
                            break;
                        case 17:
                            // Số điện thoại

                            if (string.IsNullOrEmpty(value) || value == "")
                            {
                                //import.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.SoDienThoai), index);
                            }
                            else
                            {
                                data.SoDienThoai = value;

                            }
                            break;
                        case 18:
                            // Ngày vào Đảng NgayvaoDangDuBi
                            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                data.DangVien = true;
                                try
                                {
                                    //data.NgayvaoDangDuBi = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US")); ;
                                    data.NgayvaoDangDuBi = value;
                                }
                                catch (Exception)
                                {

                                    //import.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayVaoDangChinhThuc, value, index);
                                }
                            }
                            break;
                        case 19:
                            // Ngày vào Đảng
                            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                data.DangVien = true;
                                try
                                {
                                    //data.NgayVaoDangChinhThuc = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US")); ;
                                    data.NgayVaoDangChinhThuc = value;
                                }
                                catch (Exception)
                                {

                                    //import.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayVaoDangChinhThuc, value, index);
                                }
                            }
                            break;

                        case 20:
                            // Dân tộc
                            if (string.IsNullOrEmpty(value))
                            {
                                data.MaDanToc = "KH";
                                //import.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.DanToc), index);
                            }
                            else
                            {
                                var obj = _context.DanTocs.FirstOrDefault(it => it.TenDanToc == value);
                                if (obj != null)
                                {
                                    data.MaDanToc = obj.MaDanToc;
                                }
                                else
                                {
                                    import.Error += string.Format("Không tìm thấy dân tộc có tên {0} ở dòng số {1} !", value, index);
                                }

                            }
                            break;
                        case 21:
                            // Tôn giáo

                            if (string.IsNullOrEmpty(value))
                            {
                                data.MaTonGiao = "KH";
                                //import.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.TonGiao), index);
                            }
                            else
                            {
                                var obj = _context.TonGiaos.FirstOrDefault(it => it.TenTonGiao == value);
                                if (obj != null)
                                {
                                    data.MaTonGiao = obj.MaTonGiao;
                                }
                                else
                                {
                                    import.Error += string.Format("Không tìm thấy tôn giáo có tên {0} ở dòng số {1} !", value, index);
                                }

                            }
                            break;
                        case 22:
                            // Trình độ học vấn
                            if (string.IsNullOrEmpty(value))
                            {
                                //import.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.TrinhDoHocVan), index);
                            }
                            else
                            {
                                var obj = _context.TrinhDoHocVans.FirstOrDefault(it => it.TenTrinhDoHocVan == value);
                                if (obj != null)
                                {
                                    data.MaTrinhDoHocVan = obj.MaTrinhDoHocVan;
                                }
                                else
                                {
                                    import.Error += string.Format("Không tìm thấy trình độ học vấn có tên {0} ở dòng số {1} !", value, index);
                                }

                            }
                            break;
                        case 23:
                            //  Chuyên môn (Trung cấp: TC  Cao đẳng: CĐ  Đại học: ĐH Sau ĐH: SĐH)

                            if (string.IsNullOrEmpty(value))
                            {
                                //import.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.TrinhDoChuyenMon), index);
                            }
                            else
                            {
                                var obj = _context.TrinhDoChuyenMons.FirstOrDefault(it => it.TenTrinhDoChuyenMon == value.Trim());
                                if (obj != null)
                                {
                                    data.MaTrinhDoChuyenMon = obj.MaTrinhDoChuyenMon;
                                }
                                else
                                {
                                    data.ChuyenNganh = value;
                                    //import.Error += string.Format("Không tìm thấy trình độ chuyên môn có tên {0} ở dòng số {1} !", value, index);
                                }

                            }
                            break;
                        case 24:
                            // Chính trị
                            if (!string.IsNullOrEmpty(value))
                            {
                                var obj = _context.TrinhDoChinhTris.FirstOrDefault(it => it.TenTrinhDoChinhTri == value);
                                if (obj != null)
                                {
                                    data.MaTrinhDoChinhTri = obj.MaTrinhDoChinhTri;
                                }
                                //else
                                //{
                                //    import.Error += string.Format("Không tìm thấy trình độ chính trị có tên {0} ở dòng số {1} !", tenChinhTri, index);
                                //}
                            }
                            break;
                        case 25:
                            //  Ngày vào hội (*)
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                               
                                try
                                {

                                    data.NgayVaoHoi = DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {

                                    import.Error += string.Format("Ngày vào hội dòng {0} không hợp lệ", index);
                                }
                            }
                            break;
                        case 26:
                            data.NgayThamGiaCapUyDang = value;
                            break;
                        case 27:

                            data.NgayThamGiaHDND = value;
                            break;
                        case 28:
                            data.VaiTro = !String.IsNullOrWhiteSpace(value) ? "1" : null; ;
                            break;
                        case 29:
                            //  MaTrinhDoChinhTri (*)
                            data.VaiTro = !String.IsNullOrWhiteSpace(value) ? "2" : data.VaiTro;
                            data.VaiTroKhac = value;
                            break;
                        case 30:
                            //  Hộ nghèo  (*)
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.MaGiaDinhThuocDien = "01";
                            }
                           
                            break;
                        case 31:
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.MaGiaDinhThuocDien = "02";
                            }
                            break;
                        case 32:
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.MaGiaDinhThuocDien = "03";
                            }
                            break;
                        case 33:
                            //  khac (*)
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.MaGiaDinhThuocDien = "04";
                                data.GiaDinhThuocDienKhac = value;
                            }
                        
                            break;
                        case 34:
                            //  nongdan (*)
                            data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "01" : data.MaNgheNghiep;
                            break;
                        case 35:
                            //  congnhan (*)
                            data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "02" : data.MaNgheNghiep;
                            break;
                        case 36:
                            //  công chức viên chức (*)

                            data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "03" : data.MaNgheNghiep;
                            break;
                        case 37:
                            //Hưu trí
                            data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "04" : data.MaNgheNghiep;
                            break;
                        case 38:
                            //Doanh nghiệp
                            data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "05" : data.MaNgheNghiep;
                            break;
                        case 39:
                            //  Lao động tự do
                            data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "06" : data.MaNgheNghiep;
                            break;
                        case 40:
                            //  Học sinh, sinh viên
                            data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "07" : data.MaNgheNghiep;
                            break;
                        case 41:
                            // Ngành nghề sản xuất chủ lực

                            data.Loai_DV_SX_ChN = value;
                            break;

                        case 42:
                            //  Ngành nghề sản xuất chủ lực

                            data.SoLuong = value;
                            break;
                        case 43:
                            //  Ngành nghề sản xuất chủ lực

                            data.DienTich_QuyMo = value;
                            break;
                        case 44:
                            //  Hiện tham gia sinh hoạt  đoàn thể chính trị, Hội đoàn nào khác

                            data.ThamGia_SH_DoanThe_HoiDoanKhac = value;
                            break;
                        case 45:
                            //  Tham gia câu lạc bộ, đội nhóm, mô hình, hợp tác xã, tổ hợp tác

                            data.ThamGia_CLB_DN_MH_HTX_THT = value;
                            break;
                        case 46:
                            //  Tham gia tổ hội ngành nghề, chi hội ngành nghề
                            data.ThamGia_THNN_CHNN = value;
                            break;
                        case 47:
                            //  HV nòng cốt
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.HoiVienNongCot = true;
                            }
                            break;
                        case 48:
                            //  HV ưu tú năm nào
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.HoiVienUuTuNam = value;
                                if (!String.IsNullOrWhiteSpace(value))
                                {
                                    QuaTrinhKhenThuong add = new QuaTrinhKhenThuong
                                    {
                                        IDCanBo = data.IDCanBo,
                                        MaHinhThucKhenThuong = "01",
                                        MaDanhHieuKhenThuong = "14",
                                        SoQuyetDinh = "",
                                        NguoiKy = "",
                                        IsHoiVien = true,
                                        GhiChu = value,
                                        IDQuaTrinhKhenThuong = Guid.NewGuid()

                                    };
                                    listDanhHieu.Add(add);

                                }
                            }
                            break;
                        case 49:
                            //  HV danh dự
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.HoiVienDanhDu = true;
                            }
                            break;
                        case 50:
                            //NDSXKDG (cấp cơ sở, huyện,Tp, TW năm nào)
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                QuaTrinhKhenThuong add = new QuaTrinhKhenThuong
                                {
                                    IDCanBo = data.IDCanBo,
                                    MaHinhThucKhenThuong = "01",
                                    MaDanhHieuKhenThuong = "15",
                                    SoQuyetDinh = "",
                                    NguoiKy = "",
                                    IsHoiVien = true,
                                    GhiChu = value,
                                    IDQuaTrinhKhenThuong = Guid.NewGuid()

                                };
                                listDanhHieu.Add(add);

                            }
                            break;
                        case 51:
                            //ND tiêu biểu (năm nào)
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                QuaTrinhKhenThuong add = new QuaTrinhKhenThuong
                                {
                                    IDCanBo = data.IDCanBo,
                                    MaHinhThucKhenThuong = "01",
                                    MaDanhHieuKhenThuong = "16",
                                    SoQuyetDinh = "",
                                    NguoiKy = "",
                                    GhiChu = value,
                                    IsHoiVien = true,
                                    IDQuaTrinhKhenThuong = Guid.NewGuid()

                                };
                                listDanhHieu.Add(add);

                            }
                            break;
                        case 52:
                            //ND Việt Nam xuất sắc
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                QuaTrinhKhenThuong add = new QuaTrinhKhenThuong
                                {
                                    IDCanBo = data.IDCanBo,
                                    MaHinhThucKhenThuong = "01",
                                    MaDanhHieuKhenThuong = "22",
                                    SoQuyetDinh = "",
                                    NguoiKy = "",
                                    GhiChu = value,
                                    IsHoiVien = true,
                                    IDQuaTrinhKhenThuong = Guid.NewGuid()

                                };
                                listDanhHieu.Add(add);

                            }
                            break;
                        case 53:
                            //Kỷ niệm chương vì GCND
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                QuaTrinhKhenThuong add = new QuaTrinhKhenThuong
                                {
                                    IDCanBo = data.IDCanBo,
                                    MaHinhThucKhenThuong = "01",
                                    MaDanhHieuKhenThuong = "17",
                                    SoQuyetDinh = "",
                                    NguoiKy = "",
                                    GhiChu = value,
                                    IsHoiVien = true,
                                    IDQuaTrinhKhenThuong = Guid.NewGuid()

                                };
                                listDanhHieu.Add(add);
                            }

                            break;
                        case 54:
                            //Cán bộ Hội cơ sở giỏi
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                QuaTrinhKhenThuong add = new QuaTrinhKhenThuong
                                {
                                    IDCanBo = data.IDCanBo,
                                    MaHinhThucKhenThuong = "01",
                                    MaDanhHieuKhenThuong = "18",
                                    SoQuyetDinh = "",
                                    NguoiKy = "",
                                    GhiChu = value,
                                    IsHoiVien = true,
                                    IDQuaTrinhKhenThuong = Guid.NewGuid()

                                };
                                listDanhHieu.Add(add);

                            }
                            break;
                        case 55:
                            //Giải thưởng sáng tạo nhà nông
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                QuaTrinhKhenThuong add = new QuaTrinhKhenThuong
                                {
                                    IDCanBo = data.IDCanBo,
                                    MaHinhThucKhenThuong = "01",
                                    MaDanhHieuKhenThuong = "19",
                                    SoQuyetDinh = "",
                                    NguoiKy = "",
                                    GhiChu = value,
                                    IsHoiVien = true,
                                    IDQuaTrinhKhenThuong = Guid.NewGuid()

                                };
                                listDanhHieu.Add(add);

                            }
                            break;
                        case 56:
                            //Gương điển hình tiên tiến
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                QuaTrinhKhenThuong add = new QuaTrinhKhenThuong
                                {
                                    IDCanBo = data.IDCanBo,
                                    MaHinhThucKhenThuong = "01",
                                    MaDanhHieuKhenThuong = "13",
                                    SoQuyetDinh = "",
                                    NguoiKy = "",
                                    GhiChu = value,
                                    IsHoiVien = true,
                                    IDQuaTrinhKhenThuong = Guid.NewGuid()

                                };
                                listDanhHieu.Add(add);

                            }
                            break;
                        case 57:
                            //Gương Dân vận khéo
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                QuaTrinhKhenThuong add = new QuaTrinhKhenThuong
                                {
                                    IDCanBo = data.IDCanBo,
                                    MaHinhThucKhenThuong = "01",
                                    MaDanhHieuKhenThuong = "20",
                                    SoQuyetDinh = "",
                                    NguoiKy = "",
                                    GhiChu = value,
                                    IsHoiVien = true,
                                    IDQuaTrinhKhenThuong = Guid.NewGuid()

                                };
                                listDanhHieu.Add(add);

                            }
                            break;
                        case 58:
                            //Gương điển hình học tập và làm theo Bác
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                QuaTrinhKhenThuong add = new QuaTrinhKhenThuong
                                {
                                    IDCanBo = data.IDCanBo,
                                    MaHinhThucKhenThuong = "01",
                                    MaDanhHieuKhenThuong = "21",
                                    SoQuyetDinh = "",
                                    NguoiKy = "",
                                    GhiChu = value,
                                    IsHoiVien = true,
                                    IDQuaTrinhKhenThuong = Guid.NewGuid()

                                };
                                listDanhHieu.Add(add);

                            }
                            break;
                        case 59:
                            //  Hỗ trợ Vay vốn (nguồn vốn)
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.HoTrovayVon = value;
                            }
                            break;
                        case 60:
                            //  Hỗ trợ hình thức khác
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.HoTroKhac = value;
                            }
                            break;
                        case 61:
                            //  Hỗ trợ đào tạo nghề
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.HoTroDaoTaoNghe = value;
                            }
                            break;
                        case 62:
                            //  Ghi chú
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.GhiChu = value;
                            }
                            break;

                        case 63:
                            //  Chi Hội Dân Cư CHT
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.ChiHoiDanCu_CHT = value;
                            }
                            break;
                        case 64:
                            // Chi Hội Dân Cư CHP
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.ChiHoiDanCu_CHT = value;
                            }
                            break;
                        case 65:
                            //  Chi hội nghề nghiệp CHT
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.ChiHoiNgheNghiep_CHP = value;
                            }
                            break;
                        case 66:
                            //  Chi hội nghề nghiệp CHP
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.ChiHoiNgheNghiep_CHT = value;
                            }
                            break;

                        case 67:
                            //  Ngày Rời hội
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                try
                                {
                                    data.NgayRoiHoi = DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    data.isRoiHoi = true;
                                }
                                catch (Exception)
                                {

                                    import.Error += "Ngày rời hội không hợp lệ dòng " + index.ToString();
                                }
                               
                               
                            }
                            break;
                        case 68:
                            //  Lý do rời hội
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.LyDoRoiHoi = value;
                            }
                            break;

                    }
                }
                catch (Exception ex)
                {
                    string ss = i.ToString() + value.ToString() + ex.Message;
                }
            }
            import.ListKhenThuong = listDanhHieu;
            import.CanBo = data;

            return import;
        }
        private List<FileDinhKemModel> AdFiles(IFormFile?[] filesDinhKem,CanBo canBo) {
            List< FileDinhKemModel> addFiles = new List<FileDinhKemModel>();
            foreach (var file in filesDinhKem)
            {
                FileDinhKemModel addFile = new FileDinhKemModel();
                addFile.Id = canBo.IDCanBo;
                addFile.IdCanBo = canBo.IDCanBo;
                addFile.Key = Guid.NewGuid();
                addFile.IDLoaiDinhKem = "99";
                FunctionFile.CopyFile(_hostEnvironment, file!, addFile);
                if (!String.IsNullOrEmpty(addFile.Error) && !String.IsNullOrWhiteSpace(addFile.Error))
                {
                    ModelState.AddModelError("fileInbox", addFile.Error);
                    break;
                }
                addFiles.Add(addFile);
            }
            return addFiles;
        }

        public IActionResult ConvertNgayVaoHoi() {
            var hoiviens = _context.CanBos.Where(it => it.IsHoiVien == true && it.SoQuyetDinhBoNhiem != null && it.SoQuyetDinhBoNhiem != "" );
            DateTime? ngayVaoHoi;
            int capNhat = 0;
            foreach (var item in hoiviens)
            {
                //item.SoQuyetDinhBoNhiem = item.NgayVaoHoi;
                ngayVaoHoi = Function.ConvertStringToDateV1(item.SoQuyetDinhBoNhiem!);
                if (ngayVaoHoi != null)
                {
                    item.NgayVaoHoi = ngayVaoHoi;
                    capNhat++;
                }
                else item.NgayVaoHoi = null;
            }
            _context.SaveChanges();
            return Content("Thành công " +capNhat.ToString());
        }
        private void CreateViewBagImport1()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }

        #endregion Check data type 
    }
}
