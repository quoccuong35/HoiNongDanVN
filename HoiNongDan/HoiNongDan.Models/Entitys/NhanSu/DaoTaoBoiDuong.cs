using HoiNongDan.Models.Entitys.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys
{
    public class DaoTaoBoiDuong
    {
        public Guid Id { get; set; }
        public Guid IDCanBo { get; set; }
        public CanBo CanBo { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }

        [MaxLength(50)]
        public String MaHinhThucDaoTao { get; set; }
        public HinhThucDaoTao HinhThucDaoTao { get; set; }

        [MaxLength(50)]
        public String? MaLoaiBangCap { get; set; }
        public LoaiBangCap? LoaiBangCap { get; set; }
        [MaxLength(1000)]
        public String NoiDungDaoTao { get; set; }
        public String? GhiChu { get; set; }

        public bool Actived { get; set; } = true;

        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }

    }
}
