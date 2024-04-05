using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models.ViewModels.Masterdata;
using HoiNongDan.Resources;
using HoiNongDan.Web.Areas.NhanSu.Models;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class FileDinhKemController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public FileDinhKemController(AppDbContext context, IWebHostEnvironment hostEnvironment) : base(context) {
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFile(Guid id) {
            return ExecuteDelete(() => {
                var del = _context.FileDinhKems.SingleOrDefault(it => it.Key == id);
                if (del != null)
                {
                    if (FunctionFile.Delete(_hostEnvironment, del.Url))
                    {
                        _context.Remove(del);
                        _context.SaveChanges();

                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.FileDinhKem.ToLower())
                        });
                    }
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.FileDinhKem.ToLower())
                });

            });
        }
    }
}
