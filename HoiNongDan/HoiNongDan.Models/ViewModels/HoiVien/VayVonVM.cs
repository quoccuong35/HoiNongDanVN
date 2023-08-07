using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HoiNongDan.Models
{
    public class VayVonVM
    {
        public Guid? IDVayVon { get; set; }

        public NhanSuThongTinVM NhanSu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoTienVay")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string SoTienVay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ThoiHangChoVay")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, 120, ErrorMessage = "Số tháng không hợp lệ")]
        public int ThoiHangChoVay { get; set; } = 24;

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LaiSuatVay")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(5,50, ErrorMessage = "Lãi suất vay không hợp lệ")]
        public double LaiSuatVay { get; set; } = 11;

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TuNgay")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        public DateTime? TuNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DenNgay")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        public DateTime? DenNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayTraNoCuoiCung")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Date)]
        public DateTime? NgayTraNoCuoiCung { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDungVay")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string NoiDungVay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TraXong")]
        public bool TraXong { get; set; } = false;
    }
    public class VayVonMTVM : VayVonVM {
        public HoiVienVayVon GetVayVon(HoiVienVayVon obj) {
            obj.SoTienVay = long.Parse(this.SoTienVay.Replace(",",""));
            obj.LaiSuatVay = this.LaiSuatVay;
            obj.ThoiHangChoVay = this.ThoiHangChoVay;
            obj.TuNgay = this.TuNgay!.Value;
            obj.DenNgay = this.DenNgay!.Value;
            obj.NgayTraNoCuoiCung = this.NgayTraNoCuoiCung!.Value;
            obj.NoiDungVay = this.NoiDungVay;
            obj.GhiChu = this.GhiChu!;
            obj.IDCanBo = this.NhanSu.IdCanbo!.Value;
           // obj.TraXong = this.TraXong;
            return obj;
        }
    }
    public class VayVonDetailVM{
        public Guid IDVayVon { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHV")]
        public String MaHV { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenHV")]
        public String TenHV { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoTienVay")]
        public long SoTienVay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ThoiHangChoVay")]
        public int ThoiHangChoVay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LaiSuatVay")]
        public double LaiSuatVay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TuNgay")]
        public DateTime TuNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DenNgay")]
        public DateTime DenNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayTraNoCuoiCung")]
        public DateTime NgayTraNoCuoiCung { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDungVay")]
        public string NoiDungVay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoThangQuaHan")]
        public int SoThangQuaHan { get; set; }
        public bool TraXong { get; set; }

    }
    public class VayVonSearchVM {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NamVayVon")]
        public int? NamVayVon { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHV")]
        public String MaHV { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenHV")]
        public String TenHV { get; set; }
        public bool? Actived { get; set; }
    }

    public class VayVonQuaHanSearchVM {
        [Display(Name ="Ngày")]
        public DateTime Ngay { get; set; }
        [Display(Name = "Số tháng quá hạn")]
        public int SoThang { get; set; }
    }
}
