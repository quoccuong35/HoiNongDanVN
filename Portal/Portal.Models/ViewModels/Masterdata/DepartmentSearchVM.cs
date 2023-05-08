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
    public class DepartmentSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        public Guid? IdCoso { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DepartmentName")]
        public string? Name { get; set; }
     
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
    }
}
