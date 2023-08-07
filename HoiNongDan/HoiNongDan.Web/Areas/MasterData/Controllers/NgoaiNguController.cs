using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.Entitys.MasterData;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    public class NgoaiNguController : BaseController
    {
        public NgoaiNguController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        public IActionResult _Search(NgoaiNguSearchVM search)
        {
            return ExecuteSearch(() => {
                var data = _context.TrinhDoNgoaiNgus.AsQueryable();
                if (search.Actived != null)
                {
                    data = data.Where(it => it.Actived == search.Actived);
                }
                if (!String.IsNullOrEmpty(search.TenTrinhDoNgoaiNgu) && !String.IsNullOrWhiteSpace(search.TenTrinhDoNgoaiNgu))
                {
                    data = data.Where(it => it.TenTrinhDoNgoaiNgu.Contains(search.TenTrinhDoNgoaiNgu));
                }
                var model = data.Select(it => new NgoaiNguVM
                {
                    MaTrinhDoNgoaiNgu = it.MaTrinhDoNgoaiNgu,
                    TenTrinhDoNgoaiNgu = it.TenTrinhDoNgoaiNgu,
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
            CreateViewBag();
            return View(new NgoaiNguVM());
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Create(NgoaiNguMTVM obj)
        {
            return ExecuteContainer(() => {
                TrinhDoNgoaiNgu add = new TrinhDoNgoaiNgu();
                obj.GetNgoaiNgu(add);
                add.MaTrinhDoNgoaiNgu = Guid.NewGuid();
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.TrinhDoNgoaiNgus.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.TrinhDoNgoaiNgu.ToLower())
                });

            });
        }
        #endregion Create
        #region Edit
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var item = _context.TrinhDoNgoaiNgus.SingleOrDefault(it => it.MaTrinhDoNgoaiNgu == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            var model = new NgoaiNguVM
            {
                MaTrinhDoNgoaiNgu = item.MaTrinhDoNgoaiNgu,
                TenTrinhDoNgoaiNgu = item.TenTrinhDoNgoaiNgu,
                Actived = item.Actived,
                OrderIndex = item.OrderIndex,
                Description = item.Description,
                MaNgonNgu = item.MaNgonNgu
            };
            CreateViewBag(item.MaNgonNgu);
            return View(model);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Edit(NgoaiNguMTVM obj)
        {
            return ExecuteContainer(() => {
                var edit = _context.TrinhDoNgoaiNgus.SingleOrDefault(it => it.MaTrinhDoNgoaiNgu == obj.MaTrinhDoNgoaiNgu);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.TrinhDoNgoaiNgu.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetNgoaiNgu(edit);
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.TrinhDoNgoaiNgu.ToLower())
                    });
                }
            });
        }
        #endregion Edit
        #region Del
        [HttpDelete]
        [HoiNongDanAuthorization]
        //[ValidateAntiForgeryToken]
        public JsonResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.TrinhDoNgoaiNgus.FirstOrDefault(p => p.MaTrinhDoNgoaiNgu == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.TrinhDoNgoaiNgu.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.TrinhDoNgoaiNgu.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CreateViewBag(string? MaNgonNgu =null) {
            var ngonNgus = _context.NgonNgus.Where(it => it.Actived == true).Select(it => new { it.MaNgonNgu, it.TenNgonNgu }).ToList();
            ViewBag.MaNgonNgu = new SelectList(ngonNgus, "MaNgonNgu", "TenNgonNgu", MaNgonNgu);
        }
        #endregion Helper
    }
}
