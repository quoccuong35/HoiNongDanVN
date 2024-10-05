using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys.MasterData;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.ViewModels.HoiVien;
using HoiNongDan.Resources;
using HoiNongDan.Web.Areas.NhanSu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Transactions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using System.Security.Principal;
using System.Runtime.Serialization;
using System;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HVDangKyController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 10;
        public HVDangKyController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config, IHttpContextAccessor httpContext) : base(context)
        {
            _hostEnvironment = hostEnvironment;
            _httpContext = httpContext;
        }
        #region Index
        //[HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBagSearch();
            return View();
        }
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult _Search(string? MaQuanHuyen, Guid? MaDiaBanHoatDong, DateTime? TuNgay, DateTime? DenNgay)
        {
            return ExecuteSearch(() =>
            {
                var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoatDong, TuNgay: TuNgay, DenNgay: DenNgay);
                return PartialView(model);
            });
        }
        #endregion Index

        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create()
        {
            CreateViewBag();
            HVDangKyVM model = new HVDangKyVM();
            model.MaDanToc = "KH";
            return View(model);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HVDangKyVM item)
        {
            if (!String.IsNullOrWhiteSpace(item.SoCCCD))
            {
                var checkExits = _context.CanBos.Where(it => it.SoCCCD == item.SoCCCD);
                if (checkExits.Count() > 0)
                {
                    ModelState.AddModelError("SoCCCD", "Số CCCD đã tồn tại");
                }
            }
            var account = _context.Accounts.SingleOrDefault(it => it.AccountId == AccountId());
            CheckNguoiDuyet(account!);
            return ExecuteContainer(() =>
            {
                var add = new CanBo();
                add.IDCanBo = Guid.NewGuid();
                add.MaDiaBanHoatDong = item.MaDiaBanHoiVien;
                add.NgayDangKy = item.NgayDangKy;
                add.HoVaTen = item.HoVaTen;
                add.NgaySinh = item.NgaySinh;
                add.GioiTinh = item.GioiTinh;
                add.SoCCCD = item.SoCCCD;
                add.NgayCapCCCD = item.NgayCapCCCD;
                add.SoDienThoai = item.SoDienThoai;
                add.MaDanToc = item.MaDanToc;
                add.MaTonGiao = item.MaTonGiao;
                add.MaTrinhDoHocVan = item.MaTrinhDoHocVan;
                add.MaTrinhDoChuyenMon = item.MaTrinhDoChuyenMon;
                add.ChuyenNganh = item.ChuyenNganh;
                add.MaTrinhDoChinhTri = item.MaTrinhDoChinhTri;
                add.MaNgheNghiep = item.MaNgheNghiep;
                add.ChoOHienNay = item.ChoOHienNay;
                add.HoKhauThuongTru = item.HoKhauThuongTru;
                add.DangVien = item.DangVien;
                add.HoiVienDanCu = item.HoiVienDanCu;
                add.HoiVienNganhNghe = item.HoiVienNganhNghe;

                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.HoiVienDuyet = false;
                add.AccountIdDangKy = AccountId();
                add.IsHoiVien = true;
                add.TuChoi = false;
                add.Actived = true;
                add.MaChucVu = Guid.Parse("D710D930-8342-474B-90A4-A1170A7A5691");
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                
                List<Guid> nguoiDuyets = new List<Guid>();
                string[] sTem = account!.AccountIDParent!.Split(";");
                foreach (var item in sTem)
                {
                    nguoiDuyets.Add(Guid.Parse(item));
                }
                List<HoiVienLichSuDuyet> hoiVienLichSuDuyets = new List<HoiVienLichSuDuyet>();
                nguoiDuyets.ForEach(item => {
                    hoiVienLichSuDuyets.Add(new HoiVienLichSuDuyet {
                        ID = Guid.NewGuid(),
                        IDHoiVien = add.IDCanBo,
                        AccountID = item,
                        CreateTime = DateTime.Now,
                        TrangThaiDuyet = false,
                    });
                });
                _context.HoiVienLichSuDuyets.AddRange(hoiVienLichSuDuyets);
                _context.CanBos.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.HVDangKy.ToLower())
                });
            });
        }
        #endregion Create

        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id) {
            var phamVis = Function.GetPhamVi(AccountId: AccountId()!.Value, _context: _context);
            var item = _context.CanBos.SingleOrDefault(it => it.IDCanBo == id && phamVis.Contains(it.MaDiaBanHoatDong!.Value));
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            HVDangKyVM edit = new HVDangKyVM();
            edit.ID = item.IDCanBo;
            edit.MaDiaBanHoiVien = item.MaDiaBanHoatDong;
            edit.NgayDangKy = item.NgayDangKy;
            edit.HoVaTen = item.HoVaTen;
            edit.NgaySinh = item.NgaySinh;
            edit.GioiTinh = item.GioiTinh;
            edit.SoCCCD = item.SoCCCD;
            edit.NgayCapCCCD = item.NgayCapCCCD;
            edit.SoDienThoai = item.SoDienThoai;
            edit.MaDanToc = item.MaDanToc!;
            edit.MaTonGiao = item.MaTonGiao;
            edit.MaTrinhDoHocVan = item.MaTrinhDoHocVan;
            edit.MaTrinhDoChuyenMon = item.MaTrinhDoChuyenMon;
            edit.ChuyenNganh = item.ChuyenNganh;
            edit.MaTrinhDoChinhTri = item.MaTrinhDoChinhTri;
            edit.MaNgheNghiep = item.MaNgheNghiep;
            edit.ChoOHienNay = item.ChoOHienNay!;
            edit.HoKhauThuongTru = item.HoKhauThuongTru;
            edit.DangVien = item.DangVien == null? false:item.DangVien.Value;
            edit.HoiVienDanCu = item.HoiVienDanCu == null ? false : item.HoiVienDanCu.Value; 
            edit.HoiVienNganhNghe = item.HoiVienNganhNghe == null ? false : item.HoiVienNganhNghe.Value; 
            CreateViewBag(maTrinhDoHocVan:item.MaTrinhDoHocVan,
                maTrinhDoChuyenMon:item.MaTrinhDoChuyenMon,
                maTrinhDoChinhTri:item.MaTrinhDoChinhTri,
                maDanToc:item.MaDanToc,
                maTonGiao:item.MaTonGiao,
                maNgheNghiep:item.MaNgheNghiep,
                maDiaBanHoiVien:item.MaDiaBanHoatDong);
            return View(edit);
        }

        [HttpPost]
        [HoiNongDanAuthorization]
        public IActionResult Edit(HVDangKyVM item)
        {
            if (!String.IsNullOrWhiteSpace(item.SoCCCD))
            {
                var checkExits = _context.CanBos.Where(it => it.SoCCCD == item.SoCCCD && it.IDCanBo != item.ID);
                if (checkExits.Count() > 0)
                {
                    ModelState.AddModelError("SoCCCD", "Số CCCD đã tồn tại");
                }
            }
            return ExecuteContainer(() => {
                var phamVis = Function.GetPhamVi(AccountId: AccountId()!.Value, _context: _context);
                var edit = _context.CanBos.SingleOrDefault(it => it.IDCanBo == item.ID && phamVis.Contains(it.MaDiaBanHoatDong!.Value));
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.HVDangKy.ToLower())
                    });
                }
                else
                {
                    edit.MaDiaBanHoatDong = item.MaDiaBanHoiVien;
                    edit.NgayDangKy = item.NgayDangKy;
                    edit.HoVaTen = item.HoVaTen;
                    edit.NgaySinh = item.NgaySinh;
                    edit.GioiTinh = item.GioiTinh;
                    edit.SoCCCD = item.SoCCCD;
                    edit.NgayCapCCCD = item.NgayCapCCCD;
                    edit.SoDienThoai = item.SoDienThoai;
                    edit.MaDanToc = item.MaDanToc!;
                    edit.MaTonGiao = item.MaTonGiao;
                    edit.MaTrinhDoHocVan = item.MaTrinhDoHocVan;
                    edit.MaTrinhDoChuyenMon = item.MaTrinhDoChuyenMon;
                    edit.ChuyenNganh = item.ChuyenNganh;
                    edit.MaTrinhDoChinhTri = item.MaTrinhDoChinhTri;
                    edit.MaNgheNghiep = item.MaNgheNghiep;
                    edit.ChoOHienNay = item.ChoOHienNay!;
                    edit.HoKhauThuongTru = item.HoKhauThuongTru;
                    edit.DangVien = item.DangVien;
                    edit.HoiVienDanCu = item.HoiVienDanCu;
                    edit.HoiVienNganhNghe = item.HoiVienNganhNghe;
                    edit.LastModifiedTime = DateTime.Now;
                    edit.LastModifiedAccountId = AccountId();
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.HVDangKy.ToLower())
                    });
                }
            });
        }
        #endregion Edit

        #region Import
        [HoiNongDanAuthorization]
        public IActionResult _Import()
        {
            CreateViewBagSearch();
            return PartialView();
        }

        [HoiNongDanAuthorization]
        public IActionResult Import(Guid? MaDiaBanHoiVien, String? MaQuanHuyen,DateTime? NgayDangKy)
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
            if (NgayDangKy == null)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = false,
                    Data = "Chưa chọn ngày đăng ký"
                });
            }
            var account = _context.Accounts.SingleOrDefault(it => it.AccountId == AccountId());
            if (String.IsNullOrWhiteSpace(account!.AccountIDParent)) {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = false,
                    Data = LanguageResource.ErrorDuyetHoiVien
                });
            }
            DataSet ds = GetDataSetFromExcel();
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                bool edit;
                List<string> errorList = new List<string>();
                List<Guid> nguoiDuyets = new List<Guid>();
                string[] sTem = account!.AccountIDParent!.Split(";");
                foreach (var item in sTem)
                {
                    nguoiDuyets.Add(Guid.Parse(item));
                }
                
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
                            if (dt.Rows.IndexOf(row) >= startIndex-1)
                            {

                                if (row[0] == null || String.IsNullOrWhiteSpace(row[0].ToString()))
                                    break;
                                error = new List<String>();
                                var data = CheckTemplate(row.ItemArray!, error, out edit);
                                data.MaDiaBanHoatDong = MaDiaBanHoiVien;
                                data.MaChucVu = Guid.Parse("D710D930-8342-474B-90A4-A1170A7A5691");
                                data.NgayDangKy = NgayDangKy;
                                if (error.Count > 0)
                                {
                                    errorList.AddRange(error);
                                }
                                else
                                {
                                    string result = ExecuteImportExcel(data, edit, nguoiDuyets);
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
                        });;
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
        private CanBo CheckTemplate(object[] row, List<String> error, out bool edit) {
            CanBo data = new CanBo();
            data.IDCanBo = Guid.NewGuid();
            data.HoiVienDuyet = false;
            data.AccountIdDangKy = AccountId();
            data.IsHoiVien = true;
            data.TuChoi = false;
            int index = 0; string value;
            edit = false;
            for (int i = 0; i < row.Length; i++) 
            {
                value = row[i] == null ? "": row[i].ToString()!.Trim();
                switch (i) {
                    case 0:
                        // stt
                        index = int.Parse(value);
                        break;
                    case 1:
                        //
                        if (!String.IsNullOrWhiteSpace(value)) {
                            try
                            {
                                data.IDCanBo = Guid.Parse(value);
                                edit = true;
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
                            else { 
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
                        //CMND/CCCD
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                var checkExits = _context.CanBos.Where(it => it.SoCCCD == value);
                                if (edit)
                                {

                                    if (checkExits.Where(it => it.IDCanBo != data.IDCanBo).Count() > 0)
                                    {
                                        error.Add($"Dòng {index} số CCCD đã tồn tại");
                                    }
                                }
                                else if (checkExits.Count()>0)
                                {
                                    error.Add($"Dòng {index} số CCCD đã tồn tại");
                                }
                                data.SoCCCD = value;
                            }
                            else
                            {
                                error.Add($"Dòng {index} Chưa nhập số CCCD");
                            }
                        }
                        catch
                        {
                            error.Add($"Dòng {index} có ngày sinh Nữ không hợp lệ");
                        }
                        break;
                    case 6:
                        //ngay cấp CMND/CCCD
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                data.NgayCapCCCD = Function.ConvertStringToDate(value).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                error.Add($"Dòng {index} Chưa nhập số ngày cấp CCCD");
                            }

                        }
                        catch
                        {
                            error.Add($"Dòng {index} có ngày Ngày cấp không hợp lệ");
                        }
                        break;
                    case 7:
                        //Hộ khẩu thường trú
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.HoKhauThuongTru = value;
                        }
                        else error.Add($"Dòng {index} hộ khẩu thường trú");
                        break;
                    case 8:
                        //Nơi ở hiện nay
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.ChoOHienNay = value;
                        }
                        else error.Add($"Dòng {index} chưa nhập nơi ở hiện nay");
                        break;
                    case 9:
                        //Số điện thoại
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.SoDienThoai = value;
                        }
                        break;
                    case 10:
                        //Nơi ở hiện nay
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.DangVien = true;
                        }
                        break;
                    case 11:
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
                    case 12:
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
                    case 13:
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
                    case 14:
                        //Trình độ chuyên môn
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.TrinhDoChuyenMons.FirstOrDefault(it => it.TenTrinhDoChuyenMon == value);
                            if (obj != null)
                            {
                                data.MaTrinhDoChuyenMon = obj.MaTrinhDoChuyenMon;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy Trình độ chuyên môn có tên {0} ở dòng số {1} !", value, index));
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
                    //case 16:
                    //    //Ngày tháng năm vào Hội (Theo QĐ công nhận hội viên)
                    //    if (!String.IsNullOrWhiteSpace(value))
                    //    {
                    //        data.NgayVaoHoi = value;
                    //    }
                    //    break;
                    case 16:
                        //Nghề nghiệp hiện nay
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            var obj = _context.NgheNghieps.FirstOrDefault(it => it.TenNgheNghiep == value);
                            if (obj != null)
                            {
                                data.MaNgheNghiep = obj.MaNgheNghiep;
                            }
                            else
                            {
                                error.Add(string.Format("Không tìm thấy Nghề nghiệp có tên {0} ở dòng số {1} !", value, index));
                            }
                        }
                        break;
                    case 18:
                        //Địa bàn dân cư
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.HoiVienDanCu = true;
                        }
                        break;
                    case 19:
                        //Nghề nghiệp hiện nay
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            data.HoiVienNganhNghe= true;
                        }
                        break;
                    //case 20:
                    //    // Số thẻ
                    //    if (!String.IsNullOrWhiteSpace(value))
                    //    {
                    //        data.MaCanBo = value;
                    //    }
                    //    break;
                    //case 21:
                    //    // Ngày cấp
                    //    if (!String.IsNullOrWhiteSpace(value))
                    //    {
                    //        data.NgayCapThe = Function.ConvertStringToDate(value);
                    //    }
                    //    break;
                    default:
                        break;
                }
            }
            return data;
        }
        private string ExecuteImportExcel(CanBo hvDangKy,bool edit= false, List<Guid>? nguoiDuyets = null)
        {
            if (!edit)
            {
                try
                {
                    hvDangKy.LastModifiedTime = DateTime.Now;
                    
                    List<HoiVienLichSuDuyet> hoiVienLichSuDuyets = new List<HoiVienLichSuDuyet>();
                    nguoiDuyets!.ForEach(item => {
                        hoiVienLichSuDuyets.Add(new HoiVienLichSuDuyet
                        {
                            ID = Guid.NewGuid(),
                            IDHoiVien = hvDangKy.IDCanBo,
                            AccountID = item,
                            CreateTime = DateTime.Now,
                            TrangThaiDuyet = false,
                        });
                    });
                    _context.HoiVienLichSuDuyets.AddRange(hoiVienLichSuDuyets);
                    _context.Entry(hvDangKy).State = EntityState.Added;
                }
                catch
                {

                }
            }
            else
            {
                var hvEdit = _context.CanBos.SingleOrDefault(p => p.IDCanBo == hvDangKy.IDCanBo && p.HoiVienDuyet !=true && p.TuChoi != true);
                //hvEdit!.MaCanBo = hvDangKy.MaCanBo;
                //hvEdit.NgayCapThe = hvDangKy.NgayCapThe;

                if (hvEdit != null)
                {
                    hvEdit = HoiVienDangKyMoiEdit(hvEdit, hvDangKy);
                    HistoryModelRepository history = new HistoryModelRepository(_context);
                    history.SaveUpdateHistory(hvDangKy.IDCanBo.ToString(), AccountId()!.Value, hvEdit);
                }
                else
                {
                    return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                            LanguageResource.HoiVien, hvEdit!.IDCanBo,
                                            string.Format(LanguageResource.Export_ExcelHeader,
                                            LanguageResource.HoiVien));
                }
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.InnerException!.Message + " " + hvDangKy.HoVaTen;
            }
            return LanguageResource.ImportSuccess;
        }

        private CanBo HoiVienDangKyMoiEdit(CanBo old, CanBo news)
        {
            old.HoVaTen = news.HoVaTen;
            old.GioiTinh = news.GioiTinh;
            old.SoCCCD = news.SoCCCD;
            old.NgayCapCCCD = news.NgayCapCCCD;
            old.NoiSinh = news.NoiSinh;
            old.ChoOHienNay = news.ChoOHienNay;
            old.SoDienThoai = news.SoDienThoai;
            old.DangVien = news.DangVien;
            old.MaDanToc = news.MaDanToc;
            old.MaTonGiao = news.MaTonGiao;
            old.MaTrinhDoHocVan = news.MaTrinhDoHocVan;
            old.MaTrinhDoChuyenMon = news.MaTrinhDoChuyenMon;
            old.MaTrinhDoChinhTri = news.MaTrinhDoChuyenMon;
            old.MaNgheNghiep = news.MaNgheNghiep;
            old.HoiVienDanCu = news.HoiVienDanCu;
            old.HoiVienNganhNghe = news.HoiVienNganhNghe;
          
            return old;
        }
        #endregion Import
        #region Export 
        [HoiNongDanAuthorization]
        public IActionResult ExportCreate()
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\HVDangKyMoi.xlsx");
            List<HVDangKyImportVM> data = new List<HVDangKyImportVM>();
            return Export(data, url, startIndex);
        }
        private FileContentResult Export(List<HVDangKyImportVM> data,string url,int startIndex) {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "ID", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Nam", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Nu", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoCCCD", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayCapSoCCCD", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoKhauThuongTru", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NoiOHiennay", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoDienThoai", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DangVien", isBoolean = true, isComment = true, isAllowedToEdit = true, strComment = "để X là đảng viên" });

            var danToc = _context.DanTocs.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaDanToc, Name = x.TenDanToc }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaDanToc", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = danToc, TypeId = ConstExcelController.StringId });

            var tonGiao = _context.TonGiaos.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTonGiao, Name = x.TenTonGiao }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTonGiao", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = tonGiao, TypeId = ConstExcelController.StringId });

            var hocVan = _context.TrinhDoHocVans.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoHocVan, Name = x.TenTrinhDoHocVan }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoHocVan", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = hocVan, TypeId = ConstExcelController.StringId });

            var chuyenNganh = _context.TrinhDoChuyenMons.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChuyenMon, Name = x.TenTrinhDoChuyenMon }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChuyenMon", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chuyenNganh, TypeId = ConstExcelController.StringId });

            var chinhTri = _context.TrinhDoChinhTris.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaTrinhDoChinhTri, Name = x.TenTrinhDoChinhTri }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaTrinhDoChinhTri", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = chinhTri, TypeId = ConstExcelController.StringId });

            //columns.Add(new ExcelTemplate() { ColumnName = "NgayThangVaoHoi", isAllowedToEdit = true, isText = true, strComment = "Nhập ngày tháng năm theo số quyết định" });

            var ngheNghiep = _context.NgheNghieps.ToList().Select(x => new DropdownIdTypeStringModel { Id = x.MaNgheNghiep, Name = x.TenNgheNghiep }).ToList();
            columns.Add(new ExcelTemplate() { ColumnName = "MaNgheNghiep", isAllowedToEdit = true, isDropdownlist = true, DropdownIdTypeStringData = ngheNghiep, TypeId = ConstExcelController.StringId });

            columns.Add(new ExcelTemplate() { ColumnName = "DiaBanDanCu", isAllowedToEdit = true, isText = true, strComment = "Nhập X là hội viên dân cư" });
            columns.Add(new ExcelTemplate() { ColumnName = "NganhNghe", isAllowedToEdit = true, isText = true, strComment = "Nhập X là hội viên ngành nghề" });

            //columns.Add(new ExcelTemplate() { ColumnName = "SoThe", isAllowedToEdit = true, isText = true});
            //columns.Add(new ExcelTemplate() { ColumnName = "NgayCapThe", isAllowedToEdit = true, isText = true });

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
            byte[] filecontent = ClassExportExcel.ExportExcel( url,data,  columns, heading, true, startIndex);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        public IActionResult ExportEdit(string? MaQuanHuyen, Guid? MaDiaBanHoatDong, DateTime? TuNgay, DateTime? DenNgay)
        {
            var data = _context.CanBos
                .Include(it => it.DanToc).Include(it => it.NgheNghiep)
                .Include(it => it.TonGiao).Include(it => it.TrinhDoHocVan)
                .Include(it => it.TrinhDoChuyenMon).Include(it => it.TrinhDoChinhTri)
                .Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen)
                .Join(_context.PhamVis.Where(it => it.AccountId == AccountId()),
                    hv => hv.MaDiaBanHoatDong,
                    pv => pv.MaDiabanHoatDong,
                    (hv, pv) => new { hv }
                    ).Where(
                        it => it.hv.IsHoiVien == true
                        && it.hv.HoiVienDuyet == false
                        && it.hv.TuChoi == false
                        && it.hv.isRoiHoi == false
                    ).AsQueryable();

            if (TuNgay != null)
            {
                data = data.Where(it => it.hv.NgayDangKy >= TuNgay.Value.Date);
            }
            if (DenNgay != null)
            {
                data = data.Where(it => it.hv.NgayDangKy <= DenNgay.Value.Date);
            }
            if (!String.IsNullOrWhiteSpace(MaQuanHuyen))
            {
                data = data.Where(it => it.hv.DiaBanHoatDong!.MaQuanHuyen == MaQuanHuyen);
            }
            if (MaDiaBanHoatDong != null)
            {
                data = data.Where(it => it.hv.MaDiaBanHoatDong == MaDiaBanHoatDong);
            }
            var model = data.Select(it => new HVDangKyImportVM
            {
                ID = it.hv.IDCanBo,
                HoVaTen = it.hv.HoVaTen,
                Nam = (int)it.hv.GioiTinh == 1 ? it.hv.NgaySinh : "",
                Nu = (int)it.hv.GioiTinh == 0 ? it.hv.NgaySinh : "",
                SoCCCD = it.hv.SoCCCD,
                NgayCapSoCCCD = it.hv.NgayCapCCCD,
                HoKhauThuongTru = it.hv.HoKhauThuongTru,
                NoiOHiennay = it.hv.ChoOHienNay,
                SoDienThoai = it.hv.SoDienThoai,
                DangVien = "",
                MaDanToc = it.hv.DanToc!.TenDanToc,
                MaTonGiao = it.hv.TonGiao!.TenTonGiao,
                MaTrinhDoHocVan = it.hv.TrinhDoHocVan.TenTrinhDoHocVan,
                MaTrinhDoChuyenMon = it.hv.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                MaTrinhDoChinhTri = it.hv.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                //NgayThangVaoHoi = it.hv.NgayVaoHoi,
                MaNgheNghiep = it.hv.NgheNghiep!.TenNgheNghiep,
                DiaBanDanCu = it.hv.HoiVienDanCu == true ? "X" : "",
                NganhNghe = it.hv.HoiVienNganhNghe == true ? "X" : "",
                //SoThe = it.hv.MaCanBo,
                //NgayCapThe = it.hv.NgayCapThe != null? it.hv.NgayCapThe.Value.ToString("dd/MM/yyyy"):""
            }).ToList().Select((it, index) => new HVDangKyImportVM
            {
                STT = index + 1,
                ID = it.ID,
                HoVaTen = it.HoVaTen,
                Nam = it.Nam,
                Nu = it.Nu,
                SoCCCD = it.SoCCCD,
                NgayCapSoCCCD = it.NgayCapSoCCCD,
                HoKhauThuongTru = it.HoKhauThuongTru,
                NoiOHiennay = it.NoiOHiennay,
                SoDienThoai = it.SoDienThoai,
                DangVien = "",
                MaDanToc = it.MaDanToc,
                MaTonGiao = it.MaTonGiao,
                MaTrinhDoHocVan = it.MaTrinhDoHocVan,
                MaTrinhDoChuyenMon = it.MaTrinhDoChuyenMon,
                MaTrinhDoChinhTri = it.MaTrinhDoChinhTri,
                //NgayThangVaoHoi = it.NgayThangVaoHoi,
                MaNgheNghiep = it.MaNgheNghiep,
                DiaBanDanCu = it.DiaBanDanCu,
                NganhNghe = it.NganhNghe,
                //SoThe = it.SoThe,
                //NgayCapThe = it.NgayCapThe

            }).ToList();

            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\HVDangKyMoi.xlsx");

            return Export(model, url,startIndex); ;
        }
        #endregion Export
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.CanBos.FirstOrDefault(p => p.IDCanBo == id && p.IsHoiVien == true && p.HoiVienDuyet != true && p.AccountIdDangKy == AccountId());


                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.HVDangKy.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.HVDangKy.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Helper

        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }

        private List<HVDangKyExcelVM> LoadData(string? MaQuanHuyen, Guid? MaDiaBanHoatDong, DateTime? TuNgay, DateTime? DenNgay)
        {
            TuNgay = TuNgay == null ? FirstDate() : TuNgay;
            DenNgay = DenNgay == null ? LastDate() : DenNgay;
            var data = _context.CanBos
                .Include(it => it.DanToc).Include(it => it.NgheNghiep)
                .Include(it => it.TonGiao).Include(it => it.TrinhDoHocVan)
                .Include(it => it.TrinhDoChuyenMon).Include(it => it.TrinhDoChinhTri)
                .Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen)
                .Join(_context.PhamVis.Where(it => it.AccountId == AccountId()),
                    hv => hv.MaDiaBanHoatDong,
                    pv => pv.MaDiabanHoatDong,
                    (hv, pv) => new { hv }
                    ).Where(
                        it => it.hv.IsHoiVien == true
                        && it.hv.NgayDangKy >= TuNgay 
                        && it.hv.NgayDangKy <= DenNgay
                        && it.hv.AccountIdDangKy == AccountId()
                    ).AsQueryable();
           
            if (!String.IsNullOrWhiteSpace(MaQuanHuyen))
            {
                data = data.Where(it => it.hv.DiaBanHoatDong!.MaQuanHuyen == MaQuanHuyen);
            }
            if (MaDiaBanHoatDong != null)
            {
                data = data.Where(it => it.hv.MaDiaBanHoatDong == MaDiaBanHoatDong);
            }
            var model = data.Select(it => new HVDangKyExcelVM
            {
                ID = it.hv.IDCanBo,
                HoVaTen = it.hv.HoVaTen,
                Nam = (int)it.hv.GioiTinh == 1 ? it.hv.NgaySinh : "",
                Nu = (int)it.hv.GioiTinh == 0 ? it.hv.NgaySinh : "",
                SoCCCD = it.hv.SoCCCD,
                NgayCapSoCCCD = it.hv.NgayCapCCCD,
                HoKhauThuongTru = it.hv.HoKhauThuongTru,
                NoiOHiennay = it.hv.ChoOHienNay,
                SoDienThoai = it.hv.SoDienThoai,
                DangVien = it.hv.DangVien == true ? "X" : "",
                DanToc = it.hv.DanToc!.TenDanToc,
                TonGiao = it.hv.TonGiao!.TenTonGiao,
                TrinhDoHocVan = it.hv.TrinhDoHocVan.TenTrinhDoHocVan,
                TrinhDoChuyenMon = it.hv.TrinhDoChuyenMon!.TenTrinhDoChuyenMon + " " + it.hv.ChuyenNganh,
                ChinhTri = it.hv.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                NgayThangVaoHoi = it.hv.NgayVaoHoi,
                NgheNghiep = it.hv.NgheNghiep!.TenNgheNghiep,
                DiaBanDanCu = it.hv.HoiVienDanCu == true ? "X" : "",
                NganhNghe = it.hv.HoiVienNganhNghe == true ? "X" : "",
                SoThe = it.hv.MaCanBo,
                NgayCapThe = "",
                TrangThai = it.hv.TuChoi != true && it.hv.HoiVienDuyet != true ? "1" : it.hv.HoiVienDuyet == true ? "2" : "3",
                LyDoTuChoi = it.hv.LyDoTuChoi
            }).ToList().Select((it, index) => new HVDangKyExcelVM
            {
                ID = it.ID,
                STT = index + 1,
                HoVaTen = it.HoVaTen,
                Nam = it.Nam,
                Nu = it.Nu,
                SoCCCD = it.SoCCCD,
                NgayCapSoCCCD = it.NgayCapSoCCCD,
                HoKhauThuongTru = it.HoKhauThuongTru,
                NoiOHiennay = it.NoiOHiennay,
                SoDienThoai = it.SoDienThoai,
                DangVien = it.DangVien,
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
                NgayCapThe = "",
                TrangThai = it.TrangThai,
                LyDoTuChoi = it.LyDoTuChoi
            }).ToList();
            return model;

        }

        private void CreateViewBag(string? maTrinhDoHocVan = null, string? maTrinhDoChuyenMon = null,string? maTrinhDoChinhTri = null,string? maDanToc = null, String? maTonGiao = null,string? maNgheNghiep = null,Guid? maDiaBanHoiVien = null)
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
           
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());
            var trinhDoHocVan = _context.TrinhDoHocVans.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTrinhDoHocVan = it.MaTrinhDoHocVan, TenTrinhDoHocVan = it.TenTrinhDoHocVan }).ToList();
            ViewBag.MaTrinhDoHocVan = new SelectList(trinhDoHocVan, "MaTrinhDoHocVan", "TenTrinhDoHocVan", maTrinhDoHocVan);

            var trinhDoChuyenMon = _context.TrinhDoChuyenMons.Select(it => new { MaTrinhDoChuyenMon = it.MaTrinhDoChuyenMon, TenTrinhDoChuyenMon = it.TenTrinhDoChuyenMon }).ToList();
            ViewBag.MaTrinhDoChuyenMon = new SelectList(trinhDoChuyenMon, "MaTrinhDoChuyenMon", "TenTrinhDoChuyenMon", maTrinhDoChuyenMon);

            var trinhDoChinhTri = _context.TrinhDoChinhTris.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTrinhDoChinhTri = it.MaTrinhDoChinhTri, TenTrinhDoChinhTri = it.TenTrinhDoChinhTri }).ToList();
            ViewBag.MaTrinhDoChinhTri = new SelectList(trinhDoChinhTri, "MaTrinhDoChinhTri", "TenTrinhDoChinhTri", maTrinhDoChinhTri);

            var danToc = _context.DanTocs.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaDanToc = it.MaDanToc, TenDanToc = it.TenDanToc }).ToList();
            ViewBag.MaDanToc = new SelectList(danToc, "MaDanToc", "TenDanToc", maDanToc);

            var tonGiao = _context.TonGiaos.Where(it => it.Actived == true).OrderBy(it => it.OrderIndex).Select(it => new { MaTonGiao = it.MaTonGiao, TenTonGiao = it.TenTonGiao }).ToList();
            ViewBag.MaTonGiao = new SelectList(tonGiao, "MaTonGiao", "TenTonGiao", maTonGiao);

            var ngheNghieps = _context.NgheNghieps.Select(it => new { MaNgheNghiep = it.MaNgheNghiep, TenNgheNghiep = it.TenNgheNghiep }).ToList();
            ViewBag.MaNgheNghiep = new SelectList(ngheNghieps, "MaNgheNghiep", "TenNgheNghiep", maNgheNghiep);
        }

        private void CheckNguoiDuyet(Account account) {
            if (String.IsNullOrWhiteSpace(account!.AccountIDParent)) {
                ModelState.AddModelError("", LanguageResource.ErrorDuyetHoiVien);
            }
        }
        #endregion Helper

    }
}
