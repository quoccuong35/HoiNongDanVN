using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    public class NguonVonController : Extensions.BaseController
    {
        public NguonVonController(AppDbContext context):base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult _Search(bool? Actived) {
            return ExecuteSearch(() => { 
                var data = _context.NguonVons.AsEnumerable();
                if (Actived != null) {
                    data = data.Where(it => it.Actived == Actived);
                }
                var model = data.Select(it => new NguonVonVM {
                    MaNguonVon = it.MaNguonVon,
                    TenNguonVon = it.TenNguonVon,
                    GhiChu = it.GhiChu,
                    Actived = it.Actived
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Idnex
        #region Upsert
        [HoiNongDanAuthorization]
        public IActionResult Upsert(Guid? id) { 
            NguonVonVM nguonVonVM = new NguonVonVM();
            if(id != null) {
               var nguonVon= _context.NguonVons.SingleOrDefault(it => it.MaNguonVon == id);
                nguonVonVM.MaNguonVon = nguonVon.MaNguonVon;
                nguonVonVM.TenNguonVon = nguonVon.TenNguonVon;
                nguonVonVM.Actived = nguonVon.Actived;
                nguonVonVM.GhiChu = nguonVon.GhiChu;

            }
            return View(nguonVonVM);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Upsert(NguonVonVM obj)
        {
            return ExecuteContainer(() => {
                if (obj.MaNguonVon == null)
                {
                    // insert 
                    NguonVon insert = new NguonVon
                    {
                        MaNguonVon = Guid.NewGuid(),
                        TenNguonVon = obj.TenNguonVon,
                        GhiChu = obj.GhiChu,
                        Actived = true
                        
                    };
                    _context.NguonVons.Add(insert);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.NguonVon.ToLower())
                    });
                }
                else
                {
                    var nguonVon = _context.NguonVons.SingleOrDefault(it => it.MaNguonVon == obj.MaNguonVon);
                    if (nguonVon != null)
                    {
                        nguonVon.Actived = obj.Actived == null ? true : obj.Actived;
                        nguonVon.TenNguonVon = obj.TenNguonVon;
                      
                        nguonVon.GhiChu = obj.GhiChu;
                      
                        _context.Entry(nguonVon).State = EntityState.Modified;
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.NguonVon.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Không tìm thấy thông tin co ma " + obj.MaNguonVon
                        }); ;
                    }
                    // Edit
                }
            });

        }
        #endregion Upsert
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        public JsonResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.NguonVons.FirstOrDefault(p => p.MaNguonVon == id);


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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.NguonVon.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.NguonVon.ToLower())
                    });
                }
            });
        }
        #endregion Delete
    }
}
