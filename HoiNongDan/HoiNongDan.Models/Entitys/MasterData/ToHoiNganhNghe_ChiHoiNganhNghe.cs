using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class ToHoiNganhNghe_ChiHoiNganhNghe
    {
        public Guid Ma_ToHoiNganhNghe_ChiHoiNganhNghe { get; set; }
        public String Ten { get; set; }
        public DateTime NgayThanhLap { get; set; }
        public bool Actived { get; set; } = true;
        [MaxLength(500)]
        public String? Description { get; set; }
        public String? Loai { get; set; } //01 chi hội 02 tổ hội
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public Nullable<bool> NgheNghiep { get; set; }
        public Nullable<Guid> MaDiaBanHoatDong { get; set; }
        public DiaBanHoatDong DiaBanHoatDong { get; set; }
        public Nullable<DateTime> NgayGiam { get; set; }
        public String? LyDoGiam { get; set; }
        public ICollection<ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien> ToHoiNganhNghe_ChiHoiNganhNghe_HVs { get; set; }
        public ToHoiNganhNghe_ChiHoiNganhNghe() {
            ToHoiNganhNghe_ChiHoiNganhNghe_HVs = new List<ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien>();
        }
    }
}
