using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portal.Constant;
using Portal.DataAccess;
using Portal.Extensions;
using Portal.Models;
using Portal.Resources;
using System.Text.RegularExpressions;

namespace Portal.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    [Authorize]
    public class BoiDuongController : BaseController
    {
        public BoiDuongController(AppDbContext context) : base(context) { }
        #region Index
        [PortalAuthorization]
        [HttpGet]
        public IActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        public IActionResult _Search(BoiDuongSearchVM search) {
            return ExecuteSearch(() => {
                var data = _context.QuaTrinhBoiDuongs.AsQueryable();
                if (!String.IsNullOrEmpty(search.MaHinhThucDaoTao) &&
                !String.IsNullOrWhiteSpace(search.MaHinhThucDaoTao))
                {
                    data = data.Where(it => it.MaHinhThucDaoTao == search.MaHinhThucDaoTao);
                }
                data = data.Include(it => it.CanBo).Include(it => it.HinhThucDaoTao);
                if (!String.IsNullOrEmpty(search.MaCanBo) && !String.IsNullOrWhiteSpace(search.MaCanBo))
                {
                    data = data.Where(it => it.CanBo.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen) && !String.IsNullOrWhiteSpace(search.HoVaTen))
                {
                    data = data.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
                }
                var model = data.Select(it => new BoiDuongDetai
                {
                    IDQuaTrinhBoiDuong = it.IDQuaTrinhBoiDuong,
                    MaCanBo = it.CanBo.MaCanBo,
                    HoVaTen = it.CanBo.HoVaTen,
                    NoiBoiDuong = it.NoiBoiDuong,
                    NoiDung = it.NoiDung,
                    NgayBatDau = it.NgayBatDau,
                    NgayKetThuc = it.NgayKetThuc,
                    GhiChu = it.GhiChu,
                    TenHinhThucDaoTao = it.HinhThucDaoTao.TenHinhThucDaoTao,
                });
                return PartialView(model);
            });
        }
        #endregion Index
        #region Create 
        [HttpGet]
        [PortalAuthorization]
        public IActionResult Create() {
            BoiDuongVM boiDuongVM = new BoiDuongVM();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu.CanBo = true;
            boiDuongVM.NhanSu = nhanSu;
            CreateViewBag();
            return View(boiDuongVM);
        }
        public JsonResult Create(BoiDuongMTVM obj) {
            if (obj.NhanSu.IdCanbo == null)
            {
                ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
            }
            return ExecuteContainer(() => {
                var add = new QuaTrinhBoiDuong();
                add = obj.GetQuaTrinhBoiDuong(add);
                add.IDQuaTrinhBoiDuong = Guid.NewGuid();
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                _context.Attach(add).State = EntityState.Modified;
                _context.QuaTrinhBoiDuongs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.BoiDuong.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [PortalAuthorization]
        public IActionResult Edit(Guid id)
        {
            var item = _context.QuaTrinhBoiDuongs.SingleOrDefault(it => it.IDQuaTrinhBoiDuong == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            BoiDuongVM obj = new BoiDuongVM();
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

            obj.MaHinhThucDaoTao = item.MaHinhThucDaoTao;
            obj.NoiBoiDuong = item.NoiBoiDuong;
            obj.NoiDung = item.NoiDung;
            obj.NgayBatDau = item.NgayBatDau;
            obj.NgayKetThuc = item.NgayKetThuc;
            obj.GhiChu = item.GhiChu;
            obj.NhanSu = nhanSu;
            obj.IDQuaTrinhBoiDuong = item.IDQuaTrinhBoiDuong;
            CreateViewBag(item.MaHinhThucDaoTao);
            return View(obj);
        }
        public JsonResult Edit(BoiDuongMTVM obj)
        {
            return ExecuteContainer(() => {
                var edit = _context.QuaTrinhBoiDuongs.SingleOrDefault(it => it.IDQuaTrinhBoiDuong == obj.IDQuaTrinhBoiDuong);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.BoiDuong.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetQuaTrinhBoiDuong(edit);
                    edit.LastModifiedTime = DateTime.Now;
                    edit.LastModifiedAccountId = AccountId();
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.BoiDuong.ToLower())
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
                var del = _context.QuaTrinhBoiDuongs.FirstOrDefault(p => p.IDQuaTrinhBoiDuong == id);


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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.BoiDuong.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.BoiDuong.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CreateViewBag(String? MaHinhThucDaoTao = null)
        {
            var MenuList1 = _context.HinhThucDaoTaos.Select(it => new { MaHinhThucDaoTao = it.MaHinhThucDaoTao, TenHinhThucDaoTao = it.TenHinhThucDaoTao }).ToList();
            ViewBag.MaHinhThucDaoTao = new SelectList(MenuList1, "MaHinhThucDaoTao", "TenHinhThucDaoTao", MaHinhThucDaoTao);

           
        }
        #endregion Helper
    }
}
