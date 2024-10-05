using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class BaoCaoGiamHVVM
    {
        public int STT { get; set; }
        public String? HoVaTen { get; set;  }
        public String? Nam { get; set; }
        public String? Nu { get; set;  }
        public String? QuanHuyen { get; set;  }
        public String? TenHoi { get; set;  }
        public String? DiaChi { get; set;  }

        public String? NamVaoHoi { get; set;  }


        public string? LyDoGiam { get; set;  }

        public String? ChiHoi { get; set;  }
    }
}
