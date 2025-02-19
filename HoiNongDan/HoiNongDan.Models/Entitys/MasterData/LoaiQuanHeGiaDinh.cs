using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class LoaiQuanHeGiaDinh
    {
        
        public Guid IDLoaiQuanHeGiaDinh { get; set; }
        public string TenLoaiQuanHeGiaDinh { get; set; }
        public string? Loai { get; set; }
        public bool Actived { get; set; } = true;
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public ICollection<QuanHeGiaDinh> QuanHeGiaDinhs { get; set; }
        public LoaiQuanHeGiaDinh() {
            QuanHeGiaDinhs = new List<QuanHeGiaDinh>();
        }
    }
}
