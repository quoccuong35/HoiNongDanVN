using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{ 
    public class KhuVuc
    {
        [Key]
        [MaxLength(50)]
        public string MaKhuVuc { get; set; }
        [MaxLength(250)]
        public string TenKhuVuc { get; set; }
        public ICollection<TinhThanhPho> TinhThanhPhos { get; set; }
        public KhuVuc() {
            TinhThanhPhos = new List<TinhThanhPho>();
        }
    }
}
