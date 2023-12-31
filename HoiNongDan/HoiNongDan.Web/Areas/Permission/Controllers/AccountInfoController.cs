using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HoiNongDan.Web.Areas.Permission.Controllers
{
    [Area("Permission")]
    public class AccountInfoController : BaseController
    {
        public AccountInfoController(AppDbContext context) : base(context) { }
        public IActionResult Edit()
        {
            var active = HttpContext.Session.GetString(User.Identity!.Name!.ToLower());
            if (String.IsNullOrWhiteSpace(active) || !User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }
            var user = _context.Accounts.SingleOrDefault(it => it.AccountId == AccountId());
            AccountInfo model = new AccountInfo();
            if (user != null)
            {
                model.AccountId = user.AccountId;
                model.UserName = user.UserName;
                model.FullName = user.FullName;
            }
            return PartialView("_AccountInfo", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DoiMatKhau(AccountInfo accout)
        {
            var active = HttpContext.Session.GetString(User.Identity!.Name!.ToLower());
            if ( !User.Identity.IsAuthenticated || accout.AccountId != AccountId() || String.IsNullOrWhiteSpace(active))
            {
                return BadRequest();
            }
            return ExecuteContainer(() =>
            {
                string password = RepositoryLibrary.GetMd5Sum(accout.PassWordOld);
                var user = _context.Accounts.SingleOrDefault(it => it.AccountId == AccountId() && it.Password == password);
                if (user != null)
                {
                    string passWordNew = RepositoryLibrary.GetMd5Sum(accout.PassWordNew);
                    user.Password = passWordNew;
                    HistoryModelRepository history = new HistoryModelRepository(_context);
                    history.SaveUpdateHistory(user.AccountId.ToString(), AccountId()!.Value, user);
                    _context.Accounts.Update(user);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = "Thành công"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = "Không tìm thấy thông tin người dùng với mật khẩu đang nhập"
                    });
                }
            });

        }
    }
}
