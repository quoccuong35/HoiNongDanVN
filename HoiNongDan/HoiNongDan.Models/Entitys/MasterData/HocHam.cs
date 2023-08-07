using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models 
{ 
    public class HocHam
    {
        public string MaHocHam { get; set; }
        public string TenHocHam { get; set; }

        public ICollection<CanBo> CanBos { get; set; }
        public HocHam()
        {
            CanBos = new List<CanBo>();
        }
    }
}
