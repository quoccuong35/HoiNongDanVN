using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HVDangKyVM
    {
        public Guid? ID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }


        [Display(Name ="Ngày đăng ký")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime? NgayDangKy { get; set; }
        //[MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string HoVaTen { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        public String? NgaySinh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        public GioiTinh GioiTinh { get; set; }


        [MaxLength(200)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayCapCCCD")]
        public String? NgayCapCCCD { get; set; }



        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChiHoi")]
        public Guid? MaChiHoi { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToHoi")]
        public Guid? MaToHoi { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoKhauThuongTru")]
        public string? HoKhauThuongTru { get; set; }
        public String? HoKhauThuongTru_XaPhuong { get; set; }
        public String? HoKhauThuongTru_QuanHuyen { get; set; }


        [MaxLength(1000)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ChoOHienNay { get; set; }
        public String? ChoOHienNay_XaPhuong { get; set; }
        public String? ChoOHienNay_QuanHuyen { get; set; }


        [MaxLength(100)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoDienThoai")]
        public string? SoDienThoai { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        public String? NgayvaoDangDuBi { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public String? NgayVaoDangChinhThuc { get; set; }

        [MaxLength(50)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public string MaDanToc { get; set; }

        [MaxLength(50)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string? MaTonGiao { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoHocVan")]
        public string? MaTrinhDoHocVan { get; set; }


        [MaxLength(20)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChuyenMon")]
        public string? MaTrinhDoChuyenMon { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChuyenNganh")]
        public string? ChuyenNganh { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public string? MaTrinhDoChinhTri { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaCapUyDang")]
        public String? NgayThamGiaCapUyDang { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaHDND")]
        public String? NgayThamGiaHDND { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VaiTro")]
        public string? VaiTro { get; set; }
        public string? VaiTroKhac { get; set; }
        [Display(Name = "Gia đình thuộc diện")]
        public string? MaGiaDinhThuocDien { get; set; }
        public string? GiaDinhThuocDienKhac { get; set; }

        [Display(Name = "Nghề nghiệp hiện nay")]
        public String? MaNgheNghiep { get; set; }
        [Display(Name = "Loại hình, dịch vụ sản xuất, chăn nuôi")]
        public string? Loai_DV_SX_ChN { get; set; }
        [Display(Name = "Số lượng")]
        public string? SoLuong { get; set; }
        [Display(Name = "Diện tích hoặc quy mô")]
        public string? DienTich_QuyMo { get; set; }

        [Display(Name = "Đoàn thể chính trị-Hội đoàn khác")]
        public List<Guid>? MaDoanTheChinhTri_HoiDoan { get; set; }
        [Display(Name = "Tham gia câu lạc bộ, đội nhóm, mô hình, hợp tác xã, tổ hợp tác")]
        public List<Guid>? Id_CLB_DN_MH_HTX_THT { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DangVien")]
        public bool DangVien { get; set; } = false;
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoiVienDanCu")]
        public bool HoiVienDanCu { get; set; } = false;
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoiVienNganhNghe")]
        public bool HoiVienNganhNghe { get; set; } = false;
    }

    public class HVDangKySearchVM {

        [Display(Name = "Đăng ký từ ngày")]
        [DataType(DataType.Date)]
        public DateTime? TuNgay { get; set; }

        [Display(Name = "Đăng ký Đến ngày")]
        [DataType(DataType.Date)]
        public DateTime? DenNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public string? MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string HoVaTen { get; set; }

        public String TrangThai { get; set; } = "1";
    }
}
