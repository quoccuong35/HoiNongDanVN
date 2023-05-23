using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class MienNhiemVM
    {
        public Guid? IDQuaTrinhMienNhiem { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayQuyetDinh")]
        public DateTime? NgayQuyetDinh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoQuyetDinh")]
        public String SoQuyetDinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NguoiKy")]
        public String? NguoiKy { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        public Guid IdCoSo { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public Guid IdDepartment { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public Guid MaChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSoChucVu")]
        public decimal? HeSoChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Upload")]
        public FileDinhKem? FileDinhKem { get; set; }
        public NhanSuThongTinVM NhanSu { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Loai")]
        public LoaiBoNhiem Loai { get; set; }



    }
    public class MienNhiemMTVM : MienNhiemVM
    {
        public QuaTrinhMienNhiem GetMienNhiem(QuaTrinhMienNhiem obj) {
            obj.SoQuyetDinh = this.SoQuyetDinh;
            obj.NgayQuyetDinh = this.NgayQuyetDinh!.Value;
            obj.NguoiKy = this.NguoiKy!;
            obj.IdCoSo = this.IdCoSo;
            obj.IdDepartment = this.IdDepartment;
            obj.MaChucVu = this.MaChucVu;
            obj.HeSoChucVu = this.HeSoChucVu;
            obj.GhiChu = this.GhiChu;
            obj.IDCanBo = this.NhanSu.IdCanbo!.Value;
            return obj;
        }
    }
    public class MienNhiemDetail : MienNhiemVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        public String TenCoSo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public string TenDonVi { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public String TenChucVu { get; set; }
    }
}
