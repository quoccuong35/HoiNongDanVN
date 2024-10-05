using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys.HoiVien;
using HoiNongDan.Models.ViewModels.HoiVien;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Data;
using System.Linq;
using System.Transactions;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class BanChapHanhCHController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContext; 
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 4;
        private const string url_excel = @"upload\filemau\BanChapHanhChiHoi.xlsx";
        public BanChapHanhCHController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config, IHttpContextAccessor httpContext) : base(context) {
            _hostEnvironment = hostEnvironment;
            _httpContext = httpContext;
        }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBagSearch();
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(HoiVienSearchVM search) {
            var data = LoadData(search);
            return PartialView(data);
        }
        #endregion Index
        #region Import
        [HoiNongDanAuthorization]
        public IActionResult _Import()
        {
            CreateViewBagSearch();
            return PartialView();
        }
        [HoiNongDanAuthorization]
        public IActionResult Import(Guid? MaDiaBanHoiVien, String? MaQuanHuyen)
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
            DataSet ds = GetDataSetFromExcel();
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                bool edit;
                List<string> errorList = new List<string>();
                List<Guid> nguoiDuyets = new List<Guid>();
                return ExcuteImportExcel(() =>
                {
                    const TransactionScopeOption opt = new TransactionScopeOption();

                    TimeSpan span = new TimeSpan(0, 0, 30, 30);
                    var chiHois = _context.ChiHois.ToList();
                    using (TransactionScope ts = new TransactionScope(opt, span))
                    {
                        int iCapNhat = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            if (dt.Rows.IndexOf(row) >= startIndex - 1)
                            {

                                if (row[0] == null || String.IsNullOrWhiteSpace(row[0].ToString()))
                                    break;

                                var data = CheckTemplate(row.ItemArray, chiHois);
                                data.hoiVien.MaDiaBanHoatDong = MaDiaBanHoiVien;
                                data.hoiVien.HoiVienDuyet = true;
                                data.hoiVien.Actived = true;
                                data.hoiVien.Level = "90";
                                data.hoiVien.isRoiHoi = false;
                                data.hoiVien.IsHoiVien = true;
                                data.hoiVien.IsBanChapHanh = true;
                                if (data.error.Count > 0)
                                {
                                    errorList.AddRange(data.error);
                                }
                                else
                                {
                                    string result = ExecuteImportExcel(data,  MaDiaBanHoiVien.Value);
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
        private BanChapHanhCHExcel CheckTemplate(object[] row,List<ChiHoi> chiHois)
        {
            BanChapHanhCHExcel import = new BanChapHanhCHExcel();
            CanBo data = new CanBo();
            data.IDCanBo = Guid.NewGuid();
            data.HoiVienDuyet = false;
            data.AccountIdDangKy = AccountId();
            data.IsHoiVien = true;
            data.TuChoi = false;
            int index = 0; string value;
            List<String> error = new List<string>();
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
                                import.edit = true;
                            }
                            catch
                            {
                                error.Add($"Dòng {index} có ID không hợp lệ");
                            }
                        }
                        break;
                    case 2:
                        //Ho Va ten
                        try
                        {
                            if (String.IsNullOrWhiteSpace(value))
                            {
                                error.Add($"Dòng {index} Chưa có nhập họ và tên");
                            }
                            else
                            {
                                data.HoVaTen = value;
                            }
                        }
                        catch
                        {
                            error.Add($"Dòng {index} có Họ và tên không hợp lệ");
                        }
                        break;
                    case 3:
                        //Ngày tháng năm sinh - Nam
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.GioiTinh = GioiTinh.Nam;
                                data.NgaySinh = Function.ConvertStringToDate(value).ToString("dd/MM/yyyy");
                            }
                        }
                        catch
                        {
                            error.Add($"Dòng {index} có ngày sinh Nam không hợp lệ");
                        }
                        break;
                    case 4:
                        //Ngày tháng năm sinh - nữ
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.GioiTinh = GioiTinh.Nữ;
                                data.NgaySinh = Function.ConvertStringToDate(value).ToString("dd/MM/yyyy");
                            }

                        }
                        catch
                        {
                            error.Add($"Dòng {index} có ngày sinh Nữ không hợp lệ");
                        }
                        break;
                    case 5:
                        //Số thẻ hội viên
                        data.MaCanBo = value;
                        break;
                    case 6:
                        //số CCCD
                        data.SoCCCD = value;
                        break;
                    case 7:
                        //chức danh
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var chucvu = _context.ChucVus.SingleOrDefault(it => it.TenChucVu == value);
                            if (chucvu != null)
                            {
                                data.MaChucVu = chucvu.MaChucVu;
                            }
                            else
                            {
                                  error.Add($"Dòng {index} chức danh không hợp lệ");
                            }
                        }
                      
                        break;
                    case 8:
                        //chi hội
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var exist = chiHois.Where(it => it.TenChiHoi == value);
                            if (exist.Count() > 0)
                            {
                                data.MaChiHoi = exist.First().MaChiHoi;
                            }
                            else
                            {
                                ChiHoi add = new ChiHoi { MaChiHoi = Guid.NewGuid(), TenChiHoi = value };
                                data.MaChiHoi = add.MaChiHoi;
                                import.addChiHoi.Add(add);
                                chiHois.Add(add);
                            }
                        }
                        break;
                    case 9:
                        //Số điện thoại
                       
                        break;
                    case 10:
                       
                        break;
                    case 11:
                        data.QueQuan = value;
                        break;
                    case 12:
                        //Dân tộc
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.DanTocs.FirstOrDefault(it => it.TenDanToc == value);
                            if (obj != null)
                            {
                                data.MaDanToc = obj.MaDanToc;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy dân tộc tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 13:
                        //Tôn giáo
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.TonGiaos.FirstOrDefault(it => it.TenTonGiao == value);
                            if (obj != null)
                            {
                                data.MaTonGiao = obj.MaTonGiao;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy tôn giáo có tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 14:
                        //Trình độ học vấn
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.TrinhDoHocVans.FirstOrDefault(it => it.TenTrinhDoHocVan == value);
                            if (obj != null)
                            {
                                data.MaTrinhDoHocVan = obj.MaTrinhDoHocVan;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy Trình độ học vấn có tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 15:
                        //Chính trị
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.TrinhDoChinhTris.FirstOrDefault(it => it.TenTrinhDoChinhTri == value);
                            if (obj != null)
                            {
                                data.MaTrinhDoChinhTri = obj.MaTrinhDoChinhTri;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy Trình độ Chính trị có tên {0} ở dòng số {1} !", value, index));
                            }

                        }
                        break;
                    case 16:
                        data.NgayvaoDangDuBi = value;
                        data.DangVien = true;
                        break;
                    case 17:
                        data.NgayVaoDangChinhThuc = value;
                        data.DangVien = true;
                        break;
                    default:
                        break;
                }
            }
            import.error = error;
            import.hoiVien = data;
            return import;
        }
        private string ExecuteImportExcel(BanChapHanhCHExcel import,Guid MaDiaBanHoiVien)
        {
            var hoiviens = _context.CanBos.Where(
                    it => it.MaDiaBanHoatDong == MaDiaBanHoiVien &&
                    (it.IDCanBo == import.hoiVien.IDCanBo || (it.SoCCCD!.Contains(import.hoiVien.SoCCCD!)&&  it.HoVaTen.Contains(import.hoiVien.HoVaTen)) || (it.MaCanBo == import.hoiVien.MaCanBo && it.HoVaTen.Contains(import.hoiVien.HoVaTen))));
            if (hoiviens.Count()==0)
            {
                // Them mới
                /// Kiểm tra mã tên và số CCCD xem có tồn tại chưa
                import.hoiVien.CreatedAccountId = AccountId();
                import.hoiVien.CreatedTime = DateTime.Now;
                _context.Entry(import.hoiVien).State = EntityState.Added;
            }
            else
            {
                var checkExit = hoiviens.First();
                import.hoiVien.IDCanBo = checkExit.IDCanBo;
                BanChapHanhEdit(checkExit, import.hoiVien);
                HistoryModelRepository history = new HistoryModelRepository(_context);
                history.SaveUpdateHistory(checkExit.IDCanBo.ToString(), AccountId()!.Value, checkExit);
            }
            if (import.addChiHoi.Count() > 0)
            {

                _context.ChiHois.AddRange(import.addChiHoi);
            }
            var checkChiHoiList = _context.HoiVien_ChiHois.SingleOrDefault(it => it.MaChiHoi == import.hoiVien.MaChiHoi && it.IDHoiVien == import.hoiVien.IDCanBo);
            if (checkChiHoiList == null) {
                var add = new HoiVien_ChiHoi {
                    IDHoiVien = import.hoiVien.IDCanBo,
                    MaChiHoi = import.hoiVien.MaChiHoi!.Value,
                    CreatedAccountId = AccountId(),
                    CreatedTime = DateTime.Now
                };
                _context.HoiVien_ChiHois.Add(add);
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.InnerException!.Message + " " + import.hoiVien.HoVaTen;
            }
            return LanguageResource.ImportSuccess;
        }
        private CanBo BanChapHanhEdit(CanBo hvEdit, CanBo hoivien)
        {
            hvEdit.IsBanChapHanh = true;
            hvEdit.MaCanBo = hoivien.MaCanBo;
            hvEdit.SoCCCD = hoivien.SoCCCD;
            hvEdit.NgaySinh = hoivien.NgaySinh;
            hvEdit.MaChucVu = hoivien.MaChucVu;
            hvEdit.QueQuan = hoivien.QueQuan;
            hvEdit.MaDanToc = hoivien.MaDanToc;
            hvEdit.TonGiao = hoivien.TonGiao;
            hvEdit.MaTrinhDoHocVan = hoivien.MaTrinhDoHocVan;
            hvEdit.MaTrinhDoChinhTri = hoivien.MaTrinhDoChinhTri;
            hvEdit.NgayvaoDangDuBi = hoivien.NgayvaoDangDuBi;
            hvEdit.NgayVaoDangChinhThuc = hoivien.NgayVaoDangChinhThuc;
            hvEdit.MaChiHoi = hoivien.MaChiHoi;
            hvEdit.LastModifiedAccountId = AccountId();
            hvEdit.LastModifiedTime = DateTime.Now;

            return hvEdit;
        }
        #endregion Import
        #region Export
        [HoiNongDanAuthorization]
        public IActionResult ExportCreate()
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, url_excel);
            List<BanChapHanhCHDetail> data = new List<BanChapHanhCHDetail>();
            return Export(data, url, startIndex);
        }
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit(HoiVienSearchVM search) { 
            var data = LoadData(search);
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, url_excel);

            return Export(data, url, startIndex); ;
        }
        private FileContentResult Export(List<BanChapHanhCHDetail> data, string url, int startIndex)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "IDCanBo", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinh_Nam", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinh_Nu", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "MaCanBo", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoCCCD", isAllowedToEdit = true, isText = true });


            var chucvu = _context.ChucVus.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaChucVu.ToString(), Name = x.TenChucVu }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "ChucVu", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chucvu, TypeId = ConstExcelController.StringId });
            columns.Add(new ExcelTemplate() { ColumnName = "ChiHoi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Huyen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Xa", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "QueQuan", isAllowedToEdit = true, isText = true });

            var danToc = _context.DanTocs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaDanToc, Name = x.TenDanToc }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "DanToc", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = danToc, TypeId = ConstExcelController.StringId });

            var tonGiao = _context.TonGiaos.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTonGiao, Name = x.TenTonGiao }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TonGiao", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tonGiao, TypeId = ConstExcelController.StringId });

            var hocVan = _context.TrinhDoHocVans.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoHocVan, Name = x.TenTrinhDoHocVan }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TrinhDoHocVan", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = hocVan, TypeId = ConstExcelController.StringId });

            var chuyenNganh = _context.TrinhDoChuyenMons.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChuyenMon, Name = x.TenTrinhDoChuyenMon }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChuyenMon", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chuyenNganh, TypeId = ConstExcelController.StringId });

            var chinhTri = _context.TrinhDoChinhTris.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChinhTri, Name = x.TenTrinhDoChinhTri }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TrinhDoChinhTri", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chinhTri, TypeId = ConstExcelController.StringId });



            columns.Add(new ExcelTemplate() { ColumnName = "NgayvaoDangDuBi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoDangChinhThuc", isAllowedToEdit = true, isText = true });

            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.BanChapHanhCH);
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
        [NonAction]
        private List<BanChapHanhCHDetail> LoadData(HoiVienSearchVM search) {
            // var model = (from cb in _context.CanBos
            //              join pv in _context.PhamVis on cb.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
            //              join hoivien_chihoi in _context.HoiVien_ChiHois on cb.IDCanBo equals hoivien_chihoi.IDHoiVien
            //              join chihoi in _context.ChiHois on hoivien_chihoi.MaChiHoi equals chihoi.MaChiHoi
            //              where pv.AccountId == AccountId()
            //              && cb.IsHoiVien == true).Select(it => new{ hv= it.cb,chihoi= it.chihoi })
            //.Include(it => it.DiaBanHoatDong)
            //.ThenInclude(it => it!.PhuongXa)
            //.ThenInclude(it => it!.QuanHuyen)
            //.Include(it => it.ChucVu)
            //.Include(it => it.DanToc)
            //.Include(it => it.TonGiao)
            //.Include(it => it.TrinhDoHocVan)
            //.Include(it => it.TrinhDoChinhTri)
            //.Include(it => it.ChiHoi)
            //.Include(it => it.CoSo).AsQueryable();

            var model = _context.HoiVien_ChiHois
                 .Include(it => it.ChiHoi)
                 .Include(it => it.HoiVien)
                     .ThenInclude(it => it.DanToc)
                 .Include(it => it.HoiVien)
                     .ThenInclude(it => it.TonGiao)
                  .Include(it => it.HoiVien)
                     .ThenInclude(it => it.TrinhDoHocVan)
                  .Include(it => it.HoiVien)
                     .ThenInclude(it => it.TrinhDoChinhTri)
                  .Include(it => it.HoiVien)
                     .ThenInclude(it => it.ChucVu)
                  .Include(it => it.HoiVien)
                     .ThenInclude(it => it.TrinhDoHocVan)
                 .Include(it => it.HoiVien)
                     .ThenInclude(it => it.DiaBanHoatDong)
                         .ThenInclude(it => it.QuanHuyen)
                          .ThenInclude(it => it.PhuongXas).AsQueryable();
            List<Guid> phamvis = _context.PhamVis.Where(it => it.AccountId == AccountId()).Select(it =>  it.MaDiabanHoatDong ).ToList() ;


            if (!String.IsNullOrEmpty(search.HoVaTen))
            {
                model = model.Where(it => it.HoiVien.HoVaTen.Contains(search.HoVaTen));
            }

            if (search.MaDiaBanHoiVien != null)
            {
                model = model.Where(it => it.HoiVien.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
            }
            if (search.MaChiHoi != null)
            {
                model = model.Where(it => it.MaChiHoi == search.MaChiHoi);
            }

            if (!String.IsNullOrEmpty(search.MaCanBo))
            {
                model = model.Where(it => it.HoiVien.MaCanBo == search.MaCanBo);
            }
            if (search.MaQuanHuyen != null)
            {
                model = model.Where(it => it.HoiVien.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
            }
            if (!String.IsNullOrEmpty(search.TenPhuongXa))
            {
                model = model.Where(it => it.HoiVien.DiaBanHoatDong!.PhuongXa!.TenPhuongXa.ToUpper() == search.TenPhuongXa || it.HoiVien.DiaBanHoatDong!.QuanHuyen!.TenQuanHuyen.ToUpper() == search.TenPhuongXa);
            }
            int dd = model.Count();
            //model = model.Where(it => it.HoiVien.IsBanChapHanh == true && phamvis.Contains(it.HoiVien.MaDiaBanHoatDong.Value));
            var data = model.Select(it => new 
            {
                MaDiaBanHoatDong = it.HoiVien.MaDiaBanHoatDong.Value,
                IDCanBo = it.HoiVien.IDCanBo,
                HoVaTen = it.HoiVien.HoVaTen,
                NgaySinh_Nam = it.HoiVien.GioiTinh == GioiTinh.Nam ? it.HoiVien.NgaySinh : "",
                NgaySinh_Nu = it.HoiVien.GioiTinh == GioiTinh.Nữ ? it.HoiVien.NgaySinh : "",
                MaCanBo = it.HoiVien.MaCanBo,
                SoCCCD = it.HoiVien.SoCCCD,
                ChucVu = it.HoiVien.ChucVu!.TenChucVu,
                ChiHoi = it.ChiHoi.TenChiHoi,
                Huyen = it.HoiVien.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen,
                Xa = it.HoiVien.DiaBanHoatDong.PhuongXa.TenPhuongXa,
                DanToc = it.HoiVien.DanToc!.TenDanToc,
                TonGiao = it.HoiVien.TonGiao!.TenTonGiao,
                TrinhDoChinhTri = it.HoiVien.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                TrinhDoHocVan = it.HoiVien.TrinhDoHocVan.TenTrinhDoHocVan,
                NgayvaoDangDuBi = it.HoiVien.NgayvaoDangDuBi,
                NgayVaoDangChinhThuc = it.HoiVien.NgayVaoDangChinhThuc,
                QueQuan = it.HoiVien.QueQuan
            }).ToList();

            var data1 = data.Where(it => phamvis.Contains(it.MaDiaBanHoatDong)).Select(it=>new BanChapHanhCHDetail {
                IDCanBo = it.IDCanBo,
                HoVaTen = it.HoVaTen,
                NgaySinh_Nam = it.NgaySinh_Nam,
                NgaySinh_Nu = it.NgaySinh_Nam,
                MaCanBo = it.MaCanBo,
                SoCCCD = it.SoCCCD,
                ChucVu = it.ChucVu,
                ChiHoi = it.ChiHoi,
                Huyen = it.Huyen,
                Xa = it.Xa,
                DanToc = it.DanToc,
                TonGiao = it.TonGiao,
                TrinhDoChinhTri = it.TrinhDoChinhTri,
                TrinhDoHocVan = it.TrinhDoHocVan,
                NgayvaoDangDuBi = it.NgayvaoDangDuBi,
                NgayVaoDangChinhThuc = it.NgayVaoDangChinhThuc,
                QueQuan = it.QueQuan
            }).ToList() ;
            
            return data1;
        }
        private void CreateViewBagSearch(string? maQuanHuyen = null, Guid? maDiaBan = null, Guid? maChiHoi = null)
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId(), value: maDiaBan);

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId(), value: maQuanHuyen);

            ViewBag.MaChiHoi = fnViewBag.ChiHoi(value: maChiHoi);
        }
        #endregion Helper
    }
}
