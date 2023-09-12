﻿using HoiNongDan.DataAccess;
using HoiNongDan.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HoiNongDan.Extensions
{
    public static class Function
    {
        public static bool GetPermission(String listRoless, String Roles)
        {
            if (listRoless.ToUpper().Contains(Roles.ToUpper().Trim()))
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
