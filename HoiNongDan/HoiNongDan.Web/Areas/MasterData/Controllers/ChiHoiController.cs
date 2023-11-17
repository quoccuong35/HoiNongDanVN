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
    public class ChiHoiController : BaseController
    {
        public ChiHoiController(AppDbContext context):base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(ChiHoiSearchVM obj) {
            return ExecuteSearch(() => { 
                var data = _context.ChiHois.AsQueryable();
                if (!String.IsNullOrWhiteSpace(obj.TenChiHoi))
                {
                    data = data.Where(it=>it.TenChiHoi.Contains(obj.TenChiHoi!));
                }
                if(obj.Actived != null) {
                    data = data.Where(it => it.Actived == obj.Actived);
                }
                var model = data.Select(it => new ChiHoiVM
                {
                    MaChiHoi = it.MaChiHoi,
                    TenChiHoi= it.TenChiHoi,
                    Actived = it.Actived,
                    Description = it.Description,
                    OrderIndex = it.OrderIndex,
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Upsert
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Upsert(Guid? id) {
            ChiHoiVM chiHoiVM = new ChiHoiVM();
            if (id != null) {
                var item = _context.ChiHois.SingleOrDefault(it => it.MaChiHoi == id);
                if(item != null) {
                    chiHoiVM.MaChiHoi = item.MaChiHoi;
                    chiHoiVM.TenChiHoi = item.TenChiHoi;
                    chiHoiVM.Actived = chiHoiVM.Actived;
                    chiHoiVM.Description = chiHoiVM.Description;
                    chiHoiVM.OrderIndex = item.OrderIndex;
                }
            }
            return View(chiHoiVM);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ChiHoiVM obj) {
            return ExecuteContainer(() => {
                if (obj.MaChiHoi == null)
                {
                    // insert 
                    ChiHoi insert = new ChiHoi
                    {
                        MaChiHoi = Guid.NewGuid(),
                        TenChiHoi = obj.TenChiHoi,
                        Description = obj.Description,
                        OrderIndex = obj.OrderIndex,
                        Actived = true,
                        CreatedAccountId = Guid.NewGuid(),
                        CreatedTime = DateTime.Now

                    };
                    _context.ChiHois.Add(insert);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.ChiHoi.ToLower())
                    });
                }
                else
                {
                    var edit = _context.ChiHois.SingleOrDefault(it => it.MaChiHoi == obj.MaChiHoi);
                    if (edit != null)
                    {
                        edit.Actived = obj.Actived == null ? true : obj.Actived;
                        edit.TenChiHoi = obj.TenChiHoi;
                        edit.OrderIndex = obj.OrderIndex;
                        //departmentEdit.IDCoSo = obj.IdCoSo;
                        edit.Description = obj.Description;
                        edit.LastModifiedAccountId = new Guid(CurrentUser.AccountId!);
                        edit.LastModifiedTime = DateTime.Now;

                        HistoryModelRepository history = new HistoryModelRepository(_context);
                        _context.Entry(edit).State = EntityState.Modified;
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.ChiHoi.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Không tìm thấy thông tin co ma " + obj.MaChiHoi
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
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.ChiHois.FirstOrDefault(p => p.MaChiHoi == id);


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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.ChiHoi.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.ChiHoi.ToLower())
                    });
                }
            });
        }
        #endregion Delete
    }
}
