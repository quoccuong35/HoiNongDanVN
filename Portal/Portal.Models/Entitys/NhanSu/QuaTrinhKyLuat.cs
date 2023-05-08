using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{ 
    public class QuaTrinhKyLuat
    {
        public Guid IdQuaTrinhKyLuat { get; set; }
        public Guid IDCanBo { get; set; }
        public CanBo CanBo { get; set; }
        public String SoQuyetDinh { get; set; }
        public String? NguoiKy { get; set; }

        [DataType(DataType.Date)]
        public DateTime NgayKy { get; set; }
        public String LyDo { get; set; }
        public String? GhiChu { get; set; }

        [MaxLength(50)]
        public String MaHinhThucKyLuat { get; set; }
        public HinhThucKyLuat HinhThucKyLuat { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }

    }
}
