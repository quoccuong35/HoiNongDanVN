using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class DanhGiaHoiVien
    {
        public Guid ID { get; set; }
        public Guid IDHoiVien { get; set; }
        public CanBo CanBo { get; set; }
        public int Nam { get; set; }
        [MaxLength(100)]
        public String LoaiDanhGia { get; set; }
        public String? GhiChu { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
    }
    public class DanhGiaToChucHoi {
        public Guid ID { get; set; }
        public Guid IDDiaBanHoi { get; set; }
        public DiaBanHoatDong DiaBanHoatDong { get; set; }
        [MaxLength(100)]
        public String LoaiToChuc { get; set; }
        public int Nam { get; set; }
        [MaxLength(100)]
        public String LoaiDanhGia { get; set; }
        public String? GhiChu { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }

        public int SoLuong { get; set; }
    }
}
