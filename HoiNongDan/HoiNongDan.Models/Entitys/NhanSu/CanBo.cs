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
        [MaxLength(100)]
        public string? MaCanBo { get; set; }

        [MaxLength(100)]
        public string? MaDinhDanh { get; set; }
        [MaxLength(500)]
        public string HoVaTen { get; set; }

        // [DataType(DataType.Date)]
        public String? NgaySinh { get; set; }

        public GioiTinh GioiTinh { get; set; }

        [MaxLength(200)]
        public String? SoCCCD { get; set; }

        [MaxLength(100)]
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
        [MaxLength(500)]
        public String? DonVi { get; set; }

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

        public String? NgayThamGiaCongTac { get; set; }

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

        //[DataType(DataType.Date)]
        [MaxLength(50)]
        public String? NgayvaoDangDuBi { get; set; }

        //[DataType(DataType.Date)]
        [MaxLength(50)]
        public String? NgayVaoDangChinhThuc { get; set; }

        [MaxLength(700)]
        public string? GhiChu { get; set; }

        public bool? IsCanBo { get; set; } = false;
        public bool? IsHoiVien { get; set; } = false;
        public bool? IsBanChapHanh { get; set; } = false;
        public bool? isRoiHoi { get; set; } = false;

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

        [MaxLength(200)]
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

        public DateTime?NgayRoiHoi { get; set; }
        public String? LyDoRoiHoi { get; set; }

        public String? SoQuyetDinhBoNhiem { get; set; }
        public bool? ThamGiaBTV { get; set; } = false;
        public bool? HuyenUyVien { get; set; } = false;
        public bool? DangUyVien { get; set; } = false;
        public bool? HDNNCapHuyen { get; set; } = false;
        public bool? HDNNCapXa { get; set; } = false;
        public String? DanhGiaCBCC { get; set; }
        public String? DanhGiaDangVien { get; set; }
        public bool? UBKT { get; set; } = false;
        public String? Level { get; set; }

        public bool? HoiVienNongCot { get; set; }

        [MaxLength(200)]
        public String? HoiVienUuTuNam { get; set; }

        public bool? HoiVienDanhDu { get; set; }
        [MaxLength(1000)]
        public String? HoTrovayVon { get; set; }

        [MaxLength(1000)]
        public String? HoTroDaoTaoNghe { get; set; }

        [MaxLength(1000)]
        public String? HoTroKhac { get; set; }


        [MaxLength(1000)]
        public String? SoLuong { get; set; }


        [MaxLength(1000)]
        public String? ChiHoiDanCu_CHT { get; set; }

        [MaxLength(1000)]
        public String? ChiHoiDanCu_CHP { get; set; }

        [MaxLength(1000)]
        public String? ChiHoiNgheNghiep_CHT { get; set; }


        [MaxLength(1000)]
        public String? ChiHoiNgheNghiep_CHP { get; set; }

        public bool? HoiVienDanCu { get; set; }
        public bool? HoiVienNganhNghe { get; set; }
        public DateTime? NgayCapThe { get; set; }

        public DateTime? NgayDangKy { get; set; }
        public Guid? AccountIdDangKy { get; set; }

        public bool? TuChoi { get; set; }

        [MaxLength(1000)]
        public String? LyDoTuChoi { get; set; }

        public DateTime? NgayTuChoi { get; set; }
        public Guid? AccountIdTuChoi { get; set; }

        public Guid? MaChiHoi { get; set; }
        public ChiHoi? ChiHoi { get; set; }

        public Guid? MaToHoi { get; set; }
        public ToHoi? ToHoi { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public ICollection<QuanHeGiaDinh> QuanHeGiaDinhs { get; set; }
        public ICollection<QuanHeGiaDinh> HVQuanHeGiaDinhs { get; set; }
        public ICollection<QuaTrinhKhenThuong> QuaTrinhKhenThuongs { get; set; }
        public ICollection<QuaTrinhKyLuat> QuaTrinhKyLuats { get; set; }
        public ICollection<QuaTrinhBoNhiem> QuaTrinhBoNhiems { get; set; }
        public ICollection<FileDinhKem> FileDinhKems { get; set; }
        public ICollection<QuaTrinhCongTac> QuaTrinhCongTacs { get; set; }
        public ICollection<QuaTrinhMienNhiem> QuaTrinhMienNhiems { get; set; }
        public ICollection<HoiVienHoTro> HoiVienHoTros { get; set; }
        public ICollection<CanBoQuaTrinhLuong> CanBoQuaTrinhLuongs { get; set; }
        public ICollection<HoiVienHoiDap> HoiVienHoiDaps { get; set; }
        public ICollection<DaoTaoBoiDuong> DaoTaoBoiDuongs { get; set; }
        public ICollection<DoanTheChinhTri_HoiDoan_HoiVien> DoanTheChinhTri_HoiDoan_HoiViens { get; set; }
        public ICollection<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien> CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens { get; set; }
        public ICollection<ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien> ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens { get; set; }
        public ICollection<HoiVienCapThe> HoiVienCapThes { get; set; }
        public ICollection<VayVon> VayVons { get; set; }
        public HuuTri? HuuTri { get; set; }

        public bool? DangVien { get; set; }
        public virtual ICollection<PhatTrienDang_HoiVien> PhatTrienDang_HoiViens { get; set; }
        public CanBo() {
            QuanHeGiaDinhs = new List<QuanHeGiaDinh>();
            HVQuanHeGiaDinhs = new List<QuanHeGiaDinh>();
            QuaTrinhKhenThuongs = new List<QuaTrinhKhenThuong>();
            QuaTrinhKyLuats = new List<QuaTrinhKyLuat>();
            DaoTaoBoiDuongs = new List<DaoTaoBoiDuong>();
            QuaTrinhBoNhiems = new List<QuaTrinhBoNhiem>();
            QuaTrinhCongTacs = new List<QuaTrinhCongTac>();
            QuaTrinhMienNhiems = new List<QuaTrinhMienNhiem>();
            FileDinhKems = new List<FileDinhKem>();
            HoiVienHoTros = new List<HoiVienHoTro>();
            CanBoQuaTrinhLuongs = new List<CanBoQuaTrinhLuong>();
            HoiVienHoiDaps = new List<HoiVienHoiDap>();
            DoanTheChinhTri_HoiDoan_HoiViens = new List<DoanTheChinhTri_HoiDoan_HoiVien>();
            CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens = new List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien>();
            ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens = new List<ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien>();
            PhatTrienDang_HoiViens = new List<PhatTrienDang_HoiVien>();
            HoiVienCapThes = new  List<HoiVienCapThe>();
            VayVons = new List<VayVon>();
        }

    }
}
