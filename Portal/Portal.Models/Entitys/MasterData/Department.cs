using Portal.Models.Entitys;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class Department
    {
        public Guid Id { get; set; }

        public String Code { get; set; }

        public string Name { get; set; }
        public bool Actived { get; set; }
        public String? Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public ICollection<CanBo> CanBos { get; set; }
        public Guid? IDCoSo { get; set; }
        public CoSo CoSo { get; set; }
        public Department()
        {
            CanBos = new List<CanBo>();
        }
    }
}
