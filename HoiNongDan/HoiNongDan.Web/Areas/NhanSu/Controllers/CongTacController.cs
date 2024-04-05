using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.ViewModels.Masterdata;
using HoiNongDan.Resources;
using HoiNongDan.Web.Areas.NhanSu.Models;
using System.IO;
using System.Transactions;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    [Authorize]
    public class CongTacController : BaseController
    {
        // Quá  trình công tác
        public CongTacController(AppDbContext context) : base(context)
        {
        }
        #region Index
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(QuaTrinhCongTacSeachVM search) {
            return ExecuteSearch(() => {
                var data = _context.QuaTrinhCongTacs.Include(it=>it.CanBo).Include(it=>it.ChucVu).AsQueryable();
                if (!String.IsNullOrEmpty(search.MaCanBo) && !String.IsNullOrWhiteSpace(search.MaCanBo))
                {
                    data = data.Where(it => it.CanBo.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen) && !String.IsNullOrWhiteSpace(search.HoVaTen))
                {
                    data = data.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
                }
                var model = data.Select(it=>new QuaTrinhCongTacDetailVM { 
                    IDQuaTrinhCongTac = it.IDQuaTrinhCongTac,
                    TuNgay = it.TuNgay,
                    DenNgay = it.DenNgay,
                    NoiLamViec = it.NoiLamViec,
                    GhiChu = it.GhiChu,
                    TenChucVu = it.ChucVu.TenChucVu,
                    MaCanBo = it.CanBo.MaCanBo,
                    HoVaTen = it.CanBo.HoVaTen
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create() {
            QuaTrinhCongTacVM congTac = new QuaTrinhCongTacVM();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu.CanBo = true;
            congTac.NhanSu = nhanSu;
            CreateViewBag();
            return View(congTac);
            
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Create(QuaTrinhCongTacMTVM obj) 
        {
            CheckError(obj);
            return ExecuteContainer(() => {
                QuaTrinhCongTac add = new QuaTrinhCongTac();
                add.IsBanChapHanh = false;
                add = obj.GetQuaTrinhCongTac(add);
                add.IDQuaTrinhCongTac = Guid.NewGuid();
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                _context.QuaTrinhCongTacs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.CongTac.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
            var item = _context.QuaTrinhCongTacs.SingleOrDefault(it => it.IDQuaTrinhCongTac == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            QuaTrinhCongTacVM obj = new QuaTrinhCongTacVM();


            var canBo = _context.CanBos.Include(it => it.CoSo).Include(it => it.Department)
                        .Include(it => it.PhanHe).Include(it => it.TinhTrang).Where(it => it.IDCanBo == item.IDCanBo && it.IsCanBo == true).SingleOrDefault();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu = nhanSu.GeThongTin(canBo);
            nhanSu.CanBo = true;
            nhanSu.IdCanbo = canBo!.IDCanBo;
            nhanSu.HoVaTen = canBo.HoVaTen;
            nhanSu.MaCanBo = canBo.MaCanBo;
            nhanSu.TenTinhTrang = canBo.TinhTrang.TenTinhTrang;

            nhanSu.TenDonVi = canBo.Department.Name;
            nhanSu.Edit = false;

            obj.IDQuaTrinhCongTac = item.IDQuaTrinhCongTac;
            obj.TuNgay = item.TuNgay;
            obj.MaChucVu = item.MaChucVu;
            obj.DenNgay = item.DenNgay;
            obj.NoiLamViec = item.NoiLamViec;
            obj.GhiChu = item.GhiChu;
            obj.NhanSu = nhanSu;
            obj.IsBanChapHanh = item.IsBanChapHanh;
            obj.NhiemKy = item.NhiemKy;
            CreateViewBag(item.MaChucVu);
            return View(obj);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(QuaTrinhCongTacMTVM obj) {
            CheckError(obj);
            return ExecuteContainer(() => {
                var edit = _context.QuaTrinhCongTacs.SingleOrDefault(it => it.IDQuaTrinhCongTac == obj.IDQuaTrinhCongTac);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.CongTac.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetQuaTrinhCongTac(edit);
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.CongTac.ToLower())
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
                var del = _context.QuaTrinhCongTacs.FirstOrDefault(p => p.IDQuaTrinhCongTac == id);


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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.CongTac.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.CongTac.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CheckError(QuaTrinhCongTacMTVM obj)
        {
            if (obj.NhanSu.IdCanbo == null)
            {
                ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
            }
        }
        private void CreateViewBag(Guid? MaChucVu = null)
        {
            var MenuList1 = _context.ChucVus.Select(it => new { MaChucVu = it.MaChucVu, TenChucVu = it.TenChucVu }).ToList();
            ViewBag.MaChucVu = new SelectList(MenuList1, "MaChucVu", "TenChucVu", MaChucVu);
        }
        #endregion Helper
    }
}
