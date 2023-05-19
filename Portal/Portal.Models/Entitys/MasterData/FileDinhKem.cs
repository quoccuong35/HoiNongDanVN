using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{ 
    public class FileDinhKem
    {
        public Guid Key { get; set; }
        public Guid Id { get; set; }
        public Guid IdCanBo { get; set; }
        public CanBo CanBo { get; set; }
        public string IDLoaiDinhKem { get; set; }
        [MaxLength(200)]
        public string FileName { get; set; }
        public LoaiDinhKem LoaiDinhKem { get; set; }
        [MaxLength(500)]
        public string Url { get; set;}

    }
    public class LoaiDinhKem {
        [Key]
        [MaxLength(10)]
        public String IDLoaiDinhKem { get; set; }
        [MaxLength(200)]
        public String TenLoaiDinhKem { get; set; }
        public ICollection<FileDinhKem> FileDinhKems { get; set; }
        public LoaiDinhKem() {
            FileDinhKems = new List<FileDinhKem>();
        }
    }
}
