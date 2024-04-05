using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class PhatTrienDangVM
    {
        public Guid? ID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid MaDiaBanHoiND { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenVietTat")]
        public String TenVietTat { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Nam")]
        [Range(2000,2050,ErrorMessage ="Năm phải nhập từ 2000 đến 2050")]
        public int Nam { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1,1000,ErrorMessage ="Số lượng phải từ 1 trở lên")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoLuong")]
        
        public int SoLuong { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDung")]
        public String NoiDung { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool Actived { get; set; } = true;
        public List<HoiVienInfo> HoiViens { get; set; }
        public List<FileDinhKem>? FileDinhKems { get; set; }
        public PhatTrienDangVM() {
            HoiViens = new List<HoiVienInfo>();
            FileDinhKems = new List<FileDinhKem>();
        }
    }
    public class PhatTrienDangSearchVM{
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenVietTat")]
        public String? TenVietTat { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Nam")]
        public int? Nam { get; set; }

        public bool? Actived { get; set; }
    }
    public class PhatTrienDangDetailVM {
        public Guid? ID { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHV")]
        public String MaHV { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenHV")]
        public String TenHV { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public String DiaBanHoiND { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenVietTat")]
        public String TenVietTat { get; set; }



        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Nam")]
        public int Nam { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoLuong")]
        public int SoLuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDung")]
        public String NoiDung { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }

    }
}
