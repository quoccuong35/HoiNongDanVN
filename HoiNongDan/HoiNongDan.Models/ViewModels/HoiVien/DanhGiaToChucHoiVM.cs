using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class DanhGiaToChucHoiVM
    {
        public Guid ID { get; set; }
        public String DonVi { get; set; }
        public int? CoSo_Tong { get; set; }
        public int? CoSo_HTXSNV { get; set; }
        public int? CoSo_HTTNV { get; set; }
        public int? CoSo_HTNV { get; set; }
        public int? CoSo_KHTNV { get; set; }
        public int? CoSo_KPhanLoai { get; set; }

        public int? DanCu_Tong { get; set; }
        public int? DanCu_HTXSNV { get; set; }
        public int? DanCu_HTTNV { get; set; }
        public int? DanCu_HTNV { get; set; }
        public int? DanCu_KHTNV { get; set; }
        public int? DanCu_KPhanLoai { get; set; }

        public int? NgheNghiep_Tong { get; set; }
        public int? NgheNghiep_HTXSNV { get; set; }
        public int? NgheNghiep_HTTNV { get; set; }
        public int? NgheNghiep_HTNV { get; set; }
        public int? NgheNghiep_KHTNV { get; set; }
        public int? NgheNghiep_KPhanLoai { get; set; }

        public String? GhiChu { get; set; }
    }
}
