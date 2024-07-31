using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Extensions;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Data;
using HoiNongDan.DataAccess.Migrations;
using System.Transactions;
using NuGet.Protocol.Plugins;
using System.Threading.Tasks.Sources;
using System.Reflection.Metadata;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HoiVien_ToHoiNN_ChiHoiNNController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        const string controllerCode = ConstExcelController.HoiVien_ToHoiNN_ChiHoiNN;
        const int startIndex = 4;
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

                var model = LoadToHoiChiHoiNganhNghe(search);
                return PartialView(model);

            });
        }
        #endregion Index

        #region Helper
        private void CreateViewBagSearch()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);

            ViewBag.Ma_ToHoiNganhNghe_ChiHoiNganhNghe = fnViewBag.ToHoiChiHoiNganNghe();

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }

        [NonAction]
        private void CreateViewBagImport()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);
            ViewBag.MaHinhThucHoTro = fnViewBag.HinhThucHoTro();

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }

        [NonAction]
        private List<HoiVien_ToHoiNN_ChiHoiNNDetailVM>? LoadToHoiChiHoiNganhNghe(HoiVien_ToHoiNN_ChiHoiNNSearchVM search)
        {
            var data = (from thch_hv in _context.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens
                        join thc in _context.ToHoiNganhNghe_ChiHoiNganhNghes on thch_hv.Ma_ToHoiNganhNghe_ChiHoiNganhNghe equals thc.Ma_ToHoiNganhNghe_ChiHoiNganhNghe
                        join hv in _context.CanBos on thch_hv.IDHoiVien equals hv.IDCanBo
                        join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                        where pv.AccountId == AccountId()
                        && hv.IsHoiVien == true
                        select thch_hv).Include(it => it.HoiVien).Include(it => it.HoiVien.DiaBanHoatDong).ThenInclude(it => it.QuanHuyen).ThenInclude(it => it.PhuongXas).Include(it => it.ToHoiNganhNghe_ChiHoiNganhNghe).AsQueryable();

            //var data = _context.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Include(it => it.HoiVien).Include(it => it.HoiVien.DiaBanHoatDong).Include(it => it.ToHoiNganhNghe_ChiHoiNganhNghe).AsQueryable();
            if (search.Ma_ToHoiNganhNghe_ChiHoiNganhNghe != null)
            {
                data = data.Where(it => it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe == search.Ma_ToHoiNganhNghe_ChiHoiNganhNghe);
            }
            if (search.MaDiaBanHoiVien != null)
            {
                data = data.Where(it => it.HoiVien.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
            }
            if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen))
            {
                data = data.Where(it => it.HoiVien.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
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
                ID = it.IDHoiVien.ToString() + "_" + it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe.ToString(),
                MaCanBo = it.HoiVien.MaCanBo,
                HoVaTen = it.HoiVien.HoVaTen,
                SoCCCD = it.HoiVien.SoCCCD,
                TenChiHoi = it.ToHoiNganhNghe_ChiHoiNganhNghe.Loai == "01" ? it.ToHoiNganhNghe_ChiHoiNganhNghe.Ten : null,
                TenToHoi = it.ToHoiNganhNghe_ChiHoiNganhNghe.Loai == "02" ? it.ToHoiNganhNghe_ChiHoiNganhNghe.Ten : null,
                ChoOHienNay = it.HoiVien.ChoOHienNay,
                PhuongXa = it.HoiVien.DiaBanHoatDong!.PhuongXa.TenPhuongXa,
                QuanHuyen = it.HoiVien.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                GhiChu = ""
            }).OrderBy(it=>it.HoVaTen).ToList();
            return model;
        }
        #endregion Helper

        #region Import
        [HoiNongDanAuthorization]
        
        public IActionResult _Import() {
            CreateViewBagImport();
            return PartialView();
        }

       

        [HoiNongDanAuthorization]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Import(Guid? MaDiaBanHoiVien, String? MaQuanHuyen) {
            if (String.IsNullOrWhiteSpace(MaQuanHuyen)) {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = false,
                    Data = "Chưa chọn quận huyện"
                }); 
            }
            DataSet ds = GetDataSetFromExcel();
            if (ds != null && ds.Tables.Count > 0)
            {
                var to_Chi_Hoi_NganhNghes = _context.ToHoiNganhNghe_ChiHoiNganhNghes.Where(it => it.Actived == true).ToList();

                var hoiViens = _context.CanBos.Include(it => it.DiaBanHoatDong).Where(it => it.Actived == true && it.IsHoiVien == true && it.DiaBanHoatDong!.MaQuanHuyen == MaQuanHuyen).AsQueryable();

                if (MaDiaBanHoiVien != null)
                {
                    hoiViens = hoiViens.Where(it => it.MaDiaBanHoatDong == MaDiaBanHoiVien);
                }

                if (!String.IsNullOrWhiteSpace(MaQuanHuyen))
                {
                    hoiViens = hoiViens.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == MaQuanHuyen);
                }
                var dataHoiViens = hoiViens.Include(it => it.DiaBanHoatDong).Select(it => new CanBo { IDCanBo = it.IDCanBo, MaCanBo = it.MaCanBo, HoVaTen = it.HoVaTen, SoCCCD = it.SoCCCD, GhiChu = it.DiaBanHoatDong!.TenDiaBanHoatDong }).ToList();



                var listToHoiNganhNghe_ChiHoiNganhNghe_HoiVien = _context.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Include(it => it.HoiVien).ThenInclude(it => it.DiaBanHoatDong)
                                                                 .Where(it => it.HoiVien.DiaBanHoatDong!.MaQuanHuyen == MaQuanHuyen).Select(it => new ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien { IDHoiVien = it.IDHoiVien, Ma_ToHoiNganhNghe_ChiHoiNganhNghe = it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe }).ToList();


                List<ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien> addToHoiNganhNghe_ChiHoiNganhNghe_HoiVien = new List<ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien>();
                List<ToHoiNganhNghe_ChiHoiNganhNghe> add_To_Chi_Hoi_NganhNGhe = new List<ToHoiNganhNghe_ChiHoiNganhNghe>();

                return ExcuteImportExcel(() =>
                {
                    const TransactionScopeOption opt = new TransactionScopeOption();

                    TimeSpan span = new TimeSpan(0, 0, 30, 30);
                    using (TransactionScope ts = new TransactionScope(opt, span))
                    {
                        DataTable dt = ds.Tables[0];
                        List<string> errorList = new List<string>();
                        if (dt == null || dt.Rows.Count < startIndex)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = false,
                                Data = "Không có dữ liệu import"
                            });

                        }
                        foreach (DataRow row in dt.Rows)
                        {
                            if (dt.Rows.IndexOf(row) >= startIndex)
                            {
                                if (row[0] == null || row[0].ToString() == "")
                                    break;
                                CheckTemplate(row, addToHoiNganhNghe_ChiHoiNganhNghe_HoiVien: addToHoiNganhNghe_ChiHoiNganhNghe_HoiVien,
                                    to_Chi_Hoi_NganhNghes: to_Chi_Hoi_NganhNghes, add_To_Chi_Hoi_NganhNGhe: add_To_Chi_Hoi_NganhNGhe, hoiViens: dataHoiViens, error: errorList,
                                    listToHoiNganhNghe_ChiHoiNganhNghe_HoiVien: listToHoiNganhNghe_ChiHoiNganhNghe_HoiVien);

                            }
                        }

                        if (errorList != null && errorList.Count > 0)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = false,
                                Data = String.Join("<br/>", errorList)
                            }); ;
                        }
                        if (add_To_Chi_Hoi_NganhNGhe.Count > 0)
                            _context.ToHoiNganhNghe_ChiHoiNganhNghes.AddRange(add_To_Chi_Hoi_NganhNGhe);
                        if (addToHoiNganhNghe_ChiHoiNganhNghe_HoiVien.Count > 0)
                        {
                            var adds = addToHoiNganhNghe_ChiHoiNganhNghe_HoiVien.Select(it => new { it.IDHoiVien, it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe }).Distinct().Select(it => new ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien
                            {
                                IDHoiVien = it.IDHoiVien,
                                Ma_ToHoiNganhNghe_ChiHoiNganhNghe = it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe,
                                CreatedAccountId = AccountId(),
                                CreatedTime = DateTime.Now
                            }).ToList();
                            _context.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.AddRange(adds);
                        }


                        _context.SaveChanges();
                        ts.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = LanguageResource.ImportSuccess
                        });
                    }
                });
            }
            else
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                    Data = "Không đọc được file excel"
                });
            }
           
        }

        private DataSet GetDataSetFromExcel()
        {
            DataSet ds = new DataSet();
            try
            {
                var file = Request.Form.Files[0];
                if (file != null && file.Length > 0)
                {
                    //Check file is excel
                    //Notes: Châu bổ sung .xlsb
                    if (file.FileName.Contains("xls") || file.FileName.Contains("xlsx"))
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        var fileName = Path.GetFileName(file.FileName);
                        var mapPath = Path.Combine(wwwRootPath, @"upload\excel");
                        var path = Path.Combine(mapPath, fileName);
                        if (!Directory.Exists(mapPath))
                        {
                            Directory.CreateDirectory(mapPath);
                        }
                        try
                        {
                            using (var fileStream = new FileStream(Path.Combine(mapPath, fileName), FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }

                            using (ClassImportExcel excelHelper = new ClassImportExcel(path))
                            {
                                excelHelper.Hdr = "YES";
                                excelHelper.Imex = "1";
                                ds = excelHelper.ReadDataSet();
                            }
                        }
                        catch
                        {
                            return null;
                        }
                        finally {
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                        
                    }
                }
                return ds;
            }
            catch
            {
                return null;
            }
        }

        [NonAction]
        private void CheckTemplate(DataRow row, List<ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien> addToHoiNganhNghe_ChiHoiNganhNghe_HoiVien, 
            List<ToHoiNganhNghe_ChiHoiNganhNghe> to_Chi_Hoi_NganhNghes, List<ToHoiNganhNghe_ChiHoiNganhNghe> add_To_Chi_Hoi_NganhNGhe,
            List<CanBo>hoiViens,List<String> error, List<ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien> listToHoiNganhNghe_ChiHoiNganhNghe_HoiVien)
        {

            string? index = "",hoVaTen = "", maHoiVien = "", soCCCD = "",tenChiHoi,tenToHoi = "",phuong = "";

            index = row[0].ToString();
            
            hoVaTen = row[1].ToString();
            maHoiVien = row[2] != null? row[2].ToString()!.Trim():"";
            soCCCD = row[3] != null ? row[3].ToString()!.Trim() : "";

            tenChiHoi = row[4] != null ? row[4].ToString()!.Trim() : "";

            tenToHoi = row[5] != null ? row[5].ToString()!.Trim() : "";

            phuong = row[7] != null ? row[7].ToString()!.Trim() : "";

            if (String.IsNullOrWhiteSpace(tenChiHoi) && String.IsNullOrWhiteSpace(tenToHoi))
            {
                error.Add(String.Format(LanguageResource.ErrorImportChiToHoiNganhNghe1, index + " " + hoVaTen));
            }
            else
            {
                var hoiVien = ChekcHoiVien(maHoiVien: maHoiVien, hoVaTen: hoVaTen, soCCCD: soCCCD,phuong:phuong, hoiViens: hoiViens);
                if (hoiVien.Count() == 0)
                {
                    error.Add(String.Format(LanguageResource.ErrorImportChiToHoiNganhNghe2, index + " " + hoVaTen));
                }
                if (hoiVien.Count > 1)
                {
                    error.Add(String.Format(LanguageResource.ErrorImportChiToHoiNganhNghe3, index + " " + hoVaTen));
                }
                if (hoiVien.Count == 1)
                {
                    if (!String.IsNullOrWhiteSpace(tenChiHoi))
                    {
                       
                        String[] _ten = tenChiHoi.Split(';');
                        for (int i = 0; i < _ten.Length; i++)
                        {
                            if (_ten[i] == "")
                                continue;
                            var ketquan = AddChiHoiToHoi_HoiVien(ten: _ten[i], to_Chi_Hoi_NganhNghes: to_Chi_Hoi_NganhNghes, add_To_Chi_Hoi_NganhNghe: add_To_Chi_Hoi_NganhNGhe, loai: "01",
                                idHoiVien: hoiVien.First().IDCanBo, listToHoiNganhNghe_ChiHoiNganhNghe_HoiVien: listToHoiNganhNghe_ChiHoiNganhNghe_HoiVien);
                            if (ketquan != null)
                            {
                                addToHoiNganhNghe_ChiHoiNganhNghe_HoiVien.Add(ketquan);
                            }
                           
                        }
                    }
                    if (!String.IsNullOrWhiteSpace(tenToHoi)) {

                        String[] _ten = tenToHoi.Split(';');
                        for (int i = 0; i < _ten.Length; i++) {
                            if (_ten[i] == "")
                                continue;
                            var ketquan = AddChiHoiToHoi_HoiVien(ten: _ten[i], to_Chi_Hoi_NganhNghes: to_Chi_Hoi_NganhNghes, add_To_Chi_Hoi_NganhNghe: add_To_Chi_Hoi_NganhNGhe, loai: "02",
                                idHoiVien: hoiVien.First().IDCanBo, listToHoiNganhNghe_ChiHoiNganhNghe_HoiVien: listToHoiNganhNghe_ChiHoiNganhNghe_HoiVien);
                            if (ketquan != null)
                            {
                                addToHoiNganhNghe_ChiHoiNganhNghe_HoiVien.Add(ketquan);
                            }
                        }
                    }
                }
            }
            

        }
        [NonAction]
        private List<CanBo> ChekcHoiVien(string? maHoiVien,string? hoVaTen, string? soCCCD,string? phuong, List<CanBo> hoiViens) {
            var data = new List<CanBo>();
            if (!String.IsNullOrWhiteSpace(hoVaTen))
            {
                data = hoiViens.Where(it => it.HoVaTen.Trim().ToLower() == hoVaTen.Trim().ToLower()).ToList();
                if (data.Count == 1)
                {
                    return data;
                }
                else if(data.Count>1)
                {
                    var check = data.Where(it => it.GhiChu!.Contains(phuong!)).ToList();
                    if (check.Count == 1)
                    {
                        return check;
                    }
                }
            }
            if (!String.IsNullOrWhiteSpace(maHoiVien))
            {
                if (data.Count() > 1)
                {
                    data = data.Where(it => it.MaCanBo != null && it.MaCanBo == maHoiVien).ToList();
                    if (data.Count == 1)
                    {
                        return data;
                    }

                }
                else
                {
                    // chua co du lue
                    data = hoiViens.Where(it => it.MaCanBo != null && it.MaCanBo == maHoiVien).ToList();
                    if (data.Count == 1)
                    {
                        return data;
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(soCCCD))
            {
                if (data.Count > 1)
                {
                    data = data.Where(it => it.SoCCCD != null && it.SoCCCD!.Contains(soCCCD)).ToList();
                    if (data.Count == 1)
                    {
                        return data;
                    }
                }
                else
                {
                    data = hoiViens.Where(it => it.SoCCCD != null && it.SoCCCD!.Contains(soCCCD)).ToList();
                    if (data.Count == 1)
                    {
                        return data;
                    }
                }
            }
           
            
            return data;
        }

        [NonAction]
        private ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien AddChiHoiToHoi_HoiVien(string ten, List<ToHoiNganhNghe_ChiHoiNganhNghe> to_Chi_Hoi_NganhNghes, List<ToHoiNganhNghe_ChiHoiNganhNghe> add_To_Chi_Hoi_NganhNghe,
            string loai,Guid idHoiVien,List<ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien> listToHoiNganhNghe_ChiHoiNganhNghe_HoiVien) {

            ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien kq = new ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien();
            var checkExist = to_Chi_Hoi_NganhNghes.SingleOrDefault(it => it.Ten.ToLower() == ten.ToLower() && it.Loai ==loai);
            Guid? chiToHoiNN;
            if (checkExist != null)
            {
                var exist = listToHoiNganhNghe_ChiHoiNganhNghe_HoiVien.SingleOrDefault(it => it.IDHoiVien == idHoiVien && it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe == checkExist.Ma_ToHoiNganhNghe_ChiHoiNganhNghe);
                if (exist != null)
                    return null!;
                chiToHoiNN = checkExist!.Ma_ToHoiNganhNghe_ChiHoiNganhNghe;
                
            }
            else
            {
                chiToHoiNN = Guid.NewGuid();
                add_To_Chi_Hoi_NganhNghe.Add(new ToHoiNganhNghe_ChiHoiNganhNghe
                {
                    Ma_ToHoiNganhNghe_ChiHoiNganhNghe = chiToHoiNN.Value,
                    Ten = ten,
                    Loai = loai,
                    CreatedAccountId = AccountId(),
                    CreatedTime = DateTime.Now,
                    Actived = true
                });
                to_Chi_Hoi_NganhNghes.Add(new ToHoiNganhNghe_ChiHoiNganhNghe
                {
                    Ma_ToHoiNganhNghe_ChiHoiNganhNghe = chiToHoiNN.Value,
                    Ten = ten,
                    Loai = loai,
                });
            }
            kq.Ma_ToHoiNganhNghe_ChiHoiNganhNghe = chiToHoiNN.Value;
            kq.IDHoiVien = idHoiVien;
            return kq;
        }

        #endregion Import

        #region Export 
        [HoiNongDanAuthorization]
        public IActionResult ExportCreate()
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\MauImPortToHoi_ChiHoi_NganhNhe.xlsx");
            byte[] filecontent = ClassExportExcel.ExportFileMau(url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "ToHoi_ChiHoi_NganhNghe");

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit(HoiVien_ToHoiNN_ChiHoiNNSearchVM search)
        {
            var data = (from thch_hv in _context.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens
                        join thc in _context.ToHoiNganhNghe_ChiHoiNganhNghes on thch_hv.Ma_ToHoiNganhNghe_ChiHoiNganhNghe equals thc.Ma_ToHoiNganhNghe_ChiHoiNganhNghe
                        join hv in _context.CanBos on thch_hv.IDHoiVien equals hv.IDCanBo
                        join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                        where pv.AccountId == AccountId()
                        && hv.IsHoiVien == true
                        select hv).Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.PhuongXa).Include(it => it.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens).ThenInclude(it=>it.ToHoiNganhNghe_ChiHoiNganhNghe).AsQueryable();

            if (search.Ma_ToHoiNganhNghe_ChiHoiNganhNghe != null)
            {
                data = data.Where(it => it.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Any(p=>p.Ma_ToHoiNganhNghe_ChiHoiNganhNghe == search.Ma_ToHoiNganhNghe_ChiHoiNganhNghe));
            }
            if (search.MaDiaBanHoiVien != null)
            {
                data = data.Where(it => it.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
            }
            if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen))
            {
                data = data.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
            }
            if (search.HoVaTen != null)
            {
                data = data.Where(it => it.HoVaTen.Contains(search.HoVaTen));
            }
            if (search.MaHoiVien != null)
            {
                data = data.Where(it => it.MaCanBo == search.MaHoiVien);
            }
            var model = data.Select((it) => new HoiVien_ToHoiNN_ChiHoiNNExcelVM
            {
                HoVaTen = it.HoVaTen,
                MaCanBo = it.MaCanBo,
                SoCCCD = it.SoCCCD,
                TenChiHoi = String.Join(';', it.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Where(p => p.IDHoiVien == it.IDCanBo && p.ToHoiNganhNghe_ChiHoiNganhNghe.Loai =="01").Select(it => it.ToHoiNganhNghe_ChiHoiNganhNghe.Ten).ToList()),
                TenToHoi = String.Join(';', it.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens.Where(p => p.IDHoiVien == it.IDCanBo && p.ToHoiNganhNghe_ChiHoiNganhNghe.Loai == "02").Select(it => it.ToHoiNganhNghe_ChiHoiNganhNghe.Ten).ToList()),
                ChoOHienNay = it.ChoOHienNay,
                PhuongXa = it.DiaBanHoatDong!.PhuongXa.TenPhuongXa,
                QuanHuyen = it.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                GhiChu = ""
            }).OrderBy(it=>it.PhuongXa).ThenBy(it=>it.HoVaTen).ToList();
            model = model.DistinctBy(it=>new { it.HoVaTen,it.MaCanBo,it.SoCCCD,it.ChoOHienNay, it.QuanHuyen, it.PhuongXa }).ToList();
            int stt = 1;
            model.ForEach(value => {
                value.STT = stt;
                stt++;
            });
            
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\MauImPortToHoi_ChiHoi_NganhNhe.xlsx");
            byte[] filecontent = ClassExportExcel.ExportExcel(model, startIndex +2, url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "ToHoi_ChiHoi_NganhNghe");

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
