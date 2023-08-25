using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HoiVienHoiDapDetail
    {
        public Guid ID { get; set; }
        public string HoVaTen { get; set; }
        public string NoiDung { get; set; }
        public DateTime Ngay { get; set; }
        public bool? TraLoi { get; set;}
        public Guid? IdParent { get; set;}
        public string TrangThai { get; set; }
    }

    //public class HoiVienSearchThongTin {
    //    [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
    //    [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
    //    public Guid MaDiaBanHoatDong { get; set; }

    //    [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
    //    [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
    //    public string MaHoiVien { get; set;}
    //}
}
