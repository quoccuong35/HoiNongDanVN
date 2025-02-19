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
        public int? STT { get; set; }
        public Guid? IDCanBo { get; set; }

        public string HoVaTen { get; set; }
        public String? GioiTinh { get; set; }
        public String? NgaySinh { get; set; }

        public String? SoCCCD { get; set; }

        public String? NgayCapSoCCCD { get; set; }

        public string? TenChucVu { get; set; }

        public String? DonVi { get; set; }

        public String? SoQuyetDinhBoNhiem { get; set; }

        public string? TenDanToc { get; set; }



        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string? TenTonGiao { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiSinh")]

        public string? NoiSinh { get; set; }

        [Display(Name ="Nơi cư trú")]
        public string? ChoOHienNay { get; set; }
        public string? ChoOHienNay_XaPhuong { get; set; }
        public string? ChoOHienNay_QuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        public string? NgayvaoDangDuBi { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public string? NgayVaoDangChinhThuc { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoHocVan")]
        public string? MaTrinhDoHocVan { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChuyenMon")]
        public string? MaTrinhDoChuyenMon { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChuyenNganh")]
        public string ChuyenNganh { get; set; }

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
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSo")]
        public decimal? HeSoLuong { get; set; }
        [DataType(DataType.Date)]

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayNangBacLuong")]
        public DateTime? NgayNangBacLuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuCapVuotKhung")]
        public decimal? PhuCapVuotKhung { get; set; }


        [Display(Name ="Mốc nâng bậc lần sau")]
        public DateTime? NgayNangBacLuongLanSau { get; set; }

        public string? NVCTTW { get; set; }

        public string? GiangVienKiemChuc { get; set; }

        public string? QLCapPhong { get; set; }

        public string? KTQP { get; set; }

        public string? QLNNCV { get; set; }


        public string? QLNNCVC { get; set; }

        public string? QLNNCVCC { get; set; }

        public string? DanhGiaCBCC { get; set; }

        public string? DanhGiaDangVien { get; set; }
        public string? BanChapHanh { get; set; }

        public string? GhiChu { get; set; }
    }
    public class CanBoHNDTPDelTailVM : CanBo1ExcelVM { 
        public String TenGioiTinh { get; set; }
        public String TenTrinhDoChinhTri { get; set; }
        public String DonVi { get; set; }
        public String NgaySinh {  get; set; }
        public String TenDonVi {  get; set; }
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
