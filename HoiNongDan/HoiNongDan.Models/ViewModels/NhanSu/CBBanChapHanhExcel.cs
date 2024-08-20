using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class CBBanChapHanhExcel
    {
        public int? STT { get; set; }
        public Guid? IDCanBo { get; set; }
        public string HoVaTen { get; set; }
        public string? TenChucVu { get; set; }
        public String? DonVi { get; set; }
        public String? NgaySinh_Nam { get; set; }

        public String? NgaySinh_Nu { get; set; }
        public string? TenDanToc { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string? TenTonGiao { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiSinh")]

        public string? NoiSinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        public string? ChoOHienNay { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        public string? NgayvaoDangDuBi { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public string? NgayVaoDangChinhThuc { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChuyenNganh")]
        public string ChuyenNganh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public string? TenTrinhDoChinhTri { get; set; }

        public string? SoDienThoai { get; set; }
        public string? ThoiGianChuyenCogTac { get; set; }
        public string? ChucVuMoi { get; set; }
    }
    public class CBBanChapHanhImportExcel {
        public int RowIndex { get; set; }
        public bool isNullValueId { get; set; }
        public string Error { get; set; }
        public CanBo CanBo { get; set; }
    }
}
