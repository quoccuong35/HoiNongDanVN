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
    public class QHGiaDinhSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LoaiQuanhe")]
        public Guid? IDLoaiQuanHeGiaDinh { get; set; }
    }
    public class QHGiaDinhDetail {
        public Guid IDQuanheGiaDinh { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]

        public string HoTen { get; set; }

        [MaxLength(10)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        public String NgaySinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgheNghiep")]

        public string? NgheNghiep { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiLamVien")]
        public string? NoiLamVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaChi")]
        public string? DiaChi { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LoaiQuanhe")]
        public String TenLoaiQuanHe { get; set; }

    }
}
