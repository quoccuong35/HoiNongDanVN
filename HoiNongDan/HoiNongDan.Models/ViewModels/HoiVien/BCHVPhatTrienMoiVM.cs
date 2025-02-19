using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class BCHVPhatTrienMoiVM
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
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? NgayThangVaoHoi { get; set; }
        public String? NgheNghiep { get; set; }
        public String? DiaBanDanCu { get; set; }
        public String? NganhNghe { get; set; }
        public String? SoThe { get; set; }
        public String? NgayCapThe { get; set; }
        
    }
    public class BCHVPhatTrienMoiExcelVM
    {
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

        public String? NgayThangVaoHoi { get; set; }
        public String? NgheNghiep { get; set; }
        public String? DiaBanDanCu { get; set; }
        public String? NganhNghe { get; set; }
        public String? SoThe { get; set; }
        public String? NgayCapThe { get; set; }
    }
    public class BCHVPhatTrienMoiSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public String? MaChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TuNgay")]
        [DataType(DataType.Date)]
        public DateTime? TuNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DenNgay")]
        [DataType(DataType.Date)]
        public DateTime? DenNgay { get; set; }
    }
}
