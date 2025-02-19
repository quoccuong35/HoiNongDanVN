using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models.Entitys.NhanSu;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Constant;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Entity.Core.Mapping;
using System.Data;
using System.Transactions;
using Microsoft.Extensions.Hosting;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HVKhenThuongController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        const string controllerCode = ConstExcelController.HoiVien_CLB_DN_MH_HTX_THT;
        const int startIndex = 5;
        private string filemau = @"upload\filemau\KhenThuongHoiVien.xlsx";
        public HVKhenThuongController(AppDbContext context, IWebHostEnvironment hostEnvironment) : base(context) { _hostEnvironment = hostEnvironment; }
        [HoiNongDanAuthorization]
        #region Index
        public IActionResult Index()
        {
            KhenThuongSearchVN model = new KhenThuongSearchVN();
            model.DenNam = DateTime.Now.Year;

            CreateViewBagSearch();
            return View(model);
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(KhenThuongSearchVN search)
        {
            return ExecuteSearch(() =>
            {
                var model = (from kt in _context.QuaTrinhKhenThuongs
                              join hv in _context.CanBos on kt.IDCanBo equals hv.IDCanBo
                              join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                              where kt.Loai == "02" || String.IsNullOrWhiteSpace(kt.Loai) &&
                              kt.IsHoiVien == true
                              && pv.AccountId == AccountId()
                              select kt).Include(it => it.CanBo).ThenInclude(it => it.DiaBanHoatDong).ThenInclude(it=>it!.QuanHuyen).
                              Include(it => it.DanhHieuKhenThuong).Include(it => it.HinhThucKhenThuong).Include(it=>it.capKhenThuong).AsQueryable();
                if (!String.IsNullOrEmpty(search.MaDanhHieuKhenThuong) && !String.IsNullOrWhiteSpace(search.MaDanhHieuKhenThuong))
                {
                    model = model.Where(it => it.MaDanhHieuKhenThuong == search.MaDanhHieuKhenThuong);
                }
               // model = model.Include(it => it.CanBo).ThenInclude(it => it.DiaBanHoatDong).Include(it => it.DanhHieuKhenThuong).Include(it => it.HinhThucKhenThuong);
                if (!String.IsNullOrEmpty(search.SoCCCD) && !String.IsNullOrWhiteSpace(search.SoCCCD))
                {
                    model = model.Where(it => it.CanBo.SoCCCD == search.SoCCCD);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen) && !String.IsNullOrWhiteSpace(search.HoVaTen))
                {
                    model = model.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
                }
                if (search.MaDiaBanHoiVien != null)
                {
                    model = model.Where(it => it.CanBo.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
                }
                if (!String.IsNullOrEmpty(search.MaQuanHuyen))
                {
                    model = model.Where(it => it.CanBo.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
                }
                if (!String.IsNullOrEmpty(search.MaCapKhenThuong))
                {
                    model = model.Where(it => it.MaCapKhenThuong == search.MaCapKhenThuong);
                }
                if (search.TuNam != null)
                {
                    model = model.Where(it=>it.Nam >= search.TuNam);
                }
                if (search.DenNam != null)
                {
                    model = model.Where(it => it.Nam <= search.DenNam);
                }
                var data = model.Select(it => new HVKhenThuongDetailVM
                {
                    IDQuaTrinhKhenThuong = it.IDQuaTrinhKhenThuong,
                    MaCanBo = it.CanBo.MaCanBo!,
                    HoVaTen = it.CanBo.HoVaTen,
                    Nam = it.Nam,
                    NoiDung = it.NoiDung,
                    GhiChu = it.GhiChu,
                    TenDanhHieuKhenThuong = it.DanhHieuKhenThuong.TenDanhHieuKhenThuong,
                    DiaBanHND = it.CanBo.DiaBanHoatDong!.TenDiaBanHoatDong,
                    QuanHuyen = it.CanBo.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                    CapKhenThuong = it.capKhenThuong!.TenCapKhenThuong
                }).OrderBy(it=>it.QuanHuyen).ThenBy(it=>it.DiaBanHND).ToList();
                return PartialView(data);
            });
        }
        #endregion Index
        #region Cretae
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create()
        {
            KhenThuongVM khenThuong = new KhenThuongVM();
            HoiVienInfo nhanSu = new HoiVienInfo();
            khenThuong.HoiVien = nhanSu;
            CreateViewBag();
            return View(khenThuong);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(KhenThuongVMMT obj)
        {
            CheckError(obj);
            return ExecuteContainer(() => {
                QuaTrinhKhenThuong add = new QuaTrinhKhenThuong();
                add = obj.GetKhenThuong(add);
                add.IDQuaTrinhKhenThuong = Guid.NewGuid();
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                add.IsHoiVien = true;
                _context.Attach(add).State = EntityState.Modified;
                _context.QuaTrinhKhenThuongs.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.KhenThuong.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id)
        {
            var item = _context.QuaTrinhKhenThuongs.SingleOrDefault(it => it.IDQuaTrinhKhenThuong == id && it.IsHoiVien == true);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            KhenThuongVM obj = new KhenThuongVM();
            
            obj.IDQuaTrinhKhenThuong = item.IDQuaTrinhKhenThuong;
            obj.MaHinhThucKhenThuong = item.MaHinhThucKhenThuong!;
            obj.MaDanhHieuKhenThuong = item.MaDanhHieuKhenThuong;
            obj.SoQuyetDinh = item.SoQuyetDinh!;
            obj.NgayQuyetDinh = item.NgayQuyetDinh;
            obj.NguoiKy = item.NguoiKy;
            obj.NoiDung = item.NoiDung;
            obj.MaCapKhenThuong = item.MaCapKhenThuong;
            obj.Nam = item.Nam;
            obj.GhiChu = item.GhiChu;
            obj.HoiVien = Function.GetThongTinHoiVien(item.IDCanBo,_context);
            CreateViewBag(item.MaHinhThucKhenThuong, item.MaDanhHieuKhenThuong,item.MaCapKhenThuong);
            return View(obj);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(KhenThuongVMMT obj)
        {
            CheckError(obj);
            return ExecuteContainer(() => {
                var edit = _context.QuaTrinhKhenThuongs.SingleOrDefault(it => it.IDQuaTrinhKhenThuong == obj.IDQuaTrinhKhenThuong);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.KhenThuong.ToLower())
                    });
                }
                else
                {
                    edit = obj.GetKhenThuong(edit);
                    edit.LastModifiedTime = DateTime.Now;
                    edit.LastModifiedAccountId = AccountId();
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.KhenThuong.ToLower())
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
                var del = _context.QuaTrinhKhenThuongs.FirstOrDefault(p => p.IDQuaTrinhKhenThuong == id);


                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.KhenThuong.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.KhenThuong.ToLower())
                    });
                }
            });
        }
        #endregion Delete
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
                List<QuaTrinhKhenThuong> addQuaTrinhKhenThuong = new List<QuaTrinhKhenThuong>();
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
                                CheckTemplate(row,  error: errorList,addKhenThuong: addQuaTrinhKhenThuong);
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
                        if (addQuaTrinhKhenThuong.Count > 0)
                            _context.QuaTrinhKhenThuongs.AddRange(addQuaTrinhKhenThuong);
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
        private void CheckTemplate(DataRow row,List<QuaTrinhKhenThuong> addKhenThuong, List<String> error) {
            List<string> loi = new List<string>();
            var add = new QuaTrinhKhenThuong
            {
                IDQuaTrinhKhenThuong = Guid.NewGuid(),
                MaHinhThucKhenThuong = "01",
                IsHoiVien = true,
                CreatedAccountId = AccountId(),
                CreatedTime = DateTime.Now,
                GhiChu = row[9].ToString()
            };
            string khenThuong = "", capKhenThuong = "", soCCCD = "",hoVaTen = "",index = "";
            int nam = 0;
            index = row[0].ToString();
            hoVaTen = row[1].ToString();
            soCCCD = row[2].ToString();
            khenThuong   = row[3].ToString();
            capKhenThuong = row[4].ToString();
            var hoivien = ChekcHoiVien(soCCCD);
            if (String.IsNullOrWhiteSpace(khenThuong))
            {
                loi.Add(String.Format("Thông tin khen thưởng dòng {0} Hình Thức Đạt Được Khen Tặng không hợp lệ", index));
            }
            else
            {
                var check = _context.DanhHieuKhenThuongs.SingleOrDefault(it => it.TenDanhHieuKhenThuong == khenThuong && it.IsHoiVien == true);
                if (check != null)
                {
                    add.MaDanhHieuKhenThuong = check.MaDanhHieuKhenThuong;
                }
                else
                {
                    loi.Add(String.Format("Thông tin khen thưởng dòng {0} Hình Thức Đạt Được Khen Tặng không tồn tại hệ thống", index));
                }
            }
            if (!String.IsNullOrWhiteSpace(capKhenThuong))
            {
                var check = _context.CapKhenThuongs.SingleOrDefault(it => it.TenCapKhenThuong == capKhenThuong);
                if (check != null)
                {
                    add.MaCapKhenThuong = check.MaCapKhenThuong;
                }
                else
                {
                    loi.Add(String.Format("Thông tin khen thưởng dòng {0} Cấp khen thưởng không tồn tại hệ thống", index));
                }
            }
            try
            {
                nam = int.Parse(row[5]!.ToString().Trim());
                add.Nam = nam;
            }
            catch (Exception)
            {

                loi.Add(String.Format("Thông tin năm khen thưởng dòng {0} không hợp lệ", index));
            }


            if (khenThuong == "15" && String.IsNullOrWhiteSpace(capKhenThuong))
            {
                loi.Add(String.Format("Thông tin khen thưởng dòng {0} chưa nhập cấp khen thưởng", index));
            }
            if (hoivien == null)
            {
                loi.Add(String.Format("Không tìm thấy thông tin hội viên dòng {0}", index + " " + hoVaTen));
            }
            else
            {
                add.IDCanBo = hoivien.IDCanBo;

            }
            if (loi.Count == 0)
            {
                addKhenThuong.Add(add);
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
            //List<HoiVienExcelVM> data = new List<HoiVienExcelVM>();
            //return Export(data);
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, filemau);
            List<KhenThuongExcelVM> data = new List<KhenThuongExcelVM>();
            return Export(data, url);
        }
        public IActionResult ExportEdit(KhenThuongSearchVN search)
        {
            var data = new List<KhenThuongExcelVM>();
            var model = (from kt in _context.QuaTrinhKhenThuongs
                         join hv in _context.CanBos on kt.IDCanBo equals hv.IDCanBo
                         join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                         where kt.Loai == "02" || String.IsNullOrWhiteSpace(kt.Loai) &&
                         kt.IsHoiVien == true
                         && pv.AccountId == AccountId()
                         select kt).Include(it => it.CanBo).ThenInclude(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen).
                              Include(it => it.DanhHieuKhenThuong).Include(it => it.HinhThucKhenThuong).Include(it => it.capKhenThuong).AsQueryable();
            if (!String.IsNullOrEmpty(search.MaDanhHieuKhenThuong) && !String.IsNullOrWhiteSpace(search.MaDanhHieuKhenThuong))
            {
                model = model.Where(it => it.MaDanhHieuKhenThuong == search.MaDanhHieuKhenThuong);
            }
            // model = model.Include(it => it.CanBo).ThenInclude(it => it.DiaBanHoatDong).Include(it => it.DanhHieuKhenThuong).Include(it => it.HinhThucKhenThuong);
            if (!String.IsNullOrEmpty(search.SoCCCD) && !String.IsNullOrWhiteSpace(search.SoCCCD))
            {
                model = model.Where(it => it.CanBo.SoCCCD == search.SoCCCD);
            }
            if (!String.IsNullOrEmpty(search.HoVaTen) && !String.IsNullOrWhiteSpace(search.HoVaTen))
            {
                model = model.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
            }
            if (search.MaDiaBanHoiVien != null)
            {
                model = model.Where(it => it.CanBo.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
            }
            if (!String.IsNullOrEmpty(search.MaQuanHuyen))
            {
                model = model.Where(it => it.CanBo.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
            }
            if (!String.IsNullOrEmpty(search.MaCapKhenThuong))
            {
                model = model.Where(it => it.MaCapKhenThuong == search.MaCapKhenThuong);
            }
            if (search.TuNam != null)
            {
                model = model.Where(it => it.Nam >= search.TuNam);
            }
            if (search.DenNam != null)
            {
                model = model.Where(it => it.Nam <= search.DenNam);
            }
            data = model.Select(it => new KhenThuongExcelVM
            {

                HoVaTen = it.CanBo.HoVaTen,
                SoCCCD = it.CanBo.SoCCCD,
                MaDanhHieuKhenThuong = it.DanhHieuKhenThuong.TenDanhHieuKhenThuong,
                MaCapKhenThuong = it.capKhenThuong!.TenCapKhenThuong,
                 Nam = it.Nam,
                
                QuanHuyen = it.CanBo.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen,
                PhuongXa = it.CanBo.DiaBanHoatDong!.TenDiaBanHoatDong,
                ChoOHienNay = it.CanBo.ChoOHienNay + (String.IsNullOrWhiteSpace(it.CanBo.ChoOHienNay_XaPhuong) == false ? "," + it.CanBo.ChoOHienNay_XaPhuong : "") + (String.IsNullOrWhiteSpace(it.CanBo.ChoOHienNay_QuanHuyen) == false ? ", " + it.CanBo.ChoOHienNay_QuanHuyen : ""),
                GhiChu = it.GhiChu


            }).OrderBy(it => it.QuanHuyen).ThenBy(it => it.PhuongXa).ToList();

            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url_excel = Path.Combine(wwwRootPath, filemau);

            return Export(data, url_excel);
        }
        public FileContentResult Export(List<KhenThuongExcelVM> data, string url)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoCCCD", isAllowedToEdit = true, isText = true });


            var danhHieu = _context.DanhHieuKhenThuongs.Where(it=>it.IsHoiVien == true).ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaDanhHieuKhenThuong, Name = x.TenDanhHieuKhenThuong }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaDanhHieuKhenThuong", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = danhHieu, TypeId = ConstExcelController.StringId, Title = "Danh hiệu khen tặng" });

            var capKhenThuong = _context.CapKhenThuongs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaCapKhenThuong, Name = x.TenCapKhenThuong }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaCapKhenThuong", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = capKhenThuong, TypeId = ConstExcelController.StringId, Title = "Cấp khen thưởng" });

          

            columns.Add(new ExcelTemplate() { ColumnName = "Nam", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "QuanHuyen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "PhuongXa", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GhiChu", isAllowedToEdit = true, isText = true });
            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.KhenThuong);
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
        private void CheckError(KhenThuongVMMT obj)
        {
            if (obj.HoiVien.IdCanbo == null)
            {
                ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
            }
            if (obj.MaDanhHieuKhenThuong == "15" || obj.MaDanhHieuKhenThuong == "19") {
                if (String.IsNullOrWhiteSpace(obj.MaCapKhenThuong)) {
                    ModelState.AddModelError("MaDanhHieuKhenThuong", "Chưa chọn cấp khen thưởng");
                }
            }
        }
        private void CreateViewBag(String? MaHinhThucKhenThuong = null, String? MaDanhHieuKhenThuong = null,String? MaCapKhenThng = null)
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaHinhThucKhenThuong = fnViewBag.HinhThucKhenThuong(value:MaHinhThucKhenThuong);

            //  var MenuList = _context.DanhHieuKhenThuongs.Where(it => it.IsHoiVien == true).Select(it => new { MaDanhHieuKhenThuong = it.MaDanhHieuKhenThuong, TenDanhHieuKhenThuong = it.TenDanhHieuKhenThuong }).ToList();
            ViewBag.MaDanhHieuKhenThuong = fnViewBag.DanhHieuKhenThuong(value:MaDanhHieuKhenThuong);
            ViewBag.MaCapKhenThuong = fnViewBag.CapKhenThuong(value: MaCapKhenThng);

        }
        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaHinhThucKhenThuong = fnViewBag.HinhThucKhenThuong();

          //  var MenuList = _context.DanhHieuKhenThuongs.Where(it => it.IsHoiVien == true).Select(it => new { MaDanhHieuKhenThuong = it.MaDanhHieuKhenThuong, TenDanhHieuKhenThuong = it.TenDanhHieuKhenThuong }).ToList();
            ViewBag.MaDanhHieuKhenThuong = fnViewBag.DanhHieuKhenThuong(); ;

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID:AccountId()) ;

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
            ViewBag.MaCapKhenThuong = fnViewBag.CapKhenThuong();
        }
        #endregion Helper
    }
}
