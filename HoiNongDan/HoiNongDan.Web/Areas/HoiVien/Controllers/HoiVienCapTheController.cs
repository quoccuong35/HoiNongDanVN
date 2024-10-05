using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HoiVienCapTheController : BaseController
    {
        public HoiVienCapTheController(AppDbContext context):base(context) { }

        #region Index
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Index()
        {
            HoiVienCapTheSearchVM model = new HoiVienCapTheSearchVM();
            CreateViewbag();
            return View(model);
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(HoiVienCapTheSearchVM sr) {
            return ExecuteSearch(() => {
                //var model = _context.HoiVienCapThes.Include(it => it.Dot).Include(it => it.HoiVien).ThenInclude(it=>it.DiaBanHoatDong).AsQueryable();

                var model = (from hvct in _context.HoiVienCapThes
                             join hv in _context.CanBos on hvct.IDHoiVien equals hv.IDCanBo
                             join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                             where pv.AccountId == AccountId()
                             && hv.IsHoiVien == true
                             && hv.isRoiHoi != true
                             select hvct).Include(it => it.Dot).Include(it => it.HoiVien).ThenInclude(it => it.DiaBanHoatDong).AsQueryable();

                if (!String.IsNullOrWhiteSpace(sr.MaCanBo))
                {
                    model = model.Where(it => it.HoiVien.MaCanBo == sr.MaCanBo);
                }
                if (!String.IsNullOrWhiteSpace(sr.HoVaTen))
                {
                    model = model.Where(it => it.HoiVien.HoVaTen.Contains(sr.HoVaTen));
                }
                if (!String.IsNullOrWhiteSpace(sr.MaQuanHuyen))
                {
                    model = model.Where(it => it.HoiVien.DiaBanHoatDong!.MaQuanHuyen  == sr.MaQuanHuyen);
                }
                if (sr.MaDiaBanHoiVien != null)
                {
                    model = model.Where(it => it.HoiVien.MaDiaBanHoatDong == sr.MaDiaBanHoiVien);
                }
                //if (!String.IsNullOrWhiteSpace(sr.LoaiCap))
                //{
                //    model = model.Where(it => it.LoaiCap == sr.LoaiCap);
                //}
                if (!String.IsNullOrWhiteSpace(sr.TrangThai))
                {
                    model = model.Where(it => it.TrangThai == sr.TrangThai);
                }
                if (sr.Nam != null)
                {
                    model = model.Where(it => it.Nam == sr.Nam);
                }
                if (sr.Quy != null)
                {
                    model = model.Where(it => it.Quy == sr.Quy);
                }
                if (sr.MaDot != null)
                {
                    model = model.Where(it => it.MaDot == sr.MaDot);
                }
              //  model = model.Where(it => GetPhamVi().Contains(it.HoiVien.MaDiaBanHoatDong!.Value));
                var data = model.Select(it => new HoiVienCapTheDetailVM {
                    ID = it.ID,
                    MaCanBo = it.HoiVien.MaCanBo,
                    HoVaTen = it.HoiVien.HoVaTen,
                    SoCCCD = it.HoiVien.SoCCCD,
                    NgayCapCCCD = it.HoiVien.NgayCapCCCD,
                    SoThe = it.SoThe,
                    NgayCap = it.NgayCap,
                    //TenDiaBan = it.HoiVien.DiaBanHoatDong!.TenDiaBanHoatDong,
                    DiaChi = it.HoiVien.ChoOHienNay,
                    Nam = it.Nam,
                    Quy = it.Quy,
                    TenDot = it.Dot.TenDot,
                    SoDienThoai = it.HoiVien.SoDienThoai,
                    GioiTinh = (int)it.HoiVien.GioiTinh == 1 ? "Nam" : "Nữ",
                    NgaySinh = it.HoiVien.NgaySinh,
                    TrangThai = it.TrangThai =="01"? "Chờ cấp": "Đã cấp"


                }).ToList();
                return PartialView(data);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create() {
            HoiVienCapTheVM model = new HoiVienCapTheVM();
            HoiVienInfo hoiVien = new HoiVienInfo();
            hoiVien.Edit = true;
            model.HoiVien = hoiVien;
            DotViewbag();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HoiNongDanAuthorization]
        public IActionResult Create(HoiVienCapTheVM model) {
            CheckError(model);
            return ExecuteContainer(() => {
                var data = model.HoiViens.Where(it => it.Chon == true).ToList();
                HoiVienCapThe add = new HoiVienCapThe {
                    ID = Guid.NewGuid(),
                    Nam = model.Nam,
                    Quy = model.Quy,
                    MaDot = model.MaDot,
                    TrangThai = model.TrangThai,
                    LoaiCap = "02",
                    IDHoiVien = model.HoiVien.IdCanbo!.Value,
                    Actived = true,
                    SoThe = model.SoThe,
                    NgayCap = model.NgayCap,
                    GhiChu= model.GhiChu,
                    CreatedAccountId = AccountId(),
                    CreatedTime = DateTime.Now,
                };
                if (model.CapNhatNhanSu)
                {
                    var hoiVien = _context.CanBos.SingleOrDefault(it => it.IDCanBo == model.HoiVien.IdCanbo);
                    hoiVien.MaCanBo = model.SoThe;
                    hoiVien.NgayCapThe = model.NgayCap;
                }
                //List<HoiVienCapThe> adds = new List<HoiVienCapThe>();
                //foreach (var item in data)
                //{
                //    adds.Add(new HoiVienCapThe
                //    {
                //        ID = Guid.NewGuid(),
                //        Nam = model.Nam,
                //        Quy = model.Quy,
                //        MaDot = model.MaDot,
                //        TrangThai = "01",
                //        LoaiCap = model.LoaiCap,
                //        IDHoiVien = item.IdCanbo!.Value,
                //        Actived = true,
                //        CreatedAccountId = AccountId(),
                //        CreatedTime = DateTime.Now,
                //    });
                //    ;
                //}
                _context.HoiVienCapThes.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.HoiVienCapThe.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit 
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Edit(Guid id) {
            var capThe = _context.HoiVienCapThes.SingleOrDefault(it => it.ID == id);
            if(capThe == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            HoiVienCapTheVM model = new HoiVienCapTheVM();
            model.ID = id;
            model.Nam = capThe.Nam;
            model.Quy = capThe.Quy;
            model.MaDot = capThe.MaDot;
            model.LoaiCap = capThe.LoaiCap;
            model.TrangThai = capThe.TrangThai;
            model.SoThe = capThe.SoThe;
            model.NgayCap = capThe.NgayCap;
            model.HoiVien = Function.GetThongTinHoiVien(capThe.IDHoiVien, _context);
            model.GhiChu = capThe.GhiChu;
            DotViewbag(model.MaDot);
           return View(model);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public IActionResult Edit(HoiVienCapTheVM model) {
            CheckError(model);
            return ExecuteContainer(() => {
                var edit = _context.HoiVienCapThes.SingleOrDefault(it => it.ID == model.ID);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.HoiVienCapThe.ToLower())
                    });
                }
                edit.SoThe = model.SoThe;
                edit.NgayCap = model.NgayCap;
                edit.Nam = model.Nam;
                edit.MaDot = model.MaDot;
                edit.Quy = model.Quy;
                edit.LoaiCap ="02";
                edit.TrangThai = model.TrangThai;
                edit.GhiChu = model.GhiChu;
                edit.LastModifiedAccountId = AccountId();
                edit.LastModifiedTime = DateTime.Now;
                if (model.CapNhatNhanSu)
                {
                    var hoiVien = _context.CanBos.SingleOrDefault(it=>it.IDCanBo == model.HoiVien.IdCanbo);
                    hoiVien!.MaCanBo = model.SoThe;
                    hoiVien.NgayCapThe = model.NgayCap;
                }
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.HoiVienCapThe.ToLower())
                });

            });
        }
        #endregion Edit
        #region Del
        [HttpDelete]
        [ValidateAntiForgeryToken]
        [HoiNongDanAuthorization]
        public IActionResult Delete(Guid id) {
            return ExecuteDelete(() => {
                var del = _context.HoiVienCapThes.SingleOrDefault(it => it.ID == id);
                if (del != null) {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.HoiVienCapThe.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.HoiVienCapThe.ToLower())
                    });
                }
            });
        }
        #endregion Del
        #region Helper
        private void CreateViewbag() {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId()); ;

            var dots = _context.Dots.Select(it => new { it.MaDot, it.TenDot }).ToList();
            ViewBag.MaDot = new SelectList(dots, "MaDot", "TenDot");
        }
        private void DotViewbag(Guid? MaDot = null) {
            var dots = _context.Dots.Select(it => new { it.MaDot, it.TenDot }).ToList();
            ViewBag.MaDot = new SelectList(dots, "MaDot", "TenDot",MaDot);
        }
        private void CheckError(HoiVienCapTheVM model)
        {
            if (model.HoiVien == null || model.HoiVien.IdCanbo == null) {
                ModelState.AddModelError("HoiVien", "Chưa chọn hội viên");
            }
            if (model.CapNhatNhanSu && String.IsNullOrWhiteSpace(model.SoThe)) {
                ModelState.AddModelError("SoThe", "Chưa nhập số thẻ");
            }

            //if (model.HoiViens == null || model.HoiViens.Count == 0)
            //{
            //    ModelState.AddModelError("HoiVien", "Chưa chọn hội viên");
            //}
        }
        #endregion Helper
    }
}
