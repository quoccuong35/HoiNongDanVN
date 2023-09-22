using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{ 
    public class CanBoQHXPDetailVM : CanBoQHXPExcelVM
    {
        [Display(Name ="Cấp")]
        public String? Cap { get; set; }
    }
}
