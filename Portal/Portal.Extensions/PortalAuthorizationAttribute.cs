using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Portal.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Extensions
{
    public class PortalAuthorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var descriptor = context.ActionDescriptor;
            //string actionName = descriptor.RouteValues["action"];
            //string controllerName = context.ActionDescriptor.RouteValues["controller"];
            string roles = context.ActionDescriptor.RouteValues["controller"] + ":"+descriptor.RouteValues["action"];
            //string areaName = "";
            var controller = context.Controller as BaseController;
            var listRoles = controller.CurrentUser.Roles;

            //if (context.RouteData.DataTokens["area"] != null)
            //{
            //    areaName = context.RouteData.DataTokens["area"].ToString();
            //}
            if (!Function.GetPermission(listRoles, roles))
            {
                context.Result = new RedirectResult("/Error/AccessDenied");
            }
            else
            {
                base.OnActionExecuting(context);
            }
          
            
        }
    }
}
