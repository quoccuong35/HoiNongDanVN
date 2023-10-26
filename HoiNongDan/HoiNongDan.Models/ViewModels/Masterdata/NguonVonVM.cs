using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class NguonVonVM
    {
        public Guid? MaNguonVon { get; set; }
        [Display(Name ="Tên nguồn vốn")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public String TenNguonVon { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }
        [Display(Name = "Hiện/Ẩn")]
        public bool Actived { get; set; }
    }

}
