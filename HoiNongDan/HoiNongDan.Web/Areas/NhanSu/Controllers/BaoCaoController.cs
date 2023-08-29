using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class BaoCaoController : BaseController
    {
        public BaoCaoController(AppDbContext context) : base(context) { }
        //public IActionResult ThucLucHoi()
        //{
        //    var data = _context.ThucLucHois.ToList().OrderBy(x => x.Id).ToList();
        //    return View(data);
        //}
    }
}
