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
        [MaxLength(50)]
        public String? MaNgachLuong { get; set; }
        public NgachLuong NgachLuong { get; set; }
        public Guid? MaBacLuong { get; set; }
        public BacLuong BacLuong { get; set; }
        [MaxLength(200)]
        public double? HeSoLuong { get; set; }
        [MaxLength(200)]
        public double? HeSoChucVu { get; set; }
        [MaxLength(200)]
        public double? VuotKhung { get; set; }
        [MaxLength(200)]
        public double? KiemNhiem { get; set; }
        [MaxLength(200)]
        public DateTime? NgayHuong { get; set; }
        public DateTime? NgayNangBacLuong { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? CreatedAccountId { get; set; }

        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }

    }
}
