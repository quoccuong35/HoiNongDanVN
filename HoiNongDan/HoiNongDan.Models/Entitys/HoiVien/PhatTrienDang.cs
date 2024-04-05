using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys
{
    public class PhatTrienDang
    {
        public Guid ID { get; set; }
        public String TenVietTat { get; set; }
        public int Nam { get; set; }
        public int SoLuong { get; set; }
        public string NoiDung { get; set; }
        public Guid MaDiaBanHoiND { get; set; }
        public DiaBanHoatDong DiaBanHoatDong { get; set; }
        public String? GhiChu { get; set; }
        public bool Actived { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public virtual ICollection<PhatTrienDang_HoiVien> PhatTrienDang_HoiViens { get; set; }
        public PhatTrienDang() {
            PhatTrienDang_HoiViens = new List<PhatTrienDang_HoiVien>();
        }
    }
    public class PhatTrienDang_HoiVien { 
        public Guid IDHoiVien { get; set; }
        public CanBo CanBo { get; set; }
        public Guid IDPhatTrienDang { get; set; }
        public PhatTrienDang PhatTrienDang { get; set; }
    }
}
