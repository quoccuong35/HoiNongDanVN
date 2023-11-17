using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HoiNongDan.Models
{
    public class HoiVien_CLB_DN_MH_HTX_THTSearchVM
    {
        [Display(Name = "Địa bàn hội viên")]
        public Guid? MaDiaBan { get; set; }

        [Display(Name = "Tên câu lạc bộ, đội nhóm, mô hình, hợp tác xã, tổ hợp tác")]
        public Guid? Id_CLB_DN_MH_HTX_THT { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public String? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public String? MaHoiVien { get; set; }
    }
    public class HoiVien_CLB_DN_MH_HTX_THTDetailVM: HoiVienDetailVM
    {
        public Guid? MaCaulacBo { get; set; }
        [Display(Name = "Tên câu lạc bộ, đội nhóm, mô hình, hợp tác xã, tổ hợp tác")]
        public String? TenCauLacBo { get; set; }
    }
    public class HoiVien_CLB_DN_MH_HTX_THTExcelVM
    {

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]

        public string? HoVaTen { get; set; }

        [Display(Name = "Tên hội")]
        public String TenHoiNongDan { get; set; }

        [Display(Name = "Tên câu lạc bộ, đội nhóm, mô hình, hợp tác xã, tổ hợp tác")]
        public String TenCauLacBo { get; set; }


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
