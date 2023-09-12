using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class DaoTaoBoiDuongSearchVM
    {

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }



        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaLoaiBangCap")]
        public String? MaLoaiBangCap { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHinhThucDaoTao")]
        public String? MaHinhThucDaoTao { get; set; }
    }
}
