using Portal.Models.Entitys.NhanSu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class HinhThucKhenThuong
    {
        public string MaHinhThucKhenThuong { get; set; }
        public string TenHinhThucKhenThuong { get; set; }

        public ICollection<QuaTrinhKhenThuong> QuaTrinhKhenThuong { get; set; }
        public HinhThucKhenThuong() {
            QuaTrinhKhenThuong = new List<QuaTrinhKhenThuong>();
        }
    }
    public class DanhHieuKhenThuong { 
        public string MaDanhHieuKhenThuong { get; set; }
        public string TenDanhHieuKhenThuong { get; set; }
        public ICollection<QuaTrinhKhenThuong> QuaTrinhKhenThuong { get; set; }
        public DanhHieuKhenThuong()
        {
            QuaTrinhKhenThuong = new List<QuaTrinhKhenThuong>();
        }
    }
}
