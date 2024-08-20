using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HVDangKyVM
    {
        public Guid? ID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }


        [Display(Name ="Ngày đăng ký")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime? NgayDangKy { get; set; }
        //[MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string HoVaTen { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        public String? NgaySinh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        public GioiTinh GioiTinh { get; set; }


        [MaxLength(200)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayCapCCCD")]
        public String? NgayCapCCCD { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoKhauThuongTru")]
        public string? HoKhauThuongTru { get; set; }

        [MaxLength(1000)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ChoOHienNay { get; set; }


        [MaxLength(100)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoDienThoai")]
        public string? SoDienThoai { get; set; }

        [MaxLength(50)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public string MaDanToc { get; set; }

        [MaxLength(50)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string? MaTonGiao { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoHocVan")]
        public string? MaTrinhDoHocVan { get; set; }


        [MaxLength(20)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChuyenMon")]
        public string? MaTrinhDoChuyenMon { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChuyenNganh")]
        public string? ChuyenNganh { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public string? MaTrinhDoChinhTri { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgheNghiep")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        public string? MaNgheNghiep { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoiVienDanCu")]
        public bool HoiVienDanCu { get; set; } = false;

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoiVienNganhNghe")]
        public bool HoiVienNganhNghe { get; set; } = false;


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DangVien")]
        public bool DangVien { get; set; } = false;



    }
}
