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
    public class HoiVienVayVon
    {
        public Guid IDVayVon { get; set; }
        public Guid IDCanBo { get; set; }
        public CanBo CanBo { get; set; }
        public long SoTienVay { get; set; }
        public int ThoiHangChoVay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double LaiSuatVay { get; set; }
        [DataType(DataType.Date)]
        public DateTime TuNgay { get; set; }
        [DataType(DataType.Date)]
        public DateTime DenNgay { get; set; }
        [DataType(DataType.Date)]
        public DateTime NgayTraNoCuoiCung { get; set; }
        public string NoiDungVay { get; set; }
        public string GhiChu { get; set; }
        public bool Actived { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public bool TraXong { get; set; }

    }
}
