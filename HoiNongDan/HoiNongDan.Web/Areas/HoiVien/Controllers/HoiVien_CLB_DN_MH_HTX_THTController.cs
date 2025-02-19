using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Extensions;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Models;
using HoiNongDan.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Transactions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace HoiNongDan.Web.Areas.HoiVien.Controllers
{
    [Area(ConstArea.HoiVien)]
    public class HoiVien_CLB_DN_MH_HTX_THTController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        const string controllerCode = ConstExcelController.HoiVien_CLB_DN_MH_HTX_THT;
        const int startIndex = 5;
        private string filemau = @"upload\filemau\CLB_DN_MH_HTX_THT.xlsx";
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
            var model = LoadData(search);
                return PartialView(model);

            });
        }
        #endregion Index

        #region Import
        [HoiNongDanAuthorization]
        public IActionResult _Import()
        {
            CreateViewBagImport();
            return PartialView();
        }

        [HoiNongDanAuthorization]
        public IActionResult Import() {
            //if (String.IsNullOrWhiteSpace(MaQuanHuyen))
            //{
            //    return Json(new
            //    {
            //        Code = System.Net.HttpStatusCode.Created,
            //        Success = false,
            //        Data = "Chưa chọn hội no6g dân cơ sở"
            //    }); 
            //}
            DataSet ds = GetDataSetFromExcel();
            if (ds != null && ds.Tables.Count > 0)
            {

                //if (MaDiaBanHoiVien != null)
                //{
                //    hoiViens = hoiViens.Where(it => it.MaDiaBanHoatDong == MaDiaBanHoiVien);
                //}
                //if (!String.IsNullOrWhiteSpace(MaQuanHuyen))
                //{
                //    hoiViens = hoiViens.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == MaQuanHuyen);
                //}


                var listCLB_DN_MH_HTX_THT_HoiVien = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.ToList();
                var listMasterCLB_DN_MH_HTX_THT = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.ToList();
                DataTable dt = ds.Tables[0];
                List<string> errorList = new List<string>();
                return ExcuteImportExcel(() =>
                {
                    const TransactionScopeOption opt = new TransactionScopeOption();

                    TimeSpan span = new TimeSpan(0, 0, 30, 30);
                    using (TransactionScope ts = new TransactionScope(opt, span)) {
                        List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac> addCLB_DN_MH_HTX_THTs = new List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac>();
                        List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien> addCLB_DN_MH_HTX_THT_HoiViens = new List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien>();
                        foreach (DataRow row in dt.Rows)
                        {
                            if (dt.Rows.IndexOf(row) >= startIndex-1)
                            {
                                if (row[0] == null || row[0].ToString() == "")
                                    break;
                                CheckTemplate(row, listMasterCLB_DN_MH_HTX_THT: listMasterCLB_DN_MH_HTX_THT, addCLB_DN_MH_HTX_THTs: addCLB_DN_MH_HTX_THTs, listCLB_DN_MH_HTX_THT_HoiVien: listCLB_DN_MH_HTX_THT_HoiVien,
                               addCLB_DN_MH_HTX_THT_HoiViens: addCLB_DN_MH_HTX_THT_HoiViens, error: errorList);
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
                        if (addCLB_DN_MH_HTX_THTs.Count > 0)
                            _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.AddRange(addCLB_DN_MH_HTX_THTs);
                        if (addCLB_DN_MH_HTX_THT_HoiViens.Count > 0)
                        {
                            var adds = addCLB_DN_MH_HTX_THT_HoiViens.Select(it => new { it.IDHoiVien, it.Id_CLB_DN_MH_HTX_THT }).Distinct().Select(it => new CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien
                            {
                                IDHoiVien = it.IDHoiVien,
                                Id_CLB_DN_MH_HTX_THT = it.Id_CLB_DN_MH_HTX_THT,
                                CreatedAccountId = AccountId(),
                                CreatedTime = DateTime.Now
                            }).ToList();
                            _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.AddRange(adds);
                        }


                       int stt = _context.SaveChanges();
                        if (stt > 0)
                        {
                            ts.Complete();
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = true,
                                Data = LanguageResource.ImportSuccess + " " + stt.ToString()
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = false,
                                Data = "Không import thành công"
                            });
                        }
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
        protected DataSet GetDataSetFromExcel()
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
                            return null!;
                        }
                        finally
                        {
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
                return null!;
            }
        }
        [NonAction]
        private void CheckTemplate(DataRow row,List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac> listMasterCLB_DN_MH_HTX_THT, List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac> addCLB_DN_MH_HTX_THTs,
            List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien> listCLB_DN_MH_HTX_THT_HoiVien, List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien> addCLB_DN_MH_HTX_THT_HoiViens,  List<String> error ) 
        {
            string? index = "", hoVaTen = "",  soCCCD = "", cauLacBo, doiNhom = "", moHinh = "", hopTacXa = "", toHopTac = "",phuong = "";
            index = row[0].ToString();

            hoVaTen = row[1].ToString();

            soCCCD = row[2] != null ? row[2].ToString()!.Trim() : "";
            cauLacBo = row[5] != null ? row[5].ToString()!.Trim() : "";

            doiNhom = row[6] != null ? row[6].ToString()!.Trim() : "";
            moHinh = row[7] != null ? row[7].ToString()!.Trim() : "";
            hopTacXa = row[8] != null ? row[8].ToString()!.Trim() : "";
            toHopTac = row[9] != null ? row[9].ToString()!.Trim() : "";


            if (String.IsNullOrWhiteSpace(cauLacBo) && String.IsNullOrWhiteSpace(doiNhom) && String.IsNullOrWhiteSpace(moHinh) &&
                String.IsNullOrWhiteSpace(hopTacXa) && String.IsNullOrWhiteSpace(toHopTac))
            {
                error.Add(String.Format(LanguageResource.CLB_DN_MH_HTX_THTError1, index + " " + hoVaTen));
            }
            else {
                var hoiVien = ChekcHoiVien(soCCCD: soCCCD);
                if (hoiVien == null)
                {
                    error.Add(String.Format(LanguageResource.ErrorImportChiToHoiNganhNghe2, index + " " + hoVaTen));
                }
                else{
                    if (!String.IsNullOrWhiteSpace(cauLacBo)) {
                        String[] _ten = cauLacBo.Split(';');
                        for (int i = 0; i < _ten.Length; i++)
                        {
                            if (_ten[i] == "")
                                continue;
                            var kq = Add_CLB_DN_MH_HTX_THT(ten: _ten[i], loai: "01", hoiVien.IDCanBo, listCLB_DN_MH_HTX_THT_HoiVien: listCLB_DN_MH_HTX_THT_HoiVien,
                                listMasterCLB_DN_MH_HTX_THT: listMasterCLB_DN_MH_HTX_THT, addCLB_DN_MH_HTX_THTs: addCLB_DN_MH_HTX_THTs);
                            if (kq != null)
                            {
                                addCLB_DN_MH_HTX_THT_HoiViens.Add(kq);
                            }

                        }
                    }
                    if (!String.IsNullOrWhiteSpace(doiNhom))
                    {
                        String[] _ten = doiNhom.Split(';');
                        for (int i = 0; i < _ten.Length; i++)
                        {
                            if (_ten[i] == "")
                                continue;
                            var kq = Add_CLB_DN_MH_HTX_THT(ten: _ten[i], loai: "02", hoiVien.IDCanBo, listCLB_DN_MH_HTX_THT_HoiVien: listCLB_DN_MH_HTX_THT_HoiVien,
                                listMasterCLB_DN_MH_HTX_THT: listMasterCLB_DN_MH_HTX_THT, addCLB_DN_MH_HTX_THTs: addCLB_DN_MH_HTX_THTs);
                            if (kq != null)
                            {
                                addCLB_DN_MH_HTX_THT_HoiViens.Add(kq);
                            }

                        }
                    }
                    if (!String.IsNullOrWhiteSpace(moHinh))
                    {
                        String[] _ten = moHinh.Split(';');
                        for (int i = 0; i < _ten.Length; i++)
                        {
                            if (_ten[i] == "")
                                continue;
                            var kq = Add_CLB_DN_MH_HTX_THT(ten: _ten[i], loai: "03", hoiVien.IDCanBo, listCLB_DN_MH_HTX_THT_HoiVien: listCLB_DN_MH_HTX_THT_HoiVien,
                                listMasterCLB_DN_MH_HTX_THT: listMasterCLB_DN_MH_HTX_THT, addCLB_DN_MH_HTX_THTs: addCLB_DN_MH_HTX_THTs);
                            if (kq != null)
                            {
                                addCLB_DN_MH_HTX_THT_HoiViens.Add(kq);
                            }

                        }
                    }
                    if (!String.IsNullOrWhiteSpace(hopTacXa))
                    {
                        String[] _ten = hopTacXa.Split(';');
                        for (int i = 0; i < _ten.Length; i++)
                        {
                            if (_ten[i] == "")
                                continue;
                            var kq = Add_CLB_DN_MH_HTX_THT(ten: _ten[i], loai: "04", idHoiVien: hoiVien.IDCanBo, listCLB_DN_MH_HTX_THT_HoiVien: listCLB_DN_MH_HTX_THT_HoiVien,
                                listMasterCLB_DN_MH_HTX_THT: listMasterCLB_DN_MH_HTX_THT, addCLB_DN_MH_HTX_THTs: addCLB_DN_MH_HTX_THTs);
                            if (kq != null)
                            {
                                addCLB_DN_MH_HTX_THT_HoiViens.Add(kq);
                            }

                        }
                    }
                    if (!String.IsNullOrWhiteSpace(toHopTac))
                    {
                        String[] _ten = toHopTac.Split(';');
                        for (int i = 0; i < _ten.Length; i++)
                        {
                            if (_ten[i] == "")
                                continue;
                            var kq = Add_CLB_DN_MH_HTX_THT(ten: _ten[i], loai: "05", hoiVien.IDCanBo, listCLB_DN_MH_HTX_THT_HoiVien: listCLB_DN_MH_HTX_THT_HoiVien,
                                listMasterCLB_DN_MH_HTX_THT: listMasterCLB_DN_MH_HTX_THT, addCLB_DN_MH_HTX_THTs: addCLB_DN_MH_HTX_THTs);
                            if (kq != null)
                            {
                                addCLB_DN_MH_HTX_THT_HoiViens.Add(kq);
                            }

                        }
                    }

                }
            }
        }
        [NonAction]
        private CanBo? ChekcHoiVien (string? soCCCD)
        {
            var data = (from cb in _context.CanBos
                        join pv in _context.PhamVis on cb.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                        where pv.AccountId == AccountId()
                        && cb.IsHoiVien == true && cb.HoiVienDuyet == true
                        select cb).SingleOrDefault(it => it.IsHoiVien == true && it.SoCCCD == soCCCD && it.isRoiHoi != true && it.HoiVienDuyet == true);
            return data;
        }

        private CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien Add_CLB_DN_MH_HTX_THT(string ten, string loai, Guid idHoiVien, List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien> listCLB_DN_MH_HTX_THT_HoiVien,
            List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac> listMasterCLB_DN_MH_HTX_THT, List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac> addCLB_DN_MH_HTX_THTs)
        {
            var checkExist = listMasterCLB_DN_MH_HTX_THT.SingleOrDefault(it => it.Ten.ToLower() == ten.ToLower() && it.Loai == loai);
            Guid? idCauLacBo;
            CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien kq = new CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien();
            if (checkExist != null)
            {
                var exist = listCLB_DN_MH_HTX_THT_HoiVien.SingleOrDefault(it => it.IDHoiVien == idHoiVien && it.Id_CLB_DN_MH_HTX_THT == checkExist.Id_CLB_DN_MH_HTX_THT);
                if (exist != null)
                    return null!;
                idCauLacBo = checkExist!.Id_CLB_DN_MH_HTX_THT;

            }
            else {
                idCauLacBo = Guid.NewGuid();
                addCLB_DN_MH_HTX_THTs.Add(new CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac { 
                    Id_CLB_DN_MH_HTX_THT =idCauLacBo.Value,
                    Ten = ten,
                    CreatedAccountId = AccountId(),
                    CreatedTime = DateTime.Now,
                    Actived = true,
                    Loai = loai
                });
                listMasterCLB_DN_MH_HTX_THT.Add(new CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac
                {
                    Id_CLB_DN_MH_HTX_THT = idCauLacBo.Value,
                    Ten = ten,
                    Loai = loai
                });
                
            }
            kq.Id_CLB_DN_MH_HTX_THT = idCauLacBo.Value;
            kq.IDHoiVien = idHoiVien;
            kq.CreatedAccountId = AccountId();
            kq.CreatedTime = DateTime.Now;
            return kq;
        }
        #endregion Import

        #region Export 
        [HoiNongDanAuthorization]
        public IActionResult ExportCreate()
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, filemau);
            byte[] filecontent = ClassExportExcel.ExportFileMau(url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "CLB_DN_MH_HTX_THT");

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        [HoiNongDanAuthorization]
        public IActionResult ExportEdit(HoiVien_CLB_DN_MH_HTX_THTSearchVM search)
        {
            var data = (from thch_hv in _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens
                        join thc in _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs on thch_hv.Id_CLB_DN_MH_HTX_THT equals thc.Id_CLB_DN_MH_HTX_THT
                        join hv in _context.CanBos on thch_hv.IDHoiVien equals hv.IDCanBo
                        join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                        where pv.AccountId == AccountId()
                        && hv.IsHoiVien == true
                        select hv).Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen).ThenInclude(it => it.PhuongXas).Include(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens).ThenInclude(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac).AsQueryable();

            if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen))
            {
                data = data.Where(it => it.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
            }
            if (search.MaDiaBanHoiVien != null)
            {
                data = data.Where(it => it.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
            }
            if (search.Id_CLB_DN_MH_HTX_THT != null)
            {
                data = data.Where(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Any(it => it.Id_CLB_DN_MH_HTX_THT == search.Id_CLB_DN_MH_HTX_THT));
            }
            if (!String.IsNullOrWhiteSpace(search.HoVaTen))
            {
                data = data.Where(it => it.HoVaTen.Contains(search.HoVaTen));
            }
            if (!String.IsNullOrWhiteSpace(search.SoCCCD))
            {
                data = data.Where(it => it.SoCCCD == search.SoCCCD);
            }
            if (!String.IsNullOrWhiteSpace(search.Ten))
            {
                data = data.Where(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Any(p => p.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten.Contains(search.Ten)));
            }

            var model = data.Select((it) => new HoiVien_CLB_DN_MH_HTX_THTExcelVM
            {
                HoVaTen = it.HoVaTen,
                SoCCCD = it.SoCCCD,
                QuanHuyen = it.DiaBanHoatDong!.QuanHuyen.TenQuanHuyen,
                PhuongXa = it.DiaBanHoatDong!.TenDiaBanHoatDong,
                CauLacBo = String.Join(';', it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Where(p => p.IDHoiVien == it.IDCanBo && p.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "01").Select(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten).ToList()),
                DoiNhom = String.Join(';', it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Where(p => p.IDHoiVien == it.IDCanBo && p.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "02").Select(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten).ToList()),
                MoHinh = String.Join(';', it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Where(p => p.IDHoiVien == it.IDCanBo && p.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "03").Select(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten).ToList()),
                HopTacXa = String.Join(';', it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Where(p => p.IDHoiVien == it.IDCanBo && p.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "04").Select(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten).ToList()),
                TopHopTac = String.Join(';', it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Where(p => p.IDHoiVien == it.IDCanBo && p.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "05").Select(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten).ToList()),
                ChoOHienNay = it.ChoOHienNay,


                GhiChu = ""
            }).OrderBy(it => it.PhuongXa).ThenBy(it => it.HoVaTen).ToList();

            model = model.DistinctBy(it => new { it.HoVaTen, it.SoCCCD, it.ChoOHienNay, it.QuanHuyen, it.PhuongXa }).ToList();
            int stt = 1;
            model.ForEach(value =>
            {
                value.STT = stt;
                stt++;
            });
            if (model.Count > 0)
            {
                var add = new HoiVien_CLB_DN_MH_HTX_THTExcelVM();
                model.Add(add);
            }

            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, filemau);
            byte[] filecontent = ClassExportExcel.ExportExcel(model, startIndex+1, url);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", "CLB_DN_MH_HTX_THT_"+DateTime.Now.ToString("hh_mm_ss"));

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

        #region Helper
        [NonAction]
        private void CreateViewBagImport()
        {
            FnViewBag fnViewBag = new FnViewBag(_context);

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }
        [NonAction]
        private void CreateViewBagSearch()
        {
            var chinhTriHoiDoanKhacs = _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs.Where(it => it.Actived == true).Select(it => new { Id_CLB_DN_MH_HTX_THT = it.Id_CLB_DN_MH_HTX_THT, Ten = it.Ten }).ToList();
            ViewBag.Id_CLB_DN_MH_HTX_THT = new SelectList(chinhTriHoiDoanKhacs, "Id_CLB_DN_MH_HTX_THT", "Ten");

            FnViewBag fnViewBag = new FnViewBag(_context);

            ViewBag.MaDiaBanHoiVien = fnViewBag.DiaBanHoiVien(acID: AccountId());

            ViewBag.MaQuanHuyen = fnViewBag.QuanHuyen(idAc: AccountId());
        }
        private List<HoiVien_CLB_DN_MH_HTX_THTDetailVM> LoadData(HoiVien_CLB_DN_MH_HTX_THTSearchVM search) {
            var data = (from thch_hv in _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens
                        join thc in _context.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs on thch_hv.Id_CLB_DN_MH_HTX_THT equals thc.Id_CLB_DN_MH_HTX_THT
                        join hv in _context.CanBos on thch_hv.IDHoiVien equals hv.IDCanBo
                        join pv in _context.PhamVis on hv.MaDiaBanHoatDong equals pv.MaDiabanHoatDong
                        where pv.AccountId == AccountId()
                        && hv.IsHoiVien == true
                        select thch_hv).Include(it => it.HoiVien).Include(it => it.HoiVien.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen).ThenInclude(it => it.PhuongXas).Include(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac).AsQueryable();

            if (!String.IsNullOrWhiteSpace(search.MaQuanHuyen))
            {
                data = data.Where(it => it.HoiVien.DiaBanHoatDong!.MaQuanHuyen == search.MaQuanHuyen);
            }
            if (search.MaDiaBanHoiVien != null)
            {
                data = data.Where(it => it.HoiVien.MaDiaBanHoatDong == search.MaDiaBanHoiVien);
            }
            if (search.Id_CLB_DN_MH_HTX_THT != null)
            {
                data = data.Where(it => it.Id_CLB_DN_MH_HTX_THT == search.Id_CLB_DN_MH_HTX_THT);
            }
            if (!String.IsNullOrWhiteSpace(search.HoVaTen))
            {
                data = data.Where(it => it.HoiVien.HoVaTen.Contains(search.HoVaTen));
            }
            if (!String.IsNullOrWhiteSpace(search.SoCCCD))
            {
                data = data.Where(it => it.HoiVien.SoCCCD == search.SoCCCD);
            }
            if (!String.IsNullOrWhiteSpace(search.Loai))
            {
                data = data.Where(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == search.Loai);
            }
            var model = data.Select(it => new HoiVien_CLB_DN_MH_HTX_THTDetailVM
            {
                ID = it.IDHoiVien + "_" + it.Id_CLB_DN_MH_HTX_THT.ToString(),
                HoVaTen = it.HoiVien.HoVaTen,

                SoCCCD = it.HoiVien.SoCCCD,
                Ten = it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten,
                Loai = it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "01"?"Câu lạc bộ": it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "02" ? "Đội nhóm" :
                        it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "03" ? "Mô hình" : it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "04" ? "Hợp tác xã" : "Tổ hợp tác",
                ChoOHienNay = it.HoiVien.ChoOHienNay,
                PhuongXa = it.HoiVien.DiaBanHoatDong!.PhuongXa.TenPhuongXa,
                QuanHuyen = it.HoiVien.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                GhiChu = ""
            }).ToList();
            return model.OrderBy(it=>it.QuanHuyen).ThenBy(it=>it.PhuongXa).ThenBy(it=>it.SoCCCD).ToList();
        }
        #endregion Helper
    }
}
