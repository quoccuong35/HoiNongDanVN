using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using OfficeOpenXml.Style;
using System.Linq;
using System.Reflection.Metadata;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class BCTLHNamNewController : BaseController
    {
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 9;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        public BCTLHNamNewController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context)
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
        public IActionResult _Search(string? MaQuanHuyen, Guid? MaDiaBanHoiVien, int Nam)
        {
            return ExecuteSearch(() => {
                var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoiVien, Nam: Nam).OrderBy(it=>it.TenQuanHuyen).ThenBy(it => it.Cot8).ToList();
                return PartialView(model);
            });
        }
        public IActionResult ExportEdit(String? MaQuanHuyen, Guid? MaDiaBanHoiVien, int Nam)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\BaoCaoBienDongHoiVienNew.xlsx");
            var data = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoiVien, Nam: Nam);
            data = data.OrderBy(it => it.TenQuanHuyen).ThenBy(it => it.Cot8).ToList();

            byte[] filecontent;
            bool bold = false;
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(url), false))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets["Data"];
                int stt = 0;
                int startIndex = 9;
                workSheet.Cells["A4"].Value = "BÁO CÁO THỰC LỰC HỘI NĂM " + Nam.ToString();
                foreach (var item in data)
                {
                    stt++;
                    if (item.TenDiaBanHoatDong.ToLower().Contains("tổng"))
                    {
                        workSheet.Cells["A" + startIndex].Style.Font.Bold = true;
                        workSheet.Cells["A" + startIndex].Style.Font.Size = 16;
                        workSheet.Cells["B" + startIndex].Style.Font.Bold = true;
                        workSheet.Cells["B" + startIndex].Style.Font.Size = 16;
                        bold = true;
                    }
                    workSheet.Cells["A" + startIndex].Value = stt;
                    workSheet.Cells["B" + startIndex].Value = item.TenDiaBanHoatDong;
                    workSheet.Cells["C" + startIndex].Value = item.Cot1;
                    workSheet.Cells["D" + startIndex].Value = item.Cot2;
                    workSheet.Cells["E" + startIndex].Value = item.Cot3;
                    workSheet.Cells["F" + startIndex].Value = item.Cot4;
                    workSheet.Cells["G" + startIndex].Value = item.Cot5;
                    workSheet.Cells["H" + startIndex].Value = item.Cot6;
                    workSheet.Cells["I" + startIndex].Value = item.Cot7;

                    workSheet.Cells["J" + startIndex].Value = item.Cot8;
                    workSheet.Cells["K" + startIndex].Value = item.Cot9;
                    workSheet.Cells["L" + startIndex].Value = item.Cot10;
                    workSheet.Cells["M" + startIndex].Value = item.Cot11;
                    workSheet.Cells["N" + startIndex].Value = item.Cot12;
                    workSheet.Cells["O" + startIndex].Value = item.Cot13;

                    workSheet.Cells["P" + startIndex].Value = item.Cot14;
                    workSheet.Cells["Q" + startIndex].Value = item.Cot15;
                    workSheet.Cells["R" + startIndex].Value = item.Cot16;
                    workSheet.Cells["S" + startIndex].Value = item.Cot17;
                    workSheet.Cells["T" + startIndex].Value = item.Cot18;
                    workSheet.Cells["U" + startIndex].Value = item.Cot19;

                    workSheet.Cells["V" + startIndex].Value = item.Cot20;
                    workSheet.Cells["W" + startIndex].Value = item.Cot21;
                    workSheet.Cells["X" + startIndex].Value = item.Cot22;
                    workSheet.Cells["Y" + startIndex].Value = item.Cot23;
                    workSheet.Cells["Z" + startIndex].Value = item.Cot24;
                    workSheet.Cells["AA" + startIndex].Value = item.Cot25;
                    workSheet.Cells["AB" + startIndex].Value = item.Cot26;
                    workSheet.Cells["AC" + startIndex].Value = item.Cot27;
                    workSheet.Cells["AD" + startIndex].Value = item.Cot28;
                    workSheet.Cells["AE" + startIndex].Value = item.Cot29;
                    workSheet.Cells["AF" + startIndex].Value = item.Cot30;
                    workSheet.Cells["AG" + startIndex].Value = item.Cot31;
                    workSheet.Cells["AH" + startIndex].Value = item.TenDiaBanHoatDong;
                    if (bold)
                    {
                        workSheet.Cells["AM" + startIndex].Style.Font.Bold = true;
                        workSheet.Cells["AM" + startIndex].Style.Font.Size = 16;
                       
                    }
                    workSheet.Cells["AI" + startIndex].Value = item.Cot32;
                    workSheet.Cells["AJ" + startIndex].Value = item.Cot33;
                    workSheet.Cells["AK" + startIndex].Value = item.Cot34;
                    workSheet.Cells["AL" + startIndex].Value = item.Cot35;
                    workSheet.Cells["AM" + startIndex].Value = item.Cot36;
                    workSheet.Cells["AN" + startIndex].Value = item.Cot37;
                    workSheet.Cells["AO" + startIndex].Value = item.Cot38;
                    workSheet.Cells["AP" + startIndex].Value = item.Cot39;
                    workSheet.Cells["AQ" + startIndex].Value = item.Cot40;
                    workSheet.Cells["AR" + startIndex].Value = item.Cot41;
                    workSheet.Cells["AS" + startIndex].Value = item.Cot42;
                    workSheet.Cells["AT" + startIndex].Value = item.Cot43;
                    workSheet.Cells["AU" + startIndex].Value = item.Cot44;
                    workSheet.Cells["AV" + startIndex].Value = item.Cot45;
                    workSheet.Cells["AW" + startIndex].Value = item.Cot46;
                    workSheet.Cells["AX" + startIndex].Value = item.Cot47;
                    workSheet.Cells["AY" + startIndex].Value = item.Cot48;
                    workSheet.Cells["AZ" + startIndex].Value = item.Cot49;

                    workSheet.Cells["BA" + startIndex].Value = item.Cot50;
                    workSheet.Cells["BB" + startIndex].Value = item.Cot51;
                    workSheet.Cells["BC" + startIndex].Value = item.Cot52;
                    workSheet.Cells["BD" + startIndex].Value = item.Cot53;
                    workSheet.Cells["BE" + startIndex].Value = item.Cot54;
                    if (bold)
                    {
                        workSheet.Cells["A" + startIndex.ToString() + ":BF" + startIndex.ToString()].Style.Font.Bold = true;
                        workSheet.Cells["A" + startIndex.ToString() + ":BF" + startIndex.ToString()].Style.Font.Size = 12;
                    }
                    startIndex++;
                }
                using (ExcelRange r = workSheet.Cells["A9:BF" + startIndex.ToString()])
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
            string fileNameWithFormat = string.Format("{0}.xlsx", "DanhGiaToChucHoiHoiVien");

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
  
        private List<BCTLHNamNew> LoadData(string? MaQuanHuyen, Guid? MaDiaBanHoatDong, int Nam) {
            List<BCTLHNamNew> data = new List<BCTLHNamNew>();
            
            DateTime firstDate = new DateTime(Nam, 1, 1);
            DateTime endtDate = new DateTime(Nam, 12, 31);
            string id = "";
            if (MaDiaBanHoatDong != null)
                id = MaDiaBanHoatDong!.Value.ToString();
            var model = _context.BaoCaoThucLucHoiNams.FromSqlRaw("EXEC dbo.BaoCaoThuLucHoiNam @tungay = '" + firstDate.ToString("yyyy-MM-dd") + "',@denngay = '" + endtDate.ToString("yyyy-MM-dd") + "',@maquanhuyen ='" + MaQuanHuyen + "',@id ='" + id + "',@AccountId='"+AccountId().ToString()+"'").ToList();
           
            int index = 0;
            if (String.IsNullOrWhiteSpace(MaQuanHuyen))
            {
                List<Guid> pass = new List<Guid>();
                pass.Add(Guid.Parse("662ac072-fece-41e2-9a5e-e47c362d10cb"));
                pass.Add(Guid.Parse("bf7024f4-6bef-442a-9d6b-ce4538b1a084"));
                pass.Add(Guid.Parse("40a7400d-1981-45e8-b4a6-412af186dc5d"));

                var trucThuocThanhPho = model.Where(it => pass.Contains(it.MaDiabanHoatDong));
                foreach (var it in trucThuocThanhPho)
                {
                    index++;
                    BCTLHNamNew add = new BCTLHNamNew();
                    add.STT = index;
                    add.TenDiaBanHoatDong = it.TenDiaBanHoatDong;
                    add.TenQuanHuyen = it.TenDiaBanHoatDong;
                    add.Cot1 = it.Cot1;
                    add.Cot2 = it.Cot2;
                    add.Cot3 = it.Cot3;
                    add.Cot4 = it.Cot4;
                    add.Cot5 = it.Cot5;
                    add.Cot6 = it.Cot6;
                    add.Cot7 = it.Cot7;
                    add.Cot8 = it.Cot8;
                    add.Cot9 = it.Cot9;
                    add.Cot10 = it.Cot10;
                    add.Cot11 = it.Cot11;
                    add.Cot12 = it.Cot12;
                    add.Cot13 = it.Cot13;
                    add.Cot14 = it.Cot14;
                    add.Cot15 = it.Cot15;
                    add.Cot16 = it.Cot16;
                    add.Cot17 = it.Cot17;
                    add.Cot18 = it.Cot18;
                    add.Cot19 = it.Cot19;
                    add.Cot20 = it.Cot20;
                    add.Cot21 = it.Cot21;
                    add.Cot22 = it.Cot22;
                    add.Cot23 = it.Cot23;
                    add.Cot24 = it.Cot24;
                    add.Cot25 = it.Cot25;
                    add.Cot26 = it.Cot26;
                    add.Cot27 = it.Cot27;
                    add.Cot28 = it.Cot28;
                    add.Cot29 = it.Cot29;
                    add.Cot30 = it.Cot30;
                    add.Cot31 = it.Cot31;
                    add.Cot32 = it.Cot32;
                    add.Cot33 = it.Cot33;
                    add.Cot34 = it.Cot34;
                    add.Cot35 = it.Cot35;
                    add.Cot36 = it.Cot36;
                    add.Cot37 = it.Cot37;
                    add.Cot38 = it.Cot38;
                    add.Cot39 = it.Cot39;
                    add.Cot40 = it.Cot40;
                    add.Cot41 = it.Cot41;
                    add.Cot42 = it.Cot42;
                    add.Cot43 = it.Cot43;
                    add.Cot44 = it.Cot44;
                    add.Cot45 = it.Cot45;
                    add.Cot46 = it.Cot46;
                    add.Cot47 = it.Cot47;
                    add.Cot48 = it.Cot48;
                    add.Cot49 = it.Cot49;
                    add.Cot50 = it.Cot50;
                    add.Cot51 = it.Cot51;
                    add.Cot52 = it.Cot52;
                    add.Cot53 = it.Cot53;
                    add.Cot54 = it.Cot54;
                    data.Add(add);
                }

                List <String> quanHuyens = model.Where(it=> !pass.Contains(it.MaDiabanHoatDong)).Select(it => it.TenQuanHuyen) .Distinct().ToList();
                foreach (var item in quanHuyens)
                {
                    index++;
                    BCTLHNamNew add = new BCTLHNamNew();
                    add.STT = index;
                    add.TenDiaBanHoatDong = item;
                    add.TenQuanHuyen = item;
                    var tam = model.Where(it => it.TenQuanHuyen == item && !pass.Contains(it.MaDiabanHoatDong));
                    add.Cot1 = tam.Sum(it => it.Cot1);
                    add.Cot2 = tam.Sum(it => it.Cot2);
                    add.Cot3 = tam.Sum(it => it.Cot3);
                    add.Cot4 = tam.Sum(it => it.Cot4);
                    add.Cot5 = tam.Sum(it => it.Cot5);
                    add.Cot6 = tam.Sum(it => it.Cot6);
                    add.Cot7 = tam.Sum(it => it.Cot7);
                    add.Cot8 = tam.Sum(it => it.Cot8);
                    add.Cot9 = tam.Sum(it => it.Cot9);
                    add.Cot10 = tam.Sum(it => it.Cot10);
                    add.Cot11 = tam.Sum(it => it.Cot11);
                    add.Cot12 = tam.Sum(it => it.Cot12);
                    add.Cot13 = tam.Sum(it => it.Cot13);
                    add.Cot14 = tam.Sum(it => it.Cot14);
                    add.Cot15 = tam.Sum(it => it.Cot15);
                    add.Cot16 = tam.Sum(it => it.Cot16);
                    add.Cot17 = tam.Sum(it => it.Cot17);
                    add.Cot18 = tam.Sum(it => it.Cot18);
                    add.Cot19 = tam.Sum(it => it.Cot19);
                    add.Cot20 = tam.Sum(it => it.Cot20);
                    add.Cot21 = tam.Sum(it => it.Cot21);
                    add.Cot22 = tam.Sum(it => it.Cot22);
                    add.Cot23 = tam.Sum(it => it.Cot23);
                    add.Cot24 = tam.Sum(it => it.Cot24);
                    add.Cot25 = tam.Sum(it => it.Cot25);
                    add.Cot26 = tam.Sum(it => it.Cot26);
                    add.Cot27 = tam.Sum(it => it.Cot27);
                    add.Cot28 = tam.Sum(it => it.Cot28);
                    add.Cot29 = tam.Sum(it => it.Cot29);
                    add.Cot30 = tam.Sum(it => it.Cot30);
                    add.Cot31 = tam.Sum(it => it.Cot31);
                    add.Cot32 = tam.Sum(it => it.Cot32);
                    add.Cot33 = tam.Sum(it => it.Cot33);
                    add.Cot34 = tam.Sum(it => it.Cot34);
                    add.Cot35 = tam.Sum(it => it.Cot35);
                    add.Cot36 = tam.Sum(it => it.Cot36);
                    add.Cot37 = tam.Sum(it => it.Cot37);
                    add.Cot38 = tam.Sum(it => it.Cot38);
                    add.Cot39 = tam.Sum(it => it.Cot39);
                    add.Cot40 = tam.Sum(it => it.Cot40);
                    add.Cot41 = tam.Sum(it => it.Cot41);
                    add.Cot42 = tam.Sum(it => it.Cot42);
                    add.Cot43 = tam.Sum(it => it.Cot43);
                    add.Cot44 = tam.Sum(it => it.Cot44);
                    add.Cot45 = tam.Sum(it => it.Cot45);
                    add.Cot46 = tam.Sum(it => it.Cot46);
                    add.Cot47 = tam.Sum(it => it.Cot47);
                    add.Cot48 = tam.Sum(it => it.Cot48);
                    add.Cot49 = tam.Sum(it => it.Cot49);
                    add.Cot50 = tam.Sum(it => it.Cot50);
                    add.Cot51 = tam.Sum(it => it.Cot51);
                    add.Cot52 = tam.Sum(it => it.Cot52);
                    add.Cot53 = tam.Sum(it => it.Cot53);
                    add.Cot54 = tam.Sum(it => it.Cot54);
                    data.Add(add);
                }
            }
            else if (!String.IsNullOrWhiteSpace(MaQuanHuyen) || MaDiaBanHoatDong != null)
            {
                data = model.OrderBy(it => it.TenQuanHuyen).ThenBy(it => it.Cot1).Select((p, index) => new BCTLHNamNew
                {
                    STT = index + 1,
                    TenDiaBanHoatDong = p.TenDiaBanHoatDong,
                    TenQuanHuyen = p.TenQuanHuyen,
                    Cot1 = p.Cot1,
                    Cot2 = p.Cot2,
                    Cot3 = p.Cot3,
                    Cot4 = p.Cot4,
                    Cot5 = p.Cot5,
                    Cot6 = p.Cot6,
                    Cot7 = p.Cot7,
                    Cot8 = p.Cot8,
                    Cot9 = p.Cot9,
                    Cot10 = p.Cot10,
                    Cot11 = p.Cot11,
                    Cot12 = p.Cot12,
                    Cot13 = p.Cot13,
                    Cot14 = p.Cot14,
                    Cot15 = p.Cot15,
                    Cot16 = p.Cot16,
                    Cot17 = p.Cot17,
                    Cot18 = p.Cot18,
                    Cot19 = p.Cot19,
                    Cot20 = p.Cot20,
                    Cot21 = p.Cot21,
                    Cot22 = p.Cot22,
                    Cot23 = p.Cot23,
                    Cot24 = p.Cot24,
                    Cot25 = p.Cot25,
                    Cot26 = p.Cot26,
                    Cot27 = p.Cot27,
                    Cot28 = p.Cot28,
                    Cot29 = p.Cot29,
                    Cot30 = p.Cot30,
                    Cot31 = p.Cot31,
                    Cot32 = p.Cot32,
                    Cot33 = p.Cot33,
                    Cot34 = p.Cot34,
                    Cot35 = p.Cot35,
                    Cot36 = p.Cot36,
                    Cot37 = p.Cot37,
                    Cot38 = p.Cot38,
                    Cot39 = p.Cot39,
                    Cot40 = p.Cot40,
                    Cot41 = p.Cot41,
                    Cot42 = p.Cot42,
                    Cot43 = p.Cot43,
                    Cot44 = p.Cot44,
                    Cot45 = p.Cot45,
                    Cot46 = p.Cot46,
                    Cot47 = p.Cot47,
                    Cot48 = p.Cot48,
                    Cot49 = p.Cot49,
                    Cot50 = p.Cot50,
                    Cot51 = p.Cot51,
                    Cot52 = p.Cot52,
                    Cot53 = p.Cot53,
                    Cot54 = p.Cot54
                }).ToList();

            }

            // Add tổng
            if (MaDiaBanHoatDong == null)
            {
                BCTLHNamNew tongCong = new BCTLHNamNew();
                tongCong.STT = index;
                tongCong.TenDiaBanHoatDong = "Tổng ";
                tongCong.TenQuanHuyen = "Tổng ";
                tongCong.Cot1 = data.Sum(it => it.Cot1);
                tongCong.Cot2 = data.Sum(it => it.Cot2);
                tongCong.Cot3 = data.Sum(it => it.Cot3);
                tongCong.Cot4 = data.Sum(it => it.Cot4);
                tongCong.Cot5 = data.Sum(it => it.Cot5);
                tongCong.Cot6 = data.Sum(it => it.Cot6);
                tongCong.Cot7 = data.Sum(it => it.Cot7);
                tongCong.Cot8 = data.Sum(it => it.Cot8);
                tongCong.Cot9 = data.Sum(it => it.Cot9);
                tongCong.Cot10 = data.Sum(it => it.Cot10);
                tongCong.Cot11 = data.Sum(it => it.Cot11);
                tongCong.Cot12 = data.Sum(it => it.Cot12);
                tongCong.Cot13 = data.Sum(it => it.Cot13);
                tongCong.Cot14 = data.Sum(it => it.Cot14);
                tongCong.Cot15 = data.Sum(it => it.Cot15);
                tongCong.Cot16 = data.Sum(it => it.Cot16);
                tongCong.Cot17 = data.Sum(it => it.Cot17);
                tongCong.Cot18 = data.Sum(it => it.Cot18);
                tongCong.Cot19 = data.Sum(it => it.Cot19);
                tongCong.Cot20 = data.Sum(it => it.Cot20);
                tongCong.Cot21 = data.Sum(it => it.Cot21);
                tongCong.Cot22 = data.Sum(it => it.Cot22);
                tongCong.Cot23 = data.Sum(it => it.Cot23);
                tongCong.Cot24 = data.Sum(it => it.Cot24);
                tongCong.Cot25 = data.Sum(it => it.Cot25);
                tongCong.Cot26 = data.Sum(it => it.Cot26);
                tongCong.Cot27 = data.Sum(it => it.Cot27);
                tongCong.Cot28 = data.Sum(it => it.Cot28);
                tongCong.Cot29 = data.Sum(it => it.Cot29);
                tongCong.Cot30 = data.Sum(it => it.Cot30);
                tongCong.Cot31 = data.Sum(it => it.Cot31);
                tongCong.Cot32 = data.Sum(it => it.Cot32);
                tongCong.Cot33 = data.Sum(it => it.Cot33);
                tongCong.Cot34 = data.Sum(it => it.Cot34);
                tongCong.Cot35 = data.Sum(it => it.Cot35);
                tongCong.Cot36 = data.Sum(it => it.Cot36);
                tongCong.Cot37 = data.Sum(it => it.Cot37);
                tongCong.Cot38 = data.Sum(it => it.Cot38);
                tongCong.Cot39 = data.Sum(it => it.Cot39);
                tongCong.Cot40 = data.Sum(it => it.Cot40);
                tongCong.Cot41 = data.Sum(it => it.Cot41);
                tongCong.Cot42 = data.Sum(it => it.Cot42);
                tongCong.Cot43 = data.Sum(it => it.Cot43);
                tongCong.Cot44 = data.Sum(it => it.Cot44);
                tongCong.Cot45 = data.Sum(it => it.Cot45);
                tongCong.Cot46 = data.Sum(it => it.Cot46);
                tongCong.Cot47 = data.Sum(it => it.Cot47);
                tongCong.Cot48 = data.Sum(it => it.Cot48);
                tongCong.Cot49 = data.Sum(it => it.Cot49);
                tongCong.Cot50 = data.Sum(it => it.Cot50);
                tongCong.Cot51 = data.Sum(it => it.Cot51);
                tongCong.Cot52 = data.Sum(it => it.Cot52);
                tongCong.Cot53 = data.Sum(it => it.Cot53);
                tongCong.Cot54 = data.Sum(it => it.Cot54);
                data.Add(tongCong);
            }
            return data;
        }
        #endregion Helper
    }
}
