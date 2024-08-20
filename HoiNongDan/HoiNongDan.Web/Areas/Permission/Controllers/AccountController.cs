using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Resources;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;
using System.Text.RegularExpressions;
using System.Windows.Markup;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HoiNongDan.Web.Areas.Permission.Controllers
{
    [Area(ConstArea.Permission)]
    public class AccountController : BaseController
    {
        public AccountController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View(new UserSearchVM());
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(UserSearchVM userSearch)
        {

            return ExecuteSearch(() => {
                var data = _context.Accounts.Include(it => it.AccountInRoleModels).AsQueryable();
                if (!String.IsNullOrEmpty(userSearch.UserName))
                {
                    data = data.Where(it => it.UserName.Contains(userSearch.UserName));
                }
                if (userSearch.Actived != null)
                {
                    data = data.Where(it => it.Actived == userSearch.Actived);
                }
                if (!User.Identity.Name.ToLower().Equals("admin"))
                {
                    // get nhom quyen
                    Guid? accCountID = AccountId();
                    // khac quen add min 
                    data = data.Where(it => it.CreatedAccountId == accCountID);
                }
                var accounts = data.ToList().Select(it => new UserVM
                {
                    AccountId = it.AccountId,
                    UserName = it.UserName,
                    FullName = it.FullName,
                    Password = it.Password,
                    Actived = it.Actived
                }).Distinct();
                //account.userRoless = new  
                return PartialView(accounts);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create() {
            AccountVM accout = new AccountVM();
            var roles = _context.RolesModels.Where(it => it.Actived == true).ToList();
            List<AccountDiaBan> diaBans = _context.DiaBanHoatDongs.Include(it => it.QuanHuyen).Where(it => it.Actived == true).Select(it => new AccountDiaBan
            {
                MaDiaBanHoiVien = it.Id,
                TenDiaBanHoiVien = it.TenDiaBanHoatDong,
                Selected = false,
                MaQuanHuyen = it.MaQuanHuyen,
                TenQuanHuyen = it.QuanHuyen.TenQuanHuyen,
            }).OrderBy(it => it.TenQuanHuyen).ToList();
            if (!User.Identity!.Name!.ToLower().Equals("admin")) {
                var listRole = _context.AccountInRoleModels.Where(it => it.AccountId == AccountId()).Select(it => it.RolesId).ToList();
                roles = roles.Where(it => listRole.Contains(it.RolesId) || it.CreatedAccountId == AccountId()).ToList();
                // Đia bàn hoạt động
                var listDiaBan = _context.PhamVis.Where(it => it.AccountId == AccountId()).Select(it => it.MaDiabanHoatDong).ToList();
                diaBans = diaBans.Where(it => listDiaBan.Contains(it.MaDiaBanHoiVien)).ToList();
            }
            List<UserRoles> userRoles = roles.Select(it => new UserRoles
            {
                RolesId = it.RolesId,
                RolesName = it.RolesName,
                Selected = false,
            }).ToList();
            accout.userRoless = userRoles;
            accout.DiaBans = diaBans;
            ViewBagHelper();
            return View(accout);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HoiNongDanAuthorization]
        public IActionResult Create(AccountVM obj) {
            if (CheckExistAccountName(obj.UserName))
            {
                ModelState.AddModelError("UserName", "Tài khoản đã tồn tại. Không thể thêm");
            }
            return ExecuteContainer(() => {
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
            });
        }
        #endregion Create

        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
           
            AccountEditVM accout = new AccountEditVM();

            var roles = _context.RolesModels.Where(it => it.Actived == true).ToList();
            var data = _context.Accounts.SingleOrDefault(it => it.AccountId == id );
            List<AccountDiaBan> diaBans = _context.DiaBanHoatDongs.Include(it => it.QuanHuyen).Where(it => it.Actived == true).Select(it => new AccountDiaBan
            {
                MaDiaBanHoiVien = it.Id,
                TenDiaBanHoiVien = it.TenDiaBanHoatDong,
                Selected = false,
                MaQuanHuyen = it.MaQuanHuyen,
                TenQuanHuyen = it.QuanHuyen.TenQuanHuyen,
            }).OrderBy(it => it.TenQuanHuyen).ToList();
            if (!User.Identity!.Name!.ToLower().Equals("admin"))
            {
                Guid? accID = AccountId();
                var listRole = _context.AccountInRoleModels.Where(it => it.AccountId == AccountId()).Select(it => it.RolesId).ToList();
                roles = roles.Where(it => listRole.Contains(it.RolesId) || it.CreatedAccountId == AccountId()).ToList();
                // Đia bàn hoạt động
                var listDiaBan = _context.PhamVis.Where(it => it.AccountId == AccountId()).Select(it => it.MaDiabanHoatDong).ToList();
                diaBans = diaBans.Where(it => listDiaBan.Contains(it.MaDiaBanHoiVien)).ToList();
                data = _context.Accounts.SingleOrDefault(it => it.AccountId == id && it.CreatedAccountId == accID);
            }
            List<UserRoles>  userRoles = roles.Select(it => new UserRoles
            {
                RolesId = it.RolesId,
                RolesName = it.RolesName,
                Selected = false,
            }).ToList();
          
            if (data != null)
            {

                accout.AccountId = data.AccountId;
                accout.UserName = data.UserName;
                accout.Actived = data.Actived;
                accout.FullName = data.FullName;
                if (!String.IsNullOrWhiteSpace(data.AccountIDParent))
                {
                    string[] idAc = data.AccountIDParent!.Split(";");
                    foreach (var item in idAc)
                    {
                        accout.AccountIDParent!.Add(Guid.Parse(item));
                    }
                }
                var rolesSelected = _context.AccountInRoleModels.Where(it => it.AccountId == data.AccountId).ToList();
                foreach (var item in rolesSelected)
                {
                    var exist = userRoles.FirstOrDefault(it => it.RolesId == item.RolesId);
                    if (exist != null)
                    {
                        exist.Selected = true;
                    }
                }
                var phamVis = _context.PhamVis.Where(it => it.AccountId == id).ToList();
                foreach (var item in phamVis)
                {
                    var exist = diaBans.SingleOrDefault(it => it.MaDiaBanHoiVien == item.MaDiabanHoatDong);
                    if (exist != null)
                    {
                        exist.Selected = true;
                    }
                }
                accout.userRoless = userRoles;
                accout.DiaBans = diaBans;
                ViewBagHelper();
                //return Json(accout);
                return View(accout);
            }
            else
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
         
        }
        #endregion Edit
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AccountEditVM obj)
        {
           
            return ExecuteContainer(() => {
                Account? editAcc = _context.Accounts.FirstOrDefault(x => x.AccountId == obj.AccountId);
                if (!User.Identity!.Name!.ToLower().Equals("admin"))
                {
                    editAcc = _context.Accounts.SingleOrDefault(it => it.AccountId == obj.AccountId && it.CreatedAccountId == AccountId());
                }
                if (editAcc == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = "Không tìm thấy thông tin người dùng"
                    });
                }
                else
                {
                    editAcc.Actived = obj.Actived;
                    editAcc.FullName = obj.FullName;
                    if (obj.AccountIDParent!.Count()>0)
                    {
                        editAcc.AccountIDParent = String.Join(";", obj.AccountIDParent!);
                    }
                    if (!String.IsNullOrWhiteSpace(obj.PasswordNew))
                    {
                        editAcc.Password = RepositoryLibrary.GetMd5Sum(obj.PasswordNew);
                    }
                    HistoryModelRepository history = new HistoryModelRepository(_context);
                    history.SaveUpdateHistory(editAcc.AccountId.ToString(), AccountId()!.Value, editAcc);
                    var delAccountInRoleModels = _context.AccountInRoleModels.Where(it => it.AccountId == obj.AccountId).ToList();
                    if (delAccountInRoleModels.Count > 0)
                    {
                        _context.RemoveRange(delAccountInRoleModels);
                    }
                    // Xóa pham vi
                    var delPhamVis = _context.PhamVis.Where(it => it.AccountId == obj.AccountId).ToList();
                    if (delPhamVis.Count > 0)
                    {
                        _context.RemoveRange(delPhamVis);
                    }
                    if (obj.userRoless.Count() > 0)
                    {
                        foreach (var item in obj.userRoless.Where(it => it.Selected == true))
                        {
                            editAcc.AccountInRoleModels.Add(new AccountInRoleModel
                            {
                                AccountId = editAcc.AccountId,
                                RolesId = item.RolesId,
                            });
                        }
                    }
                    if (obj.DiaBans.Count() > 0)
                    {
                        foreach (var item in obj.DiaBans.Where(it => it.Selected == true))
                        {
                            editAcc.PhamVis.Add(new PhamVi
                            {
                                AccountId = editAcc.AccountId,
                                MaDiabanHoatDong = item.MaDiaBanHoiVien,
                                CreatedAccountId = AccountId()!.Value,
                                CreatedTime = DateTime.Now
                            });
                        }
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
            });
        }
       
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id) {
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
        #region Helper
        private bool CheckExistAccountName(string accountName) {
            var account = _context.Accounts.Where(it => it.UserName.ToLower().Equals(accountName.ToLower()));
            if(account != null && account.Count()>0)
            {
                return true;
            }
            return false;
        }
        private void ViewBagHelper(List<Guid>? AccountIDParent = null) {
            var accounts = _context.Accounts.Where(it => it.Actived == true && it.AccountId != AccountId()).Select(it => new { AccountIDParent = it.AccountId, FullName = it.FullName }).ToList();
            ViewBag.AccountIDParent = new MultiSelectList(accounts, "AccountIDParent", "FullName", AccountIDParent);
        }
        #endregion Helper
    }
}
