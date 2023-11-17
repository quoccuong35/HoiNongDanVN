using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol;
using System.Data.Entity;
using System.Linq;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HVDapController : BaseController
    {
        public HVDapController(AppDbContext context):base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(String? MaHoiVien, Guid? MaDiaBanHoatDong)
        {
            return ExecuteSearch(() => { 
                var data = _context.HoiVienHoiDaps.Include(it=>it.HoiVien).Where(it=>it.TraLoi != true).AsQueryable();
                if (!String.IsNullOrWhiteSpace(MaHoiVien))
                {
                    data = data.Where(it => it.HoiVien.MaCanBo == MaHoiVien);
                }
                if (MaDiaBanHoatDong != null)
                {
                    data = data.Where(it => it.HoiVien.MaDiaBanHoatDong == MaDiaBanHoatDong);
                }
                var model = data.Select(it => new HoiVienHoiDapDetail { 
                    ID = it.ID,
                    NoiDung = it.NoiDung,
                    Ngay = it.Ngay,
                    HoVaTen = it.HoiVien.HoVaTen,
                    TrangThai = it.TrangThai,
                }).OrderByDescending(it=>it.Ngay).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region View
        [HoiNongDanAuthorization]
        public IActionResult View(Guid id) {
            var lisCauHoi = from a in _context.HoiVienHoiDaps 
                            join b in _context.CanBos on a.IDHoivien equals b.IDCanBo
                            where a.TraLoi != true && a.ID == id
                            select new HoiVienHoiDapDetail
                            {
                                ID = a.ID,
                                HoVaTen = b!.HoVaTen,
                                NoiDung = a.NoiDung,
                                TraLoi = a.TraLoi,
                                Ngay = a.Ngay,
                                IdParent = a.IdParent,
                                TrangThai = a.TrangThai,
                            };
            var data = lisCauHoi.ToList(); ;
            if (lisCauHoi.Count() > 0)
            {
                var listraloi = _context.HoiVienHoiDaps.Include(it=>it.Account).Where(it => it.IdParent != null && lisCauHoi.Select(it => it.ID).ToList().Contains(it.IdParent.Value)).Select(it => new HoiVienHoiDapDetail
                {
                    ID = it.ID,
                    HoVaTen = it.Account!.FullName,
                    NoiDung = it.NoiDung,
                    TraLoi = it.TraLoi,
                    Ngay = it.Ngay,
                    IdParent = it.IdParent,
                    TrangThai = it.TrangThai,
                }).ToList(); ;
                data.AddRange(listraloi);
            }
            return View(data);
        }
        public IActionResult ViewAll()
        {
            var lisCauHoi = from a in _context.HoiVienHoiDaps
                            join b in _context.CanBos on a.IDHoivien equals b.IDCanBo
                            where a.TraLoi != true && a.TrangThai =="01"
                            select new HoiVienHoiDapDetail
                            {
                                ID = a.ID,
                                HoVaTen = b!.HoVaTen,
                                NoiDung = a.NoiDung,
                                TraLoi = a.TraLoi,
                                Ngay = a.Ngay,
                                IdParent = a.IdParent,
                                TrangThai = a.TrangThai,
                            };
           
            var data = lisCauHoi.ToList(); ;
            if (lisCauHoi.Count() > 0)
            {
                var listraloi = _context.HoiVienHoiDaps.Include(it => it.Account).Where(it => it.IdParent != null && lisCauHoi.Select(it => it.ID).ToList().Contains(it.IdParent.Value)).Select(it => new HoiVienHoiDapDetail
                {
                    ID = it.ID,
                    HoVaTen = it.Account!.FullName,
                    NoiDung = it.NoiDung,
                    TraLoi = it.TraLoi,
                    Ngay = it.Ngay,
                    IdParent = it.IdParent,
                    TrangThai = it.TrangThai,
                }).ToList(); ;
                data.AddRange(listraloi);
            }
            return View(data);
        }
        #endregion View
        #region Create
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(String NoiDung,Guid id )
        {
            return ExecuteContainer(() => {
                HoiVienHoiDap add = new HoiVienHoiDap();
                add.TraLoi = true;
                add.NoiDung = NoiDung;
                add.Ngay = DateTime.Now;
                add.ID = Guid.NewGuid();
                add.TrangThai = "01";
                add.AcountID = AccountId()!;
                add.IdParent = id;
                var edit = _context.HoiVienHoiDaps.SingleOrDefault(it => it.ID == id);
                edit.TrangThai = "03";
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.HoiVienHoiDaps.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, "Đáp")
                });
            });
        }
        #endregion Create
        #region Update
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id)
        {
            return ExecuteContainer(() => {

                var edit = _context.HoiVienHoiDaps.SingleOrDefault(it => it.ID == id);
                edit.TrangThai = "02";
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, "Đáp")
                });
            });
        }
        #endregion Update
        #region Del
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.HoiVienHoiDaps.FirstOrDefault(p => p.ID == id);

                if (del != null)
                {
                    var delChil = _context.HoiVienHoiDaps.FirstOrDefault(p => p.IdParent == del.ID);
                    _context.Remove(del);
                    if (delChil != null)
                        _context.Remove(delChil);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, "Đáp án")
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, "Đáp án")
                    });
                }
            });
        }
        #endregion Delete 
    }
}
