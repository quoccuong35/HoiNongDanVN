using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.Entitys.MasterData;
using HoiNongDan.Models.Entitys.NhanSu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public enum GioiTinh
    {
        Nữ,
        Nam
    };
    public class CanBo
    {
        public Guid IDCanBo { get; set; }
        [MaxLength(20)]
        public string MaCanBo { get; set; }

        [MaxLength(20)]
        public string? MaDinhDanh { get; set; }
        [MaxLength(500)]
        public string HoVaTen { get; set; }

        // [DataType(DataType.Date)]
        public String? NgaySinh { get; set; }

        public GioiTinh GioiTinh { get; set; }

        [MaxLength(200)]
        public String? SoCCCD { get; set; }

        [MaxLength(20)]
        public String? NgayCapCCCD { get; set; }
        [MaxLength(50)]
        public string? MaTinhTrang { get; set; }
        public TinhTrang? TinhTrang { get; set; }

        [MaxLength(50)]
        public string? MaPhanHe { get; set; }
        public PhanHe? PhanHe { get; set; }
        public Guid? IdCoSo { get; set; }
        public CoSo? CoSo { get; set; }
        public Guid? IdDepartment { get; set; }
        public Department? Department { get; set; }
        [MaxLength(20)]
        public String? MaTrinhDoChuyenMon { get; set; }
        public TrinhDoChuyenMon? TrinhDoChuyenMon { get; set; }
        public Guid? MaChucVu { get; set; }
        public ChucVu? ChucVu { get; set; }

        [MaxLength(100)]
        public string? SoDienThoai { get; set; }

        [MaxLength(200)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? MaNgachLuong { get; set; }
        public Guid? MaBacLuong { get; set; }
        public BacLuong BacLuong { get; set; }
        public decimal? HeSoLuong { get; set; }
        [DataType(DataType.Date)]
        public DateTime? NgayNangBacLuong { get; set; }
        public decimal? PhuCapChucVu { get; set; }
        public decimal? PhuCapVuotKhung { get; set; }
        public decimal? PhuCapKiemNhiem { get; set; }
        public decimal? PhuCapKhuVuc { get; set; }
        public int? LuongKhoan { get; set; }
        [DataType(DataType.Date)]
        public DateTime? KhoanTuNgay { get; set; }
        [DataType(DataType.Date)]
        public DateTime? KhoanDenNgay { get; set; }

        [MaxLength(100)]
        public string? SoBHXH { get; set; }
        [MaxLength(100)]
        public string? SoBHYT { get; set; }
        [MaxLength(100)]
        public string? MaSoThue { get; set; }
        [DataType(DataType.Date)]
        public DateTime? NgayVaoBienChe { get; set; }
        [DataType(DataType.Date)]
        public DateTime? NgayThamGiaCongTac { get; set; }

        [MaxLength(50)]
        public string? MaHeDaoTao { get; set; }
        public HeDaoTao? HeDaoTao { get; set; }
        [MaxLength(50)]
        public string? MaTrinhDoHocVan { get; set; }
        public TrinhDoHocVan TrinhDoHocVan { get; set; }

        [MaxLength(500)]
        public String? ChuyenNganh { get; set; }

        [MaxLength(50)]
        public string? MaTrinhDoTinHoc { get; set; }
        public TrinhDoTinHoc? TrinhDoTinHoc { get; set; }
        public Guid? MaTrinhDoNgoaiNgu { get; set; }
        public TrinhDoNgoaiNgu? TrinhDoNgoaiNgu { get; set; }

        public Guid? MaDiaBanHoatDong { get; set; }
        public DiaBanHoatDong? DiaBanHoatDong { get; set; }

        [MaxLength(50)]
        public string? MaTrinhDoChinhTri { get; set; }
        public TrinhDoChinhTri? TrinhDoChinhTri { get; set; }

        [MaxLength(50)]
        public string? MaHocHam { get; set; }
        public HocHam? HocHam { get; set; }

        [MaxLength(50)]
        public String? MaHocVi { get; set; }

        public HocVi? HocVi { get; set; }

        [MaxLength(50)]
        public string? MaDanToc { get; set; }
        public DanToc? DanToc { get; set; }

        [MaxLength(50)]
        public string? MaTonGiao { get; set; }
        public TonGiao? TonGiao { get; set; }

        [MaxLength(1000)]
        public String? NoiSinh { get; set; }

        [MaxLength(1000)]
        public String? ChoOHienNay { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgayvaoDangDuBi { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgayVaoDangChinhThuc { get; set; }

        [MaxLength(700)]
        public string? GhiChu { get; set; }

        public bool? IsCanBo { get; set; } = false;
        public bool? IsHoiVien { get; set; } = false;
        public bool? IsBanChapHanh { get; set; } = false;

        [MaxLength(500)]
        public string? HoKhauThuongTru { get; set; }

        [MaxLength(100)]
        public String? NgayVaoHoi { get; set; }

        [MaxLength(100)]
        public String? NgayThamGiaCapUyDang { get; set; }

        [MaxLength(100)]
        public String? NgayThamGiaHDND { get; set; }

        [MaxLength(10)]
        public String? VaiTro { get; set; }

        [MaxLength(200)]
        public String? VaiTroKhac { get; set; }

        [MaxLength(10)]
        public String? LoaiHoiVien { get; set; }

        [MaxLength(800)]
        public String? ThamGia_SH_DoanThe_HoiDoanKhac { get; set; }

        [MaxLength(800)]
        public String? ThamGia_CLB_DN_MH_HTX_THT { get; set; }

        [MaxLength(800)]
        public String? ThamGia_THNN_CHNN { get; set; }


        [MaxLength(500)]
        public String? GiaDinhThuocDienKhac { get; set; }


        public bool? KKAnToanThucPham { get; set; } = false;


        public bool? DKMauNguoiNongDanMoi { get; set; } = false;


        public bool? HoNgheo { get; set; } = false;
         
        public bool? CanNgheo { get; set; } = false;
        public bool? GiaDinhChinhSach { get; set; } = false;

        [MaxLength(10)]
        public String? MaNgheNghiep { get; set; }
        public NgheNghiep? NgheNghiep { get; set; }
        [MaxLength(500)]
        public String? Loai_DV_SX_ChN { get; set; }
        [MaxLength(500)]
        public String? DienTich_QuyMo { get; set; }

        public bool? HoiVienDuyet { get; set; }

        public Guid? NguoiDuyet { get; set; }
        public DateTime? NgayDuyet { get; set; }

        [MaxLength(500)]
        public string? HinhAnh { get; set; }
        public bool Actived { get; set; } = true;

        public DateTime?NgayNgungHoatDong { get; set; }
        public String? LyDoNgungHoatDong { get; set; }

        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public ICollection<QuanHeGiaDinh> QuanHeGiaDinhs { get; set; }
        public ICollection<QuaTrinhKhenThuong> QuaTrinhKhenThuongs { get; set; }
        public ICollection<QuaTrinhKyLuat> QuaTrinhKyLuats { get; set; }
        public ICollection<QuaTrinhDaoTao> QuaTrinhDaoTaos { get; set; }
        public ICollection<QuaTrinhBoiDuong> QuaTrinhBoiDuongs { get; set; }
        public ICollection<QuaTrinhBoNhiem> QuaTrinhBoNhiems { get; set; }
        public ICollection<FileDinhKem> FileDinhKems { get; set; }
        public ICollection<QuaTrinhCongTac> QuaTrinhCongTacs { get; set; }
        public ICollection<QuaTrinhMienNhiem> QuaTrinhMienNhiems { get; set; }
        public ICollection<HoiVienVayVon> HoiVienVayVons { get; set; }
        public ICollection<CanBoQuaTrinhLuong> CanBoQuaTrinhLuongs { get; set; }
        public ICollection<HoiVienHoiDap> HoiVienHoiDaps { get; set; }
        public HuuTri? HuuTri { get; set; }

        public bool? DangVien { get; set; }
        public CanBo() {
            QuanHeGiaDinhs = new List<QuanHeGiaDinh>();
            QuaTrinhKhenThuongs = new List<QuaTrinhKhenThuong>();
            QuaTrinhKyLuats = new List<QuaTrinhKyLuat>();
            QuaTrinhDaoTaos = new List<QuaTrinhDaoTao>();
            QuaTrinhBoiDuongs = new List<QuaTrinhBoiDuong>();
            QuaTrinhBoNhiems = new List<QuaTrinhBoNhiem>();
            QuaTrinhCongTacs = new List<QuaTrinhCongTac>();
            QuaTrinhMienNhiems = new List<QuaTrinhMienNhiem>();
            FileDinhKems = new List<FileDinhKem>();
            HoiVienVayVons = new List<HoiVienVayVon>();
            CanBoQuaTrinhLuongs = new List<CanBoQuaTrinhLuong>();
            HoiVienHoiDaps = new List<HoiVienHoiDap>();
        }

    }
}
