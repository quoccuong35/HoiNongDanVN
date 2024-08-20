using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class ChiHoi
    {
        public Guid MaChiHoi { get; set; } 
        public string TenChiHoi { get; set; }
        public bool Actived { get; set; } = true;
        public String? Description { get; set; }
        public String? Loai { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public Nullable<int> OrderIndex { get; set; }

        public ICollection<CanBo> CanBos { get; set; }

        public ChiHoi() {
            CanBos = new List<CanBo>();
        }
    }
    public class ToHoi {
        public Guid MaToHoi { get; set; }
        public string TenToHoi { get; set; }
        public String? Loai { get; set; }
        public bool Actived { get; set; } = true;
        public String? Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public ICollection<CanBo> CanBos { get; set; }

        public ToHoi()
        {
            CanBos = new List<CanBo>();
        }
    }
}
