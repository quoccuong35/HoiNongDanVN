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
    public class NgachLuongSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenNgachLuong")]
        public string TenNgachLuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaLoai")]
        public String MaLoai { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; } = true;
    }
}
