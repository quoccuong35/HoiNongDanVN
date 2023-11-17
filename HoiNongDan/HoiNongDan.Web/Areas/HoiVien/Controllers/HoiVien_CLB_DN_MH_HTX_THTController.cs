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
    public class HoiVien_CLB_DN_MH_HTX_THTController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        const string controllerCode = ConstExcelController.HoiVien_CLB_DN_MH_HTX_THT;
        const int startIndex = 6;
        public HoiVien_CLB_DN_MH_HTX_THTController(AppDbContext context, IWebHostEnvironment hostEnvironment) :base(context) { _hostEnvironment = hostEnvironment; }
        #region Index
        [HoiNongDanAuthorization]
        public IActionResult Index()
        {
            CreateViewBagSearch();
            return View(new HoiVien_CLB_DN_MH_HTX_THTSearchVM());
        }
        [HoiNongDanAuthorization]
        public IActionResult _Search(HoiVien_CLB_DN_MH_HTX_THTSearchVM search)
        {
            return ExecuteSearch(() => {
                var data = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Include(it => it.HoiVien).Include(it => it.HoiVien.DiaBanHoatDong).Include(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac).AsQueryable();
                if (search.Id_CLB_DN_MH_HTX_THT != null)
                {
                    data = data.Where(it => it.Id_CLB_DN_MH_HTX_THT == search.Id_CLB_DN_MH_HTX_THT);
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
                var model = data.Select(it => new HoiVien_CLB_DN_MH_HTX_THTDetailVM
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
                    MaCaulacBo = it.Id_CLB_DN_MH_HTX_THT,
                    TenCauLacBo = it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten,
                    TenDiaBanHoatDong = it.HoiVien.DiaBanHoatDong!.TenDiaBanHoatDong,
                }).ToList();
                return PartialView(model);

            });
        }
        #endregion Index
        #region Helper
        private void CreateViewBagSearch()
        {
            var chinhTriHoiDoanKhacs = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.Where(it => it.Actived == true).Select(it => new { Id_CLB_DN_MH_HTX_THT = it.Id_CLB_DN_MH_HTX_THT, Ten = it.Ten }).ToList();
            ViewBag.Id_CLB_DN_MH_HTX_THT = new SelectList(chinhTriHoiDoanKhacs, "Id_CLB_DN_MH_HTX_THT", "Ten");

            var diaBans = _context.DiaBanHoatDongs.Where(it => it.Actived == true && GetPhamVi().Contains(it.Id)).Select(it => new { MaDiaBan = it.Id, Name = it.TenDiaBanHoatDong }).ToList();
            ViewBag.MaDiaBan = new SelectList(diaBans, "MaDiaBan", "Name");
        }
        #endregion Helper

        #region Export 
        public IActionResult ExportEdit(HoiVien_CLB_DN_MH_HTX_THTSearchVM search)
        {
            var data = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Include(it => it.HoiVien).Include(it => it.HoiVien.DiaBanHoatDong).Include(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac).AsQueryable();
            if (search.Id_CLB_DN_MH_HTX_THT != null)
            {
                data = data.Where(it => it.Id_CLB_DN_MH_HTX_THT == search.Id_CLB_DN_MH_HTX_THT);
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
            var model = data.Select(it => new HoiVien_CLB_DN_MH_HTX_THTExcelVM
            {
                MaCanBo = it.HoiVien.MaCanBo,
                HoVaTen = it.HoiVien.HoVaTen,
                HoKhauThuongTru = it.HoiVien.HoKhauThuongTru,
                TenHoiNongDan = it.HoiVien.DiaBanHoatDong!.TenDiaBanHoatDong,
                ChoOHienNay = it.HoiVien.ChoOHienNay!,
                SoDienThoai = it.HoiVien.SoDienThoai,
                NgayVaoHoi = it.HoiVien.NgayVaoHoi,
                TenCauLacBo = it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten,
                GioiTinh = (int)it.HoiVien.GioiTinh == 1 ? "Nam" : "Nữ"
            }).ToList();
            return Export(model);
        }
        public FileContentResult Export(List<HoiVien_CLB_DN_MH_HTX_THTExcelVM> menu)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "MaCanBo" });
            columns.Add(new ExcelTemplate() { ColumnName = "HoVaTen" });
            columns.Add(new ExcelTemplate() { ColumnName = "TenHoiNongDan" });
            columns.Add(new ExcelTemplate() { ColumnName = "TenCauLacBo" });
            columns.Add(new ExcelTemplate() { ColumnName = "GioiTinh" });
            columns.Add(new ExcelTemplate() { ColumnName = "HoKhauThuongTru" });
            columns.Add(new ExcelTemplate() { ColumnName = "ChoOHienNay" });
            columns.Add(new ExcelTemplate() { ColumnName = "SoDienThoai" });
            columns.Add(new ExcelTemplate() { ColumnName = "NgayVaoHoi" });

            //Header
            List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac);
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
        public IActionResult Delete(string id)
        {
            return ExecuteDelete(() =>
            {
                string[] keyDels = id.Split('_');

                var del = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.FirstOrDefault(p => p.IDHoiVien == Guid.Parse(keyDels[0]) && p.Id_CLB_DN_MH_HTX_THT == Guid.Parse(keyDels[1]));


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
