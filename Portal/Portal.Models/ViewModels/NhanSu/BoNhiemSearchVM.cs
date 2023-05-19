
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
    public class BoNhiemSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoQuyetDinh")]
        public string? SoQuyetDinh { get; set; }
    }
}
