using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class LichSinhHoatChiToHoi
    {
        public Guid ID { get; set; }

        public Guid IDDiaBanHoiVien { get; set; }
        public DiaBanHoatDong DiaBanHoatDong { get; set; }
        public Guid? IDMaChiToHoi { get; set; }
        public string TenNoiDungSinhHoat { get; set; }
        public DateTime Ngay { get; set; }
        public String NoiDungSinhHoat { get; set; }
        public int SoLuongNguoiThanGia { get; set; }

       
        public bool Actived { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public ICollection<LichSinhHoatChiToHoi_NguoiThamGia> LichSinhHoatChiToHoi_NguoiThamGias { get; set; }
        public LichSinhHoatChiToHoi (){
            LichSinhHoatChiToHoi_NguoiThamGias = new List<LichSinhHoatChiToHoi_NguoiThamGia>();
        }
    }
    public class LichSinhHoatChiToHoi_NguoiThamGia { 
        public Guid ID { get; set; }
        public String? MaHoiVien { get; set; }
        public String? TenHoiVien { get; set; }
        public String? ChucVu { get; set; }
        public Guid IDLichSinhHoatChiToHoi { get; set; }
        public LichSinhHoatChiToHoi LichSinhHoatChiToHoi { get; set; }

        
    }
}
