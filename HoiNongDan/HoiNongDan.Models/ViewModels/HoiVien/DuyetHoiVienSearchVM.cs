using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{ 
    public class DuyetHoiVienSearchVM
    {
        [Display(Name ="Mã hội viên")]
        public String? MaCanBo { get; set; }

        [Display(Name = "Họ tên")]
        public String? HoVaTen { get; set; }

        [Display(Name = "Địa bàn")]
        public Guid? MaDiaBanHoatDong { get; set; }
    }
}
