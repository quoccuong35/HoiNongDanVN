using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys
{
    public class FunctionModel
    {
        public FunctionModel()
        {
            PageFunctionModels = new List<PageFunctionModel>();
            PagePermissionModels = new List<PagePermissionModel>();
        }
        [MaxLength(50)]
        public string FunctionId { get; set; }
        [MaxLength(100)]
        public string FunctionName { get; set; }
        public int OrderIndex { get; set; }
        public ICollection<PageFunctionModel> PageFunctionModels { get; set; }
        public ICollection<PagePermissionModel> PagePermissionModels { get; set; }
    }
}
