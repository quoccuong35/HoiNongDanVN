using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models { 
    public class VayVonVM
    {
        public Guid? ID { get; set; }

        public HoiVienInfo HoiVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NguonVon")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        public Guid? MaNguonVon { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "HinhThucHoTro")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        //public Guid MaHinhThucHoTro { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoTienVay")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? SoTienVay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ThoiHangChoVay")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, 120, ErrorMessage = "Số tháng không hợp lệ")]
        public int? ThoiHangChoVay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LaiSuatVay")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(5, 50, ErrorMessage = "Lãi suất vay không hợp lệ")]
        public double? LaiSuatVay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TuNgay")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        public DateTime? TuNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DenNgay")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        public DateTime? DenNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayTraNoCuoiCung")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        public DateTime? NgayTraNoCuoiCung { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDung")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string NoiDung { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TraXong")]
        public bool TraXong { get; set; } = false;
    }
    public class VayVonSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NguonVon")]
        public Guid? MaNguonVon { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NamVayVon")]
        public int? NamVayVon { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHV")]
        public String MaHV { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenHV")]
        public String TenHV { get; set; }
        public bool? Actived { get; set; }
    }
    public class VayVonDetailVM
    {
        public Guid ID { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHV")]
        public String MaHV { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenHV")]
        public String TenHV { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoTienVay")]
        public long? SoTienVay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ThoiHangChoVay")]
        public int? ThoiHangChoVay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LaiSuatVay")]
        public double? LaiSuatVay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TuNgay")]
        public DateTime? TuNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DenNgay")]
        public DateTime? DenNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayTraNoCuoiCung")]
        public DateTime? NgayTraNoCuoiCung { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDungVay")]
        public string NoiDung { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoThangQuaHan")]
        public int SoThangQuaHan { get; set; }
        public bool TraXong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NguonVon")]
        public String? NguonVon { get; set; }

        public String? TienVay { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuongXa")]
        public string? PhuongXa { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public string? QuanHuyen { get; set; }
    }
}
