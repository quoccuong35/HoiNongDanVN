using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class DiaBanHoatDong_ThanhVien
    {
        public Guid Id { get; set; }
        public Guid IdDiaBan { get; set; }
        public DiaBanHoatDong DiaBan { get; set; }

        public Guid IDCanBo { get; set; }
        public CanBo CanBo { get; set; }

        public Guid MaChucVu { get; set; }
        public ChucVu ChucVu { get; set; }
       
        public DateTime NgayVao { get; set; }
        public DateTime NgayRoiDi { get; set; }
        public String? LyDo { get; set; }
        public bool RoiDi { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
    }
}
