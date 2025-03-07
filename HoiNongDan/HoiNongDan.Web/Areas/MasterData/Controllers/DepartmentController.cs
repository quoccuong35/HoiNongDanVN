﻿using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Models;
using HoiNongDan.DataAccess;
using HoiNongDan.Constant;
using Microsoft.AspNetCore.Authorization;
using HoiNongDan.Extensions;
using HoiNongDan.Resources;
using System.Collections.Generic;
using HoiNongDan.DataAccess.Repository;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    [Authorize]
    public class DepartmentController : BaseController
    {
        public DepartmentController(AppDbContext context) :base(context)
        {
        }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            //CreateViewBag();
            return View("Index");
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(DepartmentSearchVM userSearch)
        {
            return ExecuteSearch(() => {
                var data = _context.Departments.AsQueryable();
                if (!String.IsNullOrEmpty(userSearch.Name))
                {
                    data = data.Where(it => it.Name.Contains(userSearch.Name));
                }
                if (userSearch.IdCoso !=null)
                {
                    data = data.Where(it => it.IDCoSo == userSearch.IdCoso);
                }
                if (userSearch.Actived != null)
                {
                    data = data.Where(it => it.Actived == userSearch.Actived);
                }
                var model = data.OrderBy(it=>it.OrderIndex).Select(it => new DepartmentVM
                {
                    Id = it.Id,
                    Code = it.Code,
                    Name = it.Name,
                    Description = it.Description,
                    Actived = it.Actived,
                    OrderIndex = it.OrderIndex
                });
                //account.userRoless = new  
                return PartialView(model);
            });
        }
        #endregion End Index
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Upsert(Guid? id)
        {
            DepartmentVM departmentVM = new DepartmentVM();
            if (id != null)
            {
                var item = _context.Departments.SingleOrDefault(it => it.Id == id);
                departmentVM.Id = item.Id;
                departmentVM.Name = item.Name;
                departmentVM.Code = item.Code;
                departmentVM.IdParent = item.IdParent;
                //departmentVM.IdCoSo = item.IDCoSo;
                departmentVM.Actived = item.Actived;
                departmentVM.Description = item.Description;
                departmentVM.OrderIndex = item.OrderIndex;

            }
            CreateViewBag(departmentVM.IdParent);
            //CreateViewBag(departmentVM.IdCoSo);
            return View(departmentVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HoiNongDanAuthorization]
        public IActionResult Upsert(DepartmentVM obj)
        {
            return ExecuteContainer(() => {
                if (obj.Id == null)
                {
                    // insert 
                    Department insert = new Department
                    {
                        Id = Guid.NewGuid(),
                        Name = obj.Name,
                        Code = obj.Code,
                        Description = obj.Description,
                        OrderIndex = obj.OrderIndex,
                        Actived = true,
                        IdParent = obj.IdParent,
                        //IDCoSo = obj.IdCoSo,
                        CreatedAccountId = Guid.NewGuid(),
                        CreatedTime = DateTime.Now

                    };
                    _context.Departments.Add(insert);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Department.ToLower())
                    });
                }
                else
                {
                    var departmentEdit = _context.Departments.SingleOrDefault(it => it.Id == obj.Id);
                    if (departmentEdit != null)
                    {
                        departmentEdit.Actived = obj.Actived == null ? true : obj.Actived.Value;
                        departmentEdit.Name = obj.Name;
                        departmentEdit.Code = obj.Code;
                        departmentEdit.IdParent = obj.IdParent;
                        departmentEdit.OrderIndex = obj.OrderIndex;
                        //departmentEdit.IDCoSo = obj.IdCoSo;
                        departmentEdit.Description = obj.Description;
                        departmentEdit.LastModifiedAccountId = new Guid(CurrentUser.AccountId!);
                        departmentEdit.LastModifiedTime = DateTime.Now;

                        HistoryModelRepository history = new HistoryModelRepository(_context);
                        _context.Entry(departmentEdit).State = EntityState.Modified;
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Department.ToLower())
                        });
                    }
                    else {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Không tìm thấy thông tin co ma " + obj.Id
                        }); ;
                    }
                    // Edit
                }
            });
            
        }
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.Departments.FirstOrDefault(p => p.Id == id);


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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Department.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.Department.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CreateViewBag(Guid? IdParent = null)
        {
            var MenuList = _context.Departments.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { IdParent = it.Id, Name = it.Name }).ToList();
            ViewBag.IdParent = new SelectList(MenuList, "IdParent", "Name", IdParent);
        }
        #endregion Helper
    }
}
