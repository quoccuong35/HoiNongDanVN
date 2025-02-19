using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;


namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    [Authorize]
    public class HVQHGiaDinhController : BaseController
    {
        public HVQHGiaDinhController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBagSearch();
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(QHGiaDinhSearchVM search) {
            return ExecuteSearch(() => { 

                var model = (from qh in _context.QuanHeGiaDinhs
                             join hv in _context.CanBos on qh.IDCanBo equals hv.IDCanBo
                             join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                             where hv.IsHoiVien == true
                             && pv.AccountId == AccountId()
                             select qh).Include(it => it.CanBo).Include(it => it.LoaiQuanhe).Include(it => it.LoaiQuanhe).AsQueryable();
                if (search.IDLoaiQuanHeGiaDinh != null) {
                    model = model.Where(it => it.IDLoaiQuanHeGiaDinh == search.IDLoaiQuanHeGiaDinh);
                }

                if (!String.IsNullOrEmpty(search.SoCCCD) && !String.IsNullOrWhiteSpace(search.SoCCCD)) {
                    model = model.Where(it => it.CanBo.SoCCCD == search.SoCCCD);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen) && !String.IsNullOrWhiteSpace(search.HoVaTen))
                {
                    model = model.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
                }
                var data = model.Select(it => new QHGiaDinhDetail {
                    IDQuanheGiaDinh = it.IDQuanheGiaDinh,
                    MaCanBo = it.CanBo.MaCanBo!,
                    HoVaTen = it.CanBo.HoVaTen,
                    HoTen = it.HoTen,
                    NgaySinh = it.NgaySinh,
                    NgheNghiep = it.NgheNghiep,
                    NoiLamViec = it.NoiLamViec,
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
            HoiVienInfo NhanSu = new HoiVienInfo();

            model.HoiVien = NhanSu;
            CreateViewBag();
            return View(model);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(QHGiaDinhVMMT obj)
        {
            if (obj.HoiVien.IdCanbo == null)
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
            edit.NoiLamViec = item.NoiLamViec;
            edit.DiaChi = item.DiaChi;
            edit.GhiChu = item.GhiChu;

            edit.HoiVien = Function.GetThongTinHoiVien(maHoiVien:item.IDCanBo,_context);
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
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.IDLoaiQuanHeGiaDinh = fnViewBag.LoaiQuanHeGiaDinh(value:IDLoaiQuanHeGiaDinh);
        }
        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.IDLoaiQuanHeGiaDinh = fnViewBag.LoaiQuanHeGiaDinh();


            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }
        [NonAction]
        private HoiVienInfo GetThongTinNhanSu(Guid maHoiVien)
        {
            HoiVienInfo HoiVien = new HoiVienInfo();
            var phamVis = Function.GetPhamVi(AccountId: AccountId()!.Value, _context: _context);
            var data = _context.CanBos.FirstOrDefault(it => it.IDCanBo == maHoiVien && phamVis.Contains(it.MaDiaBanHoatDong!.Value) && it.IsHoiVien == true);
            var diaBan = _context.DiaBanHoatDongs.SingleOrDefault(it => it.Id == data!.MaDiaBanHoatDong);
            var quanThanhPho = _context.QuanHuyens.SingleOrDefault(it => it.MaQuanHuyen == diaBan!.MaQuanHuyen);
            HoiVien.IdCanbo = data!.IDCanBo;
            HoiVien.HoVaTen = data.HoVaTen;
            HoiVien.MaCanBo = data.MaCanBo!;
            HoiVien.DiaBan = diaBan!.TenDiaBanHoatDong;
            HoiVien.NgaySinh = data!.NgaySinh;
            HoiVien.HoKhauThuongTru = data.HoKhauThuongTru;
            HoiVien.SoCCCD = data.SoCCCD;
            HoiVien.QuanHuyen = quanThanhPho!.TenQuanHuyen;
            HoiVien.Edit = false;
            return HoiVien;
        }
        #endregion Helper
    }
}
