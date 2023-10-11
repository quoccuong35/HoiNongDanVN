using HoiNongDan.Models.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor.Tokenizer.Symbols;
using System.Xml.Linq;

namespace HoiNongDan.Models
{
    public class CanBo1ExcelVM
    {
        public Guid? IDCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string HoVaTen { get; set; }


        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        //[DataType(DataType.Date)]
        public String? NgaySinh { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public bool GioiTinh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public string TenDonVi { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public string TenChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoQuyetDinhBoNhiem")]
        public String? SoQuyetDinhBoNhiem { get; set; }


        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public string? TenDanToc { get; set; }


        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string? TenTonGiao { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiSinh")]

        public string? NoiSinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        public string? ChoOHienNay { get; set; }


        //[DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        public string? NgayvaoDangDuBi { get; set; }

        //[DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public string? NgayVaoDangChinhThuc { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChuyenNganh")]
        public string ChuyenNganh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoHocVan")]
        public string? TenTrinhDoHocVan { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChuyenMon")]
        public string? TenTrinhDoChuyenMon { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public string? TenTrinhDoChinhTri { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoNgoaiNgu")]
        public string? TenTrinhDoNgoaiNgu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoTinHoc")]
        public string? TenTrinhDoTinHoc { get; set; }

        [Display(Name = "Thời gian công tác Hội")]
        public string? NgayThamGiaCongTac { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaCapUyDang")]
        public String? NgayThamGiaCapUyDang { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaHDND")]
        public String? NgayThamGiaHDND { get; set; }
       

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgachLuong")]
        public string? TenNgachLuong { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BacLuong")]
        public int? BacLuong { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSo")]
        public decimal? HeSoLuong { get; set; }
        [DataType(DataType.Date)]

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuCapVuotKhung")]
        public decimal? PhuCapVuotKhung { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayNangBacLuong")]
        public DateTime? NgayNangBacLuong { get; set; }

        [Display(Name = "Chuyên môn")]
        public string? ChuyeMon { get; set; }

        [Display(Name = "LLCT")]
        public string? LLCT { get; set; }

        [Display(Name = "NVCT Hội do TW")]
        public string? NVCTTW { get; set; }

        [Display(Name = "Giảng viên kiêm chức")]
        public string? GiangVienKiemChuc { get; set; }

        [Display(Name = "QL cấp phòng")]
        public string? QLCapPhong { get; set; }

        [Display(Name = "KT QP&AN")]
        public string? KTQP { get; set; }

        [Display(Name = "QLNN ngạch chuyên viên")]
        public string? QLNNCV { get; set; }

        [Display(Name = "QLNN ngạch chuyên viên chính")]
        public string? QLNNCVC { get; set; }

        [Display(Name = "QLNN ngạch chuyên viên CC")]
        public string? QLNNCVCC { get; set; }

        [Display(Name = "Đánh giá CBCC")]
        public string? DanhGiaCBCC { get; set; }

        [Display(Name = "Đánh giá Đảng viên")]
        public string? DanhGiaDangVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }
    }
    public class CanBoHNDTPDelTailVM : CanBo1ExcelVM { 
        public String TenGioiTinh { get; set; }
        public String TenTrinhDoChinhTri { get; set; }
    }
    public class CanBo1ImportExcel 
    {
        public int RowIndex { get; set; }
        public bool isNullValueId { get; set; }
        public string Error { get; set; }
        public CanBo CanBo { get; set; }
        public List<DaoTaoBoiDuong> daoTaoBoiDuongs { get; set; }

        public CanBo1ImportExcel() {
            daoTaoBoiDuongs = new List<DaoTaoBoiDuong>();
        }

    }
}
