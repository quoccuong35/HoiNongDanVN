using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HVDangKyImportVM
    {
        public int STT { get; set; }
        public Guid? ID { get; set; } 
        public String? HoVaTen { get; set; }
        public String? Nam { get; set; }
        public String? Nu { get; set; }
        public String? SoCCCD { get; set; }
        public String? NgayCapSoCCCD { get; set; }
        public String? HoKhauThuongTru { get; set; }
        public String? NoiOHiennay { get; set; }
        public String? SoDienThoai { get; set; }
        public String? DangVien { get; set; }
        public String? MaDanToc { get; set; }
        public String? MaTonGiao { get; set; }
        public String? MaTrinhDoHocVan { get; set; }
        public String? MaTrinhDoChuyenMon { get; set; }
        public String? MaTrinhDoChinhTri { get; set; }
        public String? MaNgheNghiep { get; set; }
        public String? DiaBanDanCu { get; set; }
        public String? NganhNghe { get; set; }
    }
    public class HVDangKyImportNewVM
    {
        public int STT { get; set; }
        public Guid? ID { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public String? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        public String? NgaySinh { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        public String? GioiTinh { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public String? ChucVu { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public String? SoCCCD { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayCapCCCD")]
        public String? NgayCapSoCCCD { get; set; }

        [Display(Name = "Tổ hội")]
        public String? TenToHoi { get; set; }
        [Display(Name = "Chi hội")]
        public String? TenChiHoi { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoKhauThuongTru")]
        public String? HoKhauThuongTru { get; set; }
        public String? HoKhauThuongTru_XaPhuong { get; set; }
        public String? HoKhauThuongTru_QuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        public String? ChoOHienNay { get; set; }
        public String? ChoOHienNay_XaPhuong { get; set; }
        public String? ChoOHienNay_QuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoDienThoai")]
        public String? SoDienThoai { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        public String? DangVienDuBi { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public String? DangVienChinhThuc { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public String? MaDanToc { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public String? MaTonGiao { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoHocVan")]
        public String? MaTrinhDoHocVan { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChuyenMon")]
        public String? MaTrinhDoChuyenMon { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public String? MaTrinhDoChinhTri { get; set; }
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
        public string? SX_ChN { get; set; }
        [Display(Name = "Số lượng")]
        public string? SoLuong { get; set; }
        [Display(Name = "Diện tích hoặc quy mô")]
        public string? DienTich_QuyMo { get; set; }

        [Display(Name = "Hiện tham gia sinh hoạt  đoàn thể chính trị, Hội đoàn nào khác")]
        public string? SinhHoatDoanTheChinhTri { get; set; }

        [Display(Name = "Tham gia câu lạc bộ, đội nhóm, mô hình, hợp tác xã, tổ hợp tác")]
        public string? ThamGia_CLB_DN_HTX { get; set; }
    }
    public class HVDangKyDuyetImportVM
    {
        public int STT { get; set; }
        public Guid ID { get; set; }
        public String? HoVaTen { get; set; }
        public String? Nam { get; set; }
        public String? Nu { get; set; }
        public String? SoCCCD { get; set; }
        public String? NgayCapSoCCCD { get; set; }
        public String? TenQuanHuyen { get; set; }
        public String? TenHoi { get; set; }
        public String? HoKhauThuongTru { get; set; }
        public String? NoiOHiennay { get; set; }
        public String? SoDienThoai { get; set; }
        public String? DangVien { get; set; }
        public String? MaDanToc { get; set; }
        public String? MaTonGiao { get; set; }
        public String? MaTrinhDoHocVan { get; set; }
        public String? MaTrinhDoChuyenMon { get; set; }
        public String? MaTrinhDoChinhTri { get; set; }
      
        public String? MaNgheNghiep { get; set; }
        public String? DiaBanDanCu { get; set; }

        public String? NganhNghe { get; set; }
        public String? SoQuyetDinh { get; set; }
        public String? NgayThangVaoHoi { get; set; }
        public String? SoThe { get; set; }
        public String? NgayCapThe { get; set; }
    }
    public class HVDangKyDuyetDetailVM
    {
        public Guid? ID { get; set; }
        public int STT { get; set; }
        public String? HoVaTen { get; set; }
        public String? Nam { get; set; }
        public String? Nu { get; set; }
        public String? SoCCCD { get; set; }
        public String? NgayCapSoCCCD { get; set; }
        public String? TenQuanHuyen { get; set; }
        public String? TenHoi { get; set; }
        public String? HoKhauThuongTru { get; set; }
        public String? NoiOHiennay { get; set; }
        public String? SoDienThoai { get; set; }
        public String? DangVien { get; set; }
        public String? DanToc { get; set; }
        public String? TonGiao { get; set; }
        public String? TrinhDoHocVan { get; set; }
        public String? TrinhDoChuyenMon { get; set; }
        public String? ChinhTri { get; set; }
        public DateTime? NgayThangVaoHoi { get; set; }
        public String? NgheNghiep { get; set; }
        public String? DiaBanDanCu { get; set; }
        public String? NganhNghe { get; set; }
        public String? SoThe { get; set; }
        public String? NgayCapThe { get; set; }
        public bool TrangThai { get; set; } = false;
        public String? SoQuyetDinh { get; set; }
        public String? NgayQuyetDinh { get; set; }
    }
}
