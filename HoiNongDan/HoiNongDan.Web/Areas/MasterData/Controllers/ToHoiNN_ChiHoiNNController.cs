using HoiNongDan.DataAccess;
using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Extensions;
using HoiNongDan.Resources;
using Microsoft.EntityFrameworkCore;
using HoiNongDan.Models;
using HoiNongDan.Constant;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    public class ToHoiNN_ChiHoiNNController : BaseController
    {
        
        public ToHoiNN_ChiHoiNNController(AppDbContext context) : base (context) { }
        #region Index
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult _Search(string? Ten, bool? Actived)
        {
            return ExecuteSearch(() => {
                var data = _context.ToHoiNganhNghe_ChiHoiNganhNghes.AsQueryable();
                if (!String.IsNullOrWhiteSpace(Ten))
                {
                    data = data.Where(it => it.Ten.Contains(Ten));
                }
                if (Actived != null)
                {
                    data = data.Where(it => it.Actived == Actived);
                }
                var model = data.Select(it => new ToHoiNganhNghe_ChiHoiNganhNgheVM
                {
                    Ma_ToHoiNganhNghe_ChiHoiNganhNghe = it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe,
                    Actived = it.Actived,
                    Ten = it.Ten,
                    Description = it.Description,
                    OrderIndex = it.OrderIndex,
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create()
        {
            ToHoiNganhNghe_ChiHoiNganhNgheVM model = new ToHoiNganhNghe_ChiHoiNganhNgheVM();
            return View(model);
        }
        
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ToHoiNganhNghe_ChiHoiNganhNgheVM obj)
        {

            return ExecuteContainer(() =>
            {
                ToHoiNganhNghe_ChiHoiNganhNghe add= new ToHoiNganhNghe_ChiHoiNganhNghe();
                add.Ma_ToHoiNganhNghe_ChiHoiNganhNghe = Guid.NewGuid();
                add.Ten = obj.Ten;
                add.Actived = true;
                add.Description = obj.Description;
                add.OrderIndex = obj.OrderIndex;
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.ToHoiNganhNghe_ChiHoiNganhNghes.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.ToHoiNN_ChiHoiNN.ToLower())
                });

            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id)
        {
            var edit = _context.ToHoiNganhNghe_ChiHoiNganhNghes.SingleOrDefault(it => it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe == id);
            if (edit == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            var model = new ToHoiNganhNghe_ChiHoiNganhNgheVM
            {
                Ma_ToHoiNganhNghe_ChiHoiNganhNghe = edit.Ma_ToHoiNganhNghe_ChiHoiNganhNghe,
                Ten = edit.Ten,
                Actived = edit.Actived,
                OrderIndex = edit.OrderIndex,
                Description = edit.Description,
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HoiNongDanAuthorization]
        public IActionResult Edit(ToHoiNganhNghe_ChiHoiNganhNgheVM obj)
        {
            return ExecuteContainer(() =>
            {
                var edit = _context.ToHoiNganhNghe_ChiHoiNganhNghes.SingleOrDefault(it => it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe == obj.Ma_ToHoiNganhNghe_ChiHoiNganhNghe);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.ToHoiNN_ChiHoiNN.ToLower())
                    });
                }
                else
                {
                    edit.Ten = obj.Ten;
                    edit.Actived = obj.Actived;
                    edit.OrderIndex = obj.OrderIndex;
                    edit.Description = obj.Description;
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.ToHoiNN_ChiHoiNN.ToLower())
                    });
                }
            });
        }
        #endregion Edit
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.ToHoiNganhNghe_ChiHoiNganhNghes.SingleOrDefault(it => it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.ToHoiNN_ChiHoiNN.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.ToHoiNN_ChiHoiNN.ToLower())
                    });
                }
            });

        }
        #endregion Delete
    }
}
