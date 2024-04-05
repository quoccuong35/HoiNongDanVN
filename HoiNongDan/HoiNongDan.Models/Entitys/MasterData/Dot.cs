using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys.MasterData
{
    public class Dot
    {
        public Guid MaDot { get; set; }
        [MaxLength(200)]
        public ICollection<HoiVienCapThe> HoiVienCapThes { get; set; }
        public String TenDot { get; set; }

        public Dot() {
            HoiVienCapThes= new List<HoiVienCapThe>();
        }
    }
}
