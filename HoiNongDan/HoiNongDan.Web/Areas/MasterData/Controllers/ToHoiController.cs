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
    public class ToHoiController : BaseController
    {
        public ToHoiController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(ToHoiSearchVM obj)
        {
            return ExecuteSearch(() => {
                var data = _context.ToHois.AsQueryable();
                if (!String.IsNullOrWhiteSpace(obj.TenToHoi))
                {
                    data = data.Where(it => it.TenToHoi.Contains(obj.TenToHoi!));
                }
                if (obj.Actived != null)
                {
                    data = data.Where(it => it.Actived == obj.Actived);
                }
                var model = data.Select(it => new ToHoiVM
                {
                    MaToHoi = it.MaToHoi,
                    TenToHoi = it.TenToHoi,
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
        public IActionResult Upsert(Guid? id)
        {
            ToHoiVM ToHoiVM = new ToHoiVM();
            if (id != null)
            {
                var item = _context.ToHois.SingleOrDefault(it => it.MaToHoi == id);
                if (item != null)
                {
                    ToHoiVM.MaToHoi = item.MaToHoi;
                    ToHoiVM.TenToHoi = item.TenToHoi;
                    ToHoiVM.Actived = ToHoiVM.Actived;
                    ToHoiVM.Description = ToHoiVM.Description;
                    ToHoiVM.OrderIndex = item.OrderIndex;
                }
            }
            return View(ToHoiVM);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ToHoiVM obj)
        {
            return ExecuteContainer(() => {
                if (obj.MaToHoi == null)
                {
                    // insert 
                    ToHoi insert = new ToHoi
                    {
                        MaToHoi = Guid.NewGuid(),
                        TenToHoi = obj.TenToHoi,
                        Description = obj.Description,
                        OrderIndex = obj.OrderIndex,
                        Actived = true,
                        CreatedAccountId = Guid.NewGuid(),
                        CreatedTime = DateTime.Now

                    };
                    _context.ToHois.Add(insert);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.ToHoi.ToLower())
                    });
                }
                else
                {
                    var edit = _context.ToHois.SingleOrDefault(it => it.MaToHoi == obj.MaToHoi);
                    if (edit != null)
                    {
                        edit.Actived = obj.Actived == null ? true : obj.Actived;
                        edit.TenToHoi = obj.TenToHoi;
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
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.ToHoi.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Không tìm thấy thông tin co ma " + obj.MaToHoi
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
                var del = _context.ToHois.FirstOrDefault(p => p.MaToHoi == id);


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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.ToHoi.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.ToHoi.ToLower())
                    });
                }
            });
        }
        #endregion Delete
    }
}
