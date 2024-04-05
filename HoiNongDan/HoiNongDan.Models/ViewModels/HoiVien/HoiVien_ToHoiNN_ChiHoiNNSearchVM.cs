using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HoiNongDan.Models
{
    public class HoiVien_ToHoiNN_ChiHoiNNSearchVM
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }

        [Display(Name = "Tổ hội ngành nghề, chi hội ngành nghề")]
        public Guid? Ma_ToHoiNganhNghe_ChiHoiNganhNghe { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public String? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public String? MaHoiVien { get; set; }
    }
    public class HoiVien_ToHoiNN_ChiHoiNNDetailVM
    {
        public string ID {  get; set; }
        [Display(Name = "STT")]
        public int STT { get; set; }

      

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]

        public string? HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public string? MaCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }


        [Display(Name = "Tên Chi hội nghề nghiệp")]
        public String? TenChiHoi { get; set; }

        [Display(Name = "Tên Tổ hội nghề nghiệp")]
        public String? TenToHoi { get; set; }

        [Display(Name = "Khu phố ấp")]
        public string? ChoOHienNay { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuongXa")]
        public string? PhuongXa { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public string? QuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }
    }
    public class HoiVien_ToHoiNN_ChiHoiNNExcelVM
    {
        [Display(Name ="STT")]
        public int STT {  get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]

        public string? HoVaTen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHoiVien")]
        public string? MaCanBo { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public string? SoCCCD { get; set; }


        [Display(Name = "Tên Chi hội nghề nghiệp")]
        public String? TenChiHoi { get; set; }

        [Display(Name = "Tên Tổ hội nghề nghiệp")]
        public String? TenToHoi { get; set; }

        [Display(Name = "Khu phố ấp")]
        public string? ChoOHienNay { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PhuongXa")]
        public string? PhuongXa { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public string? QuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public string? GhiChu { get; set; }

    }
}
