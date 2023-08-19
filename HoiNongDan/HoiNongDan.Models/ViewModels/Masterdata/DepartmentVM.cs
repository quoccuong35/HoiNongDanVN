
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

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public string? Description { get; set; }

        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public int? OrderIndex { get; set; }


        //public Department CreateDepartment(Guid accountId)
        //{
        //    return new Department
        //    {
        //        Id = Guid.NewGuid(),
        //        Code = this.Code,
        //        Name = this.Name,
        //        OrderIndex = this.OrderIndex,
        //        Description = this.Description == null ? "" : this.Description,
        //        CreatedAccountId = accountId,
        //        CreatedTime = DateTime.Now,
        //    };
        //}
        //public Department EditDepartment(Department edit, Guid accountId)
        //{
        //    edit.Name = this.Name;
        //    edit.Code = this.Code;
        //    edit.Actived = this.Actived == null ? false : this.Actived.Value;
        //    edit.Description = this.Description == null ? "" : this.Description;
        //    edit.OrderIndex = this.OrderIndex;
        //    edit.LastModifiedTime = DateTime.Now;
        //    edit.LastModifiedAccountId = accountId;
        //    return edit;
        //}

    }
}
