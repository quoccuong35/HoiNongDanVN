using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Transactions;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class CanBoQHXPController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        const string controllerCode = ConstExcelController.CanBo;
        const int startIndex = 6;
        private string subUrl = @"upload\filemau\CanBoQHXaPhuong.xlsx";
        public CanBoQHXPController (AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context) {
            _hostEnvironment = hostEnvironment;
            DateFomat = config.GetSection("SiteSettings:DateFormat").Value.ToString().Split(',');
        }
        #region index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(Guid? MaDiaBanHoiVien, string? Loai, String? HoVaTen, Guid? MaChucVu, String? MaQuanHuyen)
        {
            return ExecuteSearch(() => {

                var model = _context.CanBos.Include(it=>it.TrinhDoChinhTri)
                .Include(it=>it.TrinhDoTinHoc)
                .Include(it=>it.QuanHuyen)
                .Include(it=>it.DiaBanHoatDong)
                .Include(it=>it.TrinhDoNgoaiNgu)
                .Include(it=>it.Department)
                .Include(it=>it.ChucVu)
                .Include(it=>it.DanToc)
                .Include(it=>it.TonGiao)
                .Include(it=>it.DaoTaoBoiDuongs)
                .Where(it => (it.Level == "20" || it.Level == "30") && it.IsCanBo == true).AsQueryable();
                if (MaDiaBanHoiVien != null) {
                    model = model.Where(it => it.MaDiaBanHoatDong == MaDiaBanHoiVien);
                }
                if (MaChucVu != null)
                {
                    model = model.Where(it => it.MaChucVu == MaChucVu);
                }
                if (Loai != null && Loai !="")
                {
                    model = model.Where(it => it.Level == Loai);
                }
                if (HoVaTen != null && HoVaTen != "")
                {
                    model = model.Where(it => it.HoVaTen.Contains(HoVaTen));
                }
                if (!String.IsNullOrWhiteSpace(MaQuanHuyen)) {
                    model = model.Where(it => it.MaQuanHuyen == MaQuanHuyen);
                }
                var nhomquyens = _context.AccountInRoleModels
                    .Join(_context.RolesModels,
                        role => role.RolesId,
                        ac => ac.RolesId,
                        (ac, role) => new { ac, role })
                    .Where(it => it.ac.AccountId == AccountId()).Select(it => it.role.RolesCode.ToLower()).ToList();
                var phamvis = _context.PhamVis
                        .Join(_context.DiaBanHoatDongs,
                        db => db.MaDiabanHoatDong,
                        pv => pv.Id,
                        (db, pv) => new { pv, db }).Where(it => it.db.AccountId == AccountId()).Select(it => new { it.pv.MaQuanHuyen, it.pv.Id }).ToList();
                if (nhomquyens.Contains("admin"))
                {
                }
                else if (nhomquyens.Contains("quanly") || nhomquyens.Contains("nguoidung")) {
                    // quan ly phuong xa
                    model = model.Where(it => phamvis.Select(it => it.Id).ToList().Contains(it.MaDiaBanHoatDong!.Value));
                }
                else if(nhomquyens.Contains("quanly_quanhuyen"))
                {
                    model = model.Where(it => phamvis.Select(it => it.MaQuanHuyen).Distinct().ToList().Contains(it.MaQuanHuyen!));
                }
                var data = model.Select(it => new CanBoQHXPDetailVM {
                    IDCanBo = it.IDCanBo,
                    MaCanBo = it.MaCanBo,
                    HoVaTen = it.HoVaTen,
                    NgaySinh = it.NgaySinh,
                    GioiTinh = it.GioiTinh == 0 ? false : true,
                    //TenDonVi = it.QuanHuyen.TenQuanHuyen,
                    TenChucVu = it.ChucVu!.TenChucVu,
                    TenDanToc = it.DanToc!.TenDanToc,
                    TenTonGiao = it.TonGiao!.TenTonGiao,
                    NoiSinh = it.NoiSinh,
                    ChoOHienNay = it.ChoOHienNay + (String.IsNullOrWhiteSpace(it.ChoOHienNay_XaPhuong) == false?"," + it.ChoOHienNay_XaPhuong:"")  + (String.IsNullOrWhiteSpace(it.ChoOHienNay_QuanHuyen ) == false? ", " +it.ChoOHienNay_QuanHuyen:""),
                    NgayvaoDangDuBi = it.NgayvaoDangDuBi,
                    NgayVaoDangChinhThuc = it.NgayVaoDangChinhThuc,
                    ChuyenNganh = it.ChuyenNganh!,
                    MaTrinhDoChinhTri = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                    TenTrinhDoNgoaiNgu = it.TrinhDoNgoaiNgu!.TenTrinhDoNgoaiNgu,
                    TenTrinhDoTinHoc = it.TrinhDoTinHoc!.TenTrinhDoTinHoc,
                    NgayThamGiaCongTac = it.NgayThamGiaCongTac,
                    ThamGiaBanChapHanh = it.IsBanChapHanh == true ? "X" : "",
                    ThamGiaBTV = it.ThamGiaBTV == true ? "X" : "",
                    UBKT = it.UBKT == true ? "X" : "",
                    HuyenUyVien = it.HuyenUyVien == true ? "X" : "",
                    DangUyVien = it.DangUyVien == true ? "X" : "",
                    HDNNCapHuyen = it.HDNNCapHuyen == true ? "X" : "",
                    HDNNCapXa = it.HDNNCapXa == true ? "X" : "",
                    NVCTTW = (it.DaoTaoBoiDuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaHinhThucDaoTao == "03").Count()) > 0 ? "X" : "",
                    GiangVienKiemChuc = (it.DaoTaoBoiDuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaHinhThucDaoTao == "04").Count()) > 0 ? "X" : "",
                    DanhGiaCBCC = it.DanhGiaCBCC,
                    DanhGiaDangVien = it.DanhGiaDangVien,
                    Cap = it.Level == "20" ? "HUYỆN QUẬN" : "XÃ PHƯỜNG, THỊ TRẤN",
                    GhiChu = it.GhiChu,
                    DonVi = it.QuanHuyen!.TenQuanHuyen + " " + it.DiaBanHoatDong!.TenDiaBanHoatDong,
                }).ToList();
                return PartialView(data);
            });
        }
        #endregion Index
        #region Create
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Create() {
            CanBoQHXPVM model = new CanBoQHXPVM();
            model.HinhAnh = @"\Images\login.png";
            UpdateViewBag();
            return View(model);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CanBoQHXPMTVM obj, IFormFile? avtFileInbox) {
            return ExecuteContainer(() => {
                CanBo add = new CanBo();
                add = obj.getCanBo(add);
                add.MaTinhTrang = "01";
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                add.IDCanBo = Guid.NewGuid();
                if (avtFileInbox != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = add.MaCanBo!;
                    var uploads = Path.Combine(wwwRootPath, @"Images\canbo");

                    bool folderExists = System.IO.Directory.Exists(uploads);
                    if (!folderExists)
                        System.IO.Directory.CreateDirectory(uploads);

                    var extension = Path.GetExtension(avtFileInbox.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        avtFileInbox.CopyTo(fileStream);
                    }
                    add.HinhAnh = @"\Images\canbo\" + fileName + extension;
                }
                _context.Attach(add).State = EntityState.Modified;
                _context.CanBos.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.CanBoHNDTP.ToLower())
                });
            });
        }
        #endregion Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
            var item = _context.CanBos.SingleOrDefault(it => it.IDCanBo == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            CanBoQHXPMTVM method = new CanBoQHXPMTVM();
            CanBoQHXPVM canbo = method.SetCanBo(item);
            if (!String.IsNullOrWhiteSpace(canbo.HinhAnh))
            {
                canbo.HinhAnh = @"\Images\login.png";
            }
            UpdateViewBag(item.IdDepartment, item.MaChucVu,item.MaDanToc,item.MaTonGiao, item.MaTrinhDoChuyenMon, item.MaTrinhDoChinhTri, item.MaTrinhDoNgoaiNgu, item.MaTrinhDoTinHoc,item.Level, item.MaTrinhDoHocVan);
            return View(canbo);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CanBoQHXPMTVM obj , IFormFile? avtFileInbox) {
            return ExecuteContainer(() => {
                var edit = _context.CanBos.SingleOrDefault(it => it.IDCanBo == obj.IDCanBo);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.CanBoHNDTP.ToLower())
                    });
                }
                edit = obj.getCanBo(edit);
                edit.MaTinhTrang = "01";
                edit.LastModifiedTime = DateTime.Now;
                edit.LastModifiedAccountId = AccountId();
                if (avtFileInbox != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = edit.MaCanBo!;
                    var uploads = Path.Combine(wwwRootPath, @"Images\canbo");

                    bool folderExists = System.IO.Directory.Exists(uploads);
                    if (!folderExists)
                        System.IO.Directory.CreateDirectory(uploads);

                    var extension = Path.GetExtension(avtFileInbox.FileName);
                    if (obj.HinhAnh != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, edit.HinhAnh!.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        avtFileInbox.CopyTo(fileStream);
                    }
                    edit.HinhAnh = @"\Images\canbo\" + fileName + extension;
                }
                HistoryModelRepository history = new HistoryModelRepository(_context);
                history.SaveUpdateHistory(edit.IDCanBo.ToString(), AccountId()!.Value, edit);
                _context.Entry(edit).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.CanBoHNDTP.ToLower())
                });
            });
        }
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
                    //_context.Entry(accountInRoleModels).State = EntityState.Deleted;
                    //_context.Entry(account).State = EntityState.Deleted;
                    if (del.HinhAnh != null && del.HinhAnh != "")
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        var oldImagePath = Path.Combine(wwwRootPath, del.HinhAnh!.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    _context.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.CanBo.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.CanBo.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Import Excel 
        public IActionResult _Import()
        {
            CreateViewBag();
            return PartialView();
        }
        [HoiNongDanAuthorization]
        public IActionResult Import(String Loai,String MaQuanHuyen, Guid? MaDiaBanHoiVien)
        {
            DataSet ds = GetDataSetFromExcel();
            List<string> errorList = new List<string>();
            if (MaQuanHuyen == null) {

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = false,
                    Data = "Chưa chọn quận huyện "
                });
            }
            if (MaDiaBanHoiVien == null && Loai =="30")
            {

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = false,
                    Data = "Chưa chọn địa bàn hội viên"
                });
            }
            return ExcuteImportExcel(() => {
                if (ds.Tables.Count > 0)
                {
                    int iCapNhat = 0;
                    using (TransactionScope ts = new TransactionScope())
                    {
                        DataTable dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            //string aa = dr.ItemArray[0].ToString();
                            if (dt.Rows.IndexOf(dr) >= startIndex - 1)
                            {
                                if (!string.IsNullOrEmpty(dr.ItemArray[0]!.ToString()))
                                {
                                    var data = CheckTemplate(dr.ItemArray!);
                                    if (!string.IsNullOrEmpty(data.Error))
                                    {
                                        errorList.Add(data.Error);
                                    }
                                    else
                                    {
                                        // Tiến hành cập nhật
                                        data.CanBo.Level = Loai;
                                        data.CanBo.MaQuanHuyen = MaQuanHuyen;
                                        data.CanBo.MaDiaBanHoatDong = MaDiaBanHoiVien;
                                        string result = ExecuteImportExcelCanBo(data);
                                        if (result != LanguageResource.ImportSuccess)
                                        {
                                            errorList.Add(result);
                                        }
                                        else iCapNhat++;
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
                            Data = LanguageResource.ImportSuccess + " " + iCapNhat.ToString()
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
        [NonAction]
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
                return null;
            }
        }
        #endregion Import Excel
        #region Export
        [HoiNongDanAuthorization]
        public IActionResult ExportCreate()
        {
            List<CanBoQHXPExcelVM> data = new List<CanBoQHXPExcelVM>();
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, subUrl);
            return Export(data: data, url: url, startIndex: startIndex);
        }
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit(Guid? MaDiaBanHoiVien, string? Loai, String? HoVaTen, Guid? MaChucVu, String? MaQuanHuyen)
        {
            var model = _context.CanBos.Include(it => it.TrinhDoChinhTri)
                .Include(it => it.TrinhDoTinHoc)
                .Include(it => it.TrinhDoNgoaiNgu)
                .Include(it => it.Department)
                .Include(it=>it.TrinhDoHocVan)
                .Include(it=>it.TrinhDoChuyenMon)
                .Include(it => it.ChucVu)
                .Include(it => it.DanToc)
                .Include(it => it.TonGiao)
                .Include(it => it.DaoTaoBoiDuongs)
                .Where(it => (it.Level == "20" || it.Level == "30") && it.IsCanBo == true).AsQueryable();
            if (MaDiaBanHoiVien != null)
            {
                model = model.Where(it => it.MaDiaBanHoatDong == MaDiaBanHoiVien);
            }
            if (MaChucVu != null)
            {
                model = model.Where(it => it.MaChucVu == MaChucVu);
            }
            if (Loai != null && Loai != "")
            {
                model = model.Where(it => it.Level == Loai);
            }
            if (HoVaTen != null && HoVaTen != "")
            {
                model = model.Where(it => it.HoVaTen.Contains(HoVaTen));
            }
            if (!String.IsNullOrWhiteSpace(MaQuanHuyen))
            {
                model = model.Where(it => it.MaQuanHuyen == MaQuanHuyen);
            }
            var data = model.Select(it => new CanBoQHXPExcelVM
            {
                IDCanBo = it.IDCanBo,
                MaCanBo = it.MaCanBo,
                HoVaTen = it.HoVaTen,
                GioiTinh = it.GioiTinh == 0 ? false : true,
                NgaySinh = it.NgaySinh,
                SoCCCD = it.SoCCCD,
                NgayCapCCCD = it.NgayCapCCCD,
                TenChucVu = it.ChucVu!.TenChucVu,
                TenDanToc = it.DanToc!.TenDanToc,
                TenTonGiao = it.TonGiao!.TenTonGiao,
                NoiSinh = it.NoiSinh,
                ChoOHienNay = it.ChoOHienNay,
                ChoOHienNay_XaPhuong = it.ChoOHienNay_XaPhuong,
                ChoOHienNay_QuanHuyen = it.ChoOHienNay_QuanHuyen,
                NgayvaoDangDuBi = it.NgayvaoDangDuBi,
                NgayVaoDangChinhThuc = it.NgayVaoDangChinhThuc,
                MaTrinhDoHocVan = it.TrinhDoHocVan.TenTrinhDoHocVan!,
                MaTrinhDoChuyenMon = it.TrinhDoChuyenMon!.TenTrinhDoChuyenMon!,
                ChuyenNganh = it.ChuyenNganh!,
                MaTrinhDoChinhTri = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                TenTrinhDoNgoaiNgu = it.TrinhDoNgoaiNgu!.TenTrinhDoNgoaiNgu,
                TenTrinhDoTinHoc = it.TrinhDoTinHoc!.TenTrinhDoTinHoc,
                NgayThamGiaCongTac = it.NgayThamGiaCongTac,
                ThamGiaBanChapHanh = it.IsBanChapHanh == true ? "X" : "",
                ThamGiaBTV = it.ThamGiaBTV == true ? "X" : "",
                UBKT = it.UBKT == true ? "X" : "",
                HuyenUyVien = it.HuyenUyVien == true ? "X" : "",
                DangUyVien = it.DangUyVien == true ? "X" : "",
                HDNNCapHuyen = it.HDNNCapHuyen == true ? "X" : "",
                HDNNCapXa = it.HDNNCapXa == true ? "X" : "",
                NVCTTW = (it.DaoTaoBoiDuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaHinhThucDaoTao == "03").Count()) > 0 ? "X" : "",
                GiangVienKiemChuc = (it.DaoTaoBoiDuongs.Where(p => p.IDCanBo == it.IDCanBo && p.MaHinhThucDaoTao == "04").Count()) > 0 ? "X" : "",
                DanhGiaCBCC = it.DanhGiaCBCC,
                DanhGiaDangVien = it.DanhGiaDangVien,
                GhiChu = it.GhiChu
            }).ToList();
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath,subUrl);
            return Export(data, url, startIndex);
        }
        [HoiNongDanAuthorization]
        public FileContentResult Export(List<CanBoQHXPExcelVM> data, string url, int startIndex)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "IDCanBo", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "MaCanBo", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
           
            columns.Add(new ExcelTemplate() { ColumnName = "GioiTinh", isAllowedToEdit = true, isBoolean = true, isComment = true, strComment = "Nam để chữ X" });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinh", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoCCCD", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayCapCCCD", isAllowedToEdit = true, isDateTime = true });



            var chuVu = _context.ChucVus.ToList().Select(x => new DropdownModel { Id = x.MaChucVu, Name = x.TenChucVu }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenChucVu", isAllowedToEdit = true, isDropdownlist = true, DropdownData = chuVu, TypeId = ConstExcelController.GuidId });

            var danToc = _context.DanTocs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaDanToc, Name = x.TenDanToc }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenDanToc", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = danToc, TypeId = ConstExcelController.StringId });

            var tonGiao = _context.TonGiaos.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTonGiao, Name = x.TenTonGiao }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTonGiao", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tonGiao, TypeId = ConstExcelController.StringId });

            columns.Add(new ExcelTemplate() { ColumnName = "NoiSinh", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay_XaPhuong", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay_QuanHuyen", isAllowedToEdit = true, isText = true });

            columns.Add(new ExcelTemplate() { ColumnName = "NgayvaoDangDuBi", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoDangChinhThuc", isAllowedToEdit = true, isDateTime = true });

            var hocVan = _context.TrinhDoHocVans.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoHocVan, Name = x.MaTrinhDoHocVan }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoHocVan", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = hocVan, TypeId = ConstExcelController.StringId });

            var chuyenMon = _context.TrinhDoChuyenMons.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChuyenMon, Name = x.TenTrinhDoChuyenMon }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChuyenMon", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chuyenMon, TypeId = ConstExcelController.StringId });

            columns.Add(new ExcelTemplate() { ColumnName = "ChuyenNganh", isAllowedToEdit = true, isText = true });
            var chinhTri = _context.TrinhDoChinhTris.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChinhTri, Name = x.TenTrinhDoChinhTri }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChinhTri", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chinhTri, TypeId = ConstExcelController.StringId });
            var ngoaiNgu = _context.TrinhDoNgoaiNgus.ToList().Select(x => new DropdownModel { Id = x.MaTrinhDoNgoaiNgu, Name = x.TenTrinhDoNgoaiNgu }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTrinhDoNgoaiNgu", isAllowedToEdit = true, isDropdownlist = true, DropdownData = ngoaiNgu, TypeId = ConstExcelController.GuidId });
            var tinHoc = _context.TrinhDoTinHocs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoTinHoc, Name = x.TenTrinhDoTinHoc }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "TenTrinhDoTinHoc", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tinHoc, TypeId = ConstExcelController.StringId });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayThamGiaCongTac", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ThamGiaBanChapHanh", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ThamGiaBTV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "UBKT", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HuyenUyVien", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DangUyVien", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HDNNCapHuyen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HDNNCapXa", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NVCTTW", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GiangVienKiemChuc", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DanhGiaCBCC", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DanhGiaDangVien", isAllowedToEdit = true, isText = true });

            columns.Add(new ExcelTemplate() { ColumnName = "GhiChu", isAllowedToEdit = true, isText = true });

            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.CanBoHNDQHXP) + " " + DateTime.Now.Year.ToString();
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
        #region Insert/Update data from excel file
        [NonAction]
        private string ExecuteImportExcelCanBo(CanBoQHXPImportExcelVM canBoExcel)
        {
            //Check:
            //1. If MenuId == "" then => Insert
            //2. Else then => Update
            if (canBoExcel.isNullValueId == true)
            {

                if (canBoExcel.daoTaoBoiDuongs.Count() > 0)
                {
                    _context.DaoTaoBoiDuongs.AddRange(canBoExcel.daoTaoBoiDuongs);
                }
                canBoExcel.CanBo.MaTinhTrang = "01";
                _context.Entry(canBoExcel.CanBo).State = EntityState.Added;
            }
            else
            {
                CanBo canBo = _context.CanBos.Where(p => p.IDCanBo == canBoExcel.CanBo.IDCanBo).FirstOrDefault();
                if (canBo != null)
                {
                    canBo = EditCanBo(canBo, canBoExcel.CanBo);
                    HistoryModelRepository history = new HistoryModelRepository(_context);
                    history.SaveUpdateHistory(canBo.IDCanBo.ToString(), AccountId()!.Value, canBo);
                }
                else
                {
                    return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                            LanguageResource.CanBo, canBo.IDCanBo,
                                            string.Format(LanguageResource.Export_ExcelHeader,
                                            LanguageResource.CanBo));
                }
            }
            _context.SaveChanges();
            return LanguageResource.ImportSuccess;
        }

        #endregion Insert/Update data from excel file
        [NonAction]
        private CanBo EditCanBo(CanBo old, CanBo news){
            old.MaCanBo = news.MaCanBo;
            old.HoVaTen = news.HoVaTen;
            old.NgaySinh = news.NgaySinh;
            old.GioiTinh = news.GioiTinh;
            old.IdDepartment = news.IdDepartment;
            old.MaChucVu = news.MaChucVu;
            old.MaDanToc = news.MaDanToc;
            old.MaTonGiao = news.MaTonGiao;
            old.NoiSinh = news.NoiSinh;
            old.ChoOHienNay = news.ChoOHienNay;
            old.NgayvaoDangDuBi = news.NgayvaoDangDuBi;
            old.NgayVaoDangChinhThuc = news.NgayVaoDangChinhThuc;
            old.ChuyenNganh = news.ChuyenNganh;
            old.MaTrinhDoChinhTri = news.MaTrinhDoChinhTri;
            old.MaTrinhDoNgoaiNgu = news.MaTrinhDoNgoaiNgu;
            old.MaTrinhDoTinHoc = news.MaTrinhDoTinHoc;
            old.IsBanChapHanh = news.IsBanChapHanh;
            old.ThamGiaBTV = news.ThamGiaBTV;
            old.UBKT = news.UBKT;
            old.HuyenUyVien = news.HuyenUyVien;
            old.DangUyVien = news.DangUyVien;
            old.HDNNCapHuyen = news.HDNNCapHuyen;
            old.HDNNCapXa = news.HDNNCapXa;
            old.DanhGiaCBCC = news.DanhGiaCBCC;
            old.DanhGiaDangVien = news.DanhGiaDangVien;
            old.GhiChu = news.GhiChu;

            return old;
        }
        #region Check data type 

        private CanBoQHXPImportExcelVM CheckTemplate(object[] row)
        {
            CanBoQHXPImportExcelVM data = new CanBoQHXPImportExcelVM();
            CanBo canBo = new CanBo();
            canBo.MaTinhTrang = "01";
            canBo.IsCanBo = true;
            canBo.Actived = true;
            canBo.IDCanBo = Guid.NewGuid();
            List<DaoTaoBoiDuong> daoTaoBoiDuongs = new List<DaoTaoBoiDuong>();
            int index = 0;
            string value = "";
            for (int i = 0; i < row.Length; i++)
            {
                value = "";
                value = row[i].ToString().Trim();
                //if (value == "")
                //    continue;
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
                            // Mã nhân viên
                            if (string.IsNullOrEmpty(value))
                            {
                                //data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.MaCanBo), index);
                            }
                            else
                            {
                                canBo.MaCanBo = value;
                            }
                            break;
                        case 3:
                            // Họ và tên
                            if (string.IsNullOrEmpty(value))
                            {
                                data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.FullName), index);
                            }
                            else
                            {
                                canBo.HoVaTen = value;
                            }
                            break;
                        case 4:
                            // Giới tính
                            if (string.IsNullOrEmpty(value))
                            {
                                canBo.GioiTinh = GioiTinh.Nữ;
                            }
                            else
                            {
                                canBo.GioiTinh = GioiTinh.Nam;
                            }
                            break;
                        case 5:
                            //  Ngày sinh (*)
                            if (string.IsNullOrEmpty(value) || value == "")
                            {
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0}", LanguageResource.NgaySinh), index);
                            }
                            else
                            {
                                try
                                {
                                    // data.NgaySinh = DateTime.ParseExact(ngaySinh,DateFomat, new CultureInfo("en-US"));
                                    canBo.NgaySinh = value;
                                }
                                catch (Exception)
                                {

                                    data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgaySinh, value, index);
                                }

                            }
                            break;
                        case 6:
                            //  SoCCCD (*)

                            if (!String.IsNullOrWhiteSpace(value))
                            {

                                if (value.Length == 11 && value.Substring(0, 1) != "0")
                                    value = "0" + value;
                                canBo.SoCCCD = value;
                                var checkSoCCCD = _context.CanBos.Where(it => it.SoCCCD == value);
                                if (value.Length != 12)
                                {
                                    data.Error += string.Format("Dữ liệu dòng {0} Số CCCD {1} phải là 12 số!", index, value);
                                }
                                Regex rg = new Regex(@"^\d+$");
                                if (!rg.IsMatch(value))
                                {
                                    data.Error += string.Format("Dữ liệu dòng {0} Số CCCD {1} phải là kiểu số!", index, value);
                                }
                                
                            }
                            break;
                        case 7:
                            // Ngày cấp số CCCD
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                try
                                {
                                    canBo.NgayCapCCCD = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US")).ToString("dd/MM/yyyy");
                                }
                                catch (Exception)
                                {

                                    data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayCapCCCD, value, index);
                                }
                            }
                            break;
                        case 8:
                            //  chức vụ (*)
                            if (string.IsNullOrEmpty(value))
                            {
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.ChucVu), index);
                            }
                            else
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
                        case 9:
                            //  dân tộc (*)
                            if (string.IsNullOrEmpty(value))
                            {
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.DanToc), index);
                            }
                            else
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
                        case 10:
                            //  tôn giáo (*)
                            if (string.IsNullOrEmpty(value))
                            {
                                //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.TonGiao), index);
                            }
                            else
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
                        case 11:
                            // Nơi sinh
                            if (!string.IsNullOrEmpty(value))
                            {
                                canBo.NoiSinh = value;
                            }
                            
                            break;
                        case 12:
                            // Nơi sinh
                            if (!string.IsNullOrEmpty(value))
                            {
                                canBo.ChoOHienNay = value;
                            }
                            
                            break;
                        case 13:
                            // Nơi sinh Xã phướng
                            if (!string.IsNullOrEmpty(value))
                            {
                                canBo.ChoOHienNay_XaPhuong = value;
                            }
                            
                            break;
                        case 14:
                            // Nơi sinh Quận Huyện
                            if (!string.IsNullOrEmpty(value))
                            {
                                canBo.ChoOHienNay_QuanHuyen = value;
                            }
                           
                            break;

                        case 15:
                            // Ngày vào đảng dự bị
                            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                canBo.NgayvaoDangDuBi = value;
                            }
                            break;
                        case 16:
                            // Ngày vào đảng chính thức
                            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                canBo.NgayVaoDangChinhThuc = value;// DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US"));
                                canBo.DangVien = true;
                            }
                            break;
                        case 17:
                            //  Trình Độ Học Vấn (*)
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
                        case 18:
                            //  Trình Độ chuyên mon (*)
                            if (!string.IsNullOrEmpty(value))
                            {
                                var obj = _context.TrinhDoChuyenMons.FirstOrDefault(it => it.TenTrinhDoChuyenMon == value);
                                if (obj != null)
                                {
                                    canBo.MaTrinhDoChuyenMon = obj.MaTrinhDoChuyenMon;
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy trình độ chuyên môn có tên {0} ở dòng số {1} !", value, index);
                                }
                            }
                            break;
                        case 19:
                            // Chuyên ngành
                            if (string.IsNullOrEmpty(value))
                            {
                                //data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.ChuyenNganh), index);
                            }
                            else
                            {
                                canBo.ChuyenNganh = value;
                            }
                            break;
                        case 20:
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
                       
                        case 21:
                            //  TenTrinhDoNgoaiNgu (*)
                            if (!string.IsNullOrEmpty(value))
                            {
                                var obj = _context.TrinhDoNgoaiNgus.FirstOrDefault(it => it.TenTrinhDoNgoaiNgu == value);
                                if (obj != null)
                                {
                                    canBo.MaTrinhDoNgoaiNgu = obj.MaTrinhDoNgoaiNgu;
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy trình độ ngoại ngữ có tên {0} ở dòng số {1} !", value, index);
                                }
                            }
                            break;
                        case 22:
                            //  TenTrinhDoTinHoc (*)
                            if (!string.IsNullOrEmpty(value))
                            {
                                var obj = _context.TrinhDoTinHocs.FirstOrDefault(it => it.TenTrinhDoTinHoc == value);
                                if (obj != null)
                                {
                                    canBo.MaTrinhDoTinHoc = obj.MaTrinhDoTinHoc;
                                }
                                else
                                {
                                    data.Error += string.Format("Không tìm thấy trình độ tin học có tên {0} ở dòng số {1} !", value, index);
                                }
                            }
                            break;
                        case 23:
                            //  TenTrinhDoTinHoc (*)
                            canBo.NgayThamGiaCongTac = value;
                            break;
                        case 24:
                            //  Tham gia BCH (*)
                            if (!String.IsNullOrWhiteSpace(value)) {
                                canBo.IsBanChapHanh = true;
                            }
                            break;
                        case 25:
                            //  TenTrinhDoTinHoc (*)
                            if (!String.IsNullOrWhiteSpace(value))
                                canBo.ThamGiaBTV = true;
                            break;
                         case 26:
                            //  TenTrinhDoTinHoc (*)
                            if (!String.IsNullOrWhiteSpace(value))
                                canBo.UBKT = true;
                            break;
                        case 27:
                            //  TenTrinhDoTinHoc (*)
                            if (!String.IsNullOrWhiteSpace(value))
                                canBo.HuyenUyVien = true;
                            break;
                        case 28:
                            //  TenTrinhDoTinHoc (*)
                            if (!String.IsNullOrWhiteSpace(value))
                                canBo.DangUyVien = true;
                            break;
                        case 29:
                            //  TenTrinhDoTinHoc (*)
                            if (!String.IsNullOrWhiteSpace(value))
                                canBo.HDNNCapHuyen = true;
                            break;
                        case 30:
                            //  TenTrinhDoTinHoc (*)
                            if (!String.IsNullOrWhiteSpace(value))
                                canBo.HDNNCapXa = true;
                            break;
                        case 31:
                            //  ngach luong(*)
                            if (!string.IsNullOrEmpty(value))
                            {
                                daoTaoBoiDuongs.Add(new DaoTaoBoiDuong
                                {
                                    Id = Guid.NewGuid(),
                                    IDCanBo = canBo.IDCanBo,
                                    MaHinhThucDaoTao = "03",
                                    MaLoaiBangCap = "11",
                                    Actived = true,
                                    NoiDungDaoTao = value,
                                    GhiChu = value
                                });
                            }
                            break;
                        case 32:
                            //  bac luong (*)
                            if (!string.IsNullOrEmpty(value))
                            {
                                daoTaoBoiDuongs.Add(new DaoTaoBoiDuong
                                {
                                    Id = Guid.NewGuid(),
                                    IDCanBo = canBo.IDCanBo,
                                    MaHinhThucDaoTao = "04",
                                    MaLoaiBangCap = "11",
                                    Actived = true,
                                    NoiDungDaoTao = value,
                                    GhiChu = value
                                }); 
                            }
                            break;
                        case 33:
                            //  TenTrinhDoTinHoc (*)
                            canBo.DanhGiaCBCC = value;
                            break;
                        case 34:
                            //  TenTrinhDoTinHoc (*)
                            canBo.DanhGiaDangVien = value;
                            break;
                        
                        case 35:
                            // ngày nâng bậc (*)
                            canBo.GhiChu = value;
                            break;
                    }
                }
                catch (Exception)
                {
                    string ss = value;
                    throw;
                }
                data.CanBo = canBo;
                data.daoTaoBoiDuongs = daoTaoBoiDuongs;
            }

            return data;
        }
        #endregion Check data type 
        #region Helper
        private void CreateViewBag() {
            //var DonVi = (from dv in _context.Departments join
            //            nv in _context.CanBos on dv.Id equals nv.IdDepartment
            //            where nv.Level =="20" || nv.Level =="30"
            //            select new { IdDepartment  = dv.Id,Name = dv.Name}).Distinct().ToList();
          //  ViewBag.MaDonVi = new SelectList(DonVi, "IdDepartment", "Name", DonVi);
            var ChucVu = (from dv in _context.ChucVus
                         join
                      nv in _context.CanBos on dv.MaChucVu equals nv.MaChucVu
                         where nv.Level == "20" || nv.Level == "30"
                         select new { MaChucVu = dv.MaChucVu, Name = dv.TenChucVu }).Distinct().ToList();
            ViewBag.MaChucVu = new SelectList(ChucVu, "MaChucVu", "Name");

            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());

            ViewBag.MaChucVu = fnViewBag.ChucVu();

        }
        private void UpdateViewBag(Guid? idDepartment = null, Guid? maChucVu = null, String? maDanToc = null, String? maTonGiao = null, String? maTrinhDoChuyenMon = null, String? maTrinhDoChinhTri = null,
            Guid? maTrinhDoNgoaiNgu = null, String? maTrinhDoTinHoc = null, String? Level = null, String? MaQuanHuyen = null, Guid? MaDiaBanHoiVien = null,String MaTrinhDoHocVan = null)
        {
            FnViewBag fnViewBag = new FnViewBag(_context);



            ViewBag.MaChucVu = fnViewBag.ChucVu(maChucVu);



            ViewBag.MaDanToc = fnViewBag.DanToc(maDanToc); ;

            ViewBag.MaTonGiao = fnViewBag.TonGiao(maTonGiao); 


            ViewBag.MaTrinhDoChuyenMon = fnViewBag.TrinhDoChuyenMon(maTrinhDoChuyenMon);
            ViewBag.MaTrinhDoHocVan = fnViewBag.TrinhDoHocVan(MaTrinhDoHocVan);

          
            ViewBag.MaTrinhDoChinhTri = fnViewBag.TrinhDoChinhTri(maTrinhDoChinhTri);

            var trinhDoNgoaiNgu = _context.TrinhDoNgoaiNgus.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTrinhDoNgoaiNgu = it.MaTrinhDoNgoaiNgu, TenTrinhDoNgoaiNgu = it.TenTrinhDoNgoaiNgu }).ToList();
            ViewBag.MaTrinhDoNgoaiNgu = new SelectList(trinhDoNgoaiNgu, "MaTrinhDoNgoaiNgu", "TenTrinhDoNgoaiNgu", maTrinhDoNgoaiNgu);

            var trinhDoTinHoc = _context.TrinhDoTinHocs.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTrinhDoTinHoc = it.MaTrinhDoTinHoc, TenTrinhDoTinHoc = it.TenTrinhDoTinHoc }).ToList();
            ViewBag.MaTrinhDoTinHoc = new SelectList(trinhDoTinHoc, "MaTrinhDoTinHoc", "TenTrinhDoTinHoc", maTrinhDoTinHoc);

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId(),value: MaDiaBanHoiVien);

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId(),value: MaQuanHuyen);

            ViewBag.Level = new SelectList(QHXP.GetData(), "Level", "Name", Level);
        }
        #endregion Helper
    }
}
