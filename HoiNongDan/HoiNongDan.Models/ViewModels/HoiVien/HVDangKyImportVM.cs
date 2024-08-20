using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.ViewModels.HoiVien
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
        //public String? NgayThangVaoHoi { get; set; }
        public String? MaNgheNghiep { get; set; }
        public String? DiaBanDanCu { get; set; }
        public String? NganhNghe { get; set; }
        //public String? SoThe { get; set; }
        //public String? NgayCapThe { get; set; }
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
        public String? SoQuyetDinh;
        public String? NgayThangVaoHoi { get; set; }
        public String? SoThe { get; set; }
        public String? NgayCapThe { get; set; }
    }
}
