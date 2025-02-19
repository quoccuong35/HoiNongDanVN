using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class CanBoQuaCacThoiKy
    {
        [Key]
        public Guid Id { get; set; }
        public String? HoVaTen { get; set; }
        public String? SoCCCD { get; set; }
        public String? SoDienThoai { get; set; }
        public String? ChucVu { get; set; }
        public String? NhiemKy { get; set; }
        public String? NoiLamViec { get; set; }
        public String? NoiCuTru { get; set; }
        public String? ChucVuHienNay { get; set; }
    }
}
