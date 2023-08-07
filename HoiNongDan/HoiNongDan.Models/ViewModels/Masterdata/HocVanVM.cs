using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.Entitys.MasterData;
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
    public class HocVanVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Ma")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string MaTrinhDoHocVan { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Ten")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string TenTrinhDoHocVan { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Actived")]
        public bool Actived { get; set; } = true;

        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }

        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public String? Description { get; set; }
    }

    public class HocVanMTVM : HocVanVM
    {
        public TrinhDoHocVan GetHocVan(TrinhDoHocVan obj)
        {
            obj.MaTrinhDoHocVan = this.MaTrinhDoHocVan;
            obj.TenTrinhDoHocVan = this.TenTrinhDoHocVan;
            obj.Actived = this.Actived;
            obj.OrderIndex = this.OrderIndex;
            return obj;
        }
    }
    public class HocVanSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Ten")]

        public String? TenTrinhDoHocVan { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

    }
}
