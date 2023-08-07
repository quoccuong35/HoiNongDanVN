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
    public class BoNhiemDetailVM
    {
        public Guid IdQuaTrinhBoNhiem { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayQuyetDinh")]
        public DateTime NgayQuyetDinh { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoQuyetDinh")]
        public String SoQuyetDinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NguoiKy")]
        public String? NguoiKy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        public String TenCoSo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public string TenDonVi { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public String TenChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSoChucVu")]
        public decimal? HeSoChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }

    }
}
