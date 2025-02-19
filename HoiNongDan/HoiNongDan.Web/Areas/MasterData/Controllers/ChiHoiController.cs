using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    public class ChiHoiController : BaseController
    {
        public ChiHoiController(AppDbContext context):base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Index()
        {
            CreateViewBagSr();
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(ChiHoiSearchVM obj) {
            return ExecuteSearch(() => {
                var diaBanHois = _context.PhamVis.Where(it => it.AccountId == AccountId()).ToList().Select(it => it.MaDiabanHoatDong).ToList();
                var data = _context.ChiHois.Where(it => diaBanHois.Contains(it.MaDiaBanHoatDong!.Value)).Include(it=>it.CanBos).Include(it=>it.DiaBanHoatDong).ThenInclude(it=>it.QuanHuyen).AsQueryable();
                if (!String.IsNullOrWhiteSpace(obj.TenChiHoi))
                {
                    data = data.Where(it=>it.TenChiHoi.Contains(obj.TenChiHoi!));
                }
                if (!String.IsNullOrWhiteSpace(obj.MaQuanHuyen))
                {
                    data = data.Where(it => it.DiaBanHoatDong!.QuanHuyen.MaQuanHuyen == obj.MaQuanHuyen);
                }
                if (obj.MaDiaBanHoiVien != null)
                {
                    data = data.Where(it => it.MaDiaBanHoatDong == obj.MaDiaBanHoiVien);
                }
                if (obj.Actived != null) {
                    data = data.Where(it => it.Actived == obj.Actived);
                }
                if (!String.IsNullOrWhiteSpace(obj.Loai) && obj.Loai =="01") {
                    data = data.Where(it => it.Loai != "02");
                }
                if (!String.IsNullOrWhiteSpace(obj.Loai) && obj.Loai == "02")
                {
                    data = data.Where(it => it.Loai == "02");
                }
                var model = data.Select(it => new ChiHoiVM
                {
                    MaChiHoi = it.MaChiHoi,
                    TenChiHoi= it.TenChiHoi,
                    Actived = it.Actived,
                    Description = it.Description,
                    OrderIndex = it.OrderIndex,
                    SoHoiVien =it.CanBos.Where(it=>it.Actived ==true && it.IsHoiVien == true && it.isRoiHoi != true).Count(),
                    TenHoi = it.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen + "-" + it.DiaBanHoatDong.TenDiaBanHoatDong
                }).Where(it=>it.SoHoiVien>0).OrderBy(it=>it.TenHoi).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Upsert
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Upsert(Guid? id) {
            ChiHoiVM chiHoiVM = new ChiHoiVM();
            Guid? maDiaBanHoi = null;
            if (id != null) {
                var item = _context.ChiHois.SingleOrDefault(it => it.MaChiHoi == id);
                if(item != null) {
                    chiHoiVM.MaChiHoi = item.MaChiHoi;
                    chiHoiVM.TenChiHoi = item.TenChiHoi;
                    chiHoiVM.Actived = item.Actived;
                    chiHoiVM.Loai = item.Loai == null?"01":item.Loai;
                    chiHoiVM.Description = item.Description;
                    chiHoiVM.OrderIndex = item.OrderIndex;
                    chiHoiVM.SoQuyetDinh = item.SoQuyetDinh;
                    chiHoiVM.NgayThanhLap = item.NgayThanhLap;
                    chiHoiVM.MaDiaBanHoatDong = item.MaDiaBanHoatDong;
                    maDiaBanHoi = item.MaDiaBanHoatDong;
                }
            }
            CreateViewBag(maDiaBanHoi);
            return View(chiHoiVM);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ChiHoiVM obj) {
            return ExecuteContainer(() => {
               
                if (obj.MaChiHoi == null)
                {
                    // insert 
                    ChiHoi insert = new ChiHoi
                    {
                        MaChiHoi = Guid.NewGuid(),
                        TenChiHoi = obj.TenChiHoi,
                        Description = obj.Description,
                        OrderIndex = obj.OrderIndex,
                        SoQuyetDinh = obj.SoQuyetDinh,
                        NgayThanhLap = obj.NgayThanhLap,
                        NgayGiam = obj.NgayGiam,
                        LyDoGiam = obj.LyDoGiam,
                        Actived = true,
                        Loai = obj.Loai,
                        MaDiaBanHoatDong = obj.MaDiaBanHoatDong,
                        CreatedAccountId = Guid.NewGuid(),
                        CreatedTime = DateTime.Now


                    };
                    _context.ChiHois.Add(insert);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.ChiHoi.ToLower())
                    });
                }
                else
                {
                    var diaBanHois = _context.PhamVis.Where(it => it.AccountId == AccountId()).ToList().Select(it => it.MaDiabanHoatDong).ToList();
                    var edit = _context.ChiHois.SingleOrDefault(it => it.MaChiHoi == obj.MaChiHoi && diaBanHois.Contains(it.MaDiaBanHoatDong!.Value));
                    if (edit != null)
                    {
                        edit.Actived = obj.Actived == null ? true : obj.Actived;
                        edit.TenChiHoi = obj.TenChiHoi;
                        edit.Loai = obj.Loai;
                        edit.OrderIndex = obj.OrderIndex;
                        edit.Description = obj.Description;
                        edit.SoQuyetDinh = obj.SoQuyetDinh;
                        edit.NgayThanhLap = obj.NgayThanhLap;
                        edit.NgayGiam = obj.NgayGiam;
                        edit.LyDoGiam = obj.LyDoGiam;
                        edit.MaDiaBanHoatDong = obj.MaDiaBanHoatDong;
                        edit.LastModifiedAccountId = new Guid(CurrentUser.AccountId!);
                        edit.LastModifiedTime = DateTime.Now;

                        HistoryModelRepository history = new HistoryModelRepository(_context);
                        history.SaveUpdateHistory(edit.MaChiHoi.ToString(), AccountId()!.Value, edit);
                        _context.Entry(edit).State = EntityState.Modified;
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.ChiHoi.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Không tìm thấy thông tin co ma " + obj.MaChiHoi
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
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.ChiHois.FirstOrDefault(p => p.MaChiHoi == id);


                if (del != null)
                {
                    //_context.Entry(accountInRoleModels).State = EntityState.Deleted;
                    //_context.Entry(account).State = EntityState.Deleted;
                    del.Description = "Xóa khỏi hệ thống";
                    HistoryModelRepository history = new HistoryModelRepository(_context);
                    history.SaveUpdateHistory(del.MaChiHoi.ToString(), AccountId()!.Value, del);
                    _context.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.ChiHoi.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.ChiHoi.ToLower())
                    });
                }
            });
        }
        #endregion Delete

        #region Helper
        private void CreateViewBag(Guid? MaDiaBanHoatDong = null)
        {
            var diaBanHois = _context.PhamVis.Where(it => it.AccountId == AccountId()).ToList().Select(it => it.MaDiabanHoatDong).ToList();

            var diaBan = _context.DiaBanHoatDongs.Where(it => diaBanHois.Contains(it.Id)).Include(it => it.QuanHuyen).Select(it => new { MaDiaBanHoatDong = it.Id, Ten = it.QuanHuyen.TenQuanHuyen + " " + it.TenDiaBanHoatDong }).ToList();
            ViewBag.MaDiaBanHoatDong = new SelectList(diaBan, "MaDiaBanHoatDong", "Ten", MaDiaBanHoatDong);
        }
        private void CreateViewBagSr()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }
        #endregion Helper
    }
}
