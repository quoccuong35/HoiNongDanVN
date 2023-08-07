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
    public class TonGiaoVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Ma")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string MaTonGiao { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Ten")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string TenTonGiao { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Actived")]
        public bool Actived { get; set; } = true;

        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }

        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public String? Description { get; set; }
    }

    public class TonGiaoMTVM : TonGiaoVM
    {
        public TonGiao GetTonGiao(TonGiao obj)
        {
            obj.MaTonGiao = this.MaTonGiao;
            obj.TenTonGiao = this.TenTonGiao;
            obj.Actived = this.Actived;
            obj.OrderIndex = this.OrderIndex;
            obj.Description = this.Description;
            return obj;
        }
    }
    public class TonGiaoSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Ten")]

        public String? TenTonGiao { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

    }
}
