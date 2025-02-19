using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class BCBDHoiVien
    {
        public string Ma { get; set; }
        public string Ten { get; set; }
        public int SL { get; set; }
        public int ThemMoi { get; set; }
        public int Giam { get; set; }
        public int ChoDuyet { get; set; }
    }
    public class BCBDHoiVienExcel
    {
        public int STT { get; set; }
        public string Ten { get; set; }
        public int SL { get; set; }
        public int ThemMoi { get; set; }
        public int Giam { get; set; }
        public int ChoDuyet { get; set; }
    }
    public class BCSLMau1
    {
        public int Stt { get; set; }
        public string Ten { get; set; }
        public int TongSL { get; set; }
        public int TongNu { get; set; }
        public int ChiHoiDanCu { get; set; }
        public int ChiHoiNganhNghe { get; set; }
        public int TongDanToc { get; set; }
        public int TongTonGiao { get; set; }
        public int DangVien { get; set; }
        public int ThamGiaCapUyDang { get; set; }
        public int ThamGiaHDNN { get; set; }
        public int DanhDu { get; set; }
        public int HVNongCot { get; set; }

        public int DoTuoi40 { get; set; }
        public int DoTuoi60 { get; set; }
        public int DoTuoiTren60 { get; set; }

        public int Cap1 { get; set; }
        public int Cap2 { get; set; }
        public int Cap3 { get; set; }

        public int ChinhTri_SC { get; set; }
        public int ChinhTri_TC { get; set; }
        public int ChinhTri_CC { get; set; }
        public int ChinhTri_CN { get; set; }

        public int HoNgheo { get; set; }
        public int CanNgheo { get; set; }
        public int GiaDinhChinhSach { get; set; }
        public int ThanhPhanKhac { get; set; }

        public int NongDan { get; set; }
        public int CongNhan { get; set; }
        public int CongChuc_VienChuc { get; set; }
        public int DoanhNghiep { get; set; }
        public int HocSinh_SinhVien { get; set; }
        public int LaoDongTuDo { get; set; }
        public int HuuTri { get; set; }
    }
}
