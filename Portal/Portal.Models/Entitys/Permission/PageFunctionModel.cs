using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models.Entitys
{
    public class PageFunctionModel
    {
        public Guid MenuId { get; set; }
        public string FunctionId { get; set; }

        public MenuModel MenuModel { get; set; }

        public FunctionModel FunctionModel { get; set; }

    }
}
