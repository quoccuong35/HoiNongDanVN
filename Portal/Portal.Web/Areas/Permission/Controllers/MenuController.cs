using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Portal.DataAccess;
using Portal.DataAccess.Repository;
using Portal.Extensions;
using Portal.Models;
using Portal.Models.Entitys;
using Portal.Resources;

namespace Portal.Web.Areas.Permission.Controllers
{
    [Area("Permission")]
    [Authorize]
    public class MenuController : BaseController
    {
        public MenuController(AppDbContext context) : base(context)
        {
        }
        #region Index
        [PortalAuthorization]
        public IActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        public IActionResult _Search(MenuSearchVM search) {
            return ExecuteSearch(() => {
                var mennus = _context.MenuModels.AsQueryable();
                if (!String.IsNullOrEmpty(search.MenuName))
                {
                    mennus = mennus.Where(it => it.MenuName.Contains(search.MenuName));
                }
                if (search.Actived != null)
                {
                    mennus = mennus.Where(it => it.Actived == search.Actived);
                }
                if (search.MenuIdParent != null)
                {
                    mennus = mennus.Where(_it => _it.MenuIdParent == search.MenuIdParent);
                }

                var data = mennus.ToList().Select(it => new Menu
                {
                    MenuId = it.MenuId,
                    MenuCode = it.MenuCode,
                    MenuName = it.MenuName,
                    OrderIndex = it.OrderIndex,
                    MenuIdParent = it.MenuIdParent,
                    Href = it.Href,
                    Icon = it.Icon,
                    MenuType = it.MenuType,
                }).OrderBy(x => {
                    if (x.MenuIdParent == null)
                        return x.MenuId;
                    else
                        return x.MenuIdParent.Value;
                }).ThenBy(it => it.OrderIndex).ThenBy(it => it.MenuId);

                return PartialView(data);
            });
            
        }
        #endregion Index
        #region Upserp
        [HttpGet]
        //[PortalAuthorization]
        public IActionResult Upsert(Guid? id) {
            MenuVM menu = new MenuVM();
            var listFunction = ListMenuFunction();
            if (id != null)
            {
                var data = _context.MenuModels.Include(it=>it.PageFunctionModels).FirstOrDefault(it => it.MenuId == id);
               
                if (data != null)
                {
                    menu.MenuId = data.MenuId;
                    menu.MenuCode = data.MenuCode;
                    menu.MenuName = data.MenuName;
                    menu.Actived = data.Actived;
                    menu.Icon = data.Icon;
                    menu.MenuIdParent = data.MenuIdParent;
                    menu.Href = data.Href;
                    menu.OrderIndex = data.OrderIndex;
                    menu.MenuType = data.MenuType;
                    foreach (var item in data.PageFunctionModels.ToList())
                    {
                        var exist = listFunction.SingleOrDefault(it => it.FunctionId == item.FunctionId);
                        if(exist != null)
                            exist.Selected = true;
                    }

                }
            }
            menu.FunctionList = listFunction;
            CreateViewBag(menu.MenuIdParent);
            return View(menu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Upsert(MenuVM obj)
        {
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
                if (obj.MenuId == null)
                {
                    MenuModel insertMenu = obj.Add(AccountId()!.Value);
                    //_context.Entry(insertMenu).State = EntityState.Added;
                    _context.MenuModels.Add(insertMenu);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Menu.ToLower())
                    });
                }
                else
                {
                    var editMenu = _context.MenuModels.Include(it=>it.PageFunctionModels).SingleOrDefault(x => x.MenuId == obj.MenuId);
                    if (editMenu == null)
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Không tìm thấy thông tin co ma " + obj.MenuId
                        });
                    }
                    else
                    {
                        if (editMenu.PageFunctionModels.Count >0)
                        {
                            editMenu.PageFunctionModels.Clear();
                        }
                        editMenu = obj.Update(AccountId()!.Value ,editMenu);
                       
                        HistoryModelRepository history = new HistoryModelRepository(_context);
                        history.SaveUpdateHistory(editMenu.MenuId.ToString(),AccountId()!.Value, editMenu);
                        _context.Entry(editMenu).State = EntityState.Modified;
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Menu.ToLower())
                        });
                    }
                    
                }
            });
            // create 
            }
        #endregion Upserp
        #region Delete
        [HttpDelete]
        [PortalAuthorization]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var page = _context.MenuModels.Include(it=>it.PageFunctionModels).FirstOrDefault(p => p.MenuId == id);
                if (page != null)
                {
                    if (page.PageFunctionModels.Count > 0)
                    {
                        page.PageFunctionModels.Clear();
                    }
                    _context.Entry(page).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Menu.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.Menu.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CreateViewBag(Guid? MenuIdParent = null)
        {
            // MenuId
            var MenuList = _context.MenuModels.Where(it=>it.Actived == true && it.MenuType ==0).OrderBy(p => p.OrderIndex).Select(it => new { MenuIdParent = it.MenuId, MenuName  = it.MenuName}).ToList();
            ViewBag.MenuIdParent = new SelectList(MenuList, "MenuIdParent", "MenuName", MenuIdParent);
      
        }
        private List<MenuFunction> ListMenuFunction() {
            return _context.FunctionModels.OrderBy(it=>it.OrderIndex).Select(it => new MenuFunction
            {
                FunctionId = it.FunctionId,
                FunctionName = it.FunctionName,
                Selected = false
            }).ToList();
        }
        #endregion Helper
    }
}
