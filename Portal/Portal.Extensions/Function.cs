using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Portal.Extensions
{
    public static class Function
    {
        public static bool GetPermission(String listRoless, String Roles)
        {
            if (listRoless.ToUpper().Contains(Roles.ToUpper()))
            {
                return true;
            }
            else
            {
                return false; ;
            }
        }
    }
}
