using HoiNongDan.Models.Entitys.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class QuaTrinhDaoTao
    {
        public Guid IDQuaTrinhDaoTao { get; set; }
        public Guid IDCanBo { get; set; }
        public CanBo CanBo { get; set; }
        public String CoSoDaoTao { get; set; }

        public DateTime? NgayTotNghiep { get; set; }
        public String QuocGia { get; set; }
        public String MaLoaiBangCap { get; set; }
        public LoaiBangCap LoaiBangCap { get; set; }
        public String MaHinhThucDaoTao { get; set; }
        public HinhThucDaoTao HinhThucDaoTao { get; set; }
        public String MaChuyenNganh { get; set; }
        public ChuyenNganh ChuyenNganh { get; set; }
        public bool? LuanAnTN { get; set; }
        public String? FileDinhKem { get; set; }
        public String? GhiChu { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }

    }
}
