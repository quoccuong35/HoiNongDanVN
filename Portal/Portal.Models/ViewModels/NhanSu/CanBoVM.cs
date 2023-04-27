using Portal.Models.Entitys.MasterData;
using Portal.Models.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class CanBoVM
    {
        public Guid? IDCanBo { get; set; }
       
        [MaxLength(20)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string MaCanBo { get; set; }
        
        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string HoVaTen { get; set; }

        
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        public GioiTinh GioiTinh { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public String SoCCCD { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayCapCCCD")]
        public DateTime NgayCapCCCD { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhTrang")]
        public string MaTinhTrang { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhanHe")]

        [MaxLength(50)]
        public string MaPhanHe { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        public Guid IdCoSo { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public Guid IdDepartment { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public Guid MaChucVu { get; set; }

        [MaxLength(20)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoDienThoai")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string SoDienThoai { get; set; }

        [MaxLength(200)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Email")]
        public string? Email { get; set; }

        [MaxLength(50)]
       
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgachLuong")]
        public string? MaNgachLuong { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BacLuong")]
        public Guid? MaBacLuong { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeSo")]
        public decimal? HeSoLuong { get; set; }
        [DataType(DataType.Date)]

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayNangBacLuong")]
        public DateTime? NgayNangBacLuong { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuCapChucVu")]
        public decimal? PhuCapChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuCapVuotKhung")]
        public decimal? PhuCapVuotKhung { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuCapKiemNhiem")]
        public decimal? PhuCapKiemNhiem { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuCapKhuVuc")]
        public decimal? PhuCapKhuVuc { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LuongKhoan")]
        public int? LuongKhoan { get; set; }
        
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "KhoanTuNgay")]
        public DateTime? KhoanTuNgay { get; set; }
       
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "KhoanDenNgay")]
        public DateTime? KhoanDenNgay { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [MaxLength(100)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoBHXH")]
        public string SoBHXH { get; set; }
       
        [MaxLength(100)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoBHYT")]
        public string SoBHYT { get; set; }
        
        [MaxLength(100)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaSoThue")]
        public string MaSoThue { get; set; }
        [DataType(DataType.Date)]

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoBienChe")]
        public DateTime? NgayVaoBienChe { get; set; }
        [DataType(DataType.Date)]

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaCongTac")]
        public DateTime? NgayThamGiaCongTac { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HeDaoTao")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        public string MaHeDaoTao { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoHocVan")]
        public string MaTrinhDoHocVan { get; set; }

        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChuyenNganh")]
        public string ChuyenNganh { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoTinHoc")]
        public string? MaTrinhDoTinHoc { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoNgoaiNgu")]
        public Guid? MaTrinhDoNgoaiNgu { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public string? MaTrinhDoChinhTri { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HocHam")]
        public string? MaHocHam { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public string MaDanToc { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string MaTonGiao { get; set; }
        [MaxLength(1000)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiSinh")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string NoiSinh { get; set; }

        [MaxLength(1000)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ChoOHienNay { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        public DateTime? NgayvaoDangDuBi { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public DateTime? NgayVaoDangChinhThuc { get; set; }

        [MaxLength(2000)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }

        [MaxLength(500)]
        public string? HinhAnh { get; set; }

    }
    public class CanBoVMMT : CanBoVM {
        public CanBo AddCanBo()
        {
            CanBo add = new CanBo();
            add.IDCanBo = Guid.NewGuid();
            add.MaCanBo = this.MaCanBo;
            add.HoVaTen = this.HoVaTen;
            add.NgaySinh = this.NgaySinh;
            add.GioiTinh = this.GioiTinh;
            add.SoCCCD = this.SoCCCD;
            add.NgayCapCCCD = this.NgayCapCCCD;
            add.MaTinhTrang = this.MaTinhTrang;
            add.IdCoSo = this.IdCoSo;
            add.IdDepartment = this.IdDepartment;
            add.MaChucVu = this.MaChucVu;
            add.SoDienThoai = this.SoDienThoai;
            add.Email = this.Email;
            add.MaNgachLuong = this.MaNgachLuong;
            add.MaBacLuong = this.MaBacLuong;
            add.HeSoLuong = this.HeSoLuong;
            add.NgayNangBacLuong = this.NgayNangBacLuong;
            add.PhuCapChucVu = this.PhuCapChucVu;
            add.PhuCapVuotKhung = this.PhuCapVuotKhung;
            add.PhuCapKiemNhiem = this.PhuCapKiemNhiem;
            add.PhuCapKhuVuc = this.PhuCapKhuVuc;
            add.LuongKhoan = this.LuongKhoan;
            add.NgayVaoBienChe = this.NgayVaoBienChe;
            add.NgayThamGiaCongTac = this.NgayThamGiaCongTac;
            add.MaHeDaoTao = this.MaHeDaoTao;
            add.MaTrinhDoHocVan = this.MaTrinhDoHocVan;
            add.ChuyenNganh = this.ChuyenNganh;
            add.MaTrinhDoTinHoc = this.MaTrinhDoTinHoc;
            add.MaTrinhDoNgoaiNgu = this.MaTrinhDoNgoaiNgu;
            add.MaTrinhDoChinhTri = this.MaTrinhDoChinhTri;
            add.MaHocHam = this.MaHocHam;
            add.MaDanToc = this.MaDanToc;
            add.MaTonGiao = this.MaTonGiao;
            add.NoiSinh = this.NoiSinh;
            add.NgayvaoDangDuBi = this.NgayvaoDangDuBi;
            add.NgayVaoDangChinhThuc = this.NgayVaoDangChinhThuc;
            add.HinhAnh = this.HinhAnh;
            add.KhoanTuNgay = this.KhoanTuNgay;
            add.KhoanDenNgay = this.KhoanDenNgay;
            add.MaSoThue = this.MaSoThue;
            add.SoBHXH = this.SoBHXH;
            add.SoBHYT = this.SoBHYT;
            add.GhiChu = this.GhiChu;
            add.ChoOHienNay = this.ChoOHienNay;
            add.MaPhanHe = this.MaPhanHe;
            return add;
        }
        public static CanBoVMMT EditCanBo(CanBo item)
        {
            CanBoVMMT edit = new CanBoVMMT();
            edit.IDCanBo = item.IDCanBo;
            edit.MaCanBo = item.MaCanBo;
            edit.HoVaTen = item.HoVaTen;
            edit.NgaySinh = item.NgaySinh;
            edit.GioiTinh = item.GioiTinh;
            edit.SoCCCD = item.SoCCCD;
            edit.NgayCapCCCD = item.NgayCapCCCD;
            edit.MaTinhTrang = item.MaTinhTrang;
            edit.IdCoSo = item.IdCoSo;
            edit.IdDepartment = item.IdDepartment;
            edit.MaChucVu = item.MaChucVu;
            edit.SoDienThoai = item.SoDienThoai;
            edit.Email = item.Email;
            edit.MaNgachLuong = item.MaNgachLuong;
            edit.MaBacLuong = item.MaBacLuong;
            edit.HeSoLuong = item.HeSoLuong;
            edit.NgayNangBacLuong = item.NgayNangBacLuong;
            edit.PhuCapChucVu = item.PhuCapChucVu;
            edit.PhuCapVuotKhung = item.PhuCapVuotKhung;
            edit.PhuCapKiemNhiem = item.PhuCapKiemNhiem;
            edit.PhuCapKhuVuc = item.PhuCapKhuVuc;
            edit.LuongKhoan = item.LuongKhoan;
            edit.NgayVaoBienChe = item.NgayVaoBienChe;
            edit.NgayThamGiaCongTac = item.NgayThamGiaCongTac;
            edit.MaHeDaoTao = item.MaHeDaoTao;
            edit.MaTrinhDoHocVan = item.MaTrinhDoHocVan;
            edit.ChuyenNganh = item.ChuyenNganh;
            edit.MaTrinhDoTinHoc = item.MaTrinhDoTinHoc;
            edit.MaTrinhDoNgoaiNgu = item.MaTrinhDoNgoaiNgu;
            edit.MaTrinhDoChinhTri = item.MaTrinhDoChinhTri;
            edit.MaHocHam = item.MaHocHam;
            edit.MaDanToc = item.MaDanToc;
            edit.MaTonGiao = item.MaTonGiao;
            edit.NoiSinh = item.NoiSinh;
            edit.NgayvaoDangDuBi = item.NgayvaoDangDuBi;
            edit.NgayVaoDangChinhThuc = item.NgayVaoDangChinhThuc;
            edit.HinhAnh = item.HinhAnh;
            edit.KhoanTuNgay = item.KhoanTuNgay;
            edit.KhoanDenNgay = item.KhoanDenNgay;
            edit.MaSoThue = item.MaSoThue;
            edit.SoBHXH = item.SoBHXH;
            edit.SoBHYT = item.SoBHYT;
            edit.GhiChu = item.GhiChu;
            edit.ChoOHienNay = item.ChoOHienNay;
            edit.MaPhanHe = item.MaPhanHe;
            edit.HinhAnh = item.HinhAnh;
            return edit;
        }

        public  CanBo EditUpdate(CanBo item) {

            item.MaCanBo = this.MaCanBo;
            item.HoVaTen = this.HoVaTen;
            item.NgaySinh = this.NgaySinh;
            item.GioiTinh = this.GioiTinh;
            item.SoCCCD = this.SoCCCD;
            item.NgayCapCCCD = this.NgayCapCCCD;
            item.MaTinhTrang = this.MaTinhTrang;
            item.IdCoSo = this.IdCoSo;
            item.IdDepartment = this.IdDepartment;
            item.MaChucVu = this.MaChucVu;
            item.SoDienThoai = this.SoDienThoai;
            item.Email = this.Email;
            item.MaNgachLuong = this.MaNgachLuong;
            item.MaBacLuong = this.MaBacLuong;
            item.HeSoLuong = this.HeSoLuong;
            item.NgayNangBacLuong = this.NgayNangBacLuong;
            item.PhuCapChucVu = this.PhuCapChucVu;
            item.PhuCapVuotKhung = this.PhuCapVuotKhung;
            item.PhuCapKiemNhiem = this.PhuCapKiemNhiem;
            item.PhuCapKhuVuc = this.PhuCapKhuVuc;
            item.LuongKhoan = this.LuongKhoan;
            item.NgayVaoBienChe = this.NgayVaoBienChe;
            item.NgayThamGiaCongTac = this.NgayThamGiaCongTac;
            item.MaHeDaoTao = this.MaHeDaoTao;
            item.MaTrinhDoHocVan = this.MaTrinhDoHocVan;
            item.ChuyenNganh = this.ChuyenNganh;
            item.MaTrinhDoTinHoc = this.MaTrinhDoTinHoc;
            item.MaTrinhDoNgoaiNgu = this.MaTrinhDoNgoaiNgu;
            item.MaTrinhDoChinhTri = this.MaTrinhDoChinhTri;
            item.MaHocHam = this.MaHocHam;
            item.MaDanToc = this.MaDanToc;
            item.MaTonGiao = this.MaTonGiao;
            item.NoiSinh = this.NoiSinh;
            item.NgayvaoDangDuBi = this.NgayvaoDangDuBi;
            item.NgayVaoDangChinhThuc = this.NgayVaoDangChinhThuc;
            //item.HinhAnh = this.HinhAnh;
            item.KhoanTuNgay = this.KhoanTuNgay;
            item.KhoanDenNgay = this.KhoanDenNgay;
            item.MaSoThue = this.MaSoThue;
            item.SoBHXH = this.SoBHXH;
            item.SoBHYT = this.SoBHYT;
            item.GhiChu = this.GhiChu;
            item.ChoOHienNay = this.ChoOHienNay;
            item.MaPhanHe = this.MaPhanHe;
            return item;
        }
    }
   
}
