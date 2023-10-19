using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using System.Data.Entity;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    [Authorize]
    public class BacLuongController : BaseController
    {
        public BacLuongController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        public IActionResult _Search(BacLuongSearchVM obj) {
            return ExecuteSearch(() => {
                var data = _context.BacLuongs.AsQueryable();
                if (!String.IsNullOrEmpty(obj.MaNgachLuong)) {
                    data = data.Where(it => it.MaNgachLuong == obj.MaNgachLuong);
                }
                if (!String.IsNullOrEmpty(obj.TenBacLuong))
                {
                    data = data.Where(it => it.TenBacLuong.Contains(obj.TenBacLuong));
                }
                if (obj.Actived != null)
                {
                    data = data.Where(it => it.Actived == obj.Actived);
                }
                var model = data.Include(it => it.NgachLuong).Select(it => new BacLuongDetailVM
                {
                    MaBacLuong = it.MaBacLuong,
                    TenBacLuong = it.TenBacLuong,
                    HeSo = it.HeSo,
                    Description = it.Description,
                    MaNgachLuong = it.MaNgachLuong,
                    TenNgachLuong = it.NgachLuong.TenNgachLuong,
                    OrderIndex = it.OrderIndex,

                }).OrderBy(it => it.TenNgachLuong).ThenBy(it => it.OrderIndex);
                return PartialView(model);
            });
        }
        #endregion Index
        #region Create
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Create() {
            BacLuongVM model = new BacLuongVM();
            CreateViewBag();
            return View(model);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Create(BacLuongVM obj)
        {
            string check = CheckExistBacLuong(obj.MaNgachLuong, obj.HeSo, obj.OrderIndex!.Value);
            if (check != "") {
                ModelState.AddModelError("error", check);
            }
            return ExecuteContainer(() => {
                BacLuong add = new BacLuong { 
                    MaBacLuong = Guid.NewGuid(),
                    TenBacLuong = obj.TenBacLuong,
                    HeSo = obj.HeSo,
                    OrderIndex = obj.OrderIndex,
                    Actived = true,
                    Description = obj.Description,
                    MaNgachLuong = obj.MaNgachLuong,
                    CreatedAccountId = AccountId(),
                    CreatedTime = DateTime.Now
                };
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.BacLuongs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.BacLuong.ToLower())
                });
            });
        }
        #endregion Create
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        public JsonResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.BacLuongs.FirstOrDefault(p => p.MaBacLuong == id);


                if (del != null)
                {
                    //_context.Entry(accountInRoleModels).State = EntityState.Deleted;
                    //_context.Entry(account).State = EntityState.Deleted;
                    _context.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.BacLuong.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.BacLuong.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CreateViewBag(String? MaNgachLuong = null)
        {
            // MenuId
            var MenuList = _context.NgachLuongs.Where(it => it.Actived == true).OrderBy(p => p.MaLoai).Select(it => new { MaNgachLuong = it.MaNgachLuong, TenNgachLuong = it.TenNgachLuong }).ToList();
            ViewBag.MaNgachLuong = new SelectList(MenuList, "MaNgachLuong", "TenNgachLuong", MaNgachLuong);

        }
        private string CheckExistBacLuong(string maNgachLuong, decimal heSo, int bac)
        {
            string kq = "";
            var exist = _context.BacLuongs.Where(it => it.MaNgachLuong == maNgachLuong && (it.HeSo == heSo || it.OrderIndex == bac)).ToList();
            if (exist.Count > 0) 
            {
                kq = String.Format("Hệ số hoặc bậc của ngạch {0} đã tồn tại không thể thêm", maNgachLuong);
            }
          
            return kq;
        }
        #endregion Helper
    }
}
