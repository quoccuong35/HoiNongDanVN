using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.ViewModels.HoiVien;
using HoiNongDan.Resources;
using HoiNongDan.Web.Areas.NhanSu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class DanhGiaHVController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        const string controllerCode = ConstExcelController.HoiVien;
        private string url = @"upload\filemau\MauDanhGiaHoiVien.xlsx";
        public DanhGiaHVController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config, IHttpContextAccessor httpContext) : base(context)
        {
            _hostEnvironment = hostEnvironment;
            _httpContext = httpContext;
        }
        const int startIndex = 8;
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            DanhGiaHVSearchVM model = new DanhGiaHVSearchVM();
            model.Nam = DateTime.Now.Year;
            CreateViewBag();
            return View(model);
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(DanhGiaHVSearchVM search)
        {
            var model = LoadDataDetail(search);
            return PartialView(model);
        }
        #region Import
        [HoiNongDanAuthorization]
        public IActionResult _Import()
        {
            CreateViewBag();
            return PartialView();
        }
        [HoiNongDanAuthorization]
        public IActionResult Import(Guid? MaDiaBanHoiVien, String? MaQuanHuyen, int? Nam)
        {
            if (MaDiaBanHoiVien == null)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = false,
                    Data = "Chưa chọn hội nông dân đăng ký"
                });
            }
            if (Nam == null)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = false,
                    Data = "Chưa chọn năm đánh giá"
                });
            }
           
            DataSet ds = GetDataSetFromExcel();
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                List<string> errorList = new List<string>();
                List<Guid> nguoiDuyets = new List<Guid>();

                return ExcuteImportExcel(() =>
                {
                    const TransactionScopeOption opt = new TransactionScopeOption();

                    TimeSpan span = new TimeSpan(0, 0, 30, 30);
                    using (TransactionScope ts = new TransactionScope(opt, span))
                    {
                        List<String> error = new List<String>();
                        int iCapNhat = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            if (dt.Rows.IndexOf(row) >= startIndex - 1)
                            {

                                if (row[0] == null || String.IsNullOrWhiteSpace(row[0].ToString()))
                                    break;
                                error = new List<String>();
                                var data = CheckTemplate(row.ItemArray!, error);
                                if (error.Count > 0)
                                {
                                    errorList.AddRange(error);
                                }
                                else
                                {
                                    string result = ExecuteImportExcel(data, Nam.Value);
                                    if (result != LanguageResource.ImportSuccess)
                                    {
                                        errorList.Add(result);
                                    }
                                    else iCapNhat++;

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
                            Data = LanguageResource.ImportSuccess + " " + iCapNhat
                        }); ;
                    }

                });
            }
            else
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                    Data = "Không đọc được file excel"
                });
            }

        }
        private DataSet GetDataSetFromExcel()
        {

            try
            {
                DataSet ds = new DataSet();
                var file = Request.Form.Files[0];
                if (file != null && file.Length > 0)
                {
                    //Check file is excel
                    //Notes: Châu bổ sung .xlsb
                    if (file.FileName.Contains("xls") || file.FileName.Contains("xlsx"))
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        var fileName = Path.GetFileName(file.FileName);
                        var mapPath = Path.Combine(wwwRootPath, @"upload\excel");
                        var path = Path.Combine(mapPath, fileName);
                        if (!Directory.Exists(mapPath))
                        {
                            Directory.CreateDirectory(mapPath);
                        }
                        try
                        {
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
                        }
                        catch
                        {
                            return null!;
                        }
                        finally
                        {
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
        private DanhGiaHVVM CheckTemplate(object[] row, List<String> error)
        {
            DanhGiaHVVM data = new DanhGiaHVVM();
           
            int index = 0,isoloai = 0; string value;
            for (int i = 0; i < row.Length; i++)
            {
                value = row[i] == null ? "" : row[i].ToString()!.Trim();
                switch (i)
                {
                    case 0:
                        // stt
                        index = int.Parse(value);
                        break;
                    case 1:
                        //
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                var checkHV = _context.CanBos.SingleOrDefault(it => it.IDCanBo == Guid.Parse(value));
                                if (checkHV == null)
                                {
                                    error.Add($"Dòng {index} Không tìm thấy thông tin hội viên");
                                }
                                else
                                    data.ID = checkHV!.IDCanBo; ;
                            }
                            catch
                            {
                                error.Add($"Dòng {index} có ID không hợp lệ");
                            }
                        }
                        else
                        {
                            error.Add($"Dòng {index} có ID không hợp lệ");
                        }
                        break;
                    case 2:
                        //SoTheHoiVien
                       if(!String.IsNullOrWhiteSpace(value))
                            data.SoTheHoiVien = value;
                        break;
                    case 3:
                        //Họ Và Tên
                        if (!String.IsNullOrWhiteSpace(value))
                            data.HoVaTen = value;
                        break;
                    case 4:
                        //Ngày tháng năm sinh - nữ
                        if (!String.IsNullOrWhiteSpace(value))
                            data.NgaySinhNam = value;
                        break;
                    case 5:
                        if (!String.IsNullOrWhiteSpace(value))
                            data.NgaySinhNu = value;
                        break;
                    case 6:
                        //ngay cấp CMND/CCCD
                        if (!String.IsNullOrWhiteSpace(value))
                            data.SoCCCD = value;
                        break;
                    case 7:
                        //Hộ khẩu thường trú
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.PhanLoai_XuatSac = value;
                            isoloai++;
                            data.SoTheHoiVien = "01";
                        }
                        break;
                    case 8:
                        //Nơi ở hiện nay
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.PhanLoai_HTTot = value;
                            isoloai++;
                            data.SoTheHoiVien = "02" ;
                        }
                            
                        break;
                    case 9:
                        //Số điện thoại
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.PhanLoai_HTNhiemVu = value;
                            isoloai++;
                            data.SoTheHoiVien = "03";
                        }
                        break;
                    case 10:
                        //Nơi ở hiện nay
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.PhanLoai_KhongHoanThanhNhiemVu = value;
                            isoloai++;
                            data.SoTheHoiVien = "04";
                        }
                        break;
                    case 11:
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.KhongPhanLoai = value;
                            isoloai++;
                            data.SoTheHoiVien = "05";
                        }
                        break;
                    case 12:
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.ChuaDuDieuKienPhanLoai = value;
                            isoloai++;
                            data.SoTheHoiVien = "06";
                        }
                        break;
                    case 13:
                        if (!String.IsNullOrWhiteSpace(value))
                            data.GhiChu = value;
                        break;

                }
            }
            if (isoloai !=1)
            {
                error.Add($"Dòng {index} có phân loại không hợp lệ");
            }
            return data;
        }
        private string ExecuteImportExcel(DanhGiaHVVM danhGiaHV, int Nam)
        {
            var checkExit = _context.DanhGiaHoiViens.SingleOrDefault(it => it.IDHoiVien == danhGiaHV.ID && it.Nam == Nam);
            if (checkExit != null)
            {
                checkExit.LoaiDanhGia = danhGiaHV.SoTheHoiVien;
                checkExit.GhiChu = danhGiaHV.GhiChu;
                checkExit.LastModifiedAccountId = AccountId();
                checkExit.LastModifiedTime = DateTime.Now;
            }
            else
            {
                DanhGiaHoiVien add = new DanhGiaHoiVien();
                add.LoaiDanhGia = danhGiaHV.SoTheHoiVien;
                add.GhiChu = danhGiaHV.GhiChu;
                add.ID = Guid.NewGuid();
                add.Nam = Nam;
                add.IDHoiVien = danhGiaHV.ID;
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                _context.DanhGiaHoiViens.Add(add);
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.InnerException!.Message + " " + danhGiaHV.HoVaTen;
            }
            return LanguageResource.ImportSuccess;
        }
        #endregion Import 
        #region Export
        public IActionResult ExportEdit(DanhGiaHVSearchVM search) {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, this.url);
            var model = LoadData(search);
            return Export(model, url, startIndex); ;
        }
        private FileContentResult Export(List<DanhGiaHVVM> data, string url, int startIndex)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "ID", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoTheHoiVien", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinhNam", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinhNu", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoCCCD", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "PhanLoai_XuatSac", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "PhanLoai_HTTot", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "PhanLoai_HTNhiemVu", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "PhanLoai_KhongHoanThanhNhiemVu", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "KhongPhanLoai", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChuaDuDieuKienPhanLoai", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GhiChu", isAllowedToEdit = true, isText = true });
            
            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.DanhGiaHV);
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
            byte[] filecontent = ClassExportExcel.ExportExcel(url, data, columns, heading, true, startIndex);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export
        #region Helper
        private void CreateViewBag() {
            FnViewBag fnViewBag = new FnViewBag(_context);

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }
        private List<DanhGiaHVVM> LoadData(DanhGiaHVSearchVM search) { 
            var canBo =_context.CanBos.Include(it=>it.DiaBanHoatDong).ThenInclude(it=>it.QuanHuyen).Include(it=>it.DanhGiaHoiViens).AsQueryable();
            search.Nam = search.Nam != null ? search.Nam : DateTime.Now.Year;
            if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen))
            {
                canBo = canBo.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
            }
            else
            {
                canBo = canBo.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == "wewewew");
            }
            if (search.MaDiaBanHoiVien !=null)
            {
                canBo = canBo.Where(it => it.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
            }

            if (search.Nam != null && search.Loai =="02")
            {
                canBo = canBo.Where(it => it.DanhGiaHoiViens.Any(p=>p.Nam == search.Nam));
            }
            if (search.Nam != null && search.Loai == "03")
            {
                canBo = canBo.Where(it => !it.DanhGiaHoiViens.Any(p => p.Nam == search.Nam));
            }
            canBo = canBo.Where(it => it.IsHoiVien == true && it.Actived == true && it.HoiVienDuyet == true && it.isRoiHoi != true);
            var model = canBo.Select(it => new DanhGiaHVVM { 
                ID = it.IDCanBo,
                SoTheHoiVien = it.MaCanBo,
                HoVaTen = it.HoVaTen,
                NgaySinhNam = it.GioiTinh == GioiTinh.Nam?it.NgaySinh:"",
                NgaySinhNu = it.GioiTinh == GioiTinh.Nữ?it.NgaySinh:"",
                SoCCCD = it.SoCCCD,
                PhanLoai_XuatSac = it.DanhGiaHoiViens.Where(it=>it.LoaiDanhGia =="01" && it.Nam == search.Nam).Count()>0?"X":"",
                PhanLoai_HTTot = it.DanhGiaHoiViens.Where(it => it.LoaiDanhGia == "02" && it.Nam == search.Nam).Count() > 0 ? "X" : "",
                PhanLoai_HTNhiemVu = it.DanhGiaHoiViens.Where(it => it.LoaiDanhGia == "03" && it.Nam == search.Nam).Count() > 0 ? "X" : "",
                PhanLoai_KhongHoanThanhNhiemVu = it.DanhGiaHoiViens.Where(it => it.LoaiDanhGia == "04" && it.Nam == search.Nam).Count() > 0 ? "X" : "",
                KhongPhanLoai = it.DanhGiaHoiViens.Where(it => it.LoaiDanhGia == "05" && it.Nam == search.Nam).Count() > 0 ? "X" : "",
                ChuaDuDieuKienPhanLoai = it.DanhGiaHoiViens.Where(it => it.LoaiDanhGia == "06" && it.Nam == search.Nam).Count() > 0 ? "X" : "",
                //GhiChu = it.GhiChu
            }).ToList();
            return model;
        }
        private List<DanhGiaHVDetailVM> LoadDataDetail(DanhGiaHVSearchVM search)
        {
            var canBo = _context.CanBos.Include(it => it.DiaBanHoatDong).ThenInclude(it => it.QuanHuyen).Include(it => it.DanhGiaHoiViens).AsQueryable();
            search.Nam = search.Nam != null ? search.Nam : DateTime.Now.Year;
            if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen))
            {
                canBo = canBo.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
            }
            else
            {
                canBo = canBo.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == "wewewew");
            }
            if (search.MaDiaBanHoiVien != null)
            {
                canBo = canBo.Where(it => it.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
            }

            if (search.Nam != null && search.Loai == "02")
            {
                canBo = canBo.Where(it => it.DanhGiaHoiViens.Any(p => p.Nam == search.Nam));
            }
            if (search.Nam != null && search.Loai == "03")
            {
                canBo = canBo.Where(it => !it.DanhGiaHoiViens.Any(p => p.Nam == search.Nam));
            }
            canBo = canBo.Where(it => it.IsHoiVien == true && it.Actived == true && it.HoiVienDuyet == true && it.isRoiHoi != true);
            var model = canBo.Select(it => new DanhGiaHVDetailVM
            {
                IDDanhGia = it.DanhGiaHoiViens.SingleOrDefault(p=>p.Nam == search.Nam && p.IDHoiVien == it.IDCanBo).ID,
                ID = it.IDCanBo,
                SoTheHoiVien = it.MaCanBo,
                HoVaTen = it.HoVaTen,
                NgaySinhNam = it.GioiTinh == GioiTinh.Nam ? it.NgaySinh : "",
                NgaySinhNu = it.GioiTinh == GioiTinh.Nữ ? it.NgaySinh : "",
                SoCCCD = it.SoCCCD,
                PhanLoai_XuatSac = it.DanhGiaHoiViens.Where(it => it.LoaiDanhGia == "01" && it.Nam == search.Nam).Count() > 0 ? "X" : "",
                PhanLoai_HTTot = it.DanhGiaHoiViens.Where(it => it.LoaiDanhGia == "02" && it.Nam == search.Nam).Count() > 0 ? "X" : "",
                PhanLoai_HTNhiemVu = it.DanhGiaHoiViens.Where(it => it.LoaiDanhGia == "03" && it.Nam == search.Nam).Count() > 0 ? "X" : "",
                PhanLoai_KhongHoanThanhNhiemVu = it.DanhGiaHoiViens.Where(it => it.LoaiDanhGia == "04" && it.Nam == search.Nam).Count() > 0 ? "X" : "",
                KhongPhanLoai = it.DanhGiaHoiViens.Where(it => it.LoaiDanhGia == "05" && it.Nam == search.Nam).Count() > 0 ? "X" : "",
                ChuaDuDieuKienPhanLoai = it.DanhGiaHoiViens.Where(it => it.LoaiDanhGia == "06" && it.Nam == search.Nam).Count() > 0 ? "X" : "",
                //GhiChu = it.GhiChu
            }).ToList();
            return model;
        }
        #endregion Helper

        #region Del
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.DanhGiaHoiViens.SingleOrDefault(it => it.ID == id);

                if (del != null)
                {
                    //_context.Entry(accountInRoleModels).State = EntityState.Deleted;
                    //_context.Entry(account).State = EntityState.Deleted;
                   
                    _context.DanhGiaHoiViens.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.DanhGiaHV.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.DanhGiaHV.ToLower())
                    });
                }
            });
        }
        #endregion Del
    }
}
