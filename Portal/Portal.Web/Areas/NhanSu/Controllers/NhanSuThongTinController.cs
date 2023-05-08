using Microsoft.AspNetCore.Mvc;
using Portal.Constant;
using Portal.DataAccess;
using Portal.Extensions;
using Portal.Models;
using Portal.Models.Entitys;
using System.Data.Entity;

namespace Portal.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class NhanSuThongTinController : BaseController
    {
        public NhanSuThongTinController(AppDbContext context) : base(context) { }
        public IActionResult SearchNhanSu(string maNhanSu,bool canBo)
        {
            return ExecuteSearch(() => {
                NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
                nhanSu.CanBo = canBo;
                nhanSu.MaCanBo = maNhanSu;
                nhanSu.HinhAnh =  @"\images\login.png" ;
                if (canBo) {
                    var data = _context.CanBos.SingleOrDefault(it => it.MaCanBo == maNhanSu);
                    if (data == null)
                    {
                        nhanSu.Error = "Không tìm thấy nhân sự có mã " + maNhanSu;
                    }
                    else
                    {
                        var coSo = _context.CoSos.SingleOrDefault(it => it.IdCoSo == data.IdCoSo);
                        var donVi = _context.Departments.SingleOrDefault(it => it.Id == data.IdDepartment);
                        var tinhTrang = _context.TinhTrangs.SingleOrDefault(it => it.MaTinhTrang == data.MaTinhTrang);
                        var phanHe = _context.PhanHes.SingleOrDefault(it => it.MaPhanHe == data.MaPhanHe);
                        nhanSu.IdCanbo = data.IDCanBo;
                        nhanSu.HoVaTen = data.HoVaTen;
                        nhanSu.MaCanBo = data.MaCanBo;
                       
                        nhanSu.HinhAnh = data.HinhAnh == null ? @"\images\login.png" : data.HinhAnh;
                        nhanSu.TenCoSo = coSo != null? coSo.TenCoSo:"";
                        nhanSu.TenDonVi = donVi !=null?donVi.Name:"";
                        nhanSu.TenTinhTrang = tinhTrang != null?tinhTrang.TenTinhTrang:"";
                        nhanSu.TenPhanHe = phanHe != null? phanHe.TenPhanHe:"";
                    }
                }
                
                return PartialView("_NhanSuThongTin", nhanSu);
            });
        }
    }
}
