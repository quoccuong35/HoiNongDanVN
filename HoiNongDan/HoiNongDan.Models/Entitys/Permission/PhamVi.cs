using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys
{
    public class PhamVi
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public Guid MaDiabanHoatDong { get; set; }
        public DiaBanHoatDong DiaBanHoatDong { get; set; }
        public Guid CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}
