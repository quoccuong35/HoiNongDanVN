using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HoiVienChinhTriHoiDoanExcelVM
    {

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]

        public string? HoVaTen { get; set; }

        [Display(Name = "Tên hội")]
        public String TenHoiNongDan { get; set; }

        [Display(Name = "Tên đoàn thể chính trị-Hội đoàn khác")]
        public String TenDoanTheChinhChi_HoiDon { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        public String GioiTinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoKhauThuongTru")]
        public string? HoKhauThuongTru { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]

        public string ChoOHienNay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoDienThoai")]
        public string? SoDienThoai { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoHoi")]
        public String? NgayVaoHoi { get; set; }

    }
}
