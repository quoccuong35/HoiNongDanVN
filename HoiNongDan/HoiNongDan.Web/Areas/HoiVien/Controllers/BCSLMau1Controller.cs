using DocumentFormat.OpenXml.Wordprocessing;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class BCSLMau1Controller : BaseController
    {
        const string controllerCode = ConstExcelController.HoiVien;
        const int startIndex = 8;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string[] DateFomat;
        public BCSLMau1Controller(AppDbContext context, IWebHostEnvironment hostEnvironment, IConfiguration config) : base(context)
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
        public IActionResult _Search(string? MaQuanHuyen, Guid? MaDiaBanHoiVien, Guid? MaChiHoi = null)
        {
            return ExecuteSearch(() => {
                var model = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoiVien: MaDiaBanHoiVien, MaChiHoi: MaChiHoi).OrderBy(it => it.TongSL).ToList();
                return PartialView(model);
            });
        }
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit(String? MaQuanHuyen, Guid? MaDiaBanHoiVien, Guid? MaChiHoi = null)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\ThongKeSoLuongHVDangHoatDong.xlsx");
            var data = LoadData(MaQuanHuyen: MaQuanHuyen, MaDiaBanHoiVien: MaDiaBanHoiVien, MaChiHoi: MaChiHoi).OrderBy(it=>it.TongSL).ToList();
            int stt = 1;
            data.ForEach (it => {
                it.Stt = stt;
                stt++;
            }) ;



            byte[] filecontent = ClassExportExcel.ExportExcel(data, startIndex, url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "ThongKeSoLuongHVDangHoatDong");

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Index
        #region Helper
        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId() );

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
            var chiHoi = _context.ToHoiNganhNghe_ChiHoiNganhNghes.Where(it=>it.Loai =="01" && it.Actived == true).Select(it=>new {
                MaChiHoi= it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe,
                Ten = it.Ten
            }).ToList();
            ViewBag.MaChiHoi = new SelectList(chiHoi, "MaChiHoi", "Ten");

        }
     
        private List<BCSLMau1> LoadData(string? MaQuanHuyen, Guid? MaDiaBanHoiVien, Guid? MaChiHoi)
        {
            try
            {
                DateTime dateNow = DateTime.Now;

                var model = _context.CanBos.Where(it => it.IsHoiVien == true && it.isRoiHoi != true 
               && it.HoiVienDuyet == true).Include(it => it.DiaBanHoatDong).ThenInclude(it => it.QuanHuyen).Include(it=>it.ChiHoi).Include(it=>it.ToHoi).AsQueryable();
                if (!String.IsNullOrEmpty(MaQuanHuyen))
                {
                    model = model.Where(it => it.DiaBanHoatDong.MaQuanHuyen == MaQuanHuyen);
                }
                if (MaDiaBanHoiVien != null)
                {
                    model = model.Where(it => it.MaDiaBanHoatDong == MaDiaBanHoiVien);
                }
                if (MaChiHoi != null)
                {
                    model = model.Where(it => it.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Any(p=>p.Ma_ToHoiNganhNghe_ChiHoiNganhNghe == MaChiHoi));
                }
                var phamvis = _context.PhamVis.Where(it => it.AccountId == AccountId()).Select(it => it.MaDiabanHoatDong).ToList();
                model = model.Where(it => phamvis.Contains(it.MaDiaBanHoatDong!.Value));
                List<Guid> pass = new List<Guid>();
                pass.Add(Guid.Parse("662ac072-fece-41e2-9a5e-e47c362d10cb"));
                pass.Add(Guid.Parse("bf7024f4-6bef-442a-9d6b-ce4538b1a084"));
                pass.Add(Guid.Parse("40a7400d-1981-45e8-b4a6-412af186dc5d"));
                var modeltemp = model.Select(it => new {
                    it.IDCanBo,
                    it.HoVaTen,
                    it.DiaBanHoatDong!.TenDiaBanHoatDong,
                    it.DiaBanHoatDong.Id,
                    it.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                    it.DiaBanHoatDong.MaQuanHuyen,
                    NgaySinh = Function.ConvertStringToDate(it.NgaySinh!),
                    it.NgayRoiHoi,
                    it.HoiVienDuyet,
                    LoaiToHoi = it.ToHoi.Loai,
                    LoaiChiHoi = it.ChiHoi.Loai,
                    it.CreatedTime,
                    it.MaDanToc,
                    it.MaTonGiao,
                    it.NgayThamGiaCapUyDang,
                    it.NgayThamGiaHDND,
                    it.HoiVienDanhDu,
                    it.HoiVienDanCu,
                    it.HoiVienNganhNghe,
                    it.TuChoi,
                    it.MaTrinhDoHocVan,
                    it.MaTrinhDoChinhTri,
                    it.MaNgheNghiep,
                    it.MaGiaDinhThuocDien,
                    it.NgayDuyet,
                    it.GioiTinh,
                    it.HoiVienNongCot,
                    it.NgayVaoDangChinhThuc
                }).ToList();
                List<BCSLMau1> data = new List<BCSLMau1>();
                string[] Cap1 = { "1/12","2/12","3/12","4/12","5/12" };
                string[] Cap2 = { "6/12", "7/12", "8/12", "9/12" };
                string[] Cap3 = { "10/12", "11/12", "12/12" };
                if (String.IsNullOrWhiteSpace(MaQuanHuyen) && MaDiaBanHoiVien == null)
                {
                   var  data1 = modeltemp.Where(it=>pass.Contains(it.Id)).GroupBy(it => new { it.Id, it.TenDiaBanHoatDong }).Select(p => new BCSLMau1
                    {
                        Ten = p.Key.TenDiaBanHoatDong,
                        TongSL = p.Count(),
                        TongNu = p.Where(it => it.GioiTinh == GioiTinh.Nữ).Count(),
                        ChiHoiDanCu = p.Where(it => it.HoiVienNganhNghe != true && it.LoaiToHoi != "02" && it.LoaiChiHoi != "02").Count(),
                        ChiHoiNganhNghe = p.Where(it => it.HoiVienNganhNghe == true || it.LoaiToHoi == "02" || it.LoaiChiHoi == "02").Count(),
                        TongDanToc = p.Where(it => it.MaDanToc != null && it.MaDanToc != "KH" && it.MaDanToc != "KINH").Count(),
                        TongTonGiao = p.Where(it => it.MaTonGiao != null && it.MaTonGiao != "KH").Count(),
                        DangVien = p.Where(it => !String.IsNullOrWhiteSpace(it.NgayVaoDangChinhThuc)).Count(),
                        ThamGiaCapUyDang = p.Where(it => !String.IsNullOrWhiteSpace(it.NgayThamGiaCapUyDang)).Count(),
                        ThamGiaHDNN = p.Where(it => !String.IsNullOrWhiteSpace(it.NgayThamGiaHDND)).Count(),
                        DanhDu = p.Where(it => it.HoiVienDanhDu == true).Count(),
                        HVNongCot = p.Where(it => it.HoiVienNongCot == true).Count(),

                        DoTuoi40 = p.Where(it => (dateNow.Year - it.NgaySinh.Year + 1) < 40).Count(),
                        DoTuoi60 = p.Where(it => (((dateNow.Year - it.NgaySinh.Year + 1) > 39) && (dateNow.Year - it.NgaySinh.Year + 1) < 60)).Count(),
                        DoTuoiTren60 = p.Where(it => (dateNow.Year - it.NgaySinh.Year + 1) > 60).Count(),

                        Cap1 = p.Where(it => Cap1.Contains(it.MaTrinhDoHocVan)).Count(),
                        Cap2 = p.Where(it => Cap2.Contains(it.MaTrinhDoHocVan)).Count(),
                        Cap3 = p.Where(it => Cap3.Contains(it.MaTrinhDoHocVan)).Count(),

                        ChinhTri_SC = p.Where(it => it.MaTrinhDoChinhTri == "SC").Count(),
                        ChinhTri_TC = p.Where(it => it.MaTrinhDoChinhTri == "TC").Count(),
                        ChinhTri_CC = p.Where(it => it.MaTrinhDoChinhTri == "CC").Count(),
                        ChinhTri_CN = p.Where(it => it.MaTrinhDoChinhTri == "DH").Count(),

                        HoNgheo = p.Where(it => it.MaGiaDinhThuocDien == "01").Count(),
                        CanNgheo = p.Where(it => it.MaGiaDinhThuocDien == "02").Count(),
                        GiaDinhChinhSach = p.Where(it => it.MaGiaDinhThuocDien == "03").Count(),
                        ThanhPhanKhac = p.Where(it => it.MaGiaDinhThuocDien == "04").Count(),

                        NongDan = p.Where(it => it.MaNgheNghiep == "01").Count(),
                        CongNhan = p.Where(it => it.MaNgheNghiep == "02").Count(),
                        CongChuc_VienChuc = p.Where(it => it.MaNgheNghiep == "03").Count(),
                        DoanhNghiep = p.Where(it => it.MaNgheNghiep == "05").Count(),
                        HocSinh_SinhVien = p.Where(it => it.MaNgheNghiep == "07").Count(),
                        LaoDongTuDo = p.Where(it => it.MaNgheNghiep == "06").Count(),
                        HuuTri = p.Where(it => it.MaNgheNghiep == "04").Count(),
                    }).OrderBy(it => it.TongSL).ToList();

                    data = modeltemp.Where(it=>!pass.Contains(it.Id)).GroupBy(it => new { it.TenQuanHuyen }).Select(p => new BCSLMau1
                    {
                        Ten = p.Key.TenQuanHuyen,
                        TongSL = p.Count(),
                        TongNu = p.Where(it => it.GioiTinh == GioiTinh.Nữ).Count(),
                        ChiHoiDanCu = p.Where(it => it.HoiVienNganhNghe != true && it.LoaiToHoi != "02" && it.LoaiChiHoi != "02").Count(),
                        ChiHoiNganhNghe = p.Where(it => it.HoiVienNganhNghe == true || it.LoaiToHoi == "02" || it.LoaiChiHoi == "02").Count(),
                        TongDanToc = p.Where(it => it.MaDanToc != null && it.MaDanToc != "KH" && it.MaDanToc != "KINH").Count(),
                        TongTonGiao = p.Where(it => it.MaTonGiao != null && it.MaTonGiao != "KH").Count(),
                        DangVien = p.Where(it => !String.IsNullOrWhiteSpace(it.NgayVaoDangChinhThuc)).Count(),
                        ThamGiaCapUyDang = p.Where(it => !String.IsNullOrWhiteSpace(it.NgayThamGiaCapUyDang)).Count(),
                        ThamGiaHDNN = p.Where(it => !String.IsNullOrWhiteSpace(it.NgayThamGiaHDND)).Count(),
                        DanhDu = p.Where(it => it.HoiVienDanhDu == true).Count(),
                        HVNongCot = p.Where(it => it.HoiVienNongCot == true).Count(),

                        DoTuoi40 = p.Where(it => (dateNow.Year - it.NgaySinh.Year + 1) < 40).Count(),
                        DoTuoi60 = p.Where(it => (((dateNow.Year - it.NgaySinh.Year + 1) > 39) && (dateNow.Year - it.NgaySinh.Year + 1) < 60)).Count(),
                        DoTuoiTren60 = p.Where(it => (dateNow.Year - it.NgaySinh.Year + 1) > 60).Count(),

                        Cap1 = p.Where(it => Cap1.Contains(it.MaTrinhDoHocVan)).Count(),
                        Cap2 = p.Where(it => Cap2.Contains(it.MaTrinhDoHocVan)).Count(),
                        Cap3 = p.Where(it => Cap3.Contains(it.MaTrinhDoHocVan)).Count(),

                        ChinhTri_SC = p.Where(it => it.MaTrinhDoChinhTri =="SC").Count(),
                        ChinhTri_TC = p.Where(it => it.MaTrinhDoChinhTri =="TC").Count(),
                        ChinhTri_CC = p.Where(it => it.MaTrinhDoChinhTri =="CC").Count(),
                        ChinhTri_CN = p.Where(it => it.MaTrinhDoChinhTri == "DH").Count(),

                        HoNgheo = p.Where(it => it.MaGiaDinhThuocDien == "01").Count(),
                        CanNgheo = p.Where(it => it.MaGiaDinhThuocDien == "02").Count(),
                        GiaDinhChinhSach = p.Where(it => it.MaGiaDinhThuocDien == "03").Count(),
                        ThanhPhanKhac = p.Where(it => it.MaGiaDinhThuocDien == "04").Count(),

                        NongDan = p.Where(it => it.MaNgheNghiep == "01").Count(),
                        CongNhan = p.Where(it => it.MaNgheNghiep == "02").Count(),
                        CongChuc_VienChuc = p.Where(it => it.MaNgheNghiep == "03").Count(),
                        DoanhNghiep = p.Where(it => it.MaNgheNghiep == "05").Count(),
                        HocSinh_SinhVien = p.Where(it => it.MaNgheNghiep == "07").Count(),
                        LaoDongTuDo = p.Where(it => it.MaNgheNghiep == "06").Count(),
                        HuuTri = p.Where(it => it.MaNgheNghiep == "04").Count(),



                     }).OrderBy(it=>it.TongSL).ToList();

                    data.AddRange(data1);
                }
                else
                {
                     data = modeltemp.GroupBy(it => new { it.Id, it.TenDiaBanHoatDong }).Select(p => new BCSLMau1
                    {
                         Ten = p.Key.TenDiaBanHoatDong,
                         TongSL = p.Count(),
                         TongNu = p.Where(it => it.GioiTinh == GioiTinh.Nữ).Count(),
                         ChiHoiDanCu = p.Where(it => it.HoiVienNganhNghe != true && it.LoaiToHoi != "02" && it.LoaiChiHoi != "02").Count(),
                         ChiHoiNganhNghe = p.Where(it => it.HoiVienNganhNghe == true || it.LoaiToHoi == "02" || it.LoaiChiHoi == "02").Count(),
                         TongDanToc = p.Where(it => it.MaDanToc != null && it.MaDanToc != "KH" && it.MaDanToc != "KINH").Count(),
                         TongTonGiao = p.Where(it => it.MaTonGiao != null && it.MaTonGiao != "KH").Count(),
                         DangVien = p.Where(it => !String.IsNullOrWhiteSpace(it.NgayVaoDangChinhThuc)).Count(),
                         ThamGiaCapUyDang = p.Where(it => !String.IsNullOrWhiteSpace(it.NgayThamGiaCapUyDang)).Count(),
                         ThamGiaHDNN = p.Where(it => !String.IsNullOrWhiteSpace(it.NgayThamGiaHDND)).Count(),
                         DanhDu = p.Where(it => it.HoiVienDanhDu == true).Count(),
                         HVNongCot = p.Where(it => it.HoiVienNongCot == true).Count(),

                         DoTuoi40 = p.Where(it => (dateNow.Year - it.NgaySinh.Year + 1) < 40).Count(),
                         DoTuoi60 = p.Where(it => (((dateNow.Year - it.NgaySinh.Year + 1) > 39) && (dateNow.Year - it.NgaySinh.Year + 1) < 60)).Count(),
                         DoTuoiTren60 = p.Where(it => (dateNow.Year - it.NgaySinh.Year + 1) > 60).Count(),

                         Cap1 = p.Where(it => Cap1.Contains(it.MaTrinhDoHocVan)).Count(),
                         Cap2 = p.Where(it => Cap2.Contains(it.MaTrinhDoHocVan)).Count(),
                         Cap3 = p.Where(it => Cap3.Contains(it.MaTrinhDoHocVan)).Count(),

                         ChinhTri_SC = p.Where(it => it.MaTrinhDoChinhTri == "SC").Count(),
                         ChinhTri_TC = p.Where(it => it.MaTrinhDoChinhTri == "TC").Count(),
                         ChinhTri_CC = p.Where(it => it.MaTrinhDoChinhTri == "CC").Count(),
                         ChinhTri_CN = p.Where(it => it.MaTrinhDoChinhTri == "DH").Count(),

                         HoNgheo = p.Where(it => it.MaGiaDinhThuocDien == "01").Count(),
                         CanNgheo = p.Where(it => it.MaGiaDinhThuocDien == "02").Count(),
                         GiaDinhChinhSach = p.Where(it => it.MaGiaDinhThuocDien == "03").Count(),
                         ThanhPhanKhac = p.Where(it => it.MaGiaDinhThuocDien == "04").Count(),

                         NongDan = p.Where(it => it.MaNgheNghiep == "01").Count(),
                         CongNhan = p.Where(it => it.MaNgheNghiep == "02").Count(),
                         CongChuc_VienChuc = p.Where(it => it.MaNgheNghiep == "03").Count(),
                         DoanhNghiep = p.Where(it => it.MaNgheNghiep == "05").Count(),
                         HocSinh_SinhVien = p.Where(it => it.MaNgheNghiep == "07").Count(),
                         LaoDongTuDo = p.Where(it => it.MaNgheNghiep == "06").Count(),
                         HuuTri = p.Where(it => it.MaNgheNghiep == "04").Count(),
                     }).OrderBy(it => it.TongSL).ToList();
                }
                if (data.Count > 0 && MaDiaBanHoiVien == null)
                {
                    BCSLMau1 tong = new BCSLMau1 { 

                        Ten = "Tổng cộng",
                        TongSL = data.Sum(it=>it.TongSL),
                        TongNu = data.Sum(it=>it.TongNu),
                        ChiHoiDanCu = data.Sum(it => it.ChiHoiDanCu),
                        ChiHoiNganhNghe = data.Sum(it => it.ChiHoiNganhNghe),
                        TongDanToc = data.Sum(it => it.TongDanToc),
                        TongTonGiao = data.Sum(it => it.TongTonGiao),
                        DangVien = data.Sum(it => it.DangVien),
                        ThamGiaCapUyDang = data.Sum(it => it.ThamGiaCapUyDang),
                        ThamGiaHDNN = data.Sum(it => it.ThamGiaHDNN),
                        DanhDu = data.Sum(it => it.DanhDu),
                        HVNongCot = data.Sum(it => it.HVNongCot),
                        DoTuoi40 = data.Sum(it=>it.DoTuoi40),
                        DoTuoi60 = data.Sum(it=>it.DoTuoi60),
                        DoTuoiTren60 = data.Sum(it => it.DoTuoiTren60),

                        Cap1 = data.Sum(it => it.Cap1),
                        Cap2 = data.Sum(it => it.Cap2),
                        Cap3 = data.Sum(it => it.Cap3),

                        ChinhTri_SC = data.Sum(it => it.ChinhTri_SC),
                        ChinhTri_TC = data.Sum(it => it.ChinhTri_TC),
                        ChinhTri_CC = data.Sum(it => it.ChinhTri_CC),
                        ChinhTri_CN = data.Sum(it => it.ChinhTri_CN),

                        HoNgheo = data.Sum(it => it.HoNgheo),
                        CanNgheo = data.Sum(it => it.CanNgheo),
                        GiaDinhChinhSach = data.Sum(it => it.GiaDinhChinhSach),
                        ThanhPhanKhac = data.Sum(it => it.ThanhPhanKhac),

                        NongDan = data.Sum(it => it.NongDan),
                        CongNhan = data.Sum(it => it.CongNhan),
                        CongChuc_VienChuc = data.Sum(it => it.CongChuc_VienChuc),
                        DoanhNghiep = data.Sum(it => it.DoanhNghiep),
                        HocSinh_SinhVien = data.Sum(it => it.HocSinh_SinhVien),
                        LaoDongTuDo = data.Sum(it => it.LaoDongTuDo),
                        HuuTri = data.Sum(it => it.HuuTri),


                    };
                    data.Add(tong);
                }
                return data;
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
