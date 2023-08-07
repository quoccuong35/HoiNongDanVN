using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HuuTri
    {
        public Guid Id { get; set; }
        public Guid IDCanBo { get; set; }
        public CanBo? CanBo { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime NgayQuyetDinh { get; set; }
        public String SoQuyetDinh { get; set; }
        public String NguoiKy { get; set; }
        public String GhiChu { get; set; }
        public bool Actived { get; set; } = true;
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
    }
}
