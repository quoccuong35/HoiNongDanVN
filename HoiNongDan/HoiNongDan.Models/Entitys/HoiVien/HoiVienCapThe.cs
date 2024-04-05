using HoiNongDan.Models.Entitys.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys
{
    public class HoiVienCapThe
    {
        public Guid ID { get; set; }
        public Guid MaDot { get; set; }
        public Dot Dot { get; set; }
        public Guid IDHoiVien { get; set; }
        public CanBo HoiVien { get; set; }
        public int Nam {  get; set; }
        public int Quy {  get; set; }
        public string? SoThe { get; set; }
        public DateTime? NgayCap { get; set; }
        public String TrangThai { get; set; } // 01 dang cho, 02 đã cấp/ 03 hủy
        public string? GhiChu { get; set; }
        public bool Actived { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public String LoaiCap { get; set; } //01 thẻ mới, 02 cấp lại

    }
}
