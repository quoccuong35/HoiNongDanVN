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
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public String? SoCCCD { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Loai")]
        public String? Loai { get; set; }
        [Display(Name = "Tên câu lạc bộ, đội nhóm, mô hình, hợp tác xã, tổ hợp tác")]
        public String? Ten { get; set; }
    }
    public class HoiVien_CLB_DN_MH_HTX_THTDetailVM
    {
        public string ID { get; set; }
        public int STT { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]

        public string? HoVaTen { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public string? QuanHuyen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuongXa")]
        public string? PhuongXa { get; set; }


        [Display(Name = "Tên")]
        public String? Ten { get; set; }


        [Display(Name = "Loại")]
        public String? Loai { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        public string? ChoOHienNay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }

    }
    public class HoiVien_CLB_DN_MH_HTX_THTExcelVM
    {
        [Display(Name = "STT")]
        public int STT { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]

        public string? HoVaTen { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public string? QuanHuyen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuongXa")]
        public string? PhuongXa { get; set; }
       

        [Display(Name = "Câu lạc bộ")]
        public String? CauLacBo { get; set; }


        [Display(Name = "Đội nhóm")]
        public String? DoiNhom { get; set; }

        [Display(Name = "Mô hình")]
        public String? MoHinh { get; set; }

        [Display(Name = "Hợp tác xã")]
        public String? HopTacXa { get; set; }


        [Display(Name = "Tổ hợp tác")]
        public String? TopHopTac { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        public string? ChoOHienNay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }



    }
}
