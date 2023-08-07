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
    public class DaoTaoDetailVM
    {
        public Guid? IDQuaTrinhDaoTao { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSoDaoTao")]
        public String? CoSoDaoTao { get; set; }

        
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayTotNghiep")]
        public DateTime? NgayTotNghiep { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuocGia")]
        public String? QuocGia { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaLoaiBangCap")]
        public String? TenLoaiBangCap { get; set; }
      
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHinhThucDaoTao")]
        public String? TenHinhThucDaoTao { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaChuyenNganh")]
        public String TenChuyenNganh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LuanAnTN")]
        public bool? LuanAnTN { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FileDinhKem")]
        public String? FileDinhKem { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }
    }
}
