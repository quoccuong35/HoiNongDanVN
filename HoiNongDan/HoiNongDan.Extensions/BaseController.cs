using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HoiNongDan.DataAccess;
using HoiNongDan.Models.ViewModels.Permission;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Http;
using HoiNongDan.Constant;

namespace HoiNongDan.Extensions
{
    [Authorize]
    public class BaseController : Controller
    {
        protected AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public BaseController(AppDbContext context)
        {
            _context = context;
            _context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
        }
        public BaseController(IHttpContextAccessor httpContext) {
            _httpContext = httpContext;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {

          
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            GetMenuList();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        protected JsonResult ValidationInvalid()
        {
            var errorList = ModelState.Values.SelectMany(v => v.Errors).ToList();
            ModelState.Clear();
            string sLoi = "";
            foreach (var error in errorList)
            {
                if (string.IsNullOrEmpty(error.ErrorMessage) && error.Exception != null)
                {
                    // ModelState.AddModelError("", error.Exception.Message);
                    sLoi += error.Exception.Message + "<br>";
                }
                else
                {
                    //ModelState.AddModelError("", error.ErrorMessage);
                    sLoi += error.ErrorMessage + "<br>";
                }
            }
            return Json(new
            {
                Code = System.Net.HttpStatusCode.InternalServerError,
                Success = false,
                // Data = ModelState.Values.SelectMany(v => v.Errors).ToList()
                Data = sLoi
            }); ;
        }

        #region Execute
        protected JsonResult ExecuteContainer(Func<JsonResult> codeToExecute)
        {
            //1. using: ModelState.IsValid
            if (ModelState.IsValid)
            {
                try
                {
                    // All code will run here
                    // Usage: return ExecuteContainer(() => { ALL RUNNING CODE HERE, remember to return });
                    return codeToExecute.Invoke();
                }
                //2. handle: DbUpdateException
                catch (DbUpdateException ex)
                {
                    string sloi = "";
                    foreach (var errorMessage in ErrorHepler.GetaAllMessages(ex))
                    {
                        sloi += errorMessage + "<br/>";
                        ModelState.AddModelError("", errorMessage);
                    }
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = sloi
                    });
                }
                // handlw:DbEntityValidationException
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                                        eve.Entry.Entity.GetType().Name,
                                                        eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                        ve.PropertyName,
                                                        ve.ErrorMessage));
                        }
                    }
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = sb.ToString()
                    });
                }
                //3. handle: Exception
                catch (Exception ex)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = ex.Message
                    });
                }
            }//4. using: ValidationInvalid()
            return ValidationInvalid();
        }

        protected ActionResult ExecuteSearch(Func<PartialViewResult> codeToExecute)
        {
            try
            {
                return codeToExecute.Invoke();
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.InternalServerError,
                    Success = false,
                    Data = ex.Message
                });
            }
        }

        protected JsonResult ExecuteDelete(Func<JsonResult> codeToExecute)
        {
            try
            {
                // All code will run here
                // Usage: return ExecuteContainer(() => { ALL RUNNING CODE HERE, remember to return });
                return codeToExecute.Invoke();
            }
            //1. handle: DbUpdateException
            catch (DbUpdateException ex)
            {
                string sloi = "";
                foreach (var errorMessage in ErrorHepler.GetaAllMessages(ex))
                {
                    sloi += errorMessage;
                    ModelState.AddModelError("", errorMessage);
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = sloi
                });
            }
            //2. handle: Exception
            catch (Exception ex)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = ex.Message
                });
            }
        }
        #endregion Execute
        #region Permission
        public AppUserPrincipal CurrentUser
        {
            get
            {
                return new AppUserPrincipal(this.User as ClaimsPrincipal);
            }
        }
        #endregion

        #region Language
        #endregion Language
        protected ActionResult ExcuteImportExcel(Func<JsonResult> codeToExecute)
        {
            try
            {
                // All code will run here
                // Usage: return ExecuteContainer(() => { ALL RUNNING CODE HERE, remember to return });
                return codeToExecute.Invoke();
            }
            //1. handle: DbUpdateException
            catch (DbUpdateException ex)
            {
                string error = "";
                foreach (var errorMessage in ErrorHepler.GetaAllMessages(ex))
                {
                    error += errorMessage + "/n";
                    //ModelState.AddModelError("", errorMessage);
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = error
                });
            }
            // 2 handlw:DbEntityValidationException
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var eve in ex.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                                    eve.Entry.Entity.GetType().Name,
                                                    eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                    ve.PropertyName,
                                                    ve.ErrorMessage));
                    }
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = sb.ToString()
                });
            }
            //3. handle: Exception
            catch (Exception ex)
            {
                string Message = "";
                if (ex.InnerException != null)
                {
                    Message = ex.InnerException.Message;
                }
                else
                {
                    Message = ex.Message;
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = "Lỗi: " + Message
                });
            }
        }

        public DateTime FirstDate()
        {
            DateTime dNow = DateTime.Now;
            return new DateTime(dNow.Year, dNow.Month, 01);
        }
        public DateTime LastDate()
        {
            DateTime dNow = DateTime.Now;
            return new DateTime(dNow.Year, dNow.Month, DateTime.DaysInMonth(dNow.Year, dNow.Month));
        }
        public Guid? AccountId()
        {
            return String.IsNullOrEmpty(CurrentUser.AccountId) ? null : new Guid(CurrentUser.AccountId);
        }
        public Guid? GetEmployeeId()
        {
            return String.IsNullOrEmpty(CurrentUser.EmployeeID)?null: new Guid(CurrentUser.EmployeeID);
        }
        #region RemoveSign For Vietnamese String
        private static readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

        public static string RemoveSign4VietnameseString(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }
        #endregion RemoveSign For Vietnamese String

        #region LoadMenu
        private void GetMenuList() {
            StringBuilder sb = new StringBuilder();
            if (User.Identity!.IsAuthenticated && !string.IsNullOrWhiteSpace(CurrentUser.UserName))
            {
                if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString(CurrentUser.UserName + ConstExcelController.SessionMenu)))
                {
                    var menu = (from menu1 in _context.MenuModels
                                join permission in _context.PagePermissionModels
                                on menu1.MenuId equals permission.MenuId
                                join acc in _context.AccountInRoleModels on permission.RolesId equals acc.RolesId
                                where menu1.MenuShow == true
                                && menu1.Actived == true
                                && acc.AccountId == Guid.Parse(CurrentUser.AccountId!)
                                select menu1).Distinct().ToList();

                    /// lấy menu cha
                    /// 
                    var menucha = menu.Where(it => it.MenuType == MenuType.Menu).OrderBy(it => it.OrderIndex);
                    foreach (var item in menucha)
                    {
                        sb.AppendLine("<li class=\"slide\">");
                        sb.AppendLine("<a class='side-menu__item' data-bs-toggle='slide' href='javascript:void(0)'><i class='side-menu__icon " + item.Icon + "'></i><span class='side-menu__label'>" + item.MenuName + "</span><i class='angle fe fe-chevron-right'></i></a>");
                        // lấy menu con
                        var pages = menu.Where(it => it.MenuIdParent == item.MenuId).OrderBy(it => it.OrderIndex);
                        if (pages.Count() > 0)
                        {
                            sb.AppendLine("<ul class=\"slide-menu mega-slide-menu\">");
                            foreach (var page in pages)
                            {
                                sb.AppendLine("<li>");
                                sb.AppendLine("<a class=\"slide-item\" href='" + page.Href + "'>");
                                sb.AppendLine(page.MenuName);
                                sb.AppendLine(" </a>");
                                sb.AppendLine("</li>");
                            }
                            sb.AppendLine("</ul>");
                        }
                        sb.AppendLine("</li>");
                    }
                    HttpContext.Session.SetString(CurrentUser.UserName + ConstExcelController.SessionMenu, sb.ToString());
                }
            }
        }
        #endregion Loadmenu
        public List<Guid> GetPhamVi() {
            List<Guid> list = new List<Guid>();
            if (User.Identity!.Name.ToLower().Equals("admin"))
            {
                list = _context.DiaBanHoatDongs.Where(it => it.Actived == true).Select(it => it.Id).ToList();
            }
            else
            {
                list = _context.PhamVis.Where(it => it.AccountId == AccountId()!.Value).Select(it => it.MaDiabanHoatDong).ToList();
            }
            return list;
        }
        #region PhamVi

        #endregion PhamVi
    }
}
