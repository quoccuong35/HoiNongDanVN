using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys.NhanSu;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using System.Transactions;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class VayVonController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        const string controllerCode = ConstExcelController.HoiVien_VayVon;
        const int startIndex = 5;
        private string filemau = @"upload\filemau\HoiVienVayVon.xlsx";
        public VayVonController(AppDbContext context, IWebHostEnvironment hostEnvironment) :base(context) { _hostEnvironment = hostEnvironment; }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            VayVonSearchVM model = new VayVonSearchVM();
            CreateViewBagSearch();
            return View(model);
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(VayVonSearchVM search)
        {
            return ExecuteSearch(() =>
            {
                var data = (from hvht in _context.VayVons
                            join hv in _context.CanBos on hvht.IDHoiVien equals hv.IDCanBo
                            join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                            where pv.AccountId == AccountId()
                            select hvht).Distinct().Include(it => it.NguonVon).Include(it => it.HoiVien).ThenInclude(it => it.DiaBanHoatDong).ThenInclude(it=>it.QuanHuyen).ThenInclude(it=>it.PhuongXas).AsQueryable();

                if (!String.IsNullOrWhiteSpace(search.SoCCCD))
                {
                    data = data.Where(it => it.HoiVien.SoCCCD == search.SoCCCD);
                }
                if (!String.IsNullOrEmpty(search.TenHV) && !String.IsNullOrWhiteSpace(search.TenHV))
                {
                    data = data.Where(it => it.HoiVien.HoVaTen.Contains(search.TenHV));
                }
                if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen))
                {
                    data = data.Where(it => it.HoiVien.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
                }
                if (search.MaNguonVon != null)
                {
                    data = data.Where(it => it.MaNguonVon == search.MaNguonVon);
                }
                if (search.Actived != null)
                {
                    data = data.Where(it => it.Actived == search.Actived);
                }
                if (search.TuNgay != null)
                {
                    data = data.Where(it => it.TuNgay >= search.TuNgay);
                }
               
                if (search.DenNgay != null)
                {
                    data = data.Where(it => it.TuNgay <= search.DenNgay);
                }
                if (search.TraXong == "01")
                {
                    data = data.Where(it => it.TraXong == false);
                }
                if (search.TraXong == "02" && search.SoThang == null)
                {
                    data = data.Where(it => it.TraXong == true);
                }
                if (search.MaDiaBanHoiVien != null)
                {
                    data = data.Where(it => it.HoiVien.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
                }
                if (search.NamVayVon != null)
                {
                    data = data.Where(it => it.TuNgay!.Value.Year == search.NamVayVon);
                }
                if (search.SoThang  != null )
                {
                    data = data.Where(it=> it.TraXong != true && it.NgayTraNoCuoiCung!.Value.AddMonths(search.SoThang.Value) < DateTime.Now.Date);
                }
                var model = data.Select(it => new VayVonDetailVM
                {
                    ID = it.IDVayVon,
                    MaHV = it.HoiVien.MaCanBo!,
                    TenHV = it.HoiVien.HoVaTen,
                    SoTienVay = it.SoTienVay,
                    ThoiHangChoVay = it.ThoiHangChoVay,
                    LaiSuatVay = it.LaiSuatVay,
                    TuNgay = it.TuNgay,
                    DenNgay = it.DenNgay,
                    NgayTraNoCuoiCung = it.NgayTraNoCuoiCung,
                    NguonVon = it.NguonVon.TenNguonVon,
                    NoiDung = it.NoiDung,
                    TraXong = it.TraXong,
                    TienVay = it.SoTienVay != null ? it.SoTienVay.Value.ToString("N0"):"",
                    PhuongXa = it.HoiVien.DiaBanHoatDong!.PhuongXa.TenPhuongXa,
                    QuanHuyen = it.HoiVien.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen,
                }).ToList();
                model.ForEach(it => {
                    it.SoThangQuaHan = !it.TraXong? GetMonthsBetween(it.NgayTraNoCuoiCung!.Value, DateTime.Now.Date):null;
                });
                model = model.OrderBy(it => it.QuanHuyen).ThenBy(it => it.PhuongXa).ToList();
                return PartialView(model);
            });
        }
        private int? GetMonthsBetween(DateTime startDate, DateTime endDate)
        {
            // Ensure startDate is earlier than endDate
            if (startDate > endDate)
            {
                return null;
            }
            int monthsApart = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month;

            return monthsApart;
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create()
        {
            VayVonVM item = new VayVonVM();
            HoiVienInfo nhanSu = new HoiVienInfo();

            item.HoiVien = nhanSu;
            CreateViewBag();
            return View(item);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VayVonVM obj)
        {
            CheckError(obj);
            return ExecuteContainer(() =>
            {
                var add = new VayVon { 
                    IDVayVon = Guid.NewGuid(),
                    IDHoiVien = obj.HoiVien.IdCanbo!.Value,
                    SoTienVay = obj.SoTienVay != null ? long.Parse(obj.SoTienVay.Replace(",", "")) : null,
                    ThoiHangChoVay = obj.ThoiHangChoVay,
                    NgayTraNoCuoiCung = obj.NgayTraNoCuoiCung,
                    LaiSuatVay = obj.LaiSuatVay,
                    TuNgay = obj.TuNgay,
                    DenNgay = obj.DenNgay,
                    NoiDung = obj.NoiDung,
                    GhiChu =  obj.GhiChu,
                    Actived = true,
                    TraXong = false,
                    MaNguonVon = obj.MaNguonVon,
                    CreatedAccountId = AccountId(),
                    CreatedTime = DateTime.Now
                };
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.VayVons.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.VayVon.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id)
        {
            var item = _context.VayVons.SingleOrDefault(it => it.IDVayVon == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            VayVonVM obj = new VayVonVM();


            obj.ID = item.IDVayVon;
            obj.SoTienVay = item.SoTienVay != null ? item.SoTienVay!.Value.ToString("N0") : null;
            obj.LaiSuatVay = item.LaiSuatVay;
            obj.ThoiHangChoVay = item.ThoiHangChoVay;
            obj.NgayTraNoCuoiCung = item.NgayTraNoCuoiCung;
            obj.TuNgay = item.TuNgay;
            obj.DenNgay = item.DenNgay;
            obj.MaNguonVon = item.MaNguonVon;
            obj.NgayTraNoCuoiCung = item.NgayTraNoCuoiCung;
            obj.NoiDung = item.NoiDung;
            obj.GhiChu = item.GhiChu;
            obj.HoiVien = Function.GetThongTinHoiVien(item.IDHoiVien,_context);
            obj.TraXong = item!.TraXong! == null ? false : item.TraXong;
            CreateViewBag(MaNguonVon: obj.MaNguonVon);
            return View(obj);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(VayVonVM obj)
        {
            CheckError(obj);
            return ExecuteContainer(() =>
            {
                var edit = _context.VayVons.SingleOrDefault(it => it.IDVayVon == obj.ID);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.VayVon.ToLower())
                    });
                }
                else
                {
                    edit.NoiDung = obj.NoiDung;
                    edit.SoTienVay = obj.SoTienVay != null ? long.Parse(obj.SoTienVay.Replace(",", "")) : null;
                    edit.LaiSuatVay = obj.LaiSuatVay;
                    edit.ThoiHangChoVay = obj.ThoiHangChoVay;
                    edit.NgayTraNoCuoiCung = obj.NgayTraNoCuoiCung;
                    edit.MaNguonVon = obj.MaNguonVon;
                    edit.TuNgay = obj.TuNgay;
                    edit.DenNgay = obj.DenNgay;
                    edit.GhiChu = obj.GhiChu;
                    edit.TraXong = obj.TraXong;
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.VayVon.ToLower())
                    });
                }
            });
        }
        #endregion Edit
        #region Del
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.VayVons.SingleOrDefault(p => p.IDVayVon == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.VayVon.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.VayVon.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Report
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Report()
        {
            return View(new VayVonQuaHanSearchVM { Ngay = DateTime.Now, SoThang = 3 });
        }
        [HttpGet]
        public IActionResult _ReportData(VayVonQuaHanSearchVM search)
        {
            return ExecuteSearch(() =>
            {
                var data = (from hvht in _context.VayVons
                            join hv in _context.CanBos on hvht.IDHoiVien equals hv.IDCanBo
                            join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                            where pv.AccountId == AccountId()
                            select hvht).Distinct().Include(it => it.NguonVon).Include(it => it.HoiVien).ThenInclude(it => it.DiaBanHoatDong)
                            .ThenInclude(it => it.QuanHuyen).ThenInclude(it => it.PhuongXas).AsQueryable();

                var model = data.Where(it => it.NgayTraNoCuoiCung!.Value.AddMonths(search.SoThang) < search.Ngay).Select(it => new VayVonDetailVM
                {
                    ID = it.IDVayVon,
                    MaHV = it.HoiVien.MaCanBo!,
                    TenHV = it.HoiVien.HoVaTen,
                    SoTienVay = it.SoTienVay,
                    ThoiHangChoVay = it.ThoiHangChoVay,
                    LaiSuatVay = it.LaiSuatVay,
                    TuNgay = it.TuNgay,
                    DenNgay = it.DenNgay,
                    NgayTraNoCuoiCung = it.NgayTraNoCuoiCung,
                    NguonVon = it.NguonVon.TenNguonVon,
                    NoiDung = it.NoiDung,
                    TraXong = it.TraXong,
                    TienVay = it.SoTienVay != null ? it.SoTienVay.Value.ToString("N0") : "",
                    PhuongXa = it.HoiVien.DiaBanHoatDong!.PhuongXa.TenPhuongXa,
                    QuanHuyen = it.HoiVien.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen,
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Report
        #region Import
        public IActionResult _Import()
        {
            return PartialView();
        }
        [HoiNongDanAuthorization]
        public IActionResult Import()
        {

            DataSet ds = GetDataSetFromExcel();
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                List<string> errorList = new List<string>();
                List<VayVon> addVayVons = new List<VayVon>();
                return ExcuteImportExcel(() =>
                {
                    const TransactionScopeOption opt = new TransactionScopeOption();

                    TimeSpan span = new TimeSpan(0, 0, 30, 30);
                    using (TransactionScope ts = new TransactionScope(opt, span))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (dt.Rows.IndexOf(row) >= startIndex - 1)
                            {
                                if (row[0] == null || row[0].ToString() == "")
                                    break;
                                CheckTemplate(row.ItemArray!, error: errorList, vayvons: addVayVons);
                            }

                        }
                        if (errorList != null && errorList.Count > 0)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = false,
                                Data = String.Join("<br/>", errorList)
                            });
                        }
                        if (addVayVons.Count > 0)
                            _context.VayVons.AddRange(addVayVons);
                        int stt = _context.SaveChanges();
                        if (stt > 0)
                        {
                            ts.Complete();
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = true,
                                Data = LanguageResource.ImportSuccess + " " + stt.ToString()
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = false,
                                Data = "Không import thành công"
                            });
                        }
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
        private void CheckTemplate(object[] row, List<VayVon> vayvons, List<String> error)
        {
            List<string> loi = new List<string>();
            string stt = "",hoTen = "";
            VayVon add = new VayVon();
            add.IDVayVon = Guid.NewGuid();
            string value = "";
            for (int i = 0; i < row.Length; i++)
            {
                value = row[i] == null ? "" : row[i].ToString().Trim().Replace(System.Environment.NewLine, string.Empty).Trim();
                switch (i)
                {

                    case 0:
                        stt = value;
                        break;
                    case 1:
                        hoTen = value;
                        
                        break;
                    case 2:
                        if (String.IsNullOrWhiteSpace(value))
                        {
                            loi.Add(String.Format("Dòng thông tin {0} chưa nhập số CCCD", stt ));
                        }
                        else
                        {
                            var hoiVien = ChekcHoiVien(value);
                            if (hoiVien == null)
                            {
                                loi.Add(String.Format(LanguageResource.ErrorImportChiToHoiNganhNghe2, stt + " " + hoTen));
                            }
                            else
                            {
                                add.IDHoiVien = hoiVien.IDCanBo;
                            }
                        }
                        
                        break;
                    case 5:
                        //Nguồn Vốn (*)
                        if (String.IsNullOrWhiteSpace(value))
                        {
                            loi.Add(String.Format("Dòng thông tin {0} chưa nhập nguồn vốn", stt));
                        }
                        else
                        {
                            var check = _context.NguonVons.SingleOrDefault(it => it.TenNguonVon == value);
                            if (check == null)
                            {
                                loi.Add(String.Format("Dòng thông tin {0} nguồn vốn {0} không tồn tại", stt, value));
                            }
                            else
                            {
                                add.MaNguonVon = check.MaNguonVon;
                            }
                        }
                       
                        break;
                    case 6:
                        //Số Tiền (*)
                        if (String.IsNullOrWhiteSpace(value))
                        {
                            loi.Add(String.Format("Dòng thông tin {0} chưa nhập số tiền", stt));
                        }
                        try
                        {
                            value = value.Replace(",", "");
                            add.SoTienVay = long.Parse(value);
                        }
                        catch (Exception)
                        {

                            loi.Add(String.Format("Dòng thông tin {0} số tiền không hợp lệ", stt));
                        }
                        break;
                    case 7:
                        //Nội Dung Vay(*)
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            add.NoiDung = value;
                        }
                        break;
                    case 8:
                        //Từ Ngày (*)
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {

                                add.TuNgay = DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch (Exception)
                            {

                                loi.Add(string.Format("Dòng  {0} có thông tin từ ngày vay vốn không hợp lệ", stt));
                            }
                        }
                        else
                        {
                            loi.Add(string.Format("Dòng {0} có thông tin từ ngày vay vốn  chưa nhập", stt));
                        }
                        break;
                    case 9:
                        //Thời Hạng Vay (*)
                        if (String.IsNullOrWhiteSpace(value))
                        {
                            loi.Add(String.Format("Dòng thông tin {0} chưa nhập thời hạng vay", stt));
                        }
                        try
                        {
                            add.ThoiHangChoVay = int.Parse(value);
                        }
                        catch (Exception)
                        {

                            loi.Add(String.Format("Dòng thông tin {0} có thời hạng vay không hợp lệ", stt));
                        }
                        break;
                    case 10:
                        //Lãi Suất Vay
                        if (String.IsNullOrWhiteSpace(value))
                        {
                            loi.Add(String.Format("Dòng thông tin {0} chưa nhập lãi suất vay", stt));
                        }
                        try
                        {
                            add.LaiSuatVay = double.Parse(value);
                        }
                        catch (Exception)
                        {

                            loi.Add(String.Format("Dòng thông tin {0} có lãi suất vay không hợp lệ", stt));
                        }
                        break;
                    case 11:
                        //Ngày Trả Nợ Cuối Cùng(*)
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                add.NgayTraNoCuoiCung = DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch (Exception)
                            {
                                loi.Add(string.Format("dòng có thông tin ngày trả nợ {0} không hợp lệ", stt));
                            }
                        }
                        else
                        {
                            loi.Add(string.Format("dòng có thông tin ngày trả nợ {0} chưa nhập", stt));
                        }
                        break;
                    case 12:
                        //Nội Dung Vay(*)
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            add.GhiChu = value;
                        }
                        break;
                }

            }
            if (loi.Count == 0)
            {
                add.DenNgay = add.TuNgay!.Value.AddMonths(add.ThoiHangChoVay!.Value);
                vayvons.Add(add);
            }
            else
            {
                error.AddRange(loi);
            }
        }
        [NonAction]
        private DataSet GetDataSetFromExcel()
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
        private CanBo? ChekcHoiVien(string? soCCCD)
        {
            var data = (from cb in _context.CanBos
                        join pv in _context.PhamVis on cb.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                        where pv.AccountId == AccountId()
                        && cb.IsHoiVien == true && cb.HoiVienDuyet == true
                        select cb).SingleOrDefault(it => it.IsHoiVien == true && it.SoCCCD == soCCCD && it.isRoiHoi != true && it.HoiVienDuyet == true);
            return data;
        }
        #endregion Import
        #region Export 
        [HoiNongDanAuthorization]
        public IActionResult ExportCreate()
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, filemau);
            List<HoiVienVayVonExcelVM> data = new List<HoiVienVayVonExcelVM>();
            return Export(data, url); 
        }
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit(VayVonSearchVM search)
        {
            var data = (from hvht in _context.VayVons
                        join hv in _context.CanBos on hvht.IDHoiVien equals hv.IDCanBo
                        join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                        where pv.AccountId == AccountId()
                        select hvht).Distinct().Include(it => it.NguonVon).Include(it => it.HoiVien).ThenInclude(it => it.DiaBanHoatDong).ThenInclude(it => it.QuanHuyen).ThenInclude(it => it.PhuongXas).AsQueryable();

            if (!String.IsNullOrWhiteSpace(search.SoCCCD))
            {
                data = data.Where(it => it.HoiVien.SoCCCD == search.SoCCCD);
            }
            if (!String.IsNullOrEmpty(search.TenHV) && !String.IsNullOrWhiteSpace(search.TenHV))
            {
                data = data.Where(it => it.HoiVien.HoVaTen.Contains(search.TenHV));
            }
            if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen))
            {
                data = data.Where(it => it.HoiVien.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
            }
            if (search.MaNguonVon != null)
            {
                data = data.Where(it => it.MaNguonVon == search.MaNguonVon);
            }
            if (search.Actived != null)
            {
                data = data.Where(it => it.Actived == search.Actived);
            }
            if (search.TuNgay != null)
            {
                data = data.Where(it => it.TuNgay >= search.TuNgay);
            }

            if (search.DenNgay != null)
            {
                data = data.Where(it => it.TuNgay <= search.DenNgay);
            }
            if (search.TraXong == "01")
            {
                data = data.Where(it => it.TraXong == false);
            }
            if (search.TraXong == "02" && search.SoThang == null)
            {
                data = data.Where(it => it.TraXong == true);
            }
            if (search.MaDiaBanHoiVien != null)
            {
                data = data.Where(it => it.HoiVien.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
            }
            if (search.NamVayVon != null)
            {
                data = data.Where(it => it.TuNgay!.Value.Year == search.NamVayVon);
            }
            if (search.SoThang != null)
            {
                data = data.Where(it => it.TraXong != true && it.NgayTraNoCuoiCung!.Value.AddMonths(search.SoThang.Value) < DateTime.Now.Date);
            }
            var model = data.Select(it => new HoiVienVayVonExcelVM
            {
                //ID = it.IDVayVon,
                HoVaTen = it.HoiVien.HoVaTen,
                SoCCCD = it.HoiVien.SoCCCD!,
                QuanHuyen = it.HoiVien.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen,
                PhuongXa = it.HoiVien.DiaBanHoatDong!.PhuongXa.TenPhuongXa,
                NguonVon = it.NguonVon.TenNguonVon,
                SoTienVay = it.SoTienVay.Value.ToString("N0"),
                NoiDung = it.NoiDung,
                TuNgay = it.TuNgay!.Value.ToString("dd/MM/yyyy"),
                ThoiHangChoVay = it.ThoiHangChoVay,
                LaiSuatVay = it.LaiSuatVay,
                NgayTraNoCuoiCung = it.NgayTraNoCuoiCung!.Value.ToString("dd/MM/yyyy"),
                ChoOHienNay = it.HoiVien.ChoOHienNay + (String.IsNullOrWhiteSpace(it.HoiVien.ChoOHienNay_XaPhuong) == false ? "," + it.HoiVien.ChoOHienNay_XaPhuong : "") + 
                (String.IsNullOrWhiteSpace(it.HoiVien.ChoOHienNay_QuanHuyen) == false ? ", " + it.HoiVien.ChoOHienNay_QuanHuyen : ""),
                GhiChu = it.GhiChu  
            }).ToList();

            model = model.OrderBy(it => it.QuanHuyen).ThenBy(it => it.PhuongXa).ToList();
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, filemau);
            byte[] filecontent = ClassExportExcel.ExportExcel(model, startIndex + 1, url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "HoiVienVayVon" + DateTime.Now.ToString("hh_mm_ss"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        public FileContentResult Export(List<HoiVienVayVonExcelVM> data, string url)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            //columns.Add(new ExcelTemplate() { ColumnName = "ID", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoCCCD", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "QuanHuyen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "PhuongXa", isAllowedToEdit = true, isText = true });


            var nguonVon = _context.NguonVons.Where(it => it.Actived == true).ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaNguonVon.ToString(), Name = x.TenNguonVon }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "NguonVon", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = nguonVon, TypeId = ConstExcelController.StringId, Title = "Nguồn Vốn" });


            columns.Add(new ExcelTemplate() { ColumnName = "SoTienVay", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NoiDung", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "TuNgay", isDateTime = true, isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DenNgay", isDateTime = true, isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ThoiHangChoVay", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "isAllowedToEdit", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayTraNoCuoiCung", isDateTime = true, isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GhiChu", isAllowedToEdit = true, isText = true });
          
            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.VayVon);
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
            byte[] filecontent = ClassExportExcel.ExportExcel(url, data, columns, heading, true, startIndex);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export 
        #region Helper
        private void CreateViewBag(Guid? MaNguonVon = null, Guid? MaHinhThucHoTro = null)
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaNguonVon = fnViewBag.NguonVon(value: MaNguonVon);
        }
        [NonAction]
        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaHinhThucHoTro = fnViewBag.HinhThucHoTro();

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
            ViewBag.MaNguonVon = fnViewBag.NguonVon();
        }
        private void CheckError(VayVonVM obj)
        {
            if (obj.HoiVien.IdCanbo == null)
            {
                ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
            }
            if (obj.DenNgay < obj.TuNgay)
            {
                ModelState.AddModelError("DenNgay", "Từ ngày đến ngày không hợp lệ");
            }
            if (obj.ThoiHangChoVay < 0)
            {
                ModelState.AddModelError("ThoiHangChoVay", "Số tháng cho vay không hợp lệ");
            }
        }
        #endregion Helper
    }
}
