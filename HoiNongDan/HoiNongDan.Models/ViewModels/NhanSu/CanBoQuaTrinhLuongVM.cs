using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class CanBoQuaTrinhLuongVM
    {
        public Guid ID { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string MaCanBo { get; set; }
        [Display(Name = "Họ Và Tên")]
        public string? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgachLuong")]
        public String? MaNgachLuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BacLuong")]
        public Guid? BacLuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSo")]
        public double? HeSoLuong { get; set; }

        [Display(Name = "Hệ số chức vụ")]

        public double? HeSoChucVu { get; set; }

        [Display(Name = "Vượt khung")]

        public double? VuotKhung { get; set; }

        [Display(Name = "Kiêm nhiệm")]

        public double? KiemNhiem { get; set; }

        [Display(Name = "Ngày hưởng")]
        public DateTime? NgayHuong { get; set; }
    }
    public class QuaTrinhLuongVM
    {
        public Guid? Id { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string MaCanBo { get; set; }
        [Display(Name = "Họ Và Tên")]
        public string? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgachLuong")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public String? MaNgachLuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BacLuong")]

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid? MaBacLuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSo")]
        public double? HeSoLuong { get; set; }

        [Display(Name = "Hệ số chức vụ")]

        public double? HeSoChucVu { get; set; }

        [Display(Name = "Vượt khung")]

        public double? VuotKhung { get; set; }

        [Display(Name = "Kiêm nhiệm")]

        public double? KiemNhiem { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Ngày hưởng")]
        [DataType(DataType.Date)]
        public DateTime? NgayHuong { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Ngày nâng bậc")]
        [DataType(DataType.Date)]
        public DateTime? NgayNangBacLuong { get; set; }



        public FileDinhKem? FileDinhKem { get; set; }
        public NhanSuThongTinVM NhanSu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CapNhatTinhTrangCanBo")]
        public bool CapNhatTinhTrangCanBo { get; set; } = true;

    }
    public class CanBoQuaTrinhLuongExcel : CanBoQuaTrinhLuongVM
    {
        public int RowIndex { get; set; }
        public bool isNullValueId { get; set; }
        public string Error { get; set; }
        public Guid IDCanBo { get; set; }


        public CanBoQuaTrinhLuong AddQuaTrinhLuong()
        {
            return new CanBoQuaTrinhLuong
            {
                //ID = ID,
                //IDCanBo = IDCanBo,
                //MaNgachLuong = MaNgachLuong,
                //BacLuong = BacLuong,
                //HeSoLuong = HeSoLuong,
                //HeSoChucVu = HeSoChucVu,
                //VuotKhung = VuotKhung,
                //KiemNhiem = KiemNhiem,
                //NgayHuong = NgayHuong,
            };
        }
    }
    public class CanBoQuaTrinhLuongDetailVM
    {
        public Guid ID { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string MaCanBo { get; set; }
        [Display(Name = "Họ Và Tên")]
        public string? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgachLuong")]
        public String? MaNgachLuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BacLuong")]
        public String? BacLuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSo")]
        public double? HeSoLuong { get; set; }

        [Display(Name = "Hệ số chức vụ")]

        public double? HeSoChucVu { get; set; }

        [Display(Name = "Vượt khung")]

        public double? VuotKhung { get; set; }

        [Display(Name = "Kiêm nhiệm")]

        public double? KiemNhiem { get; set; }

        [Display(Name = "Ngày hưởng")]
        public DateTime? NgayHuong { get; set; }

        [Display(Name = "Ngày nâng bậc")]
        public DateTime? NgayNangBac { get; set; }
    }
}
