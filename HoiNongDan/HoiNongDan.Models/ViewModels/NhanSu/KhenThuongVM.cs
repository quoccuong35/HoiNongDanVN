﻿
using HoiNongDan.Models.Entitys.NhanSu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class KhenThuongVM
    {
        public Guid? IDQuaTrinhKhenThuong { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HinhThucKhenThuong")]
        public string MaHinhThucKhenThuong { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanhHieuKhenThuong")]
        public string MaDanhHieuKhenThuong { get; set; }

   
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayQuyetDinh")]
        [DataType(DataType.Date)]
        public DateTime? NgayQuyetDinh { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoQuyetDinh")]
        public String? SoQuyetDinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NguoiKy")]
        public String? NguoiKy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDung")]
        public String? NoiDung { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CapKhenThuong")]
        public String? MaCapKhenThuong { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Nam")]
        public int? Nam {  get; set; }
        public NhanSuThongTinVM NhanSu { get; set; }

        public HoiVienInfo HoiVien { get; set; }
        public KhenThuongVM()
        {
            NhanSu = new NhanSuThongTinVM();
            HoiVien = new HoiVienInfo();
        }
    }
    public class KhenThuongVMMT:KhenThuongVM {
        public QuaTrinhKhenThuong GetKhenThuong(QuaTrinhKhenThuong obj) {
            obj.MaHinhThucKhenThuong = this.MaHinhThucKhenThuong;
            obj.MaDanhHieuKhenThuong = this.MaDanhHieuKhenThuong;
            obj.SoQuyetDinh = this.SoQuyetDinh;
            obj.NgayQuyetDinh = this.NgayQuyetDinh;
            obj.GhiChu = this.GhiChu!;
            obj.Nam = this.Nam!;
            obj.NoiDung = this.NoiDung!;
            obj.MaCapKhenThuong = this.MaCapKhenThuong!;
            obj.NguoiKy = this.NguoiKy;
            obj.IDCanBo = this.HoiVien.IdCanbo!= null ? this.HoiVien.IdCanbo.Value :  this.NhanSu.IdCanbo!.Value;
            return obj;
        }
    }

    public class KhenThuongExcelVM 
    {
       public int STT { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public String HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public String SoCCCD { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanhHieuKhenThuong")]
        public string MaDanhHieuKhenThuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CapKhenThuong")]
        public String? MaCapKhenThuong { get; set; }

        [Display(Name ="Năm")]
        public int? Nam { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public string? QuanHuyen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuongXa")]
        public string? PhuongXa { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        public string? ChoOHienNay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }

    }
}
