using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HoiVienLichSuDuyet
    {
        public Guid ID { get; set; }
        public Guid IDHoiVien { get; set; }
        public CanBo HoiVien { get; set; }
        public Guid AccountID { get; set; }
        public Guid? AccountIDDuyet { get; set; }
        public DateTime? AccountIDDuyetTime { get; set; }
        public DateTime? CreateTime { get; set; }
        public bool TrangThaiDuyet { get; set; } = false;

    }
}
