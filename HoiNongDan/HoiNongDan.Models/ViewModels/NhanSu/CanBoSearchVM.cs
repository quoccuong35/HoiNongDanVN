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
    public class CanBoSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string MaCanBo { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string HoVaTen { get; set; }
       
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhTrang")]
        public string MaTinhTrang { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhanHe")]
        public string MaPhanHe { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        public Guid? IdCoSo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public Guid? IdDepartment { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public Guid? MaChucVu { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NhiemKy")]
        public string NhiemKy { get; set; }


    }
}
