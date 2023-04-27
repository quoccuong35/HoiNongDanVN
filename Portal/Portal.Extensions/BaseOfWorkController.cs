using Portal.DataAccess;
using Portal.Models.ViewModels.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Extensions
{
    public class BaseOfWorkController : Microsoft.AspNetCore.Mvc.Controller
    {
        protected IUnitOfWork _unitOfWork;
        public BaseOfWorkController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #region Permission
        public AppUserPrincipal CurrentUser
        {
            get
            {
                return new AppUserPrincipal(this.User as ClaimsPrincipal);
            }
        }
        #endregion
    }
}
