using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Transactions;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class CBBanChapHanhController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        const string controllerCode = ConstExcelController.CanBo;
        const int startIndex = 5;
        private string urlFileMau = @"upload\filemau\CanBoBCHHNDTP.xlsx";
        public CBBanChapHanhController(AppDbContext context, IWebHostEnvironment hostEnvironment) :base(context) {
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
        public string ExecuteImportExcelMenu(CBBanChapHanhImportExcel canBoExcel)
        {
            if (canBoExcel.isNullValueId == true)
            {
                canBoExcel.CanBo.IdDepartment = Guid.Parse("EF519606-B0F2-4F5F-AA43-1A824224D5F0");
                canBoExcel.CanBo.CreatedAccountId = AccountId();
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
        private CanBo EditCanBoHoiNongDanThanhPho(CanBo old, CanBo news) {
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
            old.SoDienThoai = news.SoDienThoai;
            old.HoTroKhac = news.HoTroKhac;
            old.HoTrovayVon = news.HoTrovayVon;
            return old;
        }
        #endregion Import Excel 
        #region Export 
        [HoiNongDanAuthorization]
        public IActionResult ExportCreate()
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, urlFileMau);
            List<CBBanChapHanhExcel> data = new List<CBBanChapHanhExcel>();
            return Export(data: data, url: url, startIndex: startIndex);
        }
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit(CanBoSearchVM search)
        {
            var model = LoadData(search);
            int stt = 1;
            model.ForEach(item => {
                item.STT = stt;
                stt++;
            });
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, urlFileMau);
            return Export(model, url, startIndex);
        }
        public IActionResult Export(List<CBBanChapHanhExcel> data, string url, int startIndex)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "IDCanBo", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            var chuVu = _context.ChucVus.ToList().Select(x => new DropdownModel { Id = x.MaChucVu, Name = x.TenChucVu }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenChucVu", isAllowedToEdit = true, isDropdownlist = true, DropdownData = chuVu, TypeId = ConstExcelController.GuidId });
            columns.Add(new ExcelTemplate() { ColumnName = "DonVi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinh_Nam", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinh_Nu", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoQuyetDinhBoNhiem", isAllowedToEdit = true, isText = true });

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
            columns.Add(new ExcelTemplate() { ColumnName = "TenTrinhDoChinhTri", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chinhTri, TypeId = ConstExcelController.StringId });

            columns.Add(new ExcelTemplate() { ColumnName = "SoDienThoai", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ThoiGianChuyenCogTac", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChucVuMoi", isAllowedToEdit = true, isText = true });
            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, "CanBoBanChapHanh") + " " + DateTime.Now.Year.ToString();
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
        public CBBanChapHanhImportExcel CheckTemplate1(object[] row)
        {
            CBBanChapHanhImportExcel data = new CBBanChapHanhImportExcel();
            CanBo canBo = new CanBo();
            canBo.MaTinhTrang = "01";
            canBo.MaPhanHe = "01";
            canBo.IsCanBo = true;
            canBo.Actived = true;
            canBo.IsBanChapHanh = true;
            canBo.Level = "15";
            canBo.IDCanBo = Guid.NewGuid();

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
                            canBo.HoVaTen = value;
                            break;
                        case 3:
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
                        case 4:
                            // Chức vụ - dong vi
                            canBo.DonVi = value;
                            break;
                        case 5:
                            //  Ngày sinh - nam (*)
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                canBo.NgaySinh = value;
                                canBo.GioiTinh = GioiTinh.Nam;
                            }
                            break;
                        case 6:
                            // giới tính
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                canBo.GioiTinh = GioiTinh.Nữ;
                                canBo.NgaySinh = value;
                            }
                            break;
                        case 7:
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
                        case 8:
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
                        case 9:
                            // Nơi sinh
                            canBo.NoiSinh = value;
                            break;
                        case 10:
                            // Chổ ở hiện nay
                            canBo.ChoOHienNay = value;
                            break;

                        case 11:
                            // Ngày vào đảng dự bị
                            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                canBo.NgayvaoDangDuBi = value.Replace("y", "");

                            }
                            break;
                        case 12:
                            // Ngày vào đảng chính thức
                            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                try
                                {
                                    //canBo.NgayVaoDangChinhThuc = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US")); ;
                                    canBo.NgayVaoDangChinhThuc = value.Replace("y","");
                                    canBo.DangVien = true;
                                }
                                catch (Exception)
                                {

                                    data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayVaoDangChinhThuc, value, index);
                                }
                            }
                            break;
                        case 13:
                            // Chuyên ngành
                            canBo.ChuyenNganh = value;

                            break;
                        case 14:
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
                        case 15:
                            //  Số điện thoại (*)
                            if (!string.IsNullOrEmpty(value))
                            {
                                canBo.SoDienThoai = value;
                            }
                            break;
                        case 16:
                            //  Thời gian chuyển công tác/nghỉ hưu
                            canBo.HoTroKhac = value;
                            break;
                        case 17:
                            //  Chức vụ mới
                            canBo.HoTrovayVon = value;
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
        private List<CBBanChapHanhExcel> LoadData(CanBoSearchVM search) {
            var model = _context.CanBos.Where(it => it.IsBanChapHanh == true).AsQueryable();
            if (!String.IsNullOrEmpty(search.MaCanBo))
            {
                model = model.Where(it => it.MaCanBo == search.MaCanBo);
            }
            if (!String.IsNullOrEmpty(search.HoVaTen))
            {
                model = model.Where(it => it.HoVaTen.Contains(search.HoVaTen));
            }
            //if (!String.IsNullOrEmpty(search.NhiemKy))
            //{
            //    model = model.Where(it => it.HoVaTen.Contains(search.HoVaTen));
            //}
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
            model = model.Where(it => it.IsBanChapHanh == true && it.IsCanBo == true);
            var data = model.Include(it => it.TinhTrang)
                .Include(it => it.DanToc)
                .Include(it => it.TonGiao)
                .Include(it => it.TrinhDoChinhTri)
                .Include(it => it.TrinhDoChinhTri)
                .Include(it => it.CoSo).Select(it => new CBBanChapHanhExcel
                {
                    IDCanBo = it.IDCanBo,
                    HoVaTen = it.HoVaTen,
                    TenChucVu = it.ChucVu!.TenChucVu,
                    DonVi = it.DonVi!,
                    NgaySinh_Nam = it.GioiTinh == GioiTinh.Nam ? it.NgaySinh! : "",
                    NgaySinh_Nu = it.GioiTinh == GioiTinh.Nữ ? it.NgaySinh! : "",
                    TenDanToc = it.DanToc!.TenDanToc,
                    TenTonGiao = it.TonGiao!.TenTonGiao,
                    NoiSinh = it.NoiSinh,
                    ChoOHienNay = it.ChoOHienNay,
                    NgayvaoDangDuBi = it.NgayvaoDangDuBi,
                    NgayVaoDangChinhThuc = it.NgayVaoDangChinhThuc,
                    ChuyenNganh = it.ChuyenNganh!,
                    TenTrinhDoChinhTri = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                    SoDienThoai = it.SoDienThoai,
                    ThoiGianChuyenCogTac = it.HoTroKhac,
                    ChucVuMoi = it.HoTrovayVon
                }).ToList();
            return data;
        }
        #endregion Export
    }
}
