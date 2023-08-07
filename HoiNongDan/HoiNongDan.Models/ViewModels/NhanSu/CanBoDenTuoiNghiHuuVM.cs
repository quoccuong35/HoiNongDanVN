using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HoiNongDan.Models 
{
    [NotMapped]
    public class CanBoDenTuoiNghiHuuVM
    {
        public Guid IDCanBo { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        public DateTime NgaySinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        public GioiTinh GioiTinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgachLuong")]
        public string TenNgachLuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BacLuong")]
        public string? TenBacLuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhTrang")]
        public string TenTinhTrang { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhanHe")]
        public string TenPhanHe { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSo")]
        public decimal? HeSoLuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        public string TenCoSo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DepartmentName")]
        public string TenDonVi { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public string TenChucVu { get; set; }

        [Display(Name ="Tuổi")]
        public int Tuoi { get; set; }

        [Display(Name = "Tháng")]
        public int Thang { get; set; }
    }
}
