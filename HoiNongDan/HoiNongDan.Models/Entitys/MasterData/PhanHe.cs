using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class PhanHe
    {
       
        [MaxLength(50)]
        public string MaPhanHe { get; set; }
        [MaxLength(500)]
        public string TenPhanHe { get; set; }

        public ICollection<CanBo> CanBos { get; set; }

        public PhanHe() {
            CanBos = new List<CanBo>();
        }
    }
}
