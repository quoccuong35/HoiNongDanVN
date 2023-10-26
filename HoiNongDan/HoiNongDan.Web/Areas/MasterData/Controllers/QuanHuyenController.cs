using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    public class QuanHuyenController : BaseController
    {
       
        public QuanHuyenController(AppDbContext context):base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        public IActionResult _Search(String? MaTinhThanhPho, bool? Actived) {
            return ExecuteSearch(() => { 
                var data = _context.QuanHuyens.Include(it=> it.TinhThanhPho).AsQueryable();
                if(!String.IsNullOrEmpty(MaTinhThanhPho) )
                {
                    data = data.Where(it => it.MaTinhThanhPho == MaTinhThanhPho);
                }
                if (Actived != null)
                {
                    data = data.Where(it => it.Actived == Actived);
                }
                var model = data.Select(it=>new QuanHuyenVM { 
                    MaQuanHuyen = it.MaQuanHuyen,
                    TenQuanHuyen = it.TenQuanHuyen,
                    MaTinhThanhPho =it.TinhThanhPho.TenTinhThanhPho,
                    Actived= it.Actived,
                    Description = it.Description,
                    OrderIndex = it.OrderIndex
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Upsert
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Upsert(String? id) {
            QuanHuyenVM quanHuyenVM = new QuanHuyenVM();
            if (!String.IsNullOrWhiteSpace(id))
            {
                var quanHuyen = _context.QuanHuyens.SingleOrDefault(it => it.MaQuanHuyen == id);
                if (quanHuyen != null)
                {
                    quanHuyenVM.MaQuanHuyen = quanHuyen.MaQuanHuyen;
                    quanHuyenVM.MaTinhThanhPho = quanHuyen.MaTinhThanhPho;
                    quanHuyenVM.TenQuanHuyen = quanHuyen.TenQuanHuyen;
                    quanHuyenVM.Actived = quanHuyen.Actived;
                    quanHuyenVM.Description = quanHuyen.Description;
                    quanHuyenVM.OrderIndex = quanHuyen.OrderIndex;
                }
            }
            CreateViewBag(quanHuyenVM.MaTinhThanhPho);
            return View(quanHuyenVM);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        public JsonResult Upsert(QuanHuyenVM obj) {
            return ExecuteContainer(() => { 
                if(obj.MaQuanHuyen == null) {
                    QuanHuyen add = new QuanHuyen();
                    add.MaQuanHuyen = Guid.NewGuid().ToString();
                    add.TenQuanHuyen = obj.TenQuanHuyen;
                    add.Actived = true;
                    add.Actived = true;
                    add.OrderIndex = obj.OrderIndex;
                    add.MaTinhThanhPho = obj.MaTinhThanhPho;
                    add.CreatedAccountId = AccountId();
                    add.CreatedTime = DateTime.Now;
                    _context.QuanHuyens.Add(add);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.QuanHuyen.ToLower())
                    });
                }
                else
                {
                    var edit = _context.QuanHuyens.SingleOrDefault(it => it.MaQuanHuyen == obj.MaQuanHuyen);
                    if (edit != null)
                    {
                        edit.MaTinhThanhPho = obj.MaTinhThanhPho;
                        edit.TenQuanHuyen = obj.TenQuanHuyen;
                        edit.OrderIndex = obj.OrderIndex;
                        edit.Actived = obj.Actived;
                        edit.Description = obj.Description;
                        edit.LastModifiedAccountId =AccountId();
                        edit.LastModifiedTime = DateTime.Now;
                        HistoryModelRepository history = new HistoryModelRepository(_context);
                        _context.Entry(edit).State = EntityState.Modified;
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.QuanHuyen.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Không tìm thấy thông tin co ma " + obj!.MaQuanHuyen
                        }); ;
                    }
            }
            });
        }
        #endregion Upsert
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        public JsonResult Delete(string id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.QuanHuyens.FirstOrDefault(p => p.MaQuanHuyen == id);


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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.QuanHuyen.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.QuanHuyen.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CreateViewBag(String? maTinhThanhPho = null)
        {
            // MenuId
            var MenuList = _context.TinhThanhPhos.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { MaTinhThanhPho = it.MaTinhThanhPho, TenTinhThanhPho = it.TenTinhThanhPho }).ToList();
            ViewBag.MaTinhThanhPho = new SelectList(MenuList, "MaTinhThanhPho", "TenTinhThanhPho", maTinhThanhPho);

        }
        #endregion Helper
    }
}
