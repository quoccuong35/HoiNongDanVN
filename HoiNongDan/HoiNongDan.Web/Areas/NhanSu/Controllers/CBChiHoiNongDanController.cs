using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.Entitys.MasterData;
using HoiNongDan.Resources;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Transactions;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class CBChiHoiNongDanController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        const string controllerCode = ConstExcelController.CanBo;
        const int startIndex = 5;
        private string urlFileMau = @"upload\filemau\CanBoChiHoiNongDan.xlsx";
        public CBChiHoiNongDanController(AppDbContext context, IWebHostEnvironment hostEnvironment) : base(context)
        {
            _hostEnvironment = hostEnvironment;
        }
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult _Search(CanBoSearchVM search)
        {
            return ExecuteSearch(() => {
                var data = LoadData(search);
                return PartialView(data);
            });
        }
        #region Create
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Create() {
            CreateViewbag();
            return View();
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CanBoChiHoiNongDanVM item) {
            return ExecuteContainer(() => {
                CanBo add = new CanBo();
                add.IDCanBo = Guid.NewGuid();
                add.Level = "40";
                add.IsCanBo = true;
                add.Actived = true;
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.HoVaTen = item.HoVaTen;
                add.GioiTinh = item.GioiTinh;
                add.DonVi = item.DonVi;
                add.NgaySinh = item.NgaySinh;
                add.MaDanToc = item.MaDanToc;
                add.MaTonGiao = item.MaTonGiao;
                add.ChoOHienNay = item.ChoOHienNay;
                add.NgayvaoDangDuBi = item.NgayVaoDangDuBi;
                add.NgayVaoDangChinhThuc = item.NgayVaoDangChinhThuc;
                add.MaTrinhDoHocVan = item.MaTrinhDoHocVan;
                add.ChuyenNganh = item.ChuyenNganh;
                add.MaTrinhDoChinhTri = item.MaTrinhDoChinhTri;
                add.ChiHoiDanCu_CHT = item.ChiHoiDanCu_CHT;
                add.ChiHoiDanCu_CHP = item.ChiHoiDanCu_CHP;
                _context.Attach(add).State = EntityState.Modified;
                _context.CanBos.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.CBChiHoiNongDan.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Edit(Guid id) {
            var item = _context.CanBos.SingleOrDefault(it => it.IDCanBo == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            CanBoChiHoiNongDanVM edit = new CanBoChiHoiNongDanVM ();
            edit.IDCanBo = item.IDCanBo;
            edit.Level = "40";
            
            edit.HoVaTen = item.HoVaTen;
            edit.GioiTinh = item.GioiTinh;
            edit.DonVi = item.DonVi!;
            edit.NgaySinh = item.NgaySinh;
            edit.MaDanToc = item.MaDanToc;
            edit.MaTonGiao = item.MaTonGiao;
            edit.ChoOHienNay = item.ChoOHienNay!;
            edit.NgayVaoDangDuBi = item.NgayvaoDangDuBi;
            edit.NgayVaoDangChinhThuc = item.NgayVaoDangChinhThuc;
            edit.MaTrinhDoHocVan = item.MaTrinhDoHocVan;
            edit.ChuyenNganh = item.ChuyenNganh;
            edit.MaTrinhDoChinhTri = item.MaTrinhDoChinhTri;
            edit.ChiHoiDanCu_CHT = item.ChiHoiDanCu_CHT;
            edit.ChiHoiDanCu_CHP = item.ChiHoiDanCu_CHP;
            CreateViewbag(maTrinhDoChinhTri:edit.MaTrinhDoChinhTri,maDanToc:edit.MaDanToc,maTonGiao:edit.MaTonGiao,maTrinhDoHocVan:edit.MaTrinhDoHocVan);
            return View(edit);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CanBoChiHoiNongDanVM item) {
            return ExecuteContainer(() => {
                var edit = _context.CanBos.SingleOrDefault(it => it.IDCanBo == item.IDCanBo);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.CBChiHoiNongDan.ToLower())
                    });
                }
                else
                {
                    edit.HoVaTen = item.HoVaTen;
                    edit.GioiTinh = item.GioiTinh;
                    edit.DonVi = item.DonVi!;
                    edit.NgaySinh = item.NgaySinh;
                    edit.MaDanToc = item.MaDanToc;
                    edit.MaTonGiao = item.MaTonGiao;
                    edit.ChoOHienNay = item.ChoOHienNay!;
                    edit.NgayvaoDangDuBi = item.NgayVaoDangDuBi;
                    edit.NgayVaoDangChinhThuc = item.NgayVaoDangChinhThuc;
                    edit.MaTrinhDoHocVan = item.MaTrinhDoHocVan;
                    edit.ChuyenNganh = item.ChuyenNganh;
                    edit.MaTrinhDoChinhTri = item.MaTrinhDoChinhTri;
                    edit.ChiHoiDanCu_CHT = item.ChiHoiDanCu_CHT;
                    edit.ChiHoiDanCu_CHP = item.ChiHoiDanCu_CHP;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.CBChiHoiNongDan.ToLower())
                    });
                }
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
                var del = _context.CanBos.FirstOrDefault(p => p.IDCanBo == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.CBChiHoiNongDan.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.CBChiHoiNongDan.ToLower())
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
        public IActionResult Import()
        {
            DataSet ds = GetDataSetFromExcel();
            List<string> errorList = new List<string>();
            return ExcuteImportExcel(() => {
                if (ds.Tables.Count > 0)
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        DateTime dtNow = DateTime.Now;
                        DataTable dt = ds.Tables[0];
                        int stt = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            //string aa = dr.ItemArray[0].ToString();
                            if (dt.Rows.IndexOf(dr) >= startIndex - 1)
                            {
                                if (!string.IsNullOrEmpty(dr.ItemArray[0]!.ToString()))
                                {
                                    var data = CheckTemplate1(dr.ItemArray!);
                                    if (!string.IsNullOrEmpty(data.Error))
                                    {
                                        errorList.Add(data.Error);
                                    }
                                    else
                                    {
                                        data.CanBo.CreatedTime = dtNow;
                                        // Tiến hành cập nhật
                                        string result = ExecuteImportExcelMenu(data);
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
        [NonAction]
        public string ExecuteImportExcelMenu(CanBoChiHoiNongDanExelVM canBoExcel)
        {
            if (canBoExcel.isNullValueId == true)
            {
                canBoExcel.CanBo.CreatedAccountId = AccountId();
                canBoExcel.CanBo.Level = "40";
                _context.Entry(canBoExcel.CanBo).State = EntityState.Added;
            }
            else
            {
                var canBo = _context.CanBos.Where(p => p.IDCanBo == canBoExcel.CanBo.IDCanBo).SingleOrDefault();
                if (canBo != null)
                {
                    canBo = EditCanBoHoiNongDanThanhPho(canBo, canBoExcel.CanBo);
                    HistoryModelRepository history = new HistoryModelRepository(_context);
                    history.SaveUpdateHistory(canBo.IDCanBo.ToString(), AccountId()!.Value, canBo);
                }
                else
                {
                    return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                            LanguageResource.CanBo, canBo!.IDCanBo,
                                            string.Format(LanguageResource.Export_ExcelHeader,
                                            LanguageResource.CanBo));
                }
            }
            _context.SaveChanges();
            return LanguageResource.ImportSuccess;

        }
        [NonAction]
        private CanBo EditCanBoHoiNongDanThanhPho(CanBo old, CanBo news)
        {
            old.HoVaTen = news.HoVaTen;
            old.MaChucVu = news.MaChucVu;
            old.DonVi = news.DonVi;
            old.GioiTinh = news.GioiTinh;
            old.MaDanToc = news.MaDanToc;
            old.MaTonGiao = news.MaTonGiao;
            old.NoiSinh = news.NoiSinh;
            old.ChoOHienNay = news.ChoOHienNay;
            old.NgayvaoDangDuBi = news.NgayvaoDangDuBi;
            old.NgayVaoDangChinhThuc = news.NgayVaoDangChinhThuc;
            old.DangVien = news.DangVien;
            old.ChuyenNganh = news.ChuyenNganh;
            old.MaTrinhDoChinhTri = news.MaTrinhDoChinhTri;
            old.MaTrinhDoHocVan = news.MaTrinhDoHocVan;
            old.ChiHoiDanCu_CHP = news.ChiHoiDanCu_CHP;
            old.ChiHoiDanCu_CHT = news.ChiHoiDanCu_CHT;
            return old;
        }
        #endregion Import Excel 
        #region Export 
        [HoiNongDanAuthorization]
        public IActionResult ExportCreate()
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, urlFileMau);
            List<CanBoChiHoiNongDanDetailVM> data = new List<CanBoChiHoiNongDanDetailVM>();
            return Export(data: data, url: url, startIndex: startIndex);
        }
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit(CanBoSearchVM search)
        {
            var model = LoadData(search);
            int stt = 1;
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, urlFileMau);
            return Export(model, url, startIndex);
        }
        public IActionResult Export(List<CanBoChiHoiNongDanDetailVM> data, string url, int startIndex)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "IDCanBo", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DonVi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinh_Nam", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinh_Nu", isAllowedToEdit = true, isDateTime = true });


            var danToc = _context.DanTocs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaDanToc, Name = x.TenDanToc }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaDanToc", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = danToc, TypeId = ConstExcelController.StringId });

            var tonGiao = _context.TonGiaos.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTonGiao, Name = x.TenTonGiao }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTonGiao", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tonGiao, TypeId = ConstExcelController.StringId });

            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay", isAllowedToEdit = true, isText = true });

            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoDangDuBi", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoDangChinhThuc", isAllowedToEdit = true, isDateTime = true });

            var trinhDoHocVan = _context.TrinhDoHocVans.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoHocVan, Name = x.TenTrinhDoHocVan }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoHocVan", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = trinhDoHocVan, TypeId = ConstExcelController.StringId });

            columns.Add(new ExcelTemplate() { ColumnName = "ChuyenNganh", isAllowedToEdit = true, isText = true });

            var chinhTri = _context.TrinhDoChinhTris.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChinhTri, Name = x.TenTrinhDoChinhTri }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChinhChi", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chinhTri, TypeId = ConstExcelController.StringId });

            columns.Add(new ExcelTemplate() { ColumnName = "ChiHoiDanCu_CHT", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChiHoiDanCu_CHP", isAllowedToEdit = true, isText = true });
            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, "CanBoHoiHoiNongDanHuyenQuan") + " " + DateTime.Now.Year.ToString();
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
        public CanBoChiHoiNongDanExelVM CheckTemplate1(object[] row)
        {
            CanBoChiHoiNongDanExelVM data = new CanBoChiHoiNongDanExelVM();
            CanBo canBo = new CanBo();
            canBo.MaTinhTrang = "01";
            canBo.IsCanBo = true;
            canBo.Actived = true;
            canBo.Level = "40";
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
                            if (String.IsNullOrWhiteSpace(value))
                            {
                                data.Error += string.Format("Chưa nhập thông tin Họ tên ở dòng số {0} !", index);
                            }
                            else
                                canBo.HoVaTen = value;
                            break;
                        case 3:
                            //  chức vụ (*) - Tên
                            if (String.IsNullOrWhiteSpace(value))
                            {
                                data.Error += string.Format("Chưa nhập thông Chức vụ (ấp/khu phố, xã/phường)  dòng số {0} !", index);
                            }
                            else
                                canBo.DonVi = value;

                            break;
                        case 4:
                            //  Ngày sinh - nam (*)
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                canBo.NgaySinh = value;
                                canBo.GioiTinh = GioiTinh.Nam;
                            }
                            break;
                        case 5:
                            // giới tính
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                canBo.GioiTinh = GioiTinh.Nữ;
                                canBo.NgaySinh = value;
                            }
                            break;
                        case 6:
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
                        case 7:
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
                        case 8:
                            // Chổ ở hiện nay
                            canBo.ChoOHienNay = value;
                            break;

                        case 9:
                            // Ngày vào đảng dự bị
                            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                canBo.NgayvaoDangDuBi = value.Replace("y", "");

                            }
                            break;
                        case 10:
                            // Ngày vào đảng chính thức
                            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                try
                                {
                                    //canBo.NgayVaoDangChinhThuc = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US")); ;
                                    canBo.NgayVaoDangChinhThuc = value.Replace("y", "");
                                    canBo.DangVien = true;
                                }
                                catch (Exception)
                                {

                                    data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayVaoDangChinhThuc, value, index);
                                }
                            }
                            break;
                        case 11:
                            // Chuyên ngành
                            if (!string.IsNullOrEmpty(value))
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
                        case 12:
                            // Chuyên ngành
                            canBo.ChuyenNganh = value;
                            break;
                        case 13:
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
                        case 14:
                            //  Thời gian chuyển công tác/nghỉ hưu
                            canBo.ChiHoiDanCu_CHT = value;
                            break;
                        case 15:
                            //  Chức vụ mới
                            canBo.ChiHoiDanCu_CHP = value;
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
            return data;
        }

        [NonAction]
        private List<CanBoChiHoiNongDanDetailVM> LoadData(CanBoSearchVM search)
        {
            var model = _context.CanBos.Where(it => it.Level == "40" &&  it.IsCanBo == true).AsQueryable();
           
            if (!String.IsNullOrEmpty(search.HoVaTen))
            {
                model = model.Where(it => it.HoVaTen.Contains(search.HoVaTen));
            }
            if (search.Actived != null)
            {
                model = model.Where(it => it.Actived == search.Actived);
            }

            var data = model
                .Include(it => it.DanToc)
                .Include(it => it.TonGiao)
                .Include(it => it.TrinhDoChinhTri)
                .Include(it => it.TrinhDoHocVan)
                .Include(it => it.CoSo).Select(it => new CanBoChiHoiNongDanDetailVM
                {
                    IDCanBo = it.IDCanBo,
                    HoVaTen = it.HoVaTen,
                    DonVi = it.DonVi,
                    NgaySinh_Nam = it.GioiTinh == GioiTinh.Nam?it.NgaySinh:"",
                    NgaySinh_Nu = it.GioiTinh == GioiTinh.Nữ?it.NgaySinh:"",
                    MaDanToc = it.DanToc!.TenDanToc,
                    MaTonGiao = it.TonGiao!.TenTonGiao,
                    ChoOHienNay = it.ChoOHienNay!,
                    NgayVaoDangDuBi = it.NgayvaoDangDuBi,
                    NgayVaoDangChinhThuc = it.NgayVaoDangChinhThuc,
                    MaTrinhDoHocVan = it.TrinhDoHocVan.TenTrinhDoHocVan,
                    ChuyenNganh = it.ChuyenNganh,
                    MaTrinhDoChinhChi = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                    ChiHoiDanCu_CHT = it.ChiHoiDanCu_CHT,
                    ChiHoiDanCu_CHP = it.ChiHoiDanCu_CHP
                }).ToList();    
            return data;
        }
        #endregion Export
        #region Helper
        private void CreateViewbag(string? maTrinhDoChinhTri = null,string? maDanToc = null,string? maTonGiao = null,string? maTrinhDoHocVan = null) {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaTrinhDoHocVan = fnViewBag.TrinhDoHocVan(value: maTrinhDoHocVan);

            ViewBag.MaTrinhDoChinhTri = fnViewBag.TrinhDoChinhTri(value: maTrinhDoChinhTri);

            ViewBag.MaDanToc = fnViewBag.DanToc(value: maDanToc);

            ViewBag.MaTonGiao = fnViewBag.TonGiao(value: maTonGiao);
        }
        #endregion Helper
    }
}
