using HoiNongDan.Models.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class CanBoQHXPExcelVM
    {
        public Guid? IDCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaCanBo")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string HoVaTen { get; set; }


        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        //[DataType(DataType.Date)]
        public String? NgaySinh { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public bool GioiTinh { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department")]
        public string TenDonVi { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public string TenChucVu { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public string? TenDanToc { get; set; }


        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string? TenTonGiao { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiSinh")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? NoiSinh { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string? ChoOHienNay { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        public DateTime? NgayvaoDangDuBi { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public DateTime? NgayVaoDangChinhThuc { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChuyenNganh")]
        public string ChuyenNganh { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public string? MaTrinhDoChinhTri { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoNgoaiNgu")]
        public string? TenTrinhDoNgoaiNgu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoTinHoc")]
        public string? TenTrinhDoTinHoc { get; set; }

        [Display(Name = "Thời gian công tác Hội")]
        public string? NgayThamGiaCongTac { get; set; }

        [Display(Name = "Tham gia BCH")]
        public string? ThamGiaBanChapHanh { get; set; }
        
        [Display(Name = "Tham gia BTV")]
        public string? ThamGiaBTV { get; set; }

        [Display(Name = "UBKT")]
        public string? UBKT { get; set; }

        [Display(Name = "Huyện ủy viên")]
        public string? HuyenUyVien { get; set; }

        [Display(Name = "Đảng ủy viên")]
        public string? DangUyVien { get; set; }

        [Display(Name = "HĐNN Cấp huyện")]
        public string? HDNNCapHuyen { get; set; }

        [Display(Name = "HĐNN Cấp xã")]
        public string? HDNNCapXa { get; set; }

        [Display(Name = "NVCT Hội do TW")]
        public string? NVCTTW { get; set; }

        [Display(Name = "Giảng viên kiêm chức")]
        public string? GiangVienKiemChuc { get; set; }

        [Display(Name = "Đánh giá CBCC")]
        public string? DanhGiaCBCC { get; set; }

        [Display(Name = "Đánh giá Đảng viên")]
        public string? DanhGiaDangVien { get; set; }

        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }
    }

    public class CanBoQHXPImportExcelVM
    {
        public int RowIndex { get; set; }
        public bool isNullValueId { get; set; }
        public string Error { get; set; }
        public CanBo CanBo { get; set; }
        public List<DaoTaoBoiDuong> daoTaoBoiDuongs { get; set; }

        public CanBoQHXPImportExcelVM()
        {
            daoTaoBoiDuongs = new List<DaoTaoBoiDuong>();
        }

    }
}
