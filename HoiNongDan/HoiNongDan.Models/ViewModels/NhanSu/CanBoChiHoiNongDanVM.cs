using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models 
{ 
    public class CanBoChiHoiNongDanVM
    {
        public Guid? IDCanBo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public String HoVaTen { get; set; }

        [Display(Name = "Chức vụ (ấp/khu phố, xã/phường)")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public String DonVi { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required_Dropdownlist")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GioiTinh")]
        public GioiTinh GioiTinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgaySinh")]
        public String? NgaySinh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DanToc")]
        public String? MaDanToc { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TonGiao")]
        public String? MaTonGiao { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChoOHienNay")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ChoOHienNay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayvaoDangDuBi")]
        public String? NgayVaoDangDuBi { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "NgayVaoDangChinhThuc")]
        public String? NgayVaoDangChinhThuc { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoHocVan")]
        public String? MaTrinhDoHocVan { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ChuyenNganh")]
        public String? ChuyenNganh { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TrinhDoChinhTri")]
        public String? MaTrinhDoChinhTri { get; set; }

        [Display(Name = "Thời gian công tác Hội")]
        public String? ChiHoiDanCu_CHT { get; set; }

        [Display(Name = "Tham gia BCH xã")]
        public String? ChiHoiDanCu_CHP { get; set; }
        public string Level { get; set; } = "40";
    }
    public class CanBoChiHoiNongDanDetailVM
    {
        public int STT { get; set; }
        public Guid? IDCanBo { get; set; }


        public String? HoVaTen { get; set; }

        public String? DonVi { get; set; }

        public string? NgaySinh_Nam { get; set; }
        public string? NgaySinh_Nu { get; set; }

        public String? MaDanToc { get; set; }

        public String? MaTonGiao { get; set; }


        public string ChoOHienNay { get; set; }


        public String? NgayVaoDangDuBi { get; set; }


        public String? NgayVaoDangChinhThuc { get; set; }


        public String? MaTrinhDoHocVan { get; set; }


        public String? ChuyenNganh { get; set; }


        public String? MaTrinhDoChinhChi { get; set; }

        [Display(Name = "Thời gian công tác Hội")]
        public String? ChiHoiDanCu_CHT { get; set; }

        [Display(Name = "Tham gia BCH xã")]
        public String? ChiHoiDanCu_CHP { get; set; }
    }
    public class CanBoChiHoiNongDanExelVM :CanBoChiHoiNongDanDetailVM {
        public int RowIndex { get; set; }
        public bool isNullValueId { get; set; }
        public string Error { get; set; }
        public CanBo CanBo { get; set; }
    }
}
