using HoiNongDan.Models.Entitys.NhanSu;
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
    public class HoiVienExcelVM
    {
        public Guid? IDCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string HoVaTen { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
       // [DataType(DataType.Date)]
        public String NgaySinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public bool GioiTinh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public String SoCCCD { get; set; }


        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayCapCCCD")]
        //public String? NgayCapCCCD { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoKhauThuongTru")]
        public string? HoKhauThuongTru { get; set; }

        [MaxLength(1000)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ChoOHienNay { get; set; }

        [MaxLength(100)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoDienThoai")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? SoDienThoai { get; set; }

        //[DataType(DataType.Date)]
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        //public DateTime? NgayvaoDangDuBi { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public DateTime? NgayVaoDangChinhThuc { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public string MaDanToc { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string? MaTonGiao { get; set; }

        [MaxLength(50)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoHocVan")]
        public string? MaTrinhDoHocVan { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChuyenMon")]
        public string? MaTrinhDoChuyenMon { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public string? MaTrinhDoChinhTri { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoHoi")]
        public String? NgayVaoHoi { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaCapUyDang")]
        public String? NgayThamGiaCapUyDang { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaHDND")]
        public String? NgayThamGiaHDND { get; set; }


        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VaiTro")]
        public string? VaiTro { get; set; }

        [Display(Name ="Quan hệ chủ hộ")]
        public string? VaiTroKhac { get; set; }

        [Display(Name = "Hộ nghèo")]
        public string? HoNgheo { get; set; }

        [Display(Name = "Cận nghèo")]
        public string? CanNgheo { get; set; }

        [Display(Name = "Gia đình chính sách, con liệt sĩ")]
        public string? GiaDinhChinhSach { get; set; }

        [Display(Name = "Thành phần khác")]
        public string? GiaDinhThanhPhanKhac { get; set; }

        [Display(Name = "Nông dân")]
        public string? NongDan { get; set; }

        [Display(Name = "Công nhân")]
        public string? CongNhan { get; set; }

        [Display(Name = "Công chức, viên chức")]
        public string? CV_VC { get; set; }

        [Display(Name = "Hưu trí")]
        public string? HuuTri { get; set; }

        [Display(Name = "Doanh nghiệp")]
        public string? DoanhNghiep { get; set; }

        [Display(Name = "Lao động tự do")]
        public string? LaoDongTuDo { get; set; }

        [Display(Name = "Học sinh, sinh viên")]
        public string? HS_SV { get; set; }

        [Display(Name = "Loại hình, dịch vụ sản xuất, chăn nuôi")]
        public string? SX_ChN { get; set; }

        [Display(Name = "Diện tích hoặc quy mô")]
        public string? DienTich_QuyMo { get; set; }


        [Display(Name = "Hiện tham gia sinh hoạt  đoàn thể chính trị, Hội đoàn nào khác")]
        public string? SinhHoatDoanTheChinhTri { get; set; }

        [Display(Name = "Tham gia câu lạc bộ, đội nhóm, mô hình, hợp tác xã, tổ hợp tác")]
        public string? ThamGia_CLB_DN_HTX { get; set; }

        [Display(Name = "Tham gia tổ hội ngành nghề, chi hội ngành nghề")]
        public string? ThamGia_THNN_CHNN { get; set; }

        [Display(Name = "HV ưu tú năm nào")]
        public string? HV_UuTuNam { get; set; }

        [Display(Name = "HV danh dự")]
        public string? HV_DanhDu { get; set; }

        [Display(Name = "NDSXKDG (cấp cơ sở, huyện,Tp, TW năm nào)")]
        public string? NDSXKDG { get; set; }

        [Display(Name = "ND tiêu biểu (năm nào)")]
        public string? NoDanTieuBieu { get; set; }

        [Display(Name = "ND Việt Nam xuất sắc")]
        public string? NDXuatSac { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "KKAnToanThucPham")]
        public string? KKAnToanThucPham { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DKMauNguoiNongDanMoi")]
        public string? DKMauNguoiNongDanMoi { get; set; }

        [Display(Name = "GhiChu")]
        public string? GhiChu { get; set; }

    }
    public class HoiVienImportExcel : HoiVienMTVM
    {
        public int RowIndex { get; set; }
        public bool isNullValueId { get; set; }
        public string Error { get; set; }

        public List<QuaTrinhKhenThuong> ListKhenThuong { get; set; }

    }
}
