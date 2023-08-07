using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.ViewModels.Masterdata
{
    public class FileDinhKemModel : FileDinhKem
    {
        public string Error { get; set; }

        public FileDinhKem GetFileDinhKem() {
            return new FileDinhKem
            {
                Key = this.Key,
                Url = this.Url,
                Id = this.Id,
                IdCanBo = this.IdCanBo,
                IDLoaiDinhKem = this.IDLoaiDinhKem,
                FileName = this.FileName,
            };
        }
    }
}
