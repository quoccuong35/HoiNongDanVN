using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

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
            var url = Path.Combine(wwwRootPath, @"upload\filemau\BaoCaoBienDongHoiVienNew.xlsx");
            var data = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoatDong: MaDiaBanHoatDong, DenNgay: DenNgay);
            int stt = 1;
            data.ForEach(x =>
            {
                x.Cot1 = stt;
                stt++;
            });

            byte[] filecontent = ClassExportExcel.ExportExcel(data, startIndex, url);
            string fileNameWithFormat = string.Format("{0}.xlsx", "BaoCaoBienDongHoiVienNew");

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
        private List<BCTLHNew> LoadData(string? MaQuanHuyen, Guid? MaDiaBanHoatDong,DateTime? DenNgay)
        {
            try
            {
                var model1 = _context.CanBos.Where(it => it.Actived == true && it.IsHoiVien == true && it.HoiVienDuyet == true)
                    .Include(it => it.ChiHoi)
                    .Include(it => it.ToHoi)
                    .Include(it => it.DiaBanHoatDong)
                        .ThenInclude(it => it.QuanHuyen)
                    .Include(it => it.DiaBanHoatDong)
                        .ThenInclude(it => it.PhuongXa)
                     .Include(it => it.DiaBanHoatDong)
                        .ThenInclude(it => it.DanhGiaToChucHois)
                    .Include(it => it.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens)
                        .ThenInclude(it => it.ToHoiNganhNghe_ChiHoiNganhNghe)
                    .Include(it => it.DanhGiaHoiViens)
                    .Select(it => new
                    {
                        it.IDCanBo,
                        it.MaCanBo,
                        it.HoVaTen,
                        it.DiaBanHoatDong!.TenDiaBanHoatDong,
                        it.DiaBanHoatDong.Id,
                        it.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                        it.DiaBanHoatDong.MaQuanHuyen,
                        NgayVaoHoi = Function.ConvertStringToDate(it.NgayVaoHoi!),
                        it.NgayRoiHoi,
                        it.HoiVienDuyet,
                        it.CreatedTime,
                        it.MaDanToc,
                        it.isRoiHoi,
                        it.HoiVienDanCu,
                        it.HoiVienNganhNghe,
                        it.HoiVienDanhDu,
                        it.TuChoi,
                        it.NgayDuyet,
                        it.Actived,
                        it.GioiTinh,
                        it.HoiVienNongCot,
                        it.NgayVaoDangChinhThuc,
                        it.MaChiHoi,
                        LoaiChiHoi = it.ChiHoi!.Loai,
                        it.MaToHoi,
                        LoaiToHoi = it.ToHoi!.Loai,
                        it.MaTonGiao,
                        it.DangVien,
                        it.DiaBanHoatDong.MaPhuongXa,
                        it.DiaBanHoatDong.PhuongXa.TenPhuongXa
                        //DanhGiaToChuc = it.DiaBanHoatDong.DanhGiaToChucHois.Where(p => p.Nam == DenNgay.Value.Year && p.IDDiaBanHoi == it.MaDiaBanHoatDong).ToList(),
                        //DanhGiaHoiVien = it.DanhGiaHoiViens.Where(p => p.Nam == DenNgay.Value.Year && p.IDHoiVien == it.IDCanBo).ToList(),
                    }).ToList();

                DateTime dateNow = DateTime.Now;
                DenNgay = DenNgay == null ? dateNow : DenNgay;
                DateTime firstDay = new DateTime(DenNgay!.Value.Year, 1, 1);

                var model = _context.CanBos.Where(it => it.IsHoiVien == true && it.Actived == true && it.HoiVienDuyet == true ).Include(it => it.ChiHoi).Include(it => it.ToHoi)
                    .Include(it => it.DiaBanHoatDong).ThenInclude(it=>it.PhuongXa)
                    .ThenInclude(it => it.QuanHuyen).Include(it => it.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens)
                    .ThenInclude(it => it.ToHoiNganhNghe_ChiHoiNganhNghe).AsQueryable();
                if (!String.IsNullOrEmpty(MaQuanHuyen))
                {
                    model = model.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == MaQuanHuyen);
                }
                if (MaDiaBanHoatDong != null)
                {
                    model = model.Where(it => it.MaDiaBanHoatDong == MaDiaBanHoatDong);
                }
                var capThes = _context.HoiVienCapThes.Where(it => it.NgayCap >= firstDay && it.NgayCap <= DenNgay && it.LoaiCap == "02").Select(it => it.IDHoiVien);
                var modeltemp = model.Select(it => new {
                    it.IDCanBo,
                    it.MaCanBo,
                    it.HoVaTen,
                    it.DiaBanHoatDong!.TenDiaBanHoatDong,
                    it.DiaBanHoatDong.Id,
                    it.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                    it.DiaBanHoatDong.MaQuanHuyen,
                    NgayVaoHoi = Function.ConvertStringToDate(it.NgayVaoHoi!),
                    it.NgayRoiHoi,
                    it.HoiVienDuyet,
                    it.CreatedTime,
                    it.MaDanToc,
                    it.isRoiHoi,
                    it.HoiVienDanCu,
                    it.HoiVienNganhNghe,
                    it.HoiVienDanhDu,
                    it.TuChoi,
                    it.NgayDuyet,
                    it.Actived,
                    it.GioiTinh,
                    it.HoiVienNongCot,
                    it.NgayVaoDangChinhThuc,
                    it.MaChiHoi,
                    LoaiChiHoi = it.ChiHoi!.Loai,
                    it.MaToHoi,
                    LoaiToHoi = it.ToHoi!.Loai,
                    it.MaTonGiao,
                    it.DangVien,
                    it.DiaBanHoatDong.MaPhuongXa,
                    it.DiaBanHoatDong.PhuongXa.TenPhuongXa
                }).ToList();
                List<BCTLHNew> data = new List<BCTLHNew>();

                if (String.IsNullOrWhiteSpace(MaQuanHuyen) && MaDiaBanHoatDong == null)
                {
                    List<Guid> pass = new List<Guid>();
                    pass.Add(Guid.Parse("662ac072-fece-41e2-9a5e-e47c362d10cb"));
                    pass.Add(Guid.Parse("bf7024f4-6bef-442a-9d6b-ce4538b1a084"));
                    pass.Add(Guid.Parse("40a7400d-1981-45e8-b4a6-412af186dc5d"));
                    data = modeltemp.Where(it =>  it.NgayVaoHoi <= DenNgay.Value && !pass.Contains(it.Id) && (it.isRoiHoi != true ||
                        (it.isRoiHoi == true && it.NgayRoiHoi >= firstDay))).GroupBy(it => new { it.MaQuanHuyen, it.TenQuanHuyen }).Select(p => new BCTLHNew
                        {
                            Cot1 = 1,
                            Cot2 = p.Key.TenQuanHuyen,
                            Cot3 = p.Count(),
                            Cot4 = p.Where(it => it.GioiTinh == GioiTinh.Nữ).Count(),
                            Cot5 = p.Where(it=> it.NgayVaoHoi != null && it.NgayVaoHoi >=firstDay && it.NgayVaoHoi <= DenNgay && (it.HoiVienDanCu == true || it.HoiVienNganhNghe == null)).Count(),
                            Cot6 = p.Where(it => it.isRoiHoi == true && it.NgayRoiHoi.Value >=firstDay && it.NgayRoiHoi.Value <= DenNgay && (it.HoiVienDanCu == true || it.HoiVienNganhNghe == null)).Count(),
                            Cot7 = 0,
                            Cot8 = 0,
                            Cot9 = 0,
                            Cot10 = p.Where(it => it.HoiVienNganhNghe == true && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay).Count(),
                            Cot11 = p.Where(it => it.HoiVienNganhNghe == true && it.isRoiHoi == true && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay).Count(),
                            Cot12 = 0,
                            Cot13 = 0,
                            Cot14 = 0,
                            Cot15 = p.Where(it => it.HoiVienDanhDu == true && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay.Value).Count(),
                            Cot16 = 0,
                            Cot17 = p.Where(it => it.isRoiHoi == true && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay).Count(),
                            Cot18 = p.Where(it => it.isRoiHoi == true && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay && it.GioiTinh == GioiTinh.Nữ).Count(),
                            Cot19 = p.Where(it => it.isRoiHoi == false && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay).Count(),
                            Cot20 = p.Where(it => it.isRoiHoi == false && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && it.GioiTinh == GioiTinh.Nữ).Count(),
                            Cot21 = p.Where(it => it.MaChiHoi != null).Count(),
                            Cot22 = p.Where(it => it.LoaiChiHoi == "01" || (it.MaChiHoi != null && String.IsNullOrWhiteSpace(it.LoaiChiHoi))).Count(),
                            Cot23 = p.Where(it => it.LoaiChiHoi == "02" && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi != null && it.NgayVaoHoi <= DenNgay && it.isRoiHoi != true).Count(),
                            Cot24 = p.Where(it => it.LoaiChiHoi == "02" && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay  && it.NgayRoiHoi <= DenNgay && it.isRoiHoi == true).Count(),
                            Cot25 = 0,
                            Cot26 = 0,
                            Cot27 = 0,
                            Cot28 = p.Where(it => it.MaToHoi != null).Count(),
                            Cot29 = p.Where(it => it.LoaiToHoi == "01" || (it.MaToHoi != null && String.IsNullOrWhiteSpace(it.LoaiToHoi))).Count(),
                            Cot30 = p.Where(it => it.LoaiToHoi == "02" && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && it.isRoiHoi != true).Count(),
                            Cot31 = p.Where(it => it.LoaiToHoi == "02" && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay && it.isRoiHoi == true).Count(),
                            Cot32 = 0,
                            Cot33 = 0,
                            Cot34 = 0,
                            Cot35 = p.Where(it => !String.IsNullOrWhiteSpace(it.MaDanToc) && it.MaDanToc != "KH" && it.MaDanToc != "KINH").Count(),
                            Cot36 = p.Where(it => !String.IsNullOrWhiteSpace(it.MaTonGiao) && it.MaTonGiao != "KH").Count(),
                            Cot37 = p.Where(it => it.isRoiHoi != true && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && !String.IsNullOrWhiteSpace(it.MaCanBo)).Count(),
                            Cot38 = p.Where(it => capThes.Contains(it.IDCanBo)).Count(),
                           
                        }).ToList();
                    var dataTemp = modeltemp.Where(it => it.NgayVaoHoi <= DenNgay.Value && pass.Contains(it.Id) && (it.isRoiHoi != true ||
                        (it.isRoiHoi == true && it.NgayRoiHoi >= firstDay))).GroupBy(it => new { it.Id, it.TenDiaBanHoatDong }).Select(p => new BCTLHNew
                        {
                            Cot1 = 1,
                            Cot2 = p.Key.TenDiaBanHoatDong,
                            Cot3 = p.Count(),
                            Cot4 = p.Where(it => it.GioiTinh == GioiTinh.Nữ).Count(),
                            Cot5 = p.Where(it => it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && (it.HoiVienDanCu == true || it.HoiVienNganhNghe == null)).Count(),
                            Cot6 = p.Where(it => it.isRoiHoi == true && it.NgayRoiHoi.Value >= firstDay && it.NgayRoiHoi.Value <= DenNgay && (it.HoiVienDanCu == true || it.HoiVienNganhNghe == null)).Count(),
                            Cot7 = 0,
                            Cot8 = 0,
                            Cot9 = 0,
                            Cot10 = p.Where(it => it.HoiVienNganhNghe == true && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay).Count(),
                            Cot11 = p.Where(it => it.HoiVienNganhNghe == true && it.isRoiHoi == true && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay).Count(),
                            Cot12 = 0,
                            Cot13 = 0,
                            Cot14 = 0,
                            Cot15 = p.Where(it => it.HoiVienDanhDu == true && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay.Value).Count(),
                            Cot16 = 0,
                            Cot17 = p.Where(it => it.isRoiHoi == true && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay).Count(),
                            Cot18 = p.Where(it => it.isRoiHoi == true && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay && it.GioiTinh == GioiTinh.Nữ).Count(),
                            Cot19 = p.Where(it => it.isRoiHoi == false && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay).Count(),
                            Cot20 = p.Where(it => it.isRoiHoi == false && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && it.GioiTinh == GioiTinh.Nữ).Count(),
                            Cot21 = p.Where(it => it.MaChiHoi != null).Count(),
                            Cot22 = p.Where(it => it.LoaiChiHoi == "01").Count(),
                            Cot23 = p.Where(it => it.LoaiChiHoi == "02" && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi != null && it.NgayVaoHoi <= DenNgay && it.isRoiHoi != true).Count(),
                            Cot24 = p.Where(it => it.LoaiChiHoi == "02" && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay && it.isRoiHoi == true).Count(),
                            Cot25 = 0,
                            Cot26 = 0,
                            Cot27 = 0,
                            Cot28 = p.Where(it => it.MaToHoi != null).Count(),
                            Cot29 = p.Where(it => it.LoaiToHoi == "01").Count(),
                            Cot30 = p.Where(it => it.LoaiToHoi == "02" && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && it.isRoiHoi != true).Count(),
                            Cot31 = p.Where(it => it.LoaiToHoi == "02" && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay && it.isRoiHoi == true).Count(),
                            Cot32 = 0,
                            Cot33 = 0,
                            Cot34 = 0,
                            Cot35 = p.Where(it => !String.IsNullOrWhiteSpace(it.MaDanToc) && it.MaDanToc != "KH" && it.MaDanToc != "KINH").Count(),
                            Cot36 = p.Where(it => !String.IsNullOrWhiteSpace(it.MaTonGiao) && it.MaTonGiao != "KH").Count(),
                            Cot37 = p.Where(it => it.isRoiHoi != true && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && !String.IsNullOrWhiteSpace(it.MaCanBo)).Count(),
                            Cot38 = p.Where(it => capThes.Contains(it.IDCanBo)).Count(),

                        }).ToList();
                    data.AddRange(dataTemp);
                }
                else if (!String.IsNullOrWhiteSpace(MaQuanHuyen) && MaDiaBanHoatDong == null)
                {
                    data = modeltemp.Where(it => it.NgayVaoHoi <= DenNgay.Value && (it.isRoiHoi != true ||
                        (it.isRoiHoi == true && it.NgayRoiHoi >= firstDay))).GroupBy(it => new { it.MaPhuongXa, it.TenPhuongXa }).Select(p => new BCTLHNew
                        {
                            Cot1 = 1,
                            Cot2 = p.Key.TenPhuongXa,
                            Cot3 = p.Count(),
                            Cot4 = p.Where(it => it.GioiTinh == GioiTinh.Nữ).Count(),
                            Cot5 = p.Where(it => it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && (it.HoiVienDanCu == true || it.HoiVienNganhNghe == null)).Count(),
                            Cot6 = p.Where(it => it.isRoiHoi == true && it.NgayRoiHoi.Value >= firstDay && it.NgayRoiHoi.Value <= DenNgay && (it.HoiVienDanCu == true || it.HoiVienNganhNghe == null)).Count(),
                            Cot7 = 0,
                            Cot8 = 0,
                            Cot9 = 0,
                            Cot10 = p.Where(it => it.HoiVienNganhNghe == true && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay).Count(),
                            Cot11 = p.Where(it => it.HoiVienNganhNghe == true && it.isRoiHoi == true && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay).Count(),
                            Cot12 = 0,
                            Cot13 = 0,
                            Cot14 = 0,
                            Cot15 = p.Where(it => it.HoiVienDanhDu == true && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay.Value).Count(),
                            Cot16 = 0,
                            Cot17 = p.Where(it => it.isRoiHoi == true && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay).Count(),
                            Cot18 = p.Where(it => it.isRoiHoi == true && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay && it.GioiTinh == GioiTinh.Nữ).Count(),
                            Cot19 = p.Where(it => it.isRoiHoi == false && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay).Count(),
                            Cot20 = p.Where(it => it.isRoiHoi == false && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && it.GioiTinh == GioiTinh.Nữ).Count(),
                            Cot21 = p.Where(it => it.MaChiHoi != null).Count(),
                            Cot22 = p.Where(it => it.LoaiChiHoi == "01").Count(),
                            Cot23 = p.Where(it => it.LoaiChiHoi == "02" && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi != null && it.NgayVaoHoi <= DenNgay && it.isRoiHoi != true).Count(),
                            Cot24 = p.Where(it => it.LoaiChiHoi == "02" && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay && it.isRoiHoi == true).Count(),
                            Cot25 = 0,
                            Cot26 = 0,
                            Cot27 = 0,
                            Cot28 = p.Where(it => it.MaToHoi != null).Count(),
                            Cot29 = p.Where(it => it.LoaiToHoi == "01").Count(),
                            Cot30 = p.Where(it => it.LoaiToHoi == "02" && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && it.isRoiHoi != true).Count(),
                            Cot31 = p.Where(it => it.LoaiToHoi == "02" && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay && it.isRoiHoi == true).Count(),
                            Cot32 = 0,
                            Cot33 = 0,
                            Cot34 = 0,
                            Cot35 = p.Where(it => !String.IsNullOrWhiteSpace(it.MaDanToc) && it.MaDanToc != "KH" && it.MaDanToc != "KINH").Count(),
                            Cot36 = p.Where(it => !String.IsNullOrWhiteSpace(it.MaTonGiao) && it.MaTonGiao != "KH").Count(),
                            Cot37 = p.Where(it => it.isRoiHoi != true && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && !String.IsNullOrWhiteSpace(it.MaCanBo)).Count(),
                            Cot38 = p.Where(it => capThes.Contains(it.IDCanBo)).Count(),

                        }).ToList();
                }
                else if (MaDiaBanHoatDong != null) {
                    data = modeltemp.Where(it => it.NgayVaoHoi <= DenNgay.Value && (it.isRoiHoi != true ||
                        (it.isRoiHoi == true && it.NgayRoiHoi >= firstDay))).GroupBy(it => new { it.Id, it.TenDiaBanHoatDong }).Select(p => new BCTLHNew
                        {
                            Cot1 = 1,
                            Cot2 = p.Key.TenDiaBanHoatDong,
                            Cot3 = p.Count(),
                            Cot4 = p.Where(it => it.GioiTinh == GioiTinh.Nữ).Count(),
                            Cot5 = p.Where(it => it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && (it.HoiVienDanCu == true || it.HoiVienNganhNghe == null)).Count(),
                            Cot6 = p.Where(it => it.isRoiHoi == true && it.NgayRoiHoi.Value >= firstDay && it.NgayRoiHoi.Value <= DenNgay && (it.HoiVienDanCu == true || it.HoiVienNganhNghe == null)).Count(),
                            Cot7 = 0,
                            Cot8 = 0,
                            Cot9 = 0,
                            Cot10 = p.Where(it => it.HoiVienNganhNghe == true && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay).Count(),
                            Cot11 = p.Where(it => it.HoiVienNganhNghe == true && it.isRoiHoi == true && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay).Count(),
                            Cot12 = 0,
                            Cot13 = 0,
                            Cot14 = 0,
                            Cot15 = p.Where(it => it.HoiVienDanhDu == true && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay.Value).Count(),
                            Cot16 = 0,
                            Cot17 = p.Where(it => it.isRoiHoi == true && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay).Count(),
                            Cot18 = p.Where(it => it.isRoiHoi == true && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay && it.GioiTinh == GioiTinh.Nữ).Count(),
                            Cot19 = p.Where(it => it.isRoiHoi == false && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay).Count(),
                            Cot20 = p.Where(it => it.isRoiHoi == false && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && it.GioiTinh == GioiTinh.Nữ).Count(),
                            Cot21 = p.Where(it => it.MaChiHoi != null).Count(),
                            Cot22 = p.Where(it => it.LoaiChiHoi == "01").Count(),
                            Cot23 = p.Where(it => it.LoaiChiHoi == "02" && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi != null && it.NgayVaoHoi <= DenNgay && it.isRoiHoi != true).Count(),
                            Cot24 = p.Where(it => it.LoaiChiHoi == "02" && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay && it.isRoiHoi == true).Count(),
                            Cot25 = 0,
                            Cot26 = 0,
                            Cot27 = 0,
                            Cot28 = p.Where(it => it.MaToHoi != null).Count(),
                            Cot29 = p.Where(it => it.LoaiToHoi == "01").Count(),
                            Cot30 = p.Where(it => it.LoaiToHoi == "02" && it.NgayVaoHoi != null && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && it.isRoiHoi != true).Count(),
                            Cot31 = p.Where(it => it.LoaiToHoi == "02" && it.NgayRoiHoi != null && it.NgayRoiHoi >= firstDay && it.NgayRoiHoi <= DenNgay && it.isRoiHoi == true).Count(),
                            Cot32 = 0,
                            Cot33 = 0,
                            Cot34 = 0,
                            Cot35 = p.Where(it => !String.IsNullOrWhiteSpace(it.MaDanToc) && it.MaDanToc != "KH" && it.MaDanToc != "KINH").Count(),
                            Cot36 = p.Where(it => !String.IsNullOrWhiteSpace(it.MaTonGiao) && it.MaTonGiao != "KH").Count(),
                            Cot37 = p.Where(it => it.isRoiHoi != true && it.NgayVaoHoi >= firstDay && it.NgayVaoHoi <= DenNgay && !String.IsNullOrWhiteSpace(it.MaCanBo)).Count(),
                            Cot38 = p.Where(it => capThes.Contains(it.IDCanBo)).Count(),

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
                tong.Cot27 = data.Sum(it => it.Cot27);
                tong.Cot28 = data.Sum(it => it.Cot28);
                tong.Cot29 = data.Sum(it => it.Cot29);
                tong.Cot30 = data.Sum(it => it.Cot30);
                tong.Cot31 = data.Sum(it => it.Cot31);
                tong.Cot32 = data.Sum(it => it.Cot32);
                tong.Cot33 = data.Sum(it => it.Cot33);
                tong.Cot34 = data.Sum(it => it.Cot34);
                tong.Cot35 = data.Sum(it => it.Cot35);
                tong.Cot36 = data.Sum(it => it.Cot36);
                tong.Cot37 = data.Sum(it => it.Cot37);
                tong.Cot38 = data.Sum(it => it.Cot38);

                data.Add(tong);
                int i = 1;
                data.OrderBy(it => it.Cot3).ToList().ForEach(it => {
                    it.Cot1 = i;
                    i++;
                });
                return data.OrderBy(it=>it.Cot1).ToList();
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
