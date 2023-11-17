using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    public class ChinhTriController : BaseController
    {
        public ChinhTriController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(ChinhTriSearchVM search)
        {
            return ExecuteSearch(() => {
                var data = _context.TrinhDoChinhTris.AsQueryable();
                if (search.Actived != null)
                {
                    data = data.Where(it => it.Actived == search.Actived);
                }
                if (!String.IsNullOrEmpty(search.TenTrinhDoChinhTri) && !String.IsNullOrWhiteSpace(search.TenTrinhDoChinhTri))
                {
                    data = data.Where(it => it.TenTrinhDoChinhTri.Contains(search.TenTrinhDoChinhTri));
                }
                var model = data.Select(it => new ChinhTriVM
                {
                    MaTrinhDoChinhTri = it.MaTrinhDoChinhTri,
                    TenTrinhDoChinhTri = it.TenTrinhDoChinhTri,
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
            return View(new ChinhTriVM());
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ChinhTriMTVM obj)
        {
            CheckError(obj);
            return ExecuteContainer(() => {
                TrinhDoChinhTri add = new TrinhDoChinhTri();
                obj.GetChinhTri(add);
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.TrinhDoChinhTris.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.TrinhDoChinhTri.ToLower())
                });

            });
        }
        #endregion Create
        #region Edit
        [HoiNongDanAuthorization]
        public IActionResult Edit(string id)
        {
            var item = _context.TrinhDoChinhTris.SingleOrDefault(it => it.MaTrinhDoChinhTri == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            var model = new ChinhTriVM
            {
                MaTrinhDoChinhTri = item.MaTrinhDoChinhTri,
                TenTrinhDoChinhTri = item.TenTrinhDoChinhTri,
                Actived = item.Actived,
                OrderIndex = item.OrderIndex,
                Description = item.Description,
            };
            return View(model);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ChinhTriMTVM obj)
        {
            return ExecuteContainer(() => {
                var edit = _context.TrinhDoChinhTris.SingleOrDefault(it => it.MaTrinhDoChinhTri == obj.MaTrinhDoChinhTri);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.TrinhDoChinhTri.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetChinhTri(edit);
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.TrinhDoChinhTri.ToLower())
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
                var del = _context.TrinhDoChinhTris.FirstOrDefault(p => p.MaTrinhDoChinhTri == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.TrinhDoChinhTri.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.TrinhDoChinhTri.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CheckError(ChinhTriMTVM obj) {
            var checkExist = _context.TrinhDoChinhTris.SingleOrDefault(it => it.MaTrinhDoChinhTri == obj.MaTrinhDoChinhTri);
            if (checkExist != null)
            {
                ModelState.AddModelError("MaTrinhDoChinhTri", "Mã tồn tại không thể thêm");
            }
        }
        #endregion Helper
    }
}
