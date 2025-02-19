using HoiNongDan.DataAccess;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;



namespace HoiNongDan.Extensions
{
    public class FnViewBag
    {
        private readonly AppDbContext _context;
        public FnViewBag(AppDbContext dbContext)
        { 
            _context = dbContext;
        }

        public SelectList ChucVu(Guid? value = null)
        {
            var chucVus = _context.ChucVus.Select(it => new { MaChucVu = it.MaChucVu, TenChucVu = it.TenChucVu }).ToList();
            return new SelectList(chucVus, "MaChucVu", "TenChucVu", value);
        }

        public SelectList TrinhDoHocVan(string? value = null)
        {
            var trinhDos = _context.TrinhDoHocVans.Select(it => new { MaTrinhDoHocVan = it.MaTrinhDoHocVan, TenTrinhDoHocVan = it.TenTrinhDoHocVan }).ToList();
            return new SelectList(trinhDos, "MaTrinhDoHocVan", "TenTrinhDoHocVan", value);
        }

        public SelectList TrinhDoChuyenMon(string? value = null)
        {
            var trinhDos = _context.TrinhDoChuyenMons.Select(it => new { MaTrinhDoChuyenMon = it.MaTrinhDoChuyenMon, TenTrinhDoChuyenMon = it.TenTrinhDoChuyenMon }).ToList();
            return new SelectList(trinhDos, "MaTrinhDoChuyenMon", "TenTrinhDoChuyenMon", value);
        }

