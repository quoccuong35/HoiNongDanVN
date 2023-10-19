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
    public class TinHocController : BaseController
    {
        public TinHocController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult _Search(TinHocSearchVM search)
        {
            return ExecuteSearch(() => {
                var data = _context.TrinhDoTinHocs.AsQueryable();
                if (search.Actived != null)
                {
                    data = data.Where(it => it.Actived == search.Actived);
                }
                if (!String.IsNullOrEmpty(search.TenTrinhDoTinHoc) && !String.IsNullOrWhiteSpace(search.TenTrinhDoTinHoc))
                {
                    data = data.Where(it => it.TenTrinhDoTinHoc.Contains(search.TenTrinhDoTinHoc));
                }
                var model = data.Select(it => new TinHocVM
                {
                    MaTrinhDoTinHoc = it.MaTrinhDoTinHoc,
                    TenTrinhDoTinHoc = it.TenTrinhDoTinHoc,
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
            return View(new TinHocVM());
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Create(TinHocMTVM obj)
        {
            CheckError(obj);
            return ExecuteContainer(() => {
                TrinhDoTinHoc add = new TrinhDoTinHoc();
                obj.GetTinHoc(add);
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.TrinhDoTinHocs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.TrinhDoTinHoc.ToLower())
                });

            });
        }
        #endregion Create
        #region Edit
        [HoiNongDanAuthorization]
        public IActionResult Edit(string id)
        {
            var item = _context.TrinhDoTinHocs.SingleOrDefault(it => it.MaTrinhDoTinHoc == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            var model = new TinHocVM
            {
                MaTrinhDoTinHoc = item.MaTrinhDoTinHoc,
                TenTrinhDoTinHoc = item.TenTrinhDoTinHoc,
                Actived = item.Actived,
                OrderIndex = item.OrderIndex,
                Description = item.Description,
            };
            return View(model);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Edit(TinHocMTVM obj)
        {
            return ExecuteContainer(() => {
                var edit = _context.TrinhDoTinHocs.SingleOrDefault(it => it.MaTrinhDoTinHoc == obj.MaTrinhDoTinHoc);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.TrinhDoTinHoc.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetTinHoc(edit);
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.TrinhDoTinHoc.ToLower())
                    });
                }
            });
        }
        #endregion Edit
        #region Del
        [HttpDelete]
        [HoiNongDanAuthorization]
        //[ValidateAntiForgeryToken]
        public JsonResult Delete(string id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.TrinhDoTinHocs.FirstOrDefault(p => p.MaTrinhDoTinHoc == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.TrinhDoTinHoc.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.TrinhDoTinHoc.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CheckError(TinHocMTVM obj) {
            var checkExist = _context.TrinhDoTinHocs.SingleOrDefault(it => it.MaTrinhDoTinHoc == obj.MaTrinhDoTinHoc);
            if (checkExist != null)
            {
                ModelState.AddModelError("MaTrinhDoTinHoc", "Mã tồn tại không thể thêm");
            }
        }
        #endregion Helper
    }
}
