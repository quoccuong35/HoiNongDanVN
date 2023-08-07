using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor.Tokenizer.Symbols;

namespace HoiNongDan.Models
{
    public class HinhThucKyLuat
    {
        public String MaHinhThucKyLuat { get; set; }
        public String TenHinhThucKyLuat { get; set; }
        public bool? DinhChi { get; set; } = false;

        public ICollection<QuaTrinhKyLuat> QuaTrinhKyLuats { get; set; }

        public HinhThucKyLuat() {
            QuaTrinhKyLuats = new List<QuaTrinhKyLuat>();
        }

    }
}
