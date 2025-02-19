using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HVInfoController : BaseController
    {
        public HVInfoController(AppDbContext context) : base(context) { }
        public IActionResult SearchHV(string maHV) {
            return ExecuteSearch(() => {
                HoiVienInfo HoiVien = new HoiVienInfo();
                try
                {
                    if (String.IsNullOrEmpty(maHV))
                    {
                        ModelState.AddModelError("MaCanBo", "Chưa nhập Mã hội viên / Số CCCD ");
                    }
                    else
                    {
                        var phamVis = Function.GetPhamVi(AccountId: AccountId()!.Value, _context: _context);
                        var hoivien = _context.CanBos.Where(it =>phamVis.Contains(it.MaDiaBanHoatDong!.Value) && it.IsHoiVien == true && (it.SoCCCD == maHV || it.MaCanBo == maHV ));
                        if (hoivien == null || hoivien!.Count() ==0)
                        {
                            ModelState.AddModelError("MaCanBo", "Không tìm thấy Mã hội viên / Số CCCD " + maHV);
                            HoiVien.Error = "Không tìm thấy Mã hội viên / Số CCCD " + maHV;
                        }
                        else if (hoivien.Count() > 2) 
                        {
                            ModelState.AddModelError("MaCanBo", "Có 2 thông tin hội viên cùng Mã hội viên / Số CCCD ");
                        }
                        else
                        {
                            var data = hoivien.First();
                            var diaBan = _context.DiaBanHoatDongs.SingleOrDefault(it => it.Id == data.MaDiaBanHoatDong);
                            var quanThanhPho = _context.QuanHuyens.SingleOrDefault(it => it.MaQuanHuyen == diaBan!.MaQuanHuyen);
                            HoiVien.IdCanbo = data!.IDCanBo;
                            HoiVien.HoVaTen = data.HoVaTen;
                            HoiVien.MaCanBo = data.MaCanBo!;
                            HoiVien.DiaBan = diaBan!.TenDiaBanHoatDong;
                            HoiVien.NgaySinh = data!.NgaySinh;
                            HoiVien.HoKhauThuongTru = data.HoKhauThuongTru;
                            HoiVien.SoCCCD = data.SoCCCD;
                            HoiVien.QuanHuyen = quanThanhPho!.TenQuanHuyen;

                        }
                    }
                    
                    return PartialView("_HVThongTin", HoiVien);
                }
                catch (Exception ex)
                {
                    HoiVien.Error = ex.Message + maHV;
                    return PartialView("_HVThongTin", HoiVien);
                }
            });
        }

        public IActionResult _HoiVien() {
            CreateViewbag();
            HVInfoSearchVM sr = new HVInfoSearchVM();
            return PartialView(sr);
        }
        [HttpGet]
        public IActionResult _HoiVienSearch(HVInfoSearchVM sr)
        {
            var modal = (from hv in _context.CanBos
                          join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                          where pv.AccountId == AccountId()
                          && hv.Actived == true
                          && hv.isRoiHoi != true
                          && hv.IsHoiVien == true
                          select hv).Include(it => it.DiaBanHoatDong).AsQueryable();
           // var modal = _context.CanBos.Include(it => it.DiaBanHoatDong).Where(it => it.IsHoiVien == true && GetPhamVi().Contains(it.MaDiaBanHoatDong!.Value) && it.Actived == true && it.isRoiHoi != true).AsQueryable();
            if (sr.MaDiaBanHoiVien != null)
            {
                modal = modal.Where(it => it.MaDiaBanHoatDong == sr.MaDiaBanHoiVien);
            }
            if (!String.IsNullOrWhiteSpace(sr.MaQuanHuyen))
            {
                modal = modal.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == sr.MaQuanHuyen);
            }
            if (!String.IsNullOrWhiteSpace(sr.MaCanBo))
            {
                modal = modal.Where(it => it.MaCanBo == sr.MaCanBo);
            }
            if (!String.IsNullOrWhiteSpace(sr.HoVaTen))
            {
                modal = modal.Where(it => it.HoVaTen.Contains(sr.HoVaTen));
            }
            if (!String.IsNullOrWhiteSpace(sr.SoCCCD))
            {
                modal = modal.Where(it => it.SoCCCD!.Contains(sr.SoCCCD));
            }
            var data = modal.Select(it=>new HoiVienInfo {
                IdCanbo = it.IDCanBo,
                MaCanBo = it.MaCanBo,
                HoVaTen = it.HoVaTen,
                SoCCCD = it.SoCCCD,
                NgaySinh = it.NgaySinh,
                HoKhauThuongTru = it.HoKhauThuongTru,
                DiaBan = it.DiaBanHoatDong!.TenDiaBanHoatDong
            }).ToList();
            return PartialView("_DSHoiVien", data);
        }
        #region Helper
        [NonAction]
        private void CreateViewbag() {
            FnViewBag fnViewBag = new FnViewBag(_context);

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }
        [HttpGet]
        public IActionResult CheckMaDinhDanh(string maDinhDanh,Guid? idHoiVien)
        {
            var checkExist = _context.CanBos.Where(it => it.MaDinhDanh == maDinhDanh && it.IsHoiVien == true && it.TuChoi != true && it.isRoiHoi != true);
            if (idHoiVien != null && checkExist.Where(it => it.IDCanBo != idHoiVien.Value).Count() > 0)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Data = "Mã định danh đã tồn tại"
                });
            }
            else if(idHoiVien == null && checkExist.Count() > 0)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Data = "Mã định danh đã tồn tại"
                });
            }
            else
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = ""
                });
            }
        }
        [HttpGet]
        public IActionResult CheckSoCCCD(string soCCCD, Guid? idHoiVien)
        {
            var checkExist = _context.CanBos.Where(it => it.SoCCCD == soCCCD && it.IsHoiVien == true && it.TuChoi != true && it.isRoiHoi != true);
            if (idHoiVien != null && checkExist.Where(it => it.IDCanBo != idHoiVien.Value).Count() > 0)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Data = "Số CCCD đã tồn tại"
                });
            }
            else if (idHoiVien == null && checkExist.Count() > 0)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Data = "Số CCCD đã tồn tại"
                });
            }
            else
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = ""
                });
            }
        }
        #endregion Helper

        #region Địa bàn hội viên theo quận huyện
        public JsonResult loadDiaBanHoatDong(string? maQuanHuyen)
        {
            var diaBanHoatDong = (from db in _context.DiaBanHoatDongs
                                  join pv in _context.PhamVis on db.Id equals pv.MaDiabanHoatDong
                                  where pv.AccountId == AccountId()
                                  select new
                                  {
                                      MaDiaBanHoatDong = db.Id,
                                      Name = db.TenDiaBanHoatDong,
                                      db.MaQuanHuyen
                                  }).ToList();
            if (!String.IsNullOrWhiteSpace(maQuanHuyen))
            {
                diaBanHoatDong= diaBanHoatDong.Where(it=>it.MaQuanHuyen == maQuanHuyen).ToList();
            }
            
            return Json(diaBanHoatDong);
        }
        #endregion Địa bàn hội viên theo quận huyện

    }
}
