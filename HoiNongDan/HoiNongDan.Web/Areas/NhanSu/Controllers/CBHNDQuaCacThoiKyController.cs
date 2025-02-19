using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Transactions;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Authorize]
    [Area(ConstArea.NhanSu)]

    public class CBHNDQuaCacThoiKyController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        const int startIndex = 4;
        private string filemau = @"upload\filemau\CanBoHoiNongDanQuaCacThoiKy.xlsx";
        public CBHNDQuaCacThoiKyController(AppDbContext context, IWebHostEnvironment hostEnvironment) : base(context) { _hostEnvironment = hostEnvironment; }
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(CanBoSearchVM search)
        {
            return ExecuteSearch(() =>
            {
                var model = _context.CanBoQuaCacThoiKys.AsQueryable();
                if (!String.IsNullOrEmpty(search.MaCanBo))
                {
                    model = model.Where(it => it.SoCCCD == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen))
                {
                    model = model.Where(it => it.HoVaTen.Contains(search.HoVaTen));
                }
              

                var data = model.Select(it => new CanBoBanChapHanhQuanCacThoiKy
                {
                    ID = it.Id,
                    HoVaTen = it.HoVaTen,
                    SoCCCD = it.SoCCCD,
                    TenChucVu = it.ChucVu,
                    NhiemKy = it.NhiemKy!,
                    NoiLamViec = it.NoiLamViec!,
                    ChoOHienNay = it.NoiCuTru!,
                    SoDienThoai = it.SoDienThoai!,
                    ChucVuHienNay = it.ChucVuHienNay
                }).OrderBy(it => it.HoVaTen).ToList();
                return PartialView(data);
            });
        }
        #region Import
        [HoiNongDanAuthorization]
        public IActionResult _Import()
        {
            return PartialView();
        }

        [HoiNongDanAuthorization]
        public IActionResult Import(Guid? MaDiaBanHoiVien, String? MaQuanHuyen)
        {
            DataSet ds = GetDataSetFromExcel();
            if (ds != null && ds.Tables.Count > 0)
            {
                var hoiViens = _context.CanBos.Include(it => it.DiaBanHoatDong).Where(it => it.Actived == true && it.IsHoiVien == true && it.DiaBanHoatDong!.MaQuanHuyen == MaQuanHuyen).AsQueryable();
                return ExcuteImportExcel(() =>
                {
                    const TransactionScopeOption opt = new TransactionScopeOption();

                    TimeSpan span = new TimeSpan(0, 0, 30, 30);
                    DataTable dt = ds.Tables[0];
                    List<String> error = new List<string>();
                    List<String> subError = new List<string>();
                    int stt = 0;

                    using (TransactionScope ts = new TransactionScope(opt, span))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (dt.Rows.IndexOf(row) >= startIndex - 1)
                            {
                                if (row[0] == null || row[0].ToString() == "")
                                    break;
                                subError = new List<string>();
                                var data = CheckTemplate(row.ItemArray, subError);
                                if (subError.Count > 0)
                                {
                                    error.AddRange(subError);
                                }
                                else
                                {
                                    // Tiến hành cập nhật
                                    string result = ExecuteImportExcel(data);
                                    if (result != LanguageResource.ImportSuccess)
                                    {
                                        error.Add(result);
                                    }
                                    else
                                        stt++;
                                }

                            }

                        }
                        if (error != null && error.Count > 0)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = false,
                                Data = String.Join("<br/>", error)
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
        [NonAction]
        public string ExecuteImportExcel(CanBoBanChapHanhQuanCacThoiKy banChapHanh) {
            try
            {
                if (banChapHanh.ID == null)
                {
                    var add = new CanBoQuaCacThoiKy
                    {
                        Id = Guid.NewGuid(),
                        HoVaTen = banChapHanh.HoVaTen,
                        SoCCCD = banChapHanh.SoCCCD,
                        SoDienThoai = banChapHanh.SoDienThoai,
                        ChucVu = banChapHanh.TenChucVu,
                        NhiemKy = banChapHanh.NhiemKy,
                        NoiLamViec = banChapHanh.NoiLamViec,
                        NoiCuTru = banChapHanh.ChoOHienNay,
                        ChucVuHienNay = banChapHanh.ChucVuHienNay,
                    };
                    _context.CanBoQuaCacThoiKys.Add(add);
                }
                else
                {
                    var edit = _context.CanBoQuaCacThoiKys.SingleOrDefault(it => it.Id == banChapHanh.ID);
                    if (edit != null)
                    {
                        edit.HoVaTen = banChapHanh.HoVaTen;
                        edit.SoCCCD = banChapHanh.SoCCCD;
                        edit.SoDienThoai = banChapHanh.SoDienThoai;
                        edit.ChucVu = banChapHanh.TenChucVu;
                        edit.NhiemKy = banChapHanh.NhiemKy;
                        edit.NoiLamViec = banChapHanh.NoiLamViec;
                        edit.NoiCuTru = banChapHanh.ChoOHienNay;
                        edit.ChucVuHienNay = banChapHanh.ChucVuHienNay;
                    }
                    else
                    {
                        return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                                   LanguageResource.CanBoHNDQuaCacThoiKy, banChapHanh!.HoVaTen,
                                                   string.Format(LanguageResource.Export_ExcelHeader,
                                                   LanguageResource.CanBo));
                    }
                }
                _context.SaveChanges();
                return LanguageResource.ImportSuccess;
            }
            catch (Exception ex)
            {

                return ex.Message + " " + banChapHanh.HoVaTen; 
            }
        }
        protected DataSet GetDataSetFromExcel()
        {
            DataSet ds = new DataSet();
            try
            {
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
        [NonAction]
        private CanBoBanChapHanhQuanCacThoiKy CheckTemplate(object[] row, List<String> error)
        {
            CanBoBanChapHanhQuanCacThoiKy data = new CanBoBanChapHanhQuanCacThoiKy();
            string value = "";
            string stt = "";
            for (int i = 0; i < row.Length; i++)
            {
                value = "";
                value = row[i]!.ToString()!;
                switch (i)
                {
                    case 0:
                        stt = value;
                        break;
                    case 1:
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.ID = Guid.Parse(value);
                            }
                        }
                        catch (Exception)
                        {
                            error.Add($"Id dòng {stt} không hợp lệ");
                        }
                        break;
                    case 2:
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.HoVaTen = value;
                        }
                        else
                            error.Add($"Họ và tên dòng {stt} chưa nhập");
                        break;
                    case 3:
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.SoCCCD = value;
                        }
                        break;
                    case 4:
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.SoDienThoai = value;
                        }
                        break;

                    case 5:
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.TenChucVu = value;
                        }
                        break;
                    case 6:
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.NhiemKy = value;
                        }
                        break;
                    case 7:
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.NoiLamViec = value;
                        }
                        break;
                    case 8:
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.ChoOHienNay = value;
                        }
                        break;
                    case 9:
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.ChucVuHienNay = value;
                        }
                        break;
                }
            }
            return data;
        }
        #endregion Import

        #region Export
        [HoiNongDanAuthorization]
        public IActionResult ExportCreate()
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, filemau);
            byte[] filecontent = ClassExportExcel.ExportFileMau(url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "CanBoHoiNongDanQuaCacThoiKy");
            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit()
        {
            var model = _context.CanBoQuaCacThoiKys.ToList().Select((it, index) => new CanBoBanChapHanhQuanCacThoiKy
            {
                ID = it.Id,
                STT = index+1,
                HoVaTen = it.HoVaTen,
                SoCCCD = it.SoCCCD,
                TenChucVu = it.ChucVu,
                NhiemKy = it.NhiemKy!,
                NoiLamViec = it.NoiLamViec!,
                ChoOHienNay = it.NoiCuTru!,
                SoDienThoai = it.SoDienThoai!,
                ChucVuHienNay = it.ChucVuHienNay
            }).ToList();
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, filemau);
          //  byte[] filecontent = ClassExportExcel.ExportExcel(model, startIndex + 1, url);
            byte[] filecontent = ClassExportExcel.ExportExcel(model, startIndex+1, url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "CanBoHoiNongDanQuaCacThoiKy" + DateTime.Now.ToString("hh_mm_ss"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export
    }
}
