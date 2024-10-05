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
                var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoiVien, Nam: Nam);
                model = model.OrderBy(it => it.TenQuanHuyen).ThenBy(it => it.Cot8).ToList();
                return PartialView(model);
            });
        }
        public IActionResult ExportEdit(String? MaQuanHuyen, Guid? MaDiaBanHoatDong, int Nam)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\BaoCaoBienDongHoiVienNew.xlsx");
            var data = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoatDong, Nam: Nam);
            data = data.OrderBy(it => it.TenQuanHuyen).ThenBy(it => it.Cot8).ToList();

            byte[] filecontent;
            bool bold = false;
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(url), false))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets["Data"];
                int sttSub = 0;
                int stt = 1;
                int startIndex = 9;
                workSheet.Cells["A4"].Value = "BÁO CÁO THỰC LỰC HỘI NĂM " + Nam.ToString();
                foreach (var item in data)
                {
                    if (item.Cot2 != item.TenQuanHuyen)
                    {

                        workSheet.Cells["A" + startIndex].Value = stt;
                        workSheet.Cells["B" + startIndex].Value = item.Cot2;
                        stt++;
                        bold = false;
                    }
                    else
                    {
                        sttSub++;
                        stt = 1;
                        workSheet.Cells["A" + startIndex].Style.Font.Bold = true;
                        workSheet.Cells["A" + startIndex].Style.Font.Size = 16;
                        workSheet.Cells["A" + startIndex].Value = sttSub;

                        workSheet.Cells["B" + startIndex].Style.Font.Bold = true;
                        workSheet.Cells["B" + startIndex].Style.Font.Size = 16;
                        workSheet.Cells["B" + startIndex].Value = "Tổng" + item.Cot2;
                        bold = true;
                    }

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
                    workSheet.Cells["AA" + startIndex].Value = item.Cot27;
                    workSheet.Cells["AB" + startIndex].Value = item.Cot28;
                    workSheet.Cells["AC" + startIndex].Value = item.Cot29;
                    workSheet.Cells["AD" + startIndex].Value = item.Cot30;
                    workSheet.Cells["AE" + startIndex].Value = item.Cot31;
                    workSheet.Cells["AF" + startIndex].Value = item.Cot32;
                    workSheet.Cells["AG" + startIndex].Value = item.Cot33;
                    workSheet.Cells["AH" + startIndex].Value = item.Cot34;
                    workSheet.Cells["AI" + startIndex].Value = item.Cot35;
                    workSheet.Cells["AJ" + startIndex].Value = item.Cot36;
                    workSheet.Cells["AK" + startIndex].Value = item.Cot37;
                    workSheet.Cells["AL" + startIndex].Value = item.Cot38;

                    if (bold)
                    {
                        workSheet.Cells["AM" + startIndex].Style.Font.Bold = true;
                        workSheet.Cells["AM" + startIndex].Style.Font.Size = 16;
                        workSheet.Cells["AM" + startIndex].Value = item.Cot39;
                    }
                    else
                    {
                        workSheet.Cells["AM" + startIndex].Value = item.Cot39;
                    }
                   
                    workSheet.Cells["AN" + startIndex].Value = item.Cot40;
                    workSheet.Cells["AO" + startIndex].Value = item.Cot41;
                    workSheet.Cells["AP" + startIndex].Value = item.Cot42;
                    workSheet.Cells["AQ" + startIndex].Value = item.Cot43;
                    workSheet.Cells["AR" + startIndex].Value = item.Cot44;
                    workSheet.Cells["AS" + startIndex].Value = item.Cot45;
                    workSheet.Cells["AT" + startIndex].Value = item.Cot46;
                    workSheet.Cells["AU" + startIndex].Value = item.Cot47;
                    workSheet.Cells["AV" + startIndex].Value = item.Cot48;
                    workSheet.Cells["AW" + startIndex].Value = item.Cot49;
                    workSheet.Cells["AX" + startIndex].Value = item.Cot50;
                    workSheet.Cells["AY" + startIndex].Value = item.Cot51;
                    workSheet.Cells["AZ" + startIndex].Value = item.Cot52;

                    workSheet.Cells["BA" + startIndex].Value = item.Cot53;
                    workSheet.Cells["BB" + startIndex].Value = item.Cot54;
                    workSheet.Cells["BC" + startIndex].Value = item.Cot55;
                    workSheet.Cells["BD" + startIndex].Value = item.Cot56;
                    workSheet.Cells["BE" + startIndex].Value = item.Cot57;
                    workSheet.Cells["BF" + startIndex].Value = item.Cot58;
                    workSheet.Cells["BG" + startIndex].Value = item.Cot59;
                    workSheet.Cells["BH" + startIndex].Value = item.Cot60;
                    workSheet.Cells["BI" + startIndex].Value = item.Cot61;
                    workSheet.Cells["BJ" + startIndex].Value = item.Cot62;
                    if (bold)
                    {
                        workSheet.Cells["A" + startIndex.ToString()+":BJ"+startIndex.ToString()].Style.Font.Bold = true;
                        workSheet.Cells["A" + startIndex.ToString() + ":BJ" + startIndex.ToString()].Style.Font.Size = 12;
                    }
                    startIndex++;
                }
                using (ExcelRange r = workSheet.Cells["A9:BK" + startIndex.ToString()])
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
                id = MaDiaBanHoatDong!.ToString();
            var model = _context.BaoCaoThucLucHoiNams.FromSqlRaw("EXEC  dbo.BaoCaoThuLucHoiNam @tungay = '" + firstDate.ToString("yyyy-MM-dd") + "',@denngay = '" + endtDate.ToString("yyyy-MM-dd") + "',@maquanhuyen ='" + MaQuanHuyen + "',@id ='" + id + "',@AccountId='"+AccountId().ToString()+"'").ToList();
            data = model.Select((p, index) => new BCTLHNamNew
            {
                Cot1 = index + 1,
                Cot2 = p.TenDiaBanHoatDong,
                Cot39 = p.TenDiaBanHoatDong,
                TenQuanHuyen = p.TenQuanHuyen,
                Cot3 = p.Cot1,
                Cot4 = p.Cot2,
                Cot5 = p.Cot3,
                Cot6 = p.Cot4,
                Cot7 = p.Cot5,
                Cot8 = p.Cot6,
                Cot9 = p.Cot7,
                Cot10 = p.Cot8,
                Cot11 = p.Cot9,
                Cot12 = p.Cot10,
                Cot13 = p.Cot11,
                Cot14 = p.Cot12,
                Cot15 = p.Cot13,
                Cot16 = p.Cot14,
                Cot17 = p.Cot15,
                Cot18 = p.Cot16,
                Cot19 = p.Cot17,
                Cot20 = p.Cot18,
                Cot21 = p.Cot19,
                Cot22 = p.Cot20,
                Cot23 = p.Cot21,
                Cot24 = p.Cot22,
                Cot25 = p.Cot23,
                Cot26 = p.Cot24,
                Cot27 = p.Cot25,
                Cot28 = p.Cot26,
                Cot29 = p.Cot27,
                Cot30 = p.Cot28,
                Cot31 = p.Cot29,
                Cot32 = p.Cot30,
                Cot33 = p.Cot31,
                Cot34 = p.Cot32,
                Cot35 = p.Cot33,
                Cot36 = p.Cot34,
                Cot37 = p.Cot35, 
                Cot38 = p.Cot36,
                Cot40 = p.Cot37,
                Cot41 = p.Cot38,
                Cot42 = p.Cot39,
                Cot43 = p.Cot40,
                Cot44 = p.Cot41,
                Cot45 = p.Cot42,
                Cot46 = p.Cot43,
                Cot47 = p.Cot44,
                Cot48 = p.Cot45,
                Cot49 = p.Cot46,
                Cot50 = p.Cot47,
                Cot51 = p.Cot48,
                Cot52 = p.Cot49,
                Cot53 = p.Cot50,
                Cot54 = p.Cot51,
                Cot55 = p.Cot52,
                Cot56 = p.Cot53,
                Cot57 = p.Cot54,
                Cot58 = p.Cot55,
                Cot59 = p.Cot56,
                Cot60 = p.Cot57,
                Cot61 = p.Cot58,
                Cot62 = p.Cot59,

            }).ToList();
            List<String> quanHuyens = data.Select(it => it.TenQuanHuyen).Distinct().ToList();
            int index = 0;
            foreach (var item in quanHuyens)
            {
                index++;
                BCTLHNamNew add = new BCTLHNamNew();
                add.Cot1 = index;
                add.Cot2 = add.TenQuanHuyen = item;
                add.Cot39 = "Tổng " + item;
                var tam =  data.Where(it => it.TenQuanHuyen == item);
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
                add.Cot55 = tam.Sum(it => it.Cot55);
                add.Cot56 = tam.Sum(it => it.Cot56);
                add.Cot57 = tam.Sum(it => it.Cot57);
                add.Cot58 = tam.Sum(it => it.Cot58);
                add.Cot59 = tam.Sum(it => it.Cot59);
                add.Cot60 = tam.Sum(it => it.Cot60);
                add.Cot61 = tam.Sum(it => it.Cot61);
                add.Cot62 = tam.Sum(it => it.Cot62);
                data.Add(add);
            }
            return data;
        }
        #endregion Helper
    }
}
