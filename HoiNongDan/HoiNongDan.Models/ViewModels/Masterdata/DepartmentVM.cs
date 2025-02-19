
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace HoiNongDan.Models
{
    public class DepartmentVM
    {
        public Guid? Id { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DepartmentCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string Code { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DepartmentName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        public Guid? IdCoSo { get; set; }

        [Display(Name="Đơn vị chủ quản")]
        public Guid? IdParent { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public string? Description { get; set; }

        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public int? OrderIndex { get; set; }



    }
}
