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
    public class CanBoDetailVM
    {
        public Guid IdCanbo { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string MaCanBo { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhTrang")]
        public string TenTinhTrang { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhanHe")]
        public string TenPhanHe { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        public string TenCoSo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DepartmentName")]
        public string TenDonVi { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public string TenChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenNgachLuong")]
        public string TenNgachLuong { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenBacLuong")]
        public string? TenBacLuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSo")]
        public decimal? HeSo { get; set; }

    }
}
