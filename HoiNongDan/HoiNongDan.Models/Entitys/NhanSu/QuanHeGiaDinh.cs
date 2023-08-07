using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class QuanHeGiaDinh
    {
        public Guid IDQuanheGiaDinh { get; set; }
        public Guid? IDCanBo { get; set; }
        public CanBo CanBo { get; set; }
        public Guid? IDHoiVien { get; set; }
        public string HoTen { get; set; }
        [MaxLength(10)]
        public String NgaySinh { get; set; }
        public string NgheNghiep { get; set; }
        public string? NoiLamVien { get; set; }
        public string DiaChi { get; set; }
        public string GhiChu { get; set; }
        public Guid IDLoaiQuanHeGiaDinh { get; set; }
        public LoaiQuanHeGiaDinh LoaiQuanhe { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }

    }
}
