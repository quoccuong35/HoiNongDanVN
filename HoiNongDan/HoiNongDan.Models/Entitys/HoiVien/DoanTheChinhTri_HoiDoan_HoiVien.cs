using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class DoanTheChinhTri_HoiDoan_HoiVien
    {
        public Guid IDHoiVien { get; set; }

        public CanBo HoiVien { get; set; }
        public Guid MaDoanTheChinhTri_HoiDoan { get; set; }
        public DoanTheChinhTri_HoiDoan DoanTheChinhTri_HoiDoan { get; set; }

        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}
