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
    public class MenuSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MenuName")]
        public string MenuName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MenuIdParent")]
        public Guid? MenuIdParent { get; set; }
    }
}
