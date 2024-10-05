using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.ViewModels.Masterdata;
using HoiNongDan.Resources;
using HoiNongDan.Web.Areas.NhanSu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class PhatTrienDangController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private string _tenThuMuc = "PhatTrienDang";
        private string _loaiFileDinhKem = "85";
        public PhatTrienDangController(AppDbContext context, IWebHostEnvironment hostEnvironment) : base(context)
        {
            _hostEnvironment = hostEnvironment;
        }
        #region Index
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBagSearch();
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(PhatTrienDangSearchVM sr) {
            return ExecuteSearch(() => {

                //var model = (from hvptd in _context.PhatTrienDang_HoiViens
                //             join ptd in _context.PhatTrienDangs on hvptd.IDPhatTrienDang equals ptd.ID
                //             join hv in _context.CanBos on hvptd.IDHoiVien equals hv.IDCanBo
                //             join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                //             where hv.IsHoiVien == true
                //             && pv.AccountId == AccountId()
                //             select ptd).Include(it => it.PhatTrienDang_HoiViens).ThenInclude(it => it.CanBo).AsQueryable();
                var model = _context.PhatTrienDang_HoiViens
                    .Include(it => it.PhatTrienDang).ThenInclude(it => it.DiaBanHoatDong).ThenInclude(it=>it.QuanHuyen)

                    .Include(it => it.CanBo).AsQueryable();
                if (sr.MaDiaBanHoiVien != null)
                {
                    model = model.Where(it => it.PhatTrienDang.MaDiaBanHoiND == sr.MaDiaBanHoiVien);
                }
                if (!String.IsNullOrWhiteSpace(sr.MaQuanHuyen))
                {
                    model = model.Where(it => it.PhatTrienDang.DiaBanHoatDong.MaQuanHuyen == sr.MaQuanHuyen);
                }
                if (sr.Nam != null)
                {
                    model = model.Where(it => it.PhatTrienDang.Nam == sr.Nam);
                }
                if (!String.IsNullOrWhiteSpace(sr.TenVietTat)) {
                    model = model.Where(it => it.PhatTrienDang.TenVietTat.Contains(sr.TenVietTat));
                }
                if (sr.Actived != null)
                {
                    model = model.Where(it => it.PhatTrienDang.Actived == sr.Actived);
                }
                var data = model.Select(it => new PhatTrienDangDetailVM
                {
                    ID = it.PhatTrienDang.ID,
                    TenVietTat = it.PhatTrienDang.TenVietTat,
                    DiaBanHoiND = it.PhatTrienDang.DiaBanHoatDong.TenDiaBanHoatDong,
                    Nam = it.PhatTrienDang.Nam,
                    MaHV = it.CanBo.MaCanBo,
                    TenHV = it.CanBo.HoVaTen,
                    QuanHuyen = it.PhatTrienDang.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                    IDHoiVien = it.IDHoiVien,
                    SoLuong = it.PhatTrienDang.SoLuong,
                    GhiChu = it.PhatTrienDang.GhiChu,
                    NoiDung = it.PhatTrienDang.NoiDung
                }).Distinct().ToList();
                return PartialView(data);
            });
        }
        #endregion Index
        #region Create
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Create()
        {
            PhatTrienDangVM model = new PhatTrienDangVM();
            CreateViewBag();
            return View(model);
        }
        public IActionResult Create(PhatTrienDangVM obj, IFormFile?[] fileDinhKems) {
            CheckError(obj);
            return ExecuteContainer(() => {
                PhatTrienDang add = new PhatTrienDang();
                add.Actived = true;
                add.MaDiaBanHoiND = obj.MaDiaBanHoiVien;
                add.Nam = obj.Nam;
                add.TenVietTat = obj.TenVietTat;
                add.SoLuong = obj.SoLuong;
                add.NoiDung = obj.NoiDung;
                add.GhiChu = obj.GhiChu;
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.ID = Guid.NewGuid();
                List<PhatTrienDang_HoiVien> listHoiVien = new List<PhatTrienDang_HoiVien>();
                foreach (var item in obj.HoiViens.Where(it=>it.Chon==true))
                {
                    listHoiVien.Add(new PhatTrienDang_HoiVien { 
                        IDHoiVien = item.IdCanbo!.Value,
                        IDPhatTrienDang = add.ID
                    });
                }
                if (fileDinhKems.Count() > 0)
                {
                    var dataFile = AdFiles(fileDinhKems, add.ID);
                    _context.FileDinhKems.AddRange(dataFile);
                }
                listHoiVien = listHoiVien.DistinctBy(c => new { c.IDHoiVien, c.IDPhatTrienDang }).ToList();
                _context.PhatTrienDangs.Add(add);
                _context.PhatTrienDang_HoiViens.AddRange(listHoiVien);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.PhatTrienDang.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit 
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
            var item = _context.PhatTrienDangs.Include(it => it.PhatTrienDang_HoiViens).ThenInclude(it => it.CanBo).ThenInclude(it=>it.DiaBanHoatDong).Include(it => it.DiaBanHoatDong).SingleOrDefault(it => it.ID == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            PhatTrienDangVM obj = new PhatTrienDangVM();
            obj.ID = id;
            obj.TenVietTat = item.TenVietTat;
            obj.SoLuong = item.SoLuong;
            obj.Actived = item.Actived;
            obj.NoiDung = item.NoiDung;
            obj.Nam = item.Nam;
            obj.GhiChu = item.GhiChu;
            obj.MaDiaBanHoiVien = item.MaDiaBanHoiND;
            
            obj.FileDinhKems = _context.FileDinhKems.Where(it => it.Id == obj.ID).ToList();
            foreach (var item1 in item.PhatTrienDang_HoiViens)
            {
                obj.HoiViens.Add(new HoiVienInfo { 
                    IdCanbo = item1.CanBo.IDCanBo,
                    HoKhauThuongTru = item1.CanBo.HoKhauThuongTru,
                    SoCCCD = item1.CanBo.SoCCCD,
                    NgaySinh = item1.CanBo.NgaySinh,
                    DiaBan = item1.CanBo.DiaBanHoatDong!.TenDiaBanHoatDong,
                    MaCanBo = item1.CanBo.MaCanBo,
                    HoVaTen = item1.CanBo.HoVaTen,
                    Chon = true,
                });
            }
            CreateViewBag(obj.MaDiaBanHoiVien);
            return View(obj);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PhatTrienDangVM obj, IFormFile?[] fileDinhKems) 
        { 
            CheckError(obj);
            return ExecuteContainer(() => {
                var edit = _context.PhatTrienDangs.Include(it => it.PhatTrienDang_HoiViens).ThenInclude(it => it.CanBo).ThenInclude(it => it.DiaBanHoatDong).Include(it => it.DiaBanHoatDong).SingleOrDefault(it => it.ID == obj.ID);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = "Không tìm thấy thông tin đang sửa"
                    });
                }
                edit.TenVietTat = obj.TenVietTat;
                edit.Nam = obj.Nam;
                edit.NoiDung = obj.NoiDung;
                edit.GhiChu = obj.GhiChu;
                edit.Actived = obj.Actived;
                edit.SoLuong = obj.SoLuong;
                edit.LastModifiedAccountId = AccountId();
                edit.LastModifiedTime = DateTime.Now;
                List<PhatTrienDang_HoiVien> listHoiVien = new List<PhatTrienDang_HoiVien>();

                _context.PhatTrienDang_HoiViens.RemoveRange(edit.PhatTrienDang_HoiViens);
                foreach (var item in obj.HoiViens.Where(it => it.Chon == true))
                {
                    listHoiVien.Add(new PhatTrienDang_HoiVien
                    {
                        IDHoiVien = item.IdCanbo!.Value,
                        IDPhatTrienDang = edit.ID
                    });
                }
                listHoiVien = listHoiVien.DistinctBy(c => new { c.IDHoiVien, c.IDPhatTrienDang }).ToList();
                if (fileDinhKems.Count() > 0)
                {
                    var dataFile = AdFiles(fileDinhKems, edit.ID);
                    _context.FileDinhKems.AddRange(dataFile);
                }
                _context.PhatTrienDang_HoiViens.AddRange(listHoiVien);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.PhatTrienDang.ToLower())
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
                var page = _context.PhatTrienDangs.FirstOrDefault(p => p.ID == id);
                if (page != null)
                {

                    _context.Entry(page).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.PhatTrienDang.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.PhatTrienDang.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        [NonAction]
        private void CreateViewBag(Guid? MaDiaBanHoiND = null) {

            FnViewBag fnViewBag = new FnViewBag(_context);
              ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

        }
        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaHinhThucKhenThuong = fnViewBag.HinhThucKhenThuong();

            //  var MenuList = _context.DanhHieuKhenThuongs.Where(it => it.IsHoiVien == true).Select(it => new { MaDanhHieuKhenThuong = it.MaDanhHieuKhenThuong, TenDanhHieuKhenThuong = it.TenDanhHieuKhenThuong }).ToList();
            ViewBag.MaDanhHieuKhenThuong = fnViewBag.DanhHieuKhenThuong(); ;

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }
        private void CheckError(PhatTrienDangVM obj) {
            if (obj.HoiViens == null || obj.HoiViens.Count == 0  || obj.HoiViens.Where(it=>it.Chon==true).Count()==0) {
                ModelState.AddModelError("HoiVien", "Chưa chọn thông tin hội viên");
            }
        }
        [NonAction]
        private List<FileDinhKemModel> AdFiles(IFormFile?[] filesDinhKem, Guid id)
        {
            List<FileDinhKemModel> addFiles = new List<FileDinhKemModel>();
            foreach (var file in filesDinhKem)
            {
                FileDinhKemModel addFile = new FileDinhKemModel();
                addFile.Id = id;
                addFile.Key = Guid.NewGuid();
                addFile.IDLoaiDinhKem = _loaiFileDinhKem;
                FunctionFile.CopyFile(_hostEnvironment, file!, addFile, TenThuMuc: _tenThuMuc + @"\" + id.ToString());
                if (!String.IsNullOrEmpty(addFile.Error) && !String.IsNullOrWhiteSpace(addFile.Error))
                {
                    ModelState.AddModelError("fileInbox", addFile.Error);
                    break;
                }
                addFiles.Add(addFile);
            }
            return addFiles;
        }
        #endregion Helper
    }
}
