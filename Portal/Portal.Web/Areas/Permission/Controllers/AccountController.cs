using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Portal.Constant;
using Portal.DataAccess;
using Portal.DataAccess.Repository;
using Portal.Extensions;
using Portal.Models;
using Portal.Models.Entitys;
using Portal.Resources;
using System.Data.Entity;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace Portal.Web.Areas.Permission.Controllers
{
    [Area(ConstArea.Permission)]
    [Authorize]
    public class AccountController : BaseController
    {
        public AccountController(AppDbContext context) : base(context) { }
        #region Index
        public IActionResult Index()
        {
            return View(new UserSearchVM());
        }
        public IActionResult _Search(UserSearchVM userSearch)
        {
            return ExecuteSearch(() => {
                var data = _context.Accounts.AsQueryable();
                if (!String.IsNullOrEmpty(userSearch.UserName))
                {
                    data = data.Where(it => it.UserName.Contains(userSearch.UserName));
                }
                if (userSearch.Actived != null)
                {
                    data = data.Where(it => it.Actived == userSearch.Actived);
                }
                var accounts = data.ToList().Select(it => new UserVM
                {
                    AccountId = it.AccountId,
                    UserName = it.UserName,
                    FullName = it.FullName,
                    Password = it.Password,
                    Actived = it.Actived
                }); 
                //account.userRoless = new  
                return PartialView(accounts);
            });
        }
        #endregion Index
        #region Upsert
        [HttpGet]
        [PortalAuthorization]
        public IActionResult Upsert(Guid? id)
        {

            AccountVM accout = new AccountVM();
            List<UserRoles> userRoles = _context.RolesModels.Where(it => it.Actived == true).Select(it => new UserRoles
            {
                RolesId = it.RolesId,
                RolesName = it.RolesName,
                Selected = false,
            }).ToList();

            if (id != null)
            {
                var data = _context.Accounts.SingleOrDefault(it => it.AccountId == id);
                if (data != null)
                {
                    accout.AccountId = data.AccountId;
                    accout.UserName = data.UserName;
                    accout.Actived = data.Actived;
                    accout.Password = data.Password;
                    accout.FullName = data.FullName;
                    var rolesSelected = _context.AccountInRoleModels.Where(it => it.AccountId == data.AccountId).ToList();
                    foreach (var item in rolesSelected)
                    {
                        var exist = userRoles.FirstOrDefault(it => it.RolesId == item.RolesId);
                        if (exist != null)
                        {
                            exist.Selected = true;
                        }
                    }

                }
            }
            accout.userRoless = userRoles;
            return View(accout);
        }
        [HttpPost]
        [PortalAuthorization]
        public JsonResult Upsert(AccountVM obj) {
            if (AccountId() == null)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NetworkAuthenticationRequired,
                    Success = false,
                    Data = string.Format("Hết thời gian sử dụng vui lòng đăng nhập lại")
                });
            }
            return ExecuteContainer(() => {
                if (obj.AccountId == null)
                {
                    Account insertAcc = obj.Add(AccountId()!.Value);
                    if (insertAcc.Password != null)
                    { 
                        insertAcc.Password = RepositoryLibrary.GetMd5Sum(insertAcc.Password);
                    }
                    //Object insert = new object
                    _context.Attach(insertAcc).State = EntityState.Modified;
                    _context.Accounts.Add(insertAcc);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Account.ToLower())
                    });
                }
                else
                {
                    Account? editAcc = _context.Accounts.FirstOrDefault(x => x.AccountId == obj.AccountId);
                    if (editAcc == null)
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Không tìm thấy thông tin co ma " + obj.AccountId
                        });
                    }
                    else
                    {
                        editAcc = obj.Update(AccountId()!.Value, editAcc);

                        HistoryModelRepository history = new HistoryModelRepository(_context);
                        history.SaveUpdateHistory(editAcc.AccountId.ToString(), AccountId()!.Value, editAcc);
                        //_context.Entry(editAcc).State = EntityState.Modified;
                        //Delete Roles accout old
                        var delAccountInRoleModels = _context.AccountInRoleModels.Where(it => it.AccountId == obj.AccountId).ToList();
                        if (delAccountInRoleModels.Count > 0)
                        {
                            _context.RemoveRange(delAccountInRoleModels);
                        }
                        _context.Accounts.Update(editAcc);
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Account.ToLower())
                        });
                    }

                }
            });
       }
        #endregion Upsert
        #region Delete
        [HttpDelete]
        public JsonResult Delete(Guid id) {
            return ExecuteDelete(() =>
            {
                var account = _context.Accounts.FirstOrDefault(p => p.AccountId == id);
                var accountInRoleModels = _context.AccountInRoleModels.Where(it => it.AccountId == id).ToList();
                
                if (account != null)
                {
                    //_context.Entry(accountInRoleModels).State = EntityState.Deleted;
                    //_context.Entry(account).State = EntityState.Deleted;
                    _context.RemoveRange(accountInRoleModels);
                    _context.Remove(account);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Account.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.Account.ToLower())
                    });
                }
            });
        }
        #endregion Delete
    }
}
