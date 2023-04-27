
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Portal.Constant;
using Portal.DataAccess;
using Portal.DataAccess.Repository;
using Portal.Extensions;
using Portal.Models;
using Portal.Models.Entitys;
using Portal.Resources;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Reflection.Metadata;

namespace Portal.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class CanBoController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public CanBoController(AppDbContext context, IWebHostEnvironment hostEnvironment) : base(context) {
            _hostEnvironment = hostEnvironment;
        }
        #region Index
        [PortalAuthorization]
        public IActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        public IActionResult _Search(CanBoSearchVM search) {
            return ExecuteSearch(() => {
                var model = _context.CanBos.AsQueryable();
                if (!String.IsNullOrEmpty(search.MaCanBo))
                {
                    model = model.Where(it => it.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen))
                {
                    model = model.Where(it => it.HoVaTen.Contains(search.HoVaTen));
                }
                if (!String.IsNullOrEmpty(search.MaTinhTrang))
                {
                    model = model.Where(it => it.MaTinhTrang == search.MaTinhTrang);
                }
                if (!String.IsNullOrEmpty(search.MaPhanHe))
                {
                    model = model.Where(it => it.MaPhanHe == search.MaPhanHe);
                }
                if (search.IdCoSo != null)
                {
                    model = model.Where(it => it.IdCoSo == search.IdCoSo);
                }
                if (search.IdDepartment != null)
                {
                    model = model.Where(it => it.IdDepartment == search.IdDepartment);
                }
                if (search.MaChucVu != null)
                {
                    model = model.Where(it => it.MaChucVu == search.MaChucVu);
                }
                if (search.Actived != null)
                {
                    model = model.Where(it => it.Actived == search.Actived);
                }
                var data = model.Include(it => it.TinhTrang)
                    .Include(it => it.Department)
                    .Include(it => it.BacLuong)
                    .Include(it => it.ChucVu)
                    .Include(it => it.PhanHe)
                    .Include(it=>it.CoSo).Select(it => new CanBoDetailVM
                    {
                        MaCanBo = it.MaCanBo,
                        HoVaTen = it.HoVaTen,
                        TenTinhTrang = it.TinhTrang.TenTinhTrang,
                        TenPhanHe = it.PhanHe.TenPhanHe,
                        TenCoSo = it.CoSo.TenCoSo,
                        TenDonVi = it.Department.Name,
                        TenChucVu = it.ChucVu.TenChucVu,
                        TenBacLuong = it.BacLuong.TenBacLuong,
                        TenNgachLuong = it.MaNgachLuong!,
                        HeSo = it.HeSoLuong,
                        IdCanbo = it.IDCanBo,
                    }).ToList();
                return PartialView(data);
            });
        }
        #endregion Index
        #region Create
        [PortalAuthorization]
        [HttpGet]
        public IActionResult Create() {
            CreateViewBag();
            CanBoVMMT obj = new CanBoVMMT();
            obj.HinhAnh = @"\images\logo.png";
            return View(obj);
        }
        [PortalAuthorization]
        [HttpPost]
        public JsonResult Create(CanBoVMMT insert, IFormFile? avtFileInbox)
        {
            var checkExistMaCB = _context.CanBos.Where(it => it.MaCanBo == insert.MaCanBo).ToList();
            if (checkExistMaCB.Count > 0)
            {
                ModelState.AddModelError("MaCanBo", "Mã cán bộ tồn tại không thể thêm");
            }
            // Check trình độ học vấn sau đại học
            if (insert.MaTrinhDoHocVan == "SĐH") 
            {
                if (String.IsNullOrEmpty(insert.MaHocHam) || String.IsNullOrWhiteSpace(insert.MaHocHam))
                {
                    ModelState.AddModelError("MaHocHam", "Học hàm chưa được chọn");
                }
            }
            if (insert.MaPhanHe != "03")
            {
                if (String.IsNullOrEmpty(insert.MaNgachLuong) || String.IsNullOrWhiteSpace(insert.MaNgachLuong)) {
                    ModelState.AddModelError("MaNgachLuong", "Ngạch lương chưa được chọn");
                }
                if (insert.MaBacLuong == null)
                {
                    ModelState.AddModelError("MaBacLuong", "Bậc lương chưa được chọn");
                }
                if (insert.NgayNangBacLuong == null)
                {
                    ModelState.AddModelError("NgayNangBacLuong", "Ngày nâng bậc chưa chọn");
                }
            }
            if (insert.MaPhanHe == "03")
            {
                if (insert.LuongKhoan <= 0)
                {
                    ModelState.AddModelError("LuongKhoan", "Chưa nhập số tiền lương khoán");
                }
                if (insert.KhoanTuNgay  == null)
                {
                    ModelState.AddModelError("KhoanTuNgay", "Chưa nhập khoán từ ngày");
                }
                if (insert.KhoanDenNgay == null)
                {
                    ModelState.AddModelError("KhoanDenNgay", "Chưa nhập khoán đến ngày");
                }
            }
            return ExecuteContainer(() => {
                var add = insert.AddCanBo();
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                if (avtFileInbox != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = add.MaCanBo;
                    var uploads = Path.Combine(wwwRootPath, @"images\canbo");

                    bool folderExists = System.IO.Directory.Exists(uploads);
                    if (!folderExists)
                        System.IO.Directory.CreateDirectory(uploads);

                    var extension = Path.GetExtension(avtFileInbox.FileName);
                    //if (obj.Product.ImageUrl != null)
                    //{
                    //    var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                    //    if (System.IO.File.Exists(oldImagePath))
                    //    {
                    //        System.IO.File.Delete(oldImagePath);
                    //    }
                    //}
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        avtFileInbox.CopyTo(fileStream);
                    }
                    add.HinhAnh = @"\images\canbo\" + fileName + extension;
                }
                _context.Attach(add).State = EntityState.Modified;
                _context.CanBos.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.CanBo.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [PortalAuthorization]
        public IActionResult Edit(Guid id) {
            var item = _context.CanBos.SingleOrDefault(it => it.IDCanBo == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            CanBoVMMT edit = CanBoVMMT.EditCanBo(item);
            CreateViewBag(edit.MaTinhTrang,edit.IdCoSo,edit.IdDepartment,edit.MaChucVu,edit.MaNgachLuong,edit.MaBacLuong,edit.MaTrinhDoHocVan,
                edit.MaTrinhDoChinhTri,edit.MaTrinhDoNgoaiNgu,edit.MaTrinhDoTinHoc,edit.MaDanToc,edit.MaTonGiao,edit.MaHocHam,edit.MaHeDaoTao,edit.MaPhanHe);
            return View(edit);
        }
        [HttpPost]
        [PortalAuthorization]
        public JsonResult Edit(CanBoVMMT obj, IFormFile? avtFileInbox) {

            if (obj.MaTrinhDoHocVan == "SĐH")
            {
                if (String.IsNullOrEmpty(obj.MaHocHam) || String.IsNullOrWhiteSpace(obj.MaHocHam))
                {
                    ModelState.AddModelError("MaHocHam", "Học hàm chưa được chọn");
                }
            }
            if (obj.MaPhanHe != "03")
            {
                if (String.IsNullOrEmpty(obj.MaNgachLuong) || String.IsNullOrWhiteSpace(obj.MaNgachLuong))
                {
                    ModelState.AddModelError("MaNgachLuong", "Ngạch lương chưa được chọn");
                }
                if (obj.MaBacLuong == null)
                {
                    ModelState.AddModelError("MaBacLuong", "Bậc lương chưa được chọn");
                }
                if (obj.NgayNangBacLuong == null)
                {
                    ModelState.AddModelError("NgayNangBacLuong", "Ngày nâng bậc chưa chọn");
                }
            }
            if (obj.MaPhanHe == "03")
            {
                if (obj.LuongKhoan <= 0)
                {
                    ModelState.AddModelError("LuongKhoan", "Chưa nhập số tiền lương khoán");
                }
                if (obj.KhoanTuNgay == null)
                {
                    ModelState.AddModelError("KhoanTuNgay", "Chưa nhập khoán từ ngày");
                }
                if (obj.KhoanDenNgay == null)
                {
                    ModelState.AddModelError("KhoanDenNgay", "Chưa nhập khoán đến ngày");
                }
            }
            return ExecuteContainer(() => {
                var edit = _context.CanBos.SingleOrDefault(it => it.IDCanBo == obj.IDCanBo);
                if (edit == null) {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.CanBo.ToLower())
                    });
                }
                edit = obj.EditUpdate(edit);
                edit.LastModifiedTime = DateTime.Now;
                edit.LastModifiedAccountId = AccountId();
                if (avtFileInbox != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = edit.MaCanBo;
                    var uploads = Path.Combine(wwwRootPath, @"images\canbo");

                    bool folderExists = System.IO.Directory.Exists(uploads);
                    if (!folderExists)
                        System.IO.Directory.CreateDirectory(uploads);

                    var extension = Path.GetExtension(avtFileInbox.FileName);
                    if (obj.HinhAnh != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, edit.HinhAnh!.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        avtFileInbox.CopyTo(fileStream);
                    }
                    edit.HinhAnh = @"\images\canbo\" + fileName + extension;
                }
                HistoryModelRepository history = new HistoryModelRepository(_context);
                history.SaveUpdateHistory(edit.IDCanBo.ToString(), AccountId()!.Value, edit);
                _context.Entry(edit).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.CanBo.ToLower())
                });
            });
        }
        #endregion Edit
        #region View
        [PortalAuthorization]
        public IActionResult View(Guid id) {
            return Content("Chức năng đang phát triển");
        }
        public IActionResult Print(Guid id)
        {
            return Content("Chức năng đang phát triển");
        }
        #endregion View
        #region Delete
        [HttpDelete]
        public JsonResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.CanBos.FirstOrDefault(p => p.IDCanBo == id);


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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.CanBo.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.CanBo.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper
        public JsonResult LoadBacLuong(string maNgachLuong) {
            var data = _context.BacLuongs.Where(it => it.Actived == true && it.MaNgachLuong == maNgachLuong).OrderBy(p => p.OrderIndex).Select(it => new { MaBacLuong = it.MaBacLuong, TenBacLuong = it.TenBacLuong +" " + it.HeSo.ToString() }).ToList();
            return Json(data);
        }
        public JsonResult GetHoSoLuong(Guid id)
        {
            var data = _context.BacLuongs.SingleOrDefault(it => it.Actived == true && it.MaBacLuong == id);
            return Json(data);
        }
        private void CreateViewBag(String? maTinhTrang = null, Guid? IdCoSo = null, Guid? IdDepartment = null,
            Guid? maChucVu = null, String? maNgachLuong = null, Guid? maBacLuong = null,
            String? maTrinhDoHocVan = null, String? maTrinhDoChinhTri = null,
            Guid? maTrinhDoNgoaiNgu = null, String? maTrinhDoTinHoc = null,
            String? maDanToc = null, String? maTonGiao = null, String? maHocHam = null, String? maHeDaoTao = null, String? maPhanhe = null)
        {
            var MenuList = _context.TinhTrangs.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { MaTinhTrang = it.MaTinhTrang, TenTinhTrang = it.TenTinhTrang }).ToList();
            ViewBag.MaTinhTrang = new SelectList(MenuList, "MaTinhTrang", "TenTinhTrang", maTinhTrang);

            var MenuListCoSo = _context.CoSos.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { IdCoSo = it.IdCoSo, TenCoSo = it.TenCoSo }).ToList();
            ViewBag.IdCoSo = new SelectList(MenuListCoSo, "IdCoSo", "TenCoSo", IdCoSo);

            var DonVi = _context.Departments.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { IdDepartment = it.Id, Name = it.Name }).ToList();
            ViewBag.IdDepartment = new SelectList(DonVi, "IdDepartment", "Name", IdDepartment);

            var chucVu = _context.ChucVus.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { MaChucVu = it.MaChucVu, TenChucVu = it.TenChucVu }).ToList();
            ViewBag.MaChucVu = new SelectList(chucVu, "MaChucVu", "TenChucVu", maChucVu);

            var ngachLuong = _context.NgachLuongs.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { MaNgachLuong = it.MaNgachLuong, TenNgachLuong = it.TenNgachLuong }).ToList();
            ViewBag.MaNgachLuong = new SelectList(ngachLuong, "MaNgachLuong", "TenNgachLuong", maNgachLuong);

            var bacLuong = _context.BacLuongs.Where(it => it.Actived == true && (it.MaNgachLuong == maNgachLuong || maNgachLuong == null)).OrderBy(p => p.OrderIndex).Select(it => new { MaBacLuong = it.MaBacLuong, TenBacLuong = it.TenBacLuong + " " + it.HeSo.ToString() }).ToList();
            ViewBag.MaBacLuong = new SelectList(bacLuong, "MaBacLuong", "TenBacLuong", maBacLuong);

            var trinhDoHocVan = _context.TrinhDoHocVans.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTrinhDoHocVan = it.MaTrinhDoHocVan, TenTrinhDoHocVan = it.TenTrinhDoHocVan }).ToList();
            ViewBag.MaTrinhDoHocVan = new SelectList(trinhDoHocVan, "MaTrinhDoHocVan", "TenTrinhDoHocVan", maTrinhDoHocVan);

            var trinhDoChinhTri = _context.TrinhDoChinhTris.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTrinhDoChinhTri = it.MaTrinhDoChinhTri, TenTrinhDoChinhTri = it.TenTrinhDoChinhTri }).ToList();
            ViewBag.MaTrinhDoChinhTri = new SelectList(trinhDoChinhTri, "MaTrinhDoChinhTri", "TenTrinhDoChinhTri", maTrinhDoChinhTri);

            var trinhDoNgoaiNgu = _context.TrinhDoNgoaiNgus.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTrinhDoNgoaiNgu = it.MaTrinhDoNgoaiNgu, TenTrinhDoNgoaiNgu = it.TenTrinhDoNgoaiNgu }).ToList();
            ViewBag.MaTrinhDoNgoaiNgu = new SelectList(trinhDoNgoaiNgu, "MaTrinhDoNgoaiNgu", "TenTrinhDoNgoaiNgu", maTrinhDoNgoaiNgu);

            var trinhDoTinHoc = _context.TrinhDoTinHocs.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTrinhDoTinHoc = it.MaTrinhDoTinHoc, TenTrinhDoTinHoc = it.TenTrinhDoTinHoc }).ToList();
            ViewBag.MaTrinhDoTinHoc = new SelectList(trinhDoTinHoc, "MaTrinhDoTinHoc", "TenTrinhDoTinHoc", maTrinhDoTinHoc);

            var danToc = _context.DanTocs.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaDanToc = it.MaDanToc, TenDanToc = it.TenDanToc }).ToList();
            ViewBag.MaDanToc = new SelectList(danToc, "MaDanToc", "TenDanToc", maDanToc);

            var tonGiao = _context.TonGiaos.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTonGiao = it.MaTonGiao, TenTonGiao = it.TenTonGiao }).ToList();
            ViewBag.MaTonGiao = new SelectList(tonGiao, "MaTonGiao", "TenTonGiao", maTonGiao);

            var hocHam = _context.HocHams.Select(it => new { MaHocHam = it.MaHocHam, TenHocHam = it.TenHocHam }).ToList();
            ViewBag.MaHocHam = new SelectList(hocHam, "MaHocHam", "TenHocHam", maHocHam);

            var heDaoTao = _context.HeDaoTaos.Select(it => new { MaHeDaoTao = it.MaHeDaoTao, TenHeDaoTao = it.TenHeDaoTao }).ToList();
            ViewBag.MaHeDaoTao = new SelectList(heDaoTao, "MaHeDaoTao", "TenHeDaoTao", maHeDaoTao);

            var phanHe = _context.PhanHes.Select(it => new { MaPhanHe = it.MaPhanHe, TenPhanHe = it.TenPhanHe }).ToList();
            ViewBag.MaPhanHe = new SelectList(phanHe, "MaPhanHe", "TenPhanHe", maPhanhe);
        }
        #endregion Helper
    }

}
