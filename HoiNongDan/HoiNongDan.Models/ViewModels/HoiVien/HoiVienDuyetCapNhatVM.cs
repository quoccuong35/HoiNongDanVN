using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HoiVienDuyetCapNhatVM : HoiVienDetailVM
    {
        public Guid ID { get; set; }
        [Display(Name ="Số quyết định")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public String? SoQuyetDinh { get; set; }

        [Display(Name = "Ngày vào hội")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime? NgayVaoHoi { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Số thẻ")]
        public String SoTheHoiVien { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Ngày cấp thẻ")]
        public DateTime? NgayCapThe { get; set; }
    }
}
