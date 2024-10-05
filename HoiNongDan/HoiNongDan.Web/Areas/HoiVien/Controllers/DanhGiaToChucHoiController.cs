using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Transactions;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    /// <summary>
    /// 01 HỘI NÔNG DÂN CƠ SỞ
    /// 02 CHI HỘI DÂN CƯ					
    /// 03 CHI HỘI NGHỀ NGHIỆP		
    ///  phan loai
    /// 01 Tổng số cơ sở hội
    /// 02 Hoàn thành xuất sắc nhiệm vụ
    /// 03 Hoàn thành tốt nhiệm vụ
    /// 04 Hoàn thành nhiệm vụ
    /// 05 Không hoàn thành nhiệm vụ
    /// 06 không phân loại
    /// </summary>
    [Area(ConstArea.HoiVien)]
    public class DanhGiaToChucHoiController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        const string controllerCode = ConstExcelController.HoiVien;
        private string url = @"upload\filemau\DanhGiaToChucHoi.xlsx";
        public DanhGiaToChucHoiController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config, IHttpContextAccessor httpContext) : base(context)
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
            model.Loai = "01";
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
        public IActionResult Import( int? Nam)
        {
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
        private DanhGiaToChucHoiVM CheckTemplate(object[] row, List<String> error)
        {
            DanhGiaToChucHoiVM data = new DanhGiaToChucHoiVM();
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
                                var checkDB = _context.DiaBanHoatDongs.SingleOrDefault(it => it.Id == Guid.Parse(value));
                                if (checkDB == null)
                                {
                                    error.Add($"Dòng {index} Không tìm thấy thông địa bàn hội viên");
                                    break;
                                }
                                else
                                    data.ID = checkDB.Id;
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
                        if (!String.IsNullOrWhiteSpace(value))
                            data.DonVi = value;
                        break;
                    case 3:
                        //Tổng số cơ sở hội
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.CoSo_Tong = int.Parse(value);
                            }
                            catch (Exception )
                            {

                                error.Add($"Dòng {index} có Tổng số cơ sở hội không hợp lệ");
                            }
                        }
                        break;
                    case 4:
                        //Hoàn thành xuất sắc nhiệm vụ
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.CoSo_HTXSNV = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Hoàn thành xuất sắc nhiệm vụ không hợp lệ");
                            }
                        }
                        break;
                    case 5:
                        //Hoàn thành tốt nhiệm vụ
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.CoSo_HTTNV = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Hoàn thành tốt nhiệm vụ không hợp lệ");
                            }
                        }
                        break;
                    case 6:
                        //Hoàn thành nhiệm vụ
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.CoSo_HTNV = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Hoàn thành nhiệm vụ không hợp lệ");
                            }
                        }
                        break;
                    case 7:
                        //Không hoàn thành nhiệm vụ
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.CoSo_KHTNV = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Không hoàn thành nhiệm vụ không hợp lệ");
                            }
                        }
                        break;
                    case 8:
                        //Số cơ sở hội không phân loại
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.CoSo_KPhanLoai = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Số cơ sở hội không phân loại không hợp lệ");
                            }
                        }
                        break;
                    case 9:
                        //Số cơ sở hội không phân loại
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.DanCu_Tong = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Tổng số chi hội dân cư không hợp lệ");
                            }
                        }
                        break;
                    case 10:
                        // Hoàn thành xuất sắc nhiệm vụ
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.DanCu_HTXSNV = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Hoàn thành xuất sắc nhiệm vụ không hợp lệ");
                            }
                        }
                        break;
                    case 11:
                        //Hoàn thành tốt nhiệm vụ
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.DanCu_HTTNV = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Hoàn thành tốt nhiệm vụ không hợp lệ");
                            }
                        }
                        break;
                    case 12:
                        //Hoàn thành nhiệm vụ
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.DanCu_HTNV = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Hoàn thành nhiệm vụ không hợp lệ");
                            }
                        }
                        break;
                    case 13:
                        //Hoàn thành nhiệm vụ
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.DanCu_KHTNV = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Không hoàn thành nhiệm vụ không hợp lệ");
                            }
                        }
                        break;
                    case 14:
                        //Tổng số chi cơ sở hội không phân loại
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.DanCu_KPhanLoai = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Tổng số chi hội nghề nghiệp không hợp lệ");
                            }
                        }
                        break;

                    case 15:
                        //Tổng số chi cơ sở hội không phân loại
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.NgheNghiep_Tong = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Tổng số chi hội nghề nghiệp không hợp lệ");
                            }
                        }
                        break;
                    case 16:
                        //Hoàn thành xuất sắc nhiệm vụ
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.NgheNghiep_HTXSNV = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Hoàn thành xuất sắc nhiệm vụ không hợp lệ");
                            }
                        }
                        break;
                    case 17:
                        //Hoàn thành tốt nhiệm vụ
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.NgheNghiep_HTTNV = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Hoàn thành tốt nhiệm vụ không hợp lệ");
                            }
                        }
                        break;
                    case 18:
                        //Hoàn thành nhiệm vụ
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.NgheNghiep_HTNV = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Hoàn thành nhiệm vụ không hợp lệ");
                            }
                        }
                        break;
                    case 19:
                        //Không hoàn thành nhiệm vụ
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.NgheNghiep_KHTNV = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Không hoàn thành nhiệm vụ không hợp lệ");
                            }
                        }
                        break;
                    case 20:
                        //Tổng số chi hội không phân loại
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.NgheNghiep_KPhanLoai = int.Parse(value);
                            }
                            catch (Exception)
                            {

                                error.Add($"Dòng {index} có Tổng số chi hội không phân loại không hợp lệ");
                            }
                        }
                        break;

                }
            }
            return data;
        }
        #endregion Import 
        private string ExecuteImportExcel(DanhGiaToChucHoiVM item, int Nam)
        {
            // cơ sở
            if (item.CoSo_Tong > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "01" && it.LoaiDanhGia == "01");
                if (check != null)
                {
                    check.SoLuong = item.CoSo_Tong.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi{ 
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "01",
                        LoaiToChuc = "01",
                        SoLuong  = item.CoSo_Tong.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.CoSo_HTXSNV > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "01" && it.LoaiDanhGia == "02");
                if (check != null)
                {
                    check.SoLuong = item.CoSo_HTXSNV.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "02",
                        LoaiToChuc = "01",
                        SoLuong = item.CoSo_HTXSNV.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.CoSo_HTTNV > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "01" && it.LoaiDanhGia == "03");
                if (check != null)
                {
                    check.SoLuong = item.CoSo_HTTNV.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "03",
                        LoaiToChuc = "01",
                        SoLuong = item.CoSo_HTTNV.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.CoSo_HTNV > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "01" && it.LoaiDanhGia == "04");
                if (check != null)
                {
                    check.SoLuong = item.CoSo_HTNV.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "04",
                        LoaiToChuc = "01",
                        SoLuong = item.CoSo_HTNV.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.CoSo_KHTNV > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "01" && it.LoaiDanhGia == "05");
                if (check != null)
                {
                    check.SoLuong = item.CoSo_KHTNV.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "05",
                        LoaiToChuc = "01",
                        SoLuong = item.CoSo_KHTNV.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.CoSo_KPhanLoai > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "01" && it.LoaiDanhGia == "06");
                if (check != null)
                {
                    check.SoLuong = item.CoSo_KPhanLoai.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "06",
                        LoaiToChuc = "01",
                        SoLuong = item.CoSo_KPhanLoai.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            // dân cư
            if (item.DanCu_Tong > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "02" && it.LoaiDanhGia == "01");
                if (check != null)
                {
                    check.SoLuong = item.DanCu_Tong.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "01",
                        LoaiToChuc = "02",
                        SoLuong = item.DanCu_Tong.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.DanCu_HTXSNV > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "02" && it.LoaiDanhGia == "02");
                if (check != null)
                {
                    check.SoLuong = item.DanCu_HTXSNV.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "02",
                        LoaiToChuc = "02",
                        SoLuong = item.DanCu_HTXSNV.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.DanCu_HTTNV > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "02" && it.LoaiDanhGia == "03");
                if (check != null)
                {
                    check.SoLuong = item.DanCu_HTTNV.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "03",
                        LoaiToChuc = "02",
                        SoLuong = item.DanCu_HTTNV.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.DanCu_HTNV > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "02" && it.LoaiDanhGia == "04");
                if (check != null)
                {
                    check.SoLuong = item.DanCu_HTNV.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "04",
                        LoaiToChuc = "02",
                        SoLuong = item.DanCu_HTNV.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.DanCu_KHTNV > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "02" && it.LoaiDanhGia == "05");
                if (check != null)
                {
                    check.SoLuong = item.DanCu_KHTNV.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "05",
                        LoaiToChuc = "02",
                        SoLuong = item.DanCu_KHTNV.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.DanCu_KPhanLoai > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "02" && it.LoaiDanhGia == "06");
                if (check != null)
                {
                    check.SoLuong = item.DanCu_KPhanLoai.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "06",
                        LoaiToChuc = "02",
                        SoLuong = item.DanCu_KPhanLoai.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }

            /// nghề nghiệp
            if (item.NgheNghiep_Tong > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "03" && it.LoaiDanhGia == "01");
                if (check != null)
                {
                    check.SoLuong = item.NgheNghiep_Tong.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "01",
                        LoaiToChuc = "03",
                        SoLuong = item.NgheNghiep_Tong.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.NgheNghiep_HTXSNV > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "03" && it.LoaiDanhGia == "02");
                if (check != null)
                {
                    check.SoLuong = item.NgheNghiep_HTXSNV.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "02",
                        LoaiToChuc = "03",
                        SoLuong = item.NgheNghiep_HTXSNV.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.NgheNghiep_HTTNV > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "03" && it.LoaiDanhGia == "03");
                if (check != null)
                {
                    check.SoLuong = item.NgheNghiep_HTTNV.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "03",
                        LoaiToChuc = "03",
                        SoLuong = item.NgheNghiep_HTTNV.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.NgheNghiep_HTNV > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "03" && it.LoaiDanhGia == "04");
                if (check != null)
                {
                    check.SoLuong = item.NgheNghiep_HTNV.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "04",
                        LoaiToChuc = "03",
                        SoLuong = item.NgheNghiep_HTNV.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.NgheNghiep_KHTNV > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "03" && it.LoaiDanhGia == "05");
                if (check != null)
                {
                    check.SoLuong = item.NgheNghiep_KHTNV.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "05",
                        LoaiToChuc = "03",
                        SoLuong = item.NgheNghiep_KHTNV.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            if (item.NgheNghiep_KPhanLoai > 0)
            {
                var check = _context.DanhGiaToChucHois.SingleOrDefault(it => it.IDDiaBanHoi == item.ID && it.Nam == Nam && it.LoaiToChuc == "03" && it.LoaiDanhGia == "06");
                if (check != null)
                {
                    check.SoLuong = item.NgheNghiep_KPhanLoai.Value;
                    check.LastModifiedAccountId = AccountId();
                    check.LastModifiedTime = DateTime.Now;
                }
                else
                {
                    var add = new DanhGiaToChucHoi
                    {
                        ID = Guid.NewGuid(),
                        IDDiaBanHoi = item.ID,
                        Nam = Nam,
                        LoaiDanhGia = "06",
                        LoaiToChuc = "03",
                        SoLuong = item.NgheNghiep_KPhanLoai.Value,
                        CreatedAccountId = AccountId(),
                        CreatedTime = DateTime.Now,
                    };
                    _context.DanhGiaToChucHois.Add(add);
                }
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.InnerException!.Message + " " + item.DonVi;
            }
            return LanguageResource.ImportSuccess;
        }
        #region Export
        public IActionResult ExportEdit(DanhGiaHVSearchVM search)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, this.url);
            var model = LoadData(search);
            return Export(model, url, startIndex); ;
        }
        private FileContentResult Export(List<DanhGiaToChucHoiVM> data, string url, int startIndex)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "ID", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DonVi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "CoSo_Tong", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "CoSo_HTXSNV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "CoSo_HTTNV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "CoSo_HTNV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "CoSo_KHTNV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "CoSo_KPhanLoai", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DanCu_Tong", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DanCu_HTXSNV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DanCu_HTTNV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DanCu_HTNV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DanCu_KHTNV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DanCu_KPhanLoai", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgheNghiep_Tong", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgheNghiep_HTXSNV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgheNghiep_HTTNV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgheNghiep_HTNV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgheNghiep_KHTNV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgheNghiep_KPhanLoai", isAllowedToEdit = true, isText = true });

            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.DanhGiaToChucHoi);
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
        private void CreateViewBag()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }
        private List<DanhGiaToChucHoiVM> LoadData(DanhGiaHVSearchVM search)
        {
            var data = _context.DiaBanHoatDongs.Include(it => it.DanhGiaToChucHois).AsQueryable();
            search.Nam = search.Nam != null ? search.Nam : DateTime.Now.Year;
            if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen))
            {
                data = data.Where(it => it.MaQuanHuyen == search.MaQuanHuyen);
            }
            if (search.MaDiaBanHoiVien != null)
            {
                data = data.Where(it => it.Id == search.MaDiaBanHoiVien);
            }
            if (search.Nam != null && search.Loai == "02")
            {
                data = data.Where(it => it.DanhGiaToChucHois.Any(it => it.Nam == search.Nam));
            }
            if (search.Nam != null && search.Loai == "03")
            {
                data = data.Where(it => !it.DanhGiaToChucHois.Any(it => it.Nam == search.Nam));
            }
            var phamvis = _context.PhamVis.Where(it => it.AccountId == AccountId()).Select(it => it.MaDiabanHoatDong).ToList();
            data = data.Where(it => phamvis.Contains(it.Id));
            var model = data.Select(it => new DanhGiaToChucHoiVM {
                ID = it.Id,
                DonVi = it.TenDiaBanHoatDong,
                CoSo_Tong = it.DanhGiaToChucHois.Where(p=>p.LoaiDanhGia == "01" && p.LoaiToChuc =="01" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it=>it.SoLuong).Sum(),
                CoSo_HTXSNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "02" && p.LoaiToChuc == "01" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                CoSo_HTTNV= it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "03" && p.LoaiToChuc == "01" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                CoSo_HTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "04" && p.LoaiToChuc == "01" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                CoSo_KHTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "05" && p.LoaiToChuc == "01" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                CoSo_KPhanLoai = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "06" && p.LoaiToChuc == "01" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),

                DanCu_Tong = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "01" && p.LoaiToChuc == "02" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                DanCu_HTXSNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "02" && p.LoaiToChuc == "02" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                DanCu_HTTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "03" && p.LoaiToChuc == "02" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                DanCu_HTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "04" && p.LoaiToChuc == "02" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                DanCu_KHTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "05" && p.LoaiToChuc == "02" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                DanCu_KPhanLoai = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "06" && p.LoaiToChuc == "02" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),

                NgheNghiep_Tong = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "01" && p.LoaiToChuc == "03" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                NgheNghiep_HTXSNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "02" && p.LoaiToChuc == "03" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                NgheNghiep_HTTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "03" && p.LoaiToChuc == "03" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                NgheNghiep_HTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "04" && p.LoaiToChuc == "03" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                NgheNghiep_KHTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "05" && p.LoaiToChuc == "03" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                NgheNghiep_KPhanLoai = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "06" && p.LoaiToChuc == "03" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
            }).ToList();
            return model;
        }
        private List<DanhGiaToChucHoiDetailVM> LoadDataDetail(DanhGiaHVSearchVM search)
        {
            var data = _context.DiaBanHoatDongs.Include(it => it.DanhGiaToChucHois).AsQueryable();
            search.Nam = search.Nam != null ? search.Nam : DateTime.Now.Year;
            if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen))
            {
                data = data.Where(it => it.MaQuanHuyen == search.MaQuanHuyen);
            }
            if (search.MaDiaBanHoiVien != null)
            {
                data = data.Where(it => it.Id == search.MaDiaBanHoiVien);
            }
            if (search.Nam != null && search.Loai == "02")
            {
                data = data.Where(it => it.DanhGiaToChucHois.Any(it => it.Nam == search.Nam));
            }
            if (search.Nam != null && search.Loai == "03")
            {
                data = data.Where(it => !it.DanhGiaToChucHois.Any(it => it.Nam == search.Nam));
            }
            var phamvis = _context.PhamVis.Where(it => it.AccountId == AccountId()).Select(it=>it.MaDiabanHoatDong).ToList();
            data = data.Where(it => phamvis.Contains(it.Id));
            var model = data.Select(it => new DanhGiaToChucHoiDetailVM
            {
                ID = it.DanhGiaToChucHois.Where(p =>  p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).First().ID,
                DonVi = it.TenDiaBanHoatDong,
                CoSo_Tong = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "01" && p.LoaiToChuc == "01" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                CoSo_HTXSNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "02" && p.LoaiToChuc == "01" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                CoSo_HTTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "03" && p.LoaiToChuc == "01" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                CoSo_HTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "04" && p.LoaiToChuc == "01" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                CoSo_KHTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "05" && p.LoaiToChuc == "01" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                CoSo_KPhanLoai = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "06" && p.LoaiToChuc == "01" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),

                DanCu_Tong = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "01" && p.LoaiToChuc == "02" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                DanCu_HTXSNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "02" && p.LoaiToChuc == "02" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                DanCu_HTTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "03" && p.LoaiToChuc == "02" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                DanCu_HTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "04" && p.LoaiToChuc == "02" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                DanCu_KHTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "05" && p.LoaiToChuc == "02" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                DanCu_KPhanLoai = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "06" && p.LoaiToChuc == "02" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),

                NgheNghiep_Tong = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "01" && p.LoaiToChuc == "03" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                NgheNghiep_HTXSNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "02" && p.LoaiToChuc == "03" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                NgheNghiep_HTTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "03" && p.LoaiToChuc == "03" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                NgheNghiep_HTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "04" && p.LoaiToChuc == "03" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                NgheNghiep_KHTNV = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "05" && p.LoaiToChuc == "03" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
                NgheNghiep_KPhanLoai = it.DanhGiaToChucHois.Where(p => p.LoaiDanhGia == "06" && p.LoaiToChuc == "03" && p.Nam == search.Nam && p.IDDiaBanHoi == it.Id).Select(it => it.SoLuong).Sum(),
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
