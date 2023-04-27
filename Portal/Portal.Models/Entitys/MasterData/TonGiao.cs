using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{ 
    public class TonGiao
    {
        public string MaTonGiao { get; set; }
        public string TenTonGiao { get; set; }
        public bool Actived { get; set; } = false;
        public Nullable<int> OrderIndex { get; set; }
        [MaxLength(500)]
        public String Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public ICollection<CanBo> CanBos { get; set; }
        public TonGiao()
        {
            CanBos = new List<CanBo>();
        }
    }
}
