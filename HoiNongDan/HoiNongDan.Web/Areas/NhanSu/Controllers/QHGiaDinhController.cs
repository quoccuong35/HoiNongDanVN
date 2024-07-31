using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;


namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    [Authorize]
    public class QHGiaDinhController : BaseController
    {
        public QHGiaDinhController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(QHGiaDinhSearchVM search) {
            return ExecuteSearch(() => { 
                var model = _context.QuanHeGiaDinhs.Where(it=>it.IDHoiVien== null).AsQueryable();
                if (search.IDLoaiQuanHeGiaDinh != null) {
                    model = model.Where(it => it.IDLoaiQuanHeGiaDinh == search.IDLoaiQuanHeGiaDinh);
                }
                model = model.Include(it => it.CanBo).Include(it => it.LoaiQuanhe).Include(it=>it.LoaiQuanhe);
                if (!String.IsNullOrEmpty(search.MaCanBo) && !String.IsNullOrWhiteSpace(search.MaCanBo)) {
                    model = model.Where(it => it.CanBo.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen) && !String.IsNullOrWhiteSpace(search.HoVaTen))
                {
                    model = model.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
                }
                var data = model.Select(it => new QHGiaDinhDetail {
                    IDQuanheGiaDinh = it.IDQuanheGiaDinh,
                    MaCanBo = it.CanBo.MaCanBo,
                    HoVaTen = it.CanBo.HoVaTen,
                    HoTen = it.HoTen,
                    NgaySinh = it.NgaySinh,
                    NgheNghiep = it.NgheNghiep,
                    NoiLamVien = it.NoiLamVien,
                    DiaChi = it.DiaChi,
                    GhiChu = it.GhiChu,
                    TenLoaiQuanHe = it.LoaiQuanhe.TenLoaiQuanHeGiaDinh
                })
                .ToList();
                return PartialView(data);
            });
        }
        #endregion Index
        #region Create
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Create() {
            QHGiaDinhVM model = new QHGiaDinhVM();
            NhanSuThongTinVM NhanSu = new NhanSuThongTinVM();
            NhanSu.CanBo = true;
            model.NhanSu = NhanSu;
            CreateViewBag();
            return View(model);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(QHGiaDinhVMMT obj)
        {
            if (obj.NhanSu.IdCanbo == null)
            {
                ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
            }
            return ExecuteContainer(() => {
                QuanHeGiaDinh add = new QuanHeGiaDinh();
                add = obj.GetQuanHeGiaDinh(add);
                add.IDQuanheGiaDinh = Guid.NewGuid();
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                _context.Attach(add).State = EntityState.Modified;
                _context.QuanHeGiaDinhs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.QuanHeGiaDinh.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Edit(Guid id) {
            var item = _context.QuanHeGiaDinhs.SingleOrDefault(it => it.IDQuanheGiaDinh == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            QHGiaDinhVMMT edit = new QHGiaDinhVMMT();
            edit.IDQuanheGiaDinh = item.IDQuanheGiaDinh;
            edit.IDLoaiQuanHeGiaDinh = item.IDLoaiQuanHeGiaDinh;
            edit.HoTen = item.HoTen;
            edit.NgaySinh = item.NgaySinh;
            edit.NgheNghiep = item.NgheNghiep;
            edit.NoiLamVien = item.NoiLamVien;
            edit.DiaChi = item.DiaChi;
            edit.GhiChu = item.GhiChu;


            var canBo = _context.CanBos.Include(it => it.CoSo).Include(it => it.Department)
                        .Include(it => it.PhanHe).Include(it => it.TinhTrang).Where(it => it.IDCanBo == item.IDCanBo && it.IsCanBo == true).SingleOrDefault();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu = nhanSu.GeThongTin(canBo!);
            nhanSu.CanBo = true;
            nhanSu.IdCanbo = canBo.IDCanBo;
            nhanSu.HoVaTen = canBo.HoVaTen;
            nhanSu.MaCanBo = canBo.MaCanBo;
            nhanSu.TenTinhTrang = canBo.TinhTrang!.TenTinhTrang;
            //nhanSu.TenCoSo = canBo.CoSo.TenCoSo;
            nhanSu.TenDonVi = canBo.Department!.Name;
            //nhanSu.TenPhanHe = canBo.PhanHe.TenPhanHe;
            nhanSu.Edit = false;
            edit.NhanSu = nhanSu;
            CreateViewBag(item.IDLoaiQuanHeGiaDinh);
            return View(edit);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public IActionResult Edit(QHGiaDinhVMMT obj) {
            return ExecuteContainer(() => {
                var edit = _context.QuanHeGiaDinhs.SingleOrDefault(it => it.IDQuanheGiaDinh == obj.IDQuanheGiaDinh);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.QuanHeGiaDinh.ToLower())
                    });
                }
                else 
                {
                    edit =  obj.GetQuanHeGiaDinh(edit);
                    edit.LastModifiedTime = DateTime.Now;
                    edit.LastModifiedAccountId = AccountId();
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.QuanHeGiaDinh.ToLower())
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
                var del = _context.QuanHeGiaDinhs.FirstOrDefault(p => p.IDQuanheGiaDinh == id);


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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.QuanHeGiaDinh.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.QuanHeGiaDinh.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CreateViewBag(Guid? IDLoaiQuanHeGiaDinh = null) {
            var MenuList = _context.LoaiQuanHeGiaDinhs.Where(it => it.Actived == true).Select(it => new { IDLoaiQuanHeGiaDinh = it.IDLoaiQuanHeGiaDinh, TenLoaiQuanHeGiaDinh = it.TenLoaiQuanHeGiaDinh }).ToList();
            ViewBag.IDLoaiQuanHeGiaDinh = new SelectList(MenuList, "IDLoaiQuanHeGiaDinh", "TenLoaiQuanHeGiaDinh", IDLoaiQuanHeGiaDinh);
        }
        #endregion Helper
    }
}
