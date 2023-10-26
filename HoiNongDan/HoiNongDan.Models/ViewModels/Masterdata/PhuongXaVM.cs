using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class PhuongXaVM
    {
        public string? MaPhuongXa { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenPhuongXa")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string TenPhuongXa { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhThanhPho")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        public string MaTinhThanhPho { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        public string MaQuanHuyen { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool Actived { get; set; } = true;

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        [MaxLength(500)]
        public String? Description { get; set; }

        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }
    }
    public class PhuongXaSearchVM {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenPhuongXa")]
        public string? TenPhuongXa { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhThanhPho")]
        public string? MaTinhThanhPho { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public string? MaQuanHuyen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; } = true;
    }
}
