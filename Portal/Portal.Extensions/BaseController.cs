using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal.DataAccess;
using Portal.Models.ViewModels.Permission;
using System.Security.Claims;
using System.Text;


namespace Portal.Extensions
{
    public class BaseController : Controller
    {
        protected AppDbContext _context;
        public BaseController(AppDbContext context)
        {
            _context = context;
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
                    foreach (var errorMessage in ErrorHepler.GetaAllMessages(ex))
                    {
                        ModelState.AddModelError("", errorMessage);
                    }
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = String.Join("<br/>", ModelState.Values.SelectMany(v => v.Errors))
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
                foreach (var errorMessage in ErrorHepler.GetaAllMessages(ex))
                {
                    ModelState.AddModelError("", errorMessage);
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = ModelState.Values.SelectMany(v => v.Errors)
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
    }
}
