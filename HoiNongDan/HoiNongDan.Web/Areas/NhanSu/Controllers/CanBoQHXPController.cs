using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Migrations;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using System.Transactions;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class CanBoQHXPController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        const string controllerCode = ConstExcelController.CanBo;
        const int startIndex = 6;
        public CanBoQHXPController (AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context) {
            _hostEnvironment = hostEnvironment;
            DateFomat = config.GetSection("SiteSettings:DateFormat").Value.ToString().Split(',');
        }
        #region index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(Guid? MaDonVi,string? Loai, String? HoVaTen, Guid? MaChucVu)
        {
            return ExecuteSearch(() => {

                var moel = _context.CanBos.Include(it=>it.TrinhDoChinhTri)
                .Include(it=>it.TrinhDoTinHoc)
                .Include(it=>it.TrinhDoNgoaiNgu)
                .Include(it=>it.Department)
                .Include(it=>it.ChucVu)
                .Include(it=>it.DanToc)
                .Include(it=>it.TonGiao)
                .Include(it=>it.DaoTaoBoiDuongs)
                .Where(it => (it.Level == "20" || it.Level == "30") && it.IsCanBo == true).AsQueryable();
                if (MaDonVi !=null) {
                    moel = moel.Where(it => it.IdDepartment == MaDonVi);
                }
                if (MaChucVu != null)
                {
                    moel = moel.Where(it => it.MaChucVu == MaChucVu);
                }
                if (Loai != null && Loai !="")
                {
                    moel = moel.Where(it => it.Level == Loai);
                }
                if (HoVaTen != null && HoVaTen != "")
                {
                    moel = moel.Where(it => it.HoVaTen.Contains(HoVaTen));
                }
                var data = moel.Select(it => new CanBoQHXPDetailVM {
                    IDCanBo = it.IDCanBo,
                    MaCanBo = it.MaCanBo,
                    HoVaTen = it.HoVaTen,
                    NgaySinh = it.NgaySinh,
                    GioiTinh = it.GioiTinh== 0?false:true ,
                    TenDonVi = it.Department!.Name,
                    TenChucVu = it.ChucVu!.TenChucVu,
                    TenDanToc = it.DanToc!.TenDanToc,
                    TenTonGiao = it.TonGiao!.TenTonGiao,
                    NoiSinh = it.NoiSinh,
                    ChoOHienNay = it.ChoOHienNay,
                    NgayvaoDangDuBi = it.NgayvaoDangDuBi,
                    NgayVaoDangChinhThuc = it.NgayVaoDangChinhThuc,
                    ChuyenNganh= it.ChuyenNganh,
                    MaTrinhDoChinhTri = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                    TenTrinhDoNgoaiNgu = it.TrinhDoNgoaiNgu!.TenTrinhDoNgoaiNgu,
                    TenTrinhDoTinHoc = it.TrinhDoTinHoc!.TenTrinhDoTinHoc,
                    NgayThamGiaCongTac = it.NgayThamGiaCongTac,
                    ThamGiaBanChapHanh = it.IsBanChapHanh == true?"X":"",
                    ThamGiaBTV = it.ThamGiaBTV == true ? "X" : "",
                    UBKT = it.UBKT == true ? "X" : "",
                    HuyenUyVien = it.HuyenUyVien == true ? "X" : "",
                    DangUyVien = it.DangUyVien == true ? "X" : "",
                    HDNNCapHuyen = it.HDNNCapHuyen == true ? "X" : "",
                    HDNNCapXa = it.HDNNCapXa == true ? "X" : "",
                    NVCTTW = (it.DaoTaoBoiDuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaHinhThucDaoTao == "03").Count())>0?"X":"",
                    GiangVienKiemChuc = (it.DaoTaoBoiDuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaHinhThucDaoTao == "04").Count()) > 0 ? "X" : "",
                    DanhGiaCBCC = it.DanhGiaCBCC,
                    DanhGiaDangVien = it.DanhGiaDangVien,
                    Cap = it.Level=="20"? "HUYỆN QUẬN": "XÃ PHƯỜNG, THỊ TRẤN",
                    GhiChu   = it.GhiChu,
                    DonVi = it.DonVi
                }).ToList();
                return PartialView(data);
            });
        }
        #endregion Index
        #region Create
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Create() {
            CanBoQHXPVM model = new CanBoQHXPVM();
            UpdateViewBag();
            return View(model);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CanBoQHXPMTVM obj, IFormFile? avtFileInbox) {
            return ExecuteContainer(() => {
                CanBo add = new CanBo();
                add = obj.getCanBo(add);
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                add.IDCanBo = Guid.NewGuid();
                if (avtFileInbox != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = add.MaCanBo!;
                    var uploads = Path.Combine(wwwRootPath, @"images\canbo");

                    bool folderExists = System.IO.Directory.Exists(uploads);
                    if (!folderExists)
                        System.IO.Directory.CreateDirectory(uploads);

                    var extension = Path.GetExtension(avtFileInbox.FileName);
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
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.CanBoHNDTP.ToLower())
                });
            });
        }
        #endregion Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
            var item = _context.CanBos.SingleOrDefault(it => it.IDCanBo == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            CanBoQHXPMTVM method = new CanBoQHXPMTVM();
            CanBoQHXPVM canbo = method.SetCanBo(item);
            UpdateViewBag(item.IdDepartment, item.MaChucVu,item.MaDanToc,item.MaTonGiao, item.MaTrinhDoChuyenMon, item.MaTrinhDoChinhTri, item.MaTrinhDoNgoaiNgu, item.MaTrinhDoTinHoc,item.Level);
            return View(canbo);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CanBoQHXPMTVM obj , IFormFile? avtFileInbox) {
            return ExecuteContainer(() => {
                var edit = _context.CanBos.SingleOrDefault(it => it.IDCanBo == obj.IDCanBo);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.CanBoHNDTP.ToLower())
                    });
                }
                edit = obj.getCanBo(edit);
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
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.CanBoHNDTP.ToLower())
                });
            });
        }
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
        public IActionResult _Import()
        {
            return PartialView();
        }
        [HoiNongDanAuthorization]
        public IActionResult Import(String Loai)
        {
            DataSet ds = GetDataSetFromExcel();
            List<string> errorList = new List<string>();
            return ExcuteImportExcel(() => {
                if (ds.Tables.Count > 0)
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
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
                                                data.CanBo.Level = Loai;
                                                string result = ExecuteImportExcelCanBo(data);
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
                                //string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.CanBo);
                                //errorList.Add(error);
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
        [NonAction]
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
                return null;
            }
        }
        #endregion Import Excel
        #region Export
        [HoiNongDanAuthorization]
        public IActionResult ExportCreate()
        {
            List<CanBoQHXPExcelVM> data = new List<CanBoQHXPExcelVM>();
            return Export(data);
        }
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit(Guid? MaDonVi, string? Loai,String? HoVaTen)
        {
            var moel = _context.CanBos.Include(it => it.TrinhDoChinhTri)
                .Include(it => it.TrinhDoTinHoc)
                .Include(it => it.TrinhDoNgoaiNgu)
                .Include(it => it.Department)
                .Include(it => it.ChucVu)
                .Include(it => it.DanToc)
                .Include(it => it.TonGiao)
                .Include(it => it.DaoTaoBoiDuongs)
                .Where(it => (it.Level == "20" || it.Level == "300") && it.IsCanBo == true).AsQueryable();
            if (MaDonVi != null)
            {
                moel = moel.Where(it => it.IdDepartment == MaDonVi);
            }
            if (Loai != null)
            {
                moel = moel.Where(it => it.Level == Loai);
            }
            if (HoVaTen != null)
            {
                moel = moel.Where(it => it.HoVaTen.Contains(HoVaTen));
            }
            var data = moel.Select(it => new CanBoQHXPExcelVM
            {
                IDCanBo = it.IDCanBo,
                MaCanBo = it.MaCanBo,
                HoVaTen = it.HoVaTen,
                NgaySinh = it.NgaySinh,
                GioiTinh = it.GioiTinh == 0 ? false : true,
                TenDonVi = it.Department!.Name,
                TenChucVu = it.ChucVu!.TenChucVu,
                TenDanToc = it.DanToc!.TenDanToc,
                TenTonGiao = it.TonGiao!.TenTonGiao,
                NoiSinh = it.NoiSinh,
                ChoOHienNay = it.ChoOHienNay,
                NgayvaoDangDuBi = it.NgayvaoDangDuBi,
                NgayVaoDangChinhThuc = it.NgayVaoDangChinhThuc,
                ChuyenNganh = it.ChuyenNganh!,
                MaTrinhDoChinhTri = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                TenTrinhDoNgoaiNgu = it.TrinhDoNgoaiNgu!.TenTrinhDoNgoaiNgu,
                TenTrinhDoTinHoc = it.TrinhDoTinHoc!.TenTrinhDoTinHoc,
                NgayThamGiaCongTac = it.NgayThamGiaCongTac,
                ThamGiaBanChapHanh = it.IsBanChapHanh == true ? "X" : "",
                ThamGiaBTV = it.ThamGiaBTV == true ? "X" : "",
                UBKT = it.UBKT == true ? "X" : "",
                HuyenUyVien = it.HuyenUyVien == true ? "X" : "",
                DangUyVien = it.DangUyVien == true ? "X" : "",
                HDNNCapHuyen = it.HDNNCapHuyen == true ? "X" : "",
                HDNNCapXa = it.HDNNCapXa == true ? "X" : "",
                NVCTTW = (it.DaoTaoBoiDuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaHinhThucDaoTao == "03").Count()) > 0 ? "X" : "",
                GiangVienKiemChuc = (it.DaoTaoBoiDuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaHinhThucDaoTao == "04").Count()) > 0 ? "X" : "",
                DanhGiaCBCC = it.DanhGiaCBCC,
                DanhGiaDangVien = it.DanhGiaDangVien,
                GhiChu = it.GhiChu
            }).ToList();
            return Export(data);
        }
        [HoiNongDanAuthorization]
        public FileContentResult Export(List<CanBoQHXPExcelVM> menu)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "IDCanBo", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "MaCanBo", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinh", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GioiTinh", isBoolean = true, isComment = true, strComment = "Nam để chữ X" });


            var donVi = _context.Departments.ToList().Select(x => new DropdownModel { Id = x.Id, Name = x.Name }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenDonVi", isAllowedToEdit = true, isDropdownlist = true, DropdownData = donVi, TypeId = ConstExcelController.GuidId });


            var chuVu = _context.ChucVus.ToList().Select(x => new DropdownModel { Id = x.MaChucVu, Name = x.TenChucVu }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenChucVu", isAllowedToEdit = true, isDropdownlist = true, DropdownData = chuVu, TypeId = ConstExcelController.GuidId });

            var danToc = _context.DanTocs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaDanToc, Name = x.TenDanToc }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenDanToc", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = danToc, TypeId = ConstExcelController.StringId });

            var tonGiao = _context.TonGiaos.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTonGiao, Name = x.TenTonGiao }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTonGiao", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tonGiao, TypeId = ConstExcelController.StringId });

            columns.Add(new ExcelTemplate() { ColumnName = "NoiSinh", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay", isAllowedToEdit = true, isText = true });

            columns.Add(new ExcelTemplate() { ColumnName = "NgayvaoDangDuBi", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoDangChinhThuc", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChuyenNganh", isAllowedToEdit = true, isText = true });
            var chinhTri = _context.TrinhDoChinhTris.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChinhTri, Name = x.TenTrinhDoChinhTri }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChinhTri", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chinhTri, TypeId = ConstExcelController.StringId });
            var ngoaiNgu = _context.TrinhDoNgoaiNgus.ToList().Select(x => new DropdownModel { Id = x.MaTrinhDoNgoaiNgu, Name = x.TenTrinhDoNgoaiNgu }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTrinhDoNgoaiNgu", isAllowedToEdit = true, isDropdownlist = true, DropdownData = ngoaiNgu, TypeId = ConstExcelController.GuidId });
            var tinHoc = _context.TrinhDoTinHocs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoTinHoc, Name = x.TenTrinhDoTinHoc }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTrinhDoTinHoc", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tinHoc, TypeId = ConstExcelController.StringId });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayThamGiaCongTac", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ThamGiaBanChapHanh", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ThamGiaBTV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "UBKT", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HuyenUyVien", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DangUyVien", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HDNNCapHuyen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HDNNCapXa", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NVCTTW", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GiangVienKiemChuc", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DanhGiaCBCC", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DanhGiaDangVien", isAllowedToEdit = true, isText = true });

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

            //Header
            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(menu, columns, heading, true);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }

        #endregion Export
        #region Insert/Update data from excel file
        [NonAction]
        private string ExecuteImportExcelCanBo(CanBoQHXPImportExcelVM canBoExcel)
        {
            //Check:
            //1. If MenuId == "" then => Insert
            //2. Else then => Update
            if (canBoExcel.isNullValueId == true)
            {

                if (canBoExcel.daoTaoBoiDuongs.Count() > 0)
                {
                    _context.DaoTaoBoiDuongs.AddRange(canBoExcel.daoTaoBoiDuongs);
                }
                _context.Entry(canBoExcel.CanBo).State = EntityState.Added;
            }
            else
            {
                CanBo canBo = _context.CanBos.Where(p => p.IDCanBo == canBoExcel.CanBo.IDCanBo).FirstOrDefault();
                if (canBo != null)
                {
                    canBo = EditCanBo(canBo, canBoExcel.CanBo);
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
        [NonAction]
        private CanBo EditCanBo(CanBo old, CanBo news){
            old.MaCanBo = news.MaCanBo;
            old.HoVaTen = news.HoVaTen;
            old.NgaySinh = news.NgaySinh;
            old.GioiTinh = news.GioiTinh;
            old.IdDepartment = news.IdDepartment;
            old.MaChucVu = news.MaChucVu;
            old.MaDanToc = news.MaDanToc;
            old.MaTonGiao = news.MaTonGiao;
            old.NoiSinh = news.NoiSinh;
            old.ChoOHienNay = news.ChoOHienNay;
            old.NgayvaoDangDuBi = news.NgayvaoDangDuBi;
            old.NgayVaoDangChinhThuc = news.NgayVaoDangChinhThuc;
            old.ChuyenNganh = news.ChuyenNganh;
            old.MaTrinhDoChinhTri = news.MaTrinhDoChinhTri;
            old.MaTrinhDoNgoaiNgu = news.MaTrinhDoNgoaiNgu;
            old.MaTrinhDoTinHoc = news.MaTrinhDoTinHoc;
            old.IsBanChapHanh = news.IsBanChapHanh;
            old.ThamGiaBTV = news.ThamGiaBTV;
            old.UBKT = news.UBKT;
            old.HuyenUyVien = news.HuyenUyVien;
            old.DangUyVien = news.DangUyVien;
            old.HDNNCapHuyen = news.HDNNCapHuyen;
            old.HDNNCapXa = news.HDNNCapXa;
            old.DanhGiaCBCC = news.DanhGiaCBCC;
            old.DanhGiaDangVien = news.DanhGiaDangVien;
            old.GhiChu = news.GhiChu;

            return old;
        }
        #region Check data type 

        private CanBoQHXPImportExcelVM CheckTemplate(object[] row)
        {
            CanBoQHXPImportExcelVM data = new CanBoQHXPImportExcelVM();
            CanBo canBo = new CanBo();
            canBo.MaTinhTrang = "01";
            canBo.IsCanBo = true;
            canBo.Actived = true;
            canBo.IDCanBo = Guid.NewGuid();
            List<DaoTaoBoiDuong> daoTaoBoiDuongs = new List<DaoTaoBoiDuong>();
            int index = 0;
            string value = "";
            for (int i = 0; i < row.Length; i++)
            {
                value = "";
                value = row[i].ToString().Trim();
                //if (value == "")
                //    continue;
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
                            // Mã nhân viên
                            if (string.IsNullOrEmpty(value))
                            {
                                //data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.MaCanBo), index);
                            }
                            else
                            {
                                canBo.MaCanBo = value;
                            }
                            break;
                        case 3:
                            // Họ và tên
                            if (string.IsNullOrEmpty(value))
                            {
                                data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.FullName), index);
                            }
                            else
                            {
                                canBo.HoVaTen = value;
                            }
                            break;
                        case 4:
                            //  Ngày sinh (*)
                            if (string.IsNullOrEmpty(value) || value == "")
                            {
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0}", LanguageResource.NgaySinh), index);
                            }
                            else
                            {
                                try
                                {
                                    // data.NgaySinh = DateTime.ParseExact(ngaySinh,DateFomat, new CultureInfo("en-US"));
                                    canBo.NgaySinh = value;
                                }
                                catch (Exception)
                                {

                                    data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgaySinh, value, index);
                                }

                            }
                            break;
                        case 5:
                            // Giới tính
                            if (string.IsNullOrEmpty(value))
                            {
                                canBo.GioiTinh = GioiTinh.Nữ;
                            }
                            else
                            {
                                canBo.GioiTinh = GioiTinh.Nam;
                            }
                            break;
                        case 6:
                            //  Ban, Văn Phòng, Trung Tâm (*)
                            if (string.IsNullOrEmpty(value))
                            {
                                data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0}", LanguageResource.Department), index);
                            }
                            else
                            {
                                var donVi = _context.Departments.FirstOrDefault(it => it.Name == value);
                                if (donVi != null)
                                {
                                    canBo.IdDepartment = donVi.Id;
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy đơn vị tên {0} ở dòng số {1} !", value, index);
                                }

                            }
                            break;
                        case 7:
                            //  chức vụ (*)
                            if (string.IsNullOrEmpty(value))
                            {
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.ChucVu), index);
                            }
                            else
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
                            //  dân tộc (*)
                            if (string.IsNullOrEmpty(value))
                            {
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.DanToc), index);
                            }
                            else
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
                        case 9:
                            //  tôn giáo (*)
                            if (string.IsNullOrEmpty(value))
                            {
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.TonGiao), index);
                            }
                            else
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
                        case 10:
                            // Nơi sinh
                            if (string.IsNullOrEmpty(value))
                            {
                                //data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.NoiSinh), index);
                            }
                            else
                            {
                                canBo.NoiSinh = value;
                            }
                            break;
                        case 11:
                            // Nơi sinh
                            if (string.IsNullOrEmpty(value))
                            {
                                //data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.ChoOHienNay), index);
                            }
                            else
                            {
                                canBo.ChoOHienNay = value;
                            }
                            break;

                        case 12:
                            // Ngày vào đảng dự bị
                            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                try
                                {
                                    //canBo.NgayvaoDangDuBi = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US")); ;
                                    canBo.NgayvaoDangDuBi = value;//DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US")); ;
                                }
                                catch (Exception)
                                {

                                    data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayvaoDangDuBi, value, index);
                                }
                            }
                            break;
                        case 13:
                            // Ngày vào đảng chính thức
                            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                try
                                {
                                    //canBo.NgayVaoDangChinhThuc = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US"));
                                    canBo.NgayVaoDangChinhThuc = value;// DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US"));
                                    canBo.DangVien = true;
                                }
                                catch (Exception)
                                {

                                    data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayVaoDangChinhThuc, value, index);
                                }
                            }
                            break;
                        case 14:
                            // Chuyên ngành
                            if (string.IsNullOrEmpty(value))
                            {
                                //data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.ChuyenNganh), index);
                            }
                            else
                            {
                                canBo.ChuyenNganh = value;
                            }
                            break;
                        case 15:
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
                       
                        case 16:
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
                        case 17:
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
                        case 18:
                            //  TenTrinhDoTinHoc (*)
                            canBo.NgayThamGiaCongTac = value;
                            break;
                        case 19:
                            //  Tham gia BCH (*)
                            if (!String.IsNullOrWhiteSpace(value)) {
                                canBo.IsBanChapHanh = true;
                            }
                            break;
                        case 20:
                            //  TenTrinhDoTinHoc (*)
                            if (!String.IsNullOrWhiteSpace(value))
                                canBo.ThamGiaBTV = true;
                            break;
                         case 21:
                            //  TenTrinhDoTinHoc (*)
                            if (!String.IsNullOrWhiteSpace(value))
                                canBo.UBKT = true;
                            break;
                        case 22:
                            //  TenTrinhDoTinHoc (*)
                            if (!String.IsNullOrWhiteSpace(value))
                                canBo.HuyenUyVien = true;
                            break;
                        case 23:
                            //  TenTrinhDoTinHoc (*)
                            if (!String.IsNullOrWhiteSpace(value))
                                canBo.DangUyVien = true;
                            break;
                        case 24:
                            //  TenTrinhDoTinHoc (*)
                            if (!String.IsNullOrWhiteSpace(value))
                                canBo.HDNNCapHuyen = true;
                            break;
                        case 25:
                            //  TenTrinhDoTinHoc (*)
                            canBo.HDNNCapXa = true;
                            break;
                        case 26:
                            //  ngach luong(*)
                            if (string.IsNullOrEmpty(value))
                            {
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.NgayNangBacLuong), index);
                            }
                            else
                            {
                                daoTaoBoiDuongs.Add(new DaoTaoBoiDuong
                                {
                                    Id = Guid.NewGuid(),
                                    IDCanBo = canBo.IDCanBo,
                                    MaHinhThucDaoTao = "03",
                                    MaLoaiBangCap = "11",
                                    Actived = true,
                                    NoiDungDaoTao = value,
                                    GhiChu = value
                                }); ; ;

                            }
                            break;
                        case 27:
                            //  bac luong (*)
                            if (string.IsNullOrEmpty(value))
                            {
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.NgayNangBacLuong), index);
                            }
                            else
                            {
                                daoTaoBoiDuongs.Add(new DaoTaoBoiDuong
                                {
                                    Id = Guid.NewGuid(),
                                    IDCanBo = canBo.IDCanBo,
                                    MaHinhThucDaoTao = "04",
                                    MaLoaiBangCap = "11",
                                    Actived = true,
                                    NoiDungDaoTao = value,
                                    GhiChu = value
                                }); ; ;

                            }
                            break;
                        case 28:
                            //  TenTrinhDoTinHoc (*)
                            canBo.DanhGiaCBCC = value;
                            break;
                        case 29:
                            //  TenTrinhDoTinHoc (*)
                            canBo.DanhGiaDangVien = value;
                            break;
                        
                        case 30:
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
                data.daoTaoBoiDuongs = daoTaoBoiDuongs;
            }

            return data;
        }
        #endregion Check data type 
        #region Helper
        private void CreateViewBag() {
            var DonVi = (from dv in _context.Departments join
                        nv in _context.CanBos on dv.Id equals nv.IdDepartment
                        where nv.Level =="20" || nv.Level =="30"
                        select new { IdDepartment  = dv.Id,Name = dv.Name}).Distinct().ToList();
            ViewBag.MaDonVi = new SelectList(DonVi, "IdDepartment", "Name", DonVi);
            var ChucVu = (from dv in _context.ChucVus
                         join
                      nv in _context.CanBos on dv.MaChucVu equals nv.MaChucVu
                         where nv.Level == "20" || nv.Level == "30"
                         select new { MaChucVu = dv.MaChucVu, Name = dv.TenChucVu }).Distinct().ToList();
            ViewBag.MaChucVu = new SelectList(ChucVu, "MaChucVu", "Name");

        }
        private void UpdateViewBag(Guid? idDepartment = null, Guid? maChucVu = null, String? maDanToc = null, String? maTonGiao = null, String? maTrinhDoChuyenMon = null, String? maTrinhDoChinhTri = null,
            Guid? maTrinhDoNgoaiNgu = null, String? maTrinhDoTinHoc = null, String? Level = null)
        {
            var DonVi = _context.Departments.Where(it => it.Actived == true).Include(it => it.CoSo).OrderBy(p => p.OrderIndex).Select(it => new { IdDepartment = it.Id, Name = it.Name + " " + it.CoSo.TenCoSo }).ToList();
            ViewBag.IdDepartment = new SelectList(DonVi, "IdDepartment", "Name", idDepartment);

            var chucVu = _context.ChucVus.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { MaChucVu = it.MaChucVu, TenChucVu = it.TenChucVu }).ToList();
            ViewBag.MaChucVu = new SelectList(chucVu, "MaChucVu", "TenChucVu", maChucVu);

            var danToc = _context.DanTocs.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaDanToc = it.MaDanToc, TenDanToc = it.TenDanToc }).ToList();
            ViewBag.MaDanToc = new SelectList(danToc, "MaDanToc", "TenDanToc", maDanToc);

            var tonGiao = _context.TonGiaos.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTonGiao = it.MaTonGiao, TenTonGiao = it.TenTonGiao }).ToList();
            ViewBag.MaTonGiao = new SelectList(tonGiao, "MaTonGiao", "TenTonGiao", maTonGiao);
            var trinhDoChuyenMon = _context.TrinhDoChuyenMons.Select(it => new { MaTrinhDoChuyenMon = it.MaTrinhDoChuyenMon, TenTrinhDoChuyenMon = it.TenTrinhDoChuyenMon }).ToList();
            ViewBag.MaTrinhDoChuyenMon = new SelectList(trinhDoChuyenMon, "MaTrinhDoChuyenMon", "TenTrinhDoChuyenMon", maTrinhDoChuyenMon);

            var trinhDoChinhTri = _context.TrinhDoChinhTris.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTrinhDoChinhTri = it.MaTrinhDoChinhTri, TenTrinhDoChinhTri = it.TenTrinhDoChinhTri }).ToList();
            ViewBag.MaTrinhDoChinhTri = new SelectList(trinhDoChinhTri, "MaTrinhDoChinhTri", "TenTrinhDoChinhTri", maTrinhDoChinhTri);

            var trinhDoNgoaiNgu = _context.TrinhDoNgoaiNgus.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTrinhDoNgoaiNgu = it.MaTrinhDoNgoaiNgu, TenTrinhDoNgoaiNgu = it.TenTrinhDoNgoaiNgu }).ToList();
            ViewBag.MaTrinhDoNgoaiNgu = new SelectList(trinhDoNgoaiNgu, "MaTrinhDoNgoaiNgu", "TenTrinhDoNgoaiNgu", maTrinhDoNgoaiNgu);

            var trinhDoTinHoc = _context.TrinhDoTinHocs.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTrinhDoTinHoc = it.MaTrinhDoTinHoc, TenTrinhDoTinHoc = it.TenTrinhDoTinHoc }).ToList();
            ViewBag.MaTrinhDoTinHoc = new SelectList(trinhDoTinHoc, "MaTrinhDoTinHoc", "TenTrinhDoTinHoc", maTrinhDoTinHoc);

            ViewBag.Level = new SelectList(QHXP.GetData(), "Level", "Name", Level);
        }
        #endregion Helper
    }
}
