using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models
{
    public class BaoCaoThucLucHoiVM
    {
        public int STT { get; set; }
        public String? Cot1 { get; set; }
        
        public decimal? Cot2 { get; set; }
        
        public decimal? Cot3 { get; set; }
        
        public decimal? Cot4 { get; set; }
        
        public decimal? Cot5 { get; set; }
        
        public decimal? Cot6 { get; set; }
        
        public decimal? Cot7 { get; set; }
        
        public decimal? Cot8 { get; set; }
        
        public decimal? Cot9 { get; set; }
        
        public decimal? Cot10 { get; set; }
        
        public decimal? Cot11 { get; set; }
        
        public decimal? Cot12 { get; set; }
        
        public decimal? Cot13 { get; set; }
        
        public decimal? Cot14 { get; set; }
        
        public decimal? Cot15 { get; set; }
        
        public decimal? Cot16 { get; set; }
        
        public decimal? Cot17 { get; set; }
        
        public decimal? Cot18 { get; set; }
        
        public decimal? Cot19 { get; set; }
        
        public decimal? Cot20 { get; set; }
        
        public decimal? Cot21 { get; set; }
        
        public decimal? Cot22 { get; set; }
        
        public decimal? Cot23 { get; set; }
        
        public decimal? Cot24 { get; set; }
        
        public decimal? Cot25 { get; set; }
        
        public decimal? Cot26 { get; set; }
        
        public decimal? Cot27 { get; set; }
        
        public int? Cot28 { get; set; }
        
        public String? Cot29 { get; set; }
        
        public decimal? Cot30 { get; set; }
        
        public decimal? Cot31 { get; set; }
        
        public decimal? Cot32 { get; set; }
        
        public decimal? Cot33 { get; set; }
        
        public decimal? Cot34 { get; set; }
        
        public decimal? Cot35 { get; set; }
        
        public decimal? Cot36 { get; set; }
        
        public decimal? Cot37 { get; set; }
        
        public decimal? Cot38 { get; set; }
        
        public decimal? Cot39 { get; set; }
        
        public decimal? Cot40 { get; set; }
        
        public decimal? Cot41 { get; set; }
        
        public decimal? Cot42 { get; set; }
        
        public decimal? Cot43 { get; set; }
        
        public decimal? Cot44 { get; set; }
        
        public decimal? Cot45 { get; set; }
        
        public decimal? Cot46 { get; set; }
        
        public decimal? Cot47 { get; set; }
        
        public decimal? Cot48 { get; set; }
        
        public decimal? Cot49 { get; set; }
        
        public decimal? Cot50 { get; set; }
        
        public decimal? Cot51 { get; set; }
        
        public decimal? Cot52 { get; set; }
        
        public decimal? Cot53 { get; set; }
        
        public decimal? Cot54 { get; set; }
        
        public decimal? Cot55 { get; set; }
        
        public int? Cot56 { get; set; }
        
        public String? Cot57 { get; set; }
        
        public decimal? Cot58 { get; set; }
        
        public decimal? Cot59 { get; set; }
        
        public decimal? Cot60 { get; set; }
        
        public decimal? Cot61 { get; set; }
        
        public decimal? Cot62 { get; set; }
        
        public decimal? Cot63 { get; set; }
        
        public decimal? Cot64 { get; set; }
        
        public decimal? Cot65 { get; set; }
        
        public decimal? Cot66 { get; set; }
        
        public decimal? Cot67 { get; set; }
        
        public decimal? Cot68 { get; set; }
        
        public decimal? Cot69 { get; set; }
        
        public decimal? Cot70 { get; set; }
        
        public decimal? Cot71 { get; set; }
        
        public decimal? Cot72 { get; set; }
        
        public decimal? Cot73 { get; set; }
        
        public decimal? Cot74 { get; set; }
        
        public decimal? Cot75 { get; set; }
        
        public decimal? Cot76 { get; set; }
        
        public decimal? Cot77 { get; set; }
        
        public decimal? Cot78 { get; set; }
        
        public decimal? Cot79 { get; set; }
        
        public decimal? Cot80 { get; set; }
        public object this[string propertyName]
        {
            get
            {
                // probably faster without reflection:
                // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                // instead of the following
                Type myType = typeof(BaoCaoThucLucHoiVM);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(BaoCaoThucLucHoiVM);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);
            }
        }
    }
}
