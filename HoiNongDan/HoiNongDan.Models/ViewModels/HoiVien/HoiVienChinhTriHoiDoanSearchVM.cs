﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HoiVienChinhTriHoiDoanSearchVM
    {

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        [Display(Name ="Địa bàn hội viên")]
        public Guid? MaDiaBanHoiVien { get; set; }

        [Display(Name ="Tên đoàn thể chính trị-Hội đoàn khác")]
        public Guid? MaDoanTheChinhTri_HoiDoan { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public String? HoVaTen { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoCCCD")]
        public String? SoCCCD { get; set; }


        [Display(Name = "Tên đoàn thể chính trị-Hội đoàn khác")]
        public String? TenDoanThe { get; set; }

    }
    public class HoiVienChinhTriHoiDoanDetailVM: HoiVienDetailVM
    {
        public Guid? MaDoanTheChinhTri_HoiDoan { get; set; }
        [Display(Name = "Tên đoàn thể chính trị-Hội đoàn khác")]
        public String TenDoanTheChinhChi_HoiDon { get; set; }

    }
}
