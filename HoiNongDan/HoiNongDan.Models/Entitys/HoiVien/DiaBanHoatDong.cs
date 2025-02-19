using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.Entitys.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class DiaBanHoatDong
    {
        public Guid Id { get; set; }
        public string TenDiaBanHoatDong { get; set; }
        [MaxLength(50)]
        public String MaTinhThanhPho { get; set; }
        public TinhThanhPho TinhThanhPho { get; set; }

        [MaxLength(50)]
        public String MaQuanHuyen { get; set; }
        public QuanHuyen QuanHuyen { get; set; }  
        
        [MaxLength(50)]
        public String MaPhuongXa { get; set; }
        public PhuongXa PhuongXa { get; set; }

        [MaxLength(250)]
        public string DiaChi { get; set; }
        [DataType(DataType.Date)]
        public DateTime? NgayThanhLap { get; set; }
        [MaxLength(500)]
        public string GhiChu { get; set; }
        public bool Actived { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public ICollection<CanBo> CanBos { get; set; }
        public ICollection<PhamVi> PhamVis { get; set; }
        public ICollection<LichSinhHoatChiToHoi> LichSinhHoatChiToHois { get; set; }
        public ICollection<PhatTrienDang> PhatTrienDangs { get; set; }
        public ICollection<DanhGiaToChucHoi> DanhGiaToChucHois { get; set; }
        public ICollection<ToHoi> ToHois { get; set; }
        public ICollection<ChiHoi> ChiHois { get; set; }
        public ICollection<ToHoiNganhNghe_ChiHoiNganhNghe> ToHoiNganhNghe_ChiHoiNganhNghes { get; set; }
        public DiaBanHoatDong() {
            CanBos = new List<CanBo>();
            PhamVis = new List<PhamVi>();
            LichSinhHoatChiToHois = new List<LichSinhHoatChiToHoi>();
            PhatTrienDangs = new List<PhatTrienDang>();
            DanhGiaToChucHois = new List<DanhGiaToChucHoi>();
            ToHois = new List<ToHoi>();
            ChiHois = new List<ChiHoi>();
            ToHoiNganhNghe_ChiHoiNganhNghes = new List<ToHoiNganhNghe_ChiHoiNganhNghe>();
        }
    }
}
