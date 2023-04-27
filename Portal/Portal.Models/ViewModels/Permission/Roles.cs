using Portal.Models.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Portal.Models
{

    public class Roles
    {
        public System.Guid? RolesId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RolesCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string RolesCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RolesName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string RolesName { get; set; }

        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }
    }

    public class RolesVM : Roles {
        public List<Page> Pages { get; set; }
        public RolesVM() {
            Pages = new List<Page>();
        }
        public RolesModel Add(Guid accoundId) {
            List<PagePermissionModel> list = new List<PagePermissionModel>();
            RolesModel add = new RolesModel();
           
            add.RolesCode = this.RolesCode;
            add.RolesName = this.RolesName;
            add.Actived = true;
            add.CreatedAccountId = accoundId;
            add.CreatedTime = DateTime.Now;
            add.RolesId = Guid.NewGuid();
            add.OrderIndex = this.OrderIndex;
            if (Pages.Count > 0)
            {
                foreach (var item in Pages)
                {
                    var functions = item.Funtions.Where(it => it.Selected == true && it.MenuId == item.MenuId).Select(it => new PagePermissionModel
                    {
                        MenuId = it.MenuId,
                        FunctionId = it.FunctionId,
                    }).ToList();
                    if (functions.Count > 0)
                        list.AddRange(functions);
                }
                add.PagePermissionModels = list;
            }
          
            return add;
        }
        public RolesModel Update(Guid accountId,RolesModel editRoles) {
            List<PagePermissionModel> list = new List<PagePermissionModel>();
            if (Pages.Count > 0)
            {
                foreach (var item in Pages)
                {
                    var functions = item.Funtions.Where(it => it.Selected == true && it.MenuId == item.MenuId).Select(it => new PagePermissionModel
                    {
                        MenuId = it.MenuId,
                        FunctionId = it.FunctionId,
                    }).ToList();
                    if (functions.Count > 0)
                        list.AddRange(functions);
                }

            }
            editRoles.RolesCode = this.RolesCode;
            editRoles.RolesName = this.RolesName;
            editRoles.OrderIndex = this.OrderIndex;
            editRoles.Actived = this.Actived;
            editRoles.LastModifiedTime = DateTime.Now;
            editRoles.LastModifiedAccountId = accountId;
            editRoles.PagePermissionModels = list;
            return editRoles;
        }
    }
    public class Page {
        public Page() {
            Funtions = new List<PageFuntion>();
        }
        public Guid MenuId { get; set; }
        public string MenuName { get; set; }
        public string? MenuSubName { get; set; }
         public List<PageFuntion> Funtions { get; set; }
    }
    public class PageFuntion {
        public Guid MenuId { get; set; }
        public string FunctionId { get; set; }
        public string FunctionName { get; set; }
        public bool Selected { get; set; }
    }
    public class RolesSearch {

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RolesName")]
        public string? RolesName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
    }
}
