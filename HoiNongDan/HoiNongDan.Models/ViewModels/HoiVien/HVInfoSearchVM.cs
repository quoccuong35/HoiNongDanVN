using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HVInfoSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public string? MaQuanHuyen { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }

        public List<HoiVienInfo> HoiViens { get; set; }
        public HVInfoSearchVM(){
            HoiViens = new List<HoiVienInfo>();
        }
    }
}
