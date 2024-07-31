using HoiNongDan.DataAccess;
using HoiNongDan.Models;
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
        public SelectList HinhThucKhenThuong(string? value = null) {
            var hinhThucKhenThuongs = _context.HinhThucKhenThuongs.Select(it => new { MaHinhThucKhenThuong = it.MaHinhThucKhenThuong, TenHinhThucKhenThuong = it.TenHinhThucKhenThuong }).ToList();
            return new SelectList(hinhThucKhenThuongs, "MaHinhThucKhenThuong", "TenHinhThucKhenThuong", value);
        }
        public SelectList DanhHieuKhenThuong( string? value = null)
        {
            var danhHieuKhenThuongs = _context.DanhHieuKhenThuongs.Where(it=>it.IsHoiVien ==true).Select(it => new { MaDanhHieuKhenThuong = it.MaDanhHieuKhenThuong, TenHinhThucKhenThuong = it.TenDanhHieuKhenThuong }).ToList();

            return new SelectList(danhHieuKhenThuongs, "MaDanhHieuKhenThuong", "TenHinhThucKhenThuong", value);
        }
        public SelectList DiaBanHoiVien(Guid? value = null,Guid? acID = null)
        {
            var diaBans = (from db in _context.DiaBanHoatDongs
                                       join pv in _context.PhamVis on db.Id equals pv.MaDiabanHoatDong
                                       where pv.AccountId == acID!.Value
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
    }
}