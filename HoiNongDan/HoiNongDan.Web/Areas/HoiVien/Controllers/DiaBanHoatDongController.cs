using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    [Authorize]
    public class DiaBanHoatDongController : BaseController
    {
        public DiaBanHoatDongController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBagSearch();
            return View(new DiaBanHoatDongSerachVM());
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(DiaBanHoatDongSerachVM serach) {
            return ExecuteSearch(() => {
                var model = _context.DiaBanHoatDongs.Include(it=>it.TinhThanhPho).Include(it=>it.CanBos).Include(it=>it.QuanHuyen).AsEnumerable();
                if (!String.IsNullOrEmpty(serach.TenDiaBanHoatDong) && !String.IsNullOrWhiteSpace(serach.TenDiaBanHoatDong))
                {
                    model = model.Where(it => it.TenDiaBanHoatDong.Contains(serach.TenDiaBanHoatDong));
                }
               
                if (!String.IsNullOrEmpty(serach.MaQuanHuyen) && !String.IsNullOrWhiteSpace(serach.MaQuanHuyen))
                {
                    model = model.Where(it => it.MaQuanHuyen == serach.MaQuanHuyen);
                }
                if (!String.IsNullOrEmpty(serach.MaPhuongXa) && !String.IsNullOrWhiteSpace(serach.MaPhuongXa))
                {
                    model = model.Where(it => it.MaPhuongXa == serach.MaPhuongXa);
                }
                if (serach.Actived != null)
                {
                    model = model.Where(it => it.Actived == serach.Actived);
                }
                var data = model.Select(it => new DiaBanHoatDongDetailVM
                {
                    Id = it.Id,
                    TenDiaBanHoatDong = it.TenDiaBanHoatDong,
                    NgayThanhLap = it.NgayThanhLap,
                    DiaChi = it.DiaChi,
                    SoLuongHoiVien = it.CanBos.Where(it=>it.IsHoiVien == true && it.HoiVienDuyet == true).Count(),
                    TenTinhThanhPho = it.TinhThanhPho.TenTinhThanhPho,
                    MaQuanHuyen = it.QuanHuyen.TenQuanHuyen,
                    GhiChu = it.GhiChu
                }).OrderBy(it=>it.MaQuanHuyen);
                return PartialView(data);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create() {
            DiaBanHoatDongVM model = new DiaBanHoatDongVM();
            CreateViewBag("02");
            return View(model);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]

        public IActionResult Create(DiaBanHoatDongMTVM obj) {
            return ExecuteContainer(() =>
            {
                var add = new DiaBanHoatDong();
                add = obj.GetDiaBanHongDong(add);
                add.Id = Guid.NewGuid();
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                _context.Attach(add).State = EntityState.Modified;
                _context.DiaBanHoatDongs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.DiaBanHoatDong.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit 
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
            var diaBanHD = _context.DiaBanHoatDongs.SingleOrDefault(it => it.Id == id);
            if (diaBanHD == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            CreateViewBag(diaBanHD.MaTinhThanhPho, diaBanHD.MaQuanHuyen, diaBanHD.MaPhuongXa);
            DiaBanHoatDongVM model = new DiaBanHoatDongVM
            {
                Id = diaBanHD.Id,
                TenDiaBanHoatDong = diaBanHD.TenDiaBanHoatDong,
                DiaChi = diaBanHD.DiaChi,
                MaTinhThanhPho = diaBanHD.MaTinhThanhPho,
                MaQuanHuyen = diaBanHD.MaQuanHuyen,
                MaPhuongXa = diaBanHD.MaPhuongXa,
                Actived = diaBanHD.Actived,
                NgayThanhLap = diaBanHD.NgayThanhLap
                
            };
            DBHoatDongHoiVienVM dbHoiVien = new DBHoatDongHoiVienVM();
            dbHoiVien.IdDiaBan = diaBanHD.Id;
            model.DBHoiVien = dbHoiVien;
            //var hoiViens = _context.DiaBanHoatDongThanhViens.Include(it => it.CanBo).Include(it => it.ChucVu)
            //   .Where(it => it.RoiDi != true && it.IdDiaBan == id).Select(it => new DBHoatDongHoiVienDetailVM
            //   {
            //       Id = it.Id,
            //       IDDiaBan = id,
            //       MaHoiVien = it.CanBo.MaCanBo,
            //       HoVaTen = it.CanBo.HoVaTen,
            //       TenChucVu = it.ChucVu.TenChucVu
            //   }).ToList();
           // model.DBHoiVienDetails = hoiViens;
            return View(model);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DiaBanHoatDongMTVM obj) {
            return ExecuteContainer(() => {
                var edit = _context.DiaBanHoatDongs.SingleOrDefault(it => it.Id == obj.Id);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.DiaBanHoatDong.ToLower())
                    });
                }
                edit = obj.GetDiaBanHongDong(edit);
                HistoryModelRepository history = new HistoryModelRepository(_context);
                history.SaveUpdateHistory(edit.Id.ToString(), AccountId()!.Value, edit);
                _context.Entry(edit).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.DiaBanHoatDong.ToLower())
                });
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
                var del = _context.DiaBanHoatDongs.FirstOrDefault(p => p.Id == id);


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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.DiaBanHoatDong.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.DiaBanHoatDong.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        public void CreateViewBag(String? MaTinhThanhPho = null, String? MaQuanHuyen = null, String? MaPhuongXa = null) {
            var MenuList1 = _context.TinhThanhPhos.Where(it => it.Actived == true).Select(it => new { MaTinhThanhPho = it.MaTinhThanhPho, TenTinhThanhPho = it.TenTinhThanhPho }).ToList();
            ViewBag.MaTinhThanhPho = new SelectList(MenuList1, "MaTinhThanhPho", "TenTinhThanhPho", MaTinhThanhPho);

            var MenuList2 = _context.QuanHuyens.Where(it => it.Actived == true && it.MaTinhThanhPho == MaTinhThanhPho).Select(it => new { MaQuanHuyen = it.MaQuanHuyen, TenQuanHuyen = it.TenQuanHuyen }).ToList();
            ViewBag.MaQuanHuyen = new SelectList(MenuList2, "MaQuanHuyen", "TenQuanHuyen", MaQuanHuyen);

            var MenuList3 = _context.PhuongXas.Where(it => it.Actived == true && it.MaQuanHuyen == MaQuanHuyen).Select(it => new { MaPhuongXa = it.MaPhuongXa, TenPhuongXa = it.TenPhuongXa }).ToList();
            ViewBag.MaPhuongXa = new SelectList(MenuList3, "MaPhuongXa", "TenPhuongXa", MaPhuongXa);

            var NhanSus = _context.CanBos.Select(it => new { IDCanBo = it.IDCanBo, HoVaTen = it.MaCanBo + " " + it.HoVaTen });
            ViewBag.IDCanBo = new SelectList(NhanSus, "IDCanBo", "HoVaTen", null);

            var chucVus = _context.ChucVus.Where(it => it.Actived == true).Select(it => new
            {
                MaChucVu = it.MaChucVu,
                TenChucVu = it.TenChucVu
            }).ToList();
           ViewBag.MaChucVu = new SelectList(chucVus, "MaChucVu", "TenChucVu", null);
        }
        public JsonResult LoadQuanHuyen(string maTinhThanhPho)
        {
            var data = _context.QuanHuyens.Where(it => it.Actived == true && it.MaTinhThanhPho == maTinhThanhPho).OrderBy(p => p.OrderIndex).Select(it => new { MaQuanHuyen = it.MaQuanHuyen, TenQuanHuyen = it.TenQuanHuyen}).ToList();
            return Json(data);
        }
        public JsonResult LoadPhuongXa(string maQuanHuyen)
        {
            var data = _context.PhuongXas.Where(it => it.Actived == true && it.MaQuanHuyen == maQuanHuyen).OrderBy(p => p.OrderIndex).Select(it => new { MaPhuongXa = it.MaPhuongXa, TenPhuongXa = it.TenPhuongXa }).ToList();
            return Json(data);
        }
        [HttpPost]
        public JsonResult AddDiaBanHoiVien(DBHoatDongHoiVienVM DBHoiVien) {
            return ExecuteContainer(() =>
            {
                var add = new DiaBanHoatDong_ThanhVien();
                add.Id = Guid.NewGuid();
                add.IDCanBo = DBHoiVien.IDCanBo;
                add.IdDiaBan = DBHoiVien.IdDiaBan!.Value;
                add.NgayVao = DBHoiVien.NgayVao!.Value;
                add.MaChucVu = DBHoiVien.MaChucVu;
                add.RoiDi = false;
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                _context.Attach(add).State = EntityState.Modified;
                //_context.DiaBanHoatDongThanhViens.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, "Địa bàn thêm hội viên")
                });
            });
        }
        public void CreateViewBagSearch() {
            var MenuList2 = _context.QuanHuyens.Include(it=>it.DiaBanHoatDongs).Where(it => it.Actived == true && it.DiaBanHoatDongs.Count()>0).Select(it => new { MaQuanHuyen = it.MaQuanHuyen, TenQuanHuyen = it.TenQuanHuyen }).Distinct().ToList();
            ViewBag.MaQuanHuyen = new SelectList(MenuList2, "MaQuanHuyen", "TenQuanHuyen");
        }
        public JsonResult LoadPhuongXaSearch(string maQuanHuyen)
        {
            var data = _context.PhuongXas.Include(it=>it.DiaBanHoatDongs).Where(it => it.Actived == true && it.MaQuanHuyen == maQuanHuyen && it.DiaBanHoatDongs.Count()>0).Distinct().OrderBy(p => p.OrderIndex).Select(it => new { MaPhuongXa = it.MaPhuongXa, TenPhuongXa = it.TenPhuongXa }).ToList();
            return Json(data);
        }
        #endregion Helper
    }
}
