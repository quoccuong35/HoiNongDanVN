﻿
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Hosting;
using NuGet.Packaging.Signing;
using Portal.Constant;
using Portal.DataAccess;
using Portal.DataAccess.Repository;
using Portal.Extensions;
using Portal.Models;
using Portal.Models.Entitys;
using Portal.Models.Entitys.MasterData;
using Portal.Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Mapping;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text;
using System.Transactions;

namespace Portal.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class CanBoController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private string [] DateFomat;
        const string controllerCode = ConstExcelController.CanBo;
        const int startIndex = 6;
        public CanBoController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context) {
            _hostEnvironment = hostEnvironment;
            DateFomat = config.GetSection("SiteSettings:DateFormat").Value.ToString().Split(',');
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
                        IDCanBo = it.IDCanBo,
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
            obj.HinhAnh = @"\images\login.png";
            return View(obj);
        }
        [PortalAuthorization]
        [HttpPost]
        public JsonResult Create(CanBoVMMT insert, IFormFile? avtFileInbox)
        {
            CheckError(insert);
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

            CheckError(obj);
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
            ProfileCanBo profileCanBo = new ProfileCanBo();
            var canBo = _context.CanBos.Where(it=>it.IDCanBo==id).Include(it => it.TinhTrang)
                    .Include(it => it.Department)
                    .Include(it => it.BacLuong)
                    .Include(it => it.ChucVu)
                    .Include(it => it.PhanHe)
                    .Include(it => it.CoSo).Select(it => new CanBoDetailVM
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
                        IDCanBo = it.IDCanBo,
                        SoDienThoai = it.SoDienThoai,
                        Email = it.Email,
                        PhuCapChucVu = it.PhuCapChucVu,
                        PhuCapKiemNhiem = it.PhuCapKiemNhiem,
                        PhuCapVuotKhung = it.PhuCapVuotKhung,
                        PhuCapKhuVuc = it.PhuCapKhuVuc,
                        NgayVaoBienChe = it.NgayVaoBienChe,
                        NgayNangBacLuong = it.NgayNangBacLuong,
                    }).First();
          
            var qhGiaDinh = _context.QuanHeGiaDinhs.Where(it => it.IDCanBo == id).Include(it => it.LoaiQuanhe)
                            .Select(it => new QHGiaDinhDetail {
                                HoTen = it.HoTen,
                                NgaySinh = it.NgaySinh,
                                NgheNghiep = it.NgheNghiep,
                                NoiLamVien = it.NoiLamVien,
                                DiaChi = it.DiaChi,
                                GhiChu = it.GhiChu,
                                TenLoaiQuanHe = it.LoaiQuanhe.TenLoaiQuanHeGiaDinh
                            }).ToList();
            var daoTao = _context.QuaTrinhDaoTaos.Where(it=>it.IDCanBo == id).Select(it => new DaoTaoDetailVM
            {
                IDQuaTrinhDaoTao = it.IDQuaTrinhDaoTao,
                TenChuyenNganh = it.ChuyenNganh.TenChuyenNganh,
                TenHinhThucDaoTao = it.HinhThucDaoTao.TenHinhThucDaoTao,
                TenLoaiBangCap = it.LoaiBangCap.TenLoaiBangCap,
                CoSoDaoTao = it.CoSoDaoTao,
                NgayTotNghiep = it.NgayTotNghiep,
                QuocGia = it.QuocGia,
                GhiChu = it.GhiChu,
                LuanAnTN = it.LuanAnTN,
                FileDinhKem = it.FileDinhKem

            }).ToList();
            var boiDuong = _context.QuaTrinhBoiDuongs.Where(it=>it.IDCanBo == id).Select(it => new BoiDuongDetai
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
            }).ToList();
            var boNhiem = _context.QuaTrinhBoNhiems.Where(it=>it.IDCanBo == id).Select(it => new BoNhiemDetailVM
            {
                IdQuaTrinhBoNhiem = it.IdQuaTrinhBoNhiem,
                MaCanBo = it.CanBo.MaCanBo,
                HoVaTen = it.CanBo.HoVaTen,
                SoQuyetDinh = it.SoQuyetDinh,
                NgayQuyetDinh = it.NgayQuyetDinh,
                NguoiKy = it.NguoiKy,
                HeSoChucVu = it.HeSoChucVu,
                GhiChu = it.GhiChu,
                TenChucVu = it.ChucVu.TenChucVu,
                TenDonVi = it.Department.Name,
                TenCoSo = it.CoSo.TenCoSo
            }).ToList();
            var khenThuong = _context.QuaTrinhKhenThuongs.Where(it=>it.IDCanBo==id).Select(it => new KhenThuongDetailVM
            {
                IDQuaTrinhKhenThuong = it.IDQuaTrinhKhenThuong,
                MaCanBo = it.CanBo.MaCanBo,
                HoVaTen = it.CanBo.HoVaTen,
                SoQuyetDinh = it.SoQuyetDinh,
                NgayQuyetDinh = it.NgayQuyetDinh,
                LyDo = it.LyDo,
                NguoiKy = it.NguoiKy,
                GhiChu = it.GhiChu,
                TenDanhHieuKhenThuong = it.DanhHieuKhenThuong.TenDanhHieuKhenThuong,
                TenHinhThucKhenThuong = it.HinhThucKhenThuong.TenHinhThucKhenThuong,
            }).ToList();
            var kyLuat = _context.QuaTrinhKyLuats.Where(it=>it.IDCanBo == id).Select(it => new KyLuatDetailVM
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
            profileCanBo.CanBo = canBo;
            profileCanBo.QHGiaDinh = qhGiaDinh;
            profileCanBo.DaoTao = daoTao;
            profileCanBo.BoiDuong = boiDuong;
            profileCanBo.BoNhiem = boNhiem;
            profileCanBo.KhenThuong = khenThuong;
            profileCanBo.KyLuat = kyLuat;
            return View(profileCanBo);
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
                    if (del.HinhAnh != null && del.HinhAnh != "")
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        var oldImagePath = Path.Combine(wwwRootPath, del.HinhAnh!.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
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
        #region Import Excel 
        public IActionResult Import() {
            DataSet ds = GetDataSetFromExcel();
            List<string> errorList = new List<string>();
            return ExcuteImportExcel(() => {
                if (ds.Tables.Count > 0) {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        foreach (DataTable dt in ds.Tables) {
                            string contCode = dt.Columns[0].ColumnName.ToString();
                            if (contCode == controllerCode)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    //string aa = dr.ItemArray[0].ToString();
                                    if (dt.Rows.IndexOf(dr) >= startIndex)
                                    {
                                        if (!string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                        {
                                            var data = CheckTemplate(dr.ItemArray);
                                            if (!string.IsNullOrEmpty(data.Error))
                                            {
                                                errorList.Add(data.Error);
                                            }
                                            else
                                            {
                                                // Tiến hành cập nhật
                                                string result = ExecuteImportExcelMenu(data);
                                                if (result != LanguageResource.ImportSuccess)
                                                {
                                                    errorList.Add(result);
                                                }
                                            }
                                        }
                                        else
                                            break;
                                        //Check correct template

                                    }
                                }
                            }
                            else
                            {
                                string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.CanBo);
                                errorList.Add(error);
                            }
                        }
                        if (errorList != null && errorList.Count > 0)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = false,
                                Data = String.Join("<br/>", errorList)
                            }); ;
                        }
                        ts.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = LanguageResource.ImportSuccess
                        });

                    }
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = false,
                        Data = LanguageResource.Validation_ImportExcelFile
                    });
                }
            });

        }
        protected DataSet GetDataSetFromExcel()
        {
            DataSet ds = new DataSet();
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    if (file != null && file.Length > 0)
                    {
                        //Check file is excel
                        //Notes: Châu bổ sung .xlsb
                        if (file.FileName.Contains("xls") || file.FileName.Contains("xlsx") || file.FileName.Contains("xlsb"))
                        {
                            string wwwRootPath = _hostEnvironment.WebRootPath;
                            var fileName = Path.GetFileName(file.FileName);
                            var mapPath = Path.Combine(wwwRootPath, @"upload\excel");
                            if (!Directory.Exists(mapPath))
                            {
                                Directory.CreateDirectory(mapPath);
                            }
                            var path = Path.Combine(mapPath, fileName);
                            using (var fileStream = new FileStream(Path.Combine(mapPath, fileName), FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }

                            using (ClassImportExcel excelHelper = new ClassImportExcel(path))
                            {
                                excelHelper.Hdr = "YES";
                                excelHelper.Imex = "1";
                                ds = excelHelper.ReadDataSet();
                            }
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion Import Excel
        #region Export Data
        public IActionResult ExportCreate() {
            List<CanBoExcelVM> data = new List<CanBoExcelVM>();
            return Export(data);
        }
        public IActionResult ExportEdit(CanBoSearchVM search)
        {
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

            var data = model.Include(it=>it.TinhTrang)
                .Include(it=>it.PhanHe)
                .Include(it=>it.CoSo)
                .Include(it=>it.Department)
                .Include(it=>it.ChucVu)
                .Include(it=>it.BacLuong)
                .Include(it=>it.TrinhDoHocVan)
                .Include(it => it.TrinhDoTinHoc)
                .Include(it => it.TrinhDoNgoaiNgu)
                .Include(it => it.TrinhDoChinhTri)
                .Include(it => it.DanToc)
                .Include(it => it.TonGiao)
                .Include(it=>it.HocHam)
                .Include(it=>it.HeDaoTao)
                .Select(item => new CanBoExcelVM {
                IDCanBo = item.IDCanBo,
                MaCanBo = item.MaCanBo,
                HoVaTen = item.HoVaTen,
                NgaySinh = item.NgaySinh,
                GioiTinh = item.GioiTinh == GioiTinh.Nam ?true:false,
                TenCoSo = item.CoSo.TenCoSo,
                TenDonVi = item.Department.Name,
                TenTinhTrang = item.TinhTrang.TenTinhTrang,
                TenPhanHe = item.PhanHe.TenPhanHe,
                TenChucVu = item.ChucVu.TenChucVu,
                SoCCCD = item.SoCCCD,
                NgayCapCCCD = item.NgayCapCCCD,
                SoDienThoai = item.SoDienThoai,
                Email = item.Email,
                TenNgachLuong = item.BacLuong.NgachLuong.TenNgachLuong,
                BacLuong = item.BacLuong.OrderIndex,
                HeSoLuong = item.HeSoLuong,
                NgayNangBacLuong = item.NgayNangBacLuong,
                PhuCapChucVu = item.PhuCapChucVu,
                PhuCapVuotKhung = item.PhuCapVuotKhung,
                PhuCapKiemNhiem = item.PhuCapKiemNhiem,
                PhuCapKhuVuc = item.PhuCapKhuVuc,
                LuongKhoan = item.LuongKhoan,
                KhoanTuNgay = item.KhoanTuNgay,
                KhoanDenNgay = item.KhoanDenNgay,
                SoBHXH = item.SoBHXH,
                SoBHYT = item.SoBHYT,
                MaSoThue = item.MaSoThue,
                NgayVaoBienChe = item.NgayVaoBienChe,
                NgayThamGiaCongTac = item.NgayThamGiaCongTac,
                TenHeDaoTao = item.HeDaoTao!.TenHeDaoTao,
                TenTrinhDoHocVan = item.TrinhDoHocVan.TenTrinhDoHocVan,
                ChuyenNganh = item.ChuyenNganh,
                TenTrinhDoTinHoc = item.TrinhDoTinHoc!.TenTrinhDoTinHoc,
                TenTrinhDoNgoaiNgu = item.TrinhDoNgoaiNgu!.TenTrinhDoNgoaiNgu,
                TenTrinhDoChinhTri = item.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                TenHocHam = item.HocHam!.TenHocHam,
                TenDanToc = item.DanToc.TenDanToc,
                TenTonGiao = item.TonGiao.TenTonGiao,
                NoiSinh = item.NoiSinh,
                ChoOHienNay = item.ChoOHienNay,
                NgayvaoDangDuBi = item.NgayvaoDangDuBi,
                NgayVaoDangChinhThuc = item.NgayVaoDangChinhThuc,
                GhiChu = item.GhiChu,
        }).ToList();
            return Export(data);
        }
        public FileContentResult Export(List<CanBoExcelVM> menu) {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "IDCanBo", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "MaCanBo", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinh", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GioiTinh", isBoolean = true,  isComment = true, strComment = "Nam để chữ X" });

            var coSo = _context.CoSos.ToList().Select(x => new DropdownModel { Id = x.IdCoSo, Name = x.TenCoSo }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenCoSo", isAllowedToEdit = true, isDropdownlist = true, DropdownData = coSo, TypeId = ConstExcelController.GuidId });

            var donVi = _context.Departments.ToList().Select(x => new DropdownModel { Id = x.Id, Name = x.Name }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenDonVi", isAllowedToEdit = true, isDropdownlist = true, DropdownData = donVi, TypeId = ConstExcelController.GuidId });


            var tinhTrang = _context.TinhTrangs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTinhTrang, Name = x.TenTinhTrang }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTinhTrang", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tinhTrang, TypeId = ConstExcelController.StringId });
            
            var phanHe = _context.PhanHes.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaPhanHe, Name = x.TenPhanHe }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenPhanHe", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = phanHe, TypeId = ConstExcelController.StringId });

          
            var chuVu = _context.ChucVus.ToList().Select(x => new DropdownModel { Id = x.MaChucVu, Name = x.TenChucVu }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenChucVu", isAllowedToEdit = true, isDropdownlist = true, DropdownData = chuVu, TypeId = ConstExcelController.GuidId });

            columns.Add(new ExcelTemplate() { ColumnName = "SoCCCD", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayCapCCCD", isAllowedToEdit = true, isDateTime = true });

            columns.Add(new ExcelTemplate() { ColumnName = "SoDienThoai", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Email", isAllowedToEdit = true, isText = true });

            var ngachLuong = _context.NgachLuongs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaNgachLuong, Name = x.TenNgachLuong }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenNgachLuong", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = ngachLuong, TypeId = ConstExcelController.StringId });



            columns.Add(new ExcelTemplate() { ColumnName = "BacLuong", isAllowedToEdit = true, isText = true });

            columns.Add(new ExcelTemplate() { ColumnName = "HeSoLuong", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayNangBacLuong", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "PhuCapChucVu", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "PhuCapVuotKhung", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "PhuCapKiemNhiem", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "PhuCapKhuVuc", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "LuongKhoan", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "KhoanTuNgay", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "KhoanDenNgay", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoBHXH", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoBHYT", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "MaSoThue", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoBienChe", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayThamGiaCongTac", isAllowedToEdit = true, isDateTime = true });
            var heDaoTao = _context.HeDaoTaos.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaHeDaoTao, Name = x.TenHeDaoTao }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenHeDaoTao", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = heDaoTao, TypeId = ConstExcelController.StringId });
            
            var hocVan = _context.TrinhDoHocVans.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoHocVan, Name = x.TenTrinhDoHocVan }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTrinhDoHocVan", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = hocVan, TypeId = ConstExcelController.StringId });

            columns.Add(new ExcelTemplate() { ColumnName = "ChuyenNganh", isAllowedToEdit = true, isText = true });

            var tinHoc = _context.TrinhDoTinHocs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoTinHoc, Name = x.TenTrinhDoTinHoc }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTrinhDoTinHoc", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tinHoc, TypeId = ConstExcelController.StringId });


            var ngoaiNgu = _context.TrinhDoNgoaiNgus.ToList().Select(x => new DropdownModel { Id = x.MaTrinhDoNgoaiNgu, Name = x.TenTrinhDoNgoaiNgu }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTrinhDoNgoaiNgu", isAllowedToEdit = true, isDropdownlist = true, DropdownData = ngoaiNgu, TypeId = ConstExcelController.GuidId });

            var chinhTri = _context.TrinhDoChinhTris.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChinhTri, Name = x.TenTrinhDoChinhTri }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTrinhDoChinhTri", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chinhTri, TypeId = ConstExcelController.StringId });

            var hocHam = _context.HocHams.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaHocHam, Name = x.TenHocHam }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenHocHam", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = hocHam, TypeId = ConstExcelController.StringId });

            var danToc = _context.DanTocs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaDanToc, Name = x.TenDanToc }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenDanToc", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = danToc, TypeId = ConstExcelController.StringId });

            var tonGiao = _context.TonGiaos.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTonGiao, Name = x.TenTonGiao }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTonGiao", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tonGiao, TypeId = ConstExcelController.StringId });

            columns.Add(new ExcelTemplate() { ColumnName = "NoiSinh", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayvaoDangDuBi", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoDangChinhThuc", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GhiChu", isAllowedToEdit = true, isText = true });

            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.CanBo);
            try
            {
               
              
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = controllerCode,
                    RowsToIgnore = 1,
                    isWarning = false,
                    isCode = true
                });
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = fileheader.ToUpper(),
                    RowsToIgnore = 1,
                    isWarning = false,
                    isCode = false
                });
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = LanguageResource.Export_ExcelWarning1 + "-" + LanguageResource.Export_ExcelWarning2,
                    RowsToIgnore = 1,
                    isWarning = true,
                    isCode = false
                });
            }
            catch (Exception ex)
            {
                string ss = ex.Message;

                throw;
            }

            //Header
            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(menu, columns, heading, true);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export Data
        #region Helper
        private void CheckError(CanBoVMMT insert)
        {
            var checkExistMaCB = _context.CanBos.Where(it => it.MaCanBo == insert.MaCanBo).ToList();
            if (checkExistMaCB.Count > 0 && insert.IDCanBo == null)
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
                if (String.IsNullOrEmpty(insert.MaNgachLuong) || String.IsNullOrWhiteSpace(insert.MaNgachLuong))
                {
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
                if (insert.KhoanTuNgay == null)
                {
                    ModelState.AddModelError("KhoanTuNgay", "Chưa nhập khoán từ ngày");
                }
                if (insert.KhoanDenNgay == null)
                {
                    ModelState.AddModelError("KhoanDenNgay", "Chưa nhập khoán đến ngày");
                }
            }
            var existDonVi = _context.Departments.Where(it => it.Actived == true && it.Id == insert.IdDepartment && it.IDCoSo == insert.IdCoSo);
            if (existDonVi == null) {
                ModelState.AddModelError("IdDepartment", "Đơn vị không đúng với cơ sở");
            }
        }
        public JsonResult LoadBacLuong(string maNgachLuong) {
            var data = _context.BacLuongs.Where(it => it.Actived == true && it.MaNgachLuong == maNgachLuong).OrderBy(p => p.OrderIndex).Select(it => new { MaBacLuong = it.MaBacLuong, TenBacLuong = it.TenBacLuong +" " + it.HeSo.ToString() }).ToList();
            return Json(data);
        }
        public JsonResult LoadDonVi(Guid idCoSo) {
            var data = _context.Departments.Where(it => it.Actived == true && it.IDCoSo == idCoSo).OrderBy(p => p.OrderIndex).Select(it => new { IdDepartment = it.Id, Name = it.Name  }).ToList();
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

            var DonVi = _context.Departments.Where(it => it.Actived == true ).Include(it=>it.CoSo).OrderBy(p => p.OrderIndex).Select(it => new { IdDepartment = it.Id, Name = it.Name + " "+ it.CoSo.TenCoSo }).ToList();
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

        #region Insert/Update data from excel file
        public string ExecuteImportExcelMenu(CanBoImportExcel canBoExcel)
        {
            //Check:
            //1. If MenuId == "" then => Insert
            //2. Else then => Update
            if (canBoExcel.isNullValueId == true)
            {
                CanBo canbo = canBoExcel.AddCanBo();

                _context.Entry(canbo).State = EntityState.Added;
            }
            else
            {
                CanBo canBo = _context.CanBos.Where(p => p.IDCanBo == canBoExcel.IDCanBo).FirstOrDefault();
                if (canBo != null)
                {
                    canBo = canBoExcel.EditUpdate(canBo);
                    HistoryModelRepository history = new HistoryModelRepository(_context);
                    history.SaveUpdateHistory(canBo.IDCanBo.ToString(), AccountId()!.Value, canBo);
                }
                else
                {
                    return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                            LanguageResource.CanBo, canBo.IDCanBo,
                                            string.Format(LanguageResource.Export_ExcelHeader,
                                            LanguageResource.CanBo));
                }
            }
            _context.SaveChanges();
            return LanguageResource.ImportSuccess;
        }
        #endregion Insert/Update data from excel file
        #region Check data type 
        public CanBoImportExcel CheckTemplate(object[] row)
        {
            CanBoImportExcel data = new CanBoImportExcel();
            int index = 0;
            for (int i = 0; i < row.Length; i++)
            {
              
                switch (i)
                {
                    case 0:
                        //Row Index
                        data.RowIndex = index = int.Parse(row[i].ToString());
                        break;
                    case 1:
                        // IDCanBo
                        string idCanBo = row[i].ToString();
                        if (string.IsNullOrEmpty(idCanBo) || idCanBo == "")
                        {
                            data.isNullValueId = true;
                        }
                        else
                        {
                            data.IDCanBo = Guid.Parse(idCanBo);
                            data.isNullValueId = false;
                        }
                        break;
                    case 2:
                        // Mã nhân viên
                        string maCanBo = row[i].ToString();
                        if (string.IsNullOrEmpty(maCanBo))
                        {
                            data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.MaCanBo), index);
                        }
                        else
                        {
                            data.MaCanBo = maCanBo;
                        }
                        break;
                    case 3:
                        // Mã nhân viên
                        string hoVaTen = row[i].ToString();
                        if (string.IsNullOrEmpty(hoVaTen))
                        {
                            data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.FullName), index);
                        }
                        else
                        {
                            data.HoVaTen = hoVaTen;
                        }
                        break;
                    case 4:
                        //  Ngày sinh (*)
                        string ngaySinh = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(ngaySinh) || ngaySinh == "")
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0}", LanguageResource.NgaySinh), index);
                        }
                        else
                        {
                            try
                            {
                                data.NgaySinh = DateTime.ParseExact(ngaySinh,DateFomat, new CultureInfo("en-US"));
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgaySinh, ngaySinh, index);
                            }

                        }
                        break;
                    case 5:
                        // giới tính
                        string gioiTinh = row[i].ToString();
                        if (string.IsNullOrEmpty(gioiTinh))
                        {
                            data.GioiTinh = GioiTinh.Nữ;
                        }
                        else
                        {
                            data.GioiTinh = GioiTinh.Nam;
                        }
                        break;
                    case 6:
                        //  cơ sở (*)
                        string tenCoSo = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(tenCoSo))
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0}", LanguageResource.CoSo), index);
                        }
                        else
                        {
                            var coSo = _context.CoSos.FirstOrDefault(it => it.TenCoSo == tenCoSo);
                            if (coSo != null)
                            {
                                data.IdCoSo = coSo.IdCoSo;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy cơ sở tên {0} ở dòng số {1} !", tenCoSo, index);
                            }

                        }
                        break;
                    case 7:
                        //  DonVi (*)
                        string tenDonVi = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(tenDonVi))
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0}", LanguageResource.Department), index);
                        }
                        else
                        {
                            var donVi = _context.Departments.FirstOrDefault(it => it.Name == tenDonVi && it.IDCoSo == data.IdCoSo);
                            if (donVi != null)
                            {
                                data.IdDepartment = donVi.Id;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy đơn vị tên {0} ở dòng số {1} !", tenDonVi, index);
                            }

                        }
                        break;
                    case 8:
                        //  Tinh trang (*)
                        string tenTinhTrang = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(tenTinhTrang))
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.TinhTrang), index);
                        }
                        else
                        {
                            var obj = _context.TinhTrangs.FirstOrDefault(it => it.TenTinhTrang == tenTinhTrang);
                            if (obj != null)
                            {
                                data.MaTinhTrang = obj.MaTinhTrang;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy tình trạng tên {0} ở dòng số {1} !", tenTinhTrang, index);
                            }

                        }
                        break;
                    case 9:
                        //  PhanHe (*)
                        string ten = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(ten))
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.PhanHe), index);
                        }
                        else
                        {
                            var obj = _context.PhanHes.FirstOrDefault(it => it.TenPhanHe == ten);
                            if (obj != null)
                            {
                                data.MaPhanHe = obj.MaPhanHe;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy phân hệ tên {0} ở dòng số {1} !", ten, index);
                            }

                        }
                        break;
                    case 10:
                        //  chức vụ (*)
                        string tenChucVu = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(tenChucVu))
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.ChucVu), index);
                        }
                        else
                        {
                            var obj = _context.ChucVus.FirstOrDefault(it => it.TenChucVu == tenChucVu);
                            if (obj != null)
                            {
                                data.MaChucVu = obj.MaChucVu;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy chức vụ có tên {0} ở dòng số {1} !", tenChucVu, index);
                            }

                        }
                        break;
                    case 11:
                        //  SoCCCD (*)
                        string soCCCD = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(soCCCD))
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.SoCCCD), index);
                        }
                        else
                        {
                            data.SoCCCD = soCCCD;

                        }
                        break;
                    case 12:
                        // ngày cấp SoCCCD (*)
                        string ngayCap = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(ngayCap) || ngayCap == "")
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.NgayCapCCCD), index);
                        }
                        else
                        {
                            try
                            {
                                data.NgayCapCCCD = DateTime.ParseExact(ngayCap, DateFomat, new CultureInfo("en-US"));
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayCapCCCD, ngayCap, index);
                            }

                        }
                        break;
                    case 13:
                        // so DT (*)
                        string soDT = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(soDT) || soDT == "")
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.SoDienThoai), index);
                        }
                        else
                        {
                            data.SoDienThoai = soDT;

                        }
                        break;
                    case 14:
                        // email
                        string email = row[i] == null ? null : row[i].ToString();
                        data.Email = email;
                        break;
                    case 15:
                        //  ngach luong(*)
                        string tenNgachLuong = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(tenNgachLuong))
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.NgachLuong), index);
                        }
                        else
                        {
                            var obj = _context.NgachLuongs.FirstOrDefault(it => it.TenNgachLuong == tenNgachLuong);
                            if (obj != null)
                            {
                                data.MaNgachLuong = obj.MaNgachLuong;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy ngạch lương tên {0} ở dòng số {1} !", tenNgachLuong, index);
                            }

                        }
                        break;
                    case 16:
                        //  bac luong (*)
                        string bacLuong = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(bacLuong) && data.MaNgachLuong.Length>0)
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0}", LanguageResource.BacLuong), index);
                        }
                        else if(data.MaNgachLuong.Length>0)
                        {
                            var obj = _context.BacLuongs.FirstOrDefault(it => it.MaNgachLuong == data.MaNgachLuong && it.OrderIndex  == int.Parse(bacLuong));
                            if (obj != null)
                            {
                                data.MaBacLuong = obj.MaBacLuong;
                                data.HeSoLuong = obj.HeSo;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy bậc lương {0} ở dòng số {1} !", bacLuong, index);
                            }

                        }
                        break;
                    case 18:
                        // ngày nâng bậc (*)
                        string ngayNangBac = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(ngayNangBac) && data.MaNgachLuong.Length>0)
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.NgayNangBacLuong), index);
                        }
                        else
                        {
                            try
                            {
                                data.NgayNangBacLuong = DateTime.ParseExact(ngayNangBac, DateFomat, new CultureInfo("en-US")); 
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayNangBacLuong, ngayNangBac, index);
                            }

                        }
                        break;
                    case 19:
                        // phụ cấp chức vụ
                        string pcChucVu = row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(pcChucVu) )
                        {

                            try
                            {
                                data.PhuCapChucVu = decimal.Parse(pcChucVu);
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.PhuCapChucVu, pcChucVu, index);
                            }
                        }
                        break;
                    case 20:
                        // phụ cấp vuot khung
                        string pcVuotKhung = row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(pcVuotKhung))
                        {

                            try
                            {
                                data.PhuCapVuotKhung = decimal.Parse(pcVuotKhung);
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.PhuCapVuotKhung, pcVuotKhung, index);
                            }
                        }
                        break;
                    case 21:
                        // Phu cấp kiêm nhiệm
                        string pcKiemNhiem = row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(pcKiemNhiem))
                        {

                            try
                            {
                                data.PhuCapKiemNhiem = decimal.Parse(pcKiemNhiem);
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.PhuCapKiemNhiem, pcKiemNhiem, index);
                            }
                        }
                        break;
                    case 22:
                        // Phu cấp khu vực
                        string pcKhuVuc = row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(pcKhuVuc))
                        {

                            try
                            {
                                data.PhuCapKhuVuc = decimal.Parse(pcKhuVuc);
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.PhuCapKhuVuc, pcKhuVuc, index);
                            }
                        }
                        break;
                    case 23:
                        // Luong khoán
                        string luongKhoan = row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(luongKhoan))
                        {

                            try
                            {
                                data.LuongKhoan = int.Parse(luongKhoan);
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.LuongKhoan, luongKhoan, index);
                            }
                        }
                        break;
                    case 24:
                        // KhoanTuNgay
                        string khoanTuNgay = row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(khoanTuNgay) && !string.IsNullOrWhiteSpace(khoanTuNgay))
                        {
                            try
                            {
                                data.KhoanTuNgay = DateTime.ParseExact(khoanTuNgay, DateFomat, new CultureInfo("en-US"));
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.KhoanTuNgay, khoanTuNgay, index);
                            }
                        }
                        break;
                    case 25:
                        // KhoanDenNgay
                        string khoanDenNgay = row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(khoanDenNgay) && !string.IsNullOrWhiteSpace(khoanDenNgay))
                        {
                            try
                            {
                                data.KhoanDenNgay = DateTime.ParseExact(khoanDenNgay, DateFomat, new CultureInfo("en-US")); ;
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.KhoanDenNgay, khoanDenNgay, index);
                            }
                        }
                        break;
                    case 26:
                        // SoBHXH
                       data.SoBHXH  = row[i] == null ? null : row[i].ToString();
                        
                        break;
                    case 27:
                        // SoBHYT
                        data.SoBHYT = row[i] == null ? null : row[i].ToString();

                        break;
                    case 28:
                        // MaSoThue
                        data.MaSoThue = row[i] == null ? null : row[i].ToString();

                        break;
                    case 29:
                        // KhoanDenNgay
                        string NgayVaoBienChe = row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(NgayVaoBienChe) && !string.IsNullOrWhiteSpace(NgayVaoBienChe))
                        {
                            try
                            {
                                data.NgayVaoBienChe = DateTime.ParseExact(NgayVaoBienChe, DateFomat, new CultureInfo("en-US")); ;
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayVaoBienChe, NgayVaoBienChe, index);
                            }
                        }
                        break;
                    case 30:
                        // NgayThamGiaCongTac
                        string ngayThamGiaCongTac = row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(ngayThamGiaCongTac) && !string.IsNullOrWhiteSpace(ngayThamGiaCongTac))
                        {
                            try
                            {
                                data.NgayThamGiaCongTac = DateTime.ParseExact(ngayThamGiaCongTac, DateFomat, new CultureInfo("en-US")); ;
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayThamGiaCongTac, ngayThamGiaCongTac, index);
                            }
                        }
                        break;
                    case 31:
                        //  TenHeDaoTao (*)
                        string tenHeDaoTao = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(tenHeDaoTao))
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.HeDaoTao), index);
                        }
                        else
                        {
                            var obj = _context.HeDaoTaos.FirstOrDefault(it => it.TenHeDaoTao == tenHeDaoTao);
                            if (obj != null)
                            {
                                data.MaHeDaoTao = obj.MaHeDaoTao;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy hệ đào tạo có tên {0} ở dòng số {1} !", tenHeDaoTao, index);
                            }

                        }
                        break;
                    case 32:
                        //  TenTrinhDoHocVan (*)
                        string tenHocVan = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(tenHocVan))
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.TrinhDoHocVan), index);
                        }
                        else
                        {
                            var obj = _context.TrinhDoHocVans.FirstOrDefault(it => it.TenTrinhDoHocVan == tenHocVan);
                            if (obj != null)
                            {
                                data.MaTrinhDoHocVan = obj.MaTrinhDoHocVan;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy trình độ học vấn có tên {0} ở dòng số {1} !", tenHocVan, index);
                            }

                        }
                        break;
                    case 33:
                        // Mã nhân viên
                        string chuyenNganh = row[i].ToString();
                        if (string.IsNullOrEmpty(chuyenNganh))
                        {
                            data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.ChuyenNganh), index);
                        }
                        else
                        {
                            data.ChuyenNganh = chuyenNganh;
                        }
                        break;
                    case 34:
                        //  TenTrinhDoTinHoc (*)
                        string tenTinHoc = row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(tenTinHoc))
                        {
                            var obj = _context.TrinhDoTinHocs.FirstOrDefault(it => it.TenTrinhDoTinHoc == tenTinHoc);
                            if (obj != null)
                            {
                                data.MaTrinhDoTinHoc = obj.MaTrinhDoTinHoc;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy trình độ tin học có tên {0} ở dòng số {1} !", tenTinHoc, index);
                            }
                        }
                        break;
                    case 35:
                        //  TenTrinhDoNgoaiNgu (*)
                        string tenNgoaiNgu = row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(tenNgoaiNgu))
                        {
                            var obj = _context.TrinhDoNgoaiNgus.FirstOrDefault(it => it.TenTrinhDoNgoaiNgu == tenNgoaiNgu);
                            if (obj != null)
                            {
                                data.MaTrinhDoNgoaiNgu = obj.MaTrinhDoNgoaiNgu;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy trình độ ngoại ngữ có tên {0} ở dòng số {1} !", tenNgoaiNgu, index);
                            }
                        }
                        break;
                    case 36:
                        //  MaTrinhDoChinhTri (*)
                        string tenChinhTri = row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(tenChinhTri))
                        {
                            var obj = _context.TrinhDoChinhTris.FirstOrDefault(it => it.TenTrinhDoChinhTri == tenChinhTri);
                            if (obj != null)
                            {
                                data.MaTrinhDoChinhTri = obj.MaTrinhDoChinhTri;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy trình độ chính trị có tên {0} ở dòng số {1} !", tenChinhTri, index);
                            }
                        }
                        break;
                    case 37:
                        //  TenHocHam (*)
                        string tenHocHam = row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(tenHocHam))
                        {
                            var obj = _context.HocHams.FirstOrDefault(it => it.TenHocHam == tenHocHam);
                            if (obj != null)
                            {
                                data.MaHocHam = obj.MaHocHam;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy học hàm có tên {0} ở dòng số {1} !", tenHocHam, index);
                            }
                        }
                        break;
                    case 38:
                        //  dân tộc (*)
                        string tenDanToc = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(tenDanToc))
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.DanToc), index);
                        }
                        else
                        {
                            var obj = _context.DanTocs.FirstOrDefault(it => it.TenDanToc == tenDanToc);
                            if (obj != null)
                            {
                                data.MaDanToc = obj.MaDanToc;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy dân tộc có tên {0} ở dòng số {1} !", tenDanToc, index);
                            }

                        }
                        break;
                    case 39:
                        //  tôn giáo (*)
                        string tenTonGiao = row[i] == null ? null : row[i].ToString();
                        if (string.IsNullOrEmpty(tenTonGiao))
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.TonGiao), index);
                        }
                        else
                        {
                            var obj = _context.TonGiaos.FirstOrDefault(it => it.TenTonGiao == tenTonGiao);
                            if (obj != null)
                            {
                                data.MaTonGiao = obj.MaTonGiao;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy tôn giáo có tên {0} ở dòng số {1} !", tenTonGiao, index);
                            }

                        }
                        break;
                    case 40:
                        // Mã nhân viên
                        string noiSinh = row[i].ToString();
                        if (string.IsNullOrEmpty(noiSinh))
                        {
                            data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.NoiSinh), index);
                        }
                        else
                        {
                            data.NoiSinh = noiSinh;
                        }
                        break;
                    case 41:
                        // Mã nhân viên
                        string choO = row[i].ToString();
                        if (string.IsNullOrEmpty(choO))
                        {
                            data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.ChoOHienNay), index);
                        }
                        else
                        {
                            data.ChoOHienNay = choO;
                        }
                        break;
                    case 42:
                        // KhoanDenNgay
                        string ngayVaoDangDuBi = row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(ngayVaoDangDuBi) && !string.IsNullOrWhiteSpace(ngayVaoDangDuBi))
                        {
                            try
                            {
                                data.NgayvaoDangDuBi = DateTime.ParseExact(ngayVaoDangDuBi, DateFomat, new CultureInfo("en-US")); ;
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayvaoDangDuBi, ngayVaoDangDuBi, index);
                            }
                        }
                        break;
                    case 43:
                        // KhoanDenNgay
                        string ngayVaoDang= row[i] == null ? null : row[i].ToString();
                        if (!string.IsNullOrEmpty(ngayVaoDang) && !string.IsNullOrWhiteSpace(ngayVaoDang))
                        {
                            try
                            {
                                data.NgayVaoDangChinhThuc = DateTime.ParseExact(ngayVaoDang, DateFomat, new CultureInfo("en-US")); ;
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayVaoDangChinhThuc, ngayVaoDang, index);
                            }
                        }
                        break;
                    case 44:
                        // SoBHXH
                        data.GhiChu = row[i] == null ? null : row[i].ToString();
                        break;
                }
            }
            return data;
        }
        #endregion Check data type 
    }

}
