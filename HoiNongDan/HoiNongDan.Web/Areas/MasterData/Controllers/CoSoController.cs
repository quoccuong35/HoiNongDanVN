using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.ViewModels.Permission;
using HoiNongDan.Resources;
using System.Collections.Generic;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    [Authorize]
    public class CoSoController : BaseController
    {
        public CoSoController(AppDbContext context) : base(context) { }
        #region index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult _Search(String Name, bool? Actived) {
            return ExecuteSearch(() => {
                var data = _context.CoSos.AsQueryable();
                if (!String.IsNullOrEmpty(Name))
                {
                    data = data.Where(it => it.TenCoSo.Contains(Name));
                }
                if (Actived != null)
                {
                    data = data.Where(it => it.Actived == Actived);
                }
                var model = data.ToList().Select(it => new CoSoVM
                {
                    IdCoSo = it.IdCoSo,
                    TenCoSo = it.TenCoSo,
                    Description = it.Description,
                    Actived = it.Actived,
                    OrderIndex = it.OrderIndex
                });
                //account.userRoless = new  
                return PartialView(model);
            });
        }
        #endregion Index;
        #region Upsert
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Upsert(Guid? id)
        {
            CoSoVM model = new CoSoVM();
            if (id != null) {
                var coSo = _context.CoSos.SingleOrDefault(it => it.IdCoSo == id);
                if (id == null)
                    return BadRequest();
                else { 
                    model.IdCoSo = id;
                    model.TenCoSo = coSo.TenCoSo;
                    model.Description = coSo.Description;
                    model.OrderIndex = coSo.OrderIndex;
                }
            }
            return View(model);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Upsert(CoSoVM item) {
            return ExecuteContainer(() =>{
                if (item.IdCoSo == null) {
                    CoSo add = new CoSo
                    {
                        IdCoSo = Guid.NewGuid(),
                        TenCoSo = item.TenCoSo,
                        OrderIndex = item.OrderIndex,
                        Description = item.Description,
                        CreatedAccountId = Guid.Parse(CurrentUser.AccountId),
                        CreatedTime = DateTime.Now
                    };
                    _context.CoSos.Add(add);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.CoSo.ToLower())
                    });
                }
                else
                {
                    // Cập nhật
                    var edit = _context.CoSos.SingleOrDefault(it => it.IdCoSo == item.IdCoSo);
                    if (edit == null)
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Không tìm thấy thông tin co ma " + item.TenCoSo
                        }); ;
                    }
                    else
                    {
                        edit.TenCoSo = item.TenCoSo; ;
                        edit.Description = item.Description;
                        edit.OrderIndex = item.OrderIndex;
                        edit.LastModifiedAccountId = Guid.Parse(CurrentUser.AccountId);
                        edit.LastModifiedTime = DateTime.Now;
                        _context.Entry(edit).State = EntityState.Modified;
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.CoSo.ToLower())
                        });
                    }
                }
            });
        }
        #endregion Upsert
        #region Delete
        public JsonResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.CoSos.FirstOrDefault(p => p.IdCoSo == id);


                if (del != null)
                {
                    //_context.Entry(accountInRoleModels).State = EntityState.Deleted;
                    //_context.Entry(account).State = EntityState.Deleted;
                    _context.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.CoSo.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.CoSo.ToLower())
                    });
                }
            });
        }
        #endregion Delete
    }
}
