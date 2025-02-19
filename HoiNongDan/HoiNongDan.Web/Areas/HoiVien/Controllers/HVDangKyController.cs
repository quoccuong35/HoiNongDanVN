using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys.MasterData;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Resources;
using HoiNongDan.Web.Areas.NhanSu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Transactions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using System.Security.Principal;
using System.Runtime.Serialization;
using System;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Text.RegularExpressions;
using HoiNongDan.Models.Entitys.NhanSu;
using System.Globalization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using HoiNongDan.DataAccess.Migrations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage;
using NuGet.Packaging;


namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HVDangKyController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 10;
        public HVDangKyController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config, IHttpContextAccessor httpContext) : base(context)
        {
            _hostEnvironment = hostEnvironment;
            _httpContext = httpContext;
            _httpContext = httpContext;
        }
        #region Index
        //[HoiNongDanAuthorization]
        public IActionResult Index()
        {
            HVDangKySearchVM model = new HVDangKySearchVM();
            model.TuNgay = FirstDate();
            model.DenNgay = LastDate();
            model.TrangThai = "1";
            CreateViewBagSearch();
            return View(model);
        }
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult _Search(HVDangKySearchVM search)
        {
            return ExecuteSearch(() =>
            {
                var model = LoadData(search);
                return PartialView(model);
            });
        }
        #endregion Index

        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create()
        {
            CreateViewBag();
            HVDangKyVM model = new HVDangKyVM();
            model.MaDanToc = "KINH";
            return View(model);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HVDangKyVM item)
        {
            if (!String.IsNullOrWhiteSpace(item.SoCCCD))
            {
                var checkExits = _context.CanBos.Where(it => it.SoCCCD == item.SoCCCD && it.TuChoi != true && it.isRoiHoi != true );
                if (checkExits.Count() > 0)
                {
                    ModelState.AddModelError("SoCCCD", "Số CCCD đã tồn tại");
                }
            }
            var account = _context.Accounts.SingleOrDefault(it => it.AccountId == AccountId());
            CheckNguoiDuyet(account!);
            return ExecuteContainer(() =>
            {
                var add = new CanBo();
                add.IDCanBo = Guid.NewGuid();
                add.MaDiaBanHoatDong = item.MaDiaBanHoiVien;
                add.NgayDangKy = item.NgayDangKy;
                add.Level = "90";
                add.IsHoiVien = true;
                add.isRoiHoi = false;
                add.HoiVienDuyet = false;
                add.Actived = true;
                add.TuChoi = false;
                add.MaChiHoi = item.MaChiHoi;
                add.MaToHoi = item.MaToHoi;
                add.HoVaTen = item.HoVaTen;
                add.NgaySinh = item.NgaySinh;
                add.GioiTinh = item.GioiTinh;
                add.SoCCCD = item.SoCCCD;
                add.NgayCapCCCD = item.NgayCapCCCD;
                add.SoDienThoai = item.SoDienThoai;
                add.MaDanToc = item.MaDanToc!;
                add.MaTonGiao = item.MaTonGiao;
                add.MaTrinhDoHocVan = item.MaTrinhDoHocVan;
                add.MaTrinhDoChuyenMon = item.MaTrinhDoChuyenMon;
                add.ChuyenNganh = item.ChuyenNganh;
                add.MaTrinhDoChinhTri = item.MaTrinhDoChinhTri;
                add.NgayvaoDangDuBi = item.NgayvaoDangDuBi;
                add.NgayVaoDangChinhThuc = item.NgayVaoDangChinhThuc;
                add.NgayThamGiaHDND = item.NgayThamGiaHDND;
                add.NgayThamGiaCapUyDang = item.NgayThamGiaCapUyDang;
                add.VaiTro = item.VaiTro;
                add.VaiTroKhac = item.VaiTroKhac;
                add.MaGiaDinhThuocDien = item.MaGiaDinhThuocDien;
                add.GiaDinhThuocDienKhac = item.GiaDinhThuocDienKhac;
                add.MaNgheNghiep = item.MaNgheNghiep;
                add.ChoOHienNay = item.ChoOHienNay!;
                add.ChoOHienNay_XaPhuong = item.ChoOHienNay_XaPhuong!;
                add.ChoOHienNay_QuanHuyen = item.ChoOHienNay_QuanHuyen!;
                add.HoKhauThuongTru = item.HoKhauThuongTru;
                add.HoKhauThuongTru_XaPhuong = item.HoKhauThuongTru_XaPhuong;
                add.HoKhauThuongTru_QuanHuyen = item.HoKhauThuongTru_QuanHuyen;
                add.Loai_DV_SX_ChN = item.Loai_DV_SX_ChN;
                add.SoLuong = item.SoLuong;
                add.DienTich_QuyMo = item.DienTich_QuyMo;
                add.DangVien = item.DangVien;
                add.HoiVienDanCu = item.HoiVienDanCu;
                add.HoiVienNganhNghe = item.HoiVienNganhNghe;
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                if (item.MaDoanTheChinhTri_HoiDoan.Count() > 0)
                {
                    foreach (var item in item.MaDoanTheChinhTri_HoiDoan)
                    {
                        add.DoanTheChinhTri_HoiDoan_HoiViens.Add(new DoanTheChinhTri_HoiDoan_HoiVien { IDHoiVien = add.IDCanBo, MaDoanTheChinhTri_HoiDoan = item, CreatedAccountId = AccountId(), CreatedTime = DateTime.Now });
                    }
                }
                if (item.Id_CLB_DN_MH_HTX_THT.Count() > 0)
                {

                    foreach (var item in item.Id_CLB_DN_MH_HTX_THT)
                    {
                        add.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Add(new CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien { IDHoiVien = add.IDCanBo, Id_CLB_DN_MH_HTX_THT = item, CreatedAccountId = AccountId(), CreatedTime = DateTime.Now });
                    }
                }

                add.MaChucVu = Guid.Parse("D710D930-8342-474B-90A4-A1170A7A5691");
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                
                List<Guid> nguoiDuyets = new List<Guid>();
                string[] sTem = account!.AccountIDParent!.Split(";");
                foreach (var item in sTem)
                {
                    nguoiDuyets.Add(Guid.Parse(item));
                }
                List<HoiVienLichSuDuyet> hoiVienLichSuDuyets = new List<HoiVienLichSuDuyet>();
                nguoiDuyets.ForEach(item => {
                    hoiVienLichSuDuyets.Add(new HoiVienLichSuDuyet {
                        ID = Guid.NewGuid(),
                        IDHoiVien = add.IDCanBo,
                        AccountID = item,
                        CreateTime = DateTime.Now,
                        TrangThaiDuyet = false,
                    });
                });
                if (add.DoanTheChinhTri_HoiDoan_HoiViens.Count > 0)
                {
                    _context.DoanTheChinhTri_HoiDoan_HoiViens.AddRange(add.DoanTheChinhTri_HoiDoan_HoiViens);
                }
                if (add.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Count > 0)
                {
                    _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.AddRange(add.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens);
                }
                _context.HoiVienLichSuDuyets.AddRange(hoiVienLichSuDuyets);
                _context.Entry(add).State = EntityState.Added;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.HVDangKy.ToLower())
                });
            });
        }
        #endregion Create

        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
            var phamVis = Function.GetPhamVi(AccountId: AccountId()!.Value, _context: _context);
            var item = _context.CanBos.Include(it => it.DoanTheChinhTri_HoiDoan_HoiViens).Include(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens).SingleOrDefault(it => it.IDCanBo == id && phamVis.Contains(it.MaDiaBanHoatDong!.Value));
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            HVDangKyVM edit = new HVDangKyVM();
            edit.ID = item.IDCanBo;
            edit.MaDiaBanHoiVien = item.MaDiaBanHoatDong;
            edit.NgayDangKy = item.NgayDangKy;
            edit.HoVaTen = item.HoVaTen;
            edit.NgaySinh = item.NgaySinh;
            edit.GioiTinh = item.GioiTinh;
            edit.SoCCCD = item.SoCCCD;
            edit.NgayCapCCCD = item.NgayCapCCCD;
            edit.SoDienThoai = item.SoDienThoai;
            edit.MaDanToc = item.MaDanToc!;
            edit.NgayvaoDangDuBi = item.NgayvaoDangDuBi;
            edit.NgayVaoDangChinhThuc = item.NgayVaoDangChinhThuc;
            edit.MaTonGiao = item.MaTonGiao;
            edit.MaChiHoi = item.MaChiHoi;
            edit.MaToHoi = item.MaToHoi;
            edit.NgayThamGiaCapUyDang = item.NgayThamGiaCapUyDang;
            edit.NgayThamGiaHDND = item.NgayThamGiaHDND;
            edit.MaTrinhDoHocVan = item.MaTrinhDoHocVan;
            edit.MaTrinhDoChuyenMon = item.MaTrinhDoChuyenMon;
            edit.ChuyenNganh = item.ChuyenNganh;
            edit.MaTrinhDoChinhTri = item.MaTrinhDoChinhTri;
            edit.MaNgheNghiep = item.MaNgheNghiep;
            edit.MaGiaDinhThuocDien = item.MaGiaDinhThuocDien;
            edit.GiaDinhThuocDienKhac = item.GiaDinhThuocDienKhac;
            edit.VaiTro = item.VaiTro;
            edit.VaiTroKhac = item.VaiTroKhac;
            edit.ChoOHienNay = item.ChoOHienNay!;
            edit.ChoOHienNay_XaPhuong = item.ChoOHienNay_XaPhuong!;
            edit.ChoOHienNay_QuanHuyen = item.ChoOHienNay_QuanHuyen!;
            edit.HoKhauThuongTru = item.HoKhauThuongTru;
            edit.HoKhauThuongTru_XaPhuong = item.HoKhauThuongTru_XaPhuong;
            edit.HoKhauThuongTru_QuanHuyen = item.HoKhauThuongTru_QuanHuyen;
            edit.DangVien = item.DangVien == null? false:item.DangVien.Value;
            edit.HoiVienDanCu = item.HoiVienDanCu == null ? false : item.HoiVienDanCu.Value; 
            edit.HoiVienNganhNghe = item.HoiVienNganhNghe == null ? false : item.HoiVienNganhNghe.Value;
            edit.Loai_DV_SX_ChN = item.Loai_DV_SX_ChN;
            edit.SoLuong = item.SoLuong;
            edit.DienTich_QuyMo = item.DienTich_QuyMo;
            CreateViewBag(maTrinhDoHocVan:item.MaTrinhDoHocVan,
                maTrinhDoChuyenMon:item.MaTrinhDoChuyenMon,
                maTrinhDoChinhTri:item.MaTrinhDoChinhTri,
                maDanToc:item.MaDanToc,
                maTonGiao:item.MaTonGiao,
                maNgheNghiep:item.MaNgheNghiep,
                maDiaBanHoiVien:item.MaDiaBanHoatDong, item.MaGiaDinhThuocDien, item.MaChiHoi, item.MaToHoi,
                MaDoanTheChinhTri_HoiDoan: item.DoanTheChinhTri_HoiDoan_HoiViens.Select(it => it.MaDoanTheChinhTri_HoiDoan).ToList(),
                Id_CLB_DN_MH_HTX_THT: item.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Select(it => it.Id_CLB_DN_MH_HTX_THT).ToList());
            return View(edit);
        }

        [HttpPost]
        [HoiNongDanAuthorization]
        public IActionResult Edit(HVDangKyVM item)
        {
            if (!String.IsNullOrWhiteSpace(item.SoCCCD))
            {
                var checkExits = _context.CanBos.Where(it => it.SoCCCD == item.SoCCCD && it.IDCanBo != item.ID && it.TuChoi != true && it.isRoiHoi != true);
                if (checkExits.Count() > 0)
                {
                    ModelState.AddModelError("SoCCCD", "Số CCCD đã tồn tại");
                }
            }
            return ExecuteContainer(() => {
                var phamVis = Function.GetPhamVi(AccountId: AccountId()!.Value, _context: _context);
                var edit = _context.CanBos.Include(it => it.DoanTheChinhTri_HoiDoan_HoiViens)
                .Include(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens).SingleOrDefault(it => it.IDCanBo == item.ID && phamVis.Contains(it.MaDiaBanHoatDong!.Value));
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.HVDangKy.ToLower())
                    });
                }
                else
                {
                    edit.MaDiaBanHoatDong = item.MaDiaBanHoiVien;
                    edit.NgayDangKy = item.NgayDangKy;
                    edit.MaChiHoi = item.MaChiHoi;
                    edit.MaToHoi = item.MaToHoi;
                    edit.HoVaTen = item.HoVaTen;
                    edit.NgaySinh = item.NgaySinh;
                    edit.GioiTinh = item.GioiTinh;
                    edit.SoCCCD = item.SoCCCD;
                    edit.NgayCapCCCD = item.NgayCapCCCD;
                    edit.SoDienThoai = item.SoDienThoai;
                    edit.MaDanToc = item.MaDanToc!;
                    edit.MaTonGiao = item.MaTonGiao;
                    edit.MaTrinhDoHocVan = item.MaTrinhDoHocVan;
                    edit.MaTrinhDoChuyenMon = item.MaTrinhDoChuyenMon;
                    edit.ChuyenNganh = item.ChuyenNganh;
                    edit.MaTrinhDoChinhTri = item.MaTrinhDoChinhTri;
                    edit.NgayvaoDangDuBi = item.NgayvaoDangDuBi;
                    edit.NgayVaoDangChinhThuc = item.NgayVaoDangChinhThuc;
                    edit.NgayThamGiaHDND = item.NgayThamGiaHDND;
                    edit.NgayThamGiaCapUyDang = item.NgayThamGiaCapUyDang;
                    edit.VaiTro = item.VaiTro;
                    edit.VaiTroKhac = item.VaiTroKhac;
                    edit.MaGiaDinhThuocDien = item.MaGiaDinhThuocDien;
                    edit.GiaDinhThuocDienKhac = item.GiaDinhThuocDienKhac;
                    edit.MaNgheNghiep = item.MaNgheNghiep;
                    edit.ChoOHienNay = item.ChoOHienNay!;
                    edit.ChoOHienNay_XaPhuong = item.ChoOHienNay_XaPhuong!;
                    edit.ChoOHienNay_QuanHuyen = item.ChoOHienNay_QuanHuyen!;
                    edit.HoKhauThuongTru = item.HoKhauThuongTru;
                    edit.HoKhauThuongTru_XaPhuong = item.HoKhauThuongTru_XaPhuong;
                    edit.HoKhauThuongTru_QuanHuyen = item.HoKhauThuongTru_QuanHuyen;
                    edit.Loai_DV_SX_ChN = item.Loai_DV_SX_ChN;
                    edit.SoLuong = item.SoLuong;
                    edit.DienTich_QuyMo = item.DienTich_QuyMo;
                    edit.DangVien = item.DangVien;
                    edit.HoiVienDanCu = item.HoiVienDanCu;
                    edit.HoiVienNganhNghe = item.HoiVienNganhNghe;
                    edit.LastModifiedTime = DateTime.Now;
                    edit.LastModifiedAccountId = AccountId();
                    edit.DoanTheChinhTri_HoiDoan_HoiViens.Clear();
                    if (item.MaDoanTheChinhTri_HoiDoan != null && item.MaDoanTheChinhTri_HoiDoan.Count() > 0)
                    {

                      
                        foreach (var item in item.MaDoanTheChinhTri_HoiDoan)
                        {
                            edit.DoanTheChinhTri_HoiDoan_HoiViens.Add(new DoanTheChinhTri_HoiDoan_HoiVien { IDHoiVien = edit.IDCanBo, MaDoanTheChinhTri_HoiDoan = item, CreatedAccountId = AccountId(),CreatedTime = DateTime.Now });
                        }
                    }
                    edit.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Clear();
                    if (item.Id_CLB_DN_MH_HTX_THT != null && item.Id_CLB_DN_MH_HTX_THT.Count() > 0)
                    {
                        //var dels = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Where(it => it.IDHoiVien == edit.IDCanBo).ToList();
                        //if (dels.Count > 0)
                        //{
                        //    _context.RemoveRange(dels);
                        //}
                       
                        foreach (var item in item.Id_CLB_DN_MH_HTX_THT)
                        {
                            edit.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Add(new CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien { IDHoiVien = edit.IDCanBo, Id_CLB_DN_MH_HTX_THT = item, CreatedAccountId = AccountId(), CreatedTime = DateTime.Now });
                        }
                    }
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.HVDangKy.ToLower())
                    });
                }
            });
        }
        #endregion Edit

        #region Import
        [HoiNongDanAuthorization]
        public IActionResult _Import()
        {
            CreateViewBagSearch();
            return PartialView();
        }

        [HoiNongDanAuthorization]
        public IActionResult Import(Guid? MaDiaBanHoiVien, String? MaQuanHuyen,DateTime? NgayDangKy)
        {
            if (MaDiaBanHoiVien == null)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = false,
                    Data = "Chưa chọn hội nông dân đăng ký"
                });
            }
            if (NgayDangKy == null)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = false,
                    Data = "Chưa chọn ngày đăng ký"
                });
            }
            var account = _context.Accounts.SingleOrDefault(it => it.AccountId == AccountId());
            if (String.IsNullOrWhiteSpace(account!.AccountIDParent)) {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = false,
                    Data = LanguageResource.ErrorDuyetHoiVien
                });
            }
            DataSet ds = GetDataSetFromExcel();
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                bool edit;
                List<string> errorList = new List<string>();
              

                List<Guid> nguoiDuyets = new List<Guid>();
                string[] sTem = account!.AccountIDParent!.Split(";");
                foreach (var item in sTem)
                {
                    nguoiDuyets.Add(Guid.Parse(item));
                }
                List<ChiHoi> chiHoiAdd = new List<ChiHoi>();
                var chiHois = _context.ChiHois.Where(p => p.MaDiaBanHoatDong == MaDiaBanHoiVien.Value).ToList();
               
                List<ToHoi> toHoiAdd = new List<ToHoi>();
                var toHois = _context.ToHois.Where(p => p.MaDiaBanHoatDong == MaDiaBanHoiVien.Value).ToList(); ;

                List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac> cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacAdd = new List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac>();
                var cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.ToList();
                List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien> cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens = new List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien>();


                List<DoanTheChinhTri_HoiDoan> doanTheChinhTri_HoiDoanAdd = new List<DoanTheChinhTri_HoiDoan>();
                var doanTheChinhTri_HoiDoan = _context.DoanTheChinhTri_HoiDoans.ToList();
                List<DoanTheChinhTri_HoiDoan_HoiVien> doanTheChinhTri_HoiDoan_HoiViens = new List<DoanTheChinhTri_HoiDoan_HoiVien>();
                return ExcuteImportExcel(() =>
                {
                    const TransactionScopeOption opt = new TransactionScopeOption();

                    TimeSpan span = new TimeSpan(0, 0, 30, 30);
                    using (TransactionScope ts = new TransactionScope(opt, span))
                    {
                        List<String> error = new List<String>();
                        int iCapNhat = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            if (dt.Rows.IndexOf(row) >= startIndex-1)
                            {

                                if (row[0] == null || String.IsNullOrWhiteSpace(row[0].ToString()))
                                    break;
                                error = new List<String>();
                                chiHoiAdd = new List<ChiHoi>();
                                toHoiAdd = new List<ToHoi>();
                                cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacAdd = new List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac>();
                                doanTheChinhTri_HoiDoanAdd = new List<DoanTheChinhTri_HoiDoan>();
                                cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens = new List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien>();
                                doanTheChinhTri_HoiDoan_HoiViens = new List<DoanTheChinhTri_HoiDoan_HoiVien>();

                                var data = CheckTemplateNew(row:row.ItemArray!, error:error, edit: out edit,MaDiaBan:MaDiaBanHoiVien.Value, toHois: toHois, toHoiAdds: toHoiAdd, chiHois: chiHois, chiHoiAdd: chiHoiAdd, doanTheChinhTri_HoiDoan: 
                                    doanTheChinhTri_HoiDoan, doanTheChinhTri_HoiDoanAdd: doanTheChinhTri_HoiDoanAdd,doanTheChinhTri_HoiDoan_HoiViens: doanTheChinhTri_HoiDoan_HoiViens,
                                    cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs:cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac,cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacAdd:cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacAdd,
                                    cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens: cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens);
                                data.MaDiaBanHoatDong = MaDiaBanHoiVien;
                                data.MaChucVu = data.MaChucVu == null? Guid.Parse("D710D930-8342-474B-90A4-A1170A7A5691") : data.MaChucVu;
                                data.NgayDangKy = NgayDangKy;
                                string capNhatDanhMuc = CapNhatDanhMuc(doanTheChinhTri_HoiDoans: doanTheChinhTri_HoiDoanAdd,chiHois:chiHoiAdd,toHois:toHoiAdd,cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs:cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacAdd, HoVaTen:data.HoVaTen);
                                if (capNhatDanhMuc != LanguageResource.ImportSuccess)
                                {
                                    errorList.Add(capNhatDanhMuc);
                                }
                                if (error.Count > 0)
                                {
                                    errorList.AddRange(error);
                                }
                                else
                                {
                                    string result = ExecuteImportExcelHoiVien(canbo:data, bEdit: edit,  cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens: cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens,
                                        doanTheChinhTri_HoiDoan_HoiViens: doanTheChinhTri_HoiDoan_HoiViens, nguoiDuyets: nguoiDuyets);
                                    if (result != LanguageResource.ImportSuccess)
                                    {
                                        errorList.Add(result);
                                    }
                                    else iCapNhat++;

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
                            Data = LanguageResource.ImportSuccess + " " + iCapNhat
                        });;
                    }

                });
            }
            else
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                    Data = "Không đọc được file excel"
                });
            }

        }
        private DataSet GetDataSetFromExcel()
        {
         
            try
            {
                DataSet ds = new DataSet();
                var file = Request.Form.Files[0];
                if (file != null && file.Length > 0)
                {
                    //Check file is excel
                    //Notes: Châu bổ sung .xlsb
                    if (file.FileName.Contains("xls") || file.FileName.Contains("xlsx"))
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        var fileName = Path.GetFileName(file.FileName);
                        var mapPath = Path.Combine(wwwRootPath, @"upload\excel");
                        var path = Path.Combine(mapPath, fileName);
                        if (!Directory.Exists(mapPath))
                        {
                            Directory.CreateDirectory(mapPath);
                        }
                        try
                        {
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
                        }
                        catch
                        {
                            return null!;
                        }
                        finally
                        {
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
                return null!;
            }
        }
        private CanBo CheckTemplate(object[] row, List<String> error, out bool edit) {
            CanBo data = new CanBo();
            data.IDCanBo = Guid.NewGuid();
            data.HoiVienDuyet = false;
            data.AccountIdDangKy = AccountId();
            data.IsHoiVien = true;
            data.TuChoi = false;
            int index = 0; string value;
            edit = false;
            for (int i = 0; i < row.Length; i++) 
            {
                value = row[i] == null ? "": row[i].ToString()!.Trim();
                switch (i) {
                    case 0:
                        // stt
                        index = int.Parse(value);
                        break;
                    case 1:
                        //
                        if (!String.IsNullOrWhiteSpace(value)) {
                            try
                            {
                                data.IDCanBo = Guid.Parse(value);
                                edit = true;
                            }
                            catch
                            {
                                error.Add($"Dòng {index} có ID không hợp lệ");
                            }
                        }
                        break;
                    case 2:
                        //Ho Va ten
                        try
                        {
                            if (String.IsNullOrWhiteSpace(value))
                            {
                                error.Add($"Dòng {index} Chưa có nhập họ và tên");
                            }
                            else { 
                                data.HoVaTen = value;
                            }
                        }
                        catch
                        {
                            error.Add($"Dòng {index} có Họ và tên không hợp lệ");
                        }
                        break;
                    case 3:
                        //Ngày tháng năm sinh - Nam
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.GioiTinh = GioiTinh.Nam;
                                data.NgaySinh = Function.ConvertStringToDate(value).ToString("dd/MM/yyyy");
                            }
                        }
                        catch
                        {
                            error.Add($"Dòng {index} có ngày sinh Nam không hợp lệ");
                        }
                        break;
                    case 4:
                        //Ngày tháng năm sinh - nữ
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.GioiTinh = GioiTinh.Nữ;
                                data.NgaySinh = Function.ConvertStringToDate(value).ToString("dd/MM/yyyy");
                            }

                        }
                        catch
                        {
                            error.Add($"Dòng {index} có ngày sinh Nữ không hợp lệ");
                        }
                        break;
                    case 5:
                        //CMND/CCCD
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                if (value.Length != 12)
                                {
                                    error.Add(string.Format("Dữ liệu dòng {0} Số CCCD {1} phải là 12 số!", index, value));
                                }
                                Regex rg = new Regex(@"^\d+$");
                                if (!rg.IsMatch(value))
                                {
                                    error.Add(string.Format("Dữ liệu dòng {0} Số CCCD {1} phải là kiểu số!", index, value));
                                }
                                var checkExits = _context.CanBos.Where(it => it.SoCCCD == value && it.isRoiHoi != true);
                                if (edit)
                                {

                                    if (checkExits.Where(it => it.IDCanBo != data.IDCanBo).Count() > 0)
                                    {
                                        error.Add($"Dòng {index} số CCCD đã tồn tại");
                                    }
                                }
                                else if (checkExits.Count()>0)
                                {
                                    error.Add($"Dòng {index} số CCCD đã tồn tại");
                                }
                                data.SoCCCD = value;
                            }
                            else
                            {
                                error.Add($"Dòng {index} Chưa nhập số CCCD");
                            }
                        }
                        catch
                        {
                            error.Add($"Dòng {index} có ngày sinh Nữ không hợp lệ");
                        }
                        break;
                    case 6:
                        //ngay cấp CMND/CCCD
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.NgayCapCCCD = Function.ConvertStringToDate(value).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                error.Add($"Dòng {index} Chưa nhập số ngày cấp CCCD");
                            }

                        }
                        catch
                        {
                            error.Add($"Dòng {index} có ngày Ngày cấp không hợp lệ");
                        }
                        break;
                    case 7:
                        //Hộ khẩu thường trú
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.HoKhauThuongTru = value;
                        }
                        else error.Add($"Dòng {index} hộ khẩu thường trú");
                        break;
                    case 8:
                        //Nơi ở hiện nay
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.ChoOHienNay = value;
                        }
                        else error.Add($"Dòng {index} chưa nhập nơi ở hiện nay");
                        break;
                    case 9:
                        //Số điện thoại
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.SoDienThoai = value;
                        }
                        break;
                    case 10:
                        //Nơi ở hiện nay
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.DangVien = true;
                        }
                        break;
                    case 11:
                        //Dân tộc
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.DanTocs.FirstOrDefault(it => it.TenDanToc == value);
                            if (obj != null)
                            {
                                data.MaDanToc = obj.MaDanToc;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy dân tộc tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 12:
                        //Tôn giáo
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.TonGiaos.FirstOrDefault(it => it.TenTonGiao == value);
                            if (obj != null)
                            {
                                data.MaTonGiao = obj.MaTonGiao;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy tôn giáo có tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 13:
                        //Trình độ học vấn
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.TrinhDoHocVans.FirstOrDefault(it => it.TenTrinhDoHocVan == value);
                            if (obj != null)
                            {
                                data.MaTrinhDoHocVan = obj.MaTrinhDoHocVan;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy Trình độ học vấn có tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 14:
                        //Trình độ chuyên môn
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.TrinhDoChuyenMons.FirstOrDefault(it => it.TenTrinhDoChuyenMon == value);
                            if (obj != null)
                            {
                                data.MaTrinhDoChuyenMon = obj.MaTrinhDoChuyenMon;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy Trình độ chuyên môn có tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 15:
                        //Chính trị
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.TrinhDoChinhTris.FirstOrDefault(it => it.TenTrinhDoChinhTri == value);
                            if (obj != null)
                            {
                                data.MaTrinhDoChinhTri = obj.MaTrinhDoChinhTri;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy Trình độ Chính trị có tên {0} ở dòng số {1} !", value, index));
                            }
                            
                        }
                        break;
                    //case 16:
                    //    //Ngày tháng năm vào Hội (Theo QĐ công nhận hội viên)
                    //    if (!String.IsNullOrWhiteSpace(value))
                    //    {
                    //        data.NgayVaoHoi = value;
                    //    }
                    //    break;
                    case 16:
                        //Nghề nghiệp hiện nay
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.NgheNghieps.FirstOrDefault(it => it.TenNgheNghiep == value);
                            if (obj != null)
                            {
                                data.MaNgheNghiep = obj.MaNgheNghiep;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy Nghề nghiệp có tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 17:
                        //Địa bàn dân cư
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.HoiVienDanCu = true;
                        }
                        break;
                    case 18:
                        //Nghề nghiệp hiện nay
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.HoiVienNganhNghe= true;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (data.HoiVienDanCu == true && data.HoiVienNganhNghe == true)
            {
                error.Add(string.Format("Thông tin hội viên dòng {0} vừa chọn dân cư và nghề nghiệp không hợp lệ", index));
            }
            if (data.HoiVienDanCu != true && data.HoiVienNganhNghe != true)
            {
                error.Add(string.Format("Thông tin hội viên dòng {0} chưa xác định là hội viên dân cư hay nghề nghiệp", index));
            }
            return data;
        }
        private string ExecuteImportExcel(CanBo hvDangKy,bool edit= false, List<Guid>? nguoiDuyets = null)
        {
            if (!edit)
            {
                try
                {
                    hvDangKy.LastModifiedTime = DateTime.Now;
                    
                    List<HoiVienLichSuDuyet> hoiVienLichSuDuyets = new List<HoiVienLichSuDuyet>();
                    nguoiDuyets!.ForEach(item => {
                        hoiVienLichSuDuyets.Add(new HoiVienLichSuDuyet
                        {
                            ID = Guid.NewGuid(),
                            IDHoiVien = hvDangKy.IDCanBo,
                            AccountID = item,
                            CreateTime = DateTime.Now,
                            TrangThaiDuyet = false,
                        });
                    });
                    _context.HoiVienLichSuDuyets.AddRange(hoiVienLichSuDuyets);
                    _context.Entry(hvDangKy).State = EntityState.Added;
                }
                catch
                {

                }
            }
            else
            {
                var hvEdit = _context.CanBos.SingleOrDefault(p => p.IDCanBo == hvDangKy.IDCanBo && p.HoiVienDuyet !=true && p.TuChoi != true);
                //hvEdit!.MaCanBo = hvDangKy.MaCanBo;
                //hvEdit.NgayCapThe = hvDangKy.NgayCapThe;

                if (hvEdit != null)
                {
                    hvEdit = HoiVienDangKyMoiEdit(hvEdit, hvDangKy);
                    HistoryModelRepository history = new HistoryModelRepository(_context);
                    history.SaveUpdateHistory(hvDangKy.IDCanBo.ToString(), AccountId()!.Value, hvEdit);
                }
                else
                {
                    return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                            LanguageResource.HoiVien, hvEdit!.IDCanBo,
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
                return ex.InnerException!.Message + " " + hvDangKy.HoVaTen;
            }
            return LanguageResource.ImportSuccess;
        }

        private CanBo CheckTemplateNew(object[] row, List<String> error, out bool edit,Guid MaDiaBan, List<ToHoi> toHois, List<ToHoi> toHoiAdds, List<ChiHoi> chiHois,
            List<ChiHoi> chiHoiAdd,List<DoanTheChinhTri_HoiDoan> doanTheChinhTri_HoiDoan, List<DoanTheChinhTri_HoiDoan> doanTheChinhTri_HoiDoanAdd, List<DoanTheChinhTri_HoiDoan_HoiVien> doanTheChinhTri_HoiDoan_HoiViens,
            List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac> cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs, List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac> cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacAdd, List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien> cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens)
        {
            CanBo data = new CanBo();
            data.IDCanBo = Guid.NewGuid();
            data.HoiVienDuyet = false;
            data.AccountIdDangKy = AccountId();
            data.IsHoiVien = true;
            data.TuChoi = false;
            int index = 0; string value;
            edit = false;
            for (int i = 0; i < row.Length; i++)
            {
                value = row[i] == null ? "" : row[i].ToString()!.Trim();
                switch (i)
                {
                    case 0:
                        // stt
                        index = int.Parse(value);
                        break;
                    case 1:
                        //
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.IDCanBo = Guid.Parse(value);
                                edit = true;
                            }
                            catch
                            {
                                error.Add($"Dòng {index} có ID không hợp lệ");
                            }
                        }
                        break;
                    case 2:
                        //Ho Va ten
                        try
                        {
                            if (String.IsNullOrWhiteSpace(value))
                            {
                                error.Add($"Dòng {index} Chưa có nhập họ và tên");
                            }
                            else
                            {
                                data.HoVaTen = value;
                            }
                        }
                        catch
                        {
                            error.Add($"Dòng {index} có Họ và tên không hợp lệ");
                        }
                        break;
                    case 3:
                        //Ngày tháng năm sinh - Nam
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.NgaySinh = Function.ConvertStringToDate(value).ToString("dd/MM/yyyy");
                            }
                        }
                        catch
                        {
                            error.Add($"Dòng {index} có ngày sinh không hợp lệ");
                        }
                        break;
                    case 4:
                        //Giới tính
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.GioiTinh = GioiTinh.Nam;
                            }
                            else
                            {
                                data.GioiTinh = GioiTinh.Nữ;
                            }

                        }
                        catch
                        {
                            error.Add($"Dòng {index} có giới tính không hợp lệ");
                        }
                        break;
                    case 5:
                        // Chức vụ
                        if (string.IsNullOrEmpty(value))
                        {
                            data.MaChucVu = Guid.Parse("D710D930-8342-474B-90A4-A1170A7A5691");
                        }
                        else
                        {
                            var chucVu = _context.ChucVus.FirstOrDefault(it => it.TenChucVu == value.Trim());
                            if (chucVu != null)
                            {
                                data.MaChucVu = chucVu.MaChucVu;

                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy chức vụ dòng {0} Có tên {1} không tồn tại!", index, value));
                            }

                        }
                        break;
                    case 6:
                        //CMND/CCCD
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                if (value.Length != 12)
                                {
                                    error.Add(string.Format("Dữ liệu dòng {0} Số CCCD {1} phải là 12 số!", index, value));
                                }
                                Regex rg = new Regex(@"^\d+$");
                                if (!rg.IsMatch(value))
                                {
                                    error.Add(string.Format("Dữ liệu dòng {0} Số CCCD {1} phải là kiểu số!", index, value));
                                }
                                var checkExits = _context.CanBos.Where(it => it.SoCCCD == value && it.TuChoi != true && it.isRoiHoi != true);
                                if (edit)
                                {

                                    if (checkExits.Where(it => it.IDCanBo != data.IDCanBo).Count() > 0)
                                    {
                                        error.Add($"Dòng {index} số CCCD đã tồn tại");
                                    }
                                }
                                else if (checkExits.Count() > 0)
                                {
                                    error.Add($"Dòng {index} số CCCD đã tồn tại");
                                }
                                data.SoCCCD = value;
                            }
                            else
                            {
                                error.Add($"Dòng {index} Chưa nhập số CCCD");
                            }
                        }
                        catch
                        {
                            error.Add($"Dòng {index} có ngày sinh Nữ không hợp lệ");
                        }
                        break;
                    case 7:
                        //ngay cấp CMND/CCCD
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.NgayCapCCCD = Function.ConvertStringToDate(value).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                error.Add($"Dòng {index} Chưa nhập số ngày cấp CCCD");
                            }

                        }
                        catch
                        {
                            error.Add($"Dòng {index} có ngày Ngày cấp không hợp lệ");
                        }
                        break;
                    case 8:
                        //Tồ hội
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var exist = toHois.Where(it => it.TenToHoi.ToLower() == value.Trim().ToLower());
                            if (exist.Count() > 0)
                            {
                                data.MaToHoi = exist.First().MaToHoi;
                            }
                            else
                            {
                                ToHoi toHoi = new ToHoi { MaToHoi = Guid.NewGuid(), TenToHoi = value, MaDiaBanHoatDong = MaDiaBan };
                                if (value.Trim().Length > 2)
                                {

                                    if (value.Substring(0, 2).ToUpper() == "NN")
                                    {
                                        //  data.HoiVienNganhNghe = true;
                                        toHoi.Loai = "02";
                                    }
                                    else
                                    {
                                        //  data.HoiVienDanCu = true;
                                        toHoi.Loai = "01";
                                    }
                                }
                                else
                                {
                                    toHoi.Loai = "01";
                                }
                                data.MaToHoi = toHoi.MaToHoi;
                                toHois.Add(toHoi);
                                toHoiAdds.Add(toHoi);
                            }

                        }
                        break;
                    case 9:
                        //Chi hội
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var exist = chiHois.Where(it => it.TenChiHoi.ToLower() == value.Trim().ToLower());
                            if (exist.Count() > 0)
                            {
                                data.MaChiHoi = exist.First().MaChiHoi;
                            }
                            else
                            {
                                ChiHoi chiHoi = new ChiHoi { MaChiHoi = Guid.NewGuid(), TenChiHoi = value, MaDiaBanHoatDong = MaDiaBan };
                                if (value.Trim().Length > 2)
                                {
                                    if (value.Substring(0, 2).ToUpper() == "NN")
                                    {
                                        // data.HoiVienNganhNghe = true;
                                        chiHoi.Loai = "02";
                                    }
                                    else
                                    {
                                        // data.HoiVienDanCu = true;
                                        chiHoi.Loai = "01";
                                    }
                                }
                                else
                                {
                                    chiHoi.Loai = "01";
                                }
                                data.MaChiHoi = chiHoi.MaChiHoi;
                                chiHois.Add(chiHoi);
                                chiHoiAdd.Add(chiHoi);
                            }


                        }
                        break;
                    case 10:
                        //Hộ khẩu thường trú
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.HoKhauThuongTru = value;
                        }
                        break;
                    case 11:
                        //Hộ khẩu thường trú xã-phường
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.HoKhauThuongTru_XaPhuong = value;
                        }
                        break;
                    case 12:
                        //Hộ khẩu thường trú huyện - quận
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.HoKhauThuongTru_QuanHuyen = value;
                        }
                        break;
                    case 13:
                        //Nơi ở hiện nay
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.ChoOHienNay = value;
                        }
                        break;
                    case 14:
                        //Nơi ở hiện nay xã-phường
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.ChoOHienNay_XaPhuong = value;
                        }
                        break;
                    case 15:
                        //Nơi ở hiện nay huyện - quận
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.ChoOHienNay_QuanHuyen = value;
                        }
                        break;
                    case 16:
                        //Số điện thoại
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.SoDienThoai = value;
                        }
                        break;
                    case 17:
                        //Đảng viên Dự bị
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.NgayvaoDangDuBi = value;
                        }
                        break;
                    case 18:
                        //Đảng viên Chính  thức
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.NgayVaoDangChinhThuc = value;
                            data.DangVien = true;
                        }
                        break;
                    case 19:
                        //Dân tộc
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.DanTocs.FirstOrDefault(it => it.TenDanToc == value);
                            if (obj != null)
                            {
                                data.MaDanToc = obj.MaDanToc;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy dân tộc tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 20:
                        //Tôn giáo
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.TonGiaos.FirstOrDefault(it => it.TenTonGiao == value);
                            if (obj != null)
                            {
                                data.MaTonGiao = obj.MaTonGiao;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy tôn giáo có tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 21:
                        //Trình độ học vấn
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.TrinhDoHocVans.FirstOrDefault(it => it.TenTrinhDoHocVan == value);
                            if (obj != null)
                            {
                                data.MaTrinhDoHocVan = obj.MaTrinhDoHocVan;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy Trình độ học vấn có tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 22:
                        //Trình độ chuyên môn
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.TrinhDoChuyenMons.FirstOrDefault(it => it.TenTrinhDoChuyenMon == value);
                            if (obj != null)
                            {
                                data.MaTrinhDoChuyenMon = obj.MaTrinhDoChuyenMon;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy Trình độ chuyên môn có tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 23:
                        //Chính trị
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.TrinhDoChinhTris.FirstOrDefault(it => it.TenTrinhDoChinhTri == value);
                            if (obj != null)
                            {
                                data.MaTrinhDoChinhTri = obj.MaTrinhDoChinhTri;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy Trình độ Chính trị có tên {0} ở dòng số {1} !", value, index));
                            }

                        }
                        break;
                     case 24:
                        //Tham gia  cấp ủy Đảng
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.NgayThamGiaCapUyDang = value;
                        }
                        break;
                    case 25:
                        //Tham gia HĐND
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.NgayThamGiaHDND = value;
                        }
                        break;
                    case 26:
                        //Vai trỏ
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.VaiTro = "1";
                        }
                        break;
                    case 27:
                        //Vai trỏ khac
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.VaiTroKhac = value;
                            data.VaiTro = "2";
                        }
                        break;
                    case 28:
                        //Nghề nghiệp hiện nay
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.GiaDinhThuocDiens.FirstOrDefault(it => it.TenGiaDinhThuocDien == value);
                            if (obj != null)
                            {
                                data.MaGiaDinhThuocDien = obj.MaGiaDinhThuocDien;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy Gia đình thuộc diện có tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 29:
                        //Vai trỏ
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.GiaDinhThuocDienKhac = value;
                        }
                        break;
                    case 30:
                        //Nghề nghiệp hiện nay
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.NgheNghieps.FirstOrDefault(it => it.TenNgheNghiep == value);
                            if (obj != null)
                            {
                                data.MaNgheNghiep = obj.MaNgheNghiep;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy Nghề nghiệp có tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 31:
                        //Loại hình,  dịch vụ sản xuất, chăn nuôi
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.Loai_DV_SX_ChN = value;
                        }
                        break;
                    case 32:
                        //Nghề nghiệp hiện nay
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.SoLuong = value;
                        }
                        break;
                    case 33:
                        //Diện tích
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.DienTich_QuyMo = value;
                        }
                        break;
                    case 34:
                        //  Hiện tham gia sinh hoạt  đoàn thể chính trị, Hội đoàn nào khác
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.ThamGia_SH_DoanThe_HoiDoanKhac = value;
                            String[] arr = value.Split(";");
                            if (arr.Length > 0)
                            {
                                foreach (var item in arr)
                                {
                                    var exist = doanTheChinhTri_HoiDoan.Where(it => it.TenDoanTheChinhTri_HoiDoan == item);
                                    if (exist.Count() > 0)
                                    {
                                        doanTheChinhTri_HoiDoan_HoiViens.Add(new DoanTheChinhTri_HoiDoan_HoiVien
                                        {
                                            IDHoiVien = data.IDCanBo,
                                            MaDoanTheChinhTri_HoiDoan = exist.First().MaDoanTheChinhTri_HoiDoan,
                                        });
                                    }
                                    else
                                    {
                                        DoanTheChinhTri_HoiDoan doanthe = new DoanTheChinhTri_HoiDoan { MaDoanTheChinhTri_HoiDoan = Guid.NewGuid(), TenDoanTheChinhTri_HoiDoan = item, CreatedAccountId = AccountId(), CreatedTime = DateTime.Now, Actived = true };
                                        doanTheChinhTri_HoiDoan_HoiViens.Add(new DoanTheChinhTri_HoiDoan_HoiVien
                                        {
                                            IDHoiVien = data.IDCanBo,
                                            MaDoanTheChinhTri_HoiDoan = doanthe.MaDoanTheChinhTri_HoiDoan,
                                        });
                                        doanTheChinhTri_HoiDoan.Add(doanthe);


                                        doanTheChinhTri_HoiDoanAdd.Add(doanthe);
                                    }
                                }
                            }


                        }

                        break;
                    case 35:
                        //  Tham gia câu lạc bộ, đội nhóm, mô hình, hợp tác xã, tổ hợp tác
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.ThamGia_CLB_DN_MH_HTX_THT = value;
                            String[] arr = value.Split(";");
                            if (arr.Length > 0)
                            {
                                foreach (var item in arr)
                                {
                                    var exist = cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.Where(it => it.Ten == item);
                                    if (exist.Count() > 0)
                                    {
                                        cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Add(new CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien
                                        {
                                            IDHoiVien = data.IDCanBo,
                                            Id_CLB_DN_MH_HTX_THT = exist.First().Id_CLB_DN_MH_HTX_THT
                                        });
                                    }
                                    else
                                    {
                                        CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac caulacbo = new CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac { Id_CLB_DN_MH_HTX_THT = Guid.NewGuid(), Ten = item, CreatedAccountId = AccountId(), CreatedTime = DateTime.Now, Actived = true };
                                        cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Add(new CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien
                                        {
                                            IDHoiVien = data.IDCanBo,
                                            Id_CLB_DN_MH_HTX_THT = caulacbo.Id_CLB_DN_MH_HTX_THT,
                                        });
                                        cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.Add(caulacbo);


                                        cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacAdd.Add(caulacbo);
                                    }
                                }
                            }
                        }

                        break;
                    default:
                        break;
                }
            }
            return data;
        }

        private CanBo HoiVienDangKyMoiEdit(CanBo old, CanBo news)
        {
            old.HoVaTen = news.HoVaTen;
            old.GioiTinh = news.GioiTinh;
            old.SoCCCD = news.SoCCCD;
            old.NgayCapCCCD = news.NgayCapCCCD;
            old.NoiSinh = news.NoiSinh;
            old.ChoOHienNay = news.ChoOHienNay;
            old.SoDienThoai = news.SoDienThoai;
            old.DangVien = news.DangVien;
            old.MaDanToc = news.MaDanToc;
            old.MaTonGiao = news.MaTonGiao;
            old.MaTrinhDoHocVan = news.MaTrinhDoHocVan;
            old.MaTrinhDoChuyenMon = news.MaTrinhDoChuyenMon;
            old.MaTrinhDoChinhTri = news.MaTrinhDoChuyenMon;
            old.MaNgheNghiep = news.MaNgheNghiep;
            old.HoiVienDanCu = news.HoiVienDanCu;
            old.HoiVienNganhNghe = news.HoiVienNganhNghe;
          
            return old;
        }

        #endregion Import
        #region Export 

        [HoiNongDanAuthorization]
        public IActionResult ExportCreate()
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\HoiVienDangKyNew.xlsx");
            List<HVDangKyImportNewVM> data = new List<HVDangKyImportNewVM>();
            return ExportNew(data, url, startIndex);
        }
        private FileContentResult Export(List<HVDangKyImportVM> data,string url,int startIndex) {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "ID", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Nam", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Nu", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoCCCD", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayCapSoCCCD", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoKhauThuongTru", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NoiOHiennay", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoDienThoai", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DangVien", isBoolean = true, isComment = true, isAllowedToEdit = true, strComment = "để X là đảng viên" });

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

            //columns.Add(new ExcelTemplate() { ColumnName = "NgayThangVaoHoi", isAllowedToEdit = true, isText = true, strComment = "Nhập ngày tháng năm theo số quyết định" });

            var ngheNghiep = _context.NgheNghieps.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaNgheNghiep, Name = x.TenNgheNghiep }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaNgheNghiep", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = ngheNghiep, TypeId = ConstExcelController.StringId });

            columns.Add(new ExcelTemplate() { ColumnName = "DiaBanDanCu", isAllowedToEdit = true, isText = true, strComment = "Nhập X là hội viên dân cư" });
            columns.Add(new ExcelTemplate() { ColumnName = "NganhNghe", isAllowedToEdit = true, isText = true, strComment = "Nhập X là hội viên ngành nghề" });

            //columns.Add(new ExcelTemplate() { ColumnName = "SoThe", isAllowedToEdit = true, isText = true});
            //columns.Add(new ExcelTemplate() { ColumnName = "NgayCapThe", isAllowedToEdit = true, isText = true });

            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.HVDangKy);
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
            byte[] filecontent = ClassExportExcel.ExportExcel( url,data,  columns, heading, true, startIndex);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        public IActionResult ExportEdit(HVDangKySearchVM search)
        {
            search.TuNgay = search.TuNgay == null ? FirstDate() : search.TuNgay;
            search.DenNgay = search.DenNgay == null ? LastDate() : search.DenNgay;
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
                        && it.hv.NgayDangKy >= search.TuNgay
                        && it.hv.NgayDangKy <= search.DenNgay
                    ).AsQueryable();
            if (!String.IsNullOrWhiteSpace(search.SoCCCD))
            {
                data = data.Where(it => it.hv.SoCCCD == search.SoCCCD);
            }
            if (!String.IsNullOrWhiteSpace(search.HoVaTen))
            {
                data = data.Where(it => it.hv.HoVaTen == search.HoVaTen);
            }
            if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen))
            {
                data = data.Where(it => it.hv.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
            }
            if (search.MaDiaBanHoiVien != null)
            {
                data = data.Where(it => it.hv.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
            }
            if (search.TrangThai == "1")
            {
                data = data.Where(it => it.hv.HoiVienDuyet != true && it.hv.TuChoi != true);
            }
            if (search.TrangThai == "2")
            {
                data = data.Where(it => it.hv.HoiVienDuyet == true);
            }
            if (search.TrangThai == "3")
            {
                data = data.Where(it => it.hv.TuChoi == true);
            }
            var model = data.Select(it => new HVDangKyImportNewVM
            {
                ID = it.hv.IDCanBo,
                HoVaTen = it.hv.HoVaTen,
                NgaySinh = it.hv.NgaySinh,
                GioiTinh = (int)it.hv.GioiTinh == 1?"X":"",
                ChucVu= it.hv.ChucVu!.TenChucVu,
                SoCCCD = it.hv.SoCCCD,
                NgayCapSoCCCD = it.hv.NgayCapCCCD,
                TenToHoi = it.hv.ToHoi!.TenToHoi,
                TenChiHoi = it.hv.ChiHoi!.TenChiHoi,
                HoKhauThuongTru = it.hv.HoKhauThuongTru,
                HoKhauThuongTru_XaPhuong = it.hv.HoKhauThuongTru_XaPhuong,
                HoKhauThuongTru_QuanHuyen = it.hv.HoKhauThuongTru_QuanHuyen,
                ChoOHienNay = it.hv.ChoOHienNay,
                ChoOHienNay_XaPhuong = it.hv.ChoOHienNay_XaPhuong,
                ChoOHienNay_QuanHuyen = it.hv.ChoOHienNay_QuanHuyen,
                SoDienThoai = it.hv.SoDienThoai,
                DangVienDuBi = it.hv.NgayvaoDangDuBi,
                DangVienChinhThuc = it.hv.NgayVaoDangChinhThuc,
                MaDanToc = it.hv.DanToc!.TenDanToc,
                MaTonGiao = it.hv.TonGiao!.TenTonGiao,
                MaTrinhDoHocVan = it.hv.TrinhDoHocVan.TenTrinhDoHocVan,
                MaTrinhDoChuyenMon = it.hv.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                MaTrinhDoChinhTri = it.hv.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                NgayThamGiaCapUyDang = it.hv.NgayThamGiaCapUyDang,
                NgayThamGiaHDND = it.hv.NgayThamGiaHDND,
                VaiTro = it.hv.VaiTro=="1"?"X":"",
                VaiTroKhac = it.hv.VaiTroKhac ,
                MaGiaDinhThuocDien = it.hv.GiaDinhThuocDien!.TenGiaDinhThuocDien,
                GiaDinhThuocDienKhac = it.hv.GiaDinhThuocDienKhac,
                MaNgheNghiep = it.hv.NgheNghiep!.TenNgheNghiep,
                SX_ChN = it.hv.Loai_DV_SX_ChN,
                SoLuong = it.hv.SoLuong,
                DienTich_QuyMo = it.hv.DienTich_QuyMo,
                ThamGia_CLB_DN_HTX = String.Join(";",it.hv.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Select(it=>it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten).ToList()),
                SinhHoatDoanTheChinhTri = String.Join(";",it.hv.DoanTheChinhTri_HoiDoan_HoiViens.Select(it=>it.DoanTheChinhTri_HoiDoan.TenDoanTheChinhTri_HoiDoan))

            }).ToList();

            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\HoiVienDangKyNew.xlsx");

            return ExportNew(model, url,startIndex); ;
        }
        private FileContentResult ExportNew(List<HVDangKyImportNewVM> data, string url, int startIndex)
        {
            List<Guid> maChucVu = new List<Guid>{Guid.Parse("E7B617C6-9926-478A-947A-AEB0D76E5650"),Guid.Parse("D710D930-8342-474B-90A4-A1170A7A5691")
                ,Guid.Parse("E0107684-AB70-4D84-8463-651AB80BAA30"),Guid.Parse("32D1B70C-80F3-4370-B7A5-202942ACD397") };

            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "ID", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinh", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GioiTinh", isAllowedToEdit = true, isText = true });

            var chucVus = _context.ChucVus.ToList().Where(it => maChucVu.Contains(it.MaChucVu) && it.HoiVien == true).Select(x => new DropdownIdTypeStringModel { Id = x.MaChucVu.ToString(), Name = x.TenChucVu }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "ChucVu", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chucVus, TypeId = ConstExcelController.StringId, Title = "Chức Vụ" });
            columns.Add(new ExcelTemplate() { ColumnName = "SoCCCD", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayCapSoCCCD", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "TenToHoi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "TenChiHoi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoKhauThuongTru", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoKhauThuongTru_XaPhuong", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoKhauThuongTru_QuanHuyen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay_XaPhuong", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay_QuanHuyen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoDienThoai", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DangVienDuBi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DangVienChinhThuc", isAllowedToEdit = true, isText = true });

            var danToc = _context.DanTocs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaDanToc, Name = x.TenDanToc }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaDanToc", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = danToc, TypeId = ConstExcelController.StringId,Title="Dân Tộc"});

            var tonGiao = _context.TonGiaos.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTonGiao, Name = x.TenTonGiao }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTonGiao", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tonGiao, TypeId = ConstExcelController.StringId,Title = "Tôn Giáo" });

            var hocVan = _context.TrinhDoHocVans.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoHocVan, Name = x.TenTrinhDoHocVan }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoHocVan", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = hocVan, TypeId = ConstExcelController.StringId,Title = "Trình độ học vấn" });

            var chuyenNganh = _context.TrinhDoChuyenMons.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChuyenMon, Name = x.TenTrinhDoChuyenMon }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChuyenMon", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chuyenNganh, TypeId = ConstExcelController.StringId,Title = "Chuyên môn" });

            var chinhTri = _context.TrinhDoChinhTris.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChinhTri, Name = x.TenTrinhDoChinhTri }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChinhTri", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chinhTri, TypeId = ConstExcelController.StringId,Title = "Chính trị" });

            columns.Add(new ExcelTemplate() { ColumnName = "NgayThamGiaCapUyDang", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayThamGiaHDND", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "VaiTro", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "VaiTroKhac", isAllowedToEdit = true, isText = true });

            var giaDinh = _context.GiaDinhThuocDiens.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaGiaDinhThuocDien, Name = x.TenGiaDinhThuocDien }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaGiaDinhThuocDien", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = giaDinh, TypeId = ConstExcelController.StringId,Title = "Gia đình thuộc diện" });
            columns.Add(new ExcelTemplate() { ColumnName = "GiaDinhThuocDienKhac", isAllowedToEdit = true, isText = true });

            var ngheNghiep = _context.NgheNghieps.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaNgheNghiep, Name = x.TenNgheNghiep }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaNgheNghiep", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = ngheNghiep, TypeId = ConstExcelController.StringId,Title = "Nghề nghiệp"});

            columns.Add(new ExcelTemplate() { ColumnName = "SX_ChN", isAllowedToEdit = true, isText = true});
            columns.Add(new ExcelTemplate() { ColumnName = "SoLuong", isAllowedToEdit = true, isText = true});
            columns.Add(new ExcelTemplate() { ColumnName = "DienTich_QuyMo", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SinhHoatDoanTheChinhTri", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ThamGia_CLB_DN_HTX", isAllowedToEdit = true, isText = true });

            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.HVDangKy);
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
            byte[] filecontent = ClassExportExcel.ExportExcel(url, data, columns, heading, true, startIndex);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.CanBos.FirstOrDefault(p => p.IDCanBo == id && p.IsHoiVien == true && p.HoiVienDuyet != true && p.AccountIdDangKy == AccountId());


                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.HVDangKy.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.HVDangKy.ToLower())
                    });
                }
            });
        }
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAll(DateTime TuNgay, DateTime DenNgay) {
            return ExecuteDelete(() =>
            {
                var del = _context.CanBos.FirstOrDefault(p => p.NgayDangKy >=TuNgay && p.NgayDangKy <= DenNgay && p.IsHoiVien == true && p.HoiVienDuyet != true && p.AccountIdDangKy == AccountId());


                if (del != null)
                {
                    _context.RemoveRange(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.HVDangKy.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.HVDangKy.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper

        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }

        private List<HVDangKyExcelVM> LoadData(HVDangKySearchVM search)
        {
            search.TuNgay = search.TuNgay == null ? FirstDate() : search.TuNgay;
            search.DenNgay = search.DenNgay == null ? LastDate() : search.DenNgay;
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
                        && it.hv.NgayDangKy >= search.TuNgay
                        && it.hv.NgayDangKy <= search.DenNgay
                    ).AsQueryable();
            if (!String.IsNullOrWhiteSpace(search.SoCCCD))
            {
                data = data.Where(it => it.hv.SoCCCD == search.SoCCCD);
            }
            if (!String.IsNullOrWhiteSpace(search.HoVaTen))
            {
                data = data.Where(it => it.hv.HoVaTen == search.HoVaTen);
            }
            if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen))
            {
                data = data.Where(it => it.hv.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
            }
            if (search.MaDiaBanHoiVien != null)
            {
                data = data.Where(it => it.hv.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
            }
            if (search.TrangThai == "1")
            {
                data = data.Where(it => it.hv.HoiVienDuyet != true && it.hv.TuChoi != true);
            }
            if (search.TrangThai == "2")
            {
                data = data.Where(it => it.hv.HoiVienDuyet == true);
            }
            if (search.TrangThai == "3")
            {
                data = data.Where(it => it.hv.TuChoi == true);
            }
            var model = data.OrderBy(it => it.hv.CreatedTime).Select(it => new HVDangKyExcelVM
            {
                ID = it.hv.IDCanBo,
                HoVaTen = it.hv.HoVaTen,
                Nam = (int)it.hv.GioiTinh == 1 ? it.hv.NgaySinh : "",
                Nu = (int)it.hv.GioiTinh == 0 ? it.hv.NgaySinh : "",
                SoCCCD = it.hv.SoCCCD,
                NgayCapSoCCCD = it.hv.NgayCapCCCD,
                HoKhauThuongTru = it.hv.HoKhauThuongTru + (String.IsNullOrWhiteSpace(it.hv.HoKhauThuongTru_XaPhuong) == false ? "," + it.hv.HoKhauThuongTru_XaPhuong : "") + (String.IsNullOrWhiteSpace(it.hv.HoKhauThuongTru_QuanHuyen) == false ? ", " + it.hv.HoKhauThuongTru_QuanHuyen : ""),
                NoiOHiennay = it.hv.ChoOHienNay + (String.IsNullOrWhiteSpace(it.hv.ChoOHienNay_XaPhuong) == false ? "," + it.hv.ChoOHienNay_XaPhuong : "") + (String.IsNullOrWhiteSpace(it.hv.ChoOHienNay_QuanHuyen) == false ? ", " + it.hv.ChoOHienNay_QuanHuyen : ""),
                SoDienThoai = it.hv.SoDienThoai,
                DangVien = it.hv.DangVien == true ? "X" : "",
                DanToc = it.hv.DanToc!.TenDanToc,
                TonGiao = it.hv.TonGiao!.TenTonGiao,
                TenHoi = it.hv.DiaBanHoatDong!.TenDiaBanHoatDong,
                TenQuanHuyen = it.hv.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen,
                TrinhDoHocVan = it.hv.TrinhDoHocVan.TenTrinhDoHocVan,
                TrinhDoChuyenMon = it.hv.TrinhDoChuyenMon!.TenTrinhDoChuyenMon + " " + it.hv.ChuyenNganh,
                ChinhTri = it.hv.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                NgayThangVaoHoi = it.hv.NgayVaoHoi,
                NgheNghiep = it.hv.NgheNghiep!.TenNgheNghiep,
                DiaBanDanCu = it.hv.HoiVienDanCu == true ? "X" : "",
                NganhNghe = it.hv.HoiVienNganhNghe == true ? "X" : "",
                SoThe = it.hv.MaCanBo,
                NgayCapThe = "",
                TrangThai = it.hv.TuChoi != true && it.hv.HoiVienDuyet != true ? "1" : it.hv.HoiVienDuyet == true ? "2" : "3",
                LyDoTuChoi = it.hv.LyDoTuChoi
            }).ToList();
            var listID = model.Where(it => it.TrangThai == "1").Select(it => it.ID).ToList();
            var thonThongTinNguoiDuyet = (from lsd in _context.HoiVienLichSuDuyets
                                          join account in _context.Accounts on lsd.AccountID equals account.AccountId
                                          where lsd.TrangThaiDuyet == false && listID.Contains(lsd.IDHoiVien)
                                          select new { lsd.IDHoiVien, account.FullName }).ToList();
            int stt = 0;
            foreach (var item in model)
            {
                stt ++;
                item.STT = stt;
                if (item.TrangThai != "1")
                    continue;
                var updateNguoiDuyet = thonThongTinNguoiDuyet.Where(it => it.IDHoiVien == item.ID).Select(it=>it.FullName).ToList();
                if (updateNguoiDuyet.Count()>0)
                {
                    item.NguoiDuyet = String.Join(";",updateNguoiDuyet) ;
                }
            }

            return model.OrderBy(it=>it.TenQuanHuyen).ThenBy(it=>it.TenHoi).ToList();

        }

        private void CreateViewBag(string? maTrinhDoHocVan = null, string? maTrinhDoChuyenMon = null,string? maTrinhDoChinhTri = null,
            string? maDanToc = null, String? maTonGiao = null,string? maNgheNghiep = null,Guid? maDiaBanHoiVien = null,string? 
            MaGiaDinhThuocDien = null,Guid? MaChiHoi = null, Guid? MaToHoi = null, List<Guid>? MaDoanTheChinhTri_HoiDoan = null, List<Guid>? Id_CLB_DN_MH_HTX_THT = null)
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
           
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());
            ViewBag.MaChiHoi = fnViewBag.ChiHoi(value:MaChiHoi, AccountId());
            ViewBag.MaToHoi = fnViewBag.ToHoi(value:MaToHoi, AccountId());
            ViewBag.MaGiaDinhThuocDien = fnViewBag.GiaDinhThuocDien(value: MaGiaDinhThuocDien);
            ViewBag.MaTrinhDoHocVan = fnViewBag.TrinhDoHocVan(maTrinhDoHocVan);
            ViewBag.MaTrinhDoChuyenMon = fnViewBag.TrinhDoChuyenMon(maTrinhDoChuyenMon);

            ViewBag.MaTrinhDoChinhTri = fnViewBag.TrinhDoChinhTri(maTrinhDoChinhTri);
            ViewBag.MaDanToc = fnViewBag.DanToc(maDanToc);
            ViewBag.MaTonGiao = fnViewBag.TonGiao(maTonGiao);
            ViewBag.MaNgheNghiep = fnViewBag.NgheNghiep(value: maNgheNghiep);

            ViewBag.Id_CLB_DN_MH_HTX_THT = fnViewBag.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac(value: Id_CLB_DN_MH_HTX_THT);

            ViewBag.MaDoanTheChinhTri_HoiDoan = fnViewBag.DoanTheChinhTri_HoiDoan(value: MaDoanTheChinhTri_HoiDoan);
        }

        private void CheckNguoiDuyet(Account account) {
            if (String.IsNullOrWhiteSpace(account!.AccountIDParent)) {
                ModelState.AddModelError("", LanguageResource.ErrorDuyetHoiVien);
            }

        }

        private String CapNhatDanhMuc(List<DoanTheChinhTri_HoiDoan> doanTheChinhTri_HoiDoans,List<ChiHoi> chiHois,List<ToHoi> toHois,List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac> cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs,string HoVaTen)
        {
            try
            {
                if (cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.Count() > 0)
                {
                    _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.AddRange(cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs);
                }
                if (doanTheChinhTri_HoiDoans.Count() > 0)
                {
                    _context.DoanTheChinhTri_HoiDoans.AddRange(doanTheChinhTri_HoiDoans);
                }
                if (chiHois.Count() > 0)
                {

                    _context.ChiHois.AddRange(chiHois);
                }
                if (toHois.Count() > 0)
                {

                    _context.ToHois.AddRange(toHois);
                }
                _context.SaveChanges();
                return LanguageResource.ImportSuccess;
            }
            catch (Exception ex)
            {
                return "Lỗi cập nhật danh mục có tên " + HoVaTen + " " + ex.Message;

            }

        }
        private string ExecuteImportExcelHoiVien(CanBo canbo, bool bEdit,List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien> cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens,
            List<DoanTheChinhTri_HoiDoan_HoiVien> doanTheChinhTri_HoiDoan_HoiViens, List<Guid>? nguoiDuyets = null)
        {
            //Check:
            //1. If MenuId == "" then => Insert
            //2. Else then => Update

            if (!bEdit)
            {
                try
                {
                    canbo.Level = "90";
                    canbo.IsHoiVien = true;
                    canbo.HoiVienDuyet = false;
                    canbo.Actived = true;
                    canbo.TuChoi = false;
                    canbo.CreatedTime = DateTime.Now;
                    if (cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Count() > 0)
                    {
                        _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.AddRange(cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens);
                    }
                    if (doanTheChinhTri_HoiDoan_HoiViens.Count() > 0)
                    {
                        _context.DoanTheChinhTri_HoiDoan_HoiViens.AddRange(doanTheChinhTri_HoiDoan_HoiViens);
                    }

                    List<HoiVienLichSuDuyet> hoiVienLichSuDuyets = new List<HoiVienLichSuDuyet>();
                    nguoiDuyets!.ForEach(item => {
                        hoiVienLichSuDuyets.Add(new HoiVienLichSuDuyet
                        {
                            ID = Guid.NewGuid(),
                            IDHoiVien = canbo.IDCanBo,
                            AccountID = item,
                            CreateTime = DateTime.Now,
                            TrangThaiDuyet = false,
                        });
                    });
                    _context.HoiVienLichSuDuyets.AddRange(hoiVienLichSuDuyets);
                    _context.Entry(canbo).State = EntityState.Added;
                }
                catch
                {

                }
            }
            else
            {
                var editHoiVien = _context.CanBos.Include(it=>it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens).Include(it=>it.DoanTheChinhTri_HoiDoan_HoiViens).FirstOrDefault(p => p.IDCanBo == canbo.IDCanBo && p.MaDiaBanHoatDong == canbo.MaDiaBanHoatDong);
                if (editHoiVien != null)
                {
                    //canbo = HoiVienExcel.GetHoiVien(canbo);

                    editHoiVien.HoVaTen = canbo.HoVaTen;
                    editHoiVien.NgaySinh = canbo.NgaySinh;
                    editHoiVien.GioiTinh = canbo.GioiTinh;
                    editHoiVien.MaChucVu = canbo.MaChucVu;

                    editHoiVien.SoCCCD = canbo.SoCCCD;
                    editHoiVien.NgayCapCCCD = canbo.NgayCapCCCD;
                    editHoiVien.MaToHoi = canbo.MaToHoi;
                    editHoiVien.MaChiHoi = canbo.MaChiHoi;
                    editHoiVien.HoKhauThuongTru = canbo.HoKhauThuongTru;
                    editHoiVien.HoKhauThuongTru_XaPhuong = canbo.HoKhauThuongTru_XaPhuong;
                    editHoiVien.HoKhauThuongTru_QuanHuyen = canbo.HoKhauThuongTru_QuanHuyen;

                    editHoiVien.ChoOHienNay = canbo.ChoOHienNay;
                    editHoiVien.ChoOHienNay_XaPhuong = canbo.ChoOHienNay_XaPhuong;
                    editHoiVien.ChoOHienNay_QuanHuyen = canbo.ChoOHienNay_QuanHuyen;
                    editHoiVien.SoDienThoai = canbo.SoDienThoai;
                    editHoiVien.NgayvaoDangDuBi = canbo.NgayvaoDangDuBi;
                    editHoiVien.NgayVaoDangChinhThuc = canbo.NgayVaoDangChinhThuc;
                    editHoiVien.DangVien = canbo.DangVien;
                    editHoiVien.MaDanToc = canbo.MaDanToc;
                    editHoiVien.MaTonGiao = canbo.MaTonGiao;
                    editHoiVien.MaTrinhDoHocVan = canbo.MaTrinhDoHocVan;
                    editHoiVien.MaTrinhDoChuyenMon = canbo.MaTrinhDoChuyenMon;
                    editHoiVien.MaTrinhDoChinhTri = canbo.MaTrinhDoChinhTri;
                    editHoiVien.NgayThamGiaCapUyDang = canbo.NgayThamGiaCapUyDang;
                    editHoiVien.NgayThamGiaHDND = canbo.NgayThamGiaHDND;
                    editHoiVien.VaiTro = canbo.VaiTro;
                    editHoiVien.VaiTroKhac = canbo.VaiTroKhac;
                    editHoiVien.MaGiaDinhThuocDien = canbo.MaGiaDinhThuocDien;
                    editHoiVien.GiaDinhThuocDienKhac = canbo.GiaDinhThuocDienKhac;
                    editHoiVien.MaNgheNghiep = canbo.MaNgheNghiep;
                    editHoiVien.Loai_DV_SX_ChN = canbo.Loai_DV_SX_ChN;
                    editHoiVien.SoLuong = canbo.SoLuong;
                    editHoiVien.DienTich_QuyMo = canbo.DienTich_QuyMo;

                    editHoiVien.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Clear();
                    editHoiVien.DoanTheChinhTri_HoiDoan_HoiViens.Clear();
                    HistoryModelRepository history = new HistoryModelRepository(_context);
                    history.SaveUpdateHistory(editHoiVien.IDCanBo.ToString(), AccountId()!.Value, canbo);

                  

                    if (cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Count() > 0)
                    {
                        editHoiVien.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.AddRange(cauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens);
                    }
                    if (doanTheChinhTri_HoiDoan_HoiViens.Count() > 0)
                    {
                        editHoiVien.DoanTheChinhTri_HoiDoan_HoiViens.AddRange(doanTheChinhTri_HoiDoan_HoiViens);
                    }
                    editHoiVien.LastModifiedAccountId = AccountId();
                    editHoiVien.LastModifiedTime = DateTime.Now;
                    _context.Entry(editHoiVien).State = EntityState.Modified;
                }
                else
                {
                    return string.Format("Không tồn tại hội viên có tên {0} ", canbo.HoVaTen);
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
        #endregion Helper

    }
}
