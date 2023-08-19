using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HocVi
    {
        [Key]
        [MaxLength(50)]
        public String MaHocVi { get; set; }
        [MaxLength(200)]
        public String  TenHocVi { get; set; }
        public ICollection<CanBo> CanBos { get; set; }

        public HocVi() {
            CanBos = new List<CanBo>();
        }
    }
}
