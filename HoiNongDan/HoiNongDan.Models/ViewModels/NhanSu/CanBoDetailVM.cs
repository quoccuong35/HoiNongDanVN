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
    public class CanBoDetailVM :CanBoVM
    {
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

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public string TenDanToc { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string TenTonGiao { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoNgoaiNgu")]
        public string TenTrinhDoNgoaiNgu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgachLuong")]
        public string TenNgachLuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BacLuong")]
        public string? TenBacLuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSo")]
        public decimal? HeSo { get; set; }

        public string HinhAnh { get; set; } = @"\images\login.png";

        [Display(Name = "Trình độ học vấn")]
        public string TrinhDoHocvan { get; set; }
    }
}
