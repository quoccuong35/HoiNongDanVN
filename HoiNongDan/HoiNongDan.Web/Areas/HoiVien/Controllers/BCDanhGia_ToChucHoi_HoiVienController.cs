using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class BCDanhGia_ToChucHoi_HoiVienController : BaseController
    {
        const string controllerCode = ConstExcelController.HoiVien;
       
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        private string url = @"upload\filemau\DanhGiaToChucHoiHoiVien.xlsx";
        public BCDanhGia_ToChucHoi_HoiVienController(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context)
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
        public IActionResult _Search(string? MaQuanHuyen, Guid? MaDiaBanHoiVien,int Nam)
        {
            return ExecuteSearch(() => {
                var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoiVien,Nam:Nam).OrderBy(it => it.TenQuanHuyen).ThenByDescending(it => it.HV_Tong).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        public IActionResult ExportEdit(String? MaQuanHuyen, Guid? MaDiaBanHoatDong, int Nam)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var urlMau = Path.Combine(wwwRootPath, url);
            var data = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoatDong,Nam: Nam);
            data = data.OrderBy(it => it.TenQuanHuyen).ThenByDescending(it => it.HV_Tong).ToList();
           
            byte[] filecontent;
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(urlMau), false)) 
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets["Data"];
                int sttSub = 0;
                int stt = 1;
                int startIndex = 8;
                workSheet.Cells["A3"].Value = "ĐÁNH GIÁ TỔ CHỨC HỘI, HỘI VIÊN NĂM " + Nam.ToString();
                foreach (var item in data)
                {
                    if (item.DonVi != item.TenQuanHuyen)
                    {
                      
                        workSheet.Cells["A" + startIndex].Value = stt;
                        workSheet.Cells["B" + startIndex].Value = item.DonVi;
                        stt ++;
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
                        workSheet.Cells["B" + startIndex].Value = item.DonVi;
                    }
                   
                    workSheet.Cells["C" + startIndex].Value = item.HV_Tong;
                    workSheet.Cells["D" + startIndex].Value = item.HV_HTXSNV;
                    workSheet.Cells["E" + startIndex].Value = item.HV_HTTNV;
                    workSheet.Cells["F" + startIndex].Value = item.HV_HTNV;
                    workSheet.Cells["G" + startIndex].Value = item.HV_KHTNV;
                    workSheet.Cells["H" + startIndex].Value = item.HV_KPhanLoai;
                    workSheet.Cells["I" + startIndex].Value = item.HV_ChuaDuDKPL;

                    workSheet.Cells["J" + startIndex].Value = item.CS_Tong;
                    workSheet.Cells["K" + startIndex].Value = item.CS_HTXSNV;
                    workSheet.Cells["L" + startIndex].Value = item.CS_HTTNV;
                    workSheet.Cells["M" + startIndex].Value = item.CS_HTNV;
                    workSheet.Cells["N" + startIndex].Value = item.CS_KHTNV;
                    workSheet.Cells["O" + startIndex].Value = item.CS_KPhanLoai;

                    workSheet.Cells["P" + startIndex].Value = item.DC_Tong;
                    workSheet.Cells["Q" + startIndex].Value = item.DC_HTXSNV;
                    workSheet.Cells["R" + startIndex].Value = item.DC_HTTNV;
                    workSheet.Cells["S" + startIndex].Value = item.DC_HTNV;
                    workSheet.Cells["T" + startIndex].Value = item.DC_KHTNV;
                    workSheet.Cells["U" + startIndex].Value = item.DC_KPhanLoai;

                    workSheet.Cells["V" + startIndex].Value = item.NN_Tong;
                    workSheet.Cells["W" + startIndex].Value = item.NN_HTXSNV;
                    workSheet.Cells["X" + startIndex].Value = item.NN_HTTNV;
                    workSheet.Cells["Y" + startIndex].Value = item.NN_HTNV;
                    workSheet.Cells["Z" + startIndex].Value = item.NN_KHTNV;
                    workSheet.Cells["AA" + startIndex].Value = item.NN_KPhanLoai;
                    startIndex++;
                }
                using (ExcelRange r = workSheet.Cells["A8:AA"+startIndex.ToString()])
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
      
        #region Helper
        private void CreateViewBagSearch(string? maQuanHuyen = null, Guid? maDiaBan = null)
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId(), value: maDiaBan);

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId(), value: maQuanHuyen);

        }
        private List<BCDanhGiaToChucHoiHoiVienVM> LoadData(string? MaQuanHuyen, Guid? MaDiaBanHoatDong,int Nam)
        {
            try
            {
                List<Guid> pass = new List<Guid>();
                pass.Add(Guid.Parse("662ac072-fece-41e2-9a5e-e47c362d10cb"));
                pass.Add(Guid.Parse("bf7024f4-6bef-442a-9d6b-ce4538b1a084"));
                pass.Add(Guid.Parse("40a7400d-1981-45e8-b4a6-412af186dc5d"));

                var model = _context.DiaBanHoatDongs.Include(it=>it.QuanHuyen).Include(it => it.CanBos).ThenInclude(it => it.DanhGiaHoiViens).Include(it => it.DanhGiaToChucHois).AsQueryable();
                var phamvis = _context.PhamVis.Where(it => it.AccountId == AccountId()).Select(it => it.MaDiabanHoatDong).ToList();
                model = model.Where(it => phamvis.Contains(it.Id));
                if (!String.IsNullOrWhiteSpace(MaQuanHuyen)) {
                    model = model.Where(it => it.MaQuanHuyen == MaQuanHuyen);
                }
                var temp = model.Select(p => new BCDanhGiaToChucHoiHoiVienVM {
                    DonVi = p.TenDiaBanHoatDong,
                    HV_Tong = p.CanBos.Where(it => it.Actived == true && it.IsHoiVien == true && it.isRoiHoi != true && it.MaDiaBanHoatDong == p.Id).Count(),
                    HV_HTXSNV = p.CanBos.Where(it => it.Actived == true && it.IsHoiVien == true && it.isRoiHoi != true && it.MaDiaBanHoatDong == p.Id && it.DanhGiaHoiViens.Any(q => q.LoaiDanhGia == "01" && q.Nam == Nam)).Count(),
                    HV_HTTNV = p.CanBos.Where(it => it.Actived == true && it.IsHoiVien == true && it.isRoiHoi != true && it.MaDiaBanHoatDong == p.Id && it.DanhGiaHoiViens.Any(q => q.LoaiDanhGia == "02" && q.Nam == Nam)).Count(),
                    HV_HTNV = p.CanBos.Where(it => it.Actived == true && it.IsHoiVien == true && it.isRoiHoi != true && it.MaDiaBanHoatDong == p.Id && it.DanhGiaHoiViens.Any(q => q.LoaiDanhGia == "03" && q.Nam == Nam)).Count(),
                    HV_KHTNV = p.CanBos.Where(it => it.Actived == true && it.IsHoiVien == true && it.isRoiHoi != true && it.MaDiaBanHoatDong == p.Id && it.DanhGiaHoiViens.Any(q => q.LoaiDanhGia == "04" && q.Nam == Nam)).Count(),
                    HV_KPhanLoai = p.CanBos.Where(it => it.Actived == true && it.IsHoiVien == true && it.isRoiHoi != true && it.MaDiaBanHoatDong == p.Id && it.DanhGiaHoiViens.Any(q => q.LoaiDanhGia == "05" && q.Nam == Nam)).Count(),
                    HV_ChuaDuDKPL = p.CanBos.Where(it => it.Actived == true && it.IsHoiVien == true && it.isRoiHoi != true && it.MaDiaBanHoatDong == p.Id && it.DanhGiaHoiViens.Any(q => q.LoaiDanhGia == "06" && q.Nam == Nam)).Count(),

                    CS_Tong = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "01" && it.LoaiToChuc == "01" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    CS_HTXSNV = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "02" && it.LoaiToChuc == "01" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    CS_HTTNV = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "03" && it.LoaiToChuc == "01" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    CS_HTNV = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "04" && it.LoaiToChuc == "01" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    CS_KHTNV = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "05" && it.LoaiToChuc == "01" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    CS_KPhanLoai = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "06" && it.LoaiToChuc == "01" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,

                    DC_Tong = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "01" && it.LoaiToChuc == "02" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    DC_HTXSNV = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "02" && it.LoaiToChuc == "02" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    DC_HTTNV = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "03" && it.LoaiToChuc == "02" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    DC_HTNV = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "04" && it.LoaiToChuc == "02" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    DC_KHTNV = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "05" && it.LoaiToChuc == "02" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    DC_KPhanLoai = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "06" && it.LoaiToChuc == "02" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,

                    NN_Tong = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "01" && it.LoaiToChuc == "03" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    NN_HTXSNV = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "02" && it.LoaiToChuc == "03" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    NN_HTTNV = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "03" && it.LoaiToChuc == "03" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    NN_HTNV = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "04" && it.LoaiToChuc == "03" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    NN_KHTNV = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "05" && it.LoaiToChuc == "03" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    NN_KPhanLoai = p.DanhGiaToChucHois.SingleOrDefault(it => it.LoaiDanhGia == "06" && it.LoaiToChuc == "03" && it.IDDiaBanHoi == p.Id && it.Nam == Nam).SoLuong,
                    TenQuanHuyen = p.QuanHuyen.TenQuanHuyen

                }).ToList();
                List<String> quanHuyens = temp.Select(it=>it.TenQuanHuyen).Distinct().ToList();
                if (quanHuyens.Count > 0)
                {
                    foreach (var item in quanHuyens)
                    {
                        BCDanhGiaToChucHoiHoiVienVM add = new BCDanhGiaToChucHoiHoiVienVM();
                        add.TenQuanHuyen = item;
                        add.DonVi = item;
                        add.HV_Tong = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.HV_Tong).Sum();
                        add.HV_HTXSNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.HV_HTXSNV).Sum();
                        add.HV_HTTNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.HV_HTTNV).Sum();
                        add.HV_HTNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.HV_HTNV).Sum();
                        add.HV_KHTNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.HV_KHTNV).Sum();
                        add.HV_KPhanLoai = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.HV_KPhanLoai).Sum();
                        add.HV_ChuaDuDKPL = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.HV_ChuaDuDKPL).Sum();

                        add.CS_Tong = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.CS_Tong).Sum();
                        add.CS_HTXSNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.CS_HTXSNV).Sum();
                        add.CS_HTTNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.CS_HTTNV).Sum();
                        add.CS_HTNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.CS_HTNV).Sum();
                        add.CS_KHTNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.CS_KHTNV).Sum();
                        add.CS_KPhanLoai = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.CS_KPhanLoai).Sum();

                        add.DC_Tong = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.DC_Tong).Sum();
                        add.DC_HTXSNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.DC_HTXSNV).Sum();
                        add.DC_HTTNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.DC_HTTNV).Sum();
                        add.DC_HTNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.DC_HTNV).Sum();
                        add.DC_KHTNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.DC_KHTNV).Sum();
                        add.DC_KPhanLoai = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.DC_KPhanLoai).Sum();

                        add.NN_Tong = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.NN_Tong).Sum();
                        add.NN_HTXSNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.NN_HTXSNV).Sum();
                        add.NN_HTTNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.NN_HTTNV).Sum();
                        add.NN_HTNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.NN_HTNV).Sum();
                        add.NN_KHTNV = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.NN_KHTNV).Sum();
                        add.NN_KPhanLoai = temp.Where(it => it.TenQuanHuyen == item).Select(it => it.NN_KPhanLoai).Sum();
                        temp.Add(add);
                    }
                }
                return temp;
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
                throw;
            }
        }
        #endregion Helper
    }
}
