using HoiNongDan.Models.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class DaoTaoBoiDuongVM
    {
        public Guid? ID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaLoaiBangCap")]
        public String MaLoaiBangCap { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHinhThucDaoTao")]
        public String MaHinhThucDaoTao { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NoiDung")]
        public String NoiDungDaoTao { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TuNgay")]
        [DataType(DataType.Date)]
        public DateTime? TuNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DenNgay")]
        [DataType(DataType.Date)]
        public DateTime? DenNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }

        public NhanSuThongTinVM NhanSu { get; set; }
    }
    public class DaoTaoBoiDuongMTVM : DaoTaoBoiDuongVM
    {
        public DaoTaoBoiDuong GetQuaTrinhDaoTao(DaoTaoBoiDuong obj)
        {
            obj.IDCanBo = this.NhanSu.IdCanbo!.Value;
            obj.MaHinhThucDaoTao = this.MaHinhThucDaoTao;
            obj.MaLoaiBangCap = this.MaLoaiBangCap;
            obj.TuNgay = this.TuNgay;
            obj.DenNgay = this.DenNgay;
            obj.NoiDungDaoTao = this.NoiDungDaoTao;
            obj.GhiChu = this.GhiChu;
            return obj;
        }
    }
}
