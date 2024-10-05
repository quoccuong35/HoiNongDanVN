using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HoiNongDan.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Models.Entitys;

namespace HoiNongDan.Extensions
{
    public class HoiNongDanAuthorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if(!context.HttpContext.User.Identity!.IsAuthenticated) {
                context.Result = new RedirectResult("/Permission/Auth/LogOut");
            }
            var name = context.HttpContext.User.Identity.Name!.ToLower();
            var active = context.HttpContext.Session.GetString(name);
            if (String.IsNullOrWhiteSpace(active))
            {
                context.Result = new RedirectResult("/Permission/Auth/LogOut");
            }

            var descriptor = context.ActionDescriptor;
            string actionName = RenameAction( descriptor.RouteValues["action"]);
           
            string roles = context.ActionDescriptor.RouteValues["controller"] + ":"+ actionName;
            //string areaName = "";
            var controller = context.Controller as BaseController;
            var listRoles = controller!.CurrentUser.Roles;

            if (!Function.GetPermission(listRoles!, roles))
            {
                context.Result = new RedirectResult("/Error/AccessDenied");
            }
            else
            {
                base.OnActionExecuting(context);
            }
          
            
        }
        private string RenameAction(string name) { 
            switch(name.ToLower())
            {
                case "_search":
                    return "Index";
                case "_import":
                    return "Import";
                case "exportcreate":
                    return "Export";
                case "exportedit":
                    return "Export";
                case "doimatkhau":
                    return "Edit";
                case "tuchoi":
                    return "Edit";
                default: 
                    return name;

            }
        }
    }
}
