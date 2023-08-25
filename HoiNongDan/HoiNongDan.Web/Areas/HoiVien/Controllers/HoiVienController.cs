
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Hosting;
using NuGet.Packaging.Signing;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.Entitys.MasterData;
using HoiNongDan.Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Mapping;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text;
using System.Transactions;
using NuGet.Packaging;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HoiVienController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 6;
        public HoiVienController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context)
        {
            _hostEnvironment = hostEnvironment;
            DateFomat = config.GetSection("SiteSettings:DateFormat").Value.ToString().Split(',');
        }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {

            CreateViewBagSearch();
            return View();
        }
        public IActionResult _Search(HoiVienSearchVM search)
        {
            return ExecuteSearch(() => {
                var model = _context.CanBos.Where(it=>it.IsHoiVien == true ).AsQueryable();
                if (!String.IsNullOrEmpty(search.MaCanBo))
                {
                    model = model.Where(it => it.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen))
                {
                    model = model.Where(it => it.HoVaTen.Contains(search.HoVaTen));
                }
                
                if (search.MaDiaBanHoatDong != null)
                {
                    model = model.Where(it => it.MaDiaBanHoatDong == search.MaDiaBanHoatDong);
                }
                if (search.MaChucVu != null)
                {
                    model = model.Where(it => it.MaChucVu == search.MaChucVu);
                }
                if (search.Actived != null)
                {
                    model = model.Where(it => it.Actived == search.Actived);
                }
                if (search.DangChoDuyet == null || search.DangChoDuyet == true)
                {
                    model = model.Where(it => it.HoiVienDuyet == true);
                }
                else
                {
                    model = model.Where(it => it.HoiVienDuyet !=true && it.CreatedAccountId == AccountId());
                   
                }
                var data = model.Include(it => it.TinhTrang)
                    .Include(it => it.ChucVu)
                    .Include(it=>it.NgheNghiep)
                    .Include(it=>it.GiaDinhThuocDien)
                    .Include(it => it.DiaBanHoatDong)
                    .Include(it => it.DanToc)
                    .Include(it => it.TonGiao)
                    .Include(it => it.TrinhDoHocVan)
                    .Include(it => it.CoSo).Select(it => new HoiVienDetailVM
                    {
                        IDCanBo = it.IDCanBo,
                        MaCanBo = it.MaCanBo,
                        HoVaTen = it.HoVaTen,
                        TenDiaBanHoatDong = it.DiaBanHoatDong!.TenDiaBanHoatDong,
                        DanToc =it.DanToc!.TenDanToc,
                        TonGiao = it.TonGiao!.TenTonGiao,
                        TrinhDoHocvan = it.TrinhDoHocVan.TenTrinhDoHocVan,
                        TenChucVu = it.ChucVu.TenChucVu,
                        HinhAnh = it.HinhAnh!,
                        VaiTro = it.VaiTro =="01"? "Chủ hộ": "Quan hệ chủ hộ",
                        NgheNghiepHienNay = it.NgheNghiep!.TenNgheNghiep,
                        GiaDinhThuocDien = it.GiaDinhThuocDien!.TenGiaDinhThuocDien,
                        
                    }).ToList();
                return PartialView(data);
            });
        }
        #endregion Index
        #region Create
        [HoiNongDanAuthorization]
        [HttpGet]
        public IActionResult Create()
        {
            CreateViewBag();
            HoiVienVM obj = new HoiVienVM();
            obj.HinhAnh = @"\images\login.png";
            return View(obj);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        public JsonResult Create(HoiVienMTVM insert, IFormFile? avtFileInbox)
        {
            CheckError(insert);
            return ExecuteContainer(() => {
                CanBo add = new CanBo();
                insert.GetHoiVien(add);
                add.IDCanBo = Guid.NewGuid();
                add.Actived = true;
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                add.HoiVienDuyet = false;
                if (avtFileInbox != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = add.MaCanBo;
                    var uploads = Path.Combine(wwwRootPath, @"images\canbo");

                    bool folderExists = System.IO.Directory.Exists(uploads);
                    if (!folderExists)
                        System.IO.Directory.CreateDirectory(uploads);

                    var extension = Path.GetExtension(avtFileInbox.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        avtFileInbox.CopyTo(fileStream);
                    }
                    add.HinhAnh = @"\images\canbo\" + fileName + extension;
                }
                _context.Attach(add).State = EntityState.Modified;
                _context.CanBos.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.HoiVien.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id)
        {
            var item = _context.CanBos.SingleOrDefault(it => it.IDCanBo == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            HoiVienVM edit = HoiVienMTVM.SetHoiVien(item);
            CreateViewBag(item.IdCoSo, item.IdDepartment, item.MaChucVu,item.MaTrinhDoHocVan,
                item.MaTrinhDoChinhTri,item.MaDanToc,item.MaTonGiao,item.MaTrinhDoChuyenMon,item.MaDiaBanHoatDong,item.MaHocVi);
            return View(edit);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        public JsonResult Edit(HoiVienMTVM obj, IFormFile? avtFileInbox)
        {

            CheckError(obj);
            return ExecuteContainer(() => {
                var edit = _context.CanBos.SingleOrDefault(it => it.IDCanBo == obj.IDCanBo);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.HoiVien.ToLower())
                    });
                }
                obj.GetHoiVien(edit);
                edit.Actived = obj.Actived!.Value;
                edit.NgayNgungHoatDong = obj.NgayNgungHoatDong;
                edit.LyDoNgungHoatDong = obj.LyDoNgungHoatDong;
                edit.LastModifiedTime = DateTime.Now;
                edit.LastModifiedAccountId = AccountId();
                if (avtFileInbox != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = edit.MaCanBo;
                    var uploads = Path.Combine(wwwRootPath, @"images\canbo");

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
                    edit.HinhAnh = @"\images\canbo\" + fileName + extension;
                }
                HistoryModelRepository history = new HistoryModelRepository(_context);
                history.SaveUpdateHistory(edit.IDCanBo.ToString(), AccountId()!.Value, edit);
                _context.Entry(edit).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.HoiVien.ToLower())
                });
            });
        }
        #endregion Edit
        #region View
        [HttpGet]
        public IActionResult XemThongTin()
        {
            CreateViewBagSearch();
            ViewBag.HTTP = "HttpGet";
            return View(new HoiVienDetailVM()) ;
        }
        [HttpPost]
        public IActionResult XemThongTin(String MaHoiVien, Guid MaDiaBanHoatDong)
        {
            HoiVienDetailVM hoivien = new HoiVienDetailVM();
            try
            {
                     hoivien = _context.CanBos.Where(it => it.MaCanBo == MaHoiVien && it.HoiVienDuyet == true && it.Actived == true && it.IsHoiVien ==true && it.MaDiaBanHoatDong == MaDiaBanHoatDong).Include(it => it.TinhTrang)
                        .Include(it => it.DiaBanHoatDong)
                        .Include(it => it.DanToc)
                        .Include(it => it.TonGiao)
                        .Include(it => it.TrinhDoHocVan)
                        .Include(it => it.TrinhDoChuyenMon)
                        .Include(it => it.TrinhDoChinhTri)
                        .Include(it => it.CoSo).Select(it => new HoiVienDetailVM
                        {
                            MaCanBo = it.MaCanBo,
                            IDCanBo = it.IDCanBo,
                            HoVaTen = it.HoVaTen,
                            NgaySinh = it.NgaySinh,
                            GioiTinh = (GioiTinh)it.GioiTinh,
                            SoCCCD = it.SoCCCD!,
                            HoKhauThuongTru = it.HoKhauThuongTru,
                            ChoOHienNay = it.ChoOHienNay!,
                            SoDienThoai = it.SoDienThoai,
                            NgayvaoDangDuBi = it.NgayvaoDangDuBi,
                            NgayVaoDangChinhThuc = it.NgayVaoDangChinhThuc,
                            DanToc = it.DanToc!.TenDanToc,
                            TonGiao = it.TonGiao!.TenTonGiao,
                            TrinhDoHocvan = it.TrinhDoHocVan.TenTrinhDoHocVan,
                            TrinhDoChuyenMon = it.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                            TrinhDoChinhChi = it.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                            TenDiaBanHoatDong = it.DiaBanHoatDong.TenDiaBanHoatDong,
                            NgayVaoHoi = it.NgayVaoHoi,
                            NgayThamGiaCapUyDang = it.NgayThamGiaCapUyDang,
                            NgayThamGiaHDND = it.NgayThamGiaHDND,
                            VaiTro = it.VaiTro,
                            VaiTroKhac = it.VaiTroKhac,
                            GiaDinhThuocDien = it.MaGiaDinhThuocDien,
                            GiaDinhThuocDienKhac = it.GiaDinhThuocDienKhac,
                            NgheNghiepHienNay = it.MaNgheNghiep,
                            Loai_DV_SX_ChN = it.Loai_DV_SX_ChN,
                            SoLuong = it.SoLuong,
                            DienTich = it.DienTich,
                            ThamGia_SH_DoanThe_HoiDoanKhac = it.ThamGia_SH_DoanThe_HoiDoanKhac,
                            ThamGia_CLB_DN_MH_HTX_THT = it.ThamGia_CLB_DN_MH_HTX_THT,
                            ThamGia_THNN_CHNN = it.ThamGia_THNN_CHNN

                        }).First();
                var lisCauHoi = _context.HoiVienHoiDaps.Include(it=>it.HoiVien).Where(it => it.IDHoivien == hoivien.IDCanBo && it.TraLoi != true).OrderBy(it=>it.Ngay).Select(it=>new HoiVienHoiDapDetail { 
                    ID = it.ID,
                    HoVaTen = it.HoiVien.HoVaTen,
                    NoiDung= it.NoiDung,
                    TraLoi= it.TraLoi,
                    Ngay = it.Ngay,
                    IdParent = it.IdParent
                }).ToList();
                if (lisCauHoi.Count() >0)
                {
                    hoivien.HoiDaps.AddRange(lisCauHoi);//
                                                        // add cau tra loi
                    var listraloi = _context.HoiVienHoiDaps.Include(it => it.Account).Where(it => it.IdParent != null && lisCauHoi.Select(it => it.ID).ToList().Contains(it.IdParent.Value)).Select(it => new HoiVienHoiDapDetail
                    {
                        ID = it.ID,
                        HoVaTen = it.Account.FullName,
                        NoiDung = it.NoiDung,
                        TraLoi = it.TraLoi,
                        Ngay = it.Ngay,
                        IdParent = it.IdParent
                    }).ToList();
                    hoivien.HoiDaps.AddRange(listraloi);
                }
            }
            catch
            {

            }
            ViewBag.HTTP = "HttpPost";
            CreateViewBagSearch();
            return View(hoivien);
        }
        #endregion View
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        public JsonResult Delete(Guid id)
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
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.HoiVien.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.HoiVien.ToLower())
                    });
                }
            });
        }
        #endregion Delete
        #region Import Excel 
        public IActionResult Import()
        {
            DataSet ds = GetDataSetFromExcel();
            List<string> errorList = new List<string>();
            return ExcuteImportExcel(() => {
                if (ds.Tables.Count > 0)
                {
                    const TransactionScopeOption opt = new TransactionScopeOption();

                    TimeSpan span = new TimeSpan(0, 0, 30, 30);
                    using (TransactionScope ts = new TransactionScope(opt, span))
                    {
                        
                        foreach (DataTable dt in ds.Tables)
                        {
                            string contCode = dt.Columns[0].ColumnName.ToString();
                            if (contCode == controllerCode)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    //string aa = dr.ItemArray[0].ToString();
                                    if (dt.Rows.IndexOf(dr) >= startIndex)
                                    {
                                        if (!string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                        {
                                            var data = CheckTemplate(dr.ItemArray);
                                            if (!string.IsNullOrEmpty(data.Error))
                                            {
                                                errorList.Add(data.Error);
                                            }
                                            else
                                            {
                                                // Tiến hành cập nhật
                                                string result = ExecuteImportExcelMenu(data);
                                                if (result != LanguageResource.ImportSuccess)
                                                {
                                                    errorList.Add(result);
                                                }
                                            }
                                        }
                                        else
                                            break;
                                        //Check correct template

                                    }
                                }
                            }
                            //else
                            //{
                            //    string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.HoiVien);
                            //    errorList.Add(error);
                            //}
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
                            Data = LanguageResource.ImportSuccess
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
                return null;
            }
        }
        #endregion Import Excel
        #region Export Data
        public IActionResult ExportCreate()
        {
            List<HoiVienExcelVM> data = new List<HoiVienExcelVM>();
            return Export(data);
        }
        public IActionResult ExportEdit(CanBoSearchVM search)
        {
            var model = _context.CanBos.AsQueryable();
            if (!String.IsNullOrEmpty(search.MaCanBo))
            {
                model = model.Where(it => it.MaCanBo == search.MaCanBo);
            }
            if (!String.IsNullOrEmpty(search.HoVaTen))
            {
                model = model.Where(it => it.HoVaTen.Contains(search.HoVaTen));
            }
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

            var data = model.Include(it => it.TinhTrang)
                
                .Include(it => it.TrinhDoHocVan)
                .Include(it => it.TrinhDoChuyenMon)
                .Include(it => it.TrinhDoChinhTri)
                .Include(it => it.DanToc)
                .Include(it => it.TonGiao)
                .Select(item => new HoiVienExcelVM
                {
                    IDCanBo = item.IDCanBo,
                    MaCanBo = item.MaCanBo,
                    HoVaTen = item.HoVaTen,
                    NgaySinh = item.NgaySinh,
                    GioiTinh = item.GioiTinh == GioiTinh.Nam ? true : false,
                    SoCCCD = item.SoCCCD,
                    NgayCapCCCD = item.NgayCapCCCD,
                    HoKhauThuongTru = item.HoKhauThuongTru,
                    ChoOHienNay = item.ChoOHienNay!,
                    SoDienThoai = item.SoDienThoai,
                    NgayvaoDangDuBi = item.NgayvaoDangDuBi,
                    NgayVaoDangChinhThuc = item.NgayVaoDangChinhThuc,
                    MaDanToc = item.DanToc.TenDanToc,
                    MaTonGiao = item.TonGiao.TenTonGiao,
                    MaTrinhDoHocVan = item.TrinhDoHocVan.TenTrinhDoHocVan,
                    MaTrinhDoChuyenMon = item.TrinhDoChuyenMon!.TenTrinhDoChuyenMon,
                    MaTrinhDoChinhTri = item.TrinhDoChinhTri!.TenTrinhDoChinhTri,
                    NgayVaoHoi = item.NgayVaoHoi,
                    NgayThamGiaCapUyDang = item.NgayThamGiaCapUyDang,
                    NgayThamGiaHDND = item.NgayThamGiaHDND,
                    VaiTro = item.VaiTro =="1"?"X":"",
                    VaiTroKhac = item.VaiTroKhac,
                    HoNgheo = item.MaGiaDinhThuocDien =="01"?"X":"",
                    CanNgheo = item.MaGiaDinhThuocDien =="02"?"X":"",
                    GiaDinhChinhSach = item.MaGiaDinhThuocDien =="03"?"X":"",
                    GiaDinhThanhPhanKhac = item.GiaDinhThuocDienKhac,
                    NongDan = item.MaGiaDinhThuocDien == "01" ? "X" : "",
                    CongNhan = item.MaGiaDinhThuocDien == "02" ? "X" : "",
                    CV_VC = item.MaGiaDinhThuocDien == "03" ? "X" : "",
                    HuuTri = item.MaGiaDinhThuocDien == "04" ? "X" : "",
                    DoanhNghiep = item.MaGiaDinhThuocDien == "05" ? "X" : "",
                    LaoDongTuDo = item.MaGiaDinhThuocDien == "06" ? "X" : "",
                    HS_SV = item.MaGiaDinhThuocDien == "07" ? "X" : "",
                    SX_ChN = item.Loai_DV_SX_ChN,
                    SoLuong = item.SoLuong,
                    DienTich = item.DienTich,
                    SinhHoatDoanTheChinhTri = item.ThamGia_SH_DoanThe_HoiDoanKhac,
                    ThamGia_CLB_DN_HTX = item.ThamGia_CLB_DN_MH_HTX_THT,
                    ThamGia_THNN_CHNN = item.ThamGia_THNN_CHNN,
                    HV_NongCot = item.LoaiHoiVien == "01" ? "X" : "",
                    HV_DanhDu = item.LoaiHoiVien == "02" ? "X" : "",
                }).ToList();
            return Export(data);
        }
        public FileContentResult Export(List<HoiVienExcelVM> menu)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "IDCanBo", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "MaCanBo", isAllowedToEdit = false, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgaySinh", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GioiTinh", isBoolean = true, isComment = true, strComment = "Nam để chữ X" });
            columns.Add(new ExcelTemplate() { ColumnName = "SoCCCD", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayCapCCCD", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoKhauThuongTru", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoDienThoai", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayvaoDangDuBi", isAllowedToEdit = true, isDateTime = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoDangChinhThuc", isAllowedToEdit = true, isDateTime = true });
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
            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoHoi", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayThamGiaCapUyDang", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayThamGiaHDND", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "VaiTro", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "VaiTroKhac", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HoNgheo", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "CanNgheo", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GiaDinhChinhSach", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "GiaDinhThanhPhanKhac", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "NongDan", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "CongNhan", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "CV_VC", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HuuTri", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DoanhNghiep", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "LaoDongTuDo", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HS_SV", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SX_ChN", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SoLuong", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DienTich", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "SinhHoatDoanTheChinhTri", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ThamGia_CLB_DN_HTX", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ThamGia_THNN_CHNN", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HV_NongCot", isAllowedToEdit = true, isText = true });
            columns.Add(new ExcelTemplate() { ColumnName = "HV_DanhDu", isAllowedToEdit = true, isText = true });

            
            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.HoiVien);
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
            byte[] filecontent = ClassExportExcel.ExportExcel(menu, columns, heading, true);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export Data
        #region Helper
        private void CheckError(HoiVienMTVM insert)
        {
            var checkExistMaCB = _context.CanBos.Where(it => it.MaCanBo == insert.MaCanBo).ToList();
            if (checkExistMaCB.Count > 0 && insert.IDCanBo == null)
            {
                ModelState.AddModelError("MaCanBo", "Mã cán bộ tồn tại không thể thêm");
            }
            
            if (insert.VaiTro == "2" && (String.IsNullOrEmpty(insert.VaiTroKhac) || String.IsNullOrWhiteSpace(insert.VaiTroKhac)))
            {
                ModelState.AddModelError("VaiTroKhac", "Chưa nhập quan hệ với chủ hộ");
            }

            if (insert.MaGiaDinhThuocDien == "04" && (String.IsNullOrEmpty(insert.GiaDinhThuocDienKhac) || String.IsNullOrWhiteSpace(insert.GiaDinhThuocDienKhac)))
            {
                ModelState.AddModelError("GiaDinhThuocDienKhac", "Chưa nhập gia đình thuộc diện thành phần khác");
            }

            if (insert.Actived == false)
            {
                if ((String.IsNullOrEmpty(insert.LyDoNgungHoatDong) || String.IsNullOrWhiteSpace(insert.LyDoNgungHoatDong)))
                {
                    ModelState.AddModelError("LyDoNgungHoatDong", "Lý do ngưng hoạt động chưa nhập");
                }
                if (insert.NgayNgungHoatDong == null)
                {
                    ModelState.AddModelError("NgayNgungHoatDong", "Ngày ngưng hoạt động chưa nhập");
                }
            }
        }
      
        public JsonResult LoadDonVi(Guid idCoSo)
        {
            var data = _context.Departments.Where(it => it.Actived == true && it.IDCoSo == idCoSo).OrderBy(p => p.OrderIndex).Select(it => new { IdDepartment = it.Id, Name = it.Name }).ToList();
            return Json(data);
        }
        private void CreateViewBag( Guid? IdCoSo = null, Guid? IdDepartment = null,
            Guid? maChucVu = null,
            String? maTrinhDoHocVan = null, String? maTrinhDoChinhTri = null,
            String? maDanToc = null, String? maTonGiao = null,string? maTrinhDoChuyenMon = null,Guid? maDiaBanHoatDong = null,string? maHocVi = null)
        {

            //var MenuListCoSo = _context.CoSos.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { IdCoSo = it.IdCoSo, TenCoSo = it.TenCoSo }).ToList();
            //ViewBag.IdCoSo = new SelectList(MenuListCoSo, "IdCoSo", "TenCoSo", IdCoSo);

            //var DonVi = _context.Departments.Where(it => it.Actived == true).Include(it => it.CoSo).OrderBy(p => p.OrderIndex).Select(it => new { IdDepartment = it.Id, Name = it.Name + " " + it.CoSo.TenCoSo }).ToList();
            //ViewBag.IdDepartment = new SelectList(DonVi, "IdDepartment", "Name", IdDepartment);

            var chucVu = _context.ChucVus.Where(it => it.Actived == true).OrderBy(p => p.OrderIndex).Select(it => new { MaChucVu = it.MaChucVu, TenChucVu = it.TenChucVu }).ToList();
            ViewBag.MaChucVu = new SelectList(chucVu, "MaChucVu", "TenChucVu", maChucVu);



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

            var diaBanHoatDong = _context.DiaBanHoatDongs.Where(it => it.Actived == true).Select(it => new { MaDiaBanHoatDong = it.Id, Name = it.TenDiaBanHoatDong  }).ToList();
            ViewBag.MaDiaBanHoatDong = new SelectList(diaBanHoatDong, "MaDiaBanHoatDong", "Name", maDiaBanHoatDong);

            var hocVis = _context.HocVis.Select(it => new { MaHocVi = it.MaHocVi, TenHocVi = it.TenHocVi }).ToList();
            ViewBag.MaHocVi = new SelectList(hocVis, "MaHocVi", "TenHocVi", maHocVi);
        }
        private void CreateViewBagSearch()
        {

            var diaBanHoatDong = _context.DiaBanHoatDongs.Where(it => it.Actived == true).Select(it => new { MaDiaBanHoatDong = it.Id, Name = it.TenDiaBanHoatDong }).ToList();
            ViewBag.MaDiaBanHoatDong = new SelectList(diaBanHoatDong, "MaDiaBanHoatDong", "Name");
        }


        #endregion Helper

        #region Insert/Update data from excel file
        public string ExecuteImportExcelMenu(HoiVienImportExcel HoiVienExcel)
        {
            //Check:
            //1. If MenuId == "" then => Insert
            //2. Else then => Update
            CanBo  canbo = new CanBo(); ;
            if (HoiVienExcel.isNullValueId == true)
            {
                canbo = HoiVienExcel.GetHoiVien(canbo);
                canbo.HoiVienDuyet = false;
                canbo.IDCanBo = Guid.NewGuid();

                _context.Entry(canbo).State = EntityState.Added;
            }
            else
            {
                canbo = _context.CanBos.Where(p => p.IDCanBo == HoiVienExcel.IDCanBo).FirstOrDefault();
                if (canbo != null)
                {
                    canbo = HoiVienExcel.GetHoiVien(canbo);
                    HistoryModelRepository history = new HistoryModelRepository(_context);
                    history.SaveUpdateHistory(canbo.IDCanBo.ToString(), AccountId()!.Value, canbo);
                }
                else
                {
                    return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                            LanguageResource.HoiVien, canbo.IDCanBo,
                                            string.Format(LanguageResource.Export_ExcelHeader,
                                            LanguageResource.HoiVien));
                }
            }
            _context.SaveChanges();
            return LanguageResource.ImportSuccess;
        }
        #endregion Insert/Update data from excel file
        #region Check data type 
     
        public HoiVienImportExcel CheckTemplate(object[] row)
        {
            HoiVienImportExcel data = new HoiVienImportExcel();
            data.MaChucVu = Guid.Parse("D710D930-8342-474B-90A4-A1170A7A5691");
            string? value;
            int index = 0;
            for (int i = 0; i < row.Length; i++)
            {
                value = row[i] == null ? null : row[i].ToString().Trim();
                switch (i)
                {
                    case 0:
                        //Row Index
                        data.RowIndex = index = int.Parse(row[i].ToString());
                        break;
                    case 1:
                        // IDCanBo
                        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                        {
                            data.isNullValueId = true;
                        }
                        else
                        {
                            data.IDCanBo = Guid.Parse(value);
                            data.isNullValueId = false;
                        }
                        break;
                    case 2:
                        // Mã nhân viên
                        if (string.IsNullOrEmpty(value))
                        {
                            data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.MaCanBo), index);
                        }
                        else
                        {
                            data.MaCanBo = value;
                        }
                        break;
                    case 3:
                        // Mã nhân viên
                        if (string.IsNullOrEmpty(value))
                        {
                            data.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.FullName), index);
                        }
                        else
                        {
                            data.HoVaTen = value;
                        }
                        break;
                    case 4:

                        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0}", LanguageResource.NgaySinh), index);
                        }
                        else
                        {
                            try
                            {
                                //data.NgaySinh = DateTime.ParseExact(ngaySinh, DateFomat, new CultureInfo("en-US"));
                                data.NgaySinh = value;
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgaySinh, value, index);
                            }

                        }
                        break;
                    case 5:
                        // giới tính
                        if (string.IsNullOrEmpty(value))
                        {
                            data.GioiTinh = GioiTinh.Nữ;
                        }
                        else
                        {
                            data.GioiTinh = GioiTinh.Nam;
                        }
                        break;
                    case 6:
                        //  SoCCCD (*)
                        if (string.IsNullOrEmpty(value))
                        {
                            data.SoCCCD = "Không có";
                            //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.SoCCCD), index);
                        }
                        else
                        {
                            data.SoCCCD = value;

                        }
                        break;
                    case 7:
                        // ngày cấp SoCCCD (*)
                        if (string.IsNullOrEmpty(value) || value == "")
                        {
                            data.NgayCapCCCD = null;
                            //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.NgayCapCCCD), index);
                        }
                        else
                        {
                            try
                            {
                                data.NgayCapCCCD = value;
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayCapCCCD, value, index);
                            }

                        }
                        break;
                    case 8:
                        //  SoCCCD (*);
                        if (string.IsNullOrEmpty(value))
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.HoKhauThuongTru), index);
                        }
                        else
                        {
                            data.HoKhauThuongTru = value;

                        }
                        break;
                    case 9:
                        //  SoCCCD (*)

                        if (string.IsNullOrEmpty(value))
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.ChoOHienNay), index);
                        }
                        else
                        {
                            data.ChoOHienNay = value;

                        }
                        break;
                    case 10:
                        // so DT (*)
                        if (string.IsNullOrEmpty(value) || value == "")
                        {
                            data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa nhập thông tin {0} ", LanguageResource.SoDienThoai), index);
                        }
                        else
                        {
                            data.SoDienThoai = value;

                        }
                        break;

                    case 11:
                        // KhoanDenNgay

                        if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.NgayvaoDangDuBi = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US")); ;
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayvaoDangDuBi, value, index);
                            }
                        }
                        break;
                    case 12:
                        // KhoanDenNgay

                        if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                data.NgayVaoDangChinhThuc = DateTime.ParseExact(value, DateFomat, new CultureInfo("en-US")); ;
                            }
                            catch (Exception)
                            {

                                data.Error += string.Format("Kiểu dữ liệu cột {0} giá trị {1} ở dòng số {2} không hợp lệ!", LanguageResource.NgayVaoDangChinhThuc, value, index);
                            }
                        }
                        break;
                    case 13:
                        //  dân tộc (*)
                        if (string.IsNullOrEmpty(value))
                        {
                            data.MaDanToc = "KH";
                            //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.DanToc), index);
                        }
                        else
                        {
                            var obj = _context.DanTocs.FirstOrDefault(it => it.TenDanToc == value);
                            if (obj != null)
                            {
                                data.MaDanToc = obj.MaDanToc;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy dân tộc có tên {0} ở dòng số {1} !", value, index);
                            }

                        }
                        break;
                    case 14:
                        //  tôn giáo (*)

                        if (string.IsNullOrEmpty(value))
                        {
                            data.MaTonGiao = "KH";
                            //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.TonGiao), index);
                        }
                        else
                        {
                            var obj = _context.TonGiaos.FirstOrDefault(it => it.TenTonGiao == value);
                            if (obj != null)
                            {
                                data.MaTonGiao = obj.MaTonGiao;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy tôn giáo có tên {0} ở dòng số {1} !", value, index);
                            }

                        }
                        break;
                    case 15:

                        if (string.IsNullOrEmpty(value))
                        {
                            //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.TrinhDoHocVan), index);
                        }
                        else
                        {
                            var obj = _context.TrinhDoHocVans.FirstOrDefault(it => it.TenTrinhDoHocVan == value);
                            if (obj != null)
                            {
                                data.MaTrinhDoHocVan = obj.MaTrinhDoHocVan;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy trình độ học vấn có tên {0} ở dòng số {1} !", value, index);
                            }

                        }
                        break;
                    case 16:
                        //  TenTrinhDoHocVan (*)
                        if (string.IsNullOrEmpty(value))
                        {
                            //data.Error += string.Format(LanguageResource.Validation_ImportRequired, string.Format("chưa chọn thông tin {0} ", LanguageResource.TrinhDoChuyenMon), index);
                        }
                        else
                        {
                            var obj = _context.TrinhDoChuyenMons.FirstOrDefault(it => it.TenTrinhDoChuyenMon == value);
                            if (obj != null)
                            {
                                data.MaTrinhDoChuyenMon = obj.MaTrinhDoChuyenMon;
                            }
                            else
                            {
                                data.Error += string.Format("Không tìm thấy trình độ học vấn có tên {0} ở dòng số {1} !", value, index);
                            }

                        }
                        break;

                    case 17:
                        //  MaTrinhDoChinhTri (*)

                        if (!string.IsNullOrEmpty(value))
                        {
                            var obj = _context.TrinhDoChinhTris.FirstOrDefault(it => it.TenTrinhDoChinhTri == value);
                            if (obj != null)
                            {
                                data.MaTrinhDoChinhTri = obj.MaTrinhDoChinhTri;
                            }
                            //else
                            //{
                            //    data.Error += string.Format("Không tìm thấy trình độ chính trị có tên {0} ở dòng số {1} !", tenChinhTri, index);
                            //}
                        }
                        break;
                    case 18:
                        //  Ngày vào hội (*)
                     
                        data.NgayVaoHoi = value;
                        break;
                    case 19:
                        data.NgayThamGiaCapUyDang = value;
                        break;
                    case 20:

                        data.NgayThamGiaHDND = value;
                        break;
                    case 21:
                        data.VaiTro = !String.IsNullOrWhiteSpace(value) ? "1" : null; ;
                        break;
                    case 22:
                        //  MaTrinhDoChinhTri (*)
                        data.VaiTro = !String.IsNullOrWhiteSpace(value) ? "2" : data.VaiTro;
                        data.VaiTroKhac = value.ToLower()!="x"?value:null;
                        break;
                    case 23:
                        //  Hộ nghèo  (*)

                        data.MaGiaDinhThuocDien = data.MaGiaDinhThuocDien == null && !String.IsNullOrWhiteSpace(value) ?"01": null;
                        break;
                    case 24:
                        //  Cận nghèo (*)

                        data.MaGiaDinhThuocDien = data.MaGiaDinhThuocDien == null && !String.IsNullOrWhiteSpace(value) ? "02" : null;
                        break;
                    case 25:
                        //  Cận nghèo (*)

                        data.MaGiaDinhThuocDien = data.MaGiaDinhThuocDien == null && !String.IsNullOrWhiteSpace(value) ? "03" : null;
                        break;
                    case 26:
                        //  khac (*)

                        data.MaGiaDinhThuocDien = data.MaGiaDinhThuocDien == null && !String.IsNullOrWhiteSpace(value) && value.ToLower()!="x" ? "04" : null;
                        data.GiaDinhThuocDienKhac = value;
                        break;
                    case 27:
                        //  nongdan (*)
                        data.MaNgheNghiep =  !String.IsNullOrWhiteSpace(value) ? "01" : data.MaNgheNghiep;
                        break;
                    case 28:
                        //  congnhan (*)
                        data.MaNgheNghiep =  !String.IsNullOrWhiteSpace(value) ? "02" : data.MaNgheNghiep;
                        break;
                    case 29:
                        //  công chức viên chức (*)

                        data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "03" : data.MaNgheNghiep;
                        break;
                    case 30:
                        //  công chức viên chức (*)

                        data.MaNgheNghiep =  !String.IsNullOrWhiteSpace(value) ? "04" : data.MaNgheNghiep;
                        break;
                    case 31:
                        //  công chức viên chức (*)
  
                        data.MaNgheNghiep =  !String.IsNullOrWhiteSpace(value) ? "05" : data.MaNgheNghiep;
                        break;
                    case 32:
                        //  công chức viên chức (*)

                        data.MaNgheNghiep = !String.IsNullOrWhiteSpace(value) ? "06" : data.MaNgheNghiep;
                        break;
                    case 33:
                        //  công chức viên chức (*)

                        data.MaNgheNghiep =  !String.IsNullOrWhiteSpace(value) ? "07" : data.MaNgheNghiep;
                        break;
                    case 34:
                        //  công chức viên chức (*)

                        data.Loai_DV_SX_ChN = value;
                        break;

                    case 35:
                        //  công chức viên chức (*)

                        data.SoLuong = value;
                        break;
                    case 36:
                        //  công chức viên chức (*)

                        data.DienTich = value;
                        break;
                    case 37:
                        //  công chức viên chức (*)

                        data.ThamGia_SH_DoanThe_HoiDoanKhac = value;
                        break;
                    case 38:
                        //  công chức viên chức (*)

                        data.ThamGia_CLB_DN_MH_HTX_THT = value;
                        break;
                    case 39:
                        //  công chức viên chức (*)

                        data.ThamGia_THNN_CHNN = value;
                        break;
                    case 40:
                        //  công chức viên chức (*)
                        data.LoaiHoiVien = data.LoaiHoiVien ==null && !String.IsNullOrWhiteSpace(value) ? "01" : "";
                        break;
                    case 41:
                        //  công chức viên chức (*)
                        data.LoaiHoiVien = data.LoaiHoiVien == null && !String.IsNullOrWhiteSpace(value) ? "02" : "";
                        break;
                }
            }
            return data;
        }
        #endregion Check data type 
    }
}
