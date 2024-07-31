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
                if (!String.IsNullOrWhiteSpace(search.MaHoiVien))
                {
                    data = data.Where(it => it.HoiVien.MaCanBo == search.MaHoiVien);
                }
                if (!String.IsNullOrWhiteSpace(search.Loai))
                {
                    data = data.Where(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == search.Loai);
                }
                var model = data.Select(it => new HoiVien_CLB_DN_MH_HTX_THTDetailVM
                {
                    ID = it.IDHoiVien + "_" + it.Id_CLB_DN_MH_HTX_THT.ToString(),
                    HoVaTen = it.HoiVien.HoVaTen,
                    MaCanBo = it.HoiVien.MaCanBo,
                    SoCCCD = it.HoiVien.SoCCCD,
                    CauLacBo = it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "01" ? it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten : "",
                    DoiNhom = it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "02" ? it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten : "",
                    MoHinh = it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "03" ? it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten : "",
                    HopTacXa = it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "04" ? it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten : "",
                    TopHopTac = it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "05" ? it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten : "",
                    ChoOHienNay = it.HoiVien.ChoOHienNay,
                    PhuongXa = it.HoiVien.DiaBanHoatDong!.PhuongXa.TenPhuongXa,
                    QuanHuyen = it.HoiVien.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                    GhiChu = ""
                });
                 
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
        public IActionResult Import(Guid? MaDiaBanHoiVien, String? MaQuanHuyen) {
            if (String.IsNullOrWhiteSpace(MaQuanHuyen))
            {
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
                            if (dt.Rows.IndexOf(row) >= startIndex)
                            {
                                if (row[0] == null || row[0].ToString() == "")
                                    break;
                                CheckTemplate(row, listMasterCLB_DN_MH_HTX_THT: listMasterCLB_DN_MH_HTX_THT, addCLB_DN_MH_HTX_THTs: addCLB_DN_MH_HTX_THTs, listCLB_DN_MH_HTX_THT_HoiVien: listCLB_DN_MH_HTX_THT_HoiVien,
                               addCLB_DN_MH_HTX_THT_HoiViens: addCLB_DN_MH_HTX_THT_HoiViens, hoiViens: dataHoiViens, error: errorList);
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


                        _context.SaveChanges();
                        ts.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = LanguageResource.ImportSuccess
                        });
                        return Json("");
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
            List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien> listCLB_DN_MH_HTX_THT_HoiVien, List<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien> addCLB_DN_MH_HTX_THT_HoiViens, List<CanBo> hoiViens, List<String> error) 
        {
            string? index = "", hoVaTen = "", maHoiVien = "", soCCCD = "", cauLacBo, doiNhom = "", moHinh = "", hopTacXa = "", toHopTac = "",phuong = "";
            index = row[0].ToString();

            hoVaTen = row[1].ToString();
            maHoiVien = row[2] != null ? row[2].ToString()!.Trim() : "";
            soCCCD = row[3] != null ? row[3].ToString()!.Trim() : "";
            cauLacBo = row[4] != null ? row[4].ToString()!.Trim() : "";

            doiNhom = row[5] != null ? row[5].ToString()!.Trim() : "";
            moHinh = row[6] != null ? row[6].ToString()!.Trim() : "";
            hopTacXa = row[7] != null ? row[7].ToString()!.Trim() : "";
            toHopTac = row[8] != null ? row[8].ToString()!.Trim() : "";
            phuong = row[10] != null ? row[10].ToString()!.Trim() : "";

            if (String.IsNullOrWhiteSpace(cauLacBo) && String.IsNullOrWhiteSpace(doiNhom) && String.IsNullOrWhiteSpace(moHinh) &&
                String.IsNullOrWhiteSpace(hopTacXa) && String.IsNullOrWhiteSpace(toHopTac))
            {
                error.Add(String.Format(LanguageResource.CLB_DN_MH_HTX_THTError1, index + " " + hoVaTen));
            }
            else {
                var hoiVien = ChekcHoiVien(maHoiVien: maHoiVien, hoVaTen: hoVaTen, soCCCD: soCCCD, phuong: phuong, hoiViens: hoiViens);
                if (hoiVien.Count() == 0)
                {
                    error.Add(String.Format(LanguageResource.ErrorImportChiToHoiNganhNghe2, index + " " + hoVaTen));
                }
                if (hoiVien.Count > 1)
                {
                    error.Add(String.Format(LanguageResource.ErrorImportChiToHoiNganhNghe3, index + " " + hoVaTen));
                }
                if (hoiVien.Count == 1) {
                    if (!String.IsNullOrWhiteSpace(cauLacBo)) {
                        String[] _ten = cauLacBo.Split(';');
                        for (int i = 0; i < _ten.Length; i++)
                        {
                            if (_ten[i] == "")
                                continue;
                            var kq = Add_CLB_DN_MH_HTX_THT(ten: _ten[i], loai: "01", idHoiVien: hoiVien.First().IDCanBo, listCLB_DN_MH_HTX_THT_HoiVien: listCLB_DN_MH_HTX_THT_HoiVien,
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
                            var kq = Add_CLB_DN_MH_HTX_THT(ten: _ten[i], loai: "02", idHoiVien: hoiVien.First().IDCanBo, listCLB_DN_MH_HTX_THT_HoiVien: listCLB_DN_MH_HTX_THT_HoiVien,
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
                            var kq = Add_CLB_DN_MH_HTX_THT(ten: _ten[i], loai: "03", idHoiVien: hoiVien.First().IDCanBo, listCLB_DN_MH_HTX_THT_HoiVien: listCLB_DN_MH_HTX_THT_HoiVien,
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
                            var kq = Add_CLB_DN_MH_HTX_THT(ten: _ten[i], loai: "04", idHoiVien: hoiVien.First().IDCanBo, listCLB_DN_MH_HTX_THT_HoiVien: listCLB_DN_MH_HTX_THT_HoiVien,
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
                            var kq = Add_CLB_DN_MH_HTX_THT(ten: _ten[i], loai: "05", idHoiVien: hoiVien.First().IDCanBo, listCLB_DN_MH_HTX_THT_HoiVien: listCLB_DN_MH_HTX_THT_HoiVien,
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
        private List<CanBo> ChekcHoiVien(string? maHoiVien, string? hoVaTen, string? soCCCD, string? phuong, List<CanBo> hoiViens)
        {
            var data = new List<CanBo>();
            if (!String.IsNullOrWhiteSpace(hoVaTen))
            {
                data = hoiViens.Where(it => it.HoVaTen.Trim().ToLower() == hoVaTen.Trim().ToLower()).ToList();
                if (data.Count == 1)
                {
                    return data;
                }
                else if (data.Count > 1)
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
            var url = Path.Combine(wwwRootPath, @"upload\filemau\CLB_DN_MH_HTX_THT.xlsx");
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
                        select hv).Include(it => it.DiaBanHoatDong).ThenInclude(it => it!.QuanHuyen).ThenInclude(it => it.PhuongXas).Include(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens).ThenInclude(it=>it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac).AsQueryable();

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
                data = data.Where(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Any(it=>it.Id_CLB_DN_MH_HTX_THT == search.Id_CLB_DN_MH_HTX_THT) );
            }
            if (!String.IsNullOrWhiteSpace(search.HoVaTen))
            {
                data = data.Where(it => it.HoVaTen.Contains(search.HoVaTen));
            }
            if (!String.IsNullOrWhiteSpace(search.MaHoiVien))
            {
                data = data.Where(it => it.MaCanBo == search.MaHoiVien);
            }

            var model = data.Select((it) => new HoiVien_CLB_DN_MH_HTX_THTExcelVM
            {
                HoVaTen = it.HoVaTen,
                MaCanBo = it.MaCanBo,
                SoCCCD = it.SoCCCD,
                CauLacBo = String.Join(';', it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Where(p => p.IDHoiVien == it.IDCanBo && p.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "01").Select(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten).ToList()),
                DoiNhom = String.Join(';', it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Where(p => p.IDHoiVien == it.IDCanBo && p.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "02").Select(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten).ToList()),
                MoHinh = String.Join(';', it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Where(p => p.IDHoiVien == it.IDCanBo && p.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "03").Select(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten).ToList()),
                HopTacXa = String.Join(';', it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Where(p => p.IDHoiVien == it.IDCanBo && p.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "04").Select(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten).ToList()),
                TopHopTac = String.Join(';', it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens.Where(p => p.IDHoiVien == it.IDCanBo && p.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Loai == "05").Select(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac.Ten).ToList()),
                ChoOHienNay = it.ChoOHienNay,
                PhuongXa = it.DiaBanHoatDong!.PhuongXa.TenPhuongXa,
                QuanHuyen = it.DiaBanHoatDong.QuanHuyen.TenQuanHuyen,
                GhiChu = ""
            }).OrderBy(it => it.PhuongXa).ThenBy(it => it.HoVaTen).ToList();

            model = model.DistinctBy(it => new { it.HoVaTen, it.MaCanBo, it.SoCCCD, it.ChoOHienNay,it.QuanHuyen,it.PhuongXa }).ToList();
            int stt = 1;
            model.ForEach(value => {
                value.STT = stt;
                stt++;
            });

            string wwwRootPath = _hostEnvironment.WebRootPath;
            var url = Path.Combine(wwwRootPath, @"upload\filemau\CLB_DN_MH_HTX_THT.xlsx");
            byte[] filecontent = ClassExportExcel.ExportExcel(model, startIndex, url);
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
        #endregion Helper
    }
}
