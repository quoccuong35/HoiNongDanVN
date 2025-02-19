using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HVKhenThuongDetailVM
    {
        public Guid? IDQuaTrinhKhenThuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }
        [Display(Name = "Quận huyện")]
        public String? QuanHuyen { get; set; }

        [Display(Name = "Địa bàn HND")]
        public String? DiaBanHND { get; set; }
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "HinhThucKhenThuong")]
        //public string TenHinhThucKhenThuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanhHieuKhenThuong")]
        public string TenDanhHieuKhenThuong { get; set; }

        [Display(Name="Năm")]
        public int? Nam { get; set; }

        //[DataType(DataType.Date)]
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayQuyetDinh")]
        //public DateTime? NgayQuyetDinh { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoQuyetDinh")]
        //public String? SoQuyetDinh { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "NguoiKy")]
        //public String? NguoiKy { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "LyDo")]
        //public String? LyDo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDung")]
        public String? NoiDung { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CapKhenThuong")]
        public String? CapKhenThuong { get; set; }

    }
}
