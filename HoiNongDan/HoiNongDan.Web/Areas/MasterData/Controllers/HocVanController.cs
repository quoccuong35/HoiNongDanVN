using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.Entitys.MasterData;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    public class HocVanController : BaseController
    {
        public HocVanController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(HocVanSearchVM search)
        {
            return ExecuteSearch(() => {
                var data = _context.TrinhDoHocVans.AsQueryable();
                if (search.Actived != null)
                {
                    data = data.Where(it => it.Actived == search.Actived);
                }
                if (!String.IsNullOrEmpty(search.TenTrinhDoHocVan) && !String.IsNullOrWhiteSpace(search.TenTrinhDoHocVan))
                {
                    data = data.Where(it => it.TenTrinhDoHocVan.Contains(search.TenTrinhDoHocVan));
                }
                var model = data.Select(it => new HocVanVM
                {
                    MaTrinhDoHocVan = it.MaTrinhDoHocVan,
                    TenTrinhDoHocVan = it.TenTrinhDoHocVan,
                    Description = it.Description,
                    Actived = it.Actived,
                    OrderIndex = it.OrderIndex
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Create
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new HocVanVM());
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HocVanMTVM obj)
        {
            CheckError(obj);
            return ExecuteContainer(() => {
                TrinhDoHocVan add = new TrinhDoHocVan();
                obj.GetHocVan(add);
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.TrinhDoHocVans.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.TrinhDoHocVan.ToLower())
                });

            });
        }
        #endregion Create
        #region Edit
        [HoiNongDanAuthorization]
        public IActionResult Edit(string id)
        {
            var item = _context.TrinhDoHocVans.SingleOrDefault(it => it.MaTrinhDoHocVan == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            var model = new HocVanVM
            {
                MaTrinhDoHocVan = item.MaTrinhDoHocVan,
                TenTrinhDoHocVan = item.TenTrinhDoHocVan,
                Actived = item.Actived,
                OrderIndex = item.OrderIndex,
                Description = item.Description,
            };
            return View(model);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HocVanMTVM obj)
        {
            return ExecuteContainer(() => {
                var edit = _context.TrinhDoHocVans.SingleOrDefault(it => it.MaTrinhDoHocVan == obj.MaTrinhDoHocVan);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.TrinhDoHocVan.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetHocVan(edit);
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.TrinhDoHocVan.ToLower())
                    });
                }
            });
        }
        #endregion Edit
        #region Del
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.TrinhDoHocVans.FirstOrDefault(p => p.MaTrinhDoHocVan == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.TrinhDoHocVan.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.TrinhDoHocVan.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CheckError(HocVanMTVM obj) {
            var checkExist = _context.TrinhDoHocVans.SingleOrDefault(it => it.MaTrinhDoHocVan == obj.MaTrinhDoHocVan);
            if (checkExist != null)
            {
                ModelState.AddModelError("MaTrinhDoHocVan", "Mã tồn tại không thể thêm");
            }
        }
        #endregion Helper
    }
}
