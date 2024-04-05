using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class LichSinhHoatChiToHoiVM {
        public Guid? ID { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid MaDiaBanHoiVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChiToHoiNganhNghe")]
        public Guid? IDMaChiToHoi { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenLichSinhHoat")]
        public string TenNoiDungSinhHoat { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinhHoat")]
        public DateTime? Ngay { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDung")]
        public string NoiDungSinhHoat { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoLuongNguoiThanGia")]
        public int SoLuongNguoiThanGia { get; set; }
        public bool Actived { get; set; } = true;
        public List<FileDinhKem>? FileDinhKems { get; set; }
        public LichSinhHoatChiToHoiVM()
        {
            FileDinhKems = new List<FileDinhKem>();
        }
    }
    public class LichSinhHoatChiToHoiDetailVM
    {
        public Guid ID { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public String TenDiaBanHoi { get; set; }


        public Guid? IDMaChiToHoi { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenLichSinhHoat")]
        public string TenNoiDungSinhHoat { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinhHoat")]
        public DateTime? Ngay { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDung")]
        public string NoiDungSinhHoat { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoLuongNguoiThanGia")]
        public int SoLuongNguoiThanGia { get; set; }
       
    }
    public class LichSinhHoatChiToHoi_NguoiThamGiaVN {
        public Guid? ID { get; set; }
        public String? MaHoiVien { get; set; }
        public String? TenHoiVien { get; set; }
        public String? ChucVu { get; set; }
    }
    public class LichSinhHoatChiToHoiSearchVM
    {

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenLichSinhHoat")]
        public String? TenNoiDung { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TuNgay")]
        public DateTime? TuNgay { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DenNgay")]
        public DateTime? DenNgay { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoi { get; set; }
        public bool? Actived { get; set; }
    }
}
