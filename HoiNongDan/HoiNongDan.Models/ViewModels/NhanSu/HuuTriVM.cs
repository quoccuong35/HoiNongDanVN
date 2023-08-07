using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HuuTriVM
    {
        public Guid? Id { get; set; }
        public Guid IDCanBo { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayQuyetDinh")]
        public DateTime? NgayQuyetDinh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoQuyetDinh")]
        public String SoQuyetDinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NguoiKy")]
        public String? NguoiKy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool Actived { get; set; } = true;


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Upload")]
        public FileDinhKem? FileDinhKem { get; set; }
        public NhanSuThongTinVM NhanSu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CapNhatTinhTrangCanBo")]
        public bool CapNhatTinhTrangCanBo { get; set; } = true;

    }
    public class HuuTriMTVM : HuuTriVM {
        public HuuTri GetHuuChi(HuuTri obj) {
            obj.SoQuyetDinh = this.SoQuyetDinh;
            obj.NgayQuyetDinh = this.NgayQuyetDinh!.Value;
            obj.NguoiKy = this.NguoiKy!;
            obj.GhiChu = this.GhiChu!;
            obj.IDCanBo = this.NhanSu.IdCanbo!.Value;
            return obj;
        }
    }
    public class HuuTriSearchVM {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoQuyetDinh")]
        public string? SoQuyetDinh { get; set; }
    }
    public class HuuTriDetail : HuuTriVM
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
