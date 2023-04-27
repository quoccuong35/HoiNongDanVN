using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models.Entitys.MasterData
{
    public class TrinhDoHocVan
    {
        public string MaTrinhDoHocVan { get; set; }
        public string TenTrinhDoHocVan { get; set; }
        public bool Actived { get; set; } = true;
        public Nullable<int> OrderIndex { get; set; }
        public String? Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public ICollection<CanBo> CanBos { get; set; }
        public TrinhDoHocVan()
        {
            CanBos = new List<CanBo>();
        }
    }
}
