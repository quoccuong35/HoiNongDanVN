using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class BacLuong
    {
        public Guid MaBacLuong { get; set; }
    
        public String TenBacLuong { get; set; }
        public decimal HeSo { get; set; }
        public bool Actived { get; set; } = true;
        public String? Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public Nullable<int> OrderIndex { get; set; }
      
    
        [MaxLength(50)]
        public string MaNgachLuong { get; set; }
        public NgachLuong NgachLuong { get; set; }

        public ICollection<CanBo> CanBos { get; set; }
        public BacLuong()
        {
            CanBos = new List<CanBo>();
        }

    }
}
