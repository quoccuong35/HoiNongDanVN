using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class BoiDuongVM
    {
        public Guid? IDQuaTrinhBoiDuong { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayBatDau")]
        [DataType(DataType.Date)]
        public DateTime? NgayBatDau { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayKetThuc")]
        [DataType(DataType.Date)]
        public DateTime? NgayKetThuc { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiBoiDuong")]
        public String NoiBoiDuong { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDung")]
        public String NoiDung { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHinhThucDaoTao")]
        public string MaHinhThucDaoTao { get; set; }

        public String? FileDinhKem { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }
        public NhanSuThongTinVM NhanSu { get; set; }
    }
    public class BoiDuongMTVM : BoiDuongVM {
        public QuaTrinhBoiDuong GetQuaTrinhBoiDuong(QuaTrinhBoiDuong obj) {
            obj.MaHinhThucDaoTao = this.MaHinhThucDaoTao;
            obj.NoiBoiDuong = this.NoiBoiDuong;
            obj.NoiDung = this.NoiDung;
            obj.NgayBatDau = this.NgayBatDau!.Value;
            obj.NgayKetThuc = this.NgayKetThuc!.Value;
            obj.GhiChu = this.GhiChu;
            obj.IDCanBo = this.NhanSu.IdCanbo!.Value;
            this.FileDinhKem = this.FileDinhKem;
            return obj;
        }
    }
}
