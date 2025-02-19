using HoiNongDan.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HoiNongDan.Extensions;
using HoiNongDan.DataAccess;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Transactions;
using Microsoft.CodeAnalysis.Differencing;
using HoiNongDan.DataAccess.Migrations;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HoTroController : BaseController
    {
        const int startIndex = 5;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string filemau = @"upload\filemau\HoiVienHoTro.xlsx";
        public HoTroController(AppDbContext context, IWebHostEnvironment hostEnvironment) : base(context) { _hostEnvironment = hostEnvironment; }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            HVHoTroSearchVM model = new HVHoTroSearchVM();
            CreateViewBagSearch();
            return View(model);
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(HVHoTroSearchVM search)
        {
            return ExecuteSearch(() =>
            {
                var data = (from hvht in _context.HoiVienHoTros 
                                join hv in _context.CanBos on hvht.IDHoiVien equals hv.IDCanBo
                                join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                            where pv.AccountId == AccountId()
                            select hvht).Distinct().Include(it => it.LopHoc).Include(it => it.HoiVien).ThenInclude(it => it.DiaBanHoatDong).ThenInclude(it=>it.QuanHuyen).Include(it=>it.LopHoc).ThenInclude(it=>it.HinhThucHoTro).AsQueryable();
                
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
                
                if (search.Actived != null)
                {
                    data = data.Where(it => it.Actived == search.Actived);
                }
                if (search.MaDiaBanHoiVien != null)
                {
                    data = data.Where(it => it.HoiVien.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
                }
                if (search.IDLopHoc != null)
                {
                    data = data.Where(it => it.IDLopHoc == search.IDLopHoc);
                }
                if (search.MaHinhThucHoTro != null)
                {
                    data = data.Where(it => it.LopHoc.MaHinhThucHoTro == search.MaHinhThucHoTro);
                }
                var model = data.Select(it => new HoiVienHoTroDetailVM
                {
                    ID = it.ID,
                    MaHV = it.HoiVien.MaCanBo!,
                    TenHV = it.HoiVien.HoVaTen,
                    TenLopHoc = it.LopHoc.TenLopHoc,
                    NoiDung = it.NoiDung,
                    QuanHuyen = it.HoiVien.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                    TenHoi = it.HoiVien.DiaBanHoatDong.TenDiaBanHoatDong,
                    GhiChu = it.GhiChu
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create()
        {
            HoiVienHoTroVM item = new HoiVienHoTroVM();
            HoiVienInfo nhanSu = new HoiVienInfo();

            item.HoiVien = nhanSu;
            CreateViewBag();
            return View(item);
        }
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HoiVienHoTroVM obj)
        {
            CheckError(obj);
            return ExecuteContainer(() =>
            {
                var add = new HoiVienHoTro();
                add.ID = Guid.NewGuid();
                add.NoiDung = obj.NoiDung;
                add.GhiChu = obj.GhiChu;
                add.IDLopHoc = obj.IDLopHoc;
                add.IDHoiVien = obj.HoiVien.IdCanbo!.Value;
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.HoiVienHoTros.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.HoiVienVayVon.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id)
        {
            var item = _context.HoiVienHoTros.SingleOrDefault(it => it.ID == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            HoiVienHoTroVM obj = new HoiVienHoTroVM();


            obj.ID = item.ID;
            obj.NoiDung = item.NoiDung;
            obj.GhiChu = item.GhiChu;
            obj.IDLopHoc = item.IDLopHoc;
          
            obj.HoiVien = GetThongTinNhanSu(item.IDHoiVien);
            CreateViewBag(obj.IDLopHoc);
            return View(obj);
        }
        [HttpPost]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HoiVienHoTroVM obj)
        {
            CheckError(obj);
            return ExecuteContainer(() =>
            {
                var edit = _context.HoiVienHoTros.SingleOrDefault(it => it.ID == obj.ID);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.HoiVienVayVon.ToLower())
                    });
                }
                else
                {
                    edit.NoiDung =obj.NoiDung;
                    edit.GhiChu = obj.GhiChu;
                    edit.IDLopHoc = obj.IDLopHoc;
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.HoiVienVayVon.ToLower())
                    });
                }
            });
        }
        #endregion Edit
        #region Export
        [HoiNongDanAuthorization]
        public IActionResult ExportCreate()
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, filemau);
            byte[] filecontent = ClassExportExcel.ExportFileMau(url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "HoiVienHoTro");

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit(HoiVien_ToHoiNN_ChiHoiNNSearchVM search)
        {
            var model = new List< HoiVien_ToHoiNN_ChiHoiNNSearchVM>();
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, filemau);
            byte[] filecontent = ClassExportExcel.ExportExcel(model, startIndex, url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "HoiVienHoTro");

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export
        #region Import
        public IActionResult _Import()
        {
            CreateViewBagSearch();
            return PartialView(); 
        }
        public IActionResult Import( Guid? IDLopHoc) {
            if (IDLopHoc == null)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = "Chưa chọn lớp"
                });
            }
            else
            {
                var checkLop = _context.LopHocs.SingleOrDefault(it => it.IDLopHoc == IDLopHoc.Value && it.Actived == true);
                if(checkLop == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Không tìm thấy thông tin lớp học"
                    });
                }
            }
            DataSet ds = GetDataSetFromExcel();
            if (ds != null && ds.Tables.Count > 0) {
              
                DataTable dt = ds.Tables[0];
                if (dt == null || dt.Rows.Count < startIndex)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Không có dữ liệu import"
                    });

                }

                var hoiVienHoTros = _context.HoiVienHoTros.Where(it => it.IDLopHoc == IDLopHoc).Select(it => new HoiVienHoTro { IDHoiVien = it.IDHoiVien, IDLopHoc = it.IDLopHoc }).ToList();
                List<string> errorList = new List<string>();
                return ExcuteImportExcel(() => {
                    const TransactionScopeOption opt = new TransactionScopeOption();

                    TimeSpan span = new TimeSpan(0, 0, 30, 30);
                    using (TransactionScope ts = new TransactionScope(opt, span))
                    {
                        List<HoiVienHoTro> adds = new List<HoiVienHoTro>();
                        foreach (DataRow row in dt.Rows)
                        {
                            if (dt.Rows.IndexOf(row) >= startIndex-1)
                            {
                                if (row[0] == null || row[0].ToString() == "")
                                    break;
                                CheckTemplate(row: row, IDLopHoc: IDLopHoc.Value, adds: adds, error: errorList);
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
                        int stt = 0;
                        if (adds.Count > 0)
                        {
                            _context.HoiVienHoTros.AddRange(adds);
                            stt = _context.SaveChanges();
                        }
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
                                Data = "Không có thông tin import"
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
                            return null;
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
                return null;
            }
        }

        [NonAction]
        private void CheckTemplate(DataRow row, Guid IDLopHoc,  List<HoiVienHoTro> adds,List<String> error) {
            string? index = "", hoVaTen = "",  soCCCD = "", noiDung = "", ghiChu = "";

            index = row[0].ToString();

            hoVaTen = row[1].ToString();

            soCCCD = row[2] != null ? row[2].ToString()!.Trim() : "";

            noiDung = row[3] != null ? row[3].ToString()!.Trim() : "";
            ghiChu = row[7] != null ? row[7].ToString()!.Trim() : "";

            if (string.IsNullOrWhiteSpace(noiDung))
            {
                error.Add(String.Format("Chua nhập nội dung dòng ", index));
            }

            var hoiVien = ChekcHoiVien( hoVaTen: hoVaTen, soCCCD: soCCCD);
            if (hoiVien == null)
            {
                error.Add(String.Format(LanguageResource.ErrorImportChiToHoiNganhNghe2, index + " " + hoVaTen));
            }
            else 
            {
                adds.Add(new HoiVienHoTro
                {
                    ID = Guid.NewGuid(),
                    IDLopHoc = IDLopHoc,
                    IDHoiVien = hoiVien.IDCanBo,
                    NoiDung = noiDung,
                    GhiChu = ghiChu,
                    Actived = true,
                    CreatedAccountId = AccountId(),
                    CreatedTime = DateTime.Now,
                });
            }

        }
        [NonAction]
        private CanBo? ChekcHoiVien( string? hoVaTen, string? soCCCD)
        {
           var data = (from cb in _context.CanBos
             join pv in _context.PhamVis on cb.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
             where pv.AccountId == AccountId()
             && cb.IsHoiVien == true && cb.HoiVienDuyet == true
             select cb).SingleOrDefault(it => it.IsHoiVien == true && it.SoCCCD == soCCCD && it.isRoiHoi != true && it.HoiVienDuyet == true);
            return data;
        }

        #endregion Import
        #region Del
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.HoiVienHoTros.FirstOrDefault(p => p.ID == id);
                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.HoiVienVayVon.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.HoiVienVayVon.ToLower())
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
                var data = _context.HoiVienHoTros.Include(it => it.ID).AsQueryable();
                var model = data.Select(it => new HoiVienHoTroDetailVM
                {
                    ID = it.ID,
                    MaHV = it.HoiVien.MaCanBo!,
                    TenHV = it.HoiVien.HoVaTen,
                    NoiDung = it.NoiDung,
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Report
        #region Helper
        private void CreateViewBag(Guid? IDLopHoc = null) {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.IDLopHoc = fnViewBag.LopHoc(value: IDLopHoc);
        }
        [NonAction]
        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaHinhThucHoTro = fnViewBag.HinhThucHoTro();

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
            ViewBag.IDLopHoc = fnViewBag.LopHoc();
        }
        private void CheckError(HoiVienHoTroVM obj)
        {
            if (obj.HoiVien.IdCanbo == null)
            {
                ModelState.AddModelError("HoiVien", "Chưa chọn thông tin hội viên");
            }
        }
        [NonAction]
        private HoiVienInfo GetThongTinNhanSu(Guid maHoiVien)
        {
            HoiVienInfo HoiVien = new HoiVienInfo();
            var phamVis = Function.GetPhamVi(AccountId: AccountId()!.Value, _context: _context);
            var data = _context.CanBos.FirstOrDefault(it => it.IDCanBo == maHoiVien && phamVis.Contains(it.MaDiaBanHoatDong!.Value) && it.IsHoiVien == true);
            var diaBan = _context.DiaBanHoatDongs.SingleOrDefault(it => it.Id == data!.MaDiaBanHoatDong);
            var quanThanhPho = _context.QuanHuyens.SingleOrDefault(it => it.MaQuanHuyen == diaBan!.MaQuanHuyen);
            HoiVien.IdCanbo = data!.IDCanBo;
            HoiVien.HoVaTen = data.HoVaTen;
            HoiVien.MaCanBo = data.MaCanBo!;
            HoiVien.DiaBan = diaBan!.TenDiaBanHoatDong;
            HoiVien.NgaySinh = data!.NgaySinh;
            HoiVien.HoKhauThuongTru = data.HoKhauThuongTru;
            HoiVien.SoCCCD = data.SoCCCD;
            HoiVien.QuanHuyen = quanThanhPho!.TenQuanHuyen;
            HoiVien.Edit = false;
            return HoiVien;
        }
        #endregion Helper
    }
}
