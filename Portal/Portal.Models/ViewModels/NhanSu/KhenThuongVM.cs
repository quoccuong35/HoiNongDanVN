
using Portal.Models.Entitys.NhanSu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class KhenThuongVM
    {
        public Guid? IDQuaTrinhKhenThuong { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HinhThucKhenThuong")]
        public string MaHinhThucKhenThuong { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanhHieuKhenThuong")]
        public string MaDanhHieuKhenThuong { get; set; }

   
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayQuyetDinh")]
        [DataType(DataType.Date)]
        public DateTime? NgayQuyetDinh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoQuyetDinh")]
        public String SoQuyetDinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NguoiKy")]
        public String? NguoiKy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LyDo")]
        public String? LyDo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }

        public NhanSuThongTinVM NhanSu { get; set; }
    }
    public class KhenThuongVMMT:KhenThuongVM {
        public QuaTrinhKhenThuong GetKhenThuong(QuaTrinhKhenThuong obj) {
            obj.MaHinhThucKhenThuong = this.MaHinhThucKhenThuong;
            obj.MaDanhHieuKhenThuong = this.MaDanhHieuKhenThuong;
            obj.SoQuyetDinh = this.SoQuyetDinh;
            obj.NgayQuyetDinh = this.NgayQuyetDinh!.Value;
            obj.LyDo = this.LyDo!;
            obj.GhiChu = this.GhiChu!;
            obj.NguoiKy = this.NguoiKy;
            obj.IDCanBo = this.NhanSu.IdCanbo!.Value;
            return obj;
        }
    }
}
