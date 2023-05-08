﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models.ViewModels.Permission
{
    public class AppUserPrincipal : ClaimsPrincipal
    {
        public AppUserPrincipal(ClaimsPrincipal principal)
           : base(principal)
        {

        }
        //ID người dùng đăng nhập
        public string AccountId
        {
            get
            {
                return this.FindFirst(ClaimTypes.Sid).Value;
            }
        }
        //Tên tàikhoản đăng nhập
        public string UserName
        {
            get
            {
                return this.FindFirst(ClaimTypes.Name).Value;
            }
        }
        //Họ và tên
        public string FullName
        {
            get
            {
                return this.FindFirst("FullName")!.Value;
            }
        }
        // hình đại diện
        public string Avatar
        {
            get
            {
                return this.FindFirst(ClaimTypes.Uri).Value;
            }
        }
        // get EmployeeID user login
        public string EmployeeID
        {
            get
            {
                return this.FindFirst(ClaimTypes.PrimarySid).Value;
            }
        }
        public string Roles
        {
            get
            {
                return this.UserName == null? "": this.FindFirst("Roles")!.Value;
            }
        }
    }
}
