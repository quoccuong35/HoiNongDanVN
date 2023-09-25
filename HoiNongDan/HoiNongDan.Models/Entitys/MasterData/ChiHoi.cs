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

        public ICollection<CanBo> CanBos { get; set; }

        public ChiHoi() {
            CanBos = new List<CanBo>();
        }
    }
    public class ToHoi {
        public Guid MaToHoi { get; set; }
        public string TenToHoi { get; set; }
        public ICollection<CanBo> CanBos { get; set; }

        public ToHoi()
        {
            CanBos = new List<CanBo>();
        }
    }
}
