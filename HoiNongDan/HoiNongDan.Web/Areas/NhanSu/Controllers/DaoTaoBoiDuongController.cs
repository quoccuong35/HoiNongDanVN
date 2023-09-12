using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    [Authorize]
    public class DaoTaoBoiDuongController : BaseController
    {
        public DaoTaoBoiDuongController(AppDbContext context):base(context) { }
        #region Index
        public IActionResult Index()
        {
            CreateViewBag();
            return View(new DaoTaoBoiDuongSearchVM());
        }
        public IActionResult _Search(DaoTaoBoiDuongSearchVM search) {
            return ExecuteSearch(() => {
                var data = _context.DaoTaoBoiDuongs.AsQueryable();
                if (!String.IsNullOrWhiteSpace(search.MaLoaiBangCap)) {
                    data = data.Where(it => it.MaLoaiBangCap == search.MaLoaiBangCap);
                }
                if (!String.IsNullOrWhiteSpace(search.MaHinhThucDaoTao))
                {
                    data = data.Where(it => it.MaHinhThucDaoTao == search.MaHinhThucDaoTao);
                }
                data = data.Include(it => it.CanBo).Include(it => it.LoaiBangCap).Include(it => it.HinhThucDaoTao);
                if (!String.IsNullOrWhiteSpace(search.MaCanBo))
                {
                    data = data.Where(it => it.CanBo.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrWhiteSpace(search.HoVaTen))
                {
                    data = data.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
                }
                var model = data.Select(it => new DaoTaoBoiDuongDetailVM {
                    ID = it.Id,
                    MaCanBo = it.CanBo.MaCanBo,
                    HoVaTen = it.CanBo.HoVaTen,
                    TuNgay = it.TuNgay,
                    DenNgay = it.DenNgay,
                    NoiDungDaoTao = it.NoiDungDaoTao,
                    GhiChu = it.GhiChu,
                    TenHinhThucDaoTao = it.HinhThucDaoTao.TenHinhThucDaoTao,
                    TenLoaiBangCap = it.LoaiBangCap!.TenLoaiBangCap,
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create(Guid? id) {
            DaoTaoBoiDuongVM model = new DaoTaoBoiDuongVM();
            NhanSuThongTinVM nhansu = new NhanSuThongTinVM();
            nhansu.CanBo = true;
            if (id == null)
            {
                nhansu = GetCanBo(id);
            }
            model.NhanSu = nhansu;
            CreateViewBag();
            return View(model);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        public IActionResult Create(DaoTaoBoiDuongMTVM obj) {
            CheckError(obj);
            return ExecuteContainer(() => {
                DaoTaoBoiDuong add = new DaoTaoBoiDuong();
                obj.GetQuaTrinhDaoTao(add);
                add.Id = Guid.NewGuid();
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                _context.Attach(add).State = EntityState.Modified;
                _context.DaoTaoBoiDuongs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.DaoTaoBoiDuong.ToLower())
                });

            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
            var item = _context.DaoTaoBoiDuongs.SingleOrDefault(it => it.Id == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            DaoTaoBoiDuongVM model = new DaoTaoBoiDuongVM();
            model.NhanSu = GetCanBo(item.IDCanBo);
            model.ID = item.Id;
            model.TuNgay = item.TuNgay;
            model.DenNgay = item.DenNgay;
            model.NoiDungDaoTao = item.NoiDungDaoTao;
            model.GhiChu = item.GhiChu;
            model.MaHinhThucDaoTao = item.MaHinhThucDaoTao;
            model.MaLoaiBangCap = item.MaLoaiBangCap!;
            CreateViewBag(item.MaLoaiBangCap, item.MaHinhThucDaoTao);
            return View(model);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        public IActionResult Edit(DaoTaoBoiDuongMTVM obj) {
            CheckError(obj);
            return ExecuteContainer(() => {
                var edit = _context.DaoTaoBoiDuongs.SingleOrDefault(it => it.Id == obj.ID);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.DaoTaoBoiDuong.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetQuaTrinhDaoTao(edit);
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.DaoTaoBoiDuong.ToLower())
                    });
                }
            });
        }
        #endregion Edit
        #region Del
        [HttpDelete]
        [HoiNongDanAuthorization]
        //[ValidateAntiForgeryToken]
        public JsonResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.DaoTaoBoiDuongs.FirstOrDefault(p => p.Id == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.DaoTaoBoiDuong.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.DaoTaoBoiDuong.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        private void CheckError(DaoTaoBoiDuongMTVM obj) { 
            if(obj.TuNgay != null && obj.DenNgay != null && obj.DenNgay < obj.TuNgay)
            {
                ModelState.AddModelError("DenNgay", "Đến ngày không hợp lệ");
            }
        }
        private void CreateViewBag(String? MaLoaiBangCap = null, String? MaHinhThucDaoTao = null)
        {
            var MenuList1 = _context.HinhThucDaoTaos.Select(it => new { MaHinhThucDaoTao = it.MaHinhThucDaoTao, TenHinhThucDaoTao = it.TenHinhThucDaoTao }).ToList();
            ViewBag.MaHinhThucDaoTao = new SelectList(MenuList1, "MaHinhThucDaoTao", "TenHinhThucDaoTao", MaHinhThucDaoTao);

            var MenuList2 = _context.LoaiBangCaps.Select(it => new { MaLoaiBangCap = it.MaLoaiBangCap, TenLoaiBangCap = it.TenLoaiBangCap }).ToList();
            ViewBag.MaLoaiBangCap = new SelectList(MenuList2, "MaLoaiBangCap", "TenLoaiBangCap", MaLoaiBangCap);
        }
        private  NhanSuThongTinVM GetCanBo(Guid? id)
        {
            var canBo = _context.CanBos.Include(it => it.CoSo).Include(it => it.Department)
                        .Include(it => it.PhanHe).Include(it => it.TinhTrang).Where(it => it.IDCanBo == id).SingleOrDefault();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu.CanBo = true;
            if (canBo != null)
            {
                nhanSu = nhanSu.GeThongTin(canBo);
            }
            return nhanSu;
        }
        #endregion Helper
    }
}
