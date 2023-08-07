using HoiNongDan.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HoiNongDan.Extensions;
using HoiNongDan.DataAccess;
using HoiNongDan.Models;
using HoiNongDan.Resources;


namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class VayVonController : BaseController
    {
        public VayVonController(AppDbContext context) : base(context) { }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            VayVonSearchVM model = new VayVonSearchVM();
            return View(model);
        }
        public IActionResult _Search(VayVonSearchVM search)
        {
            return ExecuteSearch(() => {
                var data = _context.HoiVienVayVons.Include(it=>it.CanBo).AsQueryable();
                if (!String.IsNullOrEmpty(search.MaHV) && !String.IsNullOrWhiteSpace(search.MaHV))
                {
                    data = data.Where(it => it.CanBo.MaCanBo == search.MaHV);
                }
                if (!String.IsNullOrEmpty(search.TenHV) && !String.IsNullOrWhiteSpace(search.TenHV))
                {
                    data = data.Where(it => it.CanBo.HoVaTen.Contains(search.TenHV));
                }
                if (search.Actived != null)
                {
                    data = data.Where(it => it.Actived == search.Actived);
                }
                if (search.NamVayVon != null)
                {
                    data = data.Where(it => it.TuNgay.Year == search.NamVayVon);
                }
                var model = data.Select(it => new VayVonDetailVM
                {
                    IDVayVon = it.IDVayVon,
                    MaHV = it.CanBo.MaCanBo,
                    TenHV = it.CanBo.HoVaTen,
                    SoTienVay = it.SoTienVay,
                    ThoiHangChoVay = it.ThoiHangChoVay,
                    LaiSuatVay = it.LaiSuatVay,
                    TuNgay = it.TuNgay,
                    DenNgay = it.DenNgay,
                    NgayTraNoCuoiCung = it.NgayTraNoCuoiCung,
                    NoiDungVay = it.NoiDungVay,
                    TraXong = it.TraXong,
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create() {
            VayVonVM item = new VayVonVM();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            item.SoTienVay = "1,000,000";
            nhanSu.CanBo = false;
            item.NhanSu = nhanSu;
            return View(item);
        }
        public JsonResult Create(VayVonMTVM obj) {
            CheckError(obj);
            return ExecuteContainer(() => {
                var add = new HoiVienVayVon();
                add = obj.GetVayVon(add);
                add.IDVayVon = Guid.NewGuid();
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                add.TraXong = false;
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.HoiVienVayVons.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.HoiVienVayVon.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
            var item = _context.HoiVienVayVons.SingleOrDefault(it => it.IDVayVon == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            VayVonVM obj = new VayVonVM();


            var canBo = _context.CanBos.Include(it => it.CoSo).Include(it => it.Department)
                        .Include(it => it.PhanHe).Include(it => it.TinhTrang).Where(it => it.IDCanBo == item.IDCanBo).SingleOrDefault();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu = nhanSu.GeThongTin(canBo);
            nhanSu.CanBo = false;
            nhanSu.IdCanbo = canBo!.IDCanBo;
            nhanSu.HoVaTen = canBo.HoVaTen;
            nhanSu.MaCanBo = canBo.MaCanBo;
            nhanSu.TenTinhTrang = canBo.TinhTrang.TenTinhTrang;
            nhanSu.TenCoSo = canBo.CoSo.TenCoSo;
            nhanSu.TenDonVi = canBo.Department.Name;
            nhanSu.TenPhanHe = canBo.PhanHe.TenPhanHe;
            nhanSu.Edit = false;

            obj.IDVayVon = item.IDVayVon;
            obj.SoTienVay = item.SoTienVay.ToString("N0");
            obj.LaiSuatVay = item.LaiSuatVay;
            obj.ThoiHangChoVay = item.ThoiHangChoVay;
            obj.TuNgay = item.TuNgay;
            obj.DenNgay = item.DenNgay;
            obj.NgayTraNoCuoiCung = item.NgayTraNoCuoiCung;
            obj.NoiDungVay = item.NoiDungVay;
            obj.GhiChu = item.GhiChu;
            obj.NhanSu = nhanSu;
            obj.TraXong = item.TraXong == null?false:item.TraXong;
            return View(obj);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        public JsonResult Edit(VayVonMTVM obj)
        {
            CheckError(obj);
            return ExecuteContainer(() => {
                var edit = _context.HoiVienVayVons.SingleOrDefault(it => it.IDVayVon == obj.IDVayVon);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.HoiVienVayVon.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetVayVon(edit);
                    edit.TraXong = obj.TraXong;
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.HoiVienVayVon.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Del
        [HttpDelete]
        [HoiNongDanAuthorization]
        //[ValidateAntiForgeryToken]
        public JsonResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.HoiVienVayVons.FirstOrDefault(p => p.IDVayVon == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.HoiVienVayVon.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.HoiVienVayVon.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Report
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Report(){
            return View(new VayVonQuaHanSearchVM { Ngay = DateTime.Now,SoThang = 3});
        }
        [HttpGet]
        public IActionResult _ReportData(VayVonQuaHanSearchVM search)
        {
            return ExecuteSearch(() => {
                var data = _context.HoiVienVayVons.Where(it=>it.TraXong != true).Include(it => it.CanBo).AsQueryable();
                var model = data.Where(it=>it.NgayTraNoCuoiCung.AddMonths(search.SoThang) < search.Ngay).Select(it => new VayVonDetailVM
                {
                    IDVayVon = it.IDVayVon,
                    MaHV = it.CanBo.MaCanBo,
                    TenHV = it.CanBo.HoVaTen,
                    SoTienVay = it.SoTienVay,
                    ThoiHangChoVay = it.ThoiHangChoVay,
                    LaiSuatVay = it.LaiSuatVay,
                    TuNgay = it.TuNgay,
                    DenNgay = it.DenNgay,
                    NgayTraNoCuoiCung = it.NgayTraNoCuoiCung,
                    NoiDungVay = it.NoiDungVay,
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Report
        #region Helper
        private void CheckError(VayVonMTVM obj) {
            if (obj.NhanSu.IdCanbo == null)
            {
                ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
            }
            if (obj.DenNgay < obj.TuNgay)
            {
                ModelState.AddModelError("DenNgay", "Từ ngày đến ngày không hợp lệ");
            }
            if (obj.ThoiHangChoVay < 0)
            {
                ModelState.AddModelError("ThoiHangChoVay", "Số tháng cho vay không hợp lệ");
            }
        }
        #endregion Helper
    }
}
