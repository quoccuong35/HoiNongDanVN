using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using HoiNongDan.DataAccess;
using HoiNongDan.Models;
using HoiNongDan.Extensions;
using HoiNongDan.Resources;
using System.Security.Claims;
using HoiNongDan.Models.Entitys;

namespace HoiNongDan.Web.Areas.Permission.Controllers
{
    [Area("Permission")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        #region Login
        private readonly AppDbContext _db;
        public AuthController(AppDbContext db) { 
            _db = db;
        }
        public IActionResult Login() {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction(GetRedirectUrl(""));
            }
            string user = Request.Cookies["username"];
            string pass = Request.Cookies["password"];
            bool remember = Request.Cookies["remember"] == null?false:bool.Parse(Request.Cookies["remember"]);
            AccountLoginViewModel login = new AccountLoginViewModel();
            if (user != null)
            {
                login.UserName = user.ToString();
                login.Password = pass == null ? "" : pass.ToString();
                login.RememberMe = remember;
            }
            return View(login);
        }
        [HttpPost]

        public async Task<IActionResult> Login(AccountLoginViewModel model) {
            string userName = model.UserName.Trim();
            string passWord = model.Password.Trim();
            string remember = model.RememberMe.ToString().Trim();
            Account? user = _db.Accounts.Where(p => p.UserName == userName).FirstOrDefault();
            if (model.RememberMe == true)
            {
                CookieOptions userInfo = new CookieOptions();
                userInfo.Expires = DateTime.Now.AddDays(10);
                Response.Cookies.Append("username", model.UserName, userInfo);
                Response.Cookies.Append("password", model.Password, userInfo);
                Response.Cookies.Append("remember", model.RememberMe.ToString(), userInfo);
            }
            else
            {
                CookieOptions userInfo = new CookieOptions();
                Response.Cookies.Append("username", "", userInfo);
                Response.Cookies.Append("password", "", userInfo);
                Response.Cookies.Append("remember", "false", userInfo);
            }
            try
            {
                 passWord = RepositoryLibrary.GetMd5Sum(passWord);
                if (user != null)
                {

                    if (user.Actived != true)
                    {
                        string errorMessage = LanguageResource.Account_Locked;
                        ModelState.AddModelError("", errorMessage);
                        return View(model);
                    }
                    else
                    {
                        if (user.Password == passWord)
                        {
                            List<Claim> claims = new List<Claim>() {
                                new Claim(ClaimTypes.Name,user.UserName),
                                new Claim(ClaimTypes.Sid,user.AccountId.ToString()),
                                 new Claim("FullName",user.FullName.ToString()),
                                new Claim(ClaimTypes.PrimarySid,user!.EmployeeId==null?"":user!.EmployeeId.ToString()),
                                new Claim("Id",user.AccountId.ToString()),
                                new Claim("Roles",PagePermission(user.AccountId))
                               
                            };
                            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                                CookieAuthenticationDefaults.AuthenticationScheme);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                             new ClaimsPrincipal(claimsIdentity));
                            if (!string.IsNullOrEmpty(model.ReturnUrl))
                            {
                                return Redirect(GetRedirectUrl(model.ReturnUrl));
                            }
                            else
                            {
                                //return RedirectToAction("Index", "Home", null);
                                return Redirect("~/Home/Index");
                            }
                        }
                        else
                        {
                            string errorMessage = LanguageResource.Account_Confirm;
                            ModelState.AddModelError("", errorMessage);
                            return View(model);
                        }
                    }
                }
                else
                {
                    string errorMessage = LanguageResource.Account_Confirm;
                    ModelState.AddModelError("", errorMessage);
                }
                
               
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            return View(model);

        }
        #endregion
        #region GetRedirectUrl
        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Index", "Home");
            }

            return returnUrl;
        }
        #endregion GetRedirectUrl


        #region Logout
        public async Task<IActionResult> LogOut()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        private String PagePermission(Guid accId) {
            string kq = "";
            var roles =( from acc in _db.AccountInRoleModels
                        join pagerole in _db.PagePermissionModels on acc.RolesId equals pagerole.RolesId
                        join page in _db.MenuModels on pagerole.MenuId equals page.MenuId
                        where acc.AccountId == accId
                        orderby page.MenuCode
                        select new {Quyen = page.MenuCode.ToLower() +":"+ pagerole.FunctionId.ToLower() }).ToList();
            if (roles.Count > 0)
            {
                kq = String.Join(';', roles.Select(it=>it.Quyen));
            }
            
            return kq ;

        }
        #endregion
    }
}
