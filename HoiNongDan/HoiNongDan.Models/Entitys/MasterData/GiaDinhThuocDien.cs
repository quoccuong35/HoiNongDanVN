using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class GiaDinhThuocDien
    {
        [Key]
        [MaxLength(10)]
        public String MaGiaDinhThuocDien { get; set; }
        
        [MaxLength(500)]
        public String TenGiaDinhThuocDien { get; set; }
       
    }
}
