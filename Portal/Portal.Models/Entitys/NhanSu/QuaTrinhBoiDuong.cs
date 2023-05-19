using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class QuaTrinhBoiDuong
    {
        public Guid IDQuaTrinhBoiDuong { get; set; }
        public Guid IDCanBo { get; set; }
        public CanBo CanBo { get; set; }
        [DataType(DataType.Date)]
        public DateTime NgayBatDau { get; set; }
        [DataType(DataType.Date)]
        public DateTime NgayKetThuc { get; set; }
        public String NoiBoiDuong { get; set; }
        public String NoiDung { get; set; }
        public string MaHinhThucDaoTao { get; set; }
        public HinhThucDaoTao HinhThucDaoTao { get; set; }
        public String? FileDinhKem { get; set; }
        public String? GhiChu { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }

    }
}
