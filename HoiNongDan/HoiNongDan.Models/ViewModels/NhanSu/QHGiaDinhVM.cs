using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class QHGiaDinhVM
    {
        public Guid IDQuanheGiaDinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string HoTen { get; set; }

        [MaxLength(10)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public String NgaySinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgheNghiep")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? NgheNghiep { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiLamVien")]
        public string? NoiLamVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaChi")]
        public string? DiaChi { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LoaiQuanhe")]
        public Guid IDLoaiQuanHeGiaDinh { get; set; }

        public NhanSuThongTinVM NhanSu { get; set; }
        public HoiVienInfo HoiVien { get; set; }
        public QHGiaDinhVM() {
            NhanSu = new NhanSuThongTinVM();
            HoiVien = new HoiVienInfo();
        }
    }
    public class QHGiaDinhVMMT: QHGiaDinhVM {
        public QuanHeGiaDinh GetQuanHeGiaDinh(QuanHeGiaDinh item) {
            item.IDLoaiQuanHeGiaDinh = this.IDLoaiQuanHeGiaDinh;
            item.HoTen = this.HoTen;
            item.NgaySinh = this.NgaySinh;
            item.NgheNghiep = this.NgheNghiep;
            item.NoiLamVien = this.NoiLamVien;
            item.DiaChi = this.DiaChi;
            item.GhiChu = this.GhiChu;
            if (this.NhanSu != null && this.NhanSu.IdCanbo != null)
            {
                item.IDCanBo = this.NhanSu.IdCanbo;
            }
            else
            {
                item.IDHoiVien = this.HoiVien.IdCanbo;
            }
            return item;
        }
    }
}
