using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class TinhThanhPho
    {
        public string MaTinhThanhPho { get; set; }
        public string TenTinhThanhPho { get; set; }
        public string MaKhuVuc { get; set; }
        public KhuVuc KhuVuc { get; set; }
        public bool Actived { get; set; } = true;
        [MaxLength(500)]
        public String Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public ICollection<QuanHuyen> QuanHuyens { get; set; }

        public TinhThanhPho() {
            QuanHuyens = new List<QuanHuyen>();
        }

    }
}
