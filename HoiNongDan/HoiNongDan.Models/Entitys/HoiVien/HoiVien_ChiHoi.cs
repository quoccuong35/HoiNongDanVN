using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys.HoiVien
{
    public class HoiVien_ChiHoi
    {

        public Guid IDHoiVien { get; set; }

        public CanBo HoiVien { get; set; }
        public Guid MaChiHoi { get; set; }

        public ChiHoi ChiHoi { get; set; }  

        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
