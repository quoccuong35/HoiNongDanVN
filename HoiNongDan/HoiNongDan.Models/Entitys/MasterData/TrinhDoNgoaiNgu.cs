using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys
{
    public  class TrinhDoNgoaiNgu
    {
        [Key]
        public Guid MaTrinhDoNgoaiNgu { get; set; }
        [MaxLength(500)]
        public string TenTrinhDoNgoaiNgu { get; set; }
        public bool Actived { get; set; } = true;
        public Nullable<int> OrderIndex { get; set; }
        public String? Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        [MaxLength(50)]
        public String MaNgonNgu { get; set; }
        public NgonNgu NgonNgu { get; set; }
        public ICollection<CanBo> CanBos { get; set; }
        public TrinhDoNgoaiNgu()
        {
            CanBos = new List<CanBo>();
        }
    }
    public class NgonNgu {

       
        [Key]
        [MaxLength(50)]
        public string MaNgonNgu { get; set; }

        [MaxLength(500)]
        public string TenNgonNgu { get; set; }

        public bool Actived { get; set; } = true;
        public Nullable<int> OrderIndex { get; set; }
        public String? Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public ICollection<TrinhDoNgoaiNgu> TrinhDoNgoaiNgus { get; set; }
        public NgonNgu() {
            TrinhDoNgoaiNgus = new List<TrinhDoNgoaiNgu>();
        }
    }
}
