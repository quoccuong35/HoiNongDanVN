using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HoiNongDan.Models
{
    public class HoiVien_ToHoiNN_ChiHoiNNSearchVM
    {
        [Display(Name = "Địa bàn hội viên")]
        public Guid? MaDiaBan { get; set; }

        [Display(Name = "Tổ hội ngành nghề, chi hội ngành nghề")]
        public Guid? Ma_ToHoiNganhNghe_ChiHoiNganhNghe { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public String? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public String? MaHoiVien { get; set; }
    }
    public class HoiVien_ToHoiNN_ChiHoiNNDetailVM: HoiVienDetailVM
    {
        public Guid? MaToHoi { get; set; }
        [Display(Name = "Tổ hội ngành nghề, chi hội ngành nghề")]
        public String? TenToHoi { get; set; }
    }
    public class HoiVien_ToHoiNN_ChiHoiNNExcelVM
    {

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]

        public string? HoVaTen { get; set; }

        [Display(Name = "Tên hội")]
        public String TenHoiNongDan { get; set; }

        [Display(Name = "Tổ hội ngành nghề, chi hội ngành nghề")]
        public String TenToHoi { get; set; }


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
