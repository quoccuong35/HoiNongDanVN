﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Portal.Constant;
using Portal.DataAccess;
using Portal.Extensions;
using Portal.Models;
using Portal.Models.ViewModels.Masterdata;
using Portal.Resources;
using Portal.Web.Areas.NhanSu.Models;
using System.Net.WebSockets;
using System.Transactions;

namespace Portal.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    [Authorize]
    public class BoNhiemController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public BoNhiemController(AppDbContext context, IWebHostEnvironment hostEnvironment) : base(context) {
            _hostEnvironment = hostEnvironment;
        }
        #region Index
        [PortalAuthorization]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult _Search(BoNhiemSearchVM search) {
            return ExecuteSearch(() => {
                var data = _context.QuaTrinhBoNhiems.AsQueryable();
                if (!String.IsNullOrEmpty(search.SoQuyetDinh) && !String.IsNullOrWhiteSpace(search.SoQuyetDinh)) {
                    data = data.Where(it => it.SoQuyetDinh == search.SoQuyetDinh);
                }
                data = data.Include(it => it.CanBo).Include(it => it.ChucVu).Include(it=>it.CoSo).Include(it=>it.Department);
                if (!String.IsNullOrEmpty(search.MaCanBo) && !String.IsNullOrWhiteSpace(search.MaCanBo))
                {
                    data = data.Where(it => it.CanBo.MaCanBo == search.MaCanBo);
                }
                if (!String.IsNullOrEmpty(search.HoVaTen) && !String.IsNullOrWhiteSpace(search.HoVaTen))
                {
                    data = data.Where(it => it.CanBo.HoVaTen.Contains(search.HoVaTen));
                }
                var model = data.Select(it => new BoNhiemDetailVM
                {
                    IdQuaTrinhBoNhiem = it.IdQuaTrinhBoNhiem,
                    MaCanBo = it.CanBo.MaCanBo,
                    HoVaTen = it.CanBo.HoVaTen,
                    SoQuyetDinh = it.SoQuyetDinh,
                    NgayQuyetDinh = it.NgayQuyetDinh,
                    NguoiKy = it.NguoiKy,
                    HeSoChucVu = it.HeSoChucVu,
                    GhiChu = it.GhiChu,
                    TenChucVu = it.ChucVu.TenChucVu,
                    TenDonVi = it.Department.Name,
                    TenCoSo = it.CoSo.TenCoSo
                }).ToList();
                return PartialView(model);
            });
        }
        #endregion Index
        #region Create
        [HttpGet]
        [PortalAuthorization]
        public IActionResult Create() {

            BoNhiemVM boNhiem = new BoNhiemVM();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu.CanBo = true;
            boNhiem.NhanSu = nhanSu;
            CreateViewBag();
            return View(boNhiem);
        }
        [HttpPost]
        [PortalAuthorization]
        public JsonResult Create(BoNhiemMTVM obj, IFormFile? fileInbox) {
            CheckError(obj);
            var add = new QuaTrinhBoNhiem();
            add = obj.GetBoNhiem(add);
            add.IdQuaTrinhBoNhiem = Guid.NewGuid();
            FileDinhKemModel addFile = new FileDinhKemModel();
            if (fileInbox != null)
            {
                addFile.Id = add.IdQuaTrinhBoNhiem;
                addFile.IdCanBo = add.IDCanBo;
                addFile.IDLoaiDinhKem = "01";
                FunctionFile.CopyFile(_hostEnvironment, fileInbox, addFile);
                if (!String.IsNullOrEmpty(addFile.Error) && !String.IsNullOrWhiteSpace(addFile.Error))
                {
                    ModelState.AddModelError("fileInbox", "Lỗi không cập nhật được file đính kèm");
                }
            }
            return ExecuteContainer(() => {
                add.CreatedTime = DateTime.Now;
                add.CreatedAccountId = AccountId();
                var canBo = _context.CanBos.SingleOrDefault(it => it.IDCanBo == add.IDCanBo);
                add.IdCoSoCu = canBo!.IdCoSo;
                add.IdDepartmentCu = canBo.IdDepartment;
                add.MaChucVuCu = canBo.MaChucVu;
                //_context.Attach(add).State = EntityState.Modified;
                if (!String.IsNullOrEmpty(addFile.Url) && !String.IsNullOrWhiteSpace(addFile.Url))
                {
                    FileDinhKem fileDinhKem = addFile.GetFileDinhKem();
                    _context.FileDinhKems.Add(fileDinhKem);
                }
                _context.QuaTrinhBoNhiems.Add(add);
              
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.BoNhiem.ToLower())
                });
            });
        }
        #endregion Create
        #region Edit 
        [HttpGet]
        [PortalAuthorization]
        public IActionResult Edit(Guid id) {
            var item = _context.QuaTrinhBoNhiems.SingleOrDefault(it => it.IdQuaTrinhBoNhiem == id);
            if (item == null)
            {
                return Redirect("~/Error/ErrorNotFound?data=" + id);
            }
            BoNhiemVM obj = new BoNhiemVM();
            var file = _context.FileDinhKems.SingleOrDefault(it=>it.Id == id);

            var canBo = _context.CanBos.Include(it => it.CoSo).Include(it => it.Department)
                        .Include(it => it.PhanHe).Include(it => it.TinhTrang).Where(it => it.IDCanBo == item.IDCanBo).SingleOrDefault();
            NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
            nhanSu = nhanSu.GeThongTin(canBo);
            nhanSu.CanBo = true;
            nhanSu.IdCanbo = canBo!.IDCanBo;
            nhanSu.HoVaTen = canBo.HoVaTen;
            nhanSu.MaCanBo = canBo.MaCanBo;
            nhanSu.TenTinhTrang = canBo.TinhTrang.TenTinhTrang;
            nhanSu.TenCoSo = canBo.CoSo.TenCoSo;
            nhanSu.TenDonVi = canBo.Department.Name;
            nhanSu.TenPhanHe = canBo.PhanHe.TenPhanHe;
            nhanSu.Edit = false;

            obj.IdCoSo = item.IdCoSo;
            obj.IdDepartment = item.IdDepartment;
            obj.MaChucVu = item.MaChucVu;
            obj.NgayQuyetDinh = item.NgayQuyetDinh;
            obj.SoQuyetDinh = item.SoQuyetDinh;
            obj.HeSoChucVu = item.HeSoChucVu;
            obj.NguoiKy = item.NguoiKy;
            obj.GhiChu = item.GhiChu;
            obj.NhanSu = nhanSu;
            obj.IdQuaTrinhBoNhiem = item.IdQuaTrinhBoNhiem;
            obj.FileDinhKem = file;

            

            CreateViewBag(item.MaChucVu,item.IdCoSo,item.IdDepartment);
            return View(obj);
        }
        [PortalAuthorization]
        [HttpPost]
        public JsonResult Edit(BoNhiemMTVM obj,IFormFile? fileInbox) {
            CheckError(obj);
            return ExecuteContainer(() => {
                var edit = _context.QuaTrinhBoNhiems.SingleOrDefault(it => it.IdQuaTrinhBoNhiem == obj.IdQuaTrinhBoNhiem);
                if (edit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Error_NotExist, LanguageResource.BoNhiem.ToLower())
                    });
                }
                else
                {
                    using (TransactionScope tran = new TransactionScope())
                    {
                        edit = obj.GetBoNhiem(edit);
                        edit.LastModifiedTime = DateTime.Now;
                        edit.LastModifiedAccountId = AccountId();
                        FileDinhKemModel addFile = new FileDinhKemModel();
                        if (fileInbox != null)
                        {
                            var existFile = _context.FileDinhKems.SingleOrDefault(it => it.Id == edit.IdQuaTrinhBoNhiem);
                            if (existFile != null)
                            {
                                FunctionFile.Delete(_hostEnvironment, existFile.Url);
                                _context.FileDinhKems.Remove(existFile);
                            }
                            addFile.Id = edit.IdQuaTrinhBoNhiem;
                            addFile.IdCanBo = edit.IDCanBo;
                            addFile.IDLoaiDinhKem = "01";
                            FunctionFile.CopyFile(_hostEnvironment, fileInbox, addFile);
                            if (!String.IsNullOrEmpty(addFile.Error) && !String.IsNullOrWhiteSpace(addFile.Error))
                            {
                                ModelState.AddModelError("fileInbox", "Lỗi không cập nhật được file đính kèm");
                            }
                        }
                        if (!String.IsNullOrEmpty(addFile.Url) && !String.IsNullOrWhiteSpace(addFile.Url))
                        {
                            FileDinhKem fileDinhKem = addFile.GetFileDinhKem();
                            _context.FileDinhKems.Add(fileDinhKem);
                        }
                        //var canBo = _context.CanBos.SingleOrDefault(it => it.IDCanBo == edit.IDCanBo);
                        //edit.IdCoSoCu = canBo!.IdCoSo;
                        //edit.IdDepartmentCu = canBo.IdDepartment;
                        //edit.MaChucVuCu = canBo.MaChucVu;
                        _context.Entry(edit).State = EntityState.Modified;
                        _context.SaveChanges();
                        tran.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.BoNhiem.ToLower())
                        });
                    }
                }
            });
        }
        #endregion Edit
        #region Delete
        [HttpDelete]
        public JsonResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                using (TransactionScope tran = new TransactionScope())
                {
                    var del = _context.QuaTrinhBoNhiems.FirstOrDefault(p => p.IdQuaTrinhBoNhiem == id);
                    if (del != null)
                    {
                        var existFile = _context.FileDinhKems.SingleOrDefault(it => it.Id == id);
                        if (existFile != null)
                        {
                            FunctionFile.Delete(_hostEnvironment, existFile.Url);
                            _context.FileDinhKems.Remove(existFile);
                        }
                        _context.Remove(del);
                        _context.SaveChanges();
                        tran.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.BoNhiem.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.BoNhiem.ToLower())
                        });
                    }
                }
                
            });
        }
        #endregion Delete
        #region Helper
        private void CheckError(BoNhiemMTVM obj) {
            if (obj.NhanSu.IdCanbo == null)
            {
                ModelState.AddModelError("MaCanBo", "Chưa chọn cán bộ");
            }
            var exist = _context.Departments.SingleOrDefault(it => it.Id == obj.IdDepartment && it.IDCoSo == obj.IdCoSo);
            if (exist == null)
            {
                ModelState.AddModelError("IdDepartment", "Đơn vị không đúng với cơ sở");
            }
        }
        private void CreateViewBag(Guid? MaChucVu = null,Guid? IdCoSo = null, Guid? IdDepartment = null)
        {
            var MenuList1 = _context.ChucVus.Where(it=>it.Actived == true).Select(it => new { MaChucVu = it.MaChucVu, TenChucVu = it.TenChucVu }).ToList();
            ViewBag.MaChucVu = new SelectList(MenuList1, "MaChucVu", "TenChucVu", MaChucVu);
            
            var MenuList2 = _context.CoSos.Where(it => it.Actived == true).Select(it => new { IdCoSo = it.IdCoSo, TenCoSo = it.TenCoSo }).ToList();
            ViewBag.IdCoSo = new SelectList(MenuList2, "IdCoSo", "TenCoSo", IdCoSo);

            var MenuList3 = _context.Departments.Where( it=>it.Actived == true && (it.IDCoSo == IdCoSo || IdCoSo ==null)).Select(it => new { IdDepartment = it.Id, TenDonVi = it.Name }).ToList();
            ViewBag.IdDepartment = new SelectList(MenuList3, "IdDepartment", "TenDonVi", IdDepartment);


        }
        #endregion Helper
    }
}
