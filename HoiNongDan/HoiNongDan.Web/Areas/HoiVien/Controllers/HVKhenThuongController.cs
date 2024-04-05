using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models.Entitys.NhanSu;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Constant;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Entity.Core.Mapping;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HVKhenThuongController : BaseController
    {
        public HVKhenThuongController(AppDbContext context) : base(context) { }
        [HoiNongDanAuthorization]
        #region Index
        public IActionResult Index()
        {
            KhenThuongSearchVN model = new KhenThuongSearchVN();
            model.TuNam = model.DenNam = DateTime.Now.Year;

            CreateViewBagSearch();
            return View(model);
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(KhenThuongSearchVN search)
        {
            return ExecuteSearch(() =>
            {
                var model = (from kt in _context.QuaTrinhKhenThuongs
                              join hv in _context.CanBos on kt.IDCanBo equals hv.IDCanBo
                              join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                              where kt.Loai == "02" &&
                              kt.IsHoiVien == true
                              && pv.AccountId == AccountId()
                              select kt).Include(it => it.CanBo).ThenInclude(it => it.DiaBanHoatDong).
                              Include(it => it.DanhHieuKhenThuong).Include(it => it.HinhThucKhenThuong).AsQueryable();
                //var model = _context.QuaTrinhKhenThuongs.Where(it => it.IsHoiVien == true && it.Loai =="02").AsQueryable();
                if (!String.IsNullOrEmpty(search.MaDanhHieuKhenThuong) && !String.IsNullOrWhiteSpace(search.MaDanhHieuKhenThuong))
                {
                    model = model.Where(it => it.MaDanhHieuKhenThuong == search.MaDanhHieuKhenThuong);
                }
               // model = model.Include(it => it.CanBo).ThenInclude(it => it.DiaBanHoatDong).Include(it => it.DanhHieuKhenThuong).Include(it => it.HinhThucKhenThuong);
                if (!String.IsNullOrEmpty(search.MaCanBo) && !String.IsNullOrWhiteSpace(search.MaCanBo))
                {
                    model = model.Where(it => it.CanBo.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen) && !String.IsNullOrWhiteSpace(search.HoVaTen))
                {
                    model = model.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
                }
                if (search.MaDiaBanHoiVien != null)
                {
                    model = model.Where(it => it.CanBo.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
                }
                if (!String.IsNullOrEmpty(search.MaQuanHuyen))
                {
                    model = model.Where(it => it.CanBo.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
                }
                if (search.TuNam != null)
                {
                    model = model.Where(it => it.NgayQuyetDinh != null && it.NgayQuyetDinh.Value.Year >= search.TuNam);
                }
                if (search.DenNam != null)
                {
                    model = model.Where(it => it.NgayQuyetDinh != null && it.NgayQuyetDinh.Value.Year <= search.DenNam);
                }
                var data = model.Select(it => new HVKhenThuongDetailVM
                {
                    IDQuaTrinhKhenThuong = it.IDQuaTrinhKhenThuong,
                    MaCanBo = it.CanBo.MaCanBo!,
                    HoVaTen = it.CanBo.HoVaTen,
                    Nam = it.Nam,
                    NoiDung = it.NoiDung,
                    GhiChu = it.GhiChu,
                    TenDanhHieuKhenThuong = it.DanhHieuKhenThuong.TenDanhHieuKhenThuong,
                    //TenHinhThucKhenThuong = it.HinhThucKhenThuong.TenHinhThucKhenThuong,
                    DiaBanHND = it.CanBo.DiaBanHoatDong!.TenDiaBanHoatDong
                }).ToList();
                return PartialView(data);
            });
        }
        #endregion Index
        #region Cretae
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create()
        {
            KhenThuongVM khenThuong = new KhenThuongVM();
            HoiVienInfo nhanSu = new HoiVienInfo();
            khenThuong.HoiVien = nhanSu;
            CreateViewBag();
            return View(khenThuong);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(KhenThuongVMMT obj)
        {
            CheckError(obj);
            return ExecuteContainer(() => {
                QuaTrinhKhenThuong add = new QuaTrinhKhenThuong();
                add = obj.GetKhenThuong(add);
                add.IDQuaTrinhKhenThuong = Guid.NewGuid();
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                add.IsHoiVien = true;
                _context.Attach(add).State = EntityState.Modified;
                _context.QuaTrinhKhenThuongs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.KhenThuong.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id)
        {
            var item = _context.QuaTrinhKhenThuongs.SingleOrDefault(it => it.IDQuaTrinhKhenThuong == id && it.IsHoiVien == true);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            KhenThuongVM obj = new KhenThuongVM();
            
            obj.IDQuaTrinhKhenThuong = item.IDQuaTrinhKhenThuong;
            obj.MaHinhThucKhenThuong = item.MaHinhThucKhenThuong!;
            obj.MaDanhHieuKhenThuong = item.MaDanhHieuKhenThuong;
            obj.SoQuyetDinh = item.SoQuyetDinh!;
            obj.NgayQuyetDinh = item.NgayQuyetDinh;
            obj.NguoiKy = item.NguoiKy;
            obj.NoiDung = item.NoiDung;
            obj.Nam = item.Nam;
            obj.GhiChu = item.GhiChu;
            obj.HoiVien = Function.GetThongTinHoiVien(item.IDCanBo,_context);
            CreateViewBag(item.MaHinhThucKhenThuong, item.MaDanhHieuKhenThuong);
            return View(obj);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(KhenThuongVMMT obj)
        {
            CheckError(obj);
            return ExecuteContainer(() => {
                var edit = _context.QuaTrinhKhenThuongs.SingleOrDefault(it => it.IDQuaTrinhKhenThuong == obj.IDQuaTrinhKhenThuong);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.KhenThuong.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetKhenThuong(edit);
                    edit.LastModifiedTime = DateTime.Now;
                    edit.LastModifiedAccountId = AccountId();
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.KhenThuong.ToLower())
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
                var del = _context.QuaTrinhKhenThuongs.FirstOrDefault(p => p.IDQuaTrinhKhenThuong == id);


                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.KhenThuong.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.KhenThuong.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CheckError(KhenThuongVMMT obj)
        {
            if (obj.HoiVien.IdCanbo == null)
            {
                ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
            }
        }
        private void CreateViewBag(String? MaHinhThucKhenThuong = null, String? MaDanhHieuKhenThuong = null)
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaHinhThucKhenThuong = fnViewBag.HinhThucKhenThuong(value:MaHinhThucKhenThuong);

            //  var MenuList = _context.DanhHieuKhenThuongs.Where(it => it.IsHoiVien == true).Select(it => new { MaDanhHieuKhenThuong = it.MaDanhHieuKhenThuong, TenDanhHieuKhenThuong = it.TenDanhHieuKhenThuong }).ToList();
            ViewBag.MaDanhHieuKhenThuong = fnViewBag.DanhHieuKhenThuong(value:MaDanhHieuKhenThuong); ;

        }
        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaHinhThucKhenThuong = fnViewBag.HinhThucKhenThuong();

          //  var MenuList = _context.DanhHieuKhenThuongs.Where(it => it.IsHoiVien == true).Select(it => new { MaDanhHieuKhenThuong = it.MaDanhHieuKhenThuong, TenDanhHieuKhenThuong = it.TenDanhHieuKhenThuong }).ToList();
            ViewBag.MaDanhHieuKhenThuong = fnViewBag.DanhHieuKhenThuong(); ;

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID:AccountId()) ;

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }
        #endregion Helper
    }
}
