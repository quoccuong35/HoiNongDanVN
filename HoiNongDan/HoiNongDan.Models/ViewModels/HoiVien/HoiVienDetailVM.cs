﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HoiVienDetailVM : HoiVienVM
    {


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public string DanToc { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public string TonGiao { get; set; }

        [Display(Name = "Trình độ học vấn")]
        public string TrinhDoHocvan { get; set; }

        [Display(Name = "Trình độ chuyên môn")]
        public string TrinhDoChuyenMon { get; set; }

        [Display(Name = "Trình độ chính chị")]
        public string TrinhDoChinhChi { get; set; }

        [Display(Name = "Địa bàn")]
        public string TenDiaBanHoatDong { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChucVu")]
        public string TenChucVu { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VaiTro")]
        public string VaiTro { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GiaDinhThuocDien")]
        public string GiaDinhThuocDien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgheNghiepHienNay")]
        public string NgheNghiepHienNay { get; set; }

        
    }
}