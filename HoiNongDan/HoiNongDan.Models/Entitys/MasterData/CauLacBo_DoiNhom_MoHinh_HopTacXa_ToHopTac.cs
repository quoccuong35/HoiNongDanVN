using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac
    {
        public Guid Id_CLB_DN_MH_HTX_THT { get; set; }
        public String Ten { get; set; }
        public String? Loai { get; set; }
        public bool Actived { get; set; } = true;
        [MaxLength(500)]
        public String? Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public ICollection<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien> CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens { get; set; }
        public CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac() {
            CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens = new List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien>();
        }

    }
}
