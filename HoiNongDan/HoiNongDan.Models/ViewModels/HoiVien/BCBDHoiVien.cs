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
        public string Ma { get; set; }
        public string Ten { get; set; }
        public int TongSL { get; set; }
        public int TongNu { get; set; }

        public int DoTuoi40 { get; set; }
        public int DoTuoi60 { get; set; }
        public int DoTuoiTren60 { get; set; }
        public int TongDanToc { get; set; }
        public int ChiHoiDanCu { get; set; }
        public int ChiHoiNganhNghe { get; set; }
        public int DangVien { get; set; }
        public int UuTu { get; set; }
        public int HVNongCot { get; set; }
    }
    public class BCSLMau1Excel
    {
        public int Stt { get; set; }
        public string Ten { get; set; }
        public int TongSL { get; set; }
        public int TongNu { get; set; }

        public int DoTuoi40 { get; set; }
        public int DoTuoi60 { get; set; }
        public int DoTuoiTren60 { get; set; }
        public int TongDanToc { get; set; }
        public int ChiHoiDanCu { get; set; }
        public int ChiHoiNganhNghe { get; set; }
        public int DangVien { get; set; }
        public int UuTu { get; set; }
        public int HVNongCot { get; set; }
    }
}
