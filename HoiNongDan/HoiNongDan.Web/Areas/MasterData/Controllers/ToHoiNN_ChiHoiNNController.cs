using HoiNongDan.DataAccess;
using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Extensions;
using HoiNongDan.Resources;
using Microsoft.EntityFrameworkCore;
using HoiNongDan.Models;
using HoiNongDan.Constant;
using Microsoft.Extensions.Hosting;
using HoiNongDan.Models.ViewModels.Masterdata;
using HoiNongDan.Web.Areas.NhanSu.Models;
using System.IO;

namespace HoiNongDan.Web.Areas.MasterData.Controllers
{
    [Area(ConstArea.MasterData)]
    public class ToHoiNN_ChiHoiNNController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private string _tenThuMuc = "TH_CH_NN";

        public ToHoiNN_ChiHoiNNController(AppDbContext context, IWebHostEnvironment hostEnvironment) : base (context) {
            _hostEnvironment = hostEnvironment;
        }
        #region Index
        public IActionResult Index()
        {
            ToHoiNganhNghe_ChiHoiNganhNgheSearchVM model = new ToHoiNganhNghe_ChiHoiNganhNgheSearchVM();

            return View(model);
        }
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult _Search(ToHoiNganhNghe_ChiHoiNganhNgheSearchVM search)
        {
            return ExecuteSearch(() => {
                var data = _context.ToHoiNganhNghe_ChiHoiNganhNghes.Include(it=>it.ToHoiNganhNghe_ChiHoiNganhNghe_HVs).AsQueryable();
                if (!String.IsNullOrWhiteSpace(search.Ten))
                {
                    data = data.Where(it => it.Ten.Contains(search.Ten));
                }
                if (search.Actived != null)
                {
                    data = data.Where(it => it.Actived == search.Actived);
                }
                //if (search.TuNgay != null)
                //{
                //    data = data.Where(it => it.NgayThanhLap >= search.TuNgay);
                //}
                //if (search.DenNgay != null)
                //{
                //    data = data.Where(it => it.NgayThanhLap < search.DenNgay);
                //}
                if (!String.IsNullOrWhiteSpace(search.Loai))
                {
                    data = data.Where(it => it.Loai == search.Loai);
                }
                var fileDinhKems = _context.FileDinhKems.Where(it => it.IDLoaiDinhKem == "90").ToList();
                var model = data.Select(it => new ToHoiNganhNghe_ChiHoiNganhNgheVM
                {
                    Ma_ToHoiNganhNghe_ChiHoiNganhNghe = it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe,
                    Actived = it.Actived,
                    Ten = it.Ten,
                    Loai = it.Loai=="01"?"Chi hội":"Tổ hội",
                    Description = it.Description,
                    OrderIndex = it.OrderIndex,
                    SoHoiVien = it.ToHoiNganhNghe_ChiHoiNganhNghe_HVs.Count(),
                    NgayThanhLap = it.NgayThanhLap
                }).ToList();
                foreach (var item in model)
                {
                    var checkExistFile = fileDinhKems.SingleOrDefault(it => it.Id == item.Ma_ToHoiNganhNghe_ChiHoiNganhNghe);
                    if (checkExistFile != null)
                    {
                        item.Url = @"\"+checkExistFile.Url;
                    }
                }
                return PartialView(model);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Create()
        {
            ToHoiNganhNghe_ChiHoiNganhNgheVM model = new ToHoiNganhNghe_ChiHoiNganhNgheVM();
            return View(model);
        }
        
        [HoiNongDanAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ToHoiNganhNghe_ChiHoiNganhNgheVM obj, IFormFile? fileInbox)
        {

            return ExecuteContainer(() =>
            {
                ToHoiNganhNghe_ChiHoiNganhNghe add= new ToHoiNganhNghe_ChiHoiNganhNghe();
                add.Ma_ToHoiNganhNghe_ChiHoiNganhNghe = Guid.NewGuid();
                add.Ten = obj.Ten;
                add.Loai = obj.Loai;
                add.NgayThanhLap = obj.NgayThanhLap;
                add.Actived = true;
                add.Description = obj.Description;
                add.OrderIndex = obj.OrderIndex;
                add.CreatedAccountId = AccountId();
                add.CreatedTime = DateTime.Now;
                add.Actived = true;
                FileDinhKemModel addFile = new FileDinhKemModel();
                if (fileInbox != null)
                {
                    addFile.Id  = add.Ma_ToHoiNganhNghe_ChiHoiNganhNghe;
                    addFile.IDLoaiDinhKem = "90";
                    FunctionFile.CopyFile(_hostEnvironment, fileInbox, addFile, TenThuMuc: _tenThuMuc +@"\" + add.Ma_ToHoiNganhNghe_ChiHoiNganhNghe.ToString());
                    if (!String.IsNullOrEmpty(addFile.Error) && !String.IsNullOrWhiteSpace(addFile.Error))
                    {
                        ModelState.AddModelError("fileInbox", addFile.Error);
                    }
                    _context.FileDinhKems.Add(addFile);
                }
               
                _context.Attach(add).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.ToHoiNganhNghe_ChiHoiNganhNghes.Add(add);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.ToHoiNN_ChiHoiNN.ToLower())
                });

            });
        }
        #endregion Create
        #region Edit
        [HttpGet]
        [HoiNongDanAuthorization]
        public IActionResult Edit(Guid id)
        {
            var edit = _context.ToHoiNganhNghe_ChiHoiNganhNghes.SingleOrDefault(it => it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe == id);
            if (edit == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            var file = _context.FileDinhKems.SingleOrDefault(it => it.Id == id);

            var model = new ToHoiNganhNghe_ChiHoiNganhNgheVM
            {
                Ma_ToHoiNganhNghe_ChiHoiNganhNghe = edit.Ma_ToHoiNganhNghe_ChiHoiNganhNghe,
                Ten = edit.Ten,
                Actived = edit.Actived,
                NgayThanhLap = edit.NgayThanhLap,
                OrderIndex = edit.OrderIndex,
                Loai = edit.Loai,
                Description = edit.Description,
                FileDinhKem = file,
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HoiNongDanAuthorization]
        public IActionResult Edit(ToHoiNganhNghe_ChiHoiNganhNgheVM obj, IFormFile? fileInbox)
        {
            return ExecuteContainer(() =>
            {
                var edit = _context.ToHoiNganhNghe_ChiHoiNganhNghes.SingleOrDefault(it => it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe == obj.Ma_ToHoiNganhNghe_ChiHoiNganhNghe);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.ToHoiNN_ChiHoiNN.ToLower())
                    });
                }
                else
                {

                    edit.Ten = obj.Ten;
                    edit.Actived = obj.Actived;
                    edit.Loai = obj.Loai;
                    edit.NgayThanhLap = obj.NgayThanhLap;
                    edit.OrderIndex = obj.OrderIndex;
                    edit.Description = obj.Description;
                    edit.LastModifiedAccountId = AccountId();
                    edit.LastModifiedTime = DateTime.Now;
                    FileDinhKemModel addFile = new FileDinhKemModel();
                    if (fileInbox != null)
                    {
                       
                        var existFile = _context.FileDinhKems.SingleOrDefault(it => it.Id == edit.Ma_ToHoiNganhNghe_ChiHoiNganhNghe);
                        if (existFile != null)
                        {
                            FunctionFile.Delete(_hostEnvironment, existFile.Url);
                            _context.FileDinhKems.Remove(existFile);
                        }
                        addFile.Id = edit.Ma_ToHoiNganhNghe_ChiHoiNganhNghe;
                        addFile.IDLoaiDinhKem = "90";
                        FunctionFile.CopyFile(_hostEnvironment, fileInbox, addFile,TenThuMuc: _tenThuMuc+ @"\" + edit.Ma_ToHoiNganhNghe_ChiHoiNganhNghe.ToString());
                        if (!String.IsNullOrEmpty(addFile.Error) && !String.IsNullOrWhiteSpace(addFile.Error))
                        {
                            ModelState.AddModelError("fileInbox", addFile.Error);
                        }
                        _context.FileDinhKems.Add(addFile);
                    }
                    _context.Entry(edit).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.ToHoiNN_ChiHoiNN.ToLower())
                    });
                }
            });
        }
        #endregion Edit
        #region Delete
        [HttpDelete]
        [HoiNongDanAuthorization]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var del = _context.ToHoiNganhNghe_ChiHoiNganhNghes.SingleOrDefault(it => it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe == id);
                if (del != null)
                {
                    var checkFile = _context.FileDinhKems.SingleOrDefault(it => it.Id == del.Ma_ToHoiNganhNghe_ChiHoiNganhNghe);
                    if (checkFile != null)
                    {

                        if (FunctionFile.Delete(_hostEnvironment, checkFile.Url))
                        {
                            _context.Remove(checkFile);
                        }
                        FunctionFile.DeleteFolder(_hostEnvironment, url: _tenThuMuc + @"\" + del.Ma_ToHoiNganhNghe_ChiHoiNganhNghe.ToString());
                    }
                    
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
