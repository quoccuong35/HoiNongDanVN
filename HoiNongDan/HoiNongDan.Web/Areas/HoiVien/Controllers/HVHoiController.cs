using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HVHoiController : BaseController
    {
        public HVHoiController(AppDbContext context):base(context) { }

        [HttpGet]
        public IActionResult _Search(Guid id) 
        {
            var lisCauHoi = _context.HoiVienHoiDaps.Include(it => it.HoiVien).Where(it => it.IDHoivien == id && it.TraLoi != true).OrderBy(it => it.Ngay).Select(it => new HoiVienHoiDapDetail
            {
                ID = it.ID,
                HoVaTen = it.HoiVien.HoVaTen,
                NoiDung = it.NoiDung,
                TraLoi = it.TraLoi,
                Ngay = it.Ngay,
                IdParent = it.IdParent
            }).ToList();
            if (lisCauHoi.Count() > 0)
            {
                var listraloi = _context.HoiVienHoiDaps.Include(it=>it.Account).Where(it => it.IdParent != null && lisCauHoi.Select(it => it.ID).ToList().Contains(it.IdParent.Value)).Select(it => new HoiVienHoiDapDetail
                {
                    ID = it.ID,
                    HoVaTen = it.Account.FullName,
                    NoiDung = it.NoiDung,
                    TraLoi = it.TraLoi,
                    Ngay = it.Ngay,
                    IdParent = it.IdParent
                }).ToList(); ;
                lisCauHoi.AddRange(listraloi);
            }
            return PartialView(lisCauHoi);
        }
        [HttpPost]

        public JsonResult Create(Guid IDHoiVien, String NoiDung) {
            return ExecuteContainer(()=>{
                HoiVienHoiDap add = new HoiVienHoiDap();
                add.TraLoi = false;
                add.NoiDung = NoiDung;
                add.Ngay = DateTime.Now;
                add.ID = Guid.NewGuid();
                add.TrangThai = "01";
                add.IDHoivien = IDHoiVien;
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.HoiVienHoiDaps.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, "Câu hỏi")
                });
            });
        }
        #region Del
        [HttpDelete]
        public JsonResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.HoiVienHoiDaps.FirstOrDefault(p => p.ID == id);
                
                if (del != null)
                {
                    var delChil = _context.HoiVienHoiDaps.Where(p => p.IdParent == del.ID);
                    _context.Remove(del);
                    if(delChil != null && delChil.Count()>0)
                        _context.RemoveRange(delChil);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, "Xóa thông tin câu hỏi thành công")
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, "Không tìm thấy thông tin cần xóa")
                    });
                }
            });
        }
        #endregion Delete 
    }
}
