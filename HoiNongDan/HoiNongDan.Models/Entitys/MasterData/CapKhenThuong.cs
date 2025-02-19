using HoiNongDan.Models.Entitys.NhanSu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class CapKhenThuong{
        [MaxLength(50)]
        public string MaCapKhenThuong { get; set; }
        [MaxLength(500)]
        public string TenCapKhenThuong { get; set; }
        public  ICollection<QuaTrinhKhenThuong> QuaTrinhKhenThuongs { get; set; }

        CapKhenThuong() {
            QuaTrinhKhenThuongs = new List<QuaTrinhKhenThuong>();
        }
    }
}
