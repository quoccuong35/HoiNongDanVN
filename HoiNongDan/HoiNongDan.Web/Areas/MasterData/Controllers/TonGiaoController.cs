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
    public class TonGiaoController : BaseController
    {
        public TonGiaoController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult _Search(TonGiaoSearchVM search)
        {
            return ExecuteSearch(() => {
                var data = _context.TonGiaos.AsQueryable();
                if (search.Actived != null)
                {
                    data = data.Where(it => it.Actived == search.Actived);
                }
                if (!String.IsNullOrEmpty(search.TenTonGiao) && !String.IsNullOrWhiteSpace(search.TenTonGiao))
                {
                    data = data.Where(it => it.TenTonGiao.Contains(search.TenTonGiao));
                }
                var model = data.Select(it => new TonGiaoVM
                {
                    MaTonGiao = it.MaTonGiao,
                    TenTonGiao = it.TenTonGiao,
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
            return View(new TonGiaoVM());
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Create(TonGiaoMTVM obj)
        {
            CheckError(obj);
            return ExecuteContainer(() => {
                TonGiao add = new TonGiao();
                obj.GetTonGiao(add);
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.TonGiaos.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.TonGiao.ToLower())
                });

            });
        }
        #endregion Create
        #region Edit
        [HoiNongDanAuthorization]
        public IActionResult Edit(string id)
        {
            var item = _context.TonGiaos.SingleOrDefault(it => it.MaTonGiao == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            var model = new TonGiaoVM
            {
                MaTonGiao = item.MaTonGiao,
                TenTonGiao = item.TenTonGiao,
                Actived = item.Actived,
                OrderIndex = item.OrderIndex,
                Description = item.Description,
            };
            return View(model);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Edit(TonGiaoMTVM obj)
        {
            return ExecuteContainer(() => {
                var edit = _context.TonGiaos.SingleOrDefault(it => it.MaTonGiao == obj.MaTonGiao);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.TonGiao.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetTonGiao(edit);
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.TonGiao.ToLower())
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
                var del = _context.TonGiaos.FirstOrDefault(p => p.MaTonGiao == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.TonGiao.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.TonGiao.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CheckError(TonGiaoMTVM obj) {
            var checkExist = _context.TonGiaos.SingleOrDefault(it => it.MaTonGiao == obj.MaTonGiao);
            if (checkExist != null)
            {
                ModelState.AddModelError("MaTrinhDoTonGiao", "Mã tồn tại không thể thêm");
            }
        }
        #endregion Helper
    }
}
