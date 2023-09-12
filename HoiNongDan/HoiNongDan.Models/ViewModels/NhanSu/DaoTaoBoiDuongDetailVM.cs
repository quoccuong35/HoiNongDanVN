using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class DaoTaoBoiDuongDetailVM
    {
        public Guid? ID { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaLoaiBangCap")]
        public String? TenLoaiBangCap { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHinhThucDaoTao")]
        public String? TenHinhThucDaoTao { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDung")]
        public String? NoiDungDaoTao { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TuNgay")]
        [DataType(DataType.DateTime)]
        public DateTime? TuNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DenNgay")]
        [DataType(DataType.DateTime)]
        public DateTime? DenNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }
    }
}