        public SelectList TrinhDoChinhTri(string? value = null)
        {
            var trinhDoChinhTri = _context.TrinhDoChinhTris.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTrinhDoChinhTri = it.MaTrinhDoChinhTri, TenTrinhDoChinhTri = it.TenTrinhDoChinhTri }).ToList();
            return new SelectList(trinhDoChinhTri, "MaTrinhDoChinhTri", "TenTrinhDoChinhTri", value);
        }

        public SelectList DanToc(string? value = null)
        {
            var danToc = _context.DanTocs.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaDanToc = it.MaDanToc, TenDanToc = it.TenDanToc }).ToList();
            return new SelectList(danToc, "MaDanToc", "TenDanToc", value);
        }
        public SelectList TonGiao(string? value = null)
        {
            var tonGiao = _context.TonGiaos.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTonGiao = it.MaTonGiao, TenTonGiao = it.TenTonGiao }).ToList();
            return new SelectList(tonGiao, "MaTonGiao", "TenTonGiao", value);
        }
        public SelectList HinhThucKhenThuong(string? value = null) {
            var hinhThucKhenThuongs = _context.HinhThucKhenThuongs.Select(it => new { MaHinhThucKhenThuong = it.MaHinhThucKhenThuong, TenHinhThucKhenThuong = it.TenHinhThucKhenThuong }).ToList();
            return new SelectList(hinhThucKhenThuongs, "MaHinhThucKhenThuong", "TenHinhThucKhenThuong", value);
        }
        public SelectList DanhHieuKhenThuong( string? value = null)
        {
            var danhHieuKhenThuongs = _context.DanhHieuKhenThuongs.Where(it=>it.IsHoiVien ==true).Select(it => new { MaDanhHieuKhenThuong = it.MaDanhHieuKhenThuong, TenHinhThucKhenThuong = it.TenDanhHieuKhenThuong }).ToList();

            return new SelectList(danhHieuKhenThuongs, "MaDanhHieuKhenThuong", "TenHinhThucKhenThuong", value);
        }
        public SelectList DiaBanHoatDong(Guid? value = null, Guid? acID = null)
        {
            var diaBans = (from db in _context.DiaBanHoatDongs
                           join pv in _context.PhamVis on db.Id equals pv.MaDiabanHoatDong
                           where pv.AccountId == acID!.Value && db.Actived == true
                           select new
                           {
                               MaDiaBanHoatDong = db.Id,
                               Name = db.TenDiaBanHoatDong
                           }).Distinct().ToList();
            return new SelectList(diaBans, "MaDiaBanHoatDong", "Name", value);
        }
        public SelectList HocVi(string? value = null)
        {
            var hocVis = _context.HocVis.Select(it => new { MaHocVi = it.MaHocVi, TenHocVi = it.TenHocVi }).ToList();

            return new SelectList(hocVis, "MaHocVi", "TenHocVi", value);
        }

        public SelectList ChiHoi(Guid? value = null, Guid? acID = null)
        {
            var phamVis = _context.PhamVis.Where(it => it.AccountId == acID).Select(it => it.MaDiabanHoatDong).ToList();
            var chiHois = _context.ChiHois.Where(it=> phamVis.Contains(it.MaDiaBanHoatDong!.Value)).Select(it => new { MaChiHoi = it.MaChiHoi, TenChiHoi = it.TenChiHoi }).ToList();

            return new SelectList(chiHois, "MaChiHoi", "TenChiHoi", value);
        }
        public SelectList ToHoi(Guid? value = null, Guid? acID = null)
        {
            var phamVis = _context.PhamVis.Where(it => it.AccountId == acID).Select(it => it.MaDiabanHoatDong).ToList();
            var toHois = _context.ToHois.Where(it => phamVis.Contains(it.MaDiaBanHoatDong!.Value)).Select(it => new { MaToHoi = it.MaToHoi, TenToHoi = it.TenToHoi }).ToList();

            return new SelectList(toHois, "MaToHoi", "TenToHoi", value);
        }

        public SelectList NgheNghiep(string? value = null)
        {
            var ngheNghieps = _context.NgheNghieps.Select(it => new { MaNgheNghiep = it.MaNgheNghiep, TenNgheNghiep = it.TenNgheNghiep }).ToList();

            return new SelectList(ngheNghieps, "MaNgheNghiep", "TenNgheNghiep", value);
        }
        public SelectList GiaDinhThuocDien(string? value = null)
        {
            var giadinhs = _context.GiaDinhThuocDiens.Select(it => new { MaGiaDinhThuocDien = it.MaGiaDinhThuocDien, TenGiaDinhThuocDien = it.TenGiaDinhThuocDien }).ToList();

            return new SelectList(giadinhs, "MaGiaDinhThuocDien", "TenGiaDinhThuocDien", value);
        }
        public MultiSelectList DoanTheChinhTri_HoiDoan(List<Guid>? value = null)
        {
            var hoiDoans = _context.DoanTheChinhTri_HoiDoans.Where(it => it.Actived == true).Select(it => new
            {
                it.MaDoanTheChinhTri_HoiDoan,
                it.TenDoanTheChinhTri_HoiDoan
            }).ToList();

            return new MultiSelectList(hoiDoans, "MaDoanTheChinhTri_HoiDoan", "TenDoanTheChinhTri_HoiDoan", value);
        }

        public MultiSelectList CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac(List<Guid>? value = null)
        {
            var cauLacBo = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.Where(it => it.Actived == true).Select(it => new
            {
                it.Id_CLB_DN_MH_HTX_THT,
                it.Ten
            }).ToList();

            return new MultiSelectList(cauLacBo, "Id_CLB_DN_MH_HTX_THT", "Ten", value);
        }

        public MultiSelectList ToHoiNganhNghe_ChiHoiNganhNghe(List<Guid>? value = null)
        {
            var toHoiNganhNghe = _context.ToHoiNganhNghe_ChiHoiNganhNghes.Where(it => it.Actived == true).Select(it => new
            {
                it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe,
                it.Ten
            }).ToList();

            return new MultiSelectList(toHoiNganhNghe, "Ma_ToHoiNganhNghe_ChiHoiNganhNghe", "Ten", value);
        }

        public SelectList DiaBanHoiVien(Guid? value = null,Guid? acID = null)
        {
            var diaBans = (from db in _context.DiaBanHoatDongs
                                       join pv in _context.PhamVis on db.Id equals pv.MaDiabanHoatDong
                                       where pv.AccountId == acID!.Value && db.Actived == true
                                       select new {
                                           MaDiaBanHoiVien = db.Id ,
                                           TenDiaBanHoatDong = db.TenDiaBanHoatDong
                                       }).Distinct().ToList();
            return new SelectList(diaBans, "MaDiaBanHoiVien", "TenDiaBanHoatDong", value);
        }
        public SelectList QuanHuyen(String? value = null, Guid? idAc = null) {
            var quanHuyens = (from db in _context.DiaBanHoatDongs
                                  join qh in _context.QuanHuyens on db.MaQuanHuyen equals qh.MaQuanHuyen
                                  join pv in _context.PhamVis on db.Id equals pv.MaDiabanHoatDong
                                  where pv.AccountId == idAc.Value
                                  select new
                                  {
                                      qh.MaQuanHuyen,
                                      qh.TenQuanHuyen,
                                  }).Distinct().ToList();
            return new SelectList(quanHuyens, "MaQuanHuyen", "TenQuanHuyen", value);
        }
        public SelectList NguonVon(Guid? value = null)
        {
            var nguonVon = _context.NguonVons.Select(it => new { MaNguonVon = it.MaNguonVon, TenNguonVon = it.TenNguonVon }).ToList();
            return new SelectList(nguonVon, "MaNguonVon", "TenNguonVon", value);
        }
        public SelectList HinhThucHoTro(Guid? value = null) {
            var hinhThucHoTros = _context.HinhThucHoTros.Select(it => new { MaHinhThucHoTro = it.MaHinhThucHoTro, TenHinhThuc = it.TenHinhThuc }).ToList();
            return new SelectList(hinhThucHoTros, "MaHinhThucHoTro", "TenHinhThuc", value);
        }
        public SelectList LoaiQuanHeGiaDinh(Guid? value = null)
        {
            var MenuList = _context.LoaiQuanHeGiaDinhs.Where(it => it.Actived == true).Select(it => new { IDLoaiQuanHeGiaDinh = it.IDLoaiQuanHeGiaDinh, TenLoaiQuanHeGiaDinh = it.TenLoaiQuanHeGiaDinh }).ToList();
            return new SelectList(MenuList, "IDLoaiQuanHeGiaDinh", "TenLoaiQuanHeGiaDinh", value);
        }
        public SelectList ChiToHoi(Guid? value = null)
        {
            var chiToHoiNganhNghe = _context.ToHoiNganhNghe_ChiHoiNganhNghes.Select(it => new {
                IDMaChiToHoi = it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe,
                Ten = it.Ten
            }).ToList();
            return new SelectList(chiToHoiNganhNghe, "IDMaChiToHoi", "Ten", value);
        }
        public SelectList ToHoiChiHoiNganNghe() { 
            var toHoi_ChiHoi = _context.ToHoiNganhNghe_ChiHoiNganhNghes.ToList();
           return new SelectList(toHoi_ChiHoi, "Ma_ToHoiNganhNghe_ChiHoiNganhNghe", "Ten");
        }

        public SelectList LopHoc(Guid? value = null)
        {
            var lopHocs = _context.LopHocs.ToList();
            return new SelectList(lopHocs, "IDLopHoc", "TenLopHoc", value);
        }
        public SelectList CapKhenThuong(String? value = null)
        {
            var capKhenThuongs = _context.CapKhenThuongs.ToList();
            return new SelectList(capKhenThuongs, "MaCapKhenThuong", "TenCapKhenThuong", value);
        }
    }
}