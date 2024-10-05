using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys.MasterData
{
    public class PhuongXa
    {
        public string MaPhuongXa { get; set; }
        public string TenPhuongXa { get; set; }
        public string MaQuanHuyen { get; set; }
        public QuanHuyen QuanHuyen { get; set; }
        public bool Actived { get; set; } = true;
        [MaxLength(500)]
        public String Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public ICollection<DiaBanHoatDong> DiaBanHoatDongs { get; set; }

        public PhuongXa() {
            DiaBanHoatDongs = new List<DiaBanHoatDong>();
        }
       
    }
}
