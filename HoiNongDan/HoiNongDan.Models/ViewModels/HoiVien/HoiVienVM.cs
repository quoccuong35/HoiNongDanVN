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
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaDinhDanh")]
        public string? MaDinhDanh { get; set; }

        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string HoVaTen { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]

        public String? NgaySinh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        public GioiTinh GioiTinh { get; set; }


        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public Guid? MaChucVu { get; set; }

        [MaxLength(200)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }


        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayCapCCCD")]
        public String? NgayCapCCCD { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoKhauThuongTru")]
        public string? HoKhauThuongTru { get; set; }

        [MaxLength(1000)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ChoOHienNay { get; set; }

        [MaxLength(100)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoDienThoai")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? SoDienThoai { get; set; }

        //[DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        public String? NgayvaoDangDuBi { get; set; }

        //[DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public String? NgayVaoDangChinhThuc { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public string MaDanToc { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string? MaTonGiao { get; set; }


        [MaxLength(20)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChuyenMon")]
        public string? MaTrinhDoChuyenMon { get; set; }

        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChuyenNganh")]
        public string? ChuyenNganh { get; set; }
        [MaxLength(50)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoHocVan")]
        public string? MaTrinhDoHocVan { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HocVi")]
        public String? MaHocVi { get; set; }


        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public string? MaTrinhDoChinhTri { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoHoi")]
        public String? NgayVaoHoi { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaCapUyDang")]
        public String? NgayThamGiaCapUyDang { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaHDND")]
        public String? NgayThamGiaHDND { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VaiTro")]
        public string? VaiTro { get; set; }

        public string? VaiTroKhac { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LoaiHoiVien")]
        public string? LoaiHoiVien { get; set; }

        //// [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        // [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GiaDinhThuocDien")]
        // public string? MaGiaDinhThuocDien { get; set; }

        [Display(Name = "Hộ nghèo")]
        public bool HoNgheo { get; set; } = false;

        [Display(Name = "Cận nghèo")] 
        public bool CanNgheo { get; set; } = false;

        [Display(Name = "Gia đình chính sách, con liệt sĩ")]
        public bool GiaDinhChinhSach { get; set; } = false;

        [MaxLength(500)]
        public string? GiaDinhThuocDienKhac { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgheNghiepHienNay")]
        public string? MaNgheNghiep { get; set; }

        [MaxLength(800)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ThamGia_SH_DoanThe_HoiDoanKhac")]
        public String? ThamGia_SH_DoanThe_HoiDoanKhac { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ThamGia_CLB_DN_MH_HTX_THT")]
        [MaxLength(800)]
        public String? ThamGia_CLB_DN_MH_HTX_THT { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ThamGia_THNN_CHNN")]
        [MaxLength(800)]
        public String? ThamGia_THNN_CHNN { get; set; }

        [MaxLength(2000)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }
       
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoatDong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Loai_DV_SX_ChN")]
        public String? Loai_DV_SX_ChN { get; set; }

        [Display(Name = "Diện tích")]
        public String? DienTich_QuyMo { get; set; }

        [Display(Name = "Số lượng")]
        public String? SoLuong { get; set; }


        [Display(Name = "Ngày rời hội")]
        [DataType(DataType.Date)]
        public DateTime? NgayRoiHoi { get; set; }

        [Display(Name = "Lý do")]
        public String? LyDoRoiHoi { get; set; }

        public bool? Actived { get; set; }

        [MaxLength(500)]
        public string? HinhAnh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "KKAnToanThucPham")]
        public bool? KKAnToanThucPham { get; set; } = false;

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DKMauNguoiNongDanMoi")]
        public bool? DKMauNguoiNongDanMoi { get; set; } = false;
        [Display(Name = ("Đảng viên"))]
        public bool DangVien { get; set; } = false;
        [Display(Name =("Hội viên nòng cốt"))]
        public bool HoiVienNongCot { get; set; } = false;
        [Display(Name = ("Hội viên danh dự"))]
        public bool HoiVienDanhDu { get; set; } = false;

        [Display(Name = ("Hội viên Ưu tú năm"))]
        public String? HoiVienUuTuNam { get; set; }

        [Display(Name = ("Chi hội"))]
        public Guid? MaChiHoi { get; set; }

        [Display(Name = ("Tổ hội"))]
        public Guid? MaToHoi { get; set; }

        [Display(Name = "Hỗ trợ Vay vốn (nguồn vốn)")]
        public string? HoTrovayVon { get; set; }

        [Display(Name = "Hỗ trợ hình thức khác")]
        public string? HoTroKhac { get; set; }


        [Display(Name = "Hỗ trợ đào tạo nghề")]
        public string? HoTroDaoTaoNghe { get; set; }

        [Display(Name = "Đoàn thể chính trị-Hội đoàn khác")]
        public List<Guid>? MaDoanTheChinhTri_HoiDoan { get; set; }
        [Display(Name = "Tham gia câu lạc bộ, đội nhóm, mô hình, hợp tác xã, tổ hợp tác")]
        public List<Guid>? Id_CLB_DN_MH_HTX_THT { get; set; }

        [Display(Name = "Tham gia tổ hội ngành nghề, chi hội ngành nghề")]
        public List<Guid>? Ma_ToHoiNganhNghe_ChiHoiNganhNghe { get; set; }
        public List<FileDinhKem>? FileDinhKems { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoiVienDanCu")]
        public bool HoiVienDanCu { get; set; } = false;

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HoiVienNganhNghe")]
        public bool HoiVienNganhNghe { get; set; } = false;

        public List<HVKhenThuong> khenThuongs { get; set; }
        public HoiVienVM() {
            MaDoanTheChinhTri_HoiDoan = new List<Guid>();
            Id_CLB_DN_MH_HTX_THT = new List<Guid>();
            Ma_ToHoiNganhNghe_ChiHoiNganhNghe = new List<Guid>();
            FileDinhKems = new List<FileDinhKem>();
            khenThuongs = new List<HVKhenThuong>();
        }

        [Display(Name = "Rời hội")]
        public bool IsRoiHoi { get; set; }
    }
    public class HoiVienMTVM : HoiVienVM
    {
        public CanBo GetHoiVien(CanBo obj)
        {
            obj.MaCanBo = MaCanBo;
            obj.MaDinhDanh = MaDinhDanh;
            obj.HoVaTen = HoVaTen;
            obj.NgaySinh = NgaySinh!;
            obj.GioiTinh = GioiTinh;
            obj.MaTinhTrang = null;
            obj.MaChucVu = MaChucVu ;
            obj.SoCCCD = SoCCCD;
            obj.NgayCapCCCD = NgayCapCCCD;
            obj.HoKhauThuongTru = HoKhauThuongTru;
            obj.ChoOHienNay = ChoOHienNay;
            obj.SoDienThoai = SoDienThoai;
            obj.NgayvaoDangDuBi = NgayvaoDangDuBi;
            obj.NgayVaoDangChinhThuc = NgayVaoDangChinhThuc;
            if (NgayVaoDangChinhThuc != null)
            {
                obj.DangVien = true;
            }
            obj.MaDanToc = MaDanToc;
            obj.MaTonGiao = MaTonGiao;
            obj.MaTrinhDoHocVan = MaTrinhDoHocVan;
            obj.MaTrinhDoChuyenMon = MaTrinhDoChuyenMon;
            obj.MaHocVi = MaHocVi;
            obj.MaTrinhDoChinhTri = MaTrinhDoChinhTri;
            obj.NgayVaoHoi = NgayVaoHoi;
            obj.NgayThamGiaCapUyDang = NgayThamGiaCapUyDang;
            obj.NgayThamGiaHDND = NgayThamGiaHDND;
            obj.VaiTro = VaiTro;
            obj.VaiTroKhac = VaiTroKhac;
            obj.ChuyenNganh = ChuyenNganh;
            //obj.MaGiaDinhThuocDien = MaGiaDinhThuocDien;
            obj.HoNgheo = HoNgheo;
            obj.CanNgheo = CanNgheo;
            obj.GiaDinhChinhSach = GiaDinhChinhSach;
            obj.GiaDinhThuocDienKhac = GiaDinhThuocDienKhac;

            obj.MaNgheNghiep = MaNgheNghiep;
            obj.LoaiHoiVien = LoaiHoiVien;
            obj.MaDiaBanHoatDong = MaDiaBanHoatDong;
            obj.ThamGia_SH_DoanThe_HoiDoanKhac = ThamGia_SH_DoanThe_HoiDoanKhac;
            obj.ThamGia_CLB_DN_MH_HTX_THT = ThamGia_CLB_DN_MH_HTX_THT;
            obj.ThamGia_THNN_CHNN = ThamGia_THNN_CHNN;
            obj.GiaDinhThuocDienKhac = GiaDinhThuocDienKhac;
            obj.Loai_DV_SX_ChN = Loai_DV_SX_ChN;
            obj.KKAnToanThucPham = KKAnToanThucPham;
            obj.DKMauNguoiNongDanMoi = DKMauNguoiNongDanMoi;
            obj.DienTich_QuyMo = DienTich_QuyMo;
            obj.SoLuong = SoLuong;
            obj.IsHoiVien = true;
            obj.GhiChu = GhiChu;
            obj.HoiVienUuTuNam = HoiVienUuTuNam;
            obj.HoiVienNongCot = HoiVienNongCot;
            obj.HoiVienDanhDu = HoiVienDanhDu;
            obj.MaChiHoi = MaChiHoi;
            obj.MaToHoi = MaToHoi;
            obj.HoiVienDanCu = HoiVienDanCu;
            obj.HoiVienNganhNghe = HoiVienNganhNghe;

            obj.HoTrovayVon = this.HoTrovayVon;
            obj.HoTroKhac = this.HoTroKhac;
            obj.HoTroDaoTaoNghe = this.HoTroDaoTaoNghe;
            obj.DoanTheChinhTri_HoiDoan_HoiViens = GetListDoanTheChinhTri_HoiDoan();
            obj.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens = GetCauLacBoDNMHHTXTHT();
            obj.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens = GetToHoiNganhNgheChiHoiNganhNghe();

            return obj;
        }
        public static HoiVienVM SetHoiVien(CanBo item) {
            HoiVienVM obj = new HoiVienVM();
            obj.MaCanBo = item.MaCanBo;
            obj.MaDinhDanh = item.MaDinhDanh;
            obj.HoVaTen = item.HoVaTen;
            obj.NgaySinh = item.NgaySinh;
            obj.GioiTinh = item.GioiTinh;
            obj.MaChucVu = item.MaChucVu;
            obj.SoCCCD = item.SoCCCD!;
            obj.NgayCapCCCD = item.NgayCapCCCD;
            obj.HoKhauThuongTru = item.HoKhauThuongTru;
            obj.ChoOHienNay = item.ChoOHienNay!;
            obj.SoDienThoai = item.SoDienThoai;
            obj.NgayvaoDangDuBi = item.NgayvaoDangDuBi;
            obj.NgayVaoDangChinhThuc = item.NgayVaoDangChinhThuc;
            obj.MaDanToc = item.MaDanToc!;
            obj.MaTonGiao = item.MaTonGiao;
            obj.MaTrinhDoHocVan = item.MaTrinhDoHocVan;
            obj.MaTrinhDoChuyenMon = item.MaTrinhDoChuyenMon;
            obj.MaTrinhDoChinhTri = item.MaTrinhDoChinhTri;
            obj.MaHocVi = item.MaHocVi;
            obj.NgayVaoHoi = item.NgayVaoHoi;
            obj.NgayThamGiaCapUyDang = item.NgayThamGiaCapUyDang;
            obj.NgayThamGiaHDND = item.NgayThamGiaHDND;
            obj.VaiTro = item.VaiTro;
            obj.VaiTroKhac = item.VaiTroKhac;
            obj.HoNgheo = item.HoNgheo==null?false: item.HoNgheo!.Value;
            obj.CanNgheo = item.CanNgheo == null ? false : item.CanNgheo!.Value;
            obj.GiaDinhChinhSach = item.GiaDinhChinhSach == null ? false : item.GiaDinhChinhSach.Value;
            obj.GiaDinhThuocDienKhac = item.GiaDinhThuocDienKhac;
            obj.ChuyenNganh = item.ChuyenNganh;
            obj.MaNgheNghiep = item.MaNgheNghiep;
            obj.IDCanBo = item.IDCanBo;
            obj.HinhAnh = item.HinhAnh;
            obj.Actived = item.Actived;
            obj.IsRoiHoi = item.isRoiHoi == null?false:item.isRoiHoi.Value;
            obj.LyDoRoiHoi = item.LyDoRoiHoi;
            obj.NgayRoiHoi = item.NgayRoiHoi;
            obj.LoaiHoiVien = item.LoaiHoiVien!;
            obj.MaDiaBanHoatDong = item.MaDiaBanHoatDong;
            obj.ThamGia_SH_DoanThe_HoiDoanKhac = item.ThamGia_SH_DoanThe_HoiDoanKhac;
            obj.ThamGia_CLB_DN_MH_HTX_THT = item.ThamGia_CLB_DN_MH_HTX_THT;
            obj.ThamGia_THNN_CHNN = item.ThamGia_THNN_CHNN;
            obj.Loai_DV_SX_ChN = item.Loai_DV_SX_ChN;
            obj.DienTich_QuyMo = item.DienTich_QuyMo;
            obj.SoLuong = item.SoLuong;
            obj.KKAnToanThucPham = item.KKAnToanThucPham;
            obj.DKMauNguoiNongDanMoi = item.DKMauNguoiNongDanMoi;
            obj.GhiChu = item.GhiChu;
            obj.DangVien = item.DangVien== null?false:item.DangVien.Value;
            obj.HoiVienUuTuNam = item.HoiVienUuTuNam;

            obj.HoiVienNongCot = item.HoiVienNongCot == null ? false : item.HoiVienNongCot.Value;
            obj.HoiVienDanhDu = item.HoiVienDanhDu == null ? false : item.HoiVienDanhDu.Value;
            obj.HoiVienDanCu = item.HoiVienDanCu == null ? false : item.HoiVienDanCu.Value;
            obj.HoiVienNganhNghe = item.HoiVienNganhNghe == null ? false : item.HoiVienNganhNghe.Value;
            obj.MaToHoi = item.MaToHoi;
            obj.MaChiHoi = item.MaChiHoi;

            obj.HoTrovayVon = item.HoTrovayVon;
            obj.HoTroKhac = item.HoTroKhac;
            obj.HoTroDaoTaoNghe = item.HoTroDaoTaoNghe;
            obj.MaDoanTheChinhTri_HoiDoan = item.DoanTheChinhTri_HoiDoan_HoiViens.Select(it => it.MaDoanTheChinhTri_HoiDoan).ToList();
            obj.Id_CLB_DN_MH_HTX_THT = item.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Select(it => it.Id_CLB_DN_MH_HTX_THT).ToList();
            obj.Ma_ToHoiNganhNghe_ChiHoiNganhNghe = item.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Select(it => it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe).ToList();
            return obj;
        }
        public List<DoanTheChinhTri_HoiDoan_HoiVien> GetListDoanTheChinhTri_HoiDoan() {
            List<DoanTheChinhTri_HoiDoan_HoiVien> kq =new List<DoanTheChinhTri_HoiDoan_HoiVien> ();
            if(this.MaDoanTheChinhTri_HoiDoan != null && this.MaDoanTheChinhTri_HoiDoan!.Count > 0 )
            {
                foreach (var item in MaDoanTheChinhTri_HoiDoan)
                {
                    kq.Add(new DoanTheChinhTri_HoiDoan_HoiVien
                    {
                        MaDoanTheChinhTri_HoiDoan = item,
                        CreatedTime = DateTime.Now
                    });
                }
            }
            return kq;
        }
        public List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien> GetCauLacBoDNMHHTXTHT()
        {
            List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien> kq = new List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien>();
            if (this.Id_CLB_DN_MH_HTX_THT != null && this.Id_CLB_DN_MH_HTX_THT!.Count > 0)
            {
                foreach (var item in Id_CLB_DN_MH_HTX_THT!)
                {
                    kq.Add(new CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien
                    {
                        Id_CLB_DN_MH_HTX_THT = item,
                        CreatedTime = DateTime.Now
                    });
                }
            }
            return kq;
        }
        public List<ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien> GetToHoiNganhNgheChiHoiNganhNghe()
        {
            List<ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien> kq = new List<ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien>();
            if (this.Ma_ToHoiNganhNghe_ChiHoiNganhNghe != null && this.Ma_ToHoiNganhNghe_ChiHoiNganhNghe!.Count > 0)
            {
                foreach (var item in Ma_ToHoiNganhNghe_ChiHoiNganhNghe!)
                {
                    kq.Add(new ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien
                    {
                        Ma_ToHoiNganhNghe_ChiHoiNganhNghe = item,
                        CreatedTime = DateTime.Now
                    });
                }
            }
            return kq;
        }
    }

    public class HoiVienSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public string MaCanBo { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TinhTrang")]
        public string MaTinhTrang { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhanHe")]
        //public string MaPhanHe { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public string? MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuongXa")]
        public string? TenPhuongXa { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public Guid? MaChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

        [Display(Name = "Rời hội")]
        public bool? IsRoiHoi { get; set; }

        public bool? DangChoDuyet { get; set; } = false;

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChiHoi")]
        public Guid? MaChiHoi { get; set; }


        [Display(Name = "Rời từ năm")]
        public int? RoiTuNam { get; set; }
        [Display(Name = "Rời đến năm")]
        public int? RoiDenNam { get; set; }
        public int Skip { get; set; } = 0;
        public string Level { get; set; }

    }
}
