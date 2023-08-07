using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class KyLuatVM
    {
        public Guid? IdQuaTrinhKyLuat { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HinhThucKyLuat")]
        public String MaHinhThucKyLuat { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoQuyetDinh")]
        public String SoQuyetDinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NguoiKy")]
        public String? NguoiKy { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayKy")]
        public DateTime? NgayKy { get; set; }

        
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LyDo")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public String LyDo { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }
        public NhanSuThongTinVM NhanSu { get; set; }
    }
    public class KyLuatVMMT : KyLuatVM 
    {
        public QuaTrinhKyLuat GetKyLuat(QuaTrinhKyLuat obj) {
            obj.MaHinhThucKyLuat = this.MaHinhThucKyLuat;
            obj.SoQuyetDinh = this.SoQuyetDinh;
            obj.NgayKy = this.NgayKy!.Value;
            obj.NguoiKy = this.NguoiKy;
            obj.LyDo = this.LyDo;
            obj.GhiChu = this.GhiChu!;
            obj.IDCanBo = this.NhanSu.IdCanbo!.Value;
            return obj;
        }
    }
}
