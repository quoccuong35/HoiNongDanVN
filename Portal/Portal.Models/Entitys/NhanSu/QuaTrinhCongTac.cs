using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class QuaTrinhCongTac
    {
        public Guid IDQuaTrinhCongTac { get; set; }
        public Guid IDCanBo { get; set; }
        public CanBo CanBo { get; set; }
        [DataType(DataType.Date)]
        public DateTime TuNgay { get; set; }
        [DataType(DataType.Date)]
        public DateTime DenNgay { get; set; }
        public Guid MaChucVu { get; set; }
        public ChucVu ChucVu { get;set; }
        public String NoiLamViec { get; set; }
        public String? GhiChu { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
    }
}
