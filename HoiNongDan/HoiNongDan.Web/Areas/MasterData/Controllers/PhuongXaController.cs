using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys.MasterData;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    public class PhuongXaController : BaseController
    {
        public PhuongXaController(AppDbContext context):base(context) { }
        #region index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBag("02");
            return View();
        }
        public IActionResult _Search(PhuongXaSearchVM search) {
            return ExecuteSearch(() => { 
                var data = _context.PhuongXas.Include(it=>it.QuanHuyen).ThenInclude(p=>p.TinhThanhPho).AsEnumerable();
                if (!String.IsNullOrWhiteSpace(search.MaTinhThanhPho))
                {
                    data = data.Where(it => it.QuanHuyen.TinhThanhPho.MaTinhThanhPho == search.MaTinhThanhPho);
                }
                if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen)) {
                    data = data.Where(it => it.MaQuanHuyen == search.MaQuanHuyen);
                }
                if (!String.IsNullOrWhiteSpace(search.TenPhuongXa))
                {
                    data = data.Where(it => it.TenPhuongXa.Contains(search.TenPhuongXa));
                }
                if (search.Actived != null) 
                {
                    data = data.Where(it => it.Actived == search.Actived);
                }

                var model = data.Select(it => new PhuongXaVM {
                    MaPhuongXa = it.MaPhuongXa,
                    TenPhuongXa = it.TenPhuongXa,
                    MaQuanHuyen = it.QuanHuyen.TenQuanHuyen,
                    MaTinhThanhPho = it.QuanHuyen.TinhThanhPho.TenTinhThanhPho,
                    Actived = it.Actived,
                    Description = it.Description,
                    OrderIndex = it.OrderIndex,
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion index
        #region Upsert
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Upsert(string? id) { 
            PhuongXaVM model = new PhuongXaVM();
            if (id != null)
            {
                var phuongXa = _context.PhuongXas.Include(it => it.QuanHuyen).SingleOrDefault(it => it.MaPhuongXa == id);
                if (phuongXa != null)
                {
                    model.MaPhuongXa = phuongXa.MaPhuongXa;
                    model.TenPhuongXa = phuongXa.TenPhuongXa;
                    model.MaQuanHuyen = phuongXa.MaQuanHuyen;
                    model.MaTinhThanhPho = phuongXa.QuanHuyen.MaTinhThanhPho;
                    model.Description = phuongXa.Description;
                    model.OrderIndex = phuongXa.OrderIndex;
                    model.Actived = phuongXa.Actived;
                    CreateViewBag(model.MaTinhThanhPho, model.MaPhuongXa);
                }
            }
            else
            {
                CreateViewBag("02");
            }
            
            return View(model);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        public JsonResult Upsert(PhuongXaVM obj) {
            return ExecuteContainer(() => {
                if (obj.MaPhuongXa == null)
                {
                    // insert 
                    PhuongXa insert = new PhuongXa
                    {
                        MaPhuongXa = Guid.NewGuid().ToString(),
                        TenPhuongXa = obj.TenPhuongXa,
                        MaQuanHuyen = obj.MaQuanHuyen,
                        Description = obj.Description!,
                        OrderIndex = obj.OrderIndex,
                        Actived = true,
                        //IDCoSo = obj.IdCoSo,
                        CreatedAccountId = Guid.NewGuid(),
                        CreatedTime = DateTime.Now

                    };
                    _context.PhuongXas.Add(insert);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.PhuongXa.ToLower())
                    });
                }
                else
                {
                    var departmentEdit = _context.PhuongXas.SingleOrDefault(it => it.MaPhuongXa == obj.MaPhuongXa);
                    if (departmentEdit != null)
                    {
                        departmentEdit.Actived = obj.Actived;
                        departmentEdit.TenPhuongXa = obj.TenPhuongXa;
                        departmentEdit.MaQuanHuyen = obj.MaQuanHuyen;
                        departmentEdit.OrderIndex = obj.OrderIndex;
                        //departmentEdit.IDCoSo = obj.IdCoSo;
                        departmentEdit.Description = obj.Description!;
                        departmentEdit.LastModifiedAccountId = new Guid(CurrentUser.AccountId!);
                        departmentEdit.LastModifiedTime = DateTime.Now;

                        HistoryModelRepository history = new HistoryModelRepository(_context);
                        _context.Entry(departmentEdit).State = EntityState.Modified;
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.PhuongXa.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Không tìm thấy thông tin co ma " + obj.MaPhuongXa
                        }); ;
                    }
                    // Edit
                }
            });
        }
        #endregion Upsert
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        public JsonResult Delete(string id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.PhuongXas.FirstOrDefault(p => p.MaPhuongXa == id);


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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.PhuongXa.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.PhuongXa.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        public void CreateViewBag(String? MaTinhThanhPho = null, String? MaQuanHuyen = null)
        {
            var MenuList1 = _context.TinhThanhPhos.Where(it => it.Actived == true && it.MaTinhThanhPho =="02").Select(it => new { MaTinhThanhPho = it.MaTinhThanhPho, TenTinhThanhPho = it.TenTinhThanhPho }).ToList();
            ViewBag.MaTinhThanhPho = new SelectList(MenuList1, "MaTinhThanhPho", "TenTinhThanhPho", MaTinhThanhPho);

            var MenuList2 = _context.QuanHuyens.Where(it => it.Actived == true && it.MaTinhThanhPho == MaTinhThanhPho).Select(it => new { MaQuanHuyen = it.MaQuanHuyen, TenQuanHuyen = it.TenQuanHuyen }).ToList();
            ViewBag.MaQuanHuyen = new SelectList(MenuList2, "MaQuanHuyen", "TenQuanHuyen", MaQuanHuyen);
        }
        public JsonResult LoadQuanHuyen(string maTinhThanhPho)
        {
            var data = _context.QuanHuyens.Where(it => it.Actived == true && it.MaTinhThanhPho == maTinhThanhPho).OrderBy(p => p.OrderIndex).Select(it => new { MaQuanHuyen = it.MaQuanHuyen, TenQuanHuyen = it.TenQuanHuyen }).ToList();
            return Json(data);
        }
        #endregion Helper
    }
}
