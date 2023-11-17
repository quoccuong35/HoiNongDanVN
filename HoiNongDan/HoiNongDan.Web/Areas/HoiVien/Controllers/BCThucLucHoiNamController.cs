using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using System.Data;
using System.Reflection;
using System.Transactions;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class BCThucLucHoiNamController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 13;
        public BCThucLucHoiNamController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context)
        {
            _hostEnvironment = hostEnvironment;
            DateFomat = config.GetSection("SiteSettings:DateFormat").Value.ToString().Split(',');
        }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBagSearch();
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(int? Nam, int? IDDonVi) {
            return ExecuteSearch(() => { 
                var data = _context.BaoCaoThucLucHois.Include(it=>it.DonVi).Where(it=>it.Loai =="02").AsEnumerable();
                if(Nam != null) {
                    data = data.Where(it => it.Nam == Nam.Value);
                }
                if (IDDonVi != null)
                {
                    data = data.Where(it => it.IDDonVi == IDDonVi.Value);
                }
                return PartialView(data);
            });
        }
        #endregion Index
        #region Export
        public IActionResult ExportCreate()
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\BCThucLucHoi.xlsx");
            byte[] filecontent = ClassExportExcel.ExportFileMau(url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx","BaoCoThucLucHoi");

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        public IActionResult ExportEdit(int? Nam, int? IDDonVi) {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\BCThucLucHoi.xlsx");
            var data = _context.BaoCaoThucLucHois.Include(it => it.DonVi).Where(it => it.Loai == "02").AsEnumerable();
            if (Nam != null)
            {
                data = data.Where(it => it.Nam == Nam.Value);
            }
            if (IDDonVi != null)
            {
                data = data.Where(it => it.IDDonVi == IDDonVi.Value);
            }
            var model = data.Select(it => new BaoCaoThucLucHoiVM {
                Cot1 = it.DonVi.TenDonVi,
                Cot2 = it.Cot2,
                Cot3 = it.Cot3,
                Cot4 = it.Cot4,
                Cot5 = it.Cot5,
                Cot6 = it.Cot6,
                Cot7 = it.Cot7,
                Cot8 = it.Cot8,
                Cot9 = it.Cot9,
                Cot10 = it.Cot10,
                Cot11 = it.Cot11,
                Cot12 = it.Cot12,
                Cot13 = it.Cot13,
                Cot14 = it.Cot14,
                Cot15 = it.Cot15,
                Cot16 = it.Cot16,
                Cot17 = it.Cot17,
                Cot18 = it.Cot18,
                Cot19 = it.Cot19,
                Cot20 = it.Cot20,
                Cot21 = it.Cot21,
                Cot22 = it.Cot22,
                Cot23 = it.Cot23,
                Cot24 = it.Cot24,
                Cot25 = it.Cot25,
                Cot26 = it.Cot26,
                Cot27 = it.Cot27,
                Cot28 = it.Cot28,
                Cot29 = it.DonVi.TenDonVi,
                Cot30 = it.Cot30,
                Cot31 = it.Cot31,
                Cot32 = it.Cot32,
                Cot33 = it.Cot33,
                Cot34 = it.Cot34,
                Cot35 = it.Cot35,
                Cot36 = it.Cot36,
                Cot37 = it.Cot37,
                Cot38 = it.Cot38,
                Cot39 = it.Cot39,
                Cot40 = it.Cot40,
                Cot41 = it.Cot41,
                Cot42 = it.Cot42,
                Cot43 = it.Cot43,
                Cot44 = it.Cot44,
                Cot45 = it.Cot45,
                Cot46 = it.Cot46,
                Cot47 = it.Cot47,
                Cot48 = it.Cot48,
                Cot49 = it.Cot49,
                Cot50 = it.Cot50,
                Cot51 = it.Cot51,
                Cot52 = it.Cot52,
                Cot53 = it.Cot53,
                Cot54 = it.Cot54,
                Cot55 = it.Cot55,
                Cot56 = it.Cot56,
                Cot57 = it.DonVi.TenDonVi,
                Cot58 = it.Cot58,
                Cot59 = it.Cot59,
                Cot60 = it.Cot60,
                Cot61 = it.Cot61,
                Cot62 = it.Cot62,
                Cot63 = it.Cot63,
                Cot64 = it.Cot64,
                Cot65 = it.Cot65,
                Cot66 = it.Cot66,
                Cot67 = it.Cot67,
                Cot68 = it.Cot68,
                Cot69 = it.Cot69,
                Cot70 = it.Cot70,
                Cot71 = it.Cot71,
                Cot72 = it.Cot72,
                Cot73 = it.Cot73,
                Cot74 = it.Cot74,
                Cot75 = it.Cot75,
                Cot76 = it.Cot76,
                Cot77 = it.Cot77,
                Cot78 = it.Cot78,
                Cot79 = it.Cot79,
                Cot80 = it.Cot80,

            }).ToList();
            int i = 1;
            foreach (var item in model)
            {
                item.STT = i;
                item.Cot28 = i.ToString();
                item.Cot56 = i.ToString();
            }
            byte[] filecontent = ClassExportExcel.ExportExcel(model,startIndex,url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "BaoCoThucLucHoi");

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export
        #region Import
        public IActionResult _Import()
        {
            CreateViewBagSearch();
            return PartialView();
        }
        public IActionResult Import(int? Nam, int? IDDonVi)
        {
            if (Nam == null || IDDonVi ==null)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = false,
                    Data = "Chưa chọn năm hoặc đơn vị"
                }); ;
            }
            DataSet ds = GetDataSetFromExcel();
            List<string> errorList = new List<string>();
            return ExcuteImportExcel(() => {
                if (ds.Tables.Count > 0)
                {
                    const TransactionScopeOption opt = new TransactionScopeOption();

                    TimeSpan span = new TimeSpan(0, 0, 30, 30);
                    using (TransactionScope ts = new TransactionScope(opt, span))
                    {

                        foreach (DataTable dt in ds.Tables)
                        {
                            string contCode = dt.Columns[0].ColumnName.ToString();
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dt.Rows.IndexOf(dr) == startIndex)
                                {
                                    if (!string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                    {
                                        var data = CheckTemplate(dr.ItemArray, IDDonVi.Value, Nam.Value);
                                        string result = ExecuteImportExcel(data);

                                        if (result != LanguageResource.ImportSuccess)
                                        {
                                            errorList.Add(result);
                                        }
                                    }
                                    else
                                        break;
                                    //Check correct template

                                }
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
        #endregion Import

        private void CreateViewBagSearch()
        {

            var donVis = _context.DonVis.Select(it => new { IDDonVi = it.IDDonVi, Name = it.TenDonVi }).ToList();
            ViewBag.IDDonVi = new SelectList(donVis, "IDDonVi", "Name");
        }

        private BaoCaoThucLucHoi CheckTemplate(object[] row,int idDonVi, int nam)
        {
            BaoCaoThucLucHoi data = new BaoCaoThucLucHoi();
            Type examType = typeof(BaoCaoThucLucHoi);
            data.Loai = "02";
            data.IDDonVi = idDonVi;
            data.Nam = nam;
            data.ID = Guid.NewGuid();
            data.CreatedTime = DateTime.Now;
            data.CreatedAccountId = AccountId();
            string? value;
            int index = 0;
            for (int i = 0; i < row.Length; i++) {
                if (i > 80)
                {
                    break;
                }
                value = row[i].ToString();
                if (i > 0 ) {
                    try
                    {
                        if (!String.IsNullOrWhiteSpace(value) && !String.IsNullOrEmpty(value))
                        {
                            PropertyInfo piInstance = examType.GetProperty("Cot" + i);
                            piInstance.SetValue(data, Decimal.Parse(value.Replace(",","")));
                        }
                    }
                    catch 
                    {
                        PropertyInfo piInstance = examType.GetProperty("Cot" + i);
                        piInstance.SetValue(data, value);
                    }
                }
            }
            return data;
        }
        #region Insert/Update data from excel file
        public string ExecuteImportExcel(BaoCaoThucLucHoi data)
        {
            // check exist 
            var exist = _context.BaoCaoThucLucHois.SingleOrDefault(it => it.IDDonVi == data.IDDonVi && it.Nam == data.Nam && it.Loai == "02");
            if(exist != null)
            {
                _context.BaoCaoThucLucHois.Remove(exist);
            }
            _context.Entry(data).State = EntityState.Added;
            _context.SaveChanges();
            return LanguageResource.ImportSuccess;
        }
        #endregion Insert/Update data from excel file
    }
}
