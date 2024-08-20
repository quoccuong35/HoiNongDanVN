using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Linq;
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
            DateTime FirstDateOfYear = new DateTime(Nam.Value,1, 1);
            //var data = _context.CanBos.Where(it => it.IsHoiVien == true && it.TuChoi == false && it.HoiVienDuyet == true)
            //    .Include(it=>it.DiaBanHoatDong).ThenInclude(it=>it.QuanHuyen).AsQueryable();
            //var model = data.GroupBy(it => new { it.DiaBanHoatDong!.MaQuanHuyen, it.DiaBanHoatDong.QuanHuyen.TenQuanHuyen })
            //            .Select(it => new {
            //                STT = 1,
            //                Cot2 = it.Key.TenQuanHuyen,
            //                Cot3 = 0,
            //                Cot4 = 0,
            //                Cot5 = 0,
            //                Cot6 = 0,
            //                Cot7 = 0,
            //                Cot8 = 0,
            //                Cot9 = 0,
            //                Cot10 = 0,
            //                Cot11 = it.Where(p=>p.DiaBanHoatDong!.MaQuanHuyen == it.Key.MaQuanHuyen).Count(),
            //            }).ToList();

            var datatest = _context.DiaBanHoatDongs
                            .Include(it=>it.CanBos)
                                .ThenInclude(it =>it.ChiHoi)
                            .Include(it => it.CanBos)
                                .ThenInclude(it => it.ToHoi)
                            .Include(it => it.QuanHuyen)
                           .Where(it => it.CanBos.All(it => it.IsHoiVien == true && it.HoiVienDuyet == true)
                           ).Select(it=>new { 
                               it.MaQuanHuyen,
                               it.QuanHuyen.TenQuanHuyen,
                               it.CanBos
                           });
          
            return ExecuteSearch(() => { 
                var data = _context.BaoCaoThucLucHois.Include(it=>it.DonVi).Where(it=>it.Loai =="02").AsEnumerable();
                if(Nam != null) {
                    data = data.Where(it => it.Nam == Nam.Value);
                }
                //if (IDDonVi != null)
                //{
                //    data = data.Where(it => it.IDDonVi == IDDonVi.Value);
                //}
                return PartialView(data.OrderBy(it=>it.Cot1).ToList());
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
            if (data == null || data.Count() == 0)
            {
                return Content("Không có dữ liệu xuất");
            }

            var model = data.Select(it => new BaoCaoThucLucHoiVM {
                Cot1 = it.Cot1,
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
                Cot29 = it.Cot29,
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
                Cot57 = it.Cot57,
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
            byte[] filecontent = ClassExportExcel.ExportExcel(model,startIndex +1,url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "BaoCoThucLucHoiNam");

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        private List<BaoCaoThucLucHoiVM> LoadData(int? Nam, int? IDDonVi) {
            var data = _context.BaoCaoThucLucHois.Include(it => it.DonVi).Where(it => it.Loai == "02").AsEnumerable();
            if (Nam != null)
            {
                data = data.Where(it => it.Nam == Nam.Value);
            }
            if (IDDonVi != null)
            {
                data = data.Where(it => it.IDDonVi == IDDonVi.Value);
            }
           
            var model = data.Select(it => new BaoCaoThucLucHoiVM
            {
                Cot1 = it.Cot1,
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
                Cot29 = it.Cot29,
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
                Cot57 = it.Cot57,
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
            //BaoCaoThucLucHoiVM addTong = new BaoCaoThucLucHoiVM();
            //addTong.Cot1 = "Tổng cộng";
            //addTong.Cot29 = addTong.Cot1;
            //addTong.Cot57 = addTong.Cot1;
            //addTong.Cot2 = model.Sum(it => it.Cot2);
            //addTong.Cot3 = model.Sum(it => it.Cot3);
            //addTong.Cot4 = model.Sum(it => it.Cot4);
            //addTong.Cot5 = model.Sum(it => it.Cot5);
            //addTong.Cot6 = model.Sum(it => it.Cot6);
            //addTong.Cot7 = model.Sum(it => it.Cot7);
            //addTong.Cot8 = model.Sum(it => it.Cot8);
            //addTong.Cot9 = model.Sum(it => it.Cot9);
            //addTong.Cot10 = model.Sum(it => it.Cot10);
            //addTong.Cot11 = model.Sum(it => it.Cot11);
            //addTong.Cot12 = model.Sum(it => it.Cot12);
            //addTong.Cot13 = model.Sum(it => it.Cot13);
            //addTong.Cot14 = model.Sum(it => it.Cot14);
            //addTong.Cot15 = model.Sum(it => it.Cot15);
            //addTong.Cot16 = model.Sum(it => it.Cot16);
            //addTong.Cot17 = model.Sum(it => it.Cot17);
            //addTong.Cot18 = model.Sum(it => it.Cot18);
            //addTong.Cot19 = model.Sum(it => it.Cot19);
            //addTong.Cot20 = model.Sum(it => it.Cot20);
            //addTong.Cot21 = model.Sum(it => it.Cot21);
            //addTong.Cot22 = model.Sum(it => it.Cot22);
            //addTong.Cot23 = model.Sum(it => it.Cot23);
            //addTong.Cot24 = model.Sum(it => it.Cot24);
            //addTong.Cot25 = model.Sum(it => it.Cot25);
            //addTong.Cot26 = model.Sum(it => it.Cot26);
            //addTong.Cot27 = model.Sum(it => it.Cot27);
            //addTong.Cot30 = model.Sum(it => it.Cot30);
            //addTong.Cot31 = model.Sum(it => it.Cot31);
            //addTong.Cot32 = model.Sum(it => it.Cot32);
            //addTong.Cot33 = model.Sum(it => it.Cot33);
            //addTong.Cot34 = model.Sum(it => it.Cot34);
            //addTong.Cot35 = model.Sum(it => it.Cot35);
            //addTong.Cot36 = model.Sum(it => it.Cot36);
            //addTong.Cot37 = model.Sum(it => it.Cot37);
            //addTong.Cot38 = model.Sum(it => it.Cot38);
            //addTong.Cot39 = model.Sum(it => it.Cot39);
            //addTong.Cot40 = model.Sum(it => it.Cot40);
            //addTong.Cot41 = model.Sum(it => it.Cot41);
            //addTong.Cot42 = model.Sum(it => it.Cot42);
            //addTong.Cot43 = model.Sum(it => it.Cot43);
            //addTong.Cot44 = model.Sum(it => it.Cot44);
            //addTong.Cot45 = model.Sum(it => it.Cot45);
            //addTong.Cot46 = model.Sum(it => it.Cot46);
            //addTong.Cot47 = model.Sum(it => it.Cot47);
            //addTong.Cot48 = model.Sum(it => it.Cot48);
            //addTong.Cot49 = model.Sum(it => it.Cot49);
            //addTong.Cot50 = model.Sum(it => it.Cot50);
            //addTong.Cot51 = model.Sum(it => it.Cot51);
            //addTong.Cot52 = model.Sum(it => it.Cot52);
            //addTong.Cot53 = model.Sum(it => it.Cot53);
            //addTong.Cot54 = model.Sum(it => it.Cot54);
            //addTong.Cot55 = model.Sum(it => it.Cot55);
            //addTong.Cot58 = model.Sum(it => it.Cot58);
            //addTong.Cot59 = model.Sum(it => it.Cot59);
            //addTong.Cot60 = model.Sum(it => it.Cot60);
            //addTong.Cot61 = model.Sum(it => it.Cot61);
            //addTong.Cot62 = model.Sum(it => it.Cot62);
            //addTong.Cot63 = model.Sum(it => it.Cot63);
            //addTong.Cot64 = model.Sum(it => it.Cot64);
            //addTong.Cot65 = model.Sum(it => it.Cot65);
            //addTong.Cot66 = model.Sum(it => it.Cot66);
            //addTong.Cot67 = model.Sum(it => it.Cot67);
            //addTong.Cot68 = model.Sum(it => it.Cot68);
            //addTong.Cot69 = model.Sum(it => it.Cot69);
            //addTong.Cot70 = model.Sum(it => it.Cot70);
            //addTong.Cot71 = model.Sum(it => it.Cot71);
            //addTong.Cot72 = model.Sum(it => it.Cot72);
            //addTong.Cot73 = model.Sum(it => it.Cot73);
            //addTong.Cot74 = model.Sum(it => it.Cot74);
            //addTong.Cot75 = model.Sum(it => it.Cot75);
            //addTong.Cot76 = model.Sum(it => it.Cot76);
            //addTong.Cot77 = model.Sum(it => it.Cot77);
            //addTong.Cot78 = model.Sum(it => it.Cot78);
            //addTong.Cot79 = model.Sum(it => it.Cot79);
            //addTong.Cot80 = model.Sum(it => it.Cot80);

            return model;
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
            if (Nam == null)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = false,
                    Data = "Chưa chọn năm"
                }); ;
            }
            DataSet ds = GetDataSetFromExcel();
            List<string> errorList = new List<string>();
            return ExcuteImportExcel(() => {
                if (ds.Tables.Count > 0)
                {
                    const TransactionScopeOption opt = new TransactionScopeOption();

                    TimeSpan span = new TimeSpan(0, 0, 30, 30);
                    int capNhat = 0;
                    using (TransactionScope ts = new TransactionScope(opt, span))
                    {
                        DataTable dt = ds.Tables[0];
                        string contCode = dt.Columns[0].ColumnName.ToString();
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dt.Rows.IndexOf(dr) == startIndex-1)
                            {
                                if (!string.IsNullOrEmpty(dr.ItemArray[0]!.ToString()))
                                {
                                    var data = CheckTemplate(dr.ItemArray!, IDDonVi.Value, Nam.Value);
                                    string result = ExecuteImportExcel(data);

                                    if (result != LanguageResource.ImportSuccess)
                                    {
                                        errorList.Add(result);
                                    }
                                    else
                                        capNhat++;
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
                            Data = LanguageResource.ImportSuccess + " "+ capNhat.ToString()
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
            data.IDDonVi = 99;
            data.Nam = nam;
            data.ID = Guid.NewGuid();
            data.CreatedTime = DateTime.Now;
            data.CreatedAccountId = AccountId();
            string? value;
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
