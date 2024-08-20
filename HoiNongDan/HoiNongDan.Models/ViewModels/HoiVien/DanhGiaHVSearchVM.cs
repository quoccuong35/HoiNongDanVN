using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HoiNongDan.Models
{
    public class DanhGiaHVVM { 
        public Guid ID { get; set; }
        public String SoTheHoiVien { get; set; }
        public String HoVaTen { get; set; }
        public String NgaySinhNam { get; set; }
        public String NgaySinhNu { get; set; }
        public String SoCCCD { get; set; }
        public String? PhanLoai_XuatSac { get; set; }
        public String? PhanLoai_HTTot { get; set; }
        public String? PhanLoai_HTNhiemVu { get; set; }
        public String? PhanLoai_KhongHoanThanhNhiemVu { get; set; }
        public String? KhongPhanLoai { get; set; }
        public String? ChuaDuDieuKienPhanLoai { get; set; }
        public String? GhiChu { get; set; }
    }
    public class DanhGiaHVDetailVM : DanhGiaHVVM { 
        public Guid? IDDanhGia { get; set; }
    }
    public class DanhGiaHVSearchVM
    {
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Nam")]
        public int? Nam { get; set; }

        public bool DaDanhGia { get; set; } = false;
        public string? Loai { get; set; } = "01";
    }
}
