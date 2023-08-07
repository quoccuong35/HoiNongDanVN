
using HoiNongDan.Models.Entitys.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class DiaBanHoatDongVM
    {
        public Guid Id { get; set; }
        [MaxLength(250)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenDiaBanHoatDong")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string TenDiaBanHoatDong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhThanhPho")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        public String MaTinhThanhPho { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuongXa")]
        public String? MaPhuongXa { get; set; }

        [MaxLength(250)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaChi")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public String DiaChi { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThanhLap")]
        [DataType(DataType.Date)]
        public DateTime? NgayThanhLap { get; set; }
        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool Actived { get; set; } = true;
        public ICollection<DBHoatDongHoiVienDetailVM> DBHoiVienDetails { get; set; }

        public DBHoatDongHoiVienVM? DBHoiVien { get; set; }

        public DiaBanHoatDongVM() {
            DBHoiVienDetails = new List<DBHoatDongHoiVienDetailVM>();
        }
    }
    public class DiaBanHoatDongMTVM :DiaBanHoatDongVM{
        public DiaBanHoatDong GetDiaBanHongDong(DiaBanHoatDong obj) {
            obj.TenDiaBanHoatDong = this.TenDiaBanHoatDong;
            obj.DiaChi = this.DiaChi;
            obj.NgayThanhLap = this.NgayThanhLap; ;
            obj.MaTinhThanhPho = this.MaTinhThanhPho;
            obj.MaQuanHuyen = this.MaQuanHuyen!;
            obj.MaPhuongXa = this.MaPhuongXa!;
            obj.GhiChu = this.GhiChu!;
            return obj;
        }

    }
    public class DiaBanHoatDongSerachVM {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenDiaBanHoatDong")]
        public string TenDiaBanHoatDong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhThanhPho")]
        public String MaTinhThanhPho { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuongXa")]
        public String MaPhuongXa { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; } = true;
    }
    public class DiaBanHoatDongDetailVM {
        public Guid Id { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenDiaBanHoatDong")]
        public string TenDiaBanHoatDong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhThanhPho")]
        public String TenTinhThanhPho { get; set; }
        public string DiaChi { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThanhLap")]
        public DateTime? NgayThanhLap { get; set; }
        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string GhiChu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoLuongHoiVien")]
        public int SoLuongHoiVien { get; set; }
        
    }
    public class DBHoatDongHoiVienDetailVM
    { 
        public Guid Id { get; set; }
        public Guid IDDiaBan { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public String MaHoiVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public String HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public String TenChucVu { get; set; }
    }
    public class DBHoatDongHoiVienVM
    {
        public Guid? Id { get; set; }
        public Guid? IdDiaBan { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoiVien")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid IDCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid MaChucVu { get; set; }
        
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVao")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime? NgayVao { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayRoiDi")]
        public DateTime? NgayRoiDi { get; set; }
    }
}
