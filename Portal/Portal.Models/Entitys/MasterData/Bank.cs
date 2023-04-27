using System;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class Bank
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(50)]
        public String Code { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public bool Actived { get; set; } = false;
        [MaxLength(500)]
        public String Description { get; set; }
        public Nullable<System.Guid> CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public Nullable<System.Guid> LastModifiedAccountId { get; set; }
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        public Nullable<int> OrderIndex { get; set; }
    }
}
