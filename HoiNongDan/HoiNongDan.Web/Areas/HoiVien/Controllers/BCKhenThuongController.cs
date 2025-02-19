using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class BCKhenThuongController : BaseController
    {
        const int startIndex = 5;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        public BCKhenThuongController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context) {
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
        public IActionResult _Search(string? MaQuanHuyen, Guid? MaDiaBanHoiVien, int? TuNam,int? DenNam)
        {
            return ExecuteSearch(() => {
                var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoiVien, TuNam: TuNam, DenNam: DenNam).ToList();
                return PartialView(model);
            });
        }
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit(string? MaQuanHuyen, Guid? MaDiaBanHoiVien, int? TuNam, int? DenNam)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\ThongKeKhenThuongHoiVien.xlsx");
            var data = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoiVien, TuNam: TuNam,DenNam:DenNam);

            byte[] filecontent;
            bool bold = false;
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(url), false))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets["Data"];
                int stt = 0,start = startIndex;
                foreach (var item in data)
                {
                    stt++;
                    if (item.TenDiaBanHoatDong.ToLower().Contains("tổng"))
                    {
                        workSheet.Cells["A" + start].Style.Font.Bold = true;
                        workSheet.Cells["A" + start].Style.Font.Size = 16;
                        workSheet.Cells["B" + start].Style.Font.Bold = true;
                        workSheet.Cells["B" + start].Style.Font.Size = 16;
                        bold = true;
                    }
                    workSheet.Cells["A" + start].Value = stt;
                    workSheet.Cells["B" + start].Value = item.TenDiaBanHoatDong;
                    workSheet.Cells["C" + start].Value = item.HoiVienUuTu;
                    workSheet.Cells["D" + start].Value = item.NDSXKDG_TW;
                    workSheet.Cells["E" + start].Value = item.NDSXKDG_Tp;
                    workSheet.Cells["F" + start].Value = item.NDSXKDG_H;
                    workSheet.Cells["G" + start].Value = item.NDSXKDG_CS;
                    workSheet.Cells["H" + start].Value = item.NDVietNamXuatSac;
                    workSheet.Cells["I" + start].Value = item.KNCViGCND;
                    workSheet.Cells["J" + start].Value = item.CanBoHoiCSG;
                    workSheet.Cells["K" + start].Value = item.SangTaoNhaNong;
                    workSheet.Cells["L" + start].Value = item.GuongDiemHinhTienTien;
                    workSheet.Cells["M" + start].Value = item.GuongDanVangKheo;
                    workSheet.Cells["N" + start].Value = item.GuongHocTapLamTheoLoiBac;


                    start++;
                }
                using (ExcelRange r = workSheet.Cells["A5:N" + startIndex.ToString()])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }
                filecontent = package.GetAsByteArray();
            }
            string fileNameWithFormat = string.Format("{0}.xlsx", "ThongKeKhenTangHoiVien");

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Index
        #region Helper
        private void CreateViewBagSearch(string? maQuanHuyen = null, Guid? maDiaBan = null)
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId(), value: maDiaBan);

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId(), value: maQuanHuyen);

        }

        private List<BaoCaoKhenThuong> LoadData(string? MaQuanHuyen, Guid? MaDiaBanHoatDong, int?TuNam, int? DenNam)
        {
            List<BaoCaoKhenThuong> data = new List<BaoCaoKhenThuong>();
            string id = "";
            if (MaDiaBanHoatDong != null)
                id = MaDiaBanHoatDong!.Value.ToString();
            
            var model = _context.BaoCaoKhenThuongs.FromSqlRaw("EXEC dbo.[BaoCaoKhenThuongHoiVien] @TuNam = '" + TuNam + "',@DenNam = '" + DenNam + "',@maquanhuyen ='" + MaQuanHuyen + "',@id ='" + id + "',@AccountId='" + AccountId().ToString() + "'").ToList().Where(it => it.Tong > 0);

            if (String.IsNullOrWhiteSpace(MaQuanHuyen))
            {
                List<Guid> pass = new List<Guid>();
                pass.Add(Guid.Parse("662ac072-fece-41e2-9a5e-e47c362d10cb"));
                pass.Add(Guid.Parse("bf7024f4-6bef-442a-9d6b-ce4538b1a084"));
                pass.Add(Guid.Parse("40a7400d-1981-45e8-b4a6-412af186dc5d"));

                var trucThuocThanhPho = model.Where(it => pass.Contains(it.Id));
                foreach (var it in trucThuocThanhPho)
                {
                    
                    data.Add(it);
                }

                List<String> quanHuyens = model.Where(it => !pass.Contains(it.Id)).Select(it => it.TenQuanHuyen).Distinct().ToList();
                foreach (var item in quanHuyens)
                {

                    BaoCaoKhenThuong add = new BaoCaoKhenThuong();
                    var tam = model.Where(it => it.TenQuanHuyen == item && !pass.Contains(it.Id));
                    add.TenDiaBanHoatDong = item;
                    add.TenQuanHuyen = item;
                    add.HoiVienUuTu = tam.Sum(it => it.HoiVienUuTu);
                    add.NDSXKDG_TW = tam.Sum(it => it.NDSXKDG_TW);
                    add.NDSXKDG_Tp = tam.Sum(it => it.NDSXKDG_Tp);
                    add.NDSXKDG_H = tam.Sum(it => it.NDSXKDG_H);
                    add.NDSXKDG_CS = tam.Sum(it => it.NDSXKDG_CS);
                    add.NDVietNamXuatSac = tam.Sum(it => it.NDVietNamXuatSac);
                    add.KNCViGCND = tam.Sum(it => it.KNCViGCND);
                    add.CanBoHoiCSG = tam.Sum(it => it.CanBoHoiCSG);
                    add.SangTaoNhaNong = tam.Sum(it => it.SangTaoNhaNong);
                    add.GuongDiemHinhTienTien = tam.Sum(it => it.GuongDiemHinhTienTien);
                    add.GuongDanVangKheo = tam.Sum(it => it.GuongDanVangKheo);
                    add.GuongHocTapLamTheoLoiBac = tam.Sum(it => it.GuongHocTapLamTheoLoiBac);
                    data.Add(add);
                }
            }
            else if (!String.IsNullOrWhiteSpace(MaQuanHuyen) || MaDiaBanHoatDong != null)
            {
                data = model.ToList();
            }

            // Add tổng
            if (MaDiaBanHoatDong == null)
            {
                BaoCaoKhenThuong add = new BaoCaoKhenThuong();
                add.TenDiaBanHoatDong = "Tổng";
                add.TenQuanHuyen = "Tổng";
                add.HoiVienUuTu = model.Sum(it => it.HoiVienUuTu);
                add.NDSXKDG_TW = model.Sum(it => it.NDSXKDG_TW);
                add.NDSXKDG_Tp = model.Sum(it => it.NDSXKDG_Tp);
                add.NDSXKDG_H = model.Sum(it => it.NDSXKDG_H);
                add.NDSXKDG_CS = model.Sum(it => it.NDSXKDG_CS);
                add.NDVietNamXuatSac = model.Sum(it => it.NDVietNamXuatSac);
                add.KNCViGCND = model.Sum(it => it.KNCViGCND);
                add.CanBoHoiCSG = model.Sum(it => it.CanBoHoiCSG);
                add.SangTaoNhaNong = model.Sum(it => it.SangTaoNhaNong);
                add.GuongDiemHinhTienTien = model.Sum(it => it.GuongDiemHinhTienTien);
                add.GuongDanVangKheo = model.Sum(it => it.GuongDanVangKheo);
                add.GuongHocTapLamTheoLoiBac = model.Sum(it => it.GuongHocTapLamTheoLoiBac);
                add.Tong = model.Sum(it => it.Tong);
                data.Add(add);
            }
            return data.OrderBy(it=>it.Tong).ToList();
        }
        #endregion Helper
    }
}
