using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.Style;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class BCTLHNewController : BaseController
    {
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 7;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        public BCTLHNewController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context)
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
        [HoiNongDanAuthorization]
        public IActionResult _Search(string? MaQuanHuyen, Guid? MaDiaBanHoiVien,DateTime? DenNgay)
        {
            return ExecuteSearch(() => {
                var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoiVien,DenNgay:DenNgay);
                return PartialView(model);
            });
        }
        public IActionResult ExportEdit(String? MaQuanHuyen, Guid? MaDiaBanHoatDong, DateTime? DenNgay)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\ThongKeBienToChucHoiHoiVien.xlsx");
            var data = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoatDong, DenNgay: DenNgay);
            byte[] filecontent;
            bool bold = false;
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(url), false))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets["Data"];
                int sttSub = 0;
                int stt = 0;
                int startIndex = 7;
               
                foreach (var item in data)
                {
                    stt++;
                    workSheet.Cells["A" + startIndex].Value = stt;
                    workSheet.Cells["B" + startIndex].Value = item.Cot2;

                    workSheet.Cells["C" + startIndex].Value = item.Cot3;
                    workSheet.Cells["D" + startIndex].Value = item.Cot4;
                    workSheet.Cells["E" + startIndex].Value = item.Cot5;
                    workSheet.Cells["F" + startIndex].Value = item.Cot6;
                    workSheet.Cells["G" + startIndex].Value = item.Cot7;
                    workSheet.Cells["H" + startIndex].Value = item.Cot8;
                    workSheet.Cells["I" + startIndex].Value = item.Cot9;

                    workSheet.Cells["J" + startIndex].Value = item.Cot10;
                    workSheet.Cells["K" + startIndex].Value = item.Cot11;
                    workSheet.Cells["L" + startIndex].Value = item.Cot12;
                    workSheet.Cells["M" + startIndex].Value = item.Cot13;
                    workSheet.Cells["N" + startIndex].Value = item.Cot14;
                    workSheet.Cells["O" + startIndex].Value = item.Cot15;

                    workSheet.Cells["P" + startIndex].Value = item.Cot16;
                    workSheet.Cells["Q" + startIndex].Value = item.Cot17;
                    workSheet.Cells["R" + startIndex].Value = item.Cot18;
                    workSheet.Cells["S" + startIndex].Value = item.Cot19;
                    workSheet.Cells["T" + startIndex].Value = item.Cot20;
                    workSheet.Cells["U" + startIndex].Value = item.Cot21;

                    workSheet.Cells["V" + startIndex].Value = item.Cot22;
                    workSheet.Cells["W" + startIndex].Value = item.Cot23;
                    workSheet.Cells["X" + startIndex].Value = item.Cot24;
                    workSheet.Cells["Y" + startIndex].Value = item.Cot25;
                    workSheet.Cells["Z" + startIndex].Value = item.Cot26;
                    if (item == data.Last())
                    {
                        workSheet.Cells["A" + startIndex.ToString() + ":Z" + startIndex.ToString()].Style.Font.Bold = true;
                        workSheet.Cells["A" + startIndex.ToString() + ":Z" + startIndex.ToString()].Style.Font.Size = 14;
                    }
                    startIndex++;
                }
                using (ExcelRange r = workSheet.Cells["A7:Z" + startIndex.ToString()])
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
            string fileNameWithFormat = string.Format("{0}.xlsx", "ThongKeBienToChucHoiHoiVien");

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
        private List<BCTLHNew> LoadData(string? MaQuanHuyen, Guid? MaDiaBanHoatDong, DateTime? DenNgay) {
            DateTime dateNow = DateTime.Now;
            DenNgay = DenNgay == null ? dateNow : DenNgay;
            DateTime firstDay = new DateTime(DenNgay!.Value.Year, 1, 1);
            if (String.IsNullOrWhiteSpace(MaQuanHuyen))
                MaQuanHuyen = "";
            string id = "";
            if (MaDiaBanHoatDong != null)
                id = MaDiaBanHoatDong.ToString();
            var model = _context.ThongKeBien_ToChucHoi_HoiViens.FromSqlRaw("EXEC  dbo.BaoCaoThuLucHoiNew @tungay = '" + firstDay.ToString("yyyy-MM-dd") + "',@denngay = '" + DenNgay.Value.ToString("yyyy-MM-dd") + "',@maquanhuyen ='" + MaQuanHuyen+"',@id ='"+id+ "',@AccountId='"+AccountId().ToString()+"'").ToList();

            List<BCTLHNew> data = new List<BCTLHNew>();
            if (String.IsNullOrWhiteSpace(MaQuanHuyen) && MaDiaBanHoatDong == null)
            {
                List<Guid> pass = new List<Guid>();
                pass.Add(Guid.Parse("662ac072-fece-41e2-9a5e-e47c362d10cb"));
                pass.Add(Guid.Parse("bf7024f4-6bef-442a-9d6b-ce4538b1a084"));
                pass.Add(Guid.Parse("40a7400d-1981-45e8-b4a6-412af186dc5d"));

                data = model.Where(it => !pass.Contains(it.MaDiabanHoatDong)).GroupBy(it => new { it.TenQuanHuyen }).Select(p => new BCTLHNew
                {
                    Cot2 = p.Key.TenQuanHuyen,
                    Cot3 = p.Sum(it => it.Cot3),
                    Cot4 = p.Sum(it => it.Cot4),
                    Cot5 = p.Sum(it => it.Cot5),
                    Cot6 = p.Sum(it => it.Cot6),
                    Cot7 = p.Sum(it => it.Cot7),
                    Cot8 = p.Sum(it => it.Cot8),
                    Cot9 = p.Sum(it => it.Cot9),
                    Cot10 = p.Sum(it => it.Cot10),
                    Cot11 = p.Sum(it => it.Cot11),
                    Cot12 = p.Sum(it => it.Cot12),
                    Cot13 = p.Sum(it => it.Cot13),
                    Cot14 = p.Sum(it => it.Cot14),
                    Cot15 = p.Sum(it => it.Cot15),
                    Cot16 = p.Sum(it => it.Cot16),
                    Cot17 = p.Sum(it => it.Cot17),
                    Cot18 = p.Sum(it => it.Cot18),
                    Cot19 = p.Sum(it => it.Cot19),
                    Cot20 = p.Sum(it => it.Cot20),
                    Cot21 = p.Sum(it => it.Cot21),
                    Cot22 = p.Sum(it => it.Cot22),
                    Cot23 = p.Sum(it => it.Cot23),
                    Cot24 = p.Sum(it => it.Cot24),
                    Cot25 = p.Sum(it => it.Cot25),
                    Cot26 = p.Sum(it => it.Cot26),
                    

                }).ToList();
                var dataTam = model.Where(it => pass.Contains(it.MaDiabanHoatDong)).GroupBy(it => new { it.TenDiaBanHoatDong }).Select(p => new BCTLHNew
                {
                    Cot2 = p.Key.TenDiaBanHoatDong,
                    Cot3 = p.Sum(it => it.Cot3),
                    Cot4 = p.Sum(it => it.Cot4),
                    Cot5 = p.Sum(it => it.Cot5),
                    Cot6 = p.Sum(it => it.Cot6),
                    Cot7 = p.Sum(it => it.Cot7),
                    Cot8 = p.Sum(it => it.Cot8),
                    Cot9 = p.Sum(it => it.Cot9),
                    Cot10 = p.Sum(it => it.Cot10),
                    Cot11 = p.Sum(it => it.Cot11),
                    Cot12 = p.Sum(it => it.Cot12),
                    Cot13 = p.Sum(it => it.Cot13),
                    Cot14 = p.Sum(it => it.Cot14),
                    Cot15 = p.Sum(it => it.Cot15),
                    Cot16 = p.Sum(it => it.Cot16),
                    Cot17 = p.Sum(it => it.Cot17),
                    Cot18 = p.Sum(it => it.Cot18),
                    Cot19 = p.Sum(it => it.Cot19),
                    Cot20 = p.Sum(it => it.Cot20),
                    Cot21 = p.Sum(it => it.Cot21),
                    Cot22 = p.Sum(it => it.Cot22),
                    Cot23 = p.Sum(it => it.Cot23),
                    Cot24 = p.Sum(it => it.Cot24),
                    Cot25 = p.Sum(it => it.Cot25),
                    Cot26 = p.Sum(it => it.Cot26),
                    

                }).ToList();
                data.AddRange(dataTam);
            }
            else if (!String.IsNullOrWhiteSpace(MaQuanHuyen) && MaDiaBanHoatDong == null) {
                data = model.GroupBy(it => new { it.TenDiaBanHoatDong }).Select(p => new BCTLHNew
                {
                    Cot2 = p.Key.TenDiaBanHoatDong,
                    Cot3 = p.Sum(it => it.Cot3),
                    Cot4 = p.Sum(it => it.Cot4),
                    Cot5 = p.Sum(it => it.Cot5),
                    Cot6 = p.Sum(it => it.Cot6),
                    Cot7 = p.Sum(it => it.Cot7),
                    Cot8 = p.Sum(it => it.Cot8),
                    Cot9 = p.Sum(it => it.Cot9),
                    Cot10 = p.Sum(it => it.Cot10),
                    Cot11 = p.Sum(it => it.Cot11),
                    Cot12 = p.Sum(it => it.Cot12),
                    Cot13 = p.Sum(it => it.Cot13),
                    Cot14 = p.Sum(it => it.Cot14),
                    Cot15 = p.Sum(it => it.Cot15),
                    Cot16 = p.Sum(it => it.Cot16),
                    Cot17 = p.Sum(it => it.Cot17),
                    Cot18 = p.Sum(it => it.Cot18),
                    Cot19 = p.Sum(it => it.Cot19),
                    Cot20 = p.Sum(it => it.Cot20),
                    Cot21 = p.Sum(it => it.Cot21),
                    Cot22 = p.Sum(it => it.Cot22),
                    Cot23 = p.Sum(it => it.Cot23),
                    Cot24 = p.Sum(it => it.Cot24),
                    Cot25 = p.Sum(it => it.Cot25),
                    Cot26 = p.Sum(it => it.Cot26),
                }).ToList();
            }
                BCTLHNew tong = new BCTLHNew();

            tong.Cot2 = "Tổng cộng";
            tong.Cot3 = data.Sum(it => it.Cot3);
            tong.Cot4 = data.Sum(it => it.Cot4);
            tong.Cot5 = data.Sum(it => it.Cot5);
            tong.Cot6 = data.Sum(it => it.Cot6);
            tong.Cot7 = data.Sum(it => it.Cot7);
            tong.Cot8 = data.Sum(it => it.Cot8);
            tong.Cot9 = data.Sum(it => it.Cot9);
            tong.Cot10 = data.Sum(it => it.Cot10);
            tong.Cot11 = data.Sum(it => it.Cot11);
            tong.Cot12 = data.Sum(it => it.Cot12);
            tong.Cot13 = data.Sum(it => it.Cot13);
            tong.Cot14 = data.Sum(it => it.Cot14);
            tong.Cot15 = data.Sum(it => it.Cot15);
            tong.Cot16 = data.Sum(it => it.Cot16);
            tong.Cot17 = data.Sum(it => it.Cot17);
            tong.Cot18 = data.Sum(it => it.Cot18);
            tong.Cot19 = data.Sum(it => it.Cot19);
            tong.Cot20 = data.Sum(it => it.Cot20);
            tong.Cot21 = data.Sum(it => it.Cot21);
            tong.Cot22 = data.Sum(it => it.Cot22);
            tong.Cot23 = data.Sum(it => it.Cot23);
            tong.Cot24 = data.Sum(it => it.Cot24);
            tong.Cot25 = data.Sum(it => it.Cot25);
            tong.Cot26 = data.Sum(it => it.Cot26);
            
            data.Add(tong);
            int i = 1;
            data.OrderBy(it => it.Cot3).ToList().ForEach(it => {
                it.Cot1 = i;
                i++;
            });
            return data.OrderBy(it => it.Cot1).ToList();
        }
        #endregion Helper
    }
}
