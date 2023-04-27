using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Portal.Models
{
    public class CoSoVM
    {
        public Guid? IdCoSo { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenCoSo")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string TenCoSo { get; set; }//

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool Actived { get; set; } = true;

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public string? Description { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }

        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public int? OrderIndex { get; set; }
    }
}
