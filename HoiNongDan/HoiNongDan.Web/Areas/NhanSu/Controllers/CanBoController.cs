
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Hosting;
using NuGet.Packaging.Signing;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.Entitys.MasterData;
using HoiNongDan.Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Mapping;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using Microsoft.Office.Interop.Word;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using DocumentFormat.OpenXml.Packaging;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Wordprocessing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using DocumentFormat.OpenXml.Spreadsheet;
using NuGet.Packaging;
using HoiNongDan.DataAccess.Migrations;
using DocumentFormat.OpenXml;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    [Authorize]
    public class CanBoController : BaseController
    {
        /// <summary>
        /// Hội nong dân cấp thành phố
        /// </summary>
        private readonly IWebHostEnvironment _hostEnvironment;
        private string [] DateFomat;
        const string controllerCode = ConstExcelController.CanBo;
        const int startIndex = 7;
        public CanBoController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context) {
            _hostEnvironment = hostEnvironment;
            DateFomat = config.GetSection("SiteSettings:DateFormat").Value.ToString().Split(',');
        }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBag("01");
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(CanBoSearchVM search) {
            return ExecuteSearch(() => {
                var data = LoadData(search);
                return PartialView(data);
            });
        }
        #endregion Index
        #region Create
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Create() {
            CreateViewBag();
            CanBoVM obj = new CanBoVM();
            obj.HinhAnh = @"\Images\login.png";
            return View(obj);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CanBoVMMT insert, IFormFile? avtFileInbox)
        {
            CheckError(insert);
            return ExecuteContainer(() => {
                var add = insert.AddCanBo();
                add.CreatedTime = DateTime.Now;
                add.Level = "10";
                add.CreatedAccountId = AccountId();
                add.IdDepartment = Guid.Parse("3E7200F5-9BCA-4D2A-8145-8B09A18C6112");
                if (avtFileInbox != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = add.MaCanBo!;
                    var uploads = Path.Combine(wwwRootPath, @"Images\canbo");

                    bool folderExists = System.IO.Directory.Exists(uploads);
                    if (!folderExists)
                        System.IO.Directory.CreateDirectory(uploads);

                    var extension = Path.GetExtension(avtFileInbox.FileName);

                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        avtFileInbox.CopyTo(fileStream);
                    }
                    add.HinhAnh = @"\Images\canbo\" + fileName + extension;
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
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
            var item = _context.CanBos.SingleOrDefault(it => it.IDCanBo == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            CanBoVMMT edit = CanBoVMMT.EditCanBo(item);
            CreateViewBag(edit.MaTinhTrang,edit.IdCoSo,edit.IdDepartment,edit.MaChucVu,edit.MaNgachLuong,edit.MaBacLuong,edit.MaTrinhDoChuyenMon,
                edit.MaTrinhDoChinhTri,edit.MaTrinhDoNgoaiNgu,edit.MaTrinhDoTinHoc,edit.MaDanToc,edit.MaTonGiao,edit.MaHocHam,edit.MaHeDaoTao,edit.MaPhanHe,item.MaHocVi);
            return View(edit);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CanBoVMMT obj, IFormFile? avtFileInbox) {

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
                    string fileName = edit.MaCanBo!;
                    var uploads = Path.Combine(wwwRootPath, @"Images\canbo");

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
                    edit.HinhAnh = @"\Images\canbo\" + fileName + extension;
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
        [HoiNongDanAuthorization]
        public IActionResult View(Guid id) {
            ProfileCanBo profileCanBo = new ProfileCanBo();
            var canBo = _context.CanBos.Where(it=>it.IDCanBo==id).Include(it => it.TinhTrang)
                    .Include(it => it.Department)
                    .Include(it => it.BacLuong)
                    .Include(it => it.ChucVu)
                    .Include(it => it.PhanHe)
                    .Include(it=>it.TrinhDoChuyenMon)
                    .Include(it=>it.TrinhDoChinhTri)
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
                        HeSoLuong = it.HeSoLuong,
                        IDCanBo = it.IDCanBo,
                        SoDienThoai = it.SoDienThoai,
                        Email = it.Email,
                        PhuCapChucVu = it.PhuCapChucVu,
                        PhuCapKiemNhiem = it.PhuCapKiemNhiem,
                        PhuCapVuotKhung = it.PhuCapVuotKhung,
                        PhuCapKhuVuc = it.PhuCapKhuVuc,
                        NgayVaoBienChe = it.NgayVaoBienChe,
                        NgayNangBacLuong = it.NgayNangBacLuong,
                        MaTrinhDoChinhTri  = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                        MaTrinhDoChuyenMon = it.TrinhDoChuyenMon.TenTrinhDoChuyenMon,
                        ChuyenNganh = it.ChuyenNganh!,
                        HinhAnh = it.HinhAnh
                    }).First();
            
            if (canBo.HinhAnh == null || canBo.HinhAnh == "")
            {
                canBo.HinhAnh = @"\images\login.png";
            }
            var qhGiaDinh = _context.QuanHeGiaDinhs.Where(it => it.IDCanBo == id).Include(it => it.LoaiQuanhe)
                            .Select(it => new QHGiaDinhDetail {
                                HoTen = it.HoTen,
                                NgaySinh = it.NgaySinh,
                                NgheNghiep = it.NgheNghiep,
                                NoiLamViec = it.NoiLamViec,
                                DiaChi = it.DiaChi,
                                GhiChu = it.GhiChu,
                                TenLoaiQuanHe = it.LoaiQuanhe.TenLoaiQuanHeGiaDinh
                            }).ToList();
            //var daoTao = _context.QuaTrinhDaoTaos.Where(it=>it.IDCanBo == id).Select(it => new DaoTaoDetailVM
            //{
            //    IDQuaTrinhDaoTao = it.IDQuaTrinhDaoTao,
            //    TenChuyenNganh = it.ChuyenNganh.TenChuyenNganh,
            //    TenHinhThucDaoTao = it.HinhThucDaoTao.TenHinhThucDaoTao,
            //    TenLoaiBangCap = it.LoaiBangCap.TenLoaiBangCap,
            //    CoSoDaoTao = it.CoSoDaoTao,
            //    NgayTotNghiep = it.NgayTotNghiep,
            //    QuocGia = it.QuocGia,
            //    GhiChu = it.GhiChu,
            //    LuanAnTN = it.LuanAnTN,
            //    FileDinhKem = it.FileDinhKem

            //}).ToList();
            //var boiDuong = _context.QuaTrinhBoiDuongs.Where(it=>it.IDCanBo == id).Select(it => new BoiDuongDetai
            //{
            //    IDQuaTrinhBoiDuong = it.IDQuaTrinhBoiDuong,
            //    MaCanBo = it.CanBo.MaCanBo,
            //    HoVaTen = it.CanBo.HoVaTen,
            //    NoiBoiDuong = it.NoiBoiDuong,
            //    NoiDung = it.NoiDung,
            //    NgayBatDau = it.NgayBatDau,
            //    NgayKetThuc = it.NgayKetThuc,
            //    GhiChu = it.GhiChu,
            //    TenHinhThucDaoTao = it.HinhThucDaoTao.TenHinhThucDaoTao,
            //}).ToList();
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
                NoiDung = it.NoiDung,
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
           // profileCanBo.DaoTao = daoTao;
           // profileCanBo.BoiDuong = boiDuong;
            profileCanBo.BoNhiem = boNhiem;
            profileCanBo.KhenThuong = khenThuong;
            profileCanBo.KyLuat = kyLuat;
            return View(profileCanBo);
        }
        public IActionResult Print(Guid id)
        {
            var data = _context.CanBos.Include(it=>it.DanToc).Include(it=>it.TonGiao).Include(it=>it.ChucVu).Include(it=>it.BacLuong).Include(it=>it.Department)
                .Include(it=>it.TrinhDoHocVan).Include(it => it.TrinhDoChuyenMon).Include(it=>it.TrinhDoNgoaiNgu).Include(it=>it.TrinhDoTinHoc).Include(it=>it.TrinhDoChinhTri)
                .SingleOrDefault(it => it.IDCanBo == id);
            if (data != null)
            {
               
                MauThongTin2C canBo = new MauThongTin2C();
                canBo.HoVaTen = data.HoVaTen.ToUpper();
                canBo.NoiSinhXa = data.NoiSinh != null ? data.NoiSinh : "........";
                canBo.QueQuanXa = data.QueQuan != null ? data.QueQuan : "........";

                canBo.GioiTinh = data.GioiTinh == GioiTinh.Nam ? "Nam" : "Nữ";
                canBo.NgaySinh = data.NgaySinh!;
                canBo.SoCCCD = data.SoCCCD! != null ? data.SoCCCD : "........"; 
                canBo.NgayCapSoCCCD = data.NgayCapCCCD! != null ? data.NgayCapCCCD : "........";
                canBo.SoBHXH = data.SoBHXH! != null ? data.SoBHXH : "........";
                canBo.NoiSinhXa = data.NoiSinh!;
                canBo.QueQuanXa = data.QueQuan!;
                canBo.DanToc = data.DanToc != null? data.DanToc!.TenDanToc: "........";
                canBo.TonGiao = data.TonGiao != null ? data.TonGiao.TenTonGiao: "........";
                canBo.ChucVu = data!.ChucVu != null ? data!.ChucVu.TenChucVu: "........";
                canBo.TrinhDoChuyenMon = data!.TrinhDoChuyenMon != null ? data!.TrinhDoChuyenMon.TenTrinhDoChuyenMon: "........";
                canBo.TrinhDoHocVan = data!.TrinhDoHocVan != null? data!.TrinhDoHocVan.TenTrinhDoHocVan : "........";
                canBo.NgoaiNgu = data!.TrinhDoNgoaiNgu != null? data!.TrinhDoNgoaiNgu.TenTrinhDoNgoaiNgu: "........";
                canBo.TinHoc = data!.TrinhDoTinHoc !=null ? data!.TrinhDoTinHoc.TenTrinhDoTinHoc: "........";
                canBo.LyLuanChinhTri = data!.TrinhDoChinhTri != null ? data!.TrinhDoChinhTri.TenTrinhDoChinhTri: "........";
                canBo.CoQuanTuyenDung = data!.Department != null ? data!.Department.Name: "........";
                canBo.NgayVaoDangDuBi = data.NgayvaoDangDuBi ?? "........";
                canBo.NgayVaoDangChinhThuc = data.NgayVaoDangChinhThuc??"........";
                canBo.PhuCapChucVu = data.PhuCapChucVu != null ? canBo.PhuCapChucVu.ToString() : "";
                

                if (!String.IsNullOrWhiteSpace(data.MaNgachLuong))
                {
                    var ngach = _context.NgachLuongs.SingleOrDefault(it => it.MaNgachLuong == data.MaNgachLuong);
                    canBo.TenNgachCongChuc = ngach.TenNgachLuong;
                    canBo.MaNgachCongChuc = ngach.MaNgachLuong;
                }

                if (data.MaBacLuong != null)
                {
                    var bacLuong = _context.BacLuongs.SingleOrDefault(it => it.MaBacLuong == data.MaBacLuong);
                    canBo.BacLuong = bacLuong.TenBacLuong;
                    canBo.HeSoLuong = bacLuong.HeSo.ToString();
                }

                if (data.NgayNangBacLuong != null)
                {
                    canBo.NgayHuong = data.NgayNangBacLuong.Value.ToString("dd/MM/yyyy");
                }

                var list = new List<MauThongTin2C>();
                list.Add(canBo);
                System.Data.DataTable dtIn = Function.AsDataTable<MauThongTin2C>(list);

                string wwwRootPath = _hostEnvironment.WebRootPath;
                var fileName = Path.Combine(wwwRootPath, @"upload\filemau\MauCanBoC2.docx");
                var fileNameSave = Path.Combine(wwwRootPath, String.Format(@"upload\temp\{0}.docx", RemoveSign4VietnameseString(data.HoVaTen.ToUpper()).Replace(" ", "_")));
                string url = XuatWordProileV1(fileName, fileNameSave, dtIn, id);
                var fileBytes = System.IO.File.ReadAllBytes(url);
                string fileNameWithFormat = string.Format("{0}.docx", RemoveSign4VietnameseString(data.HoVaTen.ToUpper()).Replace(" ", "_"));
                var contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; // MIME type for Word files
                return File(fileBytes, contentType, fileNameWithFormat);
            }
            else
            {
                return Content("Loi");
            }
        }
        #endregion View
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
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
        [HoiNongDanAuthorization]
        public IActionResult _Import()
        {
            return PartialView();
        }
        [HoiNongDanAuthorization]
        public IActionResult Import() {
            DataSet ds = GetDataSetFromExcel();
            List<string> errorList = new List<string>();
            return ExcuteImportExcel(() => {
                if (ds.Tables.Count > 0) {
                    const TransactionScopeOption opt = new TransactionScopeOption();

                    TimeSpan span = new TimeSpan(0, 0, 30, 30);
                    using (TransactionScope ts = new TransactionScope(opt))
                    {
                        DateTime dtNow = DateTime.Now;
                        System.Data.DataTable dt = ds.Tables[0];
                        int stt = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            //string aa = dr.ItemArray[0].ToString();
                            if (dt.Rows.IndexOf(dr) >= startIndex-1)
                            {
                                if (!string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                {
                                    var data = CheckTemplate(dr.ItemArray!);
                                    if (!string.IsNullOrEmpty(data.Error))
                                    {
                                        errorList.Add(data.Error);
                                    }
                                    else
                                    {
                                        data.CanBo.CreatedTime = dtNow;
                                        // Tiến hành cập nhật
                                        string result = ExecuteImportExcelCanBo(data);
                                        if (result != LanguageResource.ImportSuccess)
                                        {
                                            errorList.Add(result);
                                        }
                                        else
                                            stt++;
                                    }
                                }
                                else
                                    break;
                                //Check correct template

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
                            Data = LanguageResource.ImportSuccess + stt.ToString()
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
            catch 
            {
                return null!;
            }
        }
        #endregion Import Excel
        #region Export Data
        [HoiNongDanAuthorization]
        public IActionResult ExportCreate() {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\CanBoHoiNongDanThanhPho.xlsx");
            List<CanBo1ExcelVM> data = new List<CanBo1ExcelVM>();
            return Export(data: data, url:url, startIndex: startIndex);
        }
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit(CanBoSearchVM search)
        {
            var model  = LoadData(search);
            int stt = 1;
            model.ForEach(item => {
                item.STT = stt;
                stt++;
            });
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\CanBoHoiNongDanThanhPho.xlsx");
            return Export(model, url, startIndex);
        }
        public IActionResult Export(List<CanBo1ExcelVM> data, string url, int startIndex)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "IDCanBo", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GioiTinh", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinh", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoCCCD", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayCapSoCCCD", isAllowedToEdit = true, isDateTime = true });

            var chuVu = _context.ChucVus.ToList().Select(x => new DropdownModel { Id = x.MaChucVu, Name = x.TenChucVu }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenChucVu", isAllowedToEdit = true, isDropdownlist = true, DropdownData = chuVu, TypeId = ConstExcelController.GuidId , Title = "Chức vụ" });
            columns.Add(new ExcelTemplate() { ColumnName = "DonVi", isAllowedToEdit = true, isText = true });
          
            columns.Add(new ExcelTemplate() { ColumnName = "SoQuyetDinhBoNhiem", isAllowedToEdit = true, isText = true });

            var danToc = _context.DanTocs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaDanToc, Name = x.TenDanToc }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenDanToc", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = danToc, TypeId = ConstExcelController.StringId, Title = "Dân tộc" });

            var tonGiao = _context.TonGiaos.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTonGiao, Name = x.TenTonGiao }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTonGiao", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tonGiao, TypeId = ConstExcelController.StringId , Title = "Tôn giáo" });


            columns.Add(new ExcelTemplate() { ColumnName = "NoiSinh", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay_XaPhuong", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay_QuanHuyen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayvaoDangDuBi", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoDangChinhThuc", isAllowedToEdit = true, isDateTime = true });

            var hocVan = _context.TrinhDoHocVans.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoHocVan, Name = x.TenTrinhDoHocVan }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoHocVan", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = hocVan, TypeId = ConstExcelController.StringId, Title = "Trình độ học vấn" });
            var chuyenNganh = _context.TrinhDoChuyenMons.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChuyenMon, Name = x.TenTrinhDoChuyenMon }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChuyenMon", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chuyenNganh, TypeId = ConstExcelController.StringId, Title = "Chuyên môn" });
            columns.Add(new ExcelTemplate() { ColumnName = "ChuyenNganh", isAllowedToEdit = true, isText = true });

            var chinhTri = _context.TrinhDoChinhTris.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChinhTri, Name = x.TenTrinhDoChinhTri }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTrinhDoChinhTri", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chinhTri, TypeId = ConstExcelController.StringId,Title= "Chính trị" });

            var ngoaiNgu = _context.TrinhDoNgoaiNgus.ToList().Select(x => new DropdownModel { Id = x.MaTrinhDoNgoaiNgu, Name = x.TenTrinhDoNgoaiNgu }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTrinhDoNgoaiNgu", isAllowedToEdit = true, isDropdownlist = true, DropdownData = ngoaiNgu, TypeId = ConstExcelController.GuidId, Title = "Ngoại ngữ" });

            var tinHoc = _context.TrinhDoTinHocs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoTinHoc, Name = x.TenTrinhDoTinHoc }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTrinhDoTinHoc", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tinHoc, TypeId = ConstExcelController.StringId , Title = "Tin học" });

            columns.Add(new ExcelTemplate() { ColumnName = "NgayThamGiaCongTac", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayThamGiaCapUyDang", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayThamGiaHDND", isAllowedToEdit = true, isText = true });


            columns.Add(new ExcelTemplate() { ColumnName = "TenNgachLuong", isAllowedToEdit = true, isText = true });

            columns.Add(new ExcelTemplate() { ColumnName = "HeSoLuong", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayNangBacLuong", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "PhuCapVuotKhung", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayNangBacLuongLanSau", isAllowedToEdit = true, isDateTime = true });

            columns.Add(new ExcelTemplate() { ColumnName = "NVCTTW", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GiangVienKiemChuc", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "QLCapPhong", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "KTQP", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "QLNNCV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "QLNNCVC", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "QLNNCVCC", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DanhGiaCBCC", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DanhGiaDangVien", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "BanChapHanh", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GhiChu", isAllowedToEdit = true, isText = true });
            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.CanBo) + " " + DateTime.Now.Year.ToString();
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
            byte[] filecontent = ClassExportExcel.ExportExcel(url, data, columns, heading, true, startIndex);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export Data
        #region Helper
        private List<CanBo1ExcelVM> LoadData(CanBoSearchVM search) {
            var model = _context.CanBos.Where(it => it.IsCanBo == true && it.Level == "10" )
                .Include(it => it.DanToc).Include(it => it.TonGiao).Include(it => it.TrinhDoTinHoc).Include(it=>it.Department)
                .Include(it => it.TrinhDoChinhTri).Include(it => it.TrinhDoNgoaiNgu).Include(it=>it.TrinhDoHocVan).Include(it=>it.TrinhDoChuyenMon).AsQueryable();
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

            var data = model.Select(it => new CanBo1ExcelVM
            {
   
                IDCanBo = it.IDCanBo,
                HoVaTen = it.HoVaTen,
                NgaySinh = it.NgaySinh,
                GioiTinh = it.GioiTinh == GioiTinh.Nữ ?"":"X",
                TenChucVu = it.ChucVu!.TenChucVu,
                DonVi = it.Department.Name,
                SoQuyetDinhBoNhiem = it.SoQuyetDinhBoNhiem,
                TenDanToc = it.DanToc!.TenDanToc,
                TenTonGiao = it.TonGiao!.TenTonGiao,
                NoiSinh = it.NoiSinh,
                ChoOHienNay = it.ChoOHienNay,
                ChoOHienNay_XaPhuong = it.ChoOHienNay_XaPhuong,
                ChoOHienNay_QuanHuyen = it.ChoOHienNay_QuanHuyen,
                NgayvaoDangDuBi = it.NgayvaoDangDuBi,
                NgayVaoDangChinhThuc = it.NgayVaoDangChinhThuc,
                MaTrinhDoHocVan = it.TrinhDoHocVan.TenTrinhDoHocVan,
                MaTrinhDoChuyenMon = it.TrinhDoChuyenMon.TenTrinhDoChuyenMon,
                ChuyenNganh = it.ChuyenNganh!,
                TenTrinhDoChinhTri = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                TenTrinhDoNgoaiNgu = it.TrinhDoNgoaiNgu!.TenTrinhDoNgoaiNgu,
                TenTrinhDoTinHoc = it.TrinhDoTinHoc!.TenTrinhDoTinHoc,
                NgayThamGiaCongTac = it.NgayThamGiaCongTac,
                NgayThamGiaCapUyDang = it.NgayThamGiaCapUyDang,
                NgayThamGiaHDND = it.NgayThamGiaHDND,
                TenNgachLuong = it.MaNgachLuong != null ? it.MaNgachLuong! + "/" + it.BacLuong.OrderIndex!.ToString() : "",
                HeSoLuong = it.HeSoLuong!,
                NgayNangBacLuong = it.NgayNangBacLuong!,
                PhuCapVuotKhung = it.PhuCapVuotKhung!,
                NgayNangBacLuongLanSau = it.NgayNangBacLuong != null? it.NgayNangBacLuong.Value.AddYears(it.BacLuong.NgachLuong.NamTangLuong):null,
                NVCTTW = it.DaoTaoBoiDuongs.Where(it => it.MaHinhThucDaoTao == "03").Count() > 0 ? "X" : "",
                GiangVienKiemChuc = it.DaoTaoBoiDuongs.Where(it => it.MaHinhThucDaoTao == "04").Count() > 0 ? "X" : "",
                QLCapPhong = it.DaoTaoBoiDuongs.Where(it => it.MaHinhThucDaoTao == "05").Count() > 0 ? "X" : "",
                KTQP = it.DaoTaoBoiDuongs.Where(it => it.MaHinhThucDaoTao == "06").Count() > 0 ? "X" : "",
                QLNNCV = it.DaoTaoBoiDuongs.Where(it => it.MaHinhThucDaoTao == "07").Count() > 0 ? "X" : "",
                QLNNCVC = it.DaoTaoBoiDuongs.Where(it => it.MaHinhThucDaoTao == "08").Count() > 0 ? "X" : "",
                QLNNCVCC = it.DaoTaoBoiDuongs.Where(it => it.MaHinhThucDaoTao == "09").Count() > 0 ? "X" : "",
                DanhGiaCBCC = it.DanhGiaCBCC!,
                DanhGiaDangVien = it.DanhGiaDangVien!,
                GhiChu = it.GhiChu!
            }).OrderBy(it=>it.DonVi).ToList();
            return data;
        }
        private void CheckError(CanBoVMMT insert)
        {
            if (!String.IsNullOrWhiteSpace(insert.MaCanBo)) {
                var checkExistMaCB = _context.CanBos.Where(it => it.MaCanBo == insert.MaCanBo).ToList();
                if ((checkExistMaCB.Count > 0 && insert.IDCanBo == null) || (checkExistMaCB.Count > 1 && insert.IDCanBo != null ) )
                {
                    ModelState.AddModelError("MaCanBo", "Mã cán bộ tồn tại không thể thêm");
                }
            }
            // Check trình độ học vấn sau đại học
            if (insert.MaTrinhDoChuyenMon == "SDH")
            {
                if (String.IsNullOrEmpty(insert.MaHocHam) || String.IsNullOrWhiteSpace(insert.MaHocHam))
                {
                    ModelState.AddModelError("MaHocHam", "Học hàm chưa được chọn");
                }
            }
            if (insert.MaTrinhDoChuyenMon != "SDH" && !String.IsNullOrEmpty(insert.MaHocHam) && !String.IsNullOrWhiteSpace(insert.MaHocHam))
            {
                if (String.IsNullOrEmpty(insert.MaHocHam) || String.IsNullOrWhiteSpace(insert.MaHocHam))
                {
                    ModelState.AddModelError("MaHocHam", "Học hàm không hợp lệ");
                }
            }
            //if (insert.MaPhanHe != "03")
            //{
            //    if (String.IsNullOrEmpty(insert.MaNgachLuong) || String.IsNullOrWhiteSpace(insert.MaNgachLuong))
            //    {
            //        ModelState.AddModelError("MaNgachLuong", "Ngạch lương chưa được chọn");
            //    }
            //    if (insert.MaBacLuong == null)
            //    {
            //        ModelState.AddModelError("MaBacLuong", "Bậc lương chưa được chọn");
            //    }
            //    if (insert.NgayNangBacLuong == null)
            //    {
            //        ModelState.AddModelError("NgayNangBacLuong", "Ngày nâng bậc chưa chọn");
            //    }
            //}
            //if (insert.MaPhanHe == "03")
            //{
            //    if (insert.LuongKhoan <= 0)
            //    {
            //        ModelState.AddModelError("LuongKhoan", "Chưa nhập số tiền lương khoán");
            //    }
            //    if (insert.KhoanTuNgay == null)
            //    {
            //        ModelState.AddModelError("KhoanTuNgay", "Chưa nhập khoán từ ngày");
            //    }
            //    if (insert.KhoanDenNgay == null)
            //    {
            //        ModelState.AddModelError("KhoanDenNgay", "Chưa nhập khoán đến ngày");
            //    }
            //}
            //var existDonVi = _context.Departments.Where(it => it.Actived == true && it.Id == insert.IdDepartment && it.IDCoSo == insert.IdCoSo);
            //if (existDonVi == null) {
            //    ModelState.AddModelError("IdDepartment", "Đơn vị không đúng với cơ sở");
            //}
        }
        [HttpGet]
        public IActionResult LoadBacLuong(string maNgachLuong) {
            var data = _context.BacLuongs.Where(it => it.Actived == true && it.MaNgachLuong == maNgachLuong).OrderBy(p => p.OrderIndex).Select(it => new { MaBacLuong = it.MaBacLuong, TenBacLuong = it.TenBacLuong +": " + it.HeSo.ToString() }).ToList();
            return Json(data);
        }
        public IActionResult LoadDonVi(Guid idCoSo) {
            var data = _context.Departments.Where(it => it.Actived == true && it.IDCoSo == idCoSo).OrderBy(p => p.OrderIndex).Select(it => new { IdDepartment = it.Id, Name = it.Name  }).ToList();
            return Json(data);
        }
        public IActionResult GetHoSoLuong(Guid id)
        {
            var data = _context.BacLuongs.SingleOrDefault(it => it.Actived == true && it.MaBacLuong == id);
            return Json(data);
        }
        [NonAction]
        private void CreateViewBag(String? maTinhTrang = null, Guid? IdCoSo = null, Guid? IdDepartment = null,
            Guid? maChucVu = null, String? maNgachLuong = null, Guid? maBacLuong = null,
            String? maTrinhDoChuyenMon = null, String? maTrinhDoChinhTri = null,
            Guid? maTrinhDoNgoaiNgu = null, String? maTrinhDoTinHoc = null,
            String? maDanToc = null, String? maTonGiao = null, String? maHocHam = null,
            String? maHeDaoTao = null, String? maPhanhe = null, string? maHocVi = null, string? maTrinDoHocVan = null)
        {
            var MenuList = _context.TinhTrangs.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { MaTinhTrang = it.MaTinhTrang, TenTinhTrang = it.TenTinhTrang }).ToList();
            ViewBag.MaTinhTrang = new SelectList(MenuList, "MaTinhTrang", "TenTinhTrang", maTinhTrang);

            var MenuListCoSo = _context.CoSos.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { IdCoSo = it.IdCoSo, TenCoSo = it.TenCoSo }).ToList();
            ViewBag.IdCoSo = new SelectList(MenuListCoSo, "IdCoSo", "TenCoSo", IdCoSo);

            var DonVi = _context.Departments.Where(it => it.Actived == true ).Include(it=>it.CoSo).OrderBy(p => p.OrderIndex).Select(it => new { IdDepartment = it.Id, Name = it.Name }).ToList();
            ViewBag.IdDepartment = new SelectList(DonVi, "IdDepartment", "Name", IdDepartment);

            var chucVu = _context.ChucVus.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { MaChucVu = it.MaChucVu, TenChucVu = it.TenChucVu }).ToList();
            ViewBag.MaChucVu = new SelectList(chucVu, "MaChucVu", "TenChucVu", maChucVu);

            var ngachLuong = _context.NgachLuongs.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { MaNgachLuong = it.MaNgachLuong, TenNgachLuong = it.TenNgachLuong }).ToList();
            ViewBag.MaNgachLuong = new SelectList(ngachLuong, "MaNgachLuong", "TenNgachLuong", maNgachLuong);

            var bacLuong = _context.BacLuongs.Where(it => it.Actived == true && (it.MaNgachLuong == maNgachLuong || maNgachLuong == null)).OrderBy(p => p.OrderIndex).Select(it => new { MaBacLuong = it.MaBacLuong, TenBacLuong = it.TenBacLuong + " " + it.HeSo.ToString() }).ToList();
            ViewBag.MaBacLuong = new SelectList(bacLuong, "MaBacLuong", "TenBacLuong", maBacLuong);
            
            var trinhDoHocVan = _context.TrinhDoHocVans.Select(it => new { MaTrinhDoHocVan = it.MaTrinhDoHocVan, TenTrinhDoHocVan = it.TenTrinhDoHocVan }).ToList();
            ViewBag.MaTrinhDoHocVan = new SelectList(trinhDoHocVan, "MaTrinhDoHocVan", "TenTrinhDoHocVan", maTrinDoHocVan);

            var trinhDoChuyenMon = _context.TrinhDoChuyenMons.Select(it => new { MaTrinhDoChuyenMon = it.MaTrinhDoChuyenMon, TenTrinhDoChuyenMon = it.TenTrinhDoChuyenMon }).ToList();
            ViewBag.MaTrinhDoChuyenMon = new SelectList(trinhDoChuyenMon, "MaTrinhDoChuyenMon", "TenTrinhDoChuyenMon", maTrinhDoChuyenMon);


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

            var hocVi = _context.HocVis.Select(it => new { MaHocVi = it.MaHocVi, TenHocVi = it.TenHocVi }).ToList();
            ViewBag.MaHocVi = new SelectList(hocVi, "MaHocVi", "TenHocVi", maHocVi);
         
        }
        #endregion Helper
        #region Insert/Update data from excel file
        [NonAction]
        public string ExecuteImportExcelCanBo(CanBo1ImportExcel canBoExcel)
        {
            //Check:
            //1. If MenuId == "" then => Insert
            //2. Else then => Update
            try
            {
                if (canBoExcel.isNullValueId == true)
                {
                    if (canBoExcel.daoTaoBoiDuongs.Count() > 0)
                    {
                        _context.DaoTaoBoiDuongs.AddRange(canBoExcel.daoTaoBoiDuongs);
                    }
                    //canBoExcel.CanBo.IdDepartment = Guid.Parse("3E7200F5-9BCA-4D2A-8145-8B09A18C6112");
                    canBoExcel.CanBo.Level = "10";
                    canBoExcel.CanBo.CreatedAccountId = AccountId();
                    _context.Entry(canBoExcel.CanBo).State = EntityState.Added;

                }
                else
                {
                    var canBo = _context.CanBos.Include(it => it.DaoTaoBoiDuongs).SingleOrDefault(p => p.IDCanBo == canBoExcel.CanBo.IDCanBo)!;
                    canBoExcel.CanBo.Level = "10";
                    if (canBo != null)
                    {
                        canBo = EditCanBoHoiNongDanThanhPho(canBo, canBoExcel.CanBo);

                        var daotao = _context.DaoTaoBoiDuongs.Where(it => it.IDCanBo == canBo.IDCanBo);
                        if (daotao.Count() > 0)
                        {
                            _context.DaoTaoBoiDuongs.RemoveRange(daotao);
                        }

                        if (canBoExcel.daoTaoBoiDuongs.Count > 0) {
                            _context.DaoTaoBoiDuongs.AddRange(canBoExcel.daoTaoBoiDuongs);
                        }

                     

                        HistoryModelRepository history = new HistoryModelRepository(_context);


                        history.SaveUpdateHistory(canBo.IDCanBo.ToString(), AccountId()!.Value, canBo);
                        canBo.LastModifiedAccountId = AccountId();
                        canBo.LastModifiedTime = DateTime.Now;
                        _context.Entry(canBo).State = EntityState.Modified;
                    }
                    else
                    {
                        return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                                LanguageResource.CanBo, canBoExcel!.CanBo.HoVaTen,
                                                string.Format(LanguageResource.Export_ExcelHeader,
                                                LanguageResource.CanBo));
                    }
                }
                _context.SaveChanges();
                return LanguageResource.ImportSuccess;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }
        private CanBo EditCanBoHoiNongDanThanhPho(CanBo old, CanBo news) {

            old.HoVaTen = news.HoVaTen;
            old.NgaySinh = news.NgaySinh;
            old.GioiTinh = news.GioiTinh;
            old.SoCCCD = news.SoCCCD;
            old.NgayCapCCCD = news.NgayCapCCCD;
            old.MaChucVu = news.MaChucVu;
            old.IdDepartment = news.IdDepartment;

            old.SoQuyetDinhBoNhiem = news.SoQuyetDinhBoNhiem;
            old.MaDanToc = news.MaDanToc;
            old.MaTonGiao = news.MaTonGiao;
            old.NoiSinh = news.NoiSinh;
            old.ChoOHienNay = news.ChoOHienNay;
            old.ChoOHienNay_XaPhuong = news.ChoOHienNay_XaPhuong;
            old.ChoOHienNay_QuanHuyen = news.ChoOHienNay_QuanHuyen;
            old.NgayvaoDangDuBi = news.NgayvaoDangDuBi;
            old.NgayVaoDangChinhThuc = news.NgayVaoDangChinhThuc;
            old.DangVien = news.DangVien;
            old.MaTrinhDoHocVan = news.MaTrinhDoHocVan;
            old.MaTrinhDoChuyenMon = news.MaTrinhDoChuyenMon;
            old.ChuyenNganh = news.ChuyenNganh;
            old.MaTrinhDoChinhTri = news.MaTrinhDoChinhTri;
            old.MaTrinhDoNgoaiNgu = news.MaTrinhDoNgoaiNgu;
            old.MaTrinhDoTinHoc = news.MaTrinhDoTinHoc;
            old.NgayThamGiaCongTac = news.NgayThamGiaCongTac;
            old.NgayThamGiaCapUyDang = news.NgayThamGiaCapUyDang;
            old.NgayThamGiaHDND = news.NgayThamGiaHDND;
            old.MaNgachLuong = news.MaNgachLuong;
            old.MaBacLuong = news.MaBacLuong;
            old.HeSoLuong = news.HeSoLuong;
            old.PhuCapVuotKhung = news.PhuCapVuotKhung;
            old.NgayNangBacLuong = news.NgayNangBacLuong;
            old.DanhGiaCBCC = news.DanhGiaCBCC;
            old.DanhGiaDangVien = news.DanhGiaDangVien;
            old.IsBanChapHanh = news.IsBanChapHanh;
            old.GhiChu = news.GhiChu;
            return old;
        }
        #endregion Insert/Update data from excel file
        #region Check data type 
        [NonAction]
        public CanBo1ImportExcel CheckTemplate(object[] row)
        {
            CanBo1ImportExcel data = new CanBo1ImportExcel();
            CanBo canBo = new CanBo();
            canBo.MaTinhTrang = "01";
            canBo.MaPhanHe = "01";
            canBo.IsCanBo = true;
            canBo.Actived = true;
            canBo.Level = "10";
            canBo.IDCanBo = Guid.NewGuid();
            List<DaoTaoBoiDuong> daoTaoBoiDuongs = new List<DaoTaoBoiDuong>();
            int index = 0;
            string value = "";
            for (int i = 0; i < row.Length; i++)
            {
                value = "";
                value = row[i]!.ToString()!;
                try
                {
                    switch (i)
                    {
                        case 0:
                            //Row Index
                            data.RowIndex = index = int.Parse(value);
                            break;
                        case 1:
                            // IDCanBo
                            if (string.IsNullOrEmpty(value) || value == "")
                            {
                                data.isNullValueId = true;
                            }
                            else
                            {
                                canBo.IDCanBo = Guid.Parse(value);
                                data.isNullValueId = false;
                            }
                            break;
                        case 2:
                            // Họ và tên
                            if (!string.IsNullOrWhiteSpace(value))
                                canBo.HoVaTen = value;
                            else
                                data.Error += string.Format("Dòng số {0} chưa nhập họ và tên ", index);
                            break;
                        case 3:
                            // Giới tính
                            if (!string.IsNullOrWhiteSpace(value))
                                canBo.GioiTinh = GioiTinh.Nam;
                            else
                                canBo.GioiTinh = GioiTinh.Nữ;
                            break;
                        case 4:
                            // Ngày sinh
                            if (!string.IsNullOrWhiteSpace(value))
                                canBo.NgaySinh = value;
                            else
                                data.Error += string.Format("Dòng số {0} chưa nhập ngày sinh ", index);
                            break;
                        case 5:
                            //CMND/CCCD
                            try
                            {
                                if (!String.IsNullOrWhiteSpace(value))
                                {
                                    if (value.Length != 12)
                                    {
                                        data.Error = (string.Format("Dữ liệu dòng {0} Số CCCD {1} phải là 12 số!", index, value));
                                    }
                                    Regex rg = new Regex(@"^\d+$");
                                    if (!rg.IsMatch(value))
                                    {
                                        data.Error =(string.Format("Dữ liệu dòng {0} Số CCCD {1} phải là kiểu số!", index, value));
                                    }
                                    canBo.SoCCCD = value;
                                }
                                
                            }
                            catch
                            {

                            }
                            break;
                        case 6:
                            //ngay cấp CMND/CCCD
                            try
                            {
                                if (!String.IsNullOrWhiteSpace(value))
                                {
                                    canBo.NgayCapCCCD = Function.ConvertStringToDate(value).ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    //data.Error =($"Dòng {index} Chưa nhập số ngày cấp CCCD");
                                }

                            }
                            catch
                            {
                            }
                            break;
                        case 7:
                            //  chức vụ (*) - Tên
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                 var obj = _context.ChucVus.FirstOrDefault(it => it.TenChucVu == value);
                                if (obj != null)
                                {
                                    canBo.MaChucVu = obj.MaChucVu;
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy chức vụ có tên {0} ở dòng số {1} !", value, index);
                                }
                            }
                           
                            break;
                        case 8:
                            // Chức vụ - dong vi
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                var obj = _context.Departments.FirstOrDefault(it => it.Name == value);
                                if (obj != null)
                                {
                                    canBo.IdDepartment = obj.Id;
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy đơn vị có tên {0} ở dòng số {1} !", value, index);
                                }
                            }
                            break;
                        case 9:
                            //  Quyết định bổ nhiệm (*)
                            canBo.SoQuyetDinhBoNhiem = value;
                            break;
                        case 10:
                            //  dân tộc (*)
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                var obj = _context.DanTocs.FirstOrDefault(it => it.TenDanToc == value);
                                if (obj != null)
                                {
                                    canBo.MaDanToc = obj.MaDanToc;
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy dân tộc có tên {0} ở dòng số {1} !", value, index);
                                }
                            }
                            break;
                        case 11:
                            //  tôn giáo (*)
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                var obj = _context.TonGiaos.FirstOrDefault(it => it.TenTonGiao == value);
                                if (obj != null)
                                {
                                    canBo.MaTonGiao = obj.MaTonGiao;
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy tôn giáo có tên {0} ở dòng số {1} !", value, index);
                                }
                            }
                           
                            break;
                        case 12:
                            // Nơi sinh
                            canBo.NoiSinh = value;
                            break;
                        case 13:
                            // Chổ ở hiện nay
                            canBo.ChoOHienNay = value;
                            break;
                        case 14:
                            // Chổ ở hiện nay
                            canBo.ChoOHienNay_XaPhuong = value;
                            break;
                        case 15:
                            // Chổ ở hiện nay
                            canBo.ChoOHienNay_QuanHuyen = value;
                            break;
                        case 16:
                            // Ngày vào đảng dự bị
                            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                canBo.NgayvaoDangDuBi = value;
                                
                            }
                            break;
                        case 17:
                            // Ngày vào đảng chính thức
                            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                try
                                {
                                    //canBo.NgayVaoDangChinhThuc = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US")); ;
                                    canBo.NgayVaoDangChinhThuc = value;
                                    canBo.DangVien = true;
                                }
                                catch (Exception)
                                {

                                    data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayVaoDangChinhThuc, value, index);
                                }
                            }
                            break;
                        case 18:
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                var obj = _context.TrinhDoHocVans.FirstOrDefault(it => it.TenTrinhDoHocVan == value);
                                if (obj != null)
                                {
                                    canBo.MaTrinhDoHocVan = obj.MaTrinhDoHocVan;
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy trình độ học vấn có tên {0} ở dòng số {1} !", value, index);
                                }
                            }
                            break;
                        case 19:
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                var obj = _context.TrinhDoChuyenMons.FirstOrDefault(it => it.TenTrinhDoChuyenMon == value);
                                if (obj != null)
                                {
                                    canBo.MaTrinhDoChuyenMon = obj.MaTrinhDoChuyenMon;
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy trình độ chuyên môm có tên {0} ở dòng số {1} !", value, index);
                                }
                            }
                            break;
                        case 20:
                            // Chuyên ngành
                            canBo.ChuyenNganh = value;
                            
                            break;
                        case 21:
                            //  Trình Độ Chính Trị (*)
                            if (!string.IsNullOrEmpty(value))
                            {
                                var obj = _context.TrinhDoChinhTris.FirstOrDefault(it => it.TenTrinhDoChinhTri == value);
                                if (obj != null)
                                {
                                    canBo.MaTrinhDoChinhTri = obj.MaTrinhDoChinhTri;
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy trình độ chính trị có tên {0} ở dòng số {1} !", value, index);
                                }
                            }
                            break;
                        case 22:
                            //  TenTrinhDoNgoaiNgu (*)
                            if (!string.IsNullOrEmpty(value))
                            {
                                var obj = _context.TrinhDoNgoaiNgus.FirstOrDefault(it => it.TenTrinhDoNgoaiNgu == value);
                                if (obj != null)
                                {
                                    canBo.MaTrinhDoNgoaiNgu = obj.MaTrinhDoNgoaiNgu;
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy trình độ ngoại ngữ có tên {0} ở dòng số {1} !", value, index);
                                }
                            }
                            break;
                        case 23:
                            //  TenTrinhDoTinHoc (*)
                            if (!string.IsNullOrEmpty(value))
                            {
                                var obj = _context.TrinhDoTinHocs.FirstOrDefault(it => it.TenTrinhDoTinHoc == value);
                                if (obj != null)
                                {
                                    canBo.MaTrinhDoTinHoc = obj.MaTrinhDoTinHoc;
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy trình độ tin học có tên {0} ở dòng số {1} !", value, index);
                                }
                            }
                            break;
                        case 24:
                            //  Thời gian công tác Hội (*)
                            canBo.NgayThamGiaCongTac = value;
                            break;
                        case 25:
                            //  Tham gia cấp ủy đảng (*)
                            canBo.NgayThamGiaCapUyDang = value;
                            break;
                        case 26:
                            //  Tham gia HĐND (*)
                            canBo.NgayThamGiaHDND = value;
                            break;
                        case 27:
                            // Mã số ngạch/bậc
                            if (!string.IsNullOrEmpty(value))
                            {
                                string[] temp = value.Split("/");
                                if (temp.Count() > 0)
                                {
                                    var obj = _context.NgachLuongs.FirstOrDefault(it => it.MaNgachLuong == temp[0]);
                                    if (obj != null)
                                    {
                                        canBo.MaNgachLuong = obj.MaNgachLuong;
                                    }
                                    else
                                    {
                                        data.Error += string.Format("Không tìm thấy ngạch lương tên {0} ở dòng số {1} !", value, index);
                                    }
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy bậc lương {0} ở dòng số {1} !", value, index);
                                }
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.NgachLuong), index);
                            }
                            break;
                        case 28:
                            //  hệ số lương (*)
                            if (!string.IsNullOrEmpty(value) )
                            {
                                var bac = _context.BacLuongs.FirstOrDefault(it => it.MaNgachLuong == canBo.MaNgachLuong && it.HeSo == decimal.Parse(value));
                                if (bac != null)
                                {
                                    canBo.MaBacLuong = bac.MaBacLuong;
                                    canBo.HeSoLuong = bac.HeSo;
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy bậc lương {0} ở dòng số {1} !", value, index);
                                }
                            }
                            break;
                        case 29:
                            // ngày nâng bậc (*)
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                try
                                {
                                    value = Function.RepleceAllString(value);
                                    canBo.NgayNangBacLuong = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US"));
                                }
                                catch (Exception)
                                {

                                    data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayNangBacLuong, value, index);
                                }
                            }
                            break;
                        case 30:
                            // Phụ cấp vượt khung %
                            if (!string.IsNullOrEmpty(value))
                            {
                                try
                                {
                                    canBo.PhuCapVuotKhung = decimal.Parse(value);
                                }
                                catch (Exception)
                                {

                                    data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.PhuCapVuotKhung, value, index);
                                }
                            }
                            break;
                        
                        case 32:
                            // NVCT Hội do TW

                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                daoTaoBoiDuongs.Add(new DaoTaoBoiDuong
                                {
                                    Id = Guid.NewGuid(),
                                    IDCanBo = canBo.IDCanBo,
                                    MaHinhThucDaoTao = "03",
                                    MaLoaiBangCap = "11",
                                    Actived = true,
                                    NoiDungDaoTao = value,
                                    CreatedAccountId = AccountId(),
                                    CreatedTime = DateTime.Now,
                                    GhiChu = value
                                });
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.NgayNangBacLuong), index);
                            }
                            
                            break;
                        case 33:
                            // Giảng viên kiêm chức

                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                daoTaoBoiDuongs.Add(new DaoTaoBoiDuong
                                {
                                    Id = Guid.NewGuid(),
                                    IDCanBo = canBo.IDCanBo,
                                    MaHinhThucDaoTao = "04",
                                    MaLoaiBangCap = "11",
                                    Actived = true,
                                    NoiDungDaoTao = value,
                                    CreatedAccountId = AccountId(),
                                    CreatedTime = DateTime.Now,
                                    GhiChu = value
                                });
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.NgayNangBacLuong), index);
                            }
                            
                            break;
                        case 34:
                            // QL cấp phòng

                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                daoTaoBoiDuongs.Add(new DaoTaoBoiDuong
                                {
                                    Id = Guid.NewGuid(),
                                    IDCanBo = canBo.IDCanBo,
                                    MaHinhThucDaoTao = "05",
                                    MaLoaiBangCap = "11",
                                    Actived = true,
                                    NoiDungDaoTao = value,
                                    CreatedAccountId = AccountId(),
                                    CreatedTime = DateTime.Now,
                                    GhiChu = value
                                });
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.NgayNangBacLuong), index);
                            }
                            
                            break;
                        case 35:
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                daoTaoBoiDuongs.Add(new DaoTaoBoiDuong
                                {
                                    Id = Guid.NewGuid(),
                                    IDCanBo = canBo.IDCanBo,
                                    MaHinhThucDaoTao = "06",
                                    MaLoaiBangCap = "11",
                                    Actived = true,
                                    NoiDungDaoTao = value,
                                    CreatedAccountId = AccountId(),
                                    CreatedTime = DateTime.Now,
                                    GhiChu = value
                                });
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.NgayNangBacLuong), index);
                            }
                           
                            break;
                        case 36:
                            if (string.IsNullOrWhiteSpace(value))
                            {
                                daoTaoBoiDuongs.Add(new DaoTaoBoiDuong
                                {
                                    Id = Guid.NewGuid(),
                                    IDCanBo = canBo.IDCanBo,
                                    MaHinhThucDaoTao = "07",
                                    MaLoaiBangCap = "11",
                                    Actived = true,
                                    NoiDungDaoTao = value,
                                    CreatedAccountId = AccountId(),
                                    CreatedTime = DateTime.Now,
                                    GhiChu = value
                                });
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.NgayNangBacLuong), index);
                            }
                            
                            break;
                        case 37:
                            // QLNN ngạch chuyên viên chính

                            if (string.IsNullOrWhiteSpace(value))
                            {
                                daoTaoBoiDuongs.Add(new DaoTaoBoiDuong
                                {
                                    Id = Guid.NewGuid(),
                                    IDCanBo = canBo.IDCanBo,
                                    MaHinhThucDaoTao = "08",
                                    MaLoaiBangCap = "11",
                                    Actived = true,
                                    NoiDungDaoTao = value,
                                    CreatedAccountId = AccountId(),
                                    CreatedTime = DateTime.Now,
                                    GhiChu = value
                                });
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.NgayNangBacLuong), index);
                            }
                           
                            break;
                        case 38:
                            // QLNN ngạch chuyên viên CC

                            if (string.IsNullOrWhiteSpace(value))
                            {
                                daoTaoBoiDuongs.Add(new DaoTaoBoiDuong
                                {
                                    Id = Guid.NewGuid(),
                                    IDCanBo = canBo.IDCanBo,
                                    MaHinhThucDaoTao = "09",
                                    MaLoaiBangCap = "11",
                                    Actived = true,
                                    NoiDungDaoTao = value,
                                    CreatedAccountId = AccountId(),
                                    CreatedTime = DateTime.Now,
                                    GhiChu = value
                                });
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.NgayNangBacLuong), index);
                            }
                            
                            break;
                        case 39:
                            // ngày nâng bậc (*)
                            canBo.DanhGiaCBCC = value;
                            break;
                        case 40:
                            // ngày nâng bậc (*)
                            canBo.DanhGiaDangVien = value;
                            break;
                        case 41:
                            //  TenTrinhDoTinHoc (*)
                            if (!string.IsNullOrEmpty(value))
                            {
                                canBo.IsBanChapHanh= true;
                            }
                            break;
                        case 42:
                            // ngày nâng bậc (*)
                            canBo.GhiChu = value;
                            break;
                    }
                }
                catch (Exception)
                {
                    string ss = value;
                    throw;
                }
                data.CanBo = canBo;
            }
            data.daoTaoBoiDuongs = daoTaoBoiDuongs;
            return data;
        }

        #endregion Check data type 
        #region XuatFileWord
        public  string XuatWordCongTac(string fileName, string fileNameSave, System.Data.DataTable dtInt)
        {
            try
            {
                System.IO.File.Copy(fileName, fileNameSave, true);

                object missing = System.Reflection.Missing.Value;
                object oFalse = false;
                object oTrue = true;
                object replaceAll = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;
                object oWrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;

                Microsoft.Office.Interop.Word.Application wApp = new Microsoft.Office.Interop.Word.Application();

                Microsoft.Office.Interop.Word.Document word = new Microsoft.Office.Interop.Word.Document();
                try
                {
                    object filename = fileNameSave;
                    word = wApp.Documents.Open(ref filename, ref
                        missing, ref oFalse, ref missing,
                        ref missing, ref missing, ref missing, ref missing, ref
                        missing, ref missing, ref missing, ref oTrue);//,

                    wApp.Visible = true;
                    Microsoft.Office.Interop.Word.Selection selection = wApp.Selection;

                    object MatchWholeWord = true;
                    object findText = "";
                    object oReplace = "";
                    foreach (DataColumn dc in dtInt.Columns)
                    {
                        findText = "rep" + dc.ColumnName;
                        string sValue = "";
                        wApp.Selection.Find.Replacement.ClearFormatting(); // Error: System.StackOverflowException
                        wApp.Selection.Find.MatchWholeWord = true;
                            sValue = dtInt.Rows[0]["" + dc.ColumnName.ToString() + ""].ToString();
                        oReplace = sValue;

                        selection.Find.Execute(ref findText, ref oFalse, ref MatchWholeWord, ref oFalse,
                            ref oFalse, ref oFalse, ref oTrue, ref oWrap, ref oFalse, ref oReplace,
                            ref replaceAll, ref oFalse, ref oFalse, ref oFalse, ref oFalse);
                    }
                    word.Save();
                }
                catch (Exception ex)
                {

                    throw;
                }
                return fileNameSave;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string XuatWordProileV1(string fileName, string fileNameSave, System.Data.DataTable dtInt,Guid IdCanBo)
        {
            try
            {
                System.IO.File.Copy(fileName, fileNameSave, true);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileNameSave, true))
                {
                    var body = wordDoc.MainDocumentPart!.Document.Body;
                    var paragraphs = body.Elements<Paragraph>();

                   
                    string findText = ""; string sValue = "";
                    foreach (DataColumn dc in dtInt.Columns)
                    {
                        sValue = findText = "";
                        findText = dc.ColumnName;
                        var texts = body.Descendants<Text>().FirstOrDefault(e => e.Text.Contains(findText));
                        if (texts != null) {
                            sValue = dtInt.Rows[0]["" + dc.ColumnName.ToString() + ""].ToString();
                            texts.Text = texts.Text.Replace(findText, sValue);
                        }
                       
                    }
                    // đào tạo
                    var tbDaoTao = body.Elements<Table>().Where(it=>it.InnerText.Contains("DaoTaoBoiDuong")).First();
                    if(tbDaoTao != null)
                    {
                        var cell = tbDaoTao.Elements<TableRow>().ToList().First().Elements<TableCell>().ToList().First();
                        cell.RemoveAllChildren();
                        cell.Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Tên trường"))));
                        var listDaoTao = _context.DaoTaoBoiDuongs.Include(it=>it.HinhThucDaoTao).Include(it=>it.LoaiBangCap).Where(it => it.IDCanBo == IdCanBo).ToList();
                        if (listDaoTao.Count > 0)
                        {

                            foreach (var item in listDaoTao)
                            {
                                string tungay = "";
                                if (item.TuNgay != null)
                                {
                                    tungay = item.TuNgay.Value.ToString("MM/yyyy");
                                }
                                if (item.DenNgay != null)
                                {
                                    tungay = tungay + "-" + item.DenNgay.Value.ToString("MM/yyyy");
                                }

                                DocumentFormat.OpenXml.Wordprocessing.TableRow tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(item.TenTruong))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(item.NoiDungDaoTao))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(tungay))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService4 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(item.HinhThucDaoTao.TenHinhThucDaoTao))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService5 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(item.LoaiBangCap.TenLoaiBangCap))));
                                tr.Append(tablecellService1, tablecellService2, tablecellService3, tablecellService4, tablecellService5);
                                tbDaoTao.Last().InsertAfterSelf(tr);
                            }
                        }
                        else
                        {
                            for (global::System.Int32 i = 0; i < 5; i++)
                            {
                                DocumentFormat.OpenXml.Wordprocessing.TableRow tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService4 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService5 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                tr.Append(tablecellService1, tablecellService2, tablecellService3, tablecellService4, tablecellService5);
                                tbDaoTao.Last().InsertAfterSelf(tr);
                            }
                        }
                    }

                    // Công Tác
                    var tbcongTac = body.Elements<Table>().Where(it => it.InnerText.Contains("CongTac")).First();
                    if (tbcongTac != null)
                    {
                        var cell = tbcongTac.Elements<TableRow>().ToList().First().Elements<TableCell>().ToList().First();
                        cell.RemoveAllChildren();
                        cell.Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Từ tháng, năm đến tháng, năm"))));
                        var listCongTac = _context.QuaTrinhCongTacs.Include(it => it.ChucVu).Where(it => it.IDCanBo == IdCanBo).ToList();
                        if (listCongTac.Count > 0)
                        {
                           
                            foreach (var item in listCongTac)
                            {
                                string tungay = "";
                                if (item.TuNgay != null)
                                {
                                    tungay = item.TuNgay.Value.ToString("MM/yyyy");
                                }
                                if (item.DenNgay != null)
                                {
                                    tungay = tungay + "-" + item.DenNgay.Value.ToString("MM/yyyy");
                                }

                                DocumentFormat.OpenXml.Wordprocessing.TableRow tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(tungay))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(item.ChucVu.TenChucVu))));
                                tr.Append(tablecellService1, tablecellService2);
                                tbcongTac.Last().InsertAfterSelf(tr);
                            }
                        }
                        else
                        {
                            for (global::System.Int32 i = 0; i < 5; i++)
                            {
                                DocumentFormat.OpenXml.Wordprocessing.TableRow tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                //DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                //DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService4 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                //DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService5 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                tr.Append(tablecellService1, tablecellService2);
                                tbcongTac.Last().InsertAfterSelf(tr);
                            }
                        }
                    }


                    // Bản thân
                    var tbBanThan = body.Elements<Table>().Where(it => it.InnerText.Contains("QuanHeBanThan")).First();
                    if (tbBanThan != null)
                    {
                        var cell = tbBanThan.Elements<TableRow>().ToList().First().Elements<TableCell>().ToList().First();
                        cell.RemoveAllChildren();
                        cell.Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Mối quan hệ"))));
                        var listBanThan = _context.QuanHeGiaDinhs.Include(it => it.LoaiQuanhe).Where(it => it.IDCanBo == IdCanBo && it.LoaiQuanhe.Loai =="1").ToList();
                        if (listBanThan.Count > 0)
                        {
                            foreach (var item in listBanThan)
                            { 
                                string noiDung = "";
                                if (!String.IsNullOrWhiteSpace(item.NgheNghiep))
                                {
                                    noiDung += item.NgheNghiep + "-";
                                }
                                if (!String.IsNullOrWhiteSpace(item.NoiLamViec))
                                {
                                    noiDung +=  item.NoiLamViec + "-";
                                }
                                if (!String.IsNullOrWhiteSpace(item.DiaChi))
                                {
                                    noiDung += item.DiaChi + "-";
                                }
                                if (!String.IsNullOrWhiteSpace(item.GhiChu))
                                {
                                    noiDung += item.GhiChu + "-";
                                }
                                if (noiDung.Length > 0)
                                {
                                    noiDung = noiDung.Substring(0, noiDung.Length - 1);
                                }
                               
                                DocumentFormat.OpenXml.Wordprocessing.TableRow tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(item.LoaiQuanhe.TenLoaiQuanHeGiaDinh))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(item.HoTen))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(item.NgaySinh))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService4 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(noiDung))));
                                tr.Append(tablecellService1, tablecellService2, tablecellService3, tablecellService4);
                                tbBanThan.Last().InsertAfterSelf(tr);
                            }
                        }
                        else
                        {
                            for (global::System.Int32 i = 0; i < 5; i++)
                            {
                                DocumentFormat.OpenXml.Wordprocessing.TableRow tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService4 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                //DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService5 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                tr.Append(tablecellService1, tablecellService2, tablecellService3, tablecellService4);
                                tbBanThan.Last().InsertAfterSelf(tr);
                            }
                        }
                    }

                    // quan hệ khác
                    var tbQuanHeKhac = body.Elements<Table>().Where(it => it.InnerText.Contains("QuanHeKhac")).First();
                    if (tbQuanHeKhac != null)
                    {
                        var cell = tbQuanHeKhac.Elements<TableRow>().ToList().First().Elements<TableCell>().ToList().First();
                        cell.RemoveAllChildren();
                        cell.Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Mối quan hệ"))));
                        var listBanThanKhac = _context.QuanHeGiaDinhs.Include(it => it.LoaiQuanhe).Where(it => it.IDCanBo == IdCanBo && it.LoaiQuanhe.Loai != "1").ToList();
                        if (listBanThanKhac.Count > 0)
                        {
                            foreach (var item in listBanThanKhac)
                            { 
                                string noiDung = "";
                                if (!String.IsNullOrWhiteSpace(item.NgheNghiep))
                                {
                                    noiDung += item.NgheNghiep + "-";
                                }
                                if (!String.IsNullOrWhiteSpace(item.NoiLamViec))
                                {
                                    noiDung += item.NoiLamViec + "-";
                                }
                                if (!String.IsNullOrWhiteSpace(item.DiaChi))
                                {
                                    noiDung += item.DiaChi + "-";
                                }
                                if (!String.IsNullOrWhiteSpace(item.GhiChu))
                                {
                                    noiDung += item.GhiChu + "-";
                                }
                                if (noiDung.Length > 0)
                                {
                                    noiDung = noiDung.Substring(0, noiDung.Length - 1);
                                }
                                DocumentFormat.OpenXml.Wordprocessing.TableRow tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(item.LoaiQuanhe.TenLoaiQuanHeGiaDinh))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(item.HoTen))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(item.NgaySinh))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService4 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(noiDung))));
                                tr.Append(tablecellService1, tablecellService2, tablecellService3, tablecellService4);
                                tbQuanHeKhac.Last().InsertAfterSelf(tr);
                            }
                        }
                        else
                        {
                            for (global::System.Int32 i = 0; i < 5; i++)
                            {
                                DocumentFormat.OpenXml.Wordprocessing.TableRow tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService4 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                //DocumentFormat.OpenXml.Wordprocessing.TableCell tablecellService5 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                                tr.Append(tablecellService1, tablecellService2, tablecellService3, tablecellService4);
                                tbQuanHeKhac.Last().InsertAfterSelf(tr);
                            }
                        }
                        var tbQuaTrinhLuong = body.Elements<Table>().Where(it => it.InnerText.Contains("Tháng/năm")).First();
                        if (tbQuaTrinhLuong != null)
                        {
                            var quaTrinhLuong = _context.CanBoQuaTrinhLuongs.Where(it => it.IDCanBo == IdCanBo).OrderBy(it => it.HeSoLuong).Include(it => it.BacLuong).ToList();

                            var rowThangNam = tbQuaTrinhLuong.Elements<TableRow>().ToList().First();
                            var rowNgach = tbQuaTrinhLuong.Elements<TableRow>().ToList()[1];
                            var rowHeSo = tbQuaTrinhLuong.Elements<TableRow>().ToList().Last();

                            int i = 1;
                            var data = rowThangNam.Elements<TableCell>().ToList();
                            foreach (var item in data) {
                                if (item == data.First())
                                    continue;
                                if (i > quaTrinhLuong.Count())
                                    break;
                                item.Append(new Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new Text(quaTrinhLuong[i - 1]!.NgayHuong!.Value.ToString("MM/yyyy").Trim()))));
                                i++;
                            }
                            i = 1;
                            data = rowNgach.Elements<TableCell>().ToList();
                            foreach (var item in data)
                            {
                                if (item == data.First())
                                    continue;
                                if (i > quaTrinhLuong.Count())
                                    break;
                                item.Append(new Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new Text(quaTrinhLuong[i - 1].MaNgachLuong.Trim() + "/" + quaTrinhLuong[i - 1].BacLuong.OrderIndex.ToString().Trim()))));
                                i++;
                            }           
                            i = 1;
                            data = rowHeSo.Elements<TableCell>().ToList();
                            foreach (var item in data)
                            {
                                if (item == data.First())
                                    continue;
                                if (i > quaTrinhLuong.Count())
                                    break;
                                item.Append(new Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new Text(quaTrinhLuong[i - 1]!.HeSoLuong.Value.ToString().Trim()))));
                                i++;

                            }
                        }
                    }
                    // Quá trình lương
                }
                return fileNameSave;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private TableCell CreateTableCell(string value) {
            TableCell tableCell = new TableCell();
            Paragraph paragraph = new Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run( new Text(value)));
            TableCellProperties tableCellProperties = new TableCellProperties(new TableWidth() {
                Type = TableWidthUnitValues.Dxa,
                Width = "250"});
            tableCell.Append(tableCellProperties);
            tableCell.Append(paragraph);
            return tableCell;
        }
        #endregion XuatFileWord
    }

}
