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
using NuGet.Protocol.Core.Types;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using HoiNongDan.Constant;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;

namespace HoiNongDan.Web.Areas.Permission.Controllers
{
    [Area("Permission")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        #region Login
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _config;
        public AuthController(AppDbContext db, IHttpContextAccessor httpContext, IConfiguration config) { 
            _db = db;
            _httpContext = httpContext;
            _config = config;
        }
       
        public IActionResult Login() {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity!.IsAuthenticated)
            {
                return RedirectToAction(GetRedirectUrl(""));
            }
            string user = "";
            string pass ="";
            string sessionID = Request.Cookies[ConstOther.sessionID]!;
           if (!String.IsNullOrWhiteSpace(sessionID)) {
                user = _httpContext.HttpContext!.Session.GetString(sessionID + "_user")!;
                pass = _httpContext.HttpContext!.Session!.GetString(sessionID + "_pass")!;
            }
          
            AccountLoginViewModel login = new AccountLoginViewModel();
            if (!String.IsNullOrWhiteSpace(user))
            {
                login.UserName = user.ToString();
                login.Password = pass.ToString();
                login.RememberMe = true;
            }
            CookieOptions userInfo = new CookieOptions();
            userInfo.Expires = DateTime.Now.AddDays(-1);
            if (!String.IsNullOrWhiteSpace(Request.Cookies["username"])) {
               
                Response.Cookies.Append("username", "", userInfo);
            }
            if (!String.IsNullOrWhiteSpace(Request.Cookies["password"]))
            {

                Response.Cookies.Append("password", "", userInfo);
            }
            if (!String.IsNullOrWhiteSpace(Request.Cookies["remember"]))
            {

                Response.Cookies.Append("remember", "", userInfo);
            }
            return View(login);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginViewModel model) {
            string userName = model.UserName.Trim();
            string passWord = model.Password.Trim();
            string remember = model.RememberMe.ToString().Trim();
            string sessionID = Request.Cookies[ConstOther.sessionID]!;
            if (!String.IsNullOrWhiteSpace(sessionID))
            {

                HttpContext.Session.Remove(sessionID + "_user");
                HttpContext.Session.Remove(sessionID + "_pass");
                CookieOptions userInfo = new CookieOptions();
                userInfo.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append(ConstOther.sessionID, "", userInfo);
            }
            if (model.RememberMe == true)
            {
                sessionID = Guid.NewGuid().ToString().ToLower();
                CookieOptions userInfo = new CookieOptions();
                userInfo.Expires = DateTime.Now.AddDays(30);
                _httpContext.HttpContext!.Session.SetString(sessionID + "_user", userName);
                _httpContext.HttpContext!.Session.SetString(sessionID + "_pass", passWord);
                Response.Cookies.Append(ConstOther.sessionID, sessionID, userInfo);
            }
            try
            {
                Account? user = _db.Accounts.Where(p => p.UserName == userName).FirstOrDefault();
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
                        int timoutCookie = int.Parse(_config.GetSection("SiteSettings:TimeOutCookie").Value.ToString());
                        if (user.Password == passWord)
                        {
                            
                            // add chug thuc
                            List<Claim> claims = new List<Claim>() {
                                new Claim(ClaimTypes.Name,user.UserName),
                                new Claim(ClaimTypes.Sid,user.AccountId.ToString()),
                                new Claim("FullName",user.FullName.ToString()),
                                new Claim(ClaimTypes.PrimarySid,user!.EmployeeId==null?"":user!.EmployeeId.ToString()),
                                new Claim("Id",user.AccountId.ToString()),
                                new Claim("Roles",PagePermission(user.AccountId))
                               
                            };
                            var authenticationProperties = new AuthenticationProperties {
                                ExpiresUtc = DateTime.UtcNow.AddMinutes(timoutCookie),
                                IsPersistent = true,
                                AllowRefresh = true
                            };
                            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                                CookieAuthenticationDefaults.AuthenticationScheme);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                             new ClaimsPrincipal(claimsIdentity), authenticationProperties);

                            HttpContext!.Session.SetString(model.UserName.ToLower(), "1");
                            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
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
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            CookieOptions userInfo = new CookieOptions();
            userInfo.Expires = DateTime.Now.AddDays(-1);
            foreach (var cookie in Request.Cookies.Keys)
            {
                if (cookie != ConstOther.sessionID && !cookie.Contains("AspNetCore.Antiforgery"))
                {
                    Response.Cookies.Delete(cookie);
                    Response.Cookies.Append(cookie, "", userInfo);
                }
            }
            HttpContext.Session.Remove(User.Identity!.Name + ConstOther.SessionMenu);
            var authenticationProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddDays(-1),
            };
            HttpContext.Session.Remove(User.Identity.Name!.ToLower());
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
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
