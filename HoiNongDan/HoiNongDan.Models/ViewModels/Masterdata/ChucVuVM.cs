using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models 
{ 
    public class ChucVuVM
    {
        public Guid? MaChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenChucVu")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string TenChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSo")]
        public Nullable<System.Decimal> HeSoChucVu { get; set; }

        public Nullable<System.Decimal> PhuCapDienThoai { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool Actived { get; set; }

        [Display(Name ="Hội viên")]
        public bool? HoiVien { get; set; }

        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        public String? Description { get; set; }

        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public int? OrderIndex { get; set; }
    }
    public class ChucVuSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenChucVu")]
        public String? TenChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; } = false;

    }
    public class ChucVuMTVM : ChucVuVM
    {
        public ChucVu GetChucVu(ChucVu obj) {
            obj.TenChucVu = this.TenChucVu;
            obj.Description = this.Description;
            obj.HeSoChucVu = this.HeSoChucVu;
            obj.PhuCapDienThoai = this.PhuCapDienThoai;
            obj.Actived = this.Actived;
            obj.HoiVien = this.HoiVien;
            obj.OrderIndex = this.OrderIndex;

            return obj;
        }
    }
}
