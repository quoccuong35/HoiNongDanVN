using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Portal.Models
{
    public class NgachLuong
    {
        [MaxLength(50)] 
        public string MaNgachLuong { get; set; }
        public string TenNgachLuong { get; set; }
        public int NamTangLuong { get; set; }
        public String MaLoai { get; set; }
        public bool Actived { get; set; } = true;
        public string? Description { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public int? OrderIndex { get; set; }
        public ICollection<BacLuong> BacLuongs { get; set; }
        public NgachLuong() {
            BacLuongs = new List<BacLuong>();
        }
    }
}
