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

namespace HoiNongDan.Web.Areas.Permission.Controllers
{
    [Area("Permission")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        #region Login
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContext;
        public AuthController(AppDbContext db, IHttpContextAccessor httpContext) { 
            _db = db;
            _httpContext = httpContext;
        }
       
        public IActionResult Login() {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction(GetRedirectUrl(""));
            }
            string user = "";
            string pass ="";
            string sessionID = Request.Cookies["sessionID"]!;

           if (!String.IsNullOrWhiteSpace(sessionID)) {
                sessionID = Security.Decrypt(sessionID);
                user = _httpContext.HttpContext!.Session!.GetString(sessionID + "_user")!;
                pass = _httpContext.HttpContext!.Session!.GetString(sessionID + "_pass")!;
            }
            AccountLoginViewModel login = new AccountLoginViewModel();
            if (user != null)
            {
                login.UserName = user.ToString();
                login.Password = pass.ToString();
                login.RememberMe = true;
            }
            return View(login);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginViewModel model) {
            string userName = model.UserName.Trim();
            string passWord = model.Password.Trim();
            string remember = model.RememberMe.ToString().Trim();
            string sessionID = Request.Cookies["sessionID"]!;
            Account? user = _db.Accounts.Where(p => p.UserName == userName).FirstOrDefault();
           
            if (model.RememberMe == true)
            {
                if (String.IsNullOrWhiteSpace(sessionID))
                {
                    sessionID = Security.Encrypt(model.UserName.ToLower());
                }
                CookieOptions userInfo = new CookieOptions();
                userInfo.Expires = DateTime.Now.AddDays(30);
                _httpContext.HttpContext!.Session.SetString(model.UserName.ToLower() + "_user", userName);
                _httpContext.HttpContext!.Session.SetString(model.UserName.ToLower() + "_pass", passWord);
                Response.Cookies.Append("sessionID", sessionID, userInfo);
            }
            else
            {

                _httpContext.HttpContext!.Session.Remove(model.UserName.ToLower() + "_user");
                _httpContext.HttpContext!.Session.Remove(model.UserName.ToLower() + "_pass");
                //CookieOptions userInfo = new CookieOptions();
                //Response.Cookies.Append("sessionID", "", userInfo);
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
            HttpContext.Session.Remove(User.Identity!.Name + ConstExcelController.SessionMenu);
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
