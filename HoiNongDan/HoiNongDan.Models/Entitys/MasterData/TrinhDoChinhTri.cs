using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HoiNongDan.Models.Entitys
{
    public class TrinhDoChinhTri
    {
        public String MaTrinhDoChinhTri { get; set; }
        public String TenTrinhDoChinhTri { get; set; }
        public bool Actived { get; set; } = true;
        public Nullable<int> OrderIndex { get; set; }
        public String? Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public ICollection<CanBo> CanBos { get; set; }
        public TrinhDoChinhTri()
        {
            CanBos = new List<CanBo>();
        }
    }
}
