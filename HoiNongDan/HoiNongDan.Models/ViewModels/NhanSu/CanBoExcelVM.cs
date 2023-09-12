using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor.Tokenizer.Symbols;
using System.Xml.Linq;

namespace HoiNongDan.Models
{
    public class CanBoExcelVM
    {
        public Guid? IDCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string HoVaTen { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        //[DataType(DataType.Date)]
        public String NgaySinh { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public bool GioiTinh { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        public string TenCoSo { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public string TenDonVi { get; set; }



        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhTrang")]
        public string TenTinhTrang { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhanHe")]
        [MaxLength(50)]
        public string TenPhanHe { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public string TenChucVu { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public String SoCCCD { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayCapCCCD")]
        public String? NgayCapCCCD { get; set; }




        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoDienThoai")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? SoDienThoai { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Email")]
        public string? Email { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgachLuong")]
        public string? TenNgachLuong { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BacLuong")]
        public int? BacLuong { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSo")]
        public decimal? HeSoLuong { get; set; }
        [DataType(DataType.Date)]

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayNangBacLuong")]
        public DateTime? NgayNangBacLuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuCapChucVu")]
        public decimal? PhuCapChucVu { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuCapVuotKhung")]
        public decimal? PhuCapVuotKhung { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuCapKiemNhiem")]
        public decimal? PhuCapKiemNhiem { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuCapKhuVuc")]
        public decimal? PhuCapKhuVuc { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LuongKhoan")]
        public int? LuongKhoan { get; set; }


        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "KhoanTuNgay")]
        public DateTime? KhoanTuNgay { get; set; }


        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "KhoanDenNgay")]
        public DateTime? KhoanDenNgay { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [MaxLength(100)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoBHXH")]
        public string SoBHXH { get; set; }

    
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoBHYT")]
        public string SoBHYT { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaSoThue")]
        public string MaSoThue { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoBienChe")]
        public DateTime? NgayVaoBienChe { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaCongTac")]
        public String? NgayThamGiaCongTac { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeDaoTao")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        public string TenHeDaoTao { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoHocVan")]
        public string TenTrinhDoHocVan { get; set; }

   
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChuyenNganh")]
        public string ChuyenNganh { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoTinHoc")]
        public string? TenTrinhDoTinHoc { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoNgoaiNgu")]
        public string? TenTrinhDoNgoaiNgu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public string? TenTrinhDoChinhTri { get; set; }

      
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HocHam")]
        public string? TenHocHam { get; set; }

    
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public string TenDanToc { get; set; }

   
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string TenTonGiao { get; set; }

 
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiSinh")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string NoiSinh { get; set; }

   
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ChoOHienNay { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        public DateTime? NgayvaoDangDuBi { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public DateTime? NgayVaoDangChinhThuc { get; set; }

      
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }
    }
    public class CanBoImportExcel : CanBoVMMT {
        public int RowIndex { get; set; }
        public bool isNullValueId { get; set; }
        public string Error { get; set; }

    }
}
