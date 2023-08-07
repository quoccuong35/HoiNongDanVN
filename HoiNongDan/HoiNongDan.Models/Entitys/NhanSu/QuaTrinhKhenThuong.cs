using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys.NhanSu
{
    public class QuaTrinhKhenThuong
    {
        public Guid IDQuaTrinhKhenThuong { get; set; }
        public Guid IDCanBo { get; set; }
        public CanBo CanBo { get; set; }

        public string MaHinhThucKhenThuong { get; set; }
        public HinhThucKhenThuong HinhThucKhenThuong { get; set; }
        public string MaDanhHieuKhenThuong { get; set; }
        public DanhHieuKhenThuong DanhHieuKhenThuong { get; set; }

        [DataType(DataType.Date)]
        public DateTime NgayQuyetDinh { get; set; }
        public String SoQuyetDinh { get; set; }
        public String? NguoiKy { get; set; }
        public String LyDo { get; set; }
        public String GhiChu { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public bool? IsCanBo { get; set; }
        public bool? IsHoiVien { get; set; }
    }
}
