using Portal.Models.Entitys;
using Portal.Models.Entitys.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
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
        [MaxLength(500)]
        public string HoVaTen { get; set; }

        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        public GioiTinh GioiTinh { get; set; }

        [MaxLength(20)]
        public String SoCCCD { get; set; }
        [DataType(DataType.Date)]
        public DateTime NgayCapCCCD { get; set; }
        [MaxLength(50)]
        public string MaTinhTrang { get; set; }
        public TinhTrang TinhTrang { get; set; }

        [MaxLength(50)]
        public string MaPhanHe { get; set; }
        public PhanHe PhanHe { get; set; }
        public Guid IdCoSo { get; set; }
        public CoSo CoSo { get; set; }
        public Guid IdDepartment { get; set; }
        public Department Department { get; set; }
        public Guid MaChucVu { get; set; }
        public ChucVu ChucVu { get; set; }

        [MaxLength(20)]
        public string SoDienThoai { get; set; }

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
        public string SoBHXH { get; set; }
        [MaxLength(100)]
        public string SoBHYT { get; set; }
        [MaxLength(100)]
        public string MaSoThue { get; set; }
        [DataType(DataType.Date)]
        public DateTime? NgayVaoBienChe { get; set; }
        [DataType(DataType.Date)]
        public DateTime? NgayThamGiaCongTac { get; set; }

        [MaxLength(50)]
        public string MaHeDaoTao { get; set; }
        [MaxLength(50)]
        public string MaTrinhDoHocVan { get; set; }
        public TrinhDoHocVan TrinhDoHocVan { get; set; }

        [MaxLength(500)]
        public string ChuyenNganh { get; set; }

        [MaxLength(50)]
        public string? MaTrinhDoTinHoc { get; set; }
        public TrinhDoTinHoc? TrinhDoTinHoc { get; set; }
        public Guid? MaTrinhDoNgoaiNgu { get; set; }
        public TrinhDoNgoaiNgu? TrinhDoNgoaiNgu { get; set; }

        [MaxLength(50)]
        public string? MaTrinhDoChinhTri { get; set; }
        public TrinhDoChinhTri? TrinhDoChinhTri { get; set; }

        [MaxLength(50)]
        public string? MaHocHam { get; set; }

        [MaxLength(50)]
        public string MaDanToc { get; set; }
        public DanToc DanToc { get; set; }

        [MaxLength(50)]
        public string MaTonGiao { get; set; }
        public TonGiao TonGiao { get; set; }

        [MaxLength(1000)]
        public string NoiSinh { get; set; }

        [MaxLength(1000)]
        public string ChoOHienNay { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgayvaoDangDuBi { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgayVaoDangChinhThuc { get; set; }

        [MaxLength(2000)]
        public string? GhiChu { get; set; }

        [MaxLength(500)]
        public string? HinhAnh { get; set; }
        public bool Actived { get; set; } = true;
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }

    }
}
