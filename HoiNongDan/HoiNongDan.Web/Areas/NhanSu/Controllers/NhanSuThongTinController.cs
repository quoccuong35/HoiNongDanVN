using Microsoft.AspNetCore.Mvc;
using HoiNongDan.Constant;
using HoiNongDan.DataAccess;
using HoiNongDan.Extensions;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using System.Data.Entity;

namespace HoiNongDan.Web.Areas.NhanSu.Controllers
{
    [Area(ConstArea.NhanSu)]
    public class NhanSuThongTinController : BaseController
    {
        public NhanSuThongTinController(AppDbContext context) : base(context) { }
        public IActionResult SearchNhanSu(string maNhanSu,bool canBo)
        {
            return ExecuteSearch(() => {
                NhanSuThongTinVM nhanSu = new NhanSuThongTinVM();
                try
                {
                    
                    nhanSu.CanBo = canBo;
                    nhanSu.MaCanBo = maNhanSu;
                    nhanSu.HinhAnh = @"\images\login.png";
                    var data = _context.CanBos.SingleOrDefault(it => it.MaCanBo == maNhanSu && it.IsCanBo == true);
                    if (data == null)
                    {
                        nhanSu.Error = "Không tìm thấy nhân sự có mã " + maNhanSu;
                    }
                    else
                    {
                        var donVi = _context.Departments.SingleOrDefault(it => it.Id == data.IdDepartment);
                        var chucvu = _context.ChucVus.SingleOrDefault(it => it.MaChucVu == data.MaChucVu);
                        nhanSu.IdCanbo = data.IDCanBo;
                        nhanSu.HoVaTen = data.HoVaTen;
                        nhanSu.MaCanBo = data.MaCanBo;
                        nhanSu.NgaySinh = data.NgaySinh;
                        nhanSu.ChuyenNganh = data.ChuyenNganh;

                        nhanSu.TenDonVi = donVi != null ? donVi.Name : "";
                        nhanSu.ChucVu = chucvu.TenChucVu;
                    }

                    return PartialView("_NhanSuThongTin", nhanSu);
                }
                catch (Exception ex)
                {
                    nhanSu.Error = ex.Message + maNhanSu;
                    return PartialView("_NhanSuThongTin", nhanSu);
                }
            });
        }
    }
}
