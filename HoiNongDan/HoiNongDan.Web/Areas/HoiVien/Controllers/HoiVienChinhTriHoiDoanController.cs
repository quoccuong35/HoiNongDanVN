using HoiNongDan.Constant;
using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Extensions;
using HoiNongDan.DataAccess;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Entity;
using NuGet.Protocol;
using System.Linq;
using HoiNongDan.Resources;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HoiVienChinhTriHoiDoanController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        const string controllerCode = ConstExcelController.HoiVienChinhTriHoiDoan;
        const int startIndex = 6;
        public HoiVienChinhTriHoiDoanController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context) {
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
        [HoiNongDanAuthorization]
        public IActionResult _Search(HoiVienChinhTriHoiDoanSearchVM search) {
            return ExecuteSearch(() => {
                var phamVis = Function.GetPhamVi(AccountId: AccountId()!.Value, _context: _context);
                var data = _context.DoanTheChinhTri_HoiDoan_HoiViens
                    .Include(it => it.HoiVien)
                    .Include(it => it.HoiVien.DiaBanHoatDong)
                    .Include(it=>it.DoanTheChinhTri_HoiDoan).AsQueryable();
                if (search.MaDoanTheChinhTri_HoiDoan != null)
                {
                    data = data.Where(it => it.MaDoanTheChinhTri_HoiDoan == search.MaDoanTheChinhTri_HoiDoan);
                }
                if (search.MaDiaBanHoiVien != null)
                {
                    data = data.Where(it => it.HoiVien.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
                }
                else
                {
                    data = data.Where(it => phamVis.Contains(it.HoiVien.MaDiaBanHoatDong!.Value));
                }
                if (search.HoVaTen != null)
                {
                    data = data.Where(it => it.HoiVien.HoVaTen.Contains(search.HoVaTen));
                }
                if (search.MaHoiVien != null)
                {
                    data = data.Where(it => it.HoiVien.MaCanBo ==search.MaHoiVien);
                }
                if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen))
                {
                    data = data.Where(it => it.HoiVien.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
                }
                var model = data.Select(it=>new HoiVienChinhTriHoiDoanDetailVM { 
                    IDCanBo = it.HoiVien.IDCanBo,
                    MaCanBo = it.HoiVien.MaCanBo,
                    HoVaTen = it.HoiVien.HoVaTen,
                    NgaySinh = it.HoiVien.NgaySinh,
                    HoKhauThuongTru = it.HoiVien.HoKhauThuongTru,
                    SoDienThoai = it.HoiVien.SoDienThoai,
                    NgayVaoHoi = it.HoiVien.NgayVaoHoi,
                    Loai_DV_SX_ChN = it.HoiVien.Loai_DV_SX_ChN,
                    SoLuong = it.HoiVien.SoLuong,
                    DienTich_QuyMo = it.HoiVien.DienTich_QuyMo,
                    MaDoanTheChinhTri_HoiDoan = it.MaDoanTheChinhTri_HoiDoan,
                    TenDoanTheChinhChi_HoiDon = it.DoanTheChinhTri_HoiDoan.TenDoanTheChinhTri_HoiDoan,
                    TenDiaBanHoatDong = it.HoiVien.DiaBanHoatDong!.TenDiaBanHoatDong,
                }).ToList();
                return PartialView(model);
                
            });
        }
        #endregion Index
        #region Helper
        private void CreateViewBagSearch() { 
            var chinhTriHoiDoanKhacs = _context.DoanTheChinhTri_HoiDoans.Where(it=>it.Actived == true).Select(it => new{ MaDoanTheChinhTri_HoiDoan = it.MaDoanTheChinhTri_HoiDoan,TenDoanTheChinhTri_HoiDoan = it.TenDoanTheChinhTri_HoiDoan}).ToList();
            ViewBag.MaDoanTheChinhTri_HoiDoan = new SelectList(chinhTriHoiDoanKhacs, "MaDoanTheChinhTri_HoiDoan", "TenDoanTheChinhTri_HoiDoan");

            FnViewBag fnViewBag = new FnViewBag(_context);

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }
        #endregion Helper

        #region Export 
        public IActionResult ExportEdit(HoiVienChinhTriHoiDoanSearchVM search)
        {
            var phamVis = Function.GetPhamVi(AccountId: AccountId()!.Value, _context: _context);
            var data = _context.DoanTheChinhTri_HoiDoan_HoiViens.Include(it => it.HoiVien).Include(it => it.HoiVien.DiaBanHoatDong).Include(it => it.DoanTheChinhTri_HoiDoan).AsQueryable();
            if (search.MaDoanTheChinhTri_HoiDoan != null)
            {
                data = data.Where(it => it.MaDoanTheChinhTri_HoiDoan == search.MaDoanTheChinhTri_HoiDoan);
            }
            if (search.MaDiaBanHoiVien != null)
            {
                data = data.Where(it => it.HoiVien.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
            }
            else
            {
                data = data.Where(it => phamVis.Contains(it.HoiVien.MaDiaBanHoatDong!.Value));
            }
            if (search.HoVaTen != null)
            {
                data = data.Where(it => it.HoiVien.HoVaTen.Contains(search.HoVaTen));
            }
            if (search.MaHoiVien != null)
            {
                data = data.Where(it => it.HoiVien.MaCanBo == search.MaHoiVien);
            }
            var model = data.Select(it => new HoiVienChinhTriHoiDoanExcelVM
            {
                MaCanBo = it.HoiVien.MaCanBo,
                HoVaTen = it.HoiVien.HoVaTen,
                HoKhauThuongTru = it.HoiVien.HoKhauThuongTru,
                TenHoiNongDan = it.HoiVien.DiaBanHoatDong!.TenDiaBanHoatDong,
                ChoOHienNay = it.HoiVien.ChoOHienNay!,
                SoDienThoai = it.HoiVien.SoDienThoai,
                NgayVaoHoi = it.HoiVien.NgayVaoHoi != null? it.HoiVien.NgayVaoHoi.Value.ToString("dd/MM/yyyy"):null,
                TenDoanTheChinhChi_HoiDon = it.DoanTheChinhTri_HoiDoan.TenDoanTheChinhTri_HoiDoan,
                GioiTinh = (int)it.HoiVien.GioiTinh ==1?"Nam":"Nữ"
            }).ToList();
            return Export(model);
        }
        public FileContentResult Export(List<HoiVienChinhTriHoiDoanExcelVM> menu)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "MaCanBo"});
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen" });
            columns.Add(new ExcelTemplate() { ColumnName = "TenHoiNongDan"});
            columns.Add(new ExcelTemplate() { ColumnName = "TenDoanTheChinhChi_HoiDon" });
            columns.Add(new ExcelTemplate() { ColumnName = "GioiTinh"});
            columns.Add(new ExcelTemplate() { ColumnName = "HoKhauThuongTru"});
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay"});
            columns.Add(new ExcelTemplate() { ColumnName = "SoDienThoai" });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoHoi" });

            

            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.HoiVienChinhTriHoiDoan);
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
        #endregion Export 
        #region Delete
        [HttpDelete]
        [ValidateAntiForgeryToken]
        [HoiNongDanAuthorization]
        public JsonResult Delete(string id)
        {
            return ExecuteDelete(() =>
            {
                string[]keyDels = id.Split('_');
               
                var del = _context.DoanTheChinhTri_HoiDoan_HoiViens.FirstOrDefault(p => p.IDHoiVien == Guid.Parse(keyDels[0]) && p.MaDoanTheChinhTri_HoiDoan == Guid.Parse(keyDels[1]));


                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.HoiVienChinhTriHoiDoan.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.HoiVienChinhTriHoiDoan.ToLower())
                    });
                }
            });
        }
        #endregion Delete
    }
}
