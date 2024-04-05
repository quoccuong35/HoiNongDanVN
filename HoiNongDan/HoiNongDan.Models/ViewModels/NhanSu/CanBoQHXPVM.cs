using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class CanBoQHXPVM
    {
        public Guid? IDCanBo { get; set; }

        [MaxLength(20)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaDinhDanh")]
        public string? MaDinhDanh { get; set; }


        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string HoVaTen { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        //[DataType(DataType.Date)]
        public String? NgaySinh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        public GioiTinh GioiTinh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public Guid? IdDepartment { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public Guid? MaChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DonVi")]
        public String? DonVi { get; set; }


        [MaxLength(50)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public string? MaDanToc { get; set; }

        [MaxLength(50)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string? MaTonGiao { get; set; }

        [MaxLength(1000)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiSinh")]
        public string? NoiSinh { get; set; }

        [MaxLength(1000)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        public string? ChoOHienNay { get; set; }

        //[DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        public string? NgayvaoDangDuBi { get; set; }

        //[DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public string? NgayVaoDangChinhThuc { get; set; }

        [MaxLength(500)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChuyenNganh")]
        public string? ChuyenNganh { get; set; }

        [MaxLength(20)]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChuyenMon")]
        public string? MaTrinhDoChuyenMon { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public string? MaTrinhDoChinhTri { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoNgoaiNgu")]
        public Guid? MaTrinhDoNgoaiNgu { get; set; }

        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoTinHoc")]
        public string? MaTrinhDoTinHoc { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayThamGiaCongTac")]
        public String? NgayThamGiaCongTac { get; set; }

        [Display(Name = "Tham gia BCH")]
        public bool IsBanChapHanh { get; set; } = false;

        [Display(Name = "Tham gia BTV")]
        public bool ThamGiaBTV { get; set; } = false;

        [Display(Name = "UBKT")]
        public bool UBKT { get; set; } = false;

        [Display(Name = "Huyện ủy viên")]
        public bool HuyenUyVien { get; set; } = false;

        [Display(Name = "Đảng ủy viên")]
        public bool DangUyVien { get; set; } = false;

        [Display(Name = "HĐNN Cấp huyện")]
        public bool HDNNCapHuyen { get; set; } = false;

        [Display(Name = "HĐNN Cấp xã")]
        public bool HDNNCapXa { get; set; } = false;

        [Display(Name = "Đánh giá CBCC")]
        public string? DanhGiaCBCC { get; set; }

        [Display(Name = "Đánh giá Đảng viên")]
        public string? DanhGiaDangVien { get; set; }

        [MaxLength(500)]
        public string? HinhAnh { get; set; }

        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        [Display(Name = "Cấp")]
        public string? Level { get; set; }
    }
    public class QHXP {
        public string Level { get; set; }
        public string Name { get; set; }

        public static List<QHXP> GetData(){
            List<QHXP> Edit = new List<QHXP>();
            Edit.Add(new QHXP { Level = "20",Name= "HUYỆN QUẬN" });
            Edit.Add(new QHXP { Level = "30", Name = "XÃ PHƯỜNG, THỊ TRẤN" });
            return Edit;
        }
    }
    public class CanBoQHXPMTVM:CanBoQHXPVM{
        public CanBo getCanBo(CanBo canBo) {
            
            canBo.MaCanBo = this.MaCanBo;
            canBo.MaDinhDanh = this.MaDinhDanh;
            canBo.HoVaTen = this.HoVaTen;
            canBo.NgaySinh = this.NgaySinh;
            canBo.GioiTinh = this.GioiTinh;
            canBo.IdDepartment = this.IdDepartment;
            canBo.MaChucVu = this.MaChucVu;
            canBo.MaDanToc = this.MaDanToc;
            canBo.MaTonGiao = this.MaTonGiao;
            canBo.NoiSinh = this.NoiSinh;
            canBo.ChoOHienNay = this.ChoOHienNay;
            canBo.NgayvaoDangDuBi = this.NgayvaoDangDuBi;
            canBo.NgayVaoDangChinhThuc = this.NgayVaoDangChinhThuc;
            canBo.ChuyenNganh = this.ChuyenNganh;
            canBo.MaTrinhDoChuyenMon = this.MaTrinhDoChuyenMon;
            canBo.MaTrinhDoChinhTri = this.MaTrinhDoChinhTri;
            canBo.MaTrinhDoNgoaiNgu = this.MaTrinhDoNgoaiNgu;
            canBo.MaTrinhDoTinHoc = this.MaTrinhDoTinHoc;
            canBo.NgayThamGiaCongTac = this.NgayThamGiaCongTac;
            canBo.IsBanChapHanh = this.IsBanChapHanh;
            canBo.ThamGiaBTV = this.ThamGiaBTV;
            canBo.UBKT = this.UBKT;
            canBo.HuyenUyVien = this.HuyenUyVien;
            canBo.DangUyVien = this.DangUyVien;
            canBo.HDNNCapHuyen = this.HDNNCapHuyen;
            canBo.HDNNCapXa = this.HDNNCapXa;
            canBo.DanhGiaCBCC = this.DanhGiaCBCC;
            canBo.DanhGiaDangVien = this.DanhGiaDangVien;
            canBo.GhiChu = this.GhiChu;
            canBo.IsCanBo = true;
            canBo.Level = this.Level;
            canBo.DonVi = this.DonVi;

            return canBo;
        }
        public CanBoQHXPVM SetCanBo(CanBo obj) {
            CanBoQHXPVM canBo = new CanBoQHXPVM();
            canBo.IDCanBo = obj.IDCanBo;
            canBo.MaCanBo = obj.MaCanBo;
            canBo.MaDinhDanh = obj.MaDinhDanh;
            canBo.HoVaTen = obj.HoVaTen;
            canBo.NgaySinh = obj.NgaySinh;
            canBo.GioiTinh = obj.GioiTinh;
            canBo.IdDepartment = obj.IdDepartment;
            canBo.MaChucVu = obj.MaChucVu;
            canBo.MaDanToc = obj.MaDanToc;
            canBo.MaTonGiao = obj.MaTonGiao;
            canBo.NoiSinh = obj.NoiSinh;
            canBo.ChoOHienNay = obj.ChoOHienNay;
            canBo.NgayvaoDangDuBi = obj.NgayvaoDangDuBi;
            canBo.NgayVaoDangChinhThuc = obj.NgayVaoDangChinhThuc;
            canBo.ChuyenNganh = obj.ChuyenNganh;
            canBo.MaTrinhDoChuyenMon = obj.MaTrinhDoChuyenMon;
            canBo.MaTrinhDoChinhTri = obj.MaTrinhDoChinhTri;
            canBo.MaTrinhDoNgoaiNgu = obj.MaTrinhDoNgoaiNgu;
            canBo.MaTrinhDoTinHoc = obj.MaTrinhDoTinHoc;
            canBo.NgayThamGiaCongTac = obj.NgayThamGiaCongTac;
            canBo.IsBanChapHanh = obj.IsBanChapHanh == null?false:obj.IsBanChapHanh.Value;
            canBo.ThamGiaBTV = obj.ThamGiaBTV == null ? false : obj.ThamGiaBTV.Value;
            canBo.UBKT = obj.UBKT == null ? false : obj.UBKT.Value;
            canBo.HuyenUyVien = obj.HuyenUyVien == null ? false : obj.HuyenUyVien.Value;
            canBo.DangUyVien = obj.DangUyVien == null ? false : obj.DangUyVien.Value;
            canBo.HDNNCapHuyen = obj.HDNNCapHuyen == null ? false : obj.HDNNCapHuyen.Value;
            canBo.HDNNCapXa = obj.HDNNCapXa == null ? false : obj.HDNNCapXa.Value;
            canBo.DanhGiaCBCC = obj.DanhGiaCBCC;
            canBo.DanhGiaDangVien = obj.DanhGiaDangVien;
            canBo.Level = obj.Level;
            canBo.GhiChu = obj.GhiChu;
            canBo.DonVi = obj.DonVi;

            return canBo;
        }
    }
}
