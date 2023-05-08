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
    public class NhanSuThongTinVM
    {
        public Guid? IdCanbo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        public string? TenDonVi { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public string? TenCoSo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhTrang")]
        public string? TenTinhTrang { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhanHe")]
        public string? TenPhanHe { get; set; }

        public string HinhAnh { get; set; } = @"\images\login.png";
        public string? Error { get; set; }
        public bool CanBo { get; set; } = true;
        public bool Edit { get; set; } = true;
    }
}
