using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class Parameter
    {
        public  Guid ID { get; set; }
        public  String Value { get; set; }
        public Guid Parameter1 { get; set; }
        public Guid AccountID { get; set; }
    }
}
