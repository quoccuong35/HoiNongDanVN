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
    public class KyLuatDetailVM
    {
        public Guid? IdQuaTrinhKyLuat { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HinhThucKyLuat")]
        public String TenHinhThucKyLuat { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayQuyetDinh")]
        public String SoQuyetDinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NguoiKy")]
        public String? NguoiKy { get; set; }

        [DataType(DataType.Date)]

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayKy")]
        public DateTime NgayKy { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LyDo")]
        public String LyDo { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }
    }
}
