using HoiNongDan.Models.Entitys.HoiVien;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models 
{ 
    public class BanChapHanhCHDetail
    {
        public Guid? IDCanBo { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string HoVaTen { get; set; }

        [Display(Name ="Nam")]
        public string? NgaySinh_Nam { get; set; }

        [Display(Name = "Nữ")]
        public string? NgaySinh_Nu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public string? MaCanBo { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public string? ChucVu { get; set; }

        [Display(Name = "Tên Chi Hội")]
        public string? ChiHoi { get; set; }

        [Display(Name = "Huyện")]
        public string? Huyen { get; set; }

        [Display(Name = "Xã")]
        public string? Xa { get; set; }
        [Display(Name = "Quê quán")]
        public string? QueQuan { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public string DanToc { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string? TonGiao { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoHocVan")]
        public string? TrinhDoHocVan { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public string? TrinhDoChinhTri { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        public String? NgayvaoDangDuBi { get; set; }

        //[DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public String? NgayVaoDangChinhThuc { get; set; }
    }
    public class BanChapHanhCHExcel {
        public List<String> error { get; set; }
        public bool edit { get; set; } = false;
        public List<ChiHoi> addChiHoi { get; set; }
        public List<HoiVien_ChiHoi>HoiVienChiHois {get;set;}
        public CanBo hoiVien { get; set; }
        public BanChapHanhCHExcel() {
            addChiHoi = new List<ChiHoi>();
            HoiVienChiHois = new List<HoiVien_ChiHoi>();
        }
    }
}
