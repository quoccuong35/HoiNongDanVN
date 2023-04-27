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
    public class BacLuongSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgachLuong")]
        public string MaNgachLuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenBacLuong")]
        public String TenBacLuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; } = true;
    }
}
