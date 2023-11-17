using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
                var model = _context.CanBoQuaTrinhLuongs.Include(it=>it.CanBo).AsQueryable();
                if (!String.IsNullOrEmpty(MaCanBo))
                {
                    model = model.Where(it => it.CanBo.MaCanBo == MaCanBo);
                }
                var data = model.Select(it=>new CanBoQuaTrinhLuongVM {
                    ID = it.ID,
                    MaCanBo = it.CanBo.MaCanBo,
                    HoVaTen = it.CanBo.HoVaTen,
                    MaNgachLuong = it.MaNgachLuong,
                    BacLuong = it.BacLuong,
                    HeoSoLuong = it.HeoSoLuong,
                    HeSoChucVu = it.HeSoChucVu,
                    NgayHuong = it.NgayHuong,
                    KiemNhiem   = it.KiemNhiem,
                    VuotKhung = it.VuotKhung,
                }).ToList();
                return PartialView(data);
            });
        }
        #endregion Index
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
                return null;
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
                value = row[i].ToString();

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
                    case 4:
                        data.MaNgachLuong = value;
                        break;
                    case 5:
                        data.BacLuong = value;
                        break;
                    case 6:
                        data.HeoSoLuong = value;
                        break;
                    case 7:
                        data.HeSoChucVu = value;
                        break;
                    case 8:
                        data.VuotKhung = value;
                        break;
                    case 9:
                        data.KiemNhiem = value;
                        break;
                    case 10:;
                        data.NgayHuong = value;
                        break;
                }
            }
            return data;
        }
        #endregion Check data type 
    }
}
