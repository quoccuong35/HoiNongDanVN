using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HoiVienDeXuatKhenThuongVM
    {
        public Guid? ID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "HinhThucKhenThuong")]
        public string MaHinhThucKhenThuong { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanhHieuKhenThuong")]
        public string MaDanhHieuKhenThuong { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Nam")]
        [Range(2020, 2050, ErrorMessage = "Trong phạm vi từ 2020 - 2050")]
        public int Nam { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Quy")]
        [Range(1, 4, ErrorMessage = "Trong phạm vi từ 1 - 4")]
        public int Quy { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayQuyetDinh")]
        [DataType(DataType.Date)]
        public DateTime? NgayQuyetDinh { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoQuyetDinh")]
        public String? SoQuyetDinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDung")]
        public String? NoiDung { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Loai")]
        public String? Loai { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }


        public HoiVienInfo HoiVien { get; set; }
        public HoiVienDeXuatKhenThuongVM()
        {
            HoiVien = new HoiVienInfo();
        }
    }
    public class HoiVienDeXuatKhenThuongDetailVM
    {
        public Guid? IDQuaTrinhKhenThuong { get; set; }



        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }

        [Display(Name = "Nam")]
        public String Nam { get; set; }


        [Display(Name = "Nữ")]
        public String Nu { get; set; }

        [Display(Name = "số CCCD")]
        public String? SoCCCD { get; set; }


        [Display(Name = "Ngày tháng năm cấp")]
        public String? NgayCap { get; set; }

        [Display(Name = "Quê quán-Địa chỉ thường trú")]
        public String? DiaChiThuongTru { get; set; }

        [Display(Name = "Dân tộc")]
        public String? DanToc { get; set; }

        [Display(Name = "Tôn giáo")]
        public String? TonGiao { get; set; }

        [Display(Name = "Học vấn")]
        public String? HocVan { get; set; }

        [Display(Name = "Chuyên môn")]
        public String? ChuyenMon { get; set; }

        [Display(Name = "Chính trị")]
        public String? ChinhTri { get; set; }

        [Display(Name = "Ngày vào Hội -Chức vụ")]
        public String? NgayVaoHoi { get; set; }

        [Display(Name = "Đã học lớp tìm hiểu về Đảng")]
        public String? DaHocLopDang { get; set; }


        [Display(Name = "Đang học lớp tìm hiều về Đảng")]
        public String? DangHocLopDang { get; set; }


        [Display(Name = "Chưa học lớp tìm hiểu về Đảng")]
        public String? ChuaHocLopDang { get; set; }

        [Display(Name = "Hội cơ sở")]
        public String? DiaBanHND { get; set; }

        [Display(Name = "Chi hội Nông dân")]
        public String? ChiHoiNongDan { get; set; }

        [Display(Name = "Năm giới thiệu")]
        public int? NamDX { get; set; }

    }
    public class HoiVienDeXuatKhenThuongSearchVM {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }

        [Display(Name ="Danh hiệu khen thưởng")]
        public string? MaDanhHieuKhenThuong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Nam")]
        [Range(2020, 2050, ErrorMessage = "Trong phạm vi từ 2020 - 2050")]
        public int? Nam { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Quy")]
        [Range(1, 4, ErrorMessage = "Trong phạm vi từ 1 - 4")]
        public int? Quy { get; set; }

    }
    public class HVKhenThuong { 
        public Guid ID { get; set; }
        public String TenDanhHieu { get; set; }
        public String Nam { get; set; }
        public string GhiChu { get; set; }
    }
}
