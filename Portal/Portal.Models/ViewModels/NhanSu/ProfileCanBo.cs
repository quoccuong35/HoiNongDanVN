using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models 
{ 
    public class ProfileCanBo
    {
        public CanBoDetailVM CanBo { get; set; }
        public List<QHGiaDinhDetail> QHGiaDinh { get; set; }
        public List<DaoTaoDetailVM> DaoTao{ get; set; }
        public List<BoiDuongDetai> BoiDuong{ get; set; }
        public List<BoNhiemDetailVM> BoNhiem{ get; set; }
        public List<KhenThuongDetailVM> KhenThuong { get; set; }
        public List<KyLuatDetailVM> KyLuat{ get; set; }
        public List<FileDinhKem> FileDinhKems { get; set; }
    }
}
