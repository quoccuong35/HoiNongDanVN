using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HoiNongDan.Models
{
    public class HoiVienCapTheVM
    {
        public Guid? ID { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Dot")]
        public Guid MaDot { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Nam")]
        [Range(2020,2050, ErrorMessage = "Trong phạm vi từ 2020 - 2050")]
        public int Nam { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Quy")]
        [Range(1,4,ErrorMessage ="Trong phạm vi từ 1 - 4")]
        public int Quy { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LoaiCap")]
        public String LoaiCap { get; set; } = "02";

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayCap")]
        [DataType(DataType.Date)]
        public DateTime? NgayCap { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoThe")]
        public String? SoThe { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CapNhatNhanSu")]
        public bool CapNhatNhanSu { get; set; } = false;


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrangThai")]
        public String TrangThai { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }


        public List<HoiVienInfo> HoiViens { get; set; }
        public HoiVienInfo HoiVien { get; set; }
        public HoiVienCapTheVM() {
            HoiViens = new List<HoiVienInfo>();
        }
    }
    public class HoiVienCapTheSearchVM {

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Nam")]
        [Range(2020, 2050, ErrorMessage = "Trong phạm vi từ 2020 - 2050")]
        public int? Nam { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Quy")]
        [Range(1, 4, ErrorMessage = "Trong phạm vi từ 1 - 4")]
        public int? Quy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Dot")]
        public Guid? MaDot { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LoaiCap")]
       
        public String LoaiCap { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrangThai")]
        public String? TrangThai { get; set; }

    }
    public class HoiVienCapTheDetailVM
    {
        public Guid ID { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public String? TenDiaBan { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Nam")]
        public int? Nam { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        public String GioiTinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        public String? NgaySinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Dot")]
        public int? Quy { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Quy")]

        public String? TenDot { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LoaiCap")]
        public String LoaiCap { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayCap")]
        public DateTime? NgayCap { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public String? SoThe { get; set; }

      

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoDienThoai")]
        public string? SoDienThoai { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayCapCCCD")]
        public String? NgayCapCCCD { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrangThai")]
        public String? TrangThai { get; set; }

        [Display(Name="Địa chỉ")]
        public String? DiaChi { get; set; }

    }
}
