using HoiNongDan.DataAccess;
using HoiNongDan.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace HoiNongDan.Extensions
{
    public static class Function
    {
        public static bool GetPermission(String listRoless, String Roles)
        {
            if (listRoless.ToUpper().Contains(Roles.ToUpper().Trim()))
            {
                return true;
            }
            else
            {
                return false; ;
            }
        }

        public static DateTime ConvertStringToDate(string value) {
            try
            {
                if(!String.IsNullOrWhiteSpace(value))
                {
                    value = TruncatePercents(value.ToUpper()).Replace(@"\", "/");
                    var arraydate = value.Split('/');
                    int day = 0, month = 0, year = 0;
                    if (arraydate.Length == 3)
                    {
                        day = int.Parse(arraydate[0]);
                        month = int.Parse(arraydate[1]);
                        year = int.Parse(arraydate[2]);
                    }
                    else if (arraydate.Length == 2)
                    {
                        day = 1;
                        month = int.Parse(arraydate[0]);
                        year = int.Parse(arraydate[1]);
                    }
                    else if (arraydate.Length == 1)
                    {
                        month = day = 1;
                        year = int.Parse(arraydate[0]);
                    }
                    if (year > 0 && month > 0 && day > 0)
                    {
                        return new DateTime(year, month, day);
                    }
                }
                
               return new DateTime(1900, 1, 1);
            }
            catch (Exception ex)
            {
                return new DateTime(1900, 1, 1); ;
            }
          
        }
        private static string TruncatePercents(string input)
        {
            return Regex.Replace(input, @"(\W)+", "$1");
        }

        public static HoiVienInfo GetThongTinHoiVien(Guid maHoiVien, AppDbContext _context) {
            HoiVienInfo HoiVien = new HoiVienInfo();
            var data = (from hv in _context.CanBos
                        join vp in _context.PhamVis on hv.MaDiaBanHoatDong equals vp.MaDiabanHoatDong
                        where hv.IDCanBo == maHoiVien
                        select hv).Include(it=>it.DiaBanHoatDong).ThenInclude(it=>it.QuanHuyen).FirstOrDefault();
            //var diaBan = _context.DiaBanHoatDongs.SingleOrDefault(it => it.Id == data!.MaDiaBanHoatDong);
            //var quanThanhPho = _context.QuanHuyens.SingleOrDefault(it => it.MaQuanHuyen == diaBan!.MaQuanHuyen);
            HoiVien.IdCanbo = data!.IDCanBo;
            HoiVien.HoVaTen = data.HoVaTen;
            HoiVien.MaCanBo = data.MaCanBo!;
            HoiVien.DiaBan = data.DiaBanHoatDong!.TenDiaBanHoatDong;
            HoiVien.NgaySinh = data!.NgaySinh;
            HoiVien.HoKhauThuongTru = data.HoKhauThuongTru;
            HoiVien.SoCCCD = data.SoCCCD;
            HoiVien.QuanHuyen = data.DiaBanHoatDong.QuanHuyen!.TenQuanHuyen;
            HoiVien.Edit = false;
            return HoiVien;
        }
    }
}
