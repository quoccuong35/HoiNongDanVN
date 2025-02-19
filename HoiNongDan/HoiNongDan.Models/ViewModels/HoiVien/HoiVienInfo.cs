using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HoiVienInfo
    {
        public bool Chon { get; set; } = false;
        public Guid? IdCanbo { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]

        public String? NgaySinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoKhauThuongTru")]
        public string? HoKhauThuongTru { get; set; }

        [Display(Name = "Quận\\Thành phố")]
        public String? QuanHuyen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")] 
        public String? DiaBan { get; set; }
        public bool Edit { get; set; } = true;
        public string? Error { get; set; }
    }
}
