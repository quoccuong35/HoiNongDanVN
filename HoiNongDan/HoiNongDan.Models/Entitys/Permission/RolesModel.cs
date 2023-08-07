using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoiNongDan.Models.Entitys;

namespace HoiNongDan.Models.Entitys
{
    public class RolesModel
    {
        public Guid RolesId { get; set; }
        public string RolesCode { get; set; }
        public string RolesName { get; set; }
        public int? OrderIndex { get; set; }
        public bool? Actived { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public ICollection<AccountInRoleModel> AccountInRoleModels { get; set; }
        public ICollection<PagePermissionModel> PagePermissionModels { get; set; }
        public RolesModel()
        {
            AccountInRoleModels = new List<AccountInRoleModel>();
            PagePermissionModels = new List<PagePermissionModel>();
        }

    }
}
