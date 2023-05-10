using Portal.Models.Entitys.NhanSu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class ChuyenNganh
    {
        public string MaChuyenNganh { get; set; }
        public string TenChuyenNganh { get; set; }
        public ICollection<QuaTrinhDaoTao> QuaTrinhDaoTaos { get; set; }
        public ChuyenNganh()
        {
            QuaTrinhDaoTaos = new List<QuaTrinhDaoTao>();
        }

    }
}
