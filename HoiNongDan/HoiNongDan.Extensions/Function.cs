﻿using HoiNongDan.DataAccess;
using HoiNongDan.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
                return false;
            }
        }

        public static DateTime ConvertStringToDate(string value) {
            try
            {
                if(!String.IsNullOrWhiteSpace(value))
                {
                    value = RepleceAllString(value);
                    value = TruncatePercents(value.ToUpper()).Replace(@"\", "/").Replace("Y","");
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
                    if (year > 0 && year <= 50)
                        year = year + 2000;
                    if (year > DateTime.Now.Year)
                        year = 0;
                    if (year > 0 && month > 0 && day > 0)
                    {
                        return new DateTime(year, month, day);
                    }
                }
                
               return new DateTime(1900, 1, 1);
            }
            catch
            {
                return new DateTime(1900, 1, 1);
            }
          
        }
        public static DateTime? ConvertStringToDateV1(string value)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    value = RepleceAllString(value);
                    value = TruncatePercents(value.ToUpper()).Replace(@"\", "/").Replace(".","/").Replace("-", "/").Replace("Y", "").Replace("y", "");
                    var arraydate = value.Split('/');
                    int day = 0, month = 0, year = 0, temp = 0;
                    if (arraydate.Length == 4)
                    {
                        day = int.Parse(arraydate[1]);
                        month = int.Parse(arraydate[2]);
                        year = int.Parse(arraydate[3]);
                    }
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
                            if(arraydate[0].Length == 4)
                            {
                                month = day = 1;
                                year = int.Parse(arraydate[0]);
                            }
                    }
                    if (year > 0 && year <= 50)
                        year = year + 2000;
                    if (year > DateTime.Now.Year)
                        year = 0;
                    if (month > 12 && day > 0 && day < 13)
                    {
                        temp = month;
                        month = day;
                        day = temp;
                    }
                    if (year > 0 && month > 0 && day > 0)
                    {
                        return new DateTime(year, month, day);
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }

        }
        private static string TruncatePercents(string input)
        {
            return Regex.Replace(input, @"(\W)+", "$1");
        }
        public static string RepleceAllString(string vale) {
            string regExp = "[^0-9//]";
            return Regex.Replace(vale, regExp, "");
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
        public static List<Guid> GetPhamVi(Guid AccountId, AppDbContext _context) {
            return _context.PhamVis.Where(it => it.AccountId == AccountId).Select(it => it.MaDiabanHoatDong).ToList();
        }

        public static decimal ConvertStringToDecimal(string value)
        {
            try
            {
                return decimal.Parse(value.Replace(",", ""));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string XuatWordCongTac(string fileName, string fileNameSave, DataTable dtInt)
        {
            try
            {
                //    System.IO.File.Copy(fileName, fileNameSave, true);
                //    using (var doc = DocX.Load(fileNameSave))
                //    {
                //        foreach (DataColumn item in dtInt.Columns)
                //        {
                //            if (item.DataType == typeof(DateTime))
                //            {
                //                try
                //                {
                //                    DateTime dtTemp = DateTime.Parse(dtInt.Rows[0]["" + item.ColumnName.ToString() + ""].ToString());
                //                    doc.ReplaceText("rep" + item.ColumnName.ToString(), dtTemp.ToString("dd/MM/yyyy"));

                //                }
                //                catch (System.Exception)
                //                {
                //                    doc.ReplaceText("rep" + item.ColumnName.ToString(), dtInt.Rows[0]["" + item.ColumnName.ToString() + ""].ToString());
                //                }
                //            }

                //            doc.ReplaceText("rep" + item.ColumnName.ToString(), dtInt.Rows[0]["" + item.ColumnName.ToString() + ""].ToString());
                //        }
                //        var table = doc.Tables.FirstOrDefault(t => t.TableCaption == "ThongTinNhanSu");
                //        if (table != null)
                //        {
                //            string values = "";
                //            for (int i = 0; i < table.RowCount; i++)
                //            {
                //                values = table.Rows[i].Paragraphs[1].Text;
                //                table.Rows[i].Paragraphs[1].ReplaceText(values, dtInt.Rows[0][values].ToString());
                //                values = table.Rows[i].Paragraphs[3].Text;
                //                if (values != "")
                //                    table.Rows[i].Paragraphs[3].ReplaceText(values, dtInt.Rows[0][values].ToString());
                //            }
                //        }
                //        doc.Save();
                //        doc.Dispose();
                //    }
                return fileNameSave;
            }
            catch (Exception ex)
            {
                return ex.Message ;
            }
        }

        public static DataTable AsDataTable<T>(this IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }


    }
}
