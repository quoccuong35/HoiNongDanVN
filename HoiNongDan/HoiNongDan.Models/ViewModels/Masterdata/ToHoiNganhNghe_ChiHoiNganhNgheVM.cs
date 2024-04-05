using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models 
{ 
    public class ToHoiNganhNghe_ChiHoiNganhNgheVM
    {
        public Guid? Ma_ToHoiNganhNghe_ChiHoiNganhNghe { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Ten")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public String Ten { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThanhLap")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        public DateTime NgayThanhLap { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Actived")]
        public bool Actived { get; set; } = true;


        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public String? Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Loai")]
        public String? Loai { get; set; }

        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }

        public FileDinhKem? FileDinhKem {  get; set; }

        [Display(Name ="Số hội viên")]
        public int? SoHoiVien { get; set; }

        public String? Url { get; set; }
    }
    public class ToHoiNganhNghe_ChiHoiNganhNgheSearchVM
    {

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Ten")]
        public String? Ten { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Loai")]
        public String? Loai { get; set; }
        //[Display(Name ="Thành lập từ ngày")]
        //[DataType(DataType.Date)]
        //public DateTime? TuNgay { get; set; }
        //[Display(Name = "Thành lập đến ngày")]
        //[DataType(DataType.Date)]
        //public DateTime? DenNgay { get; set; }
        public bool Actived { get; set; } = true;

    }
}
