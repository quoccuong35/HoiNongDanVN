using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models.Entitys
{
    public class AccountInRoleModel
    {
        public Guid AccountId { get; set; }
        public Guid RolesId { get; set; }

        public Account Account { get; set; }

        public RolesModel RolesModel { get; set; }
    }
}
