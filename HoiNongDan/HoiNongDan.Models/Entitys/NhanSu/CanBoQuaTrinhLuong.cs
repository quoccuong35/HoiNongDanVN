using HoiNongDan.Models.Entitys.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class CanBoQuaTrinhLuong
    {
        [Key]
        public Guid ID { get; set; }
        public Guid IDCanBo { get; set; }
        public CanBo CanBo { get; set; }
        [MaxLength(200)]
        public String? MaNgachLuong { get; set; }
        [MaxLength(200)]
        public String? BacLuong { get; set; }
        [MaxLength(200)]
        public String? HeoSoLuong { get; set; }
        [MaxLength(200)]
        public String? HeSoChucVu { get; set; }
        [MaxLength(200)]
        public String? VuotKhung { get; set; }
        [MaxLength(200)]
        public String? KiemNhiem { get; set; }
        [MaxLength(200)]
        public String? NgayHuong { get; set; }

    }
}
