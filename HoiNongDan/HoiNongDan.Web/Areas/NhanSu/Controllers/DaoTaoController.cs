using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys.NhanSu;
using HoiNongDan.Resources;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class DaoTaoController : BaseController
    {
        public DaoTaoController(AppDbContext context) : base(context) { }
        //#region Index
        //[HoiNongDanAuthorization]
        //public IActionResult Index()
        //{
        //    CreateViewBag();
        //    return View();
        //}
        //public IActionResult _Search(DaoTaoSearchVM search) {
        //    return ExecuteSearch(() => {
        //        var data = _context.QuaTrinhDaoTaos.AsQueryable();
        //        if (!String.IsNullOrEmpty(search.MaLoaiBangCap) && !String.IsNullOrWhiteSpace(search.MaLoaiBangCap))
        //        {
        //            data = data.Where(it => it.MaLoaiBangCap == search.MaLoaiBangCap);
        //        }
        //        if (!String.IsNullOrEmpty(search.MaHinhThucDaoTao) && !String.IsNullOrWhiteSpace(search.MaHinhThucDaoTao))
        //        {
        //            data = data.Where(it => it.MaHinhThucDaoTao == search.MaHinhThucDaoTao);
        //        }
        //        if (!String.IsNullOrEmpty(search.MaChuyenNganh) && !String.IsNullOrWhiteSpace(search.MaChuyenNganh))
        //        {
        //            data = data.Where(it => it.MaChuyenNganh == search.MaChuyenNganh);
        //        }
        //        data = data.Include(it=>it.CanBo).Include(it=>it.ChuyenNganh).Include(it=>it.LoaiBangCap).Include(it=>it.HinhThucDaoTao);
        //        if (!String.IsNullOrEmpty(search.MaCanBo) && !String.IsNullOrWhiteSpace(search.MaCanBo))
        //        {
        //            data = data.Where(it => it.CanBo.MaCanBo == search.MaCanBo);
        //        }
        //        if (!String.IsNullOrEmpty(search.HoVaTen) && !String.IsNullOrWhiteSpace(search.HoVaTen))
        //        {
        //            data = data.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
        //        }
        //        var model = data.Select(it => new DaoTaoDetailVM {
        //            IDQuaTrinhDaoTao = it.IDQuaTrinhDaoTao,
        //            TenChuyenNganh = it.ChuyenNganh.TenChuyenNganh,
        //            TenHinhThucDaoTao = it.HinhThucDaoTao.TenHinhThucDaoTao,
        //            TenLoaiBangCap = it.LoaiBangCap.TenLoaiBangCap,
        //            CoSoDaoTao = it.CoSoDaoTao,
        //            NgayTotNghiep = it.NgayTotNghiep,
        //            QuocGia =it.QuocGia,
        //            GhiChu = it.GhiChu,
        //            LuanAnTN = it.LuanAnTN,
        //            FileDinhKem = it.FileDinhKem,
        //            MaCanBo = it.CanBo.MaCanBo,
        //            HoVaTen = it.CanBo.HoVaTen

        //        }).ToList();
        //        return PartialView(model);
        //    });
        //}
        //#endregion Index
        //#region Create
        //[HttpGet]
        //[HoiNongDanAuthorization]
        //public IActionResult Create() {
        //    QuaTrinhDaoTaoVM daoTao = new QuaTrinhDaoTaoVM();
        //    NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
        //    nhanSu.CanBo = true;
        //    daoTao.NhanSu = nhanSu;
        //    CreateViewBag();
        //    return View(daoTao);
        //}
        //[HttpPost]
        //[HoiNongDanAuthorization]
        //public JsonResult Create(QuaTrinhDaoTaoMTVM obj) {
        //    if (obj.NhanSu.IdCanbo == null)
        //    {
        //        ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
        //    }
        //    return ExecuteContainer(() => {
        //        QuaTrinhDaoTao add = new QuaTrinhDaoTao();
        //        add = obj.GetQuaTrinhDaoTao(add);
        //        add.IDQuaTrinhDaoTao = Guid.NewGuid();
        //        add.CreatedTime = DateTime.Now;
        //        add.CreatedAccountId = AccountId();
        //        _context.Attach(add).State = EntityState.Modified;
        //        _context.QuaTrinhDaoTaos.Add(add);
        //        _context.SaveChanges();
        //        return Json(new
        //        {
        //            Code = System.Net.HttpStatusCode.OK,
        //            Success = true,
        //            Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.DaoTao.ToLower())
        //        });
        //    });
        //}
        //#endregion Create
        //#region Edit
        //[HttpGet]
        //[HoiNongDanAuthorization]
        //public IActionResult Edit(Guid id) {
        //    var item = _context.QuaTrinhDaoTaos.SingleOrDefault(it => it.IDQuaTrinhDaoTao == id);
        //    if (item == null)
        //    {
        //        return Redirect("~/Error/ErrorNotFound?data=" + id);
        //    }
        //    QuaTrinhDaoTaoVM obj = new QuaTrinhDaoTaoVM();
        //    var canBo = _context.CanBos.Include(it => it.CoSo).Include(it => it.Department)
        //                .Include(it => it.PhanHe).Include(it => it.TinhTrang).Where(it => it.IDCanBo == item.IDCanBo && it.IsCanBo == true).SingleOrDefault();
        //    NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
        //    nhanSu = nhanSu.GeThongTin(canBo);
        //    nhanSu.CanBo = true;
        //    nhanSu.IdCanbo = canBo.IDCanBo;
        //    nhanSu.HoVaTen = canBo.HoVaTen;
        //    nhanSu.MaCanBo = canBo.MaCanBo;
        //    nhanSu.TenTinhTrang = canBo.TinhTrang.TenTinhTrang;
        //    nhanSu.TenCoSo = canBo.CoSo.TenCoSo;
        //    nhanSu.TenDonVi = canBo.Department.Name;
        //    nhanSu.TenPhanHe = canBo.PhanHe.TenPhanHe;
        //    nhanSu.Edit = false;

        //    obj.MaChuyenNganh = item.MaChuyenNganh;
        //    obj.MaLoaiBangCap = item.MaLoaiBangCap;
        //    obj.MaHinhThucDaoTao = item.MaHinhThucDaoTao;
        //    obj.CoSoDaoTao = item.CoSoDaoTao;
        //    obj.NgayTotNghiep = item.NgayTotNghiep;
        //    obj.QuocGia = item.QuocGia;
        //    obj.LuanAnTN = item.LuanAnTN == null?false:item.LuanAnTN.Value;
        //    obj.GhiChu = item.GhiChu;
        //    obj.NhanSu = nhanSu;
        //    obj.IDQuaTrinhDaoTao = item.IDQuaTrinhDaoTao;
        //    CreateViewBag(item.MaChuyenNganh, item.MaLoaiBangCap,item.MaHinhThucDaoTao);
        //    return View(obj);
        //}
        //[HttpPost]
        //[HoiNongDanAuthorization]
        //public JsonResult Edit(QuaTrinhDaoTaoMTVM obj)
        //{
        //    return ExecuteContainer(() => {
        //        var edit = _context.QuaTrinhDaoTaos.SingleOrDefault(it => it.IDQuaTrinhDaoTao == obj.IDQuaTrinhDaoTao);
        //        if (edit == null)
        //        {
        //            return Json(new
        //            {
        //                Code = System.Net.HttpStatusCode.NotFound,
        //                Success = false,
        //                Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.DaoTao.ToLower())
        //            });
        //        }
        //        else
        //        {
        //            edit = obj.GetQuaTrinhDaoTao(edit);
        //            edit.LastModifiedTime = DateTime.Now;
        //            edit.LastModifiedAccountId = AccountId();
        //            _context.Entry(edit).State = EntityState.Modified;
        //            _context.SaveChanges();
        //            return Json(new
        //            {
        //                Code = System.Net.HttpStatusCode.OK,
        //                Success = true,
        //                Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.DaoTao.ToLower())
        //            });
        //        }
        //    });
        //}
        //#endregion Edit
        //#region Delete
        //[HttpDelete]
        //public JsonResult Delete(Guid id)
        //{
        //    return ExecuteDelete(() =>
        //    {
        //        var del = _context.QuaTrinhDaoTaos.FirstOrDefault(p => p.IDQuaTrinhDaoTao == id);


        //        if (del != null)
        //        {
        //            //_context.Entry(accountInRoleModels).State = EntityState.Deleted;
        //            //_context.Entry(account).State = EntityState.Deleted;
        //            _context.Remove(del);
        //            _context.SaveChanges();

        //            return Json(new
        //            {
        //                Code = System.Net.HttpStatusCode.OK,
        //                Success = true,
        //                Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.DaoTao.ToLower())
        //            });
        //        }
        //        else
        //        {
        //            return Json(new
        //            {
        //                Code = System.Net.HttpStatusCode.NotModified,
        //                Success = false,
        //                Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.DaoTao.ToLower())
        //            });
        //        }
        //    });
        //}
        //#endregion Delete
        //#region Helper
        //private void CreateViewBag(String? MaChuyenNganh = null,String? MaLoaiBangCap = null, String? MaHinhThucDaoTao = null) {
        //    var MenuList = _context.ChuyenNganhs.Select(it => new { MaChuyenNganh = it.MaChuyenNganh, TenChuyenNganh = it.TenChuyenNganh }).ToList();
        //    ViewBag.MaChuyenNganh = new SelectList(MenuList, "MaChuyenNganh", "TenChuyenNganh", MaChuyenNganh);

        //    var MenuList1 = _context.HinhThucDaoTaos.Select(it => new { MaHinhThucDaoTao = it.MaHinhThucDaoTao, TenHinhThucDaoTao = it.TenHinhThucDaoTao }).ToList();
        //    ViewBag.MaHinhThucDaoTao = new SelectList(MenuList1, "MaHinhThucDaoTao", "TenHinhThucDaoTao", MaHinhThucDaoTao);

        //    var MenuList2 = _context.LoaiBangCaps.Select(it => new { MaLoaiBangCap = it.MaLoaiBangCap, TenLoaiBangCap = it.TenLoaiBangCap }).ToList();
        //    ViewBag.MaLoaiBangCap = new SelectList(MenuList2, "MaLoaiBangCap", "TenLoaiBangCap", MaLoaiBangCap);
        //}
        //#endregion Helper
    }
}
