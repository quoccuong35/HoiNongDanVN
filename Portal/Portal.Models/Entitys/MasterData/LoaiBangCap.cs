using Portal.Models.Entitys.NhanSu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class LoaiBangCap
    {
        public string MaLoaiBangCap { get; set; }
        public string TenLoaiBangCap { get; set; }
        public ICollection<QuaTrinhDaoTao> QuaTrinhDaoTaos { get; set; }
        public LoaiBangCap()
        {
            QuaTrinhDaoTaos = new List<QuaTrinhDaoTao>();
        }
    }
}
