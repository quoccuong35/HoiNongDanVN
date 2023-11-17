using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Extensions;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HoiVien_ToHoiNN_ChiHoiNNController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        const string controllerCode = ConstExcelController.HoiVien_ToHoiNN_ChiHoiNN;
        const int startIndex = 6;
        public HoiVien_ToHoiNN_ChiHoiNNController(AppDbContext context, IWebHostEnvironment hostEnvironment) :base(context) { _hostEnvironment = hostEnvironment; }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBagSearch();
            return View(new HoiVien_ToHoiNN_ChiHoiNNSearchVM());
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(HoiVien_ToHoiNN_ChiHoiNNSearchVM search)
        {
            return ExecuteSearch(() => {
                var data = _context.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Include(it => it.HoiVien).Include(it => it.HoiVien.DiaBanHoatDong).Include(it => it.ToHoiNganhNghe_ChiHoiNganhNghe).AsQueryable();
                if (search.Ma_ToHoiNganhNghe_ChiHoiNganhNghe != null)
                {
                    data = data.Where(it => it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe == search.Ma_ToHoiNganhNghe_ChiHoiNganhNghe);
                }
                if (search.MaDiaBan != null)
                {
                    data = data.Where(it => it.HoiVien.MaDiaBanHoatDong == search.MaDiaBan);
                }
                else
                {
                    data = data.Where(it => GetPhamVi().Contains(it.HoiVien.MaDiaBanHoatDong!.Value));
                }
                if (search.HoVaTen != null)
                {
                    data = data.Where(it => it.HoiVien.HoVaTen.Contains(search.HoVaTen));
                }
                if (search.MaHoiVien != null)
                {
                    data = data.Where(it => it.HoiVien.MaCanBo == search.MaHoiVien);
                }
                var model = data.Select(it => new HoiVien_ToHoiNN_ChiHoiNNDetailVM
                {
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
                    MaToHoi = it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe,
                    TenToHoi = it.ToHoiNganhNghe_ChiHoiNganhNghe.Ten,
                    TenDiaBanHoatDong = it.HoiVien.DiaBanHoatDong!.TenDiaBanHoatDong,
                }).ToList();
                return PartialView(model);

            });
        }
        #endregion Index
        #region Helper
        private void CreateViewBagSearch()
        {
            var chinhTriHoiDoanKhacs = _context.ToHoiNganhNghe_ChiHoiNganhNghes.Where(it => it.Actived == true).Select(it => new { Ma_ToHoiNganhNghe_ChiHoiNganhNghe = it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe, Ten = it.Ten }).ToList();
            ViewBag.Ma_ToHoiNganhNghe_ChiHoiNganhNghe = new SelectList(chinhTriHoiDoanKhacs, "Ma_ToHoiNganhNghe_ChiHoiNganhNghe", "Ten");

            var diaBans = _context.DiaBanHoatDongs.Where(it => it.Actived == true && GetPhamVi().Contains(it.Id)).Select(it => new { MaDiaBan = it.Id, Name = it.TenDiaBanHoatDong }).ToList();
            ViewBag.MaDiaBan = new SelectList(diaBans, "MaDiaBan", "Name");
        }
        #endregion Helper

        #region Export 
        public IActionResult ExportEdit(HoiVien_ToHoiNN_ChiHoiNNSearchVM search)
        {
            var data = _context.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Include(it => it.HoiVien).Include(it => it.HoiVien.DiaBanHoatDong).Include(it => it.ToHoiNganhNghe_ChiHoiNganhNghe).AsQueryable();
            if (search.Ma_ToHoiNganhNghe_ChiHoiNganhNghe != null)
            {
                data = data.Where(it => it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe == search.Ma_ToHoiNganhNghe_ChiHoiNganhNghe);
            }
            if (search.MaDiaBan != null)
            {
                data = data.Where(it => it.HoiVien.MaDiaBanHoatDong == search.MaDiaBan);
            }
            else
            {
                data = data.Where(it => GetPhamVi().Contains(it.HoiVien.MaDiaBanHoatDong!.Value));
            }
            if (search.HoVaTen != null)
            {
                data = data.Where(it => it.HoiVien.HoVaTen.Contains(search.HoVaTen));
            }
            if (search.MaHoiVien != null)
            {
                data = data.Where(it => it.HoiVien.MaCanBo == search.MaHoiVien);
            }
            var model = data.Select(it => new HoiVien_ToHoiNN_ChiHoiNNExcelVM
            {
                MaCanBo = it.HoiVien.MaCanBo,
                HoVaTen = it.HoiVien.HoVaTen,
                HoKhauThuongTru = it.HoiVien.HoKhauThuongTru,
                TenHoiNongDan = it.HoiVien.DiaBanHoatDong!.TenDiaBanHoatDong,
                ChoOHienNay = it.HoiVien.ChoOHienNay!,
                SoDienThoai = it.HoiVien.SoDienThoai,
                NgayVaoHoi = it.HoiVien.NgayVaoHoi,
                TenToHoi = it.ToHoiNganhNghe_ChiHoiNganhNghe.Ten,
                GioiTinh = (int)it.HoiVien.GioiTinh == 1 ? "Nam" : "Nữ"
            }).ToList();
            return Export(model);
        }
        public FileContentResult Export(List<HoiVien_ToHoiNN_ChiHoiNNExcelVM> menu)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "MaCanBo" });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen" });
            columns.Add(new ExcelTemplate() { ColumnName = "TenHoiNongDan" });
            columns.Add(new ExcelTemplate() { ColumnName = "TenToHoi" });
            columns.Add(new ExcelTemplate() { ColumnName = "GioiTinh" });
            columns.Add(new ExcelTemplate() { ColumnName = "HoKhauThuongTru" });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay" });
            columns.Add(new ExcelTemplate() { ColumnName = "SoDienThoai" });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoHoi" });

            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.ToHoiNN_ChiHoiNN);
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
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            return ExecuteDelete(() =>
            {
                string[] keyDels = id.Split('_');

                var del = _context.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.FirstOrDefault(p => p.IDHoiVien == Guid.Parse(keyDels[0]) && p.Ma_ToHoiNganhNghe_ChiHoiNganhNghe == Guid.Parse(keyDels[1]));


                if (del != null)
                {
                    _context.Remove(del);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.ToHoiNN_ChiHoiNN.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.ToHoiNN_ChiHoiNN.ToLower())
                    });
                }
            });
        }
        #endregion Delete
    }
}
