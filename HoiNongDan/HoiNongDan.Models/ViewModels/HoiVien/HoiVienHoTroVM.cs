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
    public class HoiVienHoTroVM
    {
        public Guid? ID { get; set; }

        public HoiVienInfo HoiVien { get; set; }
  
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDung")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string NoiDung { get; set; }

       // [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LopHoc")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Loại Hỗ trợ, Đào tạo, Tập huấn")]
        public Guid? IDLopHoc { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }

    }
    public class HoiVienHoTroMTVM : HoiVienHoTroVM
    {
        public HoiVienHoTro GetHoTro(HoiVienHoTro obj) {
            obj.NoiDung = this.NoiDung;
            obj.GhiChu = this.GhiChu!;
            obj.IDHoiVien = this.HoiVien.IdCanbo!.Value;

        
            //obj.MaNguonVon = this.MaNguonVon;
           // obj.TraXong = this.TraXong;
            return obj;
        }
    }
    public class HoiVienHoTroDetailVM
    {
        public Guid ID { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHV")]
        public String MaHV { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenHV")]
        public String TenHV { get; set; }

        [Display(Name = "Nội dung hỗ trợ")]
        public string NoiDung { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public string QuanHuyen { get; set; }
        [Display(Name = "Tên Hội Nông Dân")]
        public string TenHoi { get; set; }

        [Display(Name = "Lớp đào tạo, tập huấn")]
        public String? TenLopHoc { get; set; }

        [Display(Name = "Ghi chú")]
        public String? GhiChu { get; set; }
    }
    public class HVHoTroSearchVM {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NamVayVon")]
        public int? NamVayVon { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public String SoCCCD { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TenHV")]
        public String TenHV { get; set; }
        public bool? Actived { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HinhThucHoTro")]
        public Guid? MaHinhThucHoTro { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LopHoc")]
        public Guid? IDLopHoc { get; set; }
    }

    public class VayVonQuaHanSearchVM {
        [Display(Name ="Ngày")]
        public DateTime Ngay { get; set; }
        [Display(Name = "Số tháng quá hạn")]
        public int SoThang { get; set; }
    }
}
