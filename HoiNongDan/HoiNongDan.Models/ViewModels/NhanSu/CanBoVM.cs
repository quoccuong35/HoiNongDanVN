﻿using HoiNongDan.Models.Entitys.MasterData;
using HoiNongDan.Models.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class CanBoVM
    {
        public Guid? IDCanBo { get; set; }
       
        [MaxLength(20)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaDinhDanh")]
        public string? MaDinhDanh { get; set; }

        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string HoVaTen { get; set; }
        
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        //[DataType(DataType.Date)]
        public String? NgaySinh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        public GioiTinh GioiTinh { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhTrang")]
        public string? MaTinhTrang { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhanHe")]
        [MaxLength(50)]
        public string? MaPhanHe { get; set; }


        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        public Guid? IdCoSo { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public Guid? IdDepartment { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public Guid? MaChucVu { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
       [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DonVi")]
        public String? DonVi { get; set; }
        [MaxLength(50)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public String? SoCCCD { get; set; }


        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayCapCCCD")]
        public String? NgayCapCCCD { get; set; }

       
        [MaxLength(100)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoDienThoai")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? SoDienThoai { get; set; }

        [MaxLength(200)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Email")]
        public string? Email { get; set; }

        [MaxLength(50)]
       
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgachLuong")]
        public string? MaNgachLuong { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BacLuong")]
        public Guid? MaBacLuong { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSo")]
        public decimal? HeSoLuong { get; set; }
        [DataType(DataType.Date)]

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayNangBacLuong")]
        public DateTime? NgayNangBacLuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuCapChucVu")]
        public decimal? PhuCapChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuCapVuotKhung")]
        public decimal? PhuCapVuotKhung { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuCapKiemNhiem")]
        public decimal? PhuCapKiemNhiem { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuCapKhuVuc")]
        public decimal? PhuCapKhuVuc { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LuongKhoan")]
        public int? LuongKhoan { get; set; }
        
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "KhoanTuNgay")]
        public DateTime? KhoanTuNgay { get; set; }
       
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "KhoanDenNgay")]
        public DateTime? KhoanDenNgay { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [MaxLength(100)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoBHXH")]
        public string? SoBHXH { get; set; }
       
        [MaxLength(100)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoBHYT")]
        public string? SoBHYT { get; set; }
        
        [MaxLength(100)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaSoThue")]
        public string? MaSoThue { get; set; }
        
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoBienChe")]
        public DateTime? NgayVaoBienChe { get; set; }
        
        //[DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaCongTac")]
        public String? NgayThamGiaCongTac { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeDaoTao")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        public string? MaHeDaoTao { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoHocVan")]
        [MaxLength(50)]
        public string? MaTrinhDoHocVan { get; set; }

        [MaxLength(20)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChuyenMon")]
        public string? MaTrinhDoChuyenMon { get; set; }

        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChuyenNganh")]
        public string? ChuyenNganh { get; set; }


        [Display(Name = "Thời gian bổ nhiệm")]
        public string? SoQuyetDinhBoNhiem { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoTinHoc")]
        public string? MaTrinhDoTinHoc { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoNgoaiNgu")]
        public Guid? MaTrinhDoNgoaiNgu { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public string? MaTrinhDoChinhTri { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HocHam")]
        public string? MaHocHam { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HocVi")]
        public String? MaHocVi { get; set; }

        [MaxLength(50)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public string? MaDanToc { get; set; }

        [MaxLength(50)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string? MaTonGiao { get; set; }
        [MaxLength(1000)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiSinh")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? NoiSinh { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Nơi cư trú")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? ChoOHienNay { get; set; }
        public string? ChoOHienNay_XaPhuong { get; set; }
        public string? ChoOHienNay_QuanHuyen { get; set; }

       // [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        public string? NgayvaoDangDuBi { get; set; }

       // [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public string? NgayVaoDangChinhThuc { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoHoi")]
        public String? NgayVaoHoi { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaCapUyDang")]
        public String? NgayThamGiaCapUyDang { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaHDND")]
        public String? NgayThamGiaHDND { get; set; }

        [MaxLength(2000)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }

        [MaxLength(2000)]
        [Display(Name = "Kết quả đánh giá CBCC")]
        public string? DanhGiaCBCC { get; set; }


        [MaxLength(2000)]
        [Display(Name = "Kết quả đánh giá Đảng viên")]
        public string? DanhGiaDangVien { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BanChapHanh")]
        public bool IsBanChapHanh { get; set; } = false;

        [MaxLength(500)]
        public string? HinhAnh { get; set; }

    }
    public class CanBoVMMT : CanBoVM {
        public CanBo AddCanBo()
        {
            CanBo add = new CanBo();
            add.IDCanBo = Guid.NewGuid();
            add.MaCanBo = this.MaCanBo;
            add.MaDinhDanh = this.MaDinhDanh;
            add.HoVaTen = this.HoVaTen;
            add.NgaySinh = this.NgaySinh;
            add.GioiTinh = this.GioiTinh;
            add.SoCCCD = this.SoCCCD!;
            add.NgayCapCCCD = this.NgayCapCCCD;
            add.MaTinhTrang = this.MaTinhTrang;
            add.IdCoSo = this.IdCoSo;
            add.IdDepartment = this.IdDepartment;
            add.MaChucVu = this.MaChucVu!.Value;
            add.DonVi = this.DonVi;
            add.SoDienThoai = this.SoDienThoai;
            add.Email = this.Email;
            add.IsBanChapHanh = IsBanChapHanh  ;
            add.MaNgachLuong = this.MaNgachLuong;
            add.MaBacLuong = this.MaBacLuong;
            add.HeSoLuong = this.HeSoLuong;
            add.NgayNangBacLuong = this.NgayNangBacLuong;
            add.PhuCapChucVu = this.PhuCapChucVu;
            add.PhuCapVuotKhung = this.PhuCapVuotKhung;
            add.PhuCapKiemNhiem = this.PhuCapKiemNhiem;
            add.PhuCapKhuVuc = this.PhuCapKhuVuc;
            add.LuongKhoan = this.LuongKhoan;
            add.NgayVaoBienChe = this.NgayVaoBienChe;
            add.NgayThamGiaCongTac = this.NgayThamGiaCongTac;
            add.MaHeDaoTao = this.MaHeDaoTao;
            add.MaTrinhDoChuyenMon = this.MaTrinhDoChuyenMon;
            add.MaTrinhDoHocVan = this.MaTrinhDoHocVan;
            add.SoQuyetDinhBoNhiem = this.SoQuyetDinhBoNhiem;
            add.ChuyenNganh = this.ChuyenNganh;
            add.MaTrinhDoTinHoc = this.MaTrinhDoTinHoc;
            add.MaTrinhDoNgoaiNgu = this.MaTrinhDoNgoaiNgu;
            add.MaTrinhDoChinhTri = this.MaTrinhDoChinhTri;
            add.MaHocVi = this.MaHocVi;
            add.MaHocHam = this.MaHocHam;
            add.MaDanToc = this.MaDanToc;
            add.MaTonGiao = this.MaTonGiao;
            add.NoiSinh = this.NoiSinh;
            add.NgayvaoDangDuBi = this.NgayvaoDangDuBi;
            add.NgayVaoDangChinhThuc = this.NgayVaoDangChinhThuc;
            add.HinhAnh = this.HinhAnh;
            add.KhoanTuNgay = this.KhoanTuNgay;
            add.KhoanDenNgay = this.KhoanDenNgay;
            add.MaSoThue = this.MaSoThue;
            add.SoBHXH = this.SoBHXH;
            add.SoBHYT = this.SoBHYT;
            add.GhiChu = this.GhiChu;
            add.DanhGiaCBCC = this.DanhGiaCBCC;
            add.DanhGiaDangVien = this.DanhGiaDangVien;
            add.ChoOHienNay = this.ChoOHienNay;
            add.ChoOHienNay_XaPhuong = this.ChoOHienNay_XaPhuong;
            add.ChoOHienNay_QuanHuyen = this.ChoOHienNay_QuanHuyen;
            add.MaPhanHe = this.MaPhanHe;
            add.IsCanBo = true;

          
            return add;
        }
        public static CanBoVMMT EditCanBo(CanBo item)
        {
            CanBoVMMT edit = new CanBoVMMT();
            edit.IDCanBo = item.IDCanBo;
            edit.MaCanBo = item.MaCanBo;
            edit.MaDinhDanh = item.MaDinhDanh;
            edit.HoVaTen = item.HoVaTen;
            edit.NgaySinh = item.NgaySinh;
            edit.GioiTinh = item.GioiTinh;
            edit.SoCCCD = item.SoCCCD;
            edit.NgayCapCCCD = item.NgayCapCCCD;
            edit.MaTinhTrang = item.MaTinhTrang;
            //edit.IdCoSo = item.IdCoSo.Value;
            edit.IdDepartment = item.IdDepartment;
            edit.MaChucVu = item.MaChucVu;
            edit.DonVi = item.DonVi;
            edit.SoDienThoai = item.SoDienThoai;
            edit.Email = item.Email;
            edit.IsBanChapHanh = item.IsBanChapHanh == null?false: item.IsBanChapHanh.Value;
            edit.MaNgachLuong = item.MaNgachLuong;
            edit.MaBacLuong = item.MaBacLuong;
            edit.HeSoLuong = item.HeSoLuong;
            edit.NgayNangBacLuong = item.NgayNangBacLuong;
            edit.PhuCapChucVu = item.PhuCapChucVu;
            edit.PhuCapVuotKhung = item.PhuCapVuotKhung;
            edit.PhuCapKiemNhiem = item.PhuCapKiemNhiem;
            edit.PhuCapKhuVuc = item.PhuCapKhuVuc;
            edit.LuongKhoan = item.LuongKhoan;
            edit.NgayVaoBienChe = item.NgayVaoBienChe;
            edit.NgayThamGiaCongTac = item.NgayThamGiaCongTac;
            edit.MaHeDaoTao = item.MaHeDaoTao;
            edit.MaTrinhDoChuyenMon = item.MaTrinhDoChuyenMon;
            edit.MaTrinhDoHocVan = item.MaTrinhDoHocVan!;
            edit.SoQuyetDinhBoNhiem = item.SoQuyetDinhBoNhiem;
            edit.ChuyenNganh = item.ChuyenNganh;
            edit.MaTrinhDoTinHoc = item.MaTrinhDoTinHoc;
            edit.MaTrinhDoNgoaiNgu = item.MaTrinhDoNgoaiNgu;
            edit.MaTrinhDoChinhTri = item.MaTrinhDoChinhTri;
            edit.MaHocVi = item.MaHocVi;
            edit.MaHocHam = item.MaHocHam;
            edit.MaDanToc = item.MaDanToc;
            edit.MaTonGiao = item.MaTonGiao;
            edit.NoiSinh = item.NoiSinh;
            edit.NgayvaoDangDuBi = item.NgayvaoDangDuBi;
            edit.NgayVaoDangChinhThuc = item.NgayVaoDangChinhThuc;
            edit.HinhAnh = item.HinhAnh;
            edit.KhoanTuNgay = item.KhoanTuNgay;
            edit.KhoanDenNgay = item.KhoanDenNgay;
            edit.MaSoThue = item.MaSoThue;
            edit.SoBHXH = item.SoBHXH;
            edit.SoBHYT = item.SoBHYT;
            edit.GhiChu = item.GhiChu;
            edit.DanhGiaCBCC = item.DanhGiaCBCC;
            edit.DanhGiaDangVien = item.DanhGiaDangVien;
            edit.ChoOHienNay = item.ChoOHienNay;
            edit.ChoOHienNay_XaPhuong = item.ChoOHienNay_XaPhuong;
            edit.ChoOHienNay_QuanHuyen = item.ChoOHienNay_QuanHuyen;
            edit.MaPhanHe = item.MaPhanHe;
            edit.HinhAnh = item.HinhAnh;
            
            edit.IsBanChapHanh = item.IsBanChapHanh==null?false: item.IsBanChapHanh.Value;
            if (item.HinhAnh == null || item.HinhAnh == "")
            {
                edit.HinhAnh = @"\Images\login.png";
            }
            return edit;
        }

        public  CanBo EditUpdate(CanBo item) {

            item.MaCanBo = this.MaCanBo;
            item.MaDinhDanh = this.MaDinhDanh;
            item.HoVaTen = this.HoVaTen;
            item.NgaySinh = this.NgaySinh;
            item.GioiTinh = this.GioiTinh;
            item.SoCCCD = this.SoCCCD;
            item.NgayCapCCCD = this.NgayCapCCCD;
            item.MaTinhTrang = this.MaTinhTrang;
            //item.IdCoSo = this.IdCoSo;
            item.IdDepartment = this.IdDepartment;
            item.MaChucVu = this.MaChucVu.Value;
            item.DonVi = this.DonVi;
            item.SoQuyetDinhBoNhiem = this.SoQuyetDinhBoNhiem;
            item.SoDienThoai = this.SoDienThoai;
            item.Email = this.Email;
            item.MaNgachLuong = this.MaNgachLuong;
            item.MaBacLuong = this.MaBacLuong;
            item.HeSoLuong = this.HeSoLuong;
            item.NgayNangBacLuong = this.NgayNangBacLuong;
            item.PhuCapChucVu = this.PhuCapChucVu;
            item.PhuCapVuotKhung = this.PhuCapVuotKhung;
            item.PhuCapKiemNhiem = this.PhuCapKiemNhiem;
            item.PhuCapKhuVuc = this.PhuCapKhuVuc;
            item.LuongKhoan = this.LuongKhoan;
            item.NgayVaoBienChe = this.NgayVaoBienChe;
            item.NgayThamGiaCongTac = this.NgayThamGiaCongTac;
            item.IsBanChapHanh = this.IsBanChapHanh ;
            //item.MaHeDaoTao = this.MaHeDaoTao;
            item.MaTrinhDoChuyenMon = this.MaTrinhDoChuyenMon;
            item.MaTrinhDoHocVan = this.MaTrinhDoHocVan;
            item.ChuyenNganh = this.ChuyenNganh;
            item.MaTrinhDoTinHoc = this.MaTrinhDoTinHoc;
            item.MaTrinhDoNgoaiNgu = this.MaTrinhDoNgoaiNgu;
            item.MaTrinhDoChinhTri = this.MaTrinhDoChinhTri;
            item.MaHocVi = this.MaHocVi;
            item.MaHocHam = this.MaHocHam;
            item.MaDanToc = this.MaDanToc;
            item.MaTonGiao = this.MaTonGiao;
            item.NoiSinh = this.NoiSinh;
            item.NgayvaoDangDuBi = this.NgayvaoDangDuBi;
            item.NgayVaoDangChinhThuc = this.NgayVaoDangChinhThuc;
            //item.HinhAnh = this.HinhAnh;
            item.KhoanTuNgay = this.KhoanTuNgay;
            item.KhoanDenNgay = this.KhoanDenNgay;
            item.MaSoThue = this.MaSoThue;
            item.SoBHXH = this.SoBHXH;
            item.SoBHYT = this.SoBHYT;
            item.GhiChu = this.GhiChu;

            item.DanhGiaCBCC = this.DanhGiaCBCC;
            item.DanhGiaDangVien = this.DanhGiaDangVien;
            item.ChoOHienNay = this.ChoOHienNay;
            item.ChoOHienNay_XaPhuong = this.ChoOHienNay_XaPhuong;
            item.ChoOHienNay_QuanHuyen = this.ChoOHienNay_QuanHuyen;
            item.MaPhanHe = this.MaPhanHe;
            item.DonVi = this.DonVi;
            return item;
        }
    }
   
}
