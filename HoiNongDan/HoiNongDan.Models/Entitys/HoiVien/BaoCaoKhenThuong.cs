using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models {
    public class BaoCaoKhenThuong
    {
        [Key]
        public Guid Id { get; set; }
        public String? TenDiaBanHoatDong { get; set; }
        public String? MaQuanHuyen { get; set; }
        public String? TenQuanHuyen { get; set; }
        public int HoiVienUuTu { get; set; }
        public int NDSXKDG_TW { get; set; }
        public int NDSXKDG_Tp { get; set; }
        public int NDSXKDG_H { get; set; }
        public int NDSXKDG_CS { get; set; }
        public int NDVietNamXuatSac { get; set; }
        public int KNCViGCND { get; set; }
        public int CanBoHoiCSG { get; set; }
        public int SangTaoNhaNong { get; set; }
        public int GuongDiemHinhTienTien { get; set; }
        public int GuongDanVangKheo { get; set; }
        public int GuongHocTapLamTheoLoiBac { get; set; }
        public int Tong { get; set; }
    }
    public class DashboardHoiVien {
        [Key]
        public Guid ID { get; set; }

        public int Tong { get; set; }
        public int TongNN { get; set; }
        public int TongDC { get; set; }
        public int HoiVien { get; set; }
        public int HoiVienNN { get; set; }
        public int HoiVienDC { get; set; }
        public int PhatTrien { get; set; }
        public int PhatTrienNN { get; set; }
        public int PhatTrienDC { get; set; }
        public int Giam { get; set; }
        public int GiamNN { get; set; }
        public int GiamDC { get; set; }
        public int ChiHoi { get; set; }
        public int ChiHoiNN { get; set; }
        public int ChiHoiDC { get; set; }

        public int ToHoi { get; set; }
        public int ToHoiNN { get; set; }
        public int ToHoiDC { get; set; }

    }
}
