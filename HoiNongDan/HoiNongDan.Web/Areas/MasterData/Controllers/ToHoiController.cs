using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    public class ToHoiController : BaseController
    {
        public ToHoiController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Index()
        {
            CreateViewBagSr();
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(ToHoiSearchVM obj)
        {
            return ExecuteSearch(() => {

                var diaBanHois = _context.PhamVis.Where(it => it.AccountId == AccountId()).ToList().Select(it => it.MaDiabanHoatDong).ToList();

                var data = _context.ToHois.Where(it=> diaBanHois.Contains(it.MaDiaBanHoatDong!.Value)).Include(it=>it.CanBos).Include(it=>it.DiaBanHoatDong).ThenInclude(it=>it.QuanHuyen).AsQueryable();

                if (!String.IsNullOrWhiteSpace(obj.TenToHoi))
                {
                    data = data.Where(it => it.TenToHoi.Contains(obj.TenToHoi!));
                }

                if (!String.IsNullOrWhiteSpace(obj.MaQuanHuyen))
                {
                    data = data.Where(it => it.DiaBanHoatDong!.QuanHuyen.MaQuanHuyen == obj.MaQuanHuyen);
                }
                if (obj.MaDiaBanHoiVien != null)
                {
                    data = data.Where(it => it.MaDiaBanHoatDong == obj.MaDiaBanHoiVien);
                }
                if (obj.Actived != null)
                {
                    data = data.Where(it => it.Actived == obj.Actived);
                }
                if (!String.IsNullOrWhiteSpace(obj.Loai) && obj.Loai == "01")
                {
                    data = data.Where(it => it.Loai != "02");
                }
                if (!String.IsNullOrWhiteSpace(obj.Loai) && obj.Loai == "02")
                {
                    data = data.Where(it => it.Loai == "02");
                }
                var model = data.Select(it => new ToHoiVM
                {
                    MaToHoi = it.MaToHoi,
                    TenToHoi = it.TenToHoi,
                    Actived = it.Actived,
                    Description = it.Description,
                    OrderIndex = it.OrderIndex,
                    TenHoi = it.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen + "-" + it.DiaBanHoatDong.TenDiaBanHoatDong, 
                    SoHoiVien = it.CanBos.Where(it => it.Actived == true && it.isRoiHoi != true && it.IsHoiVien == true).Count()
                }).Where(it=>it.SoHoiVien>0).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Upsert
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Upsert(Guid? id)
        {
            ToHoiVM ToHoiVM = new ToHoiVM();
            Guid? maDiaBanHoi = null;
            if (id != null)
            {
                var item = _context.ToHois.SingleOrDefault(it => it.MaToHoi == id);
                if (item != null)
                {
                    ToHoiVM.MaToHoi = item.MaToHoi;
                    ToHoiVM.TenToHoi = item.TenToHoi;
                    ToHoiVM.Loai = item.Loai;
                    ToHoiVM.MaDiaBanHoatDong = item.MaDiaBanHoatDong;
                    ToHoiVM.Actived = item.Actived;
                    ToHoiVM.Description = item.Description;
                    ToHoiVM.SoQuyetDinh = item.SoQuyetDinh;
                    ToHoiVM.NgayGiam = item.NgayGiam;
                    ToHoiVM.LyDoGiam = item.LyDoGiam;
                    ToHoiVM.NgayThanhLap = item.NgayThanhLap;
                    ToHoiVM.OrderIndex = item.OrderIndex;
                    maDiaBanHoi = item.MaDiaBanHoatDong;
                }
            }
            CreateViewBag(maDiaBanHoi);
            return View(ToHoiVM);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ToHoiVM obj)
        {
            return ExecuteContainer(() => {
                if (obj.MaToHoi == null)
                {
                    // insert 
                    ToHoi insert = new ToHoi
                    {
                        MaToHoi = Guid.NewGuid(),
                        TenToHoi = obj.TenToHoi,
                        Description = obj.Description,
                        OrderIndex = obj.OrderIndex,
                        SoQuyetDinh = obj.SoQuyetDinh,
                        NgayThanhLap = obj.NgayThanhLap,
                        MaDiaBanHoatDong = obj.MaDiaBanHoatDong,
                        NgayGiam = obj.NgayGiam,
                        LyDoGiam = obj.LyDoGiam,
                        Loai = obj.Loai,
                        Actived = true,
                        CreatedAccountId = Guid.NewGuid(),
                        CreatedTime = DateTime.Now

                    };
                    _context.ToHois.Add(insert);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.ToHoi.ToLower())
                    });
                }
                else
                {
                    var diaBanHois = _context.PhamVis.Where(it => it.AccountId == AccountId()).ToList().Select(it => it.MaDiabanHoatDong).ToList();
                    var edit = _context.ToHois.SingleOrDefault(it => it.MaToHoi == obj.MaToHoi && diaBanHois.Contains(it.MaDiaBanHoatDong!.Value));
                    if (edit != null)
                    {
                        edit.Actived = obj.Actived == null ? true : obj.Actived;
                        edit.TenToHoi = obj.TenToHoi;
                        edit.OrderIndex = obj.OrderIndex;
                        edit.Loai = obj.Loai;
                        edit.SoQuyetDinh = obj.SoQuyetDinh;
                        edit.NgayThanhLap = obj.NgayThanhLap;
                        edit.MaDiaBanHoatDong = obj.MaDiaBanHoatDong;
                        edit.NgayGiam = obj.NgayGiam;
                        edit.LyDoGiam = obj.LyDoGiam;
                        edit.Description = obj.Description;
                        edit.LastModifiedAccountId = new Guid(CurrentUser.AccountId!);
                        edit.LastModifiedTime = DateTime.Now;

                        HistoryModelRepository history = new HistoryModelRepository(_context);
                        history.SaveUpdateHistory(edit.MaToHoi.ToString(), AccountId()!.Value, edit);
                        _context.Entry(edit).State = EntityState.Modified;
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.ToHoi.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Không tìm thấy thông tin co ma " + obj.MaToHoi
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
                var del = _context.ToHois.FirstOrDefault(p => p.MaToHoi == id);


                if (del != null)
                {
                    //_context.Entry(accountInRoleModels).State = EntityState.Deleted;
                    //_context.Entry(account).State = EntityState.Deleted;
                    del.Description = "Xóa khỏi hệ thống";
                    HistoryModelRepository history = new HistoryModelRepository(_context);
                    history.SaveUpdateHistory(del.MaToHoi.ToString(), AccountId()!.Value, del);
                    _context.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.ToHoi.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.ToHoi.ToLower())
                    });
                }
            });
        }
        #endregion Delete

        #region Helper
        private void CreateViewBag(Guid? MaDiaBanHoatDong = null) {
            var diaBanHois = _context.PhamVis.Where(it => it.AccountId == AccountId()).ToList().Select(it => it.MaDiabanHoatDong).ToList();

            var diaBan = _context.DiaBanHoatDongs.Where(it=>diaBanHois.Contains(it.Id)).Include(it=>it.QuanHuyen).Select(it=>new { MaDiaBanHoatDong = it.Id,Ten = it.QuanHuyen.TenQuanHuyen + " " + it.TenDiaBanHoatDong}).ToList();
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
