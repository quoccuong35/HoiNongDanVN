using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys
{
    public class Author_Token
    {
        public Guid ID { get; set; }    
        public Guid AccountID { get; set; }
        public Account Account { get; set; }
        public DateTime ExpireTimeSpan { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
