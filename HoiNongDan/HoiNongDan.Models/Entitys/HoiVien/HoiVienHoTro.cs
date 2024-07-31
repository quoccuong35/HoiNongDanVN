using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class HoiVienHoTro
    {
        public Guid ID { get; set; }
        public Guid IDHoiVien { get; set; }
        public CanBo HoiVien { get; set; }
      
        public string NoiDung { get; set; }
        public string? GhiChu { get; set; }
        public bool Actived { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }

        public Guid? IDLopHoc { get; set; }
        public LopHoc LopHoc { get; set; }
    }
    public class HinhThucHoTro {
        public Guid MaHinhThucHoTro { get; set; }
        public String TenHinhThuc { get; set; }

        public ICollection<LopHoc> LopHocs { get; set; }
        public HinhThucHoTro() {
            LopHocs = new HashSet<LopHoc>();
        }
    }
    public class NguonVon { 
        public Guid MaNguonVon { get; set; }
        public String TenNguonVon { get; set; }
        public string? GhiChu { get; set; }
        public bool Actived { get; set; }
        public ICollection<VayVon> VayVons { get; set; }
        public NguonVon()
        {
            VayVons = new HashSet<VayVon>();
        }
    }
}
