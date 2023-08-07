using HoiNongDan.Models.Entitys;
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
    public class ChinhTriVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Ma")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]

        public string MaTrinhDoChinhTri { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Ten")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string TenTrinhDoChinhTri { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool Actived { get; set; }

        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }

        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public String? Description { get; set; }
    }
    public class ChinhTriMTVM : ChinhTriVM {
        public TrinhDoChinhTri GetChinhTri(TrinhDoChinhTri obj) {
            obj.MaTrinhDoChinhTri = this.MaTrinhDoChinhTri;
            obj.TenTrinhDoChinhTri = this.TenTrinhDoChinhTri;
            obj.Actived = this.Actived;
            obj.OrderIndex = this.OrderIndex;
            obj.Description = this.Description;
            return obj;
        }
    }
    public class ChinhTriSearchVM {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Ten")]

        public String? TenTrinhDoChinhTri { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

    }
}
