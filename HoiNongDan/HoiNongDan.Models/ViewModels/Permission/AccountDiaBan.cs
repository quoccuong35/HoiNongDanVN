using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class AccountDiaBan
    {
        public Guid MaDiaBanHoiVien { get; set; }
        public string TenDiaBanHoiVien { get; set; }
        public bool Selected { get; set; } = false;
        public String? MaQuanHuyen { get; set; }
        public string? TenQuanHuyen { get; set; }
    }
}
