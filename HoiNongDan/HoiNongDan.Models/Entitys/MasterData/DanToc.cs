using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class DanToc
    {
        public String MaDanToc { get; set; }
        public string TenDanToc { get; set; }
        public bool Actived { get; set; } = true;
        public Nullable<int> OrderIndex { get; set; }
        [MaxLength(500)]
        public String Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public ICollection<CanBo> CanBos { get; set; }
        public DanToc()
        {
            CanBos = new List<CanBo>();
        }
    }
}
