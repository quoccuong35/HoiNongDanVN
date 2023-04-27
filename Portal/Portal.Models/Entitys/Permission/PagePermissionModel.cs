using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models.Entitys
{
    public class PagePermissionModel
    {
        public Guid RolesId { get; set; }
        public Guid MenuId { get; set; }
        [MaxLength(50)]
        public string FunctionId { get; set; }

        public RolesModel Roles { get; set; }
        public MenuModel Menu { get; set; }
        public FunctionModel Function { get; set; }

    }
}
