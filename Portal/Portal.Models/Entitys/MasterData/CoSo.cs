using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class CoSo
    {
        public Guid IdCoSo { get; set; }
        public string TenCoSo { get; set; }
        public bool Actived { get; set; } = true;
        public String? Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public Nullable<int> OrderIndex { get; set; }

        public ICollection<CanBo> CanBos { get; set; }
        public ICollection<Department> Departments { get; set; }
        public ICollection<QuaTrinhBoNhiem> QuaTrinhBoNhiems { get; set; }
        public ICollection<QuaTrinhMienNhiem> QuaTrinhMienNhiems { get; set; }
        public CoSo() {
            CanBos = new List<CanBo>();
            Departments = new List<Department>();
            QuaTrinhBoNhiems = new List<QuaTrinhBoNhiem>();
            QuaTrinhMienNhiems = new List<QuaTrinhMienNhiem>();
        }
    }
}
