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
        public String? BacLuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSo")]
        public String? HeoSoLuong { get; set; }

        [Display(Name = "Hệ số chức vụ")]

        public String? HeSoChucVu { get; set; }

        [Display(Name = "Vượt khung")]

        public String? VuotKhung { get; set; }

        [Display(Name = "Kiêm nhiệm")]

        public String? KiemNhiem { get; set; }

        [Display(Name = "Ngày hưởng")]
        public String? NgayHuong { get; set; }
    }
    public class CanBoQuaTrinhLuongExcel : CanBoQuaTrinhLuongVM{
        public int RowIndex { get; set; }
        public bool isNullValueId { get; set; }
        public string Error { get; set; }
        public Guid IDCanBo { get; set; }


        public CanBoQuaTrinhLuong AddQuaTrinhLuong() {
            return new  CanBoQuaTrinhLuong{ 
                ID = ID,
                IDCanBo =IDCanBo,
                MaNgachLuong = MaNgachLuong,
                BacLuong = BacLuong,
                HeoSoLuong = HeoSoLuong,
                HeSoChucVu =HeSoChucVu,
                VuotKhung = VuotKhung,
                KiemNhiem = KiemNhiem,
                NgayHuong = NgayHuong,
            };
        }
    }
}
