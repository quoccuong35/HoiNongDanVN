using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Hosting;
using NuGet.Packaging.Signing;
using HoiNongDan.Models;
using HoiNongDan.Models.ViewModels.Masterdata;

namespace HoiNongDan.Web.Areas.NhanSu.Models
{
    public  class FunctionFile
    {
        public static FileDinhKemModel CopyFile(IWebHostEnvironment hostEnvironment, IFormFile file, FileDinhKemModel fileDinhKem) { 
            try
			{
                string wwwRootPath = hostEnvironment.WebRootPath;
                fileDinhKem.Key = Guid.NewGuid();
                string fileName = fileDinhKem.Key.ToString();
                var uploads = Path.Combine(wwwRootPath, @"file\"+ fileDinhKem.IdCanBo);
                bool folderExists = System.IO.Directory.Exists(uploads);
                if (!folderExists)
                    System.IO.Directory.CreateDirectory(uploads);
                var extension = Path.GetExtension(file.FileName);
                using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    fileDinhKem.FileName = fileName + extension;
                }
                fileDinhKem.Url= @"file\" + fileDinhKem.IdCanBo +@"\" + fileName +extension;

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
                    bKQ = true;
                }
                return bKQ;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
