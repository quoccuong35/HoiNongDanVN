using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HoiNongDan.Models
{
    public class HoiVien_CLB_DN_MH_HTX_THTSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }

        [Display(Name = "Tên câu lạc bộ, đội nhóm, mô hình, hợp tác xã, tổ hợp tác")]
        public Guid? Id_CLB_DN_MH_HTX_THT { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public String? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public String? MaHoiVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Loai")]
        public String? Loai { get; set; }
    }
    public class HoiVien_CLB_DN_MH_HTX_THTDetailVM
    {
        public string ID { get; set; }
        [Display(Name = "STT")]
        public int STT { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]

        public string? HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }


        [Display(Name = "Câu Lạc Bộ")]
        public String? CauLacBo { get; set; }

        [Display(Name = "Đội Nhóm")]
        public String? DoiNhom { get; set; }

        [Display(Name = "Mô Hình")]
        public String? MoHinh { get; set; }


        [Display(Name = "Hợp Tác Xã")]
        public String? HopTacXa { get; set; }

        [Display(Name = "Tổ Hợp Tác")]
        public String? TopHopTac { get; set; }


        [Display(Name = "Khu phố ấp")]
        public string? ChoOHienNay { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuongXa")]
        public string? PhuongXa { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public string? QuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }
    }
    public class HoiVien_CLB_DN_MH_HTX_THTExcelVM
    {
        [Display(Name = "STT")]
        public int STT { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]

        public string? HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }


        [Display(Name = "Câu Lạc Bộ")]
        public String? CauLacBo { get; set; }

        [Display(Name = "Đội Nhóm")]
        public String? DoiNhom { get; set; }

        [Display(Name = "Mô Hình")]
        public String? MoHinh { get; set; }


        [Display(Name = "Hợp Tác Xã")]
        public String? HopTacXa { get; set; }

        [Display(Name = "Tổ Hợp Tác")]
        public String? TopHopTac { get; set; }


        [Display(Name = "Khu Phố Ấp")]
        public string? ChoOHienNay { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuongXa")]
        public string? PhuongXa { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public string? QuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }
    }
}
