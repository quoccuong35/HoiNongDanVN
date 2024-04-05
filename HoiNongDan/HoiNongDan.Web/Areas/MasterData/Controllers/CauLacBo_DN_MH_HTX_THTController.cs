using HoiNongDan.DataAccess;
using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Extensions;
using HoiNongDan.Resources;
using Microsoft.EntityFrameworkCore;
using HoiNongDan.Models;
using HoiNongDan.Constant;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    public class CauLacBo_DN_MH_HTX_THTController : BaseController
    {
        
        public CauLacBo_DN_MH_HTX_THTController(AppDbContext context) : base (context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult _Search(string? Ten, bool? Actived, string? Loai)
        {
            return ExecuteSearch(() => {
                var data = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.AsQueryable();
                if (!String.IsNullOrWhiteSpace(Ten))
                {
                    data = data.Where(it => it.Ten.Contains(Ten));
                }
                if (Actived != null)
                {
                    data = data.Where(it => it.Actived == Actived);
                }
                if (!String.IsNullOrWhiteSpace(Loai))
                {
                    data = data.Where(it => it.Loai == Loai);
                }
                var model = data.Select(it => new CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacVM
                {
                    ID = it.Id_CLB_DN_MH_HTX_THT,
                    Actived = it.Actived,
                    Ten = it.Ten,
                    Loai = it.Loai!,
                    Description = it.Description,
                    OrderIndex = it.OrderIndex,
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create()
        {
            CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacVM model = new CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacVM();
            return View(model);
        }
        
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacVM obj)
        {

            return ExecuteContainer(() =>
            {
                CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac add= new CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac();
                add.Id_CLB_DN_MH_HTX_THT = Guid.NewGuid();
                add.Ten = obj.Ten;
                add.Loai = obj.Loai;
                add.Actived = true;
                add.Description = obj.Description;
                add.OrderIndex = obj.OrderIndex;
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.ToLower())
                });

            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id)
        {
            var edit = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.SingleOrDefault(it => it.Id_CLB_DN_MH_HTX_THT == id);
            if (edit == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            var model = new CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacVM
            {
                ID = edit.Id_CLB_DN_MH_HTX_THT,
                Ten = edit.Ten,
                Loai = edit.Loai == null?"":edit.Loai,
                Actived = edit.Actived,
                OrderIndex = edit.OrderIndex,
                Description = edit.Description,
            };
            return View(model);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacVM obj)
        {
            return ExecuteContainer(() =>
            {
                var edit = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.SingleOrDefault(it => it.Id_CLB_DN_MH_HTX_THT == obj.ID);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.ToLower())
                    });
                }
                else
                {
                    edit.Ten = obj.Ten;
                    edit.Actived = obj.Actived;
                    edit.Loai = obj.Loai;
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
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.ToLower())
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
                var del = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.SingleOrDefault(it => it.Id_CLB_DN_MH_HTX_THT == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.ToLower())
                    });
                }
            });

        }
        #endregion Delete
    }
}
