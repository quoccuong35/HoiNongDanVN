using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HoiNongDan.Models
{
    public class KhenThuongSearchVN
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanhHieuKhenThuong")]
        public String? MaDanhHieuKhenThuong { get; set; }


        [Display(Name = "Từ Năm")]
        public int? TuNam { get; set; }
        [Display(Name = "Đến Năm")]
        public int? DenNam { get; set; }
    }
}
