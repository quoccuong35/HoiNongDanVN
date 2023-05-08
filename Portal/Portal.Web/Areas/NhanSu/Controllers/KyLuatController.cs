using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portal.Constant;
using Portal.DataAccess;
using Portal.Extensions;
using Portal.Models;
using Portal.Models.Entitys.NhanSu;
using Portal.Resources;


namespace Portal.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    [Authorize]
    public class KyLuatController : BaseController
    {
        public KyLuatController(AppDbContext context) : base(context) { 
        }
        #region Index
        public IActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        public IActionResult _Search(KyLuatSearchVM search) {
            return ExecuteSearch(() => {
                var model = _context.QuaTrinhKyLuats.AsQueryable();
                if (!String.IsNullOrEmpty(search.MaHinhThucKyLuat) && !String.IsNullOrWhiteSpace(search.MaHinhThucKyLuat))
                {
                    model = model.Where(it => it.MaHinhThucKyLuat == search.MaHinhThucKyLuat);
                }
                model = model.Include(it => it.CanBo).Include(it => it.HinhThucKyLuat);
                if (!String.IsNullOrEmpty(search.MaCanBo) && !String.IsNullOrWhiteSpace(search.MaCanBo))
                {
                    model = model.Where(it => it.CanBo.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen) && !String.IsNullOrWhiteSpace(search.HoVaTen))
                {
                    model = model.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
                }
                var data = model.Select(it => new KyLuatDetailVM
                {
                    IdQuaTrinhKyLuat = it.IdQuaTrinhKyLuat,
                    MaCanBo = it.CanBo.MaCanBo,
                    HoVaTen = it.CanBo.HoVaTen,
                    SoQuyetDinh = it.SoQuyetDinh,
                    NgayKy = it.NgayKy,
                    LyDo = it.LyDo,
                    NguoiKy = it.NguoiKy,
                    GhiChu = it.GhiChu,
                    TenHinhThucKyLuat = it.HinhThucKyLuat.TenHinhThucKyLuat,
                }).ToList();
                return PartialView(data);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [PortalAuthorization]
        public IActionResult Create() { 
            KyLuatVM kyLuatVM = new KyLuatVM();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu.CanBo = true;
            kyLuatVM.NhanSu = nhanSu;
            CreateViewBag();
            return View(kyLuatVM);
        }
        [HttpPost]
        [PortalAuthorization]
        public JsonResult Create(KyLuatVMMT obj) {
            if (obj.NhanSu.IdCanbo == null)
            {
                ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
            }
            return ExecuteContainer(() => {
                QuaTrinhKyLuat add = new QuaTrinhKyLuat();
                add = obj.GetKyLuat(add);
                add.IdQuaTrinhKyLuat = Guid.NewGuid();
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                _context.Attach(add).State = EntityState.Modified;
                _context.QuaTrinhKyLuats.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.KyLuat.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [PortalAuthorization]
        public IActionResult Edit(Guid id)
        {
            var item = _context.QuaTrinhKyLuats.SingleOrDefault(it => it.IdQuaTrinhKyLuat == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            KyLuatVM obj = new KyLuatVM();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu.CanBo = true;
            var canBo = _context.CanBos.Include(it => it.CoSo).Include(it => it.Department)
                        .Include(it => it.PhanHe).Include(it => it.TinhTrang).Where(it => it.IDCanBo == item.IDCanBo).SingleOrDefault();
            nhanSu.IdCanbo = canBo.IDCanBo;
            nhanSu.HoVaTen = canBo.HoVaTen;
            nhanSu.MaCanBo = canBo.MaCanBo;
            nhanSu.TenTinhTrang = canBo.TinhTrang.TenTinhTrang;
            nhanSu.TenCoSo = canBo.CoSo.TenCoSo;
            nhanSu.TenDonVi = canBo.Department.Name;
            nhanSu.TenPhanHe = canBo.PhanHe.TenPhanHe;
            nhanSu.Edit = false;

            obj.IdQuaTrinhKyLuat = item.IdQuaTrinhKyLuat;
            obj.MaHinhThucKyLuat = item.MaHinhThucKyLuat;
            obj.NgayKy = item.NgayKy;
            obj.SoQuyetDinh = item.SoQuyetDinh;
            obj.LyDo = item.LyDo;
            obj.NguoiKy = item.NguoiKy;
            obj.GhiChu = item.GhiChu;
            obj.NhanSu = nhanSu;
            CreateViewBag(item.MaHinhThucKyLuat);
            return View(obj);
        }
        [HttpPost]
        [PortalAuthorization]
        public JsonResult Edit(KyLuatVMMT obj)
        {
            return ExecuteContainer(() => {
                var edit = _context.QuaTrinhKyLuats.SingleOrDefault(it => it.IdQuaTrinhKyLuat == obj.IdQuaTrinhKyLuat);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.KyLuat.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetKyLuat(edit);
                    edit.LastModifiedTime = DateTime.Now;
                    edit.LastModifiedAccountId = AccountId();
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.KyLuat.ToLower())
                    });
                }
            });
        }
        #endregion Edit
        #region Delete
        [HttpDelete]
        public JsonResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.QuaTrinhKyLuats.FirstOrDefault(p => p.IdQuaTrinhKyLuat == id);


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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.KyLuat.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.KyLuat.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CreateViewBag(String? MaHinhThucKyLuat = null)
        {

            var MenuList = _context.HinhThucKyLuats.Select(it => new { MaHinhThucKyLuat = it.MaHinhThucKyLuat, TenHinhThucKyLuat = it.TenHinhThucKyLuat }).ToList();
            ViewBag.MaHinhThucKyLuat = new SelectList(MenuList, "MaHinhThucKyLuat", "TenHinhThucKyLuat", MaHinhThucKyLuat);
        }
        #endregion Helper
    }
}
