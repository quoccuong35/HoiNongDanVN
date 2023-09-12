using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.Entitys.NhanSu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class LoaiBangCap
    {
        public string MaLoaiBangCap { get; set; }
        public string TenLoaiBangCap { get; set; }
        public ICollection<DaoTaoBoiDuong> DaoTaoBoiDuongs { get; set; }
        public LoaiBangCap()
        {
            DaoTaoBoiDuongs = new List<DaoTaoBoiDuong>();
        }
    }
}
