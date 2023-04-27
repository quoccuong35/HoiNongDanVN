using Portal.Models.Entitys.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class QuanHuyen
    {
        public string MaQuanHuyen { get; set; }
        public string TenQuanHuyen { get; set; }
        public string MaTinhThanhPho { get; set; }
        public TinhThanhPho TinhThanhPho { get; set; }
        public bool Actived { get; set; } = true;
        [MaxLength(500)]
        public String Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public ICollection<PhuongXa> PhuongXas { get; set; }
        public QuanHuyen() {
            PhuongXas = new List<PhuongXa>();
        }
    }
}
