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
        public dynamic? Cot1 { get; set; }
        
        public dynamic? Cot2 { get; set; }
        
        public dynamic? Cot3 { get; set; }
        
        public dynamic? Cot4 { get; set; }
        
        public dynamic? Cot5 { get; set; }
        
        public dynamic? Cot6 { get; set; }
        
        public dynamic? Cot7 { get; set; }
        
        public dynamic? Cot8 { get; set; }
        
        public dynamic? Cot9 { get; set; }
        
        public dynamic? Cot10 { get; set; }
        
        public dynamic? Cot11 { get; set; }
        
        public dynamic? Cot12 { get; set; }
        
        public dynamic? Cot13 { get; set; }
        
        public dynamic? Cot14 { get; set; }
        
        public dynamic? Cot15 { get; set; }
        
        public dynamic? Cot16 { get; set; }
        
        public dynamic? Cot17 { get; set; }
        
        public dynamic? Cot18 { get; set; }
        
        public dynamic? Cot19 { get; set; }
        
        public dynamic? Cot20 { get; set; }
        
        public dynamic? Cot21 { get; set; }
        
        public dynamic? Cot22 { get; set; }
        
        public dynamic? Cot23 { get; set; }
        
        public dynamic? Cot24 { get; set; }
        
        public dynamic? Cot25 { get; set; }
        
        public dynamic? Cot26 { get; set; }
        
        public dynamic? Cot27 { get; set; }
        
        public dynamic? Cot28 { get; set; }
        
        public dynamic? Cot29 { get; set; }
        
        public dynamic? Cot30 { get; set; }
        
        public dynamic? Cot31 { get; set; }
        
        public dynamic? Cot32 { get; set; }
        
        public dynamic? Cot33 { get; set; }
        
        public dynamic? Cot34 { get; set; }
        
        public dynamic? Cot35 { get; set; }
        
        public dynamic? Cot36 { get; set; }
        
        public dynamic? Cot37 { get; set; }
        
        public dynamic? Cot38 { get; set; }
        
        public dynamic? Cot39 { get; set; }
        
        public dynamic? Cot40 { get; set; }
        
        public dynamic? Cot41 { get; set; }
        
        public dynamic? Cot42 { get; set; }
        
        public dynamic? Cot43 { get; set; }
        
        public dynamic? Cot44 { get; set; }
        
        public dynamic? Cot45 { get; set; }
        
        public dynamic? Cot46 { get; set; }
        
        public dynamic? Cot47 { get; set; }
        
        public dynamic? Cot48 { get; set; }
        
        public dynamic? Cot49 { get; set; }
        
        public dynamic? Cot50 { get; set; }
        
        public dynamic? Cot51 { get; set; }
        
        public dynamic? Cot52 { get; set; }
        
        public dynamic? Cot53 { get; set; }
        
        public dynamic? Cot54 { get; set; }
        
        public dynamic? Cot55 { get; set; }
        
        public dynamic? Cot56 { get; set; }
        
        public dynamic? Cot57 { get; set; }
        
        public dynamic? Cot58 { get; set; }
        
        public dynamic? Cot59 { get; set; }
        
        public dynamic? Cot60 { get; set; }
        
        public dynamic? Cot61 { get; set; }
        
        public dynamic? Cot62 { get; set; }
        
        public dynamic? Cot63 { get; set; }
        
        public dynamic? Cot64 { get; set; }
        
        public dynamic? Cot65 { get; set; }
        
        public dynamic? Cot66 { get; set; }
        
        public dynamic? Cot67 { get; set; }
        
        public dynamic? Cot68 { get; set; }
        
        public dynamic? Cot69 { get; set; }
        
        public dynamic? Cot70 { get; set; }
        
        public dynamic? Cot71 { get; set; }
        
        public dynamic? Cot72 { get; set; }
        
        public dynamic? Cot73 { get; set; }
        
        public dynamic? Cot74 { get; set; }
        
        public dynamic? Cot75 { get; set; }
        
        public dynamic? Cot76 { get; set; }
        
        public dynamic? Cot77 { get; set; }
        
        public dynamic? Cot78 { get; set; }
        
        public dynamic? Cot79 { get; set; }
        
        public dynamic? Cot80 { get; set; }
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
