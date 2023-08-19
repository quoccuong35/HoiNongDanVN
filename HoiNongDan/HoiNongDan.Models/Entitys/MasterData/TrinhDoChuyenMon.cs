using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys
{
    public class TrinhDoChuyenMon
    {
        [MaxLength(20)]
        public String MaTrinhDoChuyenMon { get; set; }
        [MaxLength(200)]
        public string TenTrinhDoChuyenMon { get; set; }
        public ICollection<CanBo> CanBos { get; set; }
        public TrinhDoChuyenMon()
        {
            CanBos = new List<CanBo>();
        }
    }
}
