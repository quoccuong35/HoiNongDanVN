using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class ChiHoiVM
    {
        public Guid? MaChiHoi { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Ten")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string TenChiHoi { get; set; }

        [Display(Name ="Loại")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string Loai { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool Actived { get; set; } = true;

        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public String? Description { get; set; }

        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }

        [Display(Name = "Số hội viên")]
        public int? SoHoiVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThanhLap")]
        public Nullable<System.DateTime> NgayThanhLap { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoQuyetDinh")]
        public String? SoQuyetDinh { get; set; }

        [Display(Name = "Ngày giảm")]
        public Nullable<System.DateTime> NgayGiam { get; set; }
        [Display(Name = "Lý do giảm")]
        public String? LyDoGiam { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoatDong { get; set; }

        [Display(Name = "Tên hội")]
        public String? TenHoi { get; set; }
    }
    public class ChiHoiSearchVM {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Ten")]
        public string? TenChiHoi { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool Actived { get; set; } = true;


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        public String? Loai { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }
    }
}
