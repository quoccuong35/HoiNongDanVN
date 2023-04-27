using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models.Entitys.MasterData
{
    public class TinhTrang
    {
        public string MaTinhTrang { get; set; }
        public string TenTinhTrang { get; set; }
        public bool Actived { get; set; } = true;
        [MaxLength(500)]
        public String Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public ICollection<CanBo> CanBos { get; set; }

        public TinhTrang() {
            CanBos = new List<CanBo>();
        }
    }
}
