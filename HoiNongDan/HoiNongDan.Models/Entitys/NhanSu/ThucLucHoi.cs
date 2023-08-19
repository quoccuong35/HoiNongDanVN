using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class ThucLucHoi
    {
        [Key]
        public int Id { get; set; }
        public String? TenDV { get; set; }
        public String? TongAp { get; set; }
        public String? TongKhuVuc { get; set; }
        public String? Tong_HNN { get; set; }
        public String? Tong_HNN_Giam { get; set; }
        public String? Tong_HNN_HV { get; set; }
        public String? Tong_HNN_HV_Giam { get; set; }
        public String? Tong_LD_NN { get; set; }
        public String? Tong_LD_NN_Giam { get; set; }
        public String? HV_TongSo { get; set; }
        public String? HV_Nu { get; set; }
        public String? HV_PhatTrien { get; set; }
        public String? HV_Giam { get; set; }
        public String? HV_LuyKe_Tang { get; set; }
        public String? HV_LuyKe_Giam { get; set; }
        public String? HV_LuyKe_TrongKy { get; set; }
        public String? HV_NN_PhatTrien { get; set; }
        public String? HV_NN_Giam { get; set; }
        public String? HV_NN_LuyKe_Tang { get; set; }
        public String? HV_NN_LuyKe_Giam { get; set; }
        public String? HV_NN_LuyKe_TrongKy { get; set; }
        public String? HV_DanhDu_Nam { get; set; }
        public String? HV_DanhDu_LuyKe { get; set; }
        public String? Giam_HV_TongSo { get; set; }
        public String? Giam_HV_Nu { get; set; }
        public String? HN_PhatTrien_Tong { get; set; }
        public String? HN_PhatTrien_Nu { get; set; }
        
    }
}
