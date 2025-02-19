using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.ViewModels.Masterdata;
using HoiNongDan.Resources;
using HoiNongDan.Web.Areas.NhanSu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using System.Transactions;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class CBQuaTrinhLuongController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        const string controllerCode = ConstExcelController.CanBoQuTrinhLuong;
        const int startIndex = 6;
        public CBQuaTrinhLuongController(AppDbContext context, IWebHostEnvironment hostEnvironment) :base(context) { 
            _hostEnvironment = hostEnvironment; 
        }
        #region Index
        public IActionResult Index()
        {
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(String MaCanBo)
        {
            return ExecuteSearch(() => {
                var model = _context.CanBoQuaTrinhLuongs.Include(it=>it.CanBo).Include(it=>it.NgachLuong).Include(it=>it.BacLuong).AsQueryable();
                if (!String.IsNullOrEmpty(MaCanBo))
                {
                    model = model.Where(it => it.CanBo.MaCanBo == MaCanBo);
                }
                var data = model.Select(it=>new CanBoQuaTrinhLuongDetailVM
                {
                    ID = it.ID,
                    MaCanBo = it.CanBo!.MaCanBo,
                    HoVaTen = it.CanBo.HoVaTen,
                    MaNgachLuong = it.NgachLuong.TenNgachLuong,
                    BacLuong = it.MaNgachLuong + "/"+it.BacLuong.OrderIndex.ToString(),
                    HeSoLuong = it.HeSoLuong,
                    HeSoChucVu = it.HeSoChucVu,
                    NgayHuong = it.NgayHuong,
                    KiemNhiem   = it.KiemNhiem,
                    VuotKhung = it.VuotKhung,
                    NgayNangBac = it.NgayNangBacLuong
                }).ToList();
                return PartialView(data);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create() {
            QuaTrinhLuongVM quaTrinhLuong = new QuaTrinhLuongVM();

            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu.CanBo = true;
            quaTrinhLuong.NhanSu = nhanSu;
            CreateViewBag();
            return View(quaTrinhLuong);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Create(QuaTrinhLuongVM item, IFormFile? fileInbox)
        {
            CheckError( item);
            var add = new CanBoQuaTrinhLuong();
            add.ID = Guid.NewGuid();

            FileDinhKemModel addFile = new FileDinhKemModel();
            if (fileInbox != null)
            {
                addFile.Id = add.ID;
                addFile.IdCanBo = item.NhanSu.IdCanbo;
                addFile.IDLoaiDinhKem = "04";
                FunctionFile.CopyFile(_hostEnvironment, fileInbox, addFile);
                if (!String.IsNullOrEmpty(addFile.Error) && !String.IsNullOrWhiteSpace(addFile.Error))
                {
                    ModelState.AddModelError("fileInbox", "Lỗi không cập nhật được file đính kèm");
                }
            }
            return ExecuteContainer(() => {
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                add.IDCanBo = item.NhanSu.IdCanbo.Value;
                add.MaNgachLuong = item.MaNgachLuong;
                add.MaBacLuong = item.MaBacLuong;
                add.HeSoLuong = item.HeSoLuong;
                add.HeSoChucVu = item.HeSoChucVu ;
                add.VuotKhung = item.VuotKhung;
                add.KiemNhiem = item.KiemNhiem ;
                add.NgayHuong = item.NgayHuong;
                add.NgayNangBacLuong = item.NgayNangBacLuong ;
                //var canBo = _context.CanBos.SingleOrDefault(it => it.IDCanBo == add.IDCanBo);
                if (!String.IsNullOrEmpty(addFile.Url) && !String.IsNullOrWhiteSpace(addFile.Url))
                {
                    FileDinhKem fileDinhKem = addFile.GetFileDinhKem();
                    _context.FileDinhKems.Add(fileDinhKem);
                }

                _context.CanBoQuaTrinhLuongs.Add(add);

                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.CanBoQuaTrinhLuong.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
            var item = _context.CanBoQuaTrinhLuongs.SingleOrDefault(it => it.ID == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            QuaTrinhLuongVM obj = new QuaTrinhLuongVM();
            obj.CapNhatTinhTrangCanBo = false;
            var file = _context.FileDinhKems.SingleOrDefault(it => it.Id == id);

            var canBo = _context.CanBos.Include(it => it.CoSo).Include(it => it.Department)
                        .Include(it => it.PhanHe).Include(it => it.TinhTrang).Where(it => it.IDCanBo == item.IDCanBo).SingleOrDefault();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu = nhanSu.GeThongTin(canBo!);
            nhanSu.CanBo = true;
            nhanSu.IdCanbo = canBo!.IDCanBo;
            nhanSu.HoVaTen = canBo.HoVaTen;
            nhanSu.MaCanBo = canBo.MaCanBo;
            nhanSu.TenTinhTrang = canBo.TinhTrang.TenTinhTrang;
            //nhanSu.TenCoSo = canBo.CoSo.TenCoSo;
            nhanSu.TenDonVi = canBo.Department.Name;
            //nhanSu.TenPhanHe = canBo.PhanHe.TenPhanHe;
            nhanSu.Edit = false;

            obj.MaNgachLuong = item.MaNgachLuong;
            obj.MaBacLuong = item.MaBacLuong;
            obj.HeSoLuong = item.HeSoLuong;
            obj.HeSoChucVu = item.HeSoChucVu;
            obj.VuotKhung = item.VuotKhung;
            obj.KiemNhiem = item.KiemNhiem;
            obj.NgayHuong = item.NgayHuong;
            obj.NgayNangBacLuong = item.NgayNangBacLuong;
            obj.NhanSu = nhanSu;
            obj.Id = item.ID;
            obj.FileDinhKem = file;
            CreateViewBag(maNgachLuong: item.MaNgachLuong, maBacLuong: item.MaBacLuong);
            return View(obj);
        }

        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(QuaTrinhLuongVM item, IFormFile? fileInbox)
        {
            CheckError(item);
           
            return ExecuteContainer(() => {
                var edit = _context.CanBoQuaTrinhLuongs.SingleOrDefault(it => it.ID == item.Id!.Value);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.CanBoQuaTrinhLuong.ToLower())
                    });
                }
                else
                {
                    edit.MaNgachLuong = item.MaNgachLuong;
                    edit.MaBacLuong = item.MaBacLuong;
                    edit.HeSoLuong = item.HeSoLuong;
                    edit.HeSoChucVu = item.HeSoChucVu;
                    edit.VuotKhung = item.VuotKhung;
                    edit.KiemNhiem = item.KiemNhiem;
                    edit.NgayHuong = item.NgayHuong;
                    edit.NgayNangBacLuong = item.NgayNangBacLuong;
                    edit.LastModifiedTime = DateTime.Now;
                    edit.LastModifiedAccountId = AccountId();
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.CanBoQuaTrinhLuong.ToLower())
                    });
                }
            });
        }
        #endregion Edit
        #region Import Excel 
        public IActionResult Import()
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
            catch
            {
                return null!;
            }
        }
        #endregion Import Excel
        #region Export Data
        public IActionResult ExportCreate()
        {
            List<CanBoQuaTrinhLuongVM> data = new List<CanBoQuaTrinhLuongVM>();
            return Export(data);
        }
        public IActionResult ExportEdit()
        {
            List<CanBoQuaTrinhLuongVM> data = new List<CanBoQuaTrinhLuongVM>();
            return Export(data);
        }
        public FileContentResult Export(List<CanBoQuaTrinhLuongVM> menu)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "ID", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "MaCanBo", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "MaNgachLuong", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "BacLuong", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HeoSoLuong", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HeSoChucVu", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "VuotKhung", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "KiemNhiem", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayHuong", isAllowedToEdit = true, isDateTime = true });
            
            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.CanBoQuaTrinhLuong);
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
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.CanBoQuaTrinhLuongs.FirstOrDefault(p => p.ID == id);


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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.CanBoQuaTrinhLuong.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.CanBoQuaTrinhLuong.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Insert/Update data from excel file
        public string ExecuteImportExcelMenu(CanBoQuaTrinhLuongExcel canBoExcel)
        {
            //Check:
            //1. If MenuId == "" then => Insert
            //2. Else then => Update
            if (canBoExcel.isNullValueId == true)
            {
                CanBoQuaTrinhLuong canboQuaTrinhLuong = canBoExcel.AddQuaTrinhLuong();

                _context.Entry(canboQuaTrinhLuong).State = EntityState.Added;
            }
            else
            {
                //CanBoQuaTrinhLuong canBo = _context.Cca.Where(p => p.IDCanBo == canBoExcel.IDCanBo).FirstOrDefault();
                //if (canBo != null)
                //{
                //    canBo = canBoExcel.EditUpdate(canBo);
                //    HistoryModelRepository history = new HistoryModelRepository(_context);
                //    history.SaveUpdateHistory(canBo.IDCanBo.ToString(), AccountId()!.Value, canBo);
                //}
                //else
                //{
                //    return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                //                            LanguageResource.CanBo, canBo.IDCanBo,
                //                            string.Format(LanguageResource.Export_ExcelHeader,
                //                            LanguageResource.CanBo));
                //}
            }
            _context.SaveChanges();
            return LanguageResource.ImportSuccess;
        }
        #endregion Insert/Update data from excel file
        #region Check data type 
        public CanBoQuaTrinhLuongExcel CheckTemplate(object[] row)
        {
            CanBoQuaTrinhLuongExcel data = new CanBoQuaTrinhLuongExcel();
            int index = 0;
            string value = string.Empty;
            for (int i = 0; i < row.Length; i++)
            {
                value = row[i].ToString()!;

                switch (i)
                {
                    case 0:
                        //Row Index
                        data.RowIndex = index = int.Parse(value!);
                        break;
                    case 1:
                        // IDCanBo
                        if (string.IsNullOrEmpty(value) || value == "")
                        {
                            data.isNullValueId = true;
                        }
                        else
                        {
                            data.ID = Guid.Parse(value);
                            data.isNullValueId = false;
                        }
                        break;
                    case 2:
                        // Mã nhân viên
                       
                        if (string.IsNullOrEmpty(value))
                        {
                            data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.MaCanBo), index);
                        }
                        else
                        {
                            var canBo = _context.CanBos.SingleOrDefault(it => it.MaCanBo == value && it.IsCanBo == true);
                            if (canBo == null)
                            {
                                data.Error = string.Format("Không tìm thấy cán bộ có mã {0}", value);
                            }
                            else
                            {
                                data.IDCanBo = canBo.IDCanBo;
                            }
                        }
                        break;
                    //case 4:
                    //    data.MaNgachLuong = value;
                    //    break;
                    //case 5:
                    //    data.BacLuong = value;
                    //    break;
                    //case 6:
                    //    data.HeSoLuong = value;
                    //    break;
                    //case 7:
                    //    data.HeSoChucVu = value;
                    //    break;
                    //case 8:
                    //    data.VuotKhung = value;
                    //    break;
                    //case 9:
                    //    data.KiemNhiem = value;
                    //    break;
                    //case 10:;
                    //    data.NgayHuong = value;
                    //    break;
                }
            }
            return data;
        }
        #endregion Check data type 
        #region Helper
        [NonAction]
        private void CreateViewBag( String? maNgachLuong = null, Guid? maBacLuong = null )
        {

            var ngachLuong = _context.NgachLuongs.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { MaNgachLuong = it.MaNgachLuong, TenNgachLuong = it.TenNgachLuong }).ToList();
            ViewBag.MaNgachLuong = new SelectList(ngachLuong, "MaNgachLuong", "TenNgachLuong", maNgachLuong);

            var bacLuong = _context.BacLuongs.Where(it => it.Actived == true && (it.MaNgachLuong == maNgachLuong || maNgachLuong == null)).OrderBy(p => p.OrderIndex).Select(it => new { MaBacLuong = it.MaBacLuong, TenBacLuong = it.TenBacLuong + " " + it.HeSo.ToString() }).ToList();
            ViewBag.MaBacLuong = new SelectList(bacLuong, "MaBacLuong", "TenBacLuong", maBacLuong);

        }
        private void CheckError(QuaTrinhLuongVM item) {
            if (item.NhanSu.IdCanbo == null) {
                ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
            }
        }
        #endregion Helper
    }
}
