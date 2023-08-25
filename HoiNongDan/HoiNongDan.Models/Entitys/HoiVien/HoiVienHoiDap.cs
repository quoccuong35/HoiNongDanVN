using HoiNongDan.Models.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HoiVienHoiDap
    {
        [Key]
        public Guid ID { get; set; }
        public Guid? IDHoivien { get; set; }
        public CanBo? HoiVien { get; set; }

        [MaxLength(2000)]
        public String NoiDung { get; set; }
        public Guid? IdParent { get; set; }
        public DateTime Ngay { get; set; }
        public String TrangThai { get; set; }

        public bool? TraLoi { get; set; }

        public Guid?AcountID { get; set; }
        public Account? Account { get; set;}
    }
}
