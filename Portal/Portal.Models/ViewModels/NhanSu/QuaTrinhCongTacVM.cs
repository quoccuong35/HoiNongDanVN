using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class QuaTrinhCongTacVM
    {
        public Guid? IDQuaTrinhCongTac { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TuNgay")]
        public DateTime? TuNgay { get; set; }
        
        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DenNgay")]
        public DateTime? DenNgay { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public Guid MaChucVu { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiLamViec")]
        public String NoiLamViec { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }
        public NhanSuThongTinVM NhanSu { get; set; }
    }
    public class QuaTrinhCongTacMTVM : QuaTrinhCongTacVM {
        public QuaTrinhCongTac GetQuaTrinhCongTac(QuaTrinhCongTac obj) {
            obj.TuNgay = this.TuNgay!.Value;
            obj.DenNgay = this.DenNgay!.Value;
            obj.MaChucVu = this.MaChucVu;
            obj.NoiLamViec = this.NoiLamViec;
            obj.GhiChu = this.GhiChu;
            obj.IDCanBo = this.NhanSu.IdCanbo!.Value;
            return obj;
        }
    }
    public class QuaTrinhCongTacDetailVM:QuaTrinhCongTacVM {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public String TenChucVu { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }
    }
    public class QuaTrinhCongTacSeachVM {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }
    }
}
