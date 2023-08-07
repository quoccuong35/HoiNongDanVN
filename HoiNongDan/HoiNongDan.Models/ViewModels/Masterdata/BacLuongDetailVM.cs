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
    public class BacLuongDetailVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaBacLuong")]
        public Guid MaBacLuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaNgachLuong")]
        public string MaNgachLuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenBacLuong")]
        public String TenBacLuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSo")]
        public decimal HeSo { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public String? Description { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenNgachLuong")]
        public string TenNgachLuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Bac")]
        public int? OrderIndex { get; set; }
    }
}
