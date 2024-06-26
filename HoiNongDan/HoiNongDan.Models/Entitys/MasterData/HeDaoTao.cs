﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HeDaoTao
    {
        [Key]
        [MaxLength(50)]
        public string MaHeDaoTao { get; set; }
        [MaxLength(250)]
        public string TenHeDaoTao { get; set; }
        public ICollection<CanBo> CanBos { get; set; }
        public HeDaoTao() {
            CanBos = new List<CanBo>();
        }
    }
}
