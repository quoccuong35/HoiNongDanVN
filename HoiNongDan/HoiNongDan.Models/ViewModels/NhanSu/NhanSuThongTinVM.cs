using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HoiNongDan.Models
{
    public class NhanSuThongTinVM
    {
        public Guid? IdCanbo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        public string? TenDonVi { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public string? TenCoSo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhTrang")]
        public string? TenTinhTrang { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhanHe")]
        public string? TenPhanHe { get; set; }

        public string HinhAnh { get; set; } = @"\images\login.png";
        public string? Error { get; set; }
        public bool CanBo { get; set; }
        public bool Edit { get; set; } = true;

        public NhanSuThongTinVM GeThongTin(CanBo canBo) {
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu.IdCanbo = canBo.IDCanBo;
            nhanSu.HoVaTen = canBo.HoVaTen;
            nhanSu.MaCanBo = canBo.MaCanBo;
            nhanSu.TenTinhTrang = canBo.TinhTrang.TenTinhTrang;
            nhanSu.TenCoSo = canBo.CoSo.TenCoSo;
            nhanSu.TenDonVi = canBo.Department.Name;
            nhanSu.TenPhanHe = canBo.PhanHe.TenPhanHe;
            if (!String.IsNullOrWhiteSpace(canBo.HinhAnh) && !String.IsNullOrEmpty(canBo.HinhAnh))
            { 
                nhanSu.HinhAnh = canBo.HinhAnh;
            }
            return nhanSu;
        }
    }
}
