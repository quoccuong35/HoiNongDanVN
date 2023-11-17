using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    public class DoanTheChinhTriHoiDoanController : BaseController
    {
        public DoanTheChinhTriHoiDoanController(AppDbContext context) : base(context) { }

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create()
        {
            DoanTheChinhTri_HoiDoanVM model = new DoanTheChinhTri_HoiDoanVM();
            return View(model);
        }
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult _Search(string? Ten, bool? Actived) {
            return ExecuteSearch(() => {
                var data = _context.DoanTheChinhTri_HoiDoans.AsQueryable();
                if (!String.IsNullOrWhiteSpace(Ten)) {
                    data = data.Where(it => it.TenDoanTheChinhTri_HoiDoan.Contains(Ten));
                }
                if (Actived != null) {
                    data = data.Where(it => it.Actived == Actived);
                }
                var model = data.Select(it => new DoanTheChinhTri_HoiDoanVM
                {
                    MaDonTheChinhTri_HoiDoan = it.MaDoanTheChinhTri_HoiDoan,
                    Actived = it.Actived,
                    TenDoanTheChinhTri_HoiDoan = it.TenDoanTheChinhTri_HoiDoan,
                    Description = it.Description,
                    OrderIndex = it.OrderIndex,
                });
                return PartialView(model);
            });
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DoanTheChinhTri_HoiDoanVM obj)
        {

            return ExecuteContainer(() =>
            {
                DoanTheChinhTri_HoiDoan add = new DoanTheChinhTri_HoiDoan();
                add.MaDoanTheChinhTri_HoiDoan = Guid.NewGuid();
                add.TenDoanTheChinhTri_HoiDoan = obj.TenDoanTheChinhTri_HoiDoan;
                add.Actived = true;
                add.Description = obj.Description;
                add.OrderIndex = obj.OrderIndex;
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.DoanTheChinhTri_HoiDoans.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.DoanTheChinhTriHoiDoan.ToLower())
                });

            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id)
        {
            var editDoanTheChinhTriHoiDoan = _context.DoanTheChinhTri_HoiDoans.SingleOrDefault(it => it.MaDoanTheChinhTri_HoiDoan == id);
            if (editDoanTheChinhTriHoiDoan == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            var model = new DoanTheChinhTri_HoiDoanVM
            {
                MaDonTheChinhTri_HoiDoan = editDoanTheChinhTriHoiDoan.MaDoanTheChinhTri_HoiDoan,
                TenDoanTheChinhTri_HoiDoan = editDoanTheChinhTriHoiDoan.TenDoanTheChinhTri_HoiDoan,
                Actived = editDoanTheChinhTriHoiDoan.Actived,
                OrderIndex = editDoanTheChinhTriHoiDoan.OrderIndex,
                Description = editDoanTheChinhTriHoiDoan.Description,
            };
            return View(model);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DoanTheChinhTri_HoiDoanVM obj)
        {
            return ExecuteContainer(() =>
            {
                var edit = _context.DoanTheChinhTri_HoiDoans.SingleOrDefault(it => it.MaDoanTheChinhTri_HoiDoan == obj.MaDonTheChinhTri_HoiDoan);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.DoanTheChinhTriHoiDoan.ToLower())
                    });
                }
                else
                {
                    edit.TenDoanTheChinhTri_HoiDoan = obj.TenDoanTheChinhTri_HoiDoan;
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
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.DoanTheChinhTriHoiDoan.ToLower())
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
                var del = _context.DoanTheChinhTri_HoiDoans.SingleOrDefault(it => it.MaDoanTheChinhTri_HoiDoan == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.DoanTheChinhTriHoiDoan.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.DoanTheChinhTriHoiDoan.ToLower())
                    });
                }
            });

        }
        #endregion Delete
    }
}
