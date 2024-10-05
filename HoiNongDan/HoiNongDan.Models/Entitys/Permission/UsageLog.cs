using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys
{
    public class UsageLog
    {
        [Key]
        public Guid ID { get; set; }
        public Guid AccountID { get; set; }
        public Account Account { get; set; }
        public DateTime TimeAccessed { get; set; }
        [MaxLength(500)]
        public String URLAccessed { get; set; }
        public String? IPAddress { get; set; }

    }
}
