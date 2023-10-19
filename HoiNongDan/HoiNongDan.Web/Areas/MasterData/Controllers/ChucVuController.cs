using HoiNongDan.Constant;
using HoiNongDan.Extensions;
using HoiNongDan.DataAccess;
using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    public class ChucVuController : BaseController
    {
        public ChucVuController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult _Search(ChucVuSearchVM search) {
            return ExecuteSearch(() => {
                var data = _context.ChucVus.AsQueryable();
                if (search.Actived != null)
                {
                    data = data.Where(it => it.Actived == search.Actived);
                }
                if (!String.IsNullOrEmpty(search.TenChucVu) && !String.IsNullOrWhiteSpace(search.TenChucVu))
                {
                    data = data.Where(it => it.TenChucVu.Contains(search.TenChucVu));
                }
                var model = data.Select(it => new ChucVuVM
                {
                    MaChucVu = it.MaChucVu,
                    TenChucVu = it.TenChucVu,
                    HeSoChucVu = it.HeSoChucVu,
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
        public IActionResult Create() { 
            return View(new ChucVuVM());
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Create(ChucVuMTVM obj)
        {
            return ExecuteContainer(() => {
                ChucVu add = new ChucVu();
                obj.GetChucVu(add);
                add.MaChucVu = Guid.NewGuid();
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.ChucVus.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.ChucVu.ToLower())
                });

            });
        }
        #endregion Create
        #region Edit
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
            var item = _context.ChucVus.SingleOrDefault(it => it.MaChucVu == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            var model = new ChucVuVM {
                MaChucVu = item.MaChucVu,
                TenChucVu = item.TenChucVu,
                Actived = item.Actived,
                OrderIndex = item.OrderIndex,
                HeSoChucVu = item.HeSoChucVu,
                Description = item.Description,
            };
            return View(model);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Edit(ChucVuMTVM obj)
        {
            return ExecuteContainer(() => {
                var edit = _context.ChucVus.SingleOrDefault(it => it.MaChucVu == obj.MaChucVu);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.ChucVu.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetChucVu(edit);
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.ChucVu.ToLower())
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
                var del = _context.ChucVus.FirstOrDefault(p => p.MaChucVu == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.ChucVu.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.ChucVu.ToLower())
                    });
                }
            });
        }
        #endregion Delete
    }
}
