using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.ViewModels.Masterdata;
using HoiNongDan.Resources;
using HoiNongDan.Web.Areas.NhanSu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using System.Data.Entity;


namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    public class LopHocController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private string _tenThuMuc = "LopHoc";
        private string _loaiFileDinhKem = "83";
        public LopHocController (AppDbContext context, IWebHostEnvironment hostEnvironment) : base(context) {
            _hostEnvironment = hostEnvironment;
        }
        #region Index
        public IActionResult Index()
        {
         
            CreateViewBag();
            return View();
        }

        public IActionResult _Search(String? Ten, Guid? MaHinhThucHoTro,DateTime? TuNgay, DateTime? DenNgay, bool? Actived) {
            return ExecuteSearch(() =>
            {
                var data = _context.LopHocs.Include(it=>it.HinhThucHoTro).AsQueryable();
                if (!String.IsNullOrWhiteSpace(Ten))
                {
                    data = data.Where(it => it.TenLopHoc.Contains(Ten));
                }
                if(MaHinhThucHoTro != null)
                {
                    data = data.Where(it => it.MaHinhThucHoTro == MaHinhThucHoTro);
                }

                if (TuNgay != null)
                {
                    data = data.Where(it => it.TuNgay >= TuNgay);
                }
                if (DenNgay != null)
                {
                    data = data.Where(it => it.TuNgay <= DenNgay);
                }
                if (Actived != null) {
                    data = data.Where(it => it.Actived == Actived);
                }
                var model = data.Select(it => new LopHocDetailVM {
                    IDLopHoc = it.IDLopHoc,
                    TenLopHoc = it.TenLopHoc,
                    TuNgay = it.TuNgay,
                    DenNgay = it.DenNgay,
                    TenHinhThucHoTro = it.HinhThucHoTro.TenHinhThuc,
                    SoLuong = it.HoiVienHoTros.Where(p=>p.IDLopHoc == it.IDLopHoc).Count(),
                    Description = it.Description,
                    Actived = it.Actived,
                    OrderIndex = it.OrderIndex
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Create 
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create() {
            LopHocVM model = new LopHocVM();
            CreateViewBag();
            return View(model);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LopHocVM obj, IFormFile?[] fileDinhKems)
        {
            return ExecuteContainer(() => {
                LopHoc add = new LopHoc {
                    IDLopHoc = Guid.NewGuid(),
                    TenLopHoc = obj.TenLopHoc,
                    MaHinhThucHoTro = obj.MaHinhThucHoTro,
                    Description = obj.Description,
                    Actived = true,
                    CreatedAccountId = AccountId(),
                    TuNgay = obj.TuNgay,
                    DenNgay = obj.DenNgay,
                    OrderIndex = obj.OrderIndex,
                    CreatedTime = DateTime.Now,
                };
                if (fileDinhKems.Count() > 0)
                {
                    var dataFile = AdFiles(fileDinhKems, add.IDLopHoc);
                    _context.FileDinhKems.AddRange(dataFile);
                }
                _context.LopHocs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.LopHoc.ToLower())
                });
            });
        }
        #endregion Create

        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
            var obj = _context.LopHocs.SingleOrDefault(it => it.IDLopHoc == id);
            if (obj == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            var edit = new LopHocVM();
            edit.IDLopHoc = obj.IDLopHoc;
            edit.TenLopHoc = obj.TenLopHoc;
            edit.TuNgay = obj.TuNgay;
            edit.DenNgay = obj.DenNgay;
            edit.Description = obj.Description;
            edit.OrderIndex = obj.OrderIndex;
            edit.Actived = obj.Actived;
            edit.MaHinhThucHoTro = obj.MaHinhThucHoTro;
            CreateViewBag(obj.MaHinhThucHoTro);
            edit.FileDinhKems = _context.FileDinhKems.Where(it => it.Id == obj.IDLopHoc).ToList();
            return View(edit);
        }

        public IActionResult Edit(LopHocVM obj, IFormFile?[] fileDinhKems) {
            return ExecuteContainer(() => {
                var edit = _context.LopHocs.SingleOrDefault(it => it.IDLopHoc == obj.IDLopHoc);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = "Không tìm thấy thông tin đang sửa"
                    });
                }
                edit.TenLopHoc = obj.TenLopHoc;
                edit.TuNgay = obj.TuNgay;
                edit.DenNgay = obj.DenNgay;
                edit.Description = obj.Description;
                edit.Actived = obj.Actived;
                edit.MaHinhThucHoTro = obj.MaHinhThucHoTro;
                edit.OrderIndex = obj.OrderIndex;

                if (fileDinhKems.Count() > 0)
                {
                    var dataFile = AdFiles(fileDinhKems, edit.IDLopHoc);
                    _context.FileDinhKems.AddRange(dataFile);
                }
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.LopHoc.ToLower())
                });
            });
        }
        #endregion Edit

        #region Delete 
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id) {
            return ExecuteDelete(() =>
            {
                var del = _context.LopHocs.SingleOrDefault(it => it.IDLopHoc == id);
                if (del != null)
                {
                    var checkFile = _context.FileDinhKems.SingleOrDefault(it => it.Id == del.IDLopHoc);
                    if (checkFile != null)
                    {

                        if (FunctionFile.Delete(_hostEnvironment, checkFile.Url))
                        {
                            _context.Remove(checkFile);
                        }
                        FunctionFile.DeleteFolder(_hostEnvironment, url: _tenThuMuc + @"\" + del.IDLopHoc.ToString());
                    }

                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.LopHoc.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.LopHoc.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        [NonAction]
        private void CreateViewBag(Guid? MaHinhThucHoTro = null) {
            var data = _context.HinhThucHoTros.Select(it => new { it.MaHinhThucHoTro, it.TenHinhThuc });
            ViewBag.MaHinhThucHoTro = new SelectList(data, "MaHinhThucHoTro", "TenHinhThuc", MaHinhThucHoTro);
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
