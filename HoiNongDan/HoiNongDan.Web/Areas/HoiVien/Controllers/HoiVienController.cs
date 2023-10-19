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

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HoiVienController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        private string[] DateFomat;
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 6;
        public HoiVienController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config, IHttpContextAccessor httpContext) : base(context)
        {
            _hostEnvironment = hostEnvironment;
            DateFomat = config.GetSection("SiteSettings:DateFormat").Value.ToString().Split(',');
            _httpContext = httpContext;
        }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {

            CreateViewBagSearch();
            return View();
        }
        public IActionResult _Search(HoiVienSearchVM search)
        {
            return ExecuteSearch(() => {
                var model = _context.CanBos.Where(it=>it.IsHoiVien == true ).AsQueryable();
                if (!String.IsNullOrEmpty(search.MaCanBo))
                {
                    model = model.Where(it => it.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen))
                {
                    model = model.Where(it => it.HoVaTen.Contains(search.HoVaTen));
                }

                if (search.MaDiaBanHoatDong != null)
                {
                    model = model.Where(it => it.MaDiaBanHoatDong == search.MaDiaBanHoatDong);
                }
                //else
                //{

                //    model = model.Where(it => it.MaDiaBanHoatDong == Guid.Parse("4633016E-727F-4B69-A5D2-0DF7F42AA345"));
                //}
                if (search.MaQuanHuyen != null)
                {
                    model = model.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
                }
                if (search.Actived != null)
                {
                    model = model.Where(it => it.Actived == search.Actived);
                }
                if (search.DangChoDuyet == null || search.DangChoDuyet == true)
                {
                    model = model.Where(it => it.HoiVienDuyet == true);
                }
                else
                {
                    model = model.Where(it => it.HoiVienDuyet !=true && it.CreatedAccountId == AccountId());
                   
                }
                var data = model
                    .Include(it => it.NgheNghiep)
                    .Include(it => it.DiaBanHoatDong)
                    .Include(it => it.DanToc)
                    .Include(it => it.TonGiao)
                    .Include(it => it.TrinhDoHocVan)
                    .Include(it => it.TrinhDoChuyenMon)
                    .Include(it => it.TrinhDoChinhTri)
                    .Include(it=>it.QuaTrinhKhenThuongs)
                    .Include(it=>it.ChiHoi)
                    .Include(it=>it.ToHoi)
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
                        DangVien = it.DangVien == null ? false : it.DangVien.Value,
                        NgayvaoDangDuBi = it.NgayvaoDangDuBi,
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
                        VaiTro = it.VaiTro == "01" ? "Chủ hộ" : "Quan hệ chủ hộ",
                        VaiTroKhac = it.VaiTroKhac,
                        HoNgheo = it.HoNgheo==null?false : it.HoNgheo.Value,
                        CanNgheo = it.CanNgheo == null ? false : it.CanNgheo.Value,
                        GiaDinhChinhSach = it.GiaDinhChinhSach == null ? false : it.GiaDinhChinhSach.Value,
                        GiaDinhThuocDienKhac = it.GiaDinhThuocDienKhac,
                        NgheNghiepHienNay = it.NgheNghiep!.TenNgheNghiep,
                        Loai_DV_SX_ChN = it.Loai_DV_SX_ChN,
                        DienTich_QuyMo = it.DienTich_QuyMo,
                        SoLuong = it.SoLuong,
                        LoaiHoiVien = it.LoaiHoiVien,
                        ThamGia_SH_DoanThe_HoiDoanKhac = it.ThamGia_SH_DoanThe_HoiDoanKhac,
                        ThamGia_CLB_DN_MH_HTX_THT = it.ThamGia_CLB_DN_MH_HTX_THT,
                        ThamGia_THNN_CHNN = it.ThamGia_THNN_CHNN,
                        HoiVienNongCot = it.HoiVienNongCot == null ? false : it.HoiVienNongCot.Value,
                        HoiVienUuTuNam = it.HoiVienUuTuNam,
                        HoiVienDanhDu = it.HoiVienDanhDu == null ? false : it.HoiVienDanhDu.Value,
                        
                        NDSXKDG = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "15").Select(it => it.GhiChu).ToList()),
                        NDTieuBieu = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "16").Select(it => it.GhiChu).ToList()),
                        NDVietnamXS = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "22").Select(it => it.GhiChu).ToList()),
                        KNCGCND = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "17").Select(it => it.GhiChu).ToList()),
                        CanBoHoiCoSoGioi = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "18").Select(it => it.GhiChu).ToList()),
                        SangTaoNhaNong = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "19").Select(it => it.GhiChu).ToList()),
                        GuongDiemHinh = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "13").Select(it => it.GhiChu).ToList()),
                        GuongDanVanKheo = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "20").Select(it => it.GhiChu).ToList()),
                        GuongDiemHinhHocTapLamTheoBac = String.Join(',', it.QuaTrinhKhenThuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaDanhHieuKhenThuong == "21").Select(it => it.GhiChu).ToList()),
                        HoTrovayVon  = it.HoTrovayVon,
                        HoTroKhac = it.HoTroKhac,
                        HoTroDaoTaoNghe = it.HoTroDaoTaoNghe,
                      
                        KKAnToanThucPham = it.KKAnToanThucPham,
                        DKMauNguoiNongDanMoi = it.DKMauNguoiNongDanMoi,
                        ChiHoi = it.ChiHoi!.TenChiHoi,
                        ToHoi = it.ToHoi!.TenToHoi,
                        GhiChu = it.GhiChu,
                    }).ToList();
                return PartialView(data);
            });
        }
        [HttpGet]
        public JsonResult LoadBigData(HoiVienSearchVM search)
        {
            var model = _context.CanBos.Where(it => it.IsHoiVien == true).AsQueryable();
            if (!String.IsNullOrEmpty(search.MaCanBo))
            {
                model = model.Where(it => it.MaCanBo == search.MaCanBo);
            }
            if (!String.IsNullOrEmpty(search.HoVaTen))
            {
                model = model.Where(it => it.HoVaTen.Contains(search.HoVaTen));
            }

            if (search.MaDiaBanHoatDong != null)
            {
                model = model.Where(it => it.MaDiaBanHoatDong == search.MaDiaBanHoatDong);
            }
            //else
            //{

            //    model = model.Where(it => it.MaDiaBanHoatDong == Guid.Parse("4633016E-727F-4B69-A5D2-0DF7F42AA345"));
            //}
            if (search.MaQuanHuyen != null)
            {
                model = model.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
            }
            if (search.Actived != null)
            {
                model = model.Where(it => it.Actived == search.Actived);
            }
            if (search.DangChoDuyet == null || search.DangChoDuyet == true)
            {
                model = model.Where(it => it.HoiVienDuyet == true);
            }
            else
            {
                model = model.Where(it => it.HoiVienDuyet != true && it.CreatedAccountId == AccountId());
            }
            var data = model
                .Include(it => it.NgheNghiep)
                .Include(it => it.DiaBanHoatDong)
                .Include(it => it.DanToc)
                .Include(it => it.TonGiao)
                .Include(it => it.TrinhDoHocVan)
                .Include(it => it.TrinhDoChuyenMon)
                .Include(it => it.TrinhDoChinhTri)
                .Include(it => it.QuaTrinhKhenThuongs)
                .Include(it => it.ChiHoi)
                .Include(it => it.ToHoi)
                .Include(it => it.CoSo).Select(it => new 
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

                    TenDiaBanHoatDong = it.DiaBanHoatDong!.TenDiaBanHoatDong,
                    DanToc = it.DanToc!.TenDanToc,
                    TonGiao = it.TonGiao!.TenTonGiao,
                    TrinhDoHocvan = it.TrinhDoHocVan.TenTrinhDoHocVan,
                    MaTrinhDoChuyenMon = it.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                    MaTrinhDoChinhTri = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                    NgayVaoHoi = it.NgayVaoHoi,
                    NgayThamGiaCapUyDang = it.NgayThamGiaCapUyDang,
                    NgayThamGiaHDND = it.NgayThamGiaHDND,
                    VaiTro = it.VaiTro == "1" ? "Chủ hộ" : "Quan hệ chủ hộ",
                    VaiTroKhac = it.VaiTroKhac,
                    HoNgheo = it.HoNgheo == true ? "X" : "",
                    CanNgheo = it.CanNgheo == true ? "X" : "",
                    GiaDinhChinhSach = it.GiaDinhChinhSach == true ? "X" : "",
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
                    HoTrovayVon = it.HoTrovayVon,
                    HoTroKhac = it.HoTroKhac,
                    HoTroDaoTaoNghe = it.HoTroDaoTaoNghe,

                    KKAnToanThucPham = it.KKAnToanThucPham,
                    DKMauNguoiNongDanMoi = it.DKMauNguoiNongDanMoi,
                    ChiHoi = it.ChiHoi!.TenChiHoi,
                    ToHoi = it.ToHoi!.TenToHoi,
                    GhiChu = it.GhiChu,
                }).ToList();
            var json = Json(data);
            return json;
        }
        public IActionResult Demo() {
            CreateViewBagSearch();
            return View();
        }
        #endregion Index
        #region Create
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Create()
        {
            CreateViewBag();
            HoiVienVM obj = new HoiVienVM();
            
            obj.HinhAnh = @"\images\login.png";
            return View(obj);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Create(HoiVienMTVM insert, IFormFile? avtFileInbox)
        {
            CheckError(insert);
            return ExecuteContainer(() => {
                CanBo add = new CanBo();
                insert.GetHoiVien(add);
                add.IDCanBo = Guid.NewGuid();
                add.Actived = true;
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                add.HoiVienDuyet = false;
                if (avtFileInbox != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = add.MaCanBo;
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
            var item = _context.CanBos.SingleOrDefault(it => it.IDCanBo == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            HoiVienVM edit = HoiVienMTVM.SetHoiVien(item);
            CreateViewBag(item.IdCoSo, item.IdDepartment, item.MaChucVu,item.MaTrinhDoHocVan,
                item.MaTrinhDoChinhTri,item.MaDanToc,item.MaTonGiao,item.MaTrinhDoChuyenMon,item.MaDiaBanHoatDong,item.MaHocVi);
            return View(edit);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        public JsonResult Edit(HoiVienMTVM obj, IFormFile? avtFileInbox)
        {

            CheckError(obj);
            return ExecuteContainer(() => {
                var edit = _context.CanBos.SingleOrDefault(it => it.IDCanBo == obj.IDCanBo);
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
                edit.NgayNgungHoatDong = obj.NgayNgungHoatDong;
                edit.LyDoNgungHoatDong = obj.LyDoNgungHoatDong;
                edit.LastModifiedTime = DateTime.Now;
                edit.LastModifiedAccountId = AccountId();
                if (avtFileInbox != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = edit.MaCanBo;
                    var uploads = Path.Combine(wwwRootPath, @"images\canbo");

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
                    edit.HinhAnh = @"\images\canbo\" + fileName + extension;
                }
                HistoryModelRepository history = new HistoryModelRepository(_context);
                history.SaveUpdateHistory(edit.IDCanBo.ToString(), AccountId()!.Value, edit);
                _context.Entry(edit).State = EntityState.Modified;
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
                     hoivien = _context.CanBos.Where(it => it.MaCanBo == MaHoiVien && it.HoiVienDuyet == true && it.Actived == true && it.IsHoiVien ==true && it.MaDiaBanHoatDong == MaDiaBanHoatDong).Include(it => it.TinhTrang)
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
        public JsonResult Delete(Guid id)
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
        public IActionResult _Import()
        {
            CreateViewBagImport();
            return PartialView();
        }
        [HoiNongDanAuthorization]
        public IActionResult Import(Guid? MaDiaBanHoatDong)
        {
            if (MaDiaBanHoatDong == null) {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = false,
                    Data = "Chưa chọn địa bàn hội muốn Import"
                }); ;
            }
            DataSet ds = GetDataSetFromExcel();
            List<string> errorList = new List<string>();
            var chiHois = _context.ChiHois.ToList();
            var toHois = _context.ToHois.ToList();
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
                            if (contCode == controllerCode)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    //string aa = dr.ItemArray[0].ToString();
                                    if (dt.Rows.IndexOf(dr) >= startIndex)
                                    {
                                        if (!string.IsNullOrEmpty(dr.ItemArray[0]!.ToString()))
                                        {
                                            var data = CheckTemplate(dr.ItemArray!, chiHois, toHois);
                                            if (!string.IsNullOrEmpty(data.Error))
                                            {
                                                errorList.Add(data.Error);
                                            }
                                            else
                                            {
                                                // Tiến hành cập nhật
                                                data.CanBo.MaDiaBanHoatDong = MaDiaBanHoatDong;

                                                string result = ExecuteImportExcelMenu(data);
                                                if (result != LanguageResource.ImportSuccess)
                                                {
                                                    errorList.Add(result);
                                                }
                                            }
                                        }
                                        else
                                            break;
                                        //Check correct template

                                    }
                                }
                            }
                            //else
                            //{
                            //    string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.HoiVien);
                            //    errorList.Add(error);
                            //}
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
                            Data = LanguageResource.ImportSuccess
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
                if (Request.Form.Files.Count > 0)
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
                }
                return ds;
            }
            catch
            {
                return null;
            }
        }
        #endregion Import Excel
        #region Export Data
        public IActionResult ExportCreate()
        {
            List<HoiVienExcelVM> data = new List<HoiVienExcelVM>();
            return Export(data);
        }
        public IActionResult ExportEdit(HoiVienSearchVM search)
        {
            var model = _context.CanBos.Where(it => it.IsHoiVien == true).AsQueryable();
            if (!String.IsNullOrEmpty(search.MaCanBo))
            {
                model = model.Where(it => it.MaCanBo == search.MaCanBo);
            }
            if (!String.IsNullOrEmpty(search.HoVaTen))
            {
                model = model.Where(it => it.HoVaTen.Contains(search.HoVaTen));
            }

            if (search.MaDiaBanHoatDong != null)
            {
                model = model.Where(it => it.MaDiaBanHoatDong == search.MaDiaBanHoatDong);
            }
            if (search.MaChucVu != null)
            {
                model = model.Where(it => it.MaChucVu == search.MaChucVu);
            }
            if (search.Actived != null)
            {
                model = model.Where(it => it.Actived == search.Actived);
            }
            if (search.DangChoDuyet == null || search.DangChoDuyet == true)
            {
                model = model.Where(it => it.HoiVienDuyet == true);
            }
            else
            {
                model = model.Where(it => it.HoiVienDuyet != true && it.CreatedAccountId == AccountId());

            }

            var data = model.Include(it => it.TrinhDoHocVan)
                .Include(it => it.TrinhDoChuyenMon)
                .Include(it => it.TrinhDoChinhTri)
                .Include(it => it.DanToc)
                .Include(it => it.TonGiao)
                .Include(it => it.ChiHoi)
                .Include(it => it.ToHoi)
                .Select(item => new HoiVienExcelVM
                {
                    IDCanBo = item.IDCanBo,
                    MaCanBo = item.MaCanBo,
                    HoVaTen = item.HoVaTen,
                    NgaySinh = item.NgaySinh,
                    GioiTinh = item.GioiTinh == GioiTinh.Nam ? true : false,
                    SoCCCD = item.SoCCCD,
                    // NgayCapCCCD = item.NgayCapCCCD,
                    HoKhauThuongTru = item.HoKhauThuongTru,
                    ChoOHienNay = item.ChoOHienNay!,
                    SoDienThoai = item.SoDienThoai,
                    //NgayvaoDangDuBi = item.NgayvaoDangDuBi,
                    NgayVaoDangChinhThuc = item.NgayVaoDangChinhThuc,
                    MaDanToc = item.DanToc.TenDanToc,
                    MaTonGiao = item.TonGiao.TenTonGiao,
                    MaTrinhDoHocVan = item.TrinhDoHocVan.TenTrinhDoHocVan,
                    MaTrinhDoChuyenMon = item.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                    MaTrinhDoChinhTri = item.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                    NgayVaoHoi = item.NgayVaoHoi,
                    NgayThamGiaCapUyDang = item.NgayThamGiaCapUyDang,
                    NgayThamGiaHDND = item.NgayThamGiaHDND,
                    VaiTro = item.VaiTro == "1" ? "X" : "",
                    VaiTroKhac = item.VaiTroKhac,
                    HoNgheo = item.HoNgheo == true ? "X" : "",
                    CanNgheo = item.CanNgheo == true ? "X" : "",
                    GiaDinhChinhSach = item.GiaDinhChinhSach == true ? "X" : "",
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
                    TenChiHoi = item.ChiHoi.TenChiHoi,
                    TenToHoi = item.ToHoi.TenToHoi,
                    ChiHoiDanCu_CHT = item.ChiHoiDanCu_CHT,
                    ChiHoiDanCu_CHP = item.ChiHoiDanCu_CHP,
                    ChiHoiNgheNghiep_CHT = item.ChiHoiNgheNghiep_CHT,
                    ChiHoiNgheNghiep_CHP = item.ChiHoiNgheNghiep_CHP,
                }).ToList();
            return Export(data);
        }
        public FileContentResult Export(List<HoiVienExcelVM> menu)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "IDCanBo", isAllowedToEdit = false, isText = true });
            
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinh", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GioiTinh", isBoolean = true, isComment = true, strComment = "Nam để chữ X" });
            columns.Add(new ExcelTemplate() { ColumnName = "SoCCCD", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "MaCanBo", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoKhauThuongTru", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoDienThoai", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayvaoDangDuBi", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoDangChinhThuc", isAllowedToEdit = true, isDateTime = true });
           
            var danToc = _context.DanTocs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaDanToc, Name = x.TenDanToc }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaDanToc", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = danToc, TypeId = ConstExcelController.StringId });
           
            var tonGiao = _context.TonGiaos.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTonGiao, Name = x.TenTonGiao }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTonGiao", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tonGiao, TypeId = ConstExcelController.StringId });
           
            var hocVan = _context.TrinhDoHocVans.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoHocVan, Name = x.TenTrinhDoHocVan }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoHocVan", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = hocVan, TypeId = ConstExcelController.StringId });
            
            var chuyenNganh = _context.TrinhDoChuyenMons.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChuyenMon, Name = x.TenTrinhDoChuyenMon }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChuyenMon", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chuyenNganh, TypeId = ConstExcelController.StringId });
            
            var chinhTri = _context.TrinhDoChinhTris.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChinhTri, Name = x.TenTrinhDoChinhTri }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChinhTri", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chinhTri, TypeId = ConstExcelController.StringId });
           
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
            columns.Add(new ExcelTemplate() { ColumnName = "TenChiHoi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "TenToHoi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChiHoiDanCu_CHT", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChiHoiDanCu_CHP", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChiHoiNgheNghiep_CHT", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChiHoiNgheNghiep_CHP", isAllowedToEdit = true, isText = true });
           


            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.HoiVien) ;
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
            byte[] filecontent = ClassExportExcel.ExportExcel(menu, columns, heading, true);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export Data
        #region Print the hoi vien
        [HttpPost]
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
                    NgayVaoHoi = it.NgayVaoHoi!,
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
                array = item.NgayVaoHoi.Replace(@"\", "/").Split("/");
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
            if (insert.Actived == false)
            {
                if ((String.IsNullOrEmpty(insert.LyDoNgungHoatDong) || String.IsNullOrWhiteSpace(insert.LyDoNgungHoatDong)))
                {
                    ModelState.AddModelError("LyDoNgungHoatDong", "Lý do ngưng hoạt động chưa nhập");
                }
                if (insert.NgayNgungHoatDong == null)
                {
                    ModelState.AddModelError("NgayNgungHoatDong", "Ngày ngưng hoạt động chưa nhập");
                }
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
            String? maDanToc = null, String? maTonGiao = null,string? maTrinhDoChuyenMon = null,Guid? maDiaBanHoatDong = null,string? maHocVi = null,Guid? MaChiHoi = null, Guid? MaToHoi = null)
        {

            //var MenuListCoSo = _context.CoSos.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { IdCoSo = it.IdCoSo, TenCoSo = it.TenCoSo }).ToList();
            //ViewBag.IdCoSo = new SelectList(MenuListCoSo, "IdCoSo", "TenCoSo", IdCoSo);

            //var DonVi = _context.Departments.Where(it => it.Actived == true).Include(it => it.CoSo).OrderBy(p => p.OrderIndex).Select(it => new { IdDepartment = it.Id, Name = it.Name + " " + it.CoSo.TenCoSo }).ToList();
            //ViewBag.IdDepartment = new SelectList(DonVi, "IdDepartment", "Name", IdDepartment);

            var chucVu = _context.ChucVus.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { MaChucVu = it.MaChucVu, TenChucVu = it.TenChucVu }).ToList();
            ViewBag.MaChucVu = new SelectList(chucVu, "MaChucVu", "TenChucVu", maChucVu);



            var trinhDoHocVan = _context.TrinhDoHocVans.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTrinhDoHocVan = it.MaTrinhDoHocVan, TenTrinhDoHocVan = it.TenTrinhDoHocVan }).ToList();
            ViewBag.MaTrinhDoHocVan = new SelectList(trinhDoHocVan, "MaTrinhDoHocVan", "TenTrinhDoHocVan", maTrinhDoHocVan);

            var trinhDoChuyenMon = _context.TrinhDoChuyenMons.Select(it => new { MaTrinhDoChuyenMon = it.MaTrinhDoChuyenMon, TenTrinhDoChuyenMon = it.TenTrinhDoChuyenMon }).ToList();
            ViewBag.MaTrinhDoChuyenMon = new SelectList(trinhDoChuyenMon, "MaTrinhDoChuyenMon", "TenTrinhDoChuyenMon", maTrinhDoChuyenMon);

            var trinhDoChinhTri = _context.TrinhDoChinhTris.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTrinhDoChinhTri = it.MaTrinhDoChinhTri, TenTrinhDoChinhTri = it.TenTrinhDoChinhTri }).ToList();
            ViewBag.MaTrinhDoChinhTri = new SelectList(trinhDoChinhTri, "MaTrinhDoChinhTri", "TenTrinhDoChinhTri", maTrinhDoChinhTri);

            var danToc = _context.DanTocs.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaDanToc = it.MaDanToc, TenDanToc = it.TenDanToc }).ToList();
            ViewBag.MaDanToc = new SelectList(danToc, "MaDanToc", "TenDanToc", maDanToc);

            var tonGiao = _context.TonGiaos.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTonGiao = it.MaTonGiao, TenTonGiao = it.TenTonGiao }).ToList();
            ViewBag.MaTonGiao = new SelectList(tonGiao, "MaTonGiao", "TenTonGiao", maTonGiao);

            var diaBanHoatDong = _context.DiaBanHoatDongs.Where(it => it.Actived == true).Select(it => new { MaDiaBanHoatDong = it.Id, Name = it.TenDiaBanHoatDong  }).ToList();
            ViewBag.MaDiaBanHoatDong = new SelectList(diaBanHoatDong, "MaDiaBanHoatDong", "Name", maDiaBanHoatDong);

            var hocVis = _context.HocVis.Select(it => new { MaHocVi = it.MaHocVi, TenHocVi = it.TenHocVi }).ToList();
            ViewBag.MaHocVi = new SelectList(hocVis, "MaHocVi", "TenHocVi", maHocVi);

            var chiHois = _context.ChiHois.Select(it => new { MaChiHoi = it.MaChiHoi, TenChiHoi = it.TenChiHoi }).ToList();
            ViewBag.MaChiHoi = new SelectList(chiHois, "MaChiHoi", "TenChiHoi", MaChiHoi);

            var toHois = _context.ToHois.Select(it => new { MaToHoi = it.MaToHoi, TenToHoi = it.TenToHoi }).ToList();
            ViewBag.MaToHoi = new SelectList(toHois, "MaToHoi", "TenToHoi", MaToHoi);
        }
        private void CreateViewBagSearch()
        {
            var data = (from hv in _context.CanBos
                        join diaban in _context.DiaBanHoatDongs on hv.MaDiaBanHoatDong equals diaban.Id
                        join quanhuyen in _context.QuanHuyens on diaban.MaQuanHuyen equals quanhuyen.MaQuanHuyen
                        where hv.IsHoiVien == true
                        select new {
                            MaDiaBanHoatDong = diaban.Id,
                            Name = diaban.TenDiaBanHoatDong,
                            MaQuanHuyen = quanhuyen.MaQuanHuyen,
                            TenQuanHuyen = quanhuyen.TenQuanHuyen
                        }
                                  ).Distinct().ToList();

            var diaBanHoatDong = data.Select(it => new { MaDiaBanHoatDong = it.MaDiaBanHoatDong, Name = it.Name }).Distinct().ToList();
            ViewBag.MaDiaBanHoatDong = new SelectList(diaBanHoatDong, "MaDiaBanHoatDong", "Name");

            var quanHuyen = data.Select(it => new { MaQuanHuyen = it.MaQuanHuyen, TenQuanHuyen = it.TenQuanHuyen }).Distinct().ToList();
            ViewBag.MaQuanHuyen = new SelectList(quanHuyen, "MaQuanHuyen", "TenQuanHuyen");

        }
        private void CreateViewBagImport() {
            var diaBanHoatDong = _context.DiaBanHoatDongs.Where(it => it.Actived == true).OrderByDescending(it => it.CreatedTime).Select(it => new { MaDiaBanHoatDong = it.Id, Name = it.TenDiaBanHoatDong }).ToList();
            ViewBag.MaDiaBanHoatDong = new SelectList(diaBanHoatDong, "MaDiaBanHoatDong", "Name");
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
                var data = (from hv in _context.CanBos
                            join diaban in _context.DiaBanHoatDongs on hv.MaDiaBanHoatDong equals diaban.Id
                            where hv.IsHoiVien == true && diaban.Actived == true
                            select new
                            {
                                MaDiaBanHoatDong = diaban.Id,
                                Name = diaban.TenDiaBanHoatDong,
                            }
                                 ).Distinct().ToList();
                return Json(data);
            }
           
        }
        #endregion Helper
        #region Insert/Update data from excel file
        public string ExecuteImportExcelMenu(HoiVienImportExcel HoiVienExcel)
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
                    canbo.HoiVienDuyet = false;
                    canbo.Actived = true;
                    canbo.HoiVienDuyet = true;
                    canbo.Level = "90";
                    canbo.IsHoiVien = true;
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
                catch (Exception ex)
                {

                    
                }
            }
            else
            {
                //canbo = _context.CanBos.Where(p => p.IDCanBo == HoiVienExcel.IDCanBo).FirstOrDefault();
                //if (canbo != null)
                //{
                //    canbo = HoiVienExcel.GetHoiVien(canbo);
                //    HistoryModelRepository history = new HistoryModelRepository(_context);
                //    history.SaveUpdateHistory(canbo.IDCanBo.ToString(), AccountId()!.Value, canbo);
                //}
                //else
                //{
                //    return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                //                            LanguageResource.HoiVien, canbo.IDCanBo,
                //                            string.Format(LanguageResource.Export_ExcelHeader,
                //                            LanguageResource.HoiVien));
                //}
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
        public HoiVienImportExcel CheckTemplate(object[] row,List<ChiHoi> chiHois, List<ToHoi>toHois)
        {
            HoiVienImportExcel import = new HoiVienImportExcel();
            CanBo data = new CanBo();
            data.MaChucVu = Guid.Parse("D710D930-8342-474B-90A4-A1170A7A5691");
            List<QuaTrinhKhenThuong> listDanhHieu = new List<QuaTrinhKhenThuong>();
            string? value;
            int index = 0;
            for (int i = 0; i < row.Length; i++)
            {
                value = row[i] == null ? "" : row[i].ToString().Trim().Replace(System.Environment.NewLine,string.Empty);
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
                            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                            {
                                //import.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0}", LanguageResource.NgaySinh), index);
                            }
                            else
                            {
                                try
                                {
                                    //data.NgaySinh = DateTime.ParseExact(ngaySinh, DateFomat, new CultureInfo("en-US"));
                                    data.NgaySinh = value;
                                }
                                catch (Exception)
                                {

                                    import.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgaySinh, value, index);
                                }

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
                            if (string.IsNullOrEmpty(value))
                            {
                                data.HoKhauThuongTru = "Không có";
                                //import.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.SoCCCD), index);
                            }
                            else
                            {
                                data.SoCCCD = value;

                            }
                            break;
                        case 6:
                            // Mã nhân viên
                            if (string.IsNullOrEmpty(value))
                            {
                                //import.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.MaCanBo), index);
                            }
                            else
                            {
                                data.MaCanBo = value;
                            }
                            break;
                        case 7:
                            //Hộ khẩu thường trú
                            if (string.IsNullOrEmpty(value) || value == "")
                            {
                                data.HoKhauThuongTru = null;
                                //import.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.NgayCapCCCD), index);
                            }
                            else
                            {
                                try
                                {
                                    data.HoKhauThuongTru = value;
                                }
                                catch (Exception)
                                {

                                    import.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayCapCCCD, value, index);
                                }

                            }
                            break;
                        case 8:
                            // Nơi ở hiện nay
                            if (string.IsNullOrEmpty(value))
                            {
                                //import.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.ChoOHienNay), index);
                            }
                            else
                            {
                                data.ChoOHienNay = value;

                            }
                            break;
                        case 9:
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
                        case 10:
                            // Ngày vào Đảng NgayvaoDangDuBi
                            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                data.DangVien = true;
                                try
                                {
                                    //data.NgayvaoDangDuBi = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US")); ;
                                    data.NgayvaoDangDuBi = value ;
                                }
                                catch (Exception)
                                {

                                    //import.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayVaoDangChinhThuc, value, index);
                                }
                            }
                            break;
                        case 11:
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

                        case 12:
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
                        case 13:
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
                        case 14:
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
                        case 15:
                            //  Chuyên môn (Trung cấp: TC  Cao đẳng: CĐ  Đại học: ĐH Sau ĐH: SĐH)

                            if (string.IsNullOrEmpty(value))
                            {
                                //import.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.TrinhDoChuyenMon), index);
                            }
                            else
                            {
                                var obj = _context.TrinhDoChuyenMons.FirstOrDefault(it => it.TenTrinhDoChuyenMon == value);
                                if (obj != null)
                                {
                                    data.MaTrinhDoChuyenMon = obj.MaTrinhDoChuyenMon;
                                }
                                else
                                {
                                    import.Error += string.Format("Không tìm thấy trình độ học vấn có tên {0} ở dòng số {1} !", value, index);
                                }

                            }
                            break;
                        case 16:
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
                        case 17:
                            //  Ngày vào hội (*)

                            data.NgayVaoHoi = value;
                            break;
                        case 18:
                            data.NgayThamGiaCapUyDang = value;
                            break;
                        case 19:

                            data.NgayThamGiaHDND = value;
                            break;
                        case 20:
                            data.VaiTro = !String.IsNullOrWhiteSpace(value) ? "1" : null; ;
                            break;
                        case 21:
                            //  MaTrinhDoChinhTri (*)
                            data.VaiTro = !String.IsNullOrWhiteSpace(value) ? "2" : data.VaiTro;
                            data.VaiTroKhac = value;
                            break;
                        case 22:
                            //  Hộ nghèo  (*)

                            data.HoNgheo = !String.IsNullOrWhiteSpace(value) ? true : false;
                            break;
                        case 23:
                            data.CanNgheo = !String.IsNullOrWhiteSpace(value) ? true : false;
                            break;
                        case 24:
                            data.GiaDinhChinhSach = !String.IsNullOrWhiteSpace(value) ? true : false;
                            break;
                        case 25:
                            //  khac (*)

                            data.GiaDinhThuocDienKhac = value;
                            break;
                        case 26:
                            //  nongdan (*)
                            data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "01" : data.MaNgheNghiep;
                            break;
                        case 27:
                            //  congnhan (*)
                            data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "02" : data.MaNgheNghiep;
                            break;
                        case 28:
                            //  công chức viên chức (*)

                            data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "03" : data.MaNgheNghiep;
                            break;
                        case 29:
                            //Hưu trí
                            data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "04" : data.MaNgheNghiep;
                            break;
                        case 30:
                            //Doanh nghiệp
                            data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "05" : data.MaNgheNghiep;
                            break;
                        case 31:
                            //  Lao động tự do
                            data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "06" : data.MaNgheNghiep;
                            break;
                        case 32:
                            //  Học sinh, sinh viên
                            data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "07" : data.MaNgheNghiep;
                            break;
                        case 33:
                            // Ngành nghề sản xuất chủ lực

                            data.Loai_DV_SX_ChN = value;
                            break;

                        case 34:
                            //  Ngành nghề sản xuất chủ lực

                            data.SoLuong = value;
                            break;
                        case 35:
                            //  Ngành nghề sản xuất chủ lực

                            data.DienTich_QuyMo = value;
                            break;
                        case 36:
                            //  Hiện tham gia sinh hoạt  đoàn thể chính trị, Hội đoàn nào khác

                            data.ThamGia_SH_DoanThe_HoiDoanKhac = value;
                            break;
                        case 37:
                            //  Tham gia câu lạc bộ, đội nhóm, mô hình, hợp tác xã, tổ hợp tác

                            data.ThamGia_CLB_DN_MH_HTX_THT = value;
                            break;
                        case 38:
                            //  Tham gia tổ hội ngành nghề, chi hội ngành nghề
                            data.ThamGia_THNN_CHNN = value;
                            break;
                        case 39:
                            //  HV nòng cốt
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.HoiVienNongCot = true;
                            }
                            break;
                        case 40:
                            //  HV ưu tú năm nào
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.HoiVienUuTuNam = value;
                            }
                            break;
                        case 41:
                            //  HV danh dự
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.HoiVienDanhDu = true;
                            }
                            break;
                        case 42:
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
                        case 43:
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
                        case 44:
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
                        case 45:
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
                        case 46:
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
                            case 47:
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
                        case 48:
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
                        case 49:
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
                        case 50:
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
                        case 51:
                            //  Hỗ trợ Vay vốn (nguồn vốn)
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.HoTrovayVon = value;
                            }
                            break;
                        case 52:
                            //  Hỗ trợ hình thức khác
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.HoTroKhac = value;
                            }
                            break;
                        case 53:
                            //  Hỗ trợ đào tạo nghề
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.HoTroDaoTaoNghe = value;
                            }
                            break;
                        case 54:
                            //  Hỗ trợ đào tạo nghề
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.GhiChu = value;
                            }
                            break;
                        case 55:
                            //  Chi hội
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                var exist = chiHois.Where(it => it.TenChiHoi == value);
                                if (exist.Count()>0)
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
                        case 56:
                            //  Tổ hội
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                var exist = toHois.Where(it => it.TenToHoi == value);
                                if (exist.Count()>0)
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
                        case 57:
                            //  Hỗ trợ đào tạo nghề
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.ChiHoiDanCu_CHT = value;
                            }
                            break;
                        case 58:
                            //  Hỗ trợ đào tạo nghề
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.ChiHoiDanCu_CHT = value;
                            }
                            break;
                        case 59:
                            //  Hỗ trợ đào tạo nghề
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.ChiHoiNgheNghiep_CHP = value;
                            }
                            break;
                        case 60:
                            //  Hỗ trợ đào tạo nghề
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.ChiHoiNgheNghiep_CHT = value;
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
        #endregion Check data type 
    }
}
