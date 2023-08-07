using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HoiNongDan.Models
{
    public class HoiVienVM
    {
        public Guid? IDCanBo { get; set; }

        [MaxLength(20)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string MaCanBo { get; set; }

        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string HoVaTen { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        public GioiTinh GioiTinh { get; set; }

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
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string SoCCCD { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayCapCCCD")]
        public DateTime? NgayCapCCCD { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoKhauThuongTru")]
        public string? HoKhauThuongTru { get; set; }

        [MaxLength(1000)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ChoOHienNay { get; set; }

        [MaxLength(20)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoDienThoai")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string SoDienThoai { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        public DateTime? NgayvaoDangDuBi { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public DateTime? NgayVaoDangChinhThuc { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public string MaDanToc { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string MaTonGiao { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoHocVan")]
        public string MaTrinhDoHocVan { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public string? MaTrinhDoChinhTri { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaCongTac")]
        public DateTime? NgayThamGiaCongTac { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaCapUyDang")]
        public DateTime? NgayThamGiaCapUyDang { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaHDND")]
        public DateTime? NgayThamGiaHDND { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VaiTro")]
        public string? VaiTro { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GiaDinhThuocDien")]
        public string? MaGiaDinhThuocDien { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgheNghiepHienNay")]
        public string? MaNgheNghiep { get; set; }

        [MaxLength(2000)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }
        [MaxLength(500)]
        public string? HinhAnh { get; set; }
    }
    public class HoiVienMTVM : HoiVienVM
    {
        public CanBo GetHoiVien(CanBo obj)
        {
            obj.MaCanBo = MaCanBo;
            obj.HoVaTen = HoVaTen;
            obj.NgaySinh = NgaySinh!.Value;
            obj.GioiTinh = GioiTinh;
            obj.MaTinhTrang = null;
            obj.IdCoSo = IdCoSo;
            obj.IdDepartment = IdDepartment;
            obj.MaChucVu = MaChucVu;
            obj.SoCCCD = SoCCCD;
            obj.NgayCapCCCD = NgayCapCCCD!.Value;
            obj.HoKhauThuongTru = HoKhauThuongTru;
            obj.ChoOHienNay = ChoOHienNay;
            obj.SoDienThoai = SoDienThoai;
            obj.NgayvaoDangDuBi = NgayvaoDangDuBi;
            obj.NgayVaoDangChinhThuc = NgayVaoDangChinhThuc;
            obj.MaDanToc = MaDanToc;
            obj.MaTonGiao = MaTonGiao;
            obj.MaTrinhDoHocVan = MaTrinhDoHocVan;
            obj.MaTrinhDoChinhTri = MaTrinhDoChinhTri;
            obj.NgayThamGiaCongTac = NgayThamGiaCongTac;
            obj.NgayThamGiaCapUyDang = NgayThamGiaCapUyDang;
            obj.NgayThamGiaHDND = NgayThamGiaHDND;
            obj.VaiTro = VaiTro;
            obj.MaGiaDinhThuocDien = MaGiaDinhThuocDien;
            obj.MaNgheNghiep = MaNgheNghiep;
            obj.IsHoiVien = true;
           
            return obj;
        }
        public static HoiVienVM SetHoiVien(CanBo item) {
            HoiVienVM obj = new HoiVienVM();
            obj.MaCanBo = item.MaCanBo;
            obj.HoVaTen = item.HoVaTen;
            obj.NgaySinh = item.NgaySinh;
            obj.GioiTinh = item.GioiTinh;
            obj.IdCoSo = item.IdCoSo;
            obj.IdDepartment = item.IdDepartment;
            obj.MaChucVu = item.MaChucVu;
            obj.SoCCCD = item.SoCCCD;
            obj.NgayCapCCCD = item.NgayCapCCCD;
            obj.HoKhauThuongTru = item.HoKhauThuongTru;
            obj.ChoOHienNay = item.ChoOHienNay!;
            obj.SoDienThoai = item.SoDienThoai;
            obj.NgayvaoDangDuBi = item.NgayvaoDangDuBi;
            obj.NgayVaoDangChinhThuc = item.NgayVaoDangChinhThuc;
            obj.MaDanToc = item.MaDanToc;
            obj.MaTonGiao = item.MaTonGiao;
            obj.MaTrinhDoHocVan = item.MaTrinhDoHocVan;
            obj.MaTrinhDoChinhTri = item.MaTrinhDoChinhTri;
            obj.NgayThamGiaCongTac = item.NgayThamGiaCongTac;
            obj.NgayThamGiaCapUyDang = item.NgayThamGiaCapUyDang;
            obj.NgayThamGiaHDND = item.NgayThamGiaHDND;
            obj.VaiTro = item.VaiTro;
            obj.MaGiaDinhThuocDien = item.MaGiaDinhThuocDien;
            obj.MaNgheNghiep = item.MaNgheNghiep;
            obj.IDCanBo = item.IDCanBo;
            obj.HinhAnh = item.HinhAnh;
            return obj;
        }
    }
    public class HoiVienDetailVM : HoiVienVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhTrang")]
        public string TenTinhTrang { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhanHe")]
        public string TenPhanHe { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSo")]
        public string TenCoSo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DepartmentName")]
        public string TenDonVi { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public string TenChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VaiTro")]
        public string VaiTro { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GiaDinhThuocDien")]
        public string GiaDinhThuocDien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgheNghiepHienNay")]
        public string NgheNghiepHienNay { get; set; }

        public string HinhAnh { get; set; } = @"\images\login.png";
    }
}
