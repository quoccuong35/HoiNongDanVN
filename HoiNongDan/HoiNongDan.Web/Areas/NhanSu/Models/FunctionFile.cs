using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Hosting;
using NuGet.Packaging.Signing;
using HoiNongDan.Models;
using HoiNongDan.Models.ViewModels.Masterdata;

namespace HoiNongDan.Web.Areas.NhanSu.Models
{
    public  class FunctionFile
    {
        public static FileDinhKemModel CopyFile(IWebHostEnvironment hostEnvironment, IFormFile file, FileDinhKemModel fileDinhKem,string?TenThuMuc = null) { 
            try
			{
                string wwwRootPath = hostEnvironment.WebRootPath;
                fileDinhKem.Key = Guid.NewGuid();
                string fileName = Path.GetFileName(file.FileName);
                fileName = fileName.Substring(0, fileName.LastIndexOf("."));
                string fodel = TenThuMuc != null ? TenThuMuc : fileDinhKem.IdCanBo.ToString();
                var uploads = Path.Combine(wwwRootPath, @"file\"+ fodel);
                bool folderExists = System.IO.Directory.Exists(uploads);
                if (!folderExists)
                    System.IO.Directory.CreateDirectory(uploads);
                var extension = Path.GetExtension(file.FileName);
                string path = Path.Combine(uploads, fileName + extension);
                if (System.IO.File.Exists(path))
                {
                    fileDinhKem.Error = " Đã tồn tại tên file " + file.FileName;
                }
                if (String.IsNullOrWhiteSpace(fileDinhKem.Error))
                {
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        fileDinhKem.FileName = fileName;
                    }
                    fileDinhKem.Url = @"file\" + fodel + @"\" + fileName + extension;
                }
               

            }
			catch (Exception ex)
			{
                fileDinhKem.Error = ex.Message;
			}
            return fileDinhKem;
        }
        public static bool Delete(IWebHostEnvironment hostEnvironment, string url) {
            try
            {
                bool bKQ = false;
                string wwwRootPath = hostEnvironment.WebRootPath;
                var oldImagePath = Path.Combine(wwwRootPath, url.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                    var urlFolder = oldImagePath.Substring(0, oldImagePath.LastIndexOf('\\'));
                    if (System.IO.Directory.Exists(urlFolder))
                    {
                        string[] allFile = System.IO.Directory.GetFiles(urlFolder, "*.*", SearchOption.AllDirectories);
                        if(allFile != null && allFile.Length==0) {
                            System.IO.Directory.Delete(urlFolder);
                        }
                    }
                    bKQ = true;
                }
                
                /// xóa xem nếu thư mục rổng
                return bKQ;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public static bool DeleteFolder(IWebHostEnvironment hostEnvironment, string url) {
            var uploads = Path.Combine(hostEnvironment.WebRootPath, @"file\" + url);
            bool folderExists = System.IO.Directory.Exists(uploads);
            if (folderExists)
            {
                System.IO.Directory.Delete(uploads);
                return true;
            }
            return false;
        }
    }
}
