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
    public class HocViController : BaseController
    {
        public HocViController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search()
        {
            return ExecuteSearch(() => {
                var data = _context.HocVis.AsQueryable();
                var model = data.Select(it => new HocVi
                {
                    MaHocVi = it.MaHocVi,
                    TenHocVi = it.TenHocVi,
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
            return View(new HocVi());
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HocVi obj)
        {
            CheckError(obj);
            return ExecuteContainer(() => {

                _context.Attach(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.HocVi.ToLower())
                });

            });
        }
        #endregion Create
        #region Edit
        [HoiNongDanAuthorization]
        public IActionResult Edit(string id)
        {
            var item = _context.HocVis.SingleOrDefault(it => it.MaHocVi == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            return View(item);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HocVi obj)
        {
            return ExecuteContainer(() => {
                var edit = _context.HocVis.SingleOrDefault(it => it.MaHocVi == obj.MaHocVi);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.HocVi.ToLower())
                    });
                }
                else
                {
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.HocVi.ToLower())
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
                var del = _context.HocVis.FirstOrDefault(p => p.MaHocVi == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.HocVi.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.HocVi.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CheckError(HocVi obj) {
            var checkExist = _context.HocVis.SingleOrDefault(it => it.MaHocVi == obj.MaHocVi);
            if (checkExist != null)
            {
                ModelState.AddModelError("MaHocVi", "Mã tồn tại không thể thêm");
            }
        }
        #endregion Helper
    }
}
