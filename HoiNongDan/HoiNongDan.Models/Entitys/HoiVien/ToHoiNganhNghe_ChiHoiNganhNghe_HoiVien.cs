using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien
    {
        public Guid IDHoiVien { get; set; }

        public CanBo HoiVien { get; set; }
        public Guid Ma_ToHoiNganhNghe_ChiHoiNganhNghe { get; set; }
        public ToHoiNganhNghe_ChiHoiNganhNghe ToHoiNganhNghe_ChiHoiNganhNghe { get; set; }

        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? NgayVao { get; set; }
        public DateTime? NgayRoi { get; set; }
        public String? LyDoRoi { get; set; }
    }
}
