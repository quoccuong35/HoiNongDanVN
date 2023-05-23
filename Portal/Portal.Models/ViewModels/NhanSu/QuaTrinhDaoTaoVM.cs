using Portal.Models.Entitys.MasterData;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Portal.Models
{
    public class QuaTrinhDaoTaoVM
    {
        public Guid? IDQuaTrinhDaoTao { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CoSoDaoTao")]
        public String CoSoDaoTao { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayTotNghiep")]
        [DataType(DataType.Date)]
        public DateTime? NgayTotNghiep { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuocGia")]
        public String QuocGia { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaLoaiBangCap")]
        public String MaLoaiBangCap { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaHinhThucDaoTao")]
        public String MaHinhThucDaoTao { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaChuyenNganh")]
        public String MaChuyenNganh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LuanAnTN")]
        public bool LuanAnTN { get; set; } = false;

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FileDinhKem")]
        public String? FileDinhKem { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GhiChu")]
        public String? GhiChu { get; set; }
        public NhanSuThongTinVM NhanSu { get; set; }
    }
    public class QuaTrinhDaoTaoMTVM :QuaTrinhDaoTaoVM{
        public QuaTrinhDaoTao GetQuaTrinhDaoTao(QuaTrinhDaoTao obj) {
            obj.IDCanBo = this.NhanSu.IdCanbo!.Value;
            obj.CoSoDaoTao = this.CoSoDaoTao;
            obj.MaHinhThucDaoTao = this.MaHinhThucDaoTao;
            obj.MaChuyenNganh = this.MaChuyenNganh;
            obj.MaLoaiBangCap = this.MaLoaiBangCap;
            obj.QuocGia = this.QuocGia;
            obj.NgayTotNghiep = this.NgayTotNghiep;
            obj.LuanAnTN = this.LuanAnTN;
            obj.GhiChu = this.GhiChu;

            return obj;
        }
    }
}
