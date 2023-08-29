using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys.HoiVien
{
    public class DonVi
    {
        [Key]
        public int IDDonVi { get; set; }
        [MaxLength(500)]
        public string TenDonVi { get; set; }

        public ICollection<BaoCaoThucLucHoi> BaoCaoThucLucHois { get; set; }

        public DonVi() {
            BaoCaoThucLucHois = new List<BaoCaoThucLucHoi>();
        }
    }
}
