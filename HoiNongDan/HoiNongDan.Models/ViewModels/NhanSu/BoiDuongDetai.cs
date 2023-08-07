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
    public class BoiDuongDetai
    {
        public Guid IDQuaTrinhBoiDuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayBatDau")]
        [DataType(DataType.Date)]
        public DateTime NgayBatDau { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayKetThuc")]
        [DataType(DataType.Date)]
        public DateTime NgayKetThuc { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiBoiDuong")]
        public String NoiBoiDuong { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDung")]
        public String NoiDung { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHinhThucDaoTao")]
        public string TenHinhThucDaoTao { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }
    }
}
