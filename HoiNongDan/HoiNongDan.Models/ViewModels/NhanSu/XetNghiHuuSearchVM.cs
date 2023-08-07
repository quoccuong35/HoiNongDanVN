using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HoiNongDan.Models
{
    public class XetNghiHuuSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        public Guid? IdCoSo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public Guid? IdDepartment { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(Name ="Tuổi")]
        public int Nam_Tuoi { get; set; } = 60;

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Tháng")]
        public int Nam_Thang { get; set; } = 9;

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Tuổi")]
        public int Nu_Tuoi { get; set; } = 56;

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Tháng")]
        public int Nu_Thang { get; set; } = 0;

        [DataType(DataType.Date)]
        public DateTime TuNgay { get; set; } = DateTime.Now;
    }
}
