using HoiNongDan.Models.Entitys.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien
    {
        public Guid IDHoiVien { get; set; }

        public CanBo HoiVien { get; set; }
        public Guid Id_CLB_DN_MH_HTX_THT { get; set; }
        public CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac { get; set; }

        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}
