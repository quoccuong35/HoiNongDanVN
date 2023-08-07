using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class NgheNghiep
    {
        [MaxLength(10)]
        [Key]
        public String MaNgheNghiep { get; set; }
        [MaxLength(500)]
        public String TenNgheNghiep { get; set; }

        public ICollection<CanBo> CanBos { get; set; }
        public NgheNghiep()
        {
            CanBos = new List<CanBo>();
        }
    }
}
