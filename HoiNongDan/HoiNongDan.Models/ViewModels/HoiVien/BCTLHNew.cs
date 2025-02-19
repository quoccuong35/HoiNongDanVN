using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models 
{ 
    public  class BCTLHNew
    {
        public int Cot1 { get; set; }
        public String Cot2 { get; set; }
        public int? Cot3 { get; set; }
        public int? Cot4 { get; set; }
        public int? Cot5 { get; set; }
        public int? Cot6 { get; set; }
        public int? Cot7 { get; set; }
        public int? Cot8 { get; set; }
        public int? Cot9 { get; set; }
        public int? Cot10 { get; set; }
        public int? Cot11 { get; set; }
        public int? Cot12 { get; set; }
        public int? Cot13 { get; set; }
        public int? Cot14 { get; set; }
        public int? Cot15 { get; set; }
        public int? Cot16 { get; set; }
        public int? Cot17 { get; set; }
        public int? Cot18 { get; set; }
        public int? Cot19 { get; set; }
        public int? Cot20 { get; set; }
        public int? Cot21 { get; set; }
        public int? Cot22 { get; set; }
        public int? Cot23 { get; set; }
        public int? Cot24 { get; set; }
        public int? Cot25 { get; set; }
        public int? Cot26 { get; set; }
       
        public object this[string propertyName]
        {
            get
            {
                // probably faster without reflection:
                // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                // instead of the following
                Type myType = typeof(BCTLHNew);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(BCTLHNew);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);
            }
        }
    }
  
    public class BCTLHNamNew
    {
        public int STT { get; set; }
        public String TenQuanHuyen { get; set; }
        public String TenDiaBanHoatDong { get; set; }
        public Guid MaDiaBanHoatDong { get; set; }
        public int Cot1 { get; set; }
        public int Cot2 { get; set; }
        public int Cot3 { get; set; }
        public int Cot4 { get; set; }
        public int Cot5 { get; set; }
        public int Cot6 { get; set; }
        public int Cot7 { get; set; }
        public int Cot8 { get; set; }
        public int Cot9 { get; set; }
        public int Cot10 { get; set; }
        public int Cot11 { get; set; }
        public int Cot12 { get; set; }
        public int Cot13 { get; set; }
        public int Cot14 { get; set; }
        public int Cot15 { get; set; }
        public int Cot16 { get; set; }
        public int Cot17 { get; set; }
        public int Cot18 { get; set; }
        public int Cot19 { get; set; }
        public int Cot20 { get; set; }
        public int Cot21 { get; set; }
        public int Cot22 { get; set; }
        public int Cot23 { get; set; }
        public int Cot24 { get; set; }
        public int Cot25 { get; set; }
        public int Cot26 { get; set; }
        public int Cot27 { get; set; }
        public int Cot28 { get; set; }
        public int Cot29 { get; set; }
        public int Cot30 { get; set; }
        public int Cot31 { get; set; }
        public int Cot32 { get; set; }
        public int Cot33 { get; set; }
        public int Cot34 { get; set; }
        public int Cot35 { get; set; }
        public int Cot36 { get; set; }
        public int Cot37 { get; set; }
        public int Cot38 { get; set; }
        public int Cot39 { get; set; }
        public int Cot40 { get; set; }
        public int Cot41 { get; set; }
        public int Cot42 { get; set; }
        public int Cot43 { get; set; }
        public int Cot44 { get; set; }
        public int Cot45 { get; set; }
        public int Cot46 { get; set; }
        public int Cot47 { get; set; }
        public int Cot48 { get; set; }
        public int Cot49 { get; set; }
        public int Cot50 { get; set; }
        public int Cot51 { get; set; }
        public int Cot52 { get; set; }
        public int Cot53 { get; set; }
        public int Cot54 { get; set; }
        public object this[string propertyName]
        {
            get
            {
                // probably faster without reflection:
                // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                // instead of the following
                Type myType = typeof(BCTLHNamNew);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(BCTLHNamNew);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);
            }
        }

    }
    public class BCTLHNewSearchVM {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "QuanHuyen")]
        public String? MaQuanHuyen { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DiaBanHoatDong")]
        public Guid? MaDiaBanHoiVien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TuNgay")]
        [DataType(DataType.Date)]
        public DateTime? TuNgay { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DenNgay")]
        [DataType(DataType.Date)]
        public DateTime? DenNgay { get; set; }
    }
}
