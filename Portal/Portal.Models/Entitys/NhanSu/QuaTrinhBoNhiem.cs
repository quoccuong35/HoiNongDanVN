
using Portal.Models.Entitys.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class QuaTrinhBoNhiem
    {
        public Guid IdQuaTrinhBoNhiem { get; set; }

        public Guid IDCanBo { get; set; }
        public CanBo CanBo { get; set; }

        [DataType(DataType.Date)]
        public DateTime NgayQuyetDinh { get; set; }

        public String  SoQuyetDinh { get; set; }

        public String?  NguoiKy { get; set; }

        public Guid IdCoSo { get; set; }
        public CoSo CoSo { get; set; }

        public Guid IdDepartment { get; set; }
        public Department Department { get; set; }

        public Guid MaChucVu { get; set; }
        public ChucVu ChucVu { get; set; }

        public decimal? HeSoChucVu { get; set; }
        public String? GhiChu { get; set; }

        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }

    }
}
