using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NuGet.Packaging;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Transactions;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class DuyetHoiVienController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 10;
        private string[] DateFomat;
        public DuyetHoiVienController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config, IHttpContextAccessor httpContext) : base(context) {
            _hostEnvironment = hostEnvironment;
            _httpContext = httpContext;
            DateFomat = config.GetSection("SiteSettings:DateFormat").Value.ToString().Split(',');
        }
        #region index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBagSearch();
            DuyetHoiVienSearchVM searchVM = new DuyetHoiVienSearchVM();
            DateTime tuNgay = DateTime.Now.AddMonths(-2);

            searchVM.TuNgay = new DateTime(tuNgay.Year, tuNgay.Month, 01);
            searchVM.DenNgay = DateTime.Now.Date;
            var ketQua = _context.GetMaxMaHoiViens.FromSqlRaw("EXEC  dbo.GetMaxMaHoiVien").ToList();
            if (ketQua.Count > 0)
            {
                ViewBag.SoThe = ketQua[0].SoThe +1;
            }
            return View(searchVM);
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(DuyetHoiVienSearchVM searchVM)
        {
            return ExecuteSearch(() => {
               var model = LoadData(searchVM)
                    .OrderBy(it => it.TenQuanHuyen).ThenBy(it => it.TenHoi).ThenBy(it => it.NgayThangVaoHoi).ThenBy(it => it.SoQuyetDinh).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Update duyệt hội viên
        [HoiNongDanAuthorization]
        public IActionResult View(Guid id)
        {
            var hoivien = _context.CanBos.Where(it => it.IDCanBo == id && it.HoiVienDuyet == false && it.TuChoi == false).Include(it => it.TinhTrang)
                    .Include(it => it.DiaBanHoatDong)
                    .ThenInclude(it=>it!.QuanHuyen)
                    .Include(it => it.DanToc)
                    .Include(it => it.TonGiao)
                    .Include(it => it.TrinhDoHocVan)
                    .Include(it => it.TrinhDoChuyenMon)
                    .Include(it => it.TrinhDoChinhTri)
                    .Include(it=>it.NgheNghiep)
                    .Include(it => it.CoSo).Select(it => new HoiVienDuyetCapNhatVM
                    {
                        MaCanBo = it.MaCanBo,
                        IDCanBo = it.IDCanBo,
                        HoVaTen = it.HoVaTen,
                        NgaySinh = it.NgaySinh,
                        GioiTinh = (GioiTinh)it.GioiTinh,
                        SoCCCD = it.SoCCCD!,
                        NgayCapCCCD = it.NgayCapCCCD,
                        HoKhauThuongTru = it.HoKhauThuongTru,
                        SoQuyetDinh = it.SoQuyetDinhBoNhiem,
                        NgayVaoHoi = it.NgayVaoHoi,
                        ChoOHienNay = it.ChoOHienNay!,
                        SoDienThoai = it.SoDienThoai,
                        DanToc = it.DanToc!.TenDanToc,
                        TonGiao = it.TonGiao!.TenTonGiao,
                        TrinhDoHocvan = it.TrinhDoHocVan.TenTrinhDoHocVan,
                        TrinhDoChuyenMon = it.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                        TrinhDoChinhChi = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                        TenDiaBanHoatDong = it.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen + "-"+ it.DiaBanHoatDong.TenDiaBanHoatDong,
                        NgheNghiepHienNay = it.MaNgheNghiep!,
                        HoiVienDanCu = it.HoiVienDanCu== null?false:it.HoiVienDanCu.Value,
                        DangVien = it.DangVien == null ? false : it.DangVien.Value,
                        HoiVienNganhNghe = it.HoiVienNganhNghe == null ? false : it.HoiVienNganhNghe.Value,

                    }).SingleOrDefault();
            if (hoivien == null)
            {

                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            dynamic model = new ExpandoObject();
            HoiVienDuyetCapNhatVM duyet = new HoiVienDuyetCapNhatVM();
            duyet.ID = hoivien.IDCanBo!.Value;
            model.hv = hoivien;
            model.duyet = duyet;
            return View(hoivien);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // edit từng người ở cấp thành phố
        public IActionResult Upsert(HoiVienDuyetCapNhatVM hv) {
            var editCanBo = _context.CanBos.SingleOrDefault(it => it.IDCanBo == hv.ID && it.HoiVienDuyet == false && it.TuChoi == false);
            if (editCanBo == null)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.DuyetHoiVienMoi.ToLower())
                });
            }
            else
            {
                var capNhatDaDuyet = _context.HoiVienLichSuDuyets.Where(it => it.IDHoiVien == editCanBo!.IDCanBo && it.TrangThaiDuyet == false && it.AccountID  == AccountId());
                if (capNhatDaDuyet.Count() > 0)
                {
                    foreach (var item1 in capNhatDaDuyet)
                    {
                        item1.AccountIDDuyet = AccountId();
                        item1.AccountIDDuyetTime = DateTime.Now;
                        item1.TrangThaiDuyet = true;
                    }
                }
                editCanBo.HoiVienDuyet = true;
                editCanBo.NgayDuyet = DateTime.Now;
                editCanBo.NguoiDuyet = AccountId();
                editCanBo.MaCanBo = hv.SoTheHoiVien;
                editCanBo.NgayVaoHoi = hv.NgayVaoHoi;
                editCanBo.SoQuyetDinhBoNhiem = hv.SoQuyetDinh;
                editCanBo.NgayCapThe = hv.NgayCapThe;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = "Duyệt thành công"
                }); 
            }
        }
        // Duyệt tự động
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DuyetTuDong(List<Guid> lid, string? SoQuyetDinh, DateTime? NgayQuyetDinh, DateTime NgayCapThe,decimal SoThe)
        {
            const TransactionScopeOption opt = new TransactionScopeOption();
            TimeSpan span = new TimeSpan(0, 0, 30, 30);
            List<String> error = new List<string>();
            var ketQua = _context.GetMaxMaHoiViens.FromSqlRaw("EXEC  dbo.GetMaxMaHoiVien").ToList();
            if (ketQua.Count > 0)
            {
                if ((ketQua[0].SoThe + 1) != SoThe)
                {
                    error.Add("Số thẻ đã thay đổi nhấn F5 để lấy thông tin số thể mới");
                }
               
            }
            //using (var transaction = _context.Database.BeginTransaction()) {
            //    try
            //    {

            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //}
            using (TransactionScope ts = new TransactionScope(opt, span))
            {
                try
                {
                    int nam = NgayCapThe.Year;
                    string soCuoiNam = nam.ToString().Substring(nam.ToString().Length - 2).ToString();
                    foreach (var id in lid)
                    {
                        var editCanBo = _context.CanBos.SingleOrDefault(it => it.IDCanBo == id && it.HoiVienDuyet == false && it.TuChoi == false);
                        if (editCanBo == null)
                        {
                            error.Add(string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.DuyetHoiVienMoi.ToLower() + "ID " + id.ToString()));
                        }
                        else
                        {
                            var capNhatDaDuyet = _context.HoiVienLichSuDuyets.Where(it => it.IDHoiVien == editCanBo!.IDCanBo && it.TrangThaiDuyet == false && it.AccountID == AccountId());
                            if (capNhatDaDuyet.Count() > 0)
                            {
                                foreach (var item1 in capNhatDaDuyet)
                                {
                                    item1.AccountIDDuyet = AccountId();
                                    item1.AccountIDDuyetTime = DateTime.Now;
                                    item1.TrangThaiDuyet = true;
                                }
                            }
                            editCanBo.HoiVienDuyet = true;
                            editCanBo.NgayDuyet = DateTime.Now;
                            editCanBo.NguoiDuyet = AccountId();
                            editCanBo.MaCanBo = soCuoiNam + "." + SoThe.ToString();
                            editCanBo.NgayCapThe = NgayCapThe;
                            if (editCanBo.NgayVaoHoi == null && NgayQuyetDinh == null)
                            {
                                error.Add($"Hội viên {editCanBo.HoVaTen} chưa có thông tin ngày vào hội");
                            }
                            if (editCanBo.NgayVaoHoi == null && NgayQuyetDinh != null)
                            {
                                editCanBo.NgayVaoHoi = NgayQuyetDinh;
                            }
                            if (editCanBo.SoQuyetDinhBoNhiem == null && String.IsNullOrWhiteSpace(SoQuyetDinh))
                            {
                                error.Add($"Hội viên {editCanBo.HoVaTen} chưa có thông tin số quyết định vào hội");
                            }
                            if (editCanBo.SoQuyetDinhBoNhiem == null && !String.IsNullOrWhiteSpace(SoQuyetDinh))
                            {
                                editCanBo.SoQuyetDinhBoNhiem = SoQuyetDinh;
                            }
                            SoThe++;
                        }
                    }
                    if (error.Count > 0)
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = String.Join(", ", error)
                        });
                    }
                    else
                    {
                        int capNhat = _context.SaveChanges();
                        var soTheMoi = _context.GetMaxMaHoiViens.FromSqlRaw("EXEC  dbo.GetMaxMaHoiVien").ToList();
                        ts.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = "Duyệt thành công " + lid.Count().ToString() + " hội viên",
                            sothe = soTheMoi[0].SoThe + 1
                        });
                    }
                }

                catch (Exception ex)
                {
                    //ts.Dispose();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Lỗi hệ thống liên hệ admin " + ex.Message.ToString()
                    });
                }
                
            }
            
        }
        #region Import 
        [HoiNongDanAuthorization]
        // duyệt hội viên cấp thành phố
        public IActionResult _Import()
        {
            return PartialView();
        }

        [HoiNongDanAuthorization]
        // Duyệt cấp thành phố
        public IActionResult Import()
        {
            // duyệt hội viên cấp thành phố
            DataSet ds = GetDataSetFromExcel();
            if (ds != null && ds.Tables.Count > 0)
            {
                var account = _context.Accounts.SingleOrDefault(it => it.AccountId == AccountId());
                DataTable dt = ds.Tables[0];
                bool edit;
                List<string> errorList = new List<string>();
                

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
                                    var editCanBo = _context.CanBos.SingleOrDefault(it => it.IDCanBo == data.IDCanBo && it.HoiVienDuyet == false && it.TuChoi == false);
                                    if (editCanBo == null)
                                    {
                                        errorList.Add("Không tìm thấy thông tin cán bộ có ID " + editCanBo.IDCanBo);
                                    }
                                    else
                                    {
                                        var capNhatDaDuyet = _context.HoiVienLichSuDuyets.Where(it => it.IDHoiVien == editCanBo!.IDCanBo && it.TrangThaiDuyet == false).ToList();
                                        if (capNhatDaDuyet.Count() > 0)
                                        {
                                            var checkExit = capNhatDaDuyet.SingleOrDefault(it => it.AccountID == AccountId());
                                            if (checkExit == null)
                                            {
                                                errorList.Add("Thông tin cán bộ có tên " + editCanBo.HoVaTen + " chưa duyệt cấp quận huyện");
                                            }
                                            else {
                                                capNhatDaDuyet.ForEach(it => {
                                                    it.AccountIDDuyet = AccountId();
                                                    it.AccountIDDuyetTime = DateTime.Now;
                                                    it.TrangThaiDuyet = true;
                                                });
                                            }
                                            
                                        }
                                        editCanBo.MaCanBo = data.MaCanBo;
                                        if (editCanBo.NgayVaoHoi == null && data.NgayVaoHoi == null)
                                        {
                                            errorList.Add($"Thông tin hội viên {editCanBo.HoVaTen} ngày vào hội chưa nhập");
                                        }
                                        if (String.IsNullOrWhiteSpace(editCanBo.SoQuyetDinhBoNhiem) && String.IsNullOrWhiteSpace(data.SoQuyetDinhBoNhiem))
                                        {
                                            errorList.Add($"Thông tin hội viên {editCanBo.HoVaTen} số quyết định chưa nhập");
                                        }
                                        if (editCanBo.NgayVaoHoi == null && data.NgayVaoHoi != null)
                                            editCanBo.NgayVaoHoi = data.NgayVaoHoi;
                                        if (String.IsNullOrWhiteSpace(editCanBo.SoQuyetDinhBoNhiem) && !String.IsNullOrWhiteSpace(data.SoQuyetDinhBoNhiem))
                                            editCanBo.SoQuyetDinhBoNhiem = data.SoQuyetDinhBoNhiem;
                                        editCanBo.NgayCapThe = data.NgayCapThe;
                                        editCanBo.HoiVienDuyet = true;
                                        editCanBo.NgayDuyet = DateTime.Now;
                                        editCanBo.NguoiDuyet = AccountId();
                                        
                                        iCapNhat++;
                                    }
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
                        _context.SaveChanges();
                        ts.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = "Duyệt thành công" + " " + iCapNhat
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
        private CanBo CheckTemplate(object[] row, List<String> error)
        {
            CanBo data = new CanBo();
            data.IDCanBo = Guid.NewGuid();
            data.HoiVienDuyet = false;
            data.AccountIdDangKy = AccountId();
            data.IsHoiVien = true;
            data.TuChoi = false;
            int index = 0; string value;
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
                                data.IDCanBo = Guid.Parse(value);
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
                    case 21:
                        //Số Quyết Định
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            data.SoQuyetDinhBoNhiem = value;
                        }
                        
                        break;
                    case 22:
                        //Ngày quyết định ngày vào hội
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                value = Function.RepleceAllString(value);
                                data.NgayVaoHoi = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US"));
                            }
                            catch (Exception)
                            {

                                error.Add(string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayNangBacLuong, value, index));
                            }
                        }

                        break;
                    case 23:
                        //Ngày tháng năm sinh - Nam
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            data.MaCanBo = value;
                        }
                        else
                        {
                            error.Add($"Dòng {index} chưa nhập số thẻ hội viên");
                        }
                        break;
                    case 24:
                        //Ngày cấp thẻ
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                try
                                {
                                    value = Function.RepleceAllString(value);
                                    data.NgayCapThe = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US"));
                                }
                                catch (Exception)
                                {

                                    error.Add(string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayNangBacLuong, value, index));
                                }
                            }
                            else
                            {
                                error.Add($"Dòng {index} chưa nhập ngày tháng năm cấp thẻ");
                            }
                        }
                        catch
                        {
                            error.Add($"Dòng {index} có Họ và tên không hợp lệ");
                        }
                        break;
                    default:
                        break;
                }
            }
            return data;
        }
        #endregion Import
        #region Export
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit(DuyetHoiVienSearchVM searchVM)
        {
           var model = LoadData(searchVM);

            var data = model.OrderBy(it=>it.NgayThangVaoHoi).ThenBy(it=>it.SoQuyetDinh).Select(it => new HVDangKyDuyetImportVM
            {
                ID = it.ID!.Value,
                HoVaTen = it.HoVaTen,
                Nam = it.Nam,
                Nu = it.Nu,
                SoCCCD = it.SoCCCD,
                NgayCapSoCCCD = it.NgayCapSoCCCD,
                TenQuanHuyen = it.TenQuanHuyen,
                TenHoi = it.TenHoi,
                HoKhauThuongTru = it.HoKhauThuongTru,
                SoQuyetDinh = it.SoQuyetDinh,
                NgayThangVaoHoi = it.NgayQuyetDinh,
                NoiOHiennay = it.NoiOHiennay,
                SoDienThoai = it.SoDienThoai,
                DangVien = it.DangVien,
                MaDanToc = it.DanToc,
                MaTonGiao = it.TonGiao,
                MaTrinhDoHocVan = it.TrinhDoHocVan,
                MaTrinhDoChuyenMon = it.TrinhDoChuyenMon,
                MaTrinhDoChinhTri = it.ChinhTri,
                MaNgheNghiep = it.NgheNghiep,
                DiaBanDanCu = it.DiaBanDanCu,
                NganhNghe = it.NganhNghe,
                SoThe = it.SoThe,
                NgayCapThe = it.NgayCapThe
            }).ToList();
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\HVDuyetNhanh.xlsx");

            return Export(data, url, startIndex); ;
        }
        private FileContentResult Export(List<HVDangKyDuyetImportVM> data, string url, int startIndex)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "ID", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Nam", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Nu", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoCCCD", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayCapSoCCCD", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "TenQuanHuyen", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "TenHoi", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoKhauThuongTru", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NoiOHiennay", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoDienThoai", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DangVien", isComment = true, isAllowedToEdit = false, strComment = "để X là đảng viên" });

      
            columns.Add(new ExcelTemplate() { ColumnName = "MaDanToc", isAllowedToEdit = false, isText = true });

   
            columns.Add(new ExcelTemplate() { ColumnName = "MaTonGiao", isAllowedToEdit = false, isText = true });

      
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoHocVan", isAllowedToEdit = false, isText = true });

    
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChuyenMon", isAllowedToEdit = false, isText = true });

  
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChinhTri", isAllowedToEdit = false, isText = true });

            columns.Add(new ExcelTemplate() { ColumnName = "MaNgheNghiep", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DiaBanDanCu", isAllowedToEdit = false, isText = true, strComment = "Nhập X là hội viên dân cư" });
            columns.Add(new ExcelTemplate() { ColumnName = "NganhNghe", isAllowedToEdit = false, isText = true, strComment = "Nhập X là hội viên ngành nghề" });

            columns.Add(new ExcelTemplate() { ColumnName = "SoQuyetDinh", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayThangVaoHoi", isAllowedToEdit = true, isText = true, strComment = "Nhập ngày tháng năm theo số quyết định" });

            columns.Add(new ExcelTemplate() { ColumnName = "SoThe", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayCapThe", isAllowedToEdit = true, isText = true });

            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.HVDangKy);
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

        [HttpPost]
        // duyệt all hội viên cấp quận huyện
        public IActionResult Edit(List<Guid> lid,string soQuyetDinh, DateTime ngayQuyetDinh) {
            return ExecuteContainer(() => {
                const TransactionScopeOption opt = new TransactionScopeOption();
                TimeSpan span = new TimeSpan(0, 0, 30, 30);
                using (TransactionScope ts = new TransactionScope(opt, span)) {
                    List<string> error = new List<string>();
                    DateTime dateTime = DateTime.Now;
                    var acount = _context.Accounts
                    .SingleOrDefault(it => it.AccountId == AccountId() );
                    List<Guid>? guids = new List<Guid>();
                    if (!String.IsNullOrWhiteSpace(acount!.AccountIDParent!))
                    {
                        string[] sTemp;
                        sTemp = acount.AccountIDParent!.Split(";");
                        foreach (var s in sTemp)
                        {
                            guids.Add(Guid.Parse(s));
                        }
                    }
                    List<HoiVienLichSuDuyet> hoiVienLichSuDuyets = new List<HoiVienLichSuDuyet>();
                    foreach (Guid item in lid) 
                    {
                        hoiVienLichSuDuyets = new List<HoiVienLichSuDuyet>();
                        var edit = _context.CanBos.SingleOrDefault(it => it.IDCanBo == item && it.HoiVienDuyet == false);
                        if(edit == null)
                        {
                            error.Add("Không tìm thấy thông tin duyệt" + item);
                            continue;
                        }
                        if (edit != null && guids.Count() == 0)
                        {
                            edit.HoiVienDuyet = true;
                            edit.NguoiDuyet = AccountId();
                            edit.NgayDuyet = dateTime;
                        }
                        edit!.NgayVaoHoi = ngayQuyetDinh;
                        edit.SoQuyetDinhBoNhiem = soQuyetDinh;
                        var capNhatDaDuyet = _context.HoiVienLichSuDuyets.Where(it => it.IDHoiVien == edit!.IDCanBo && it.TrangThaiDuyet == false).ToList();
                        if (capNhatDaDuyet.Count() > 0) {
                            var checkExist = capNhatDaDuyet.SingleOrDefault(it => it.AccountID == AccountId());
                            if (checkExist == null)
                            {
                                  error.Add("Không tìm thấy thông tin duyệt" + item);
                            }
                            else
                            {
                                capNhatDaDuyet.ForEach(it => {
                                    it.AccountIDDuyet = AccountId();
                                    it.AccountIDDuyetTime = DateTime.Now;
                                    it.TrangThaiDuyet = true;
                                });
                            }
                            
                        }
                        if (guids.Count() > 0)
                        {
                            guids.ForEach(item => {
                                hoiVienLichSuDuyets.Add(new HoiVienLichSuDuyet
                                {
                                    ID = Guid.NewGuid(),
                                    IDHoiVien = edit!.IDCanBo,
                                    AccountID = item,
                                    CreateTime = DateTime.Now,
                                    TrangThaiDuyet = false,
                                });
                            });
                        }
                        if (hoiVienLichSuDuyets.Count > 0)
                        {
                            _context.HoiVienLichSuDuyets.AddRange(hoiVienLichSuDuyets);
                        }
                    }
                    if (error.Count > 0)
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = String.Join(", ", error)
                        });
                    }
                    else
                    {
                        _context.SaveChanges();
                        ts.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = "Duyệt thành công " + lid.Count().ToString() + " hội viên"
                        });
                    }
                }
            });
        }
        #endregion Update duyệt hội viên
        #region Tu chối duyệt hội viên
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HoiNongDanAuthorization]
        public IActionResult TuChoi(List<Guid> lid, string lyDo) {
            const TransactionScopeOption opt = new TransactionScopeOption();

            TimeSpan span = new TimeSpan(0, 0, 30, 30);
            using (TransactionScope ts = new TransactionScope(opt, span))
            {
                List<string> error = new List<string>();
                DateTime dateTime = DateTime.Now;
                foreach (Guid item in lid)
                {
                    var edit = _context.CanBos.SingleOrDefault(it => it.IDCanBo == item && it.HoiVienDuyet == false);
                    var capNhatDaDuyet = _context.HoiVienLichSuDuyets.Where(it => it.IDHoiVien == edit!.IDCanBo && it.TrangThaiDuyet == false).ToList();
                    if (capNhatDaDuyet.Count() > 0)
                    {
                        capNhatDaDuyet.ForEach(it => {
                            it.TrangThaiDuyet = true;
                            it.AccountIDDuyet = AccountId();
                            it.AccountIDDuyetTime = dateTime;
                        });
                    }
                    if (edit != null)
                    {
                        edit.TuChoi = true;
                        edit.AccountIdTuChoi = AccountId();
                        edit.NgayTuChoi = dateTime;
                        edit.LyDoTuChoi = lyDo;
                    }
                    else
                    {
                        error.Add("Không tìm thấy cán bộ có mã" + item);
                    }
                }
                if (error.Count > 0)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = String.Join(", ", error)
                    });
                }
                else
                {
                    _context.SaveChanges();
                    ts.Complete();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = "Từ chối thành công " + lid.Count().ToString() + " hội viên"
                    });
                }
            }
        }
        #endregion Tu chối duyệt hội viên
        #region Helper
        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }
        private List<HVDangKyDuyetDetailVM> LoadData(DuyetHoiVienSearchVM searchVM)
        {
            var data = _context.CanBos
                .Include(it => it.DanToc).Include(it => it.NgheNghiep)
                .Include(it => it.TonGiao).Include(it => it.TrinhDoHocVan)
                .Include(it => it.TrinhDoChuyenMon).Include(it => it.TrinhDoChinhTri)
                .Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen)
                .Join(_context.HoiVienLichSuDuyets.Where(it => it.AccountID == AccountId() && it.TrangThaiDuyet == searchVM.TrangThaiDuyet),
                    hv => hv.IDCanBo,
                    pv => pv.IDHoiVien,
                    (hv, pv) => new { hv ,pv}
                    ).Where(
                        it => it.hv.IsHoiVien == true
                        && it.hv.TuChoi == false
                        && it.hv.isRoiHoi == false
                    ).AsQueryable();
            if (!searchVM.TrangThaiDuyet)
            {
                data = data.Where(it => it.hv.HoiVienDuyet != true);
                if (searchVM.TuNgay != null)
                {
                    data = data.Where(it => it.hv.NgayDangKy >= searchVM.TuNgay.Value.Date);
                }
                if (searchVM.DenNgay != null)
                {
                    data = data.Where(it => it.hv.NgayDangKy <= searchVM.DenNgay.Value.Date);
                }
            }
            else
            {
                data = data.Where(it => it.hv.HoiVienDuyet == true);
                if (searchVM.TuNgay != null)
                {
                    data = data.Where(it => it.hv.NgayDangKy >= searchVM.TuNgay.Value.Date);
                }
                if (searchVM.DenNgay != null)
                {
                    data = data.Where(it => it.hv.NgayDangKy <= searchVM.DenNgay.Value.Date);
                }
            }
            
            if (!String.IsNullOrWhiteSpace(searchVM.MaQuanHuyen))
            {
                data = data.Where(it => it.hv.DiaBanHoatDong!.MaQuanHuyen == searchVM.MaQuanHuyen);
            }
            if (searchVM.MaDiaBanHoiVien != null)
            {
                data = data.Where(it => it.hv.MaDiaBanHoatDong == searchVM.MaDiaBanHoiVien);
            }
            if (!String.IsNullOrWhiteSpace(searchVM.SoQuyetDinh))
            {
                data = data.Where(it => it.hv.SoQuyetDinhBoNhiem == searchVM.SoQuyetDinh);
            }
            if (!String.IsNullOrWhiteSpace(searchVM.SoCCCD))
            {
                data = data.Where(it => it.hv.SoCCCD == searchVM.SoCCCD);
            }
            var model = data.OrderBy(it => it.hv.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen).OrderBy(it => it.hv.MaDiaBanHoatDong).OrderBy(it => it.hv.CreatedTime).Select(it => new HVDangKyDuyetDetailVM
            {
                ID = it.hv.IDCanBo,
                HoVaTen = it.hv.HoVaTen,
                Nam = (int)it.hv.GioiTinh == 1 ? it.hv.NgaySinh : "",
                Nu = (int)it.hv.GioiTinh == 0 ? it.hv.NgaySinh : "",
                SoCCCD = it.hv.SoCCCD,
                NgayCapSoCCCD = it.hv.NgayCapCCCD,
                TenQuanHuyen = it.hv.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen,
                TenHoi = it.hv.DiaBanHoatDong.TenDiaBanHoatDong,
                HoKhauThuongTru = it.hv.HoKhauThuongTru,
                NoiOHiennay = it.hv.ChoOHienNay,
                SoDienThoai = it.hv.SoDienThoai,
                DangVien = "",
                SoThe = it.hv.MaCanBo,

              
                DanToc = it.hv.DanToc!.TenDanToc,
                TonGiao = it.hv.TonGiao!.TenTonGiao,
                TrinhDoHocVan = it.hv.TrinhDoHocVan.TenTrinhDoHocVan,
                TrinhDoChuyenMon = it.hv.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                ChinhTri = it.hv.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                NgayThangVaoHoi = it.hv.NgayVaoHoi,
                NgheNghiep = it.hv.NgheNghiep!.TenNgheNghiep,
                DiaBanDanCu = it.hv.HoiVienDanCu == true ? "X" : "",
                NganhNghe = it.hv.HoiVienNganhNghe == true ? "X" : "",
                SoQuyetDinh = it.hv.SoQuyetDinhBoNhiem,
                NgayQuyetDinh = it.hv.NgayVaoHoi != null ? it.hv.NgayVaoHoi.Value.ToString("dd/MM/yyyy") : "",
                NgayCapThe = it.hv.NgayCapThe != null ? it.hv.NgayCapThe.Value.ToString("dd/MM/yyyy") : "",
                TrangThai = it.pv.TrangThaiDuyet
            }).OrderBy(it => it.TenQuanHuyen).ThenBy(it => it.TenHoi).ToList().Select((it, index) => new HVDangKyDuyetDetailVM
            {
                ID = it.ID,
                STT = index + 1,
                HoVaTen = it.HoVaTen,
                Nam = it.Nam,
                Nu = it.Nu,
                SoCCCD = it.SoCCCD,
                TenQuanHuyen = it.TenQuanHuyen,
                TenHoi = it.TenHoi,
                NgayCapSoCCCD = it.NgayCapSoCCCD,
                HoKhauThuongTru = it.HoKhauThuongTru,
                NoiOHiennay = it.NoiOHiennay,
                SoDienThoai = it.SoDienThoai,
                DangVien = "",
                DanToc = it.DanToc,
                TonGiao = it.TonGiao,
                TrinhDoHocVan = it.TrinhDoHocVan,
                TrinhDoChuyenMon = it.TrinhDoChuyenMon,
                ChinhTri = it.ChinhTri,
                NgayThangVaoHoi = it.NgayThangVaoHoi,
                NgheNghiep = it.NgheNghiep,
                DiaBanDanCu = it.DiaBanDanCu,
                NganhNghe = it.NganhNghe,
                SoThe = it.SoThe,
                NgayCapThe = it.NgayCapThe,
                TrangThai = it.TrangThai,
                NgayQuyetDinh = it.NgayQuyetDinh,
                SoQuyetDinh = it.SoQuyetDinh

            }).OrderBy(it=>it.SoThe).ToList();
            return model;

        }
        #endregion Helper
    }
}
