using HoiNongDan.Models.Entitys.NhanSu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HinhThucDaoTao
    {
        [MaxLength(50)]
        public string MaHinhThucDaoTao { get; set; }
        public string TenHinhThucDaoTao { get; set; }
        public ICollection<QuaTrinhDaoTao> QuaTrinhDaoTaos { get; set; }
        public ICollection<QuaTrinhBoiDuong> QuaTrinhBoiDuongs { get; set; }
        public HinhThucDaoTao() {
            QuaTrinhDaoTaos = new List<QuaTrinhDaoTao>();
            QuaTrinhBoiDuongs = new List<QuaTrinhBoiDuong>();
        }
    }
}
