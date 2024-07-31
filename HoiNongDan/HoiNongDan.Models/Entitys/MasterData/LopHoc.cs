using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class LopHoc
    {
        public Guid IDLopHoc { get; set; }
        public string TenLopHoc { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public Guid MaHinhThucHoTro { get; set; }
        public HinhThucHoTro HinhThucHoTro { get; set; }
        public bool Actived { get; set; } = true;
        public String? Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public ICollection<HoiVienHoTro> HoiVienHoTros { get; set; }
        public LopHoc() {
            HoiVienHoTros = new List<HoiVienHoTro>();
        }
    }
}
