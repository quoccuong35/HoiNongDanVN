
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal.Constant;
using Portal.DataAccess;
using Portal.Extensions;
using Portal.Models;
using Portal.Models.Entitys;
using Portal.Resources;

namespace Portal.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    [Authorize]
    public class NgachLuongController : BaseController
    {
        public NgachLuongController(AppDbContext context) : base(context) { }
        #region Index
        [PortalAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult _Search(NgachLuongSearchVM obj) {
            return ExecuteSearch(() => {
                var model = _context.NgachLuongs.AsQueryable();
                if (!String.IsNullOrEmpty(obj.TenNgachLuong)) {
                    model = model.Where(it => it.TenNgachLuong.Contains(obj.TenNgachLuong));
                }
                if (!String.IsNullOrEmpty(obj.MaLoai))
                {
                    model = model.Where(it => it.MaLoai.Contains(obj.MaLoai));
                }
                if (obj.Actived != null)
                {
                    model = model.Where(it => it.Actived == obj.Actived);
                }
                var data = model.Select(it => new NgachLuongVM { 
                    MaNgachLuong = it.MaNgachLuong,
                    TenNgachLuong = it.TenNgachLuong,
                    NamTangLuong = it.NamTangLuong,
                    MaLoai = it.MaLoai,
                    OrderIndex = it.OrderIndex,
                    Description = it.Description
                }).ToList().OrderBy(it=>it.OrderIndex);
                return PartialView(data);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [PortalAuthorization]
        public IActionResult Create() {
            NgachLuongVM model = new NgachLuongVM();
            return View(model);
        }
        [HttpPost]
        [PortalAuthorization]
        public JsonResult Create(NgachLuongVM obj) {
            return ExecuteContainer(() => {
                if (CheckExistNgachLuong(obj.MaNgachLuong)) {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = string.Format(LanguageResource.Check_Exist, obj.MaNgachLuong)
                    });
                }
                NgachLuong add = new NgachLuong
                {
                    MaNgachLuong = obj.MaNgachLuong,
                    TenNgachLuong = obj.TenNgachLuong,
                    NamTangLuong = obj.NamTangLuong,
                    MaLoai = obj.MaLoai,
                    OrderIndex = obj.OrderIndex,
                    Description = obj.Description,
                    CreatedTime = DateTime.Now,
                    CreatedAccountId = AccountId()
                };
                _context.Attach(add).State = EntityState.Modified;
                _context.NgachLuongs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.NgachLuong.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [PortalAuthorization]
        public IActionResult Edit(string id) {
            var item = _context.NgachLuongs.SingleOrDefault(it => it.MaNgachLuong == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data="+id);
            }

            NgachLuongVM model = new NgachLuongVM { 
                MaNgachLuong = item.MaNgachLuong,
                TenNgachLuong = item.TenNgachLuong,
                MaLoai = item.MaLoai,
                NamTangLuong = item.NamTangLuong,
                Description = item.Description,
                OrderIndex = item.OrderIndex
            };

            return View(model);
        }
        [HttpPost]
        [PortalAuthorization]
        public JsonResult Edit(NgachLuongVM obj)
        {
            return ExecuteContainer(() => {
                var edit = _context.NgachLuongs.SingleOrDefault(it => it.MaNgachLuong == obj.MaNgachLuong);
                edit.TenNgachLuong = obj.TenNgachLuong;
                edit.MaLoai = obj.MaLoai;
                edit.NamTangLuong = obj.NamTangLuong;
                edit.Description = obj.Description;
                edit.LastModifiedAccountId = AccountId();
                edit.LastModifiedTime = DateTime.Now;
                _context.Entry(edit).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.NgachLuong.ToLower())
                });
            });
        }
        #endregion Edit
        #region Delete
        public JsonResult Delete(string id) {
            return ExecuteDelete(() =>
            {
                var del = _context.NgachLuongs.FirstOrDefault(p => p.MaNgachLuong == id);


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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.NgachLuong.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.NgachLuong.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private bool CheckExistNgachLuong(string maNgachLuong) {
            return _context.NgachLuongs.SingleOrDefault(it => it.MaNgachLuong == maNgachLuong.Trim()) != null ? true : false;
        }
        #endregion Helper
    }
}
