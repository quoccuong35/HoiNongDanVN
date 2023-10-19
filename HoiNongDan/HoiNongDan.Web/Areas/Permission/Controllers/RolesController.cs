using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using NuGet.Packaging.Signing;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.ViewModels.Permission;
using HoiNongDan.Resources;
using System;
using System.Drawing;
using System.Linq;

namespace HoiNongDan.Web.Areas.Permission.Controllers
{
    [Area(ConstArea.Permission)]
    [Authorize]
    public class RolesController : BaseController
    {
        public RolesController(AppDbContext context) : base(context)
        {
        }
        #region Index
       // [HoiNongDanAuthorizationAttribute]
        public IActionResult Index()
        {
            return View(new RolesSearch());
        }
        public IActionResult _Search(RolesSearch search)
        {
            return ExecuteSearch(() =>
            {
                var roless = _context.RolesModels.AsQueryable();
                if (!String.IsNullOrEmpty(search.RolesName))
                {
                    roless = roless.Where(it => it.RolesName.Contains(search.RolesName));
                }
                if (search.Actived != null)
                {
                    roless = roless.Where(it => it.Actived == search.Actived);
                }
                var datas = roless.ToList().Select(it => new Roles
                {
                    RolesId = it.RolesId,
                    RolesCode = it.RolesCode,
                    RolesName = it.RolesName,
                    OrderIndex = it.OrderIndex,
                    Actived = it.Actived,
                }).ToList();
                return PartialView(datas);
            });
            
        }
        #endregion Index
        #region Upsert
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Upsert(Guid? id) {
            RolesVM roles = new RolesVM();
            if (id != null) {
                var data = _context.RolesModels.FirstOrDefault(it => it.RolesId == id!);
                if (data != null) {
                    roles.RolesId = data.RolesId;
                    roles.Actived = data.Actived;
                    roles.RolesCode = data.RolesCode;
                    roles.RolesName = data.RolesName;
                    roles.OrderIndex = data.OrderIndex;
                }
                
            }
            roles.Pages = GetMenuFunctions(roles.RolesId);
            return View(roles);
        }
        [HttpPost]
        public JsonResult Upsert(RolesVM roles) {
            if (AccountId() == null)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NetworkAuthenticationRequired,
                    Success = false,
                    Data = string.Format("Hết thời gian sử dụng vui lòng đăng nhập lại")
                });
            }

            return ExecuteContainer(()=>{
                if (roles.RolesId == null)
                {
                    RolesModel insertRoles = roles.Add(AccountId()!.Value);
                    //_context.Entry(insertRoles).State = EntityState.Added;
                    _context.RolesModels.Add(insertRoles);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Roles.ToLower())
                    });
                }
                else
                {
                    RolesModel? editRoles = _context.RolesModels.Include(it=>it.PagePermissionModels).FirstOrDefault(x => x.RolesId == roles.RolesId);
                    if (editRoles == null)
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Không tìm thấy thông tin co ma " + roles.RolesId
                        });
                    }
                    else
                    {
                        if (editRoles.PagePermissionModels.Count > 0)
                        {
                            editRoles.PagePermissionModels.Clear();
                        }
                        editRoles = roles.Update(AccountId()!.Value, editRoles);
                        HistoryModelRepository history = new HistoryModelRepository(_context);
                        history.SaveUpdateHistory(editRoles.RolesId.ToString(), AccountId()!.Value, editRoles);
                        _context.Entry(editRoles).State = EntityState.Modified;
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Roles.ToLower())
                        });
                    }

                }
            });
        }
        #endregion Upsert
        #region Delete
        [HoiNongDanAuthorization]
        public JsonResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var page = _context.RolesModels.FirstOrDefault(p => p.RolesId == id);
                if (page != null)
                {

                    _context.Entry(page).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Roles.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.Roles.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        public List<HoiNongDan.Models.Page> GetMenuFunctions(Guid? id) {
            List<HoiNongDan.Models.Page> list = new List<HoiNongDan.Models.Page>();
            var menus = _context.MenuModels.Where(it => it.Actived == true).ToList();
            var pageFuncs = _context.PageFunctionModels.Include(it=>it.FunctionModel).OrderBy(it=>it.FunctionModel.OrderIndex).ToList();
            var pagePermis = _context.PagePermissionModels.Where(it => it.RolesId == id).ToList();
            foreach (var item in menus.Where(it=>it.MenuIdParent == null).OrderBy(it=>it.OrderIndex))
            {
                HoiNongDan.Models.Page add = new HoiNongDan.Models.Page();
                add.MenuId = item.MenuId;
                add.MenuName = item.MenuName;
                add.MenuSubName = "";
                add.Funtions = AddPageFunction(add, pageFuncs, pagePermis);
                list.Add(add);
                var childItems = menus.Where(it => it.MenuIdParent == add.MenuId).OrderBy(it => it.OrderIndex).ToList();
                if (childItems.Count > 0)
                {
                    SetMenuFunctionParent(list, menus, item, pageFuncs, pagePermis);
                }
            }
            return list;
        }
        public void SetMenuFunctionParent(List<HoiNongDan.Models.Page> list,List<MenuModel> Pages, MenuModel page, List<PageFunctionModel> pageFuncs, List<PagePermissionModel> pagePermis)
        {
            var childItems = Pages.Where(it => it.MenuIdParent == page.MenuId).OrderBy(it => it.OrderIndex).ToList();
            if (childItems.Count > 0)
            {
                foreach (var item in childItems)
                {
                    HoiNongDan.Models.Page add = new HoiNongDan.Models.Page();
                    add.MenuId = item.MenuId;
                    add.MenuName = item.MenuName;
                    add.MenuSubName = page.MenuName;
                    add.Funtions = AddPageFunction(add, pageFuncs, pagePermis);
                    list.Add(add);
                    var subItems = Pages.Where(it => it.MenuIdParent == item.MenuId).OrderBy(it => it.OrderIndex).ToList();
                    if (subItems.Count > 0)
                    {
                        SetMenuFunctionParent(list, Pages, item, pageFuncs, pagePermis);
                    }
                }
            }
        }
        public List<PageFuntion> AddPageFunction(HoiNongDan.Models.Page add,List<PageFunctionModel> pageFuncs,List<PagePermissionModel>pagePermis) {
            var list = pageFuncs.Where(it=>it.MenuId == add.MenuId).Select(it => new PageFuntion {
                MenuId = it.MenuId,
                FunctionName = it.FunctionModel.FunctionName,
                FunctionId = it.FunctionId,
                Selected = pagePermis.SingleOrDefault(p=>p.FunctionId == it.FunctionId && p.MenuId == add.MenuId) != null?true:false,
            }).ToList();


            return list;
        }
        #endregion Helper
    }
}
