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
    public class DanTocController : BaseController
    {
        public DanTocController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult _Search(DanTocSearchVM search)
        {
            return ExecuteSearch(() => {
                var data = _context.DanTocs.AsQueryable();
                if (search.Actived != null)
                {
                    data = data.Where(it => it.Actived == search.Actived);
                }
                if (!String.IsNullOrEmpty(search.TenDanToc) && !String.IsNullOrWhiteSpace(search.TenDanToc))
                {
                    data = data.Where(it => it.TenDanToc.Contains(search.TenDanToc));
                }
                var model = data.Select(it => new DanTocVM
                {
                    MaDanToc = it.MaDanToc,
                    TenDanToc = it.TenDanToc,
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
            return View(new DanTocVM());
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Create(DanTocMTVM obj)
        {
            CheckError(obj);
            return ExecuteContainer(() => {
                DanToc add = new DanToc();
                obj.GetDanToc(add);
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.DanTocs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.DanToc.ToLower())
                });

            });
        }
        #endregion Create
        #region Edit
        [HoiNongDanAuthorization]
        public IActionResult Edit(string id)
        {
            var item = _context.DanTocs.SingleOrDefault(it => it.MaDanToc == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            var model = new DanTocVM
            {
                MaDanToc = item.MaDanToc,
                TenDanToc = item.TenDanToc,
                Actived = item.Actived,
                OrderIndex = item.OrderIndex,
                Description = item.Description,
            };
            return View(model);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Edit(DanTocMTVM obj)
        {
            return ExecuteContainer(() => {
                var edit = _context.DanTocs.SingleOrDefault(it => it.MaDanToc == obj.MaDanToc);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.DanToc.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetDanToc(edit);
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.DanToc.ToLower())
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
                var del = _context.DanTocs.FirstOrDefault(p => p.MaDanToc == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.DanToc.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.DanToc.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CheckError(DanTocMTVM obj) {
            var checkExist = _context.DanTocs.SingleOrDefault(it => it.MaDanToc == obj.MaDanToc);
            if (checkExist != null)
            {
                ModelState.AddModelError("MaTrinhDoDanToc", "Mã tồn tại không thể thêm");
            }
        }
        #endregion Helper
    }
}
