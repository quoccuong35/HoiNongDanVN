using Portal.Models.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Portal.Models
{
    public class UserVM
    {
        public UserVM() {
            userRoless = new List<UserRoles>();
        }
        public Guid? AccountId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UserName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Password")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string FullName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
        public Guid? EmployeeId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Roles")]
        public List<UserRoles> userRoless { get; set; }
    }

    public class AccountVM : UserVM
    {
        public Account Add(Guid accountId) {
            Account account = new Account();
            account.AccountId = Guid.NewGuid();
            account.UserName = this.UserName;
            account.FullName = this.FullName;
            account.Password = Password;
            account.Actived = true;
            account.CreatedAccountId = accountId;
            account.CreatedTime = DateTime.Now;
            account.AccountInRoleModels = AccountInRoleModels();
            
            return account;
        }
        public List<AccountInRoleModel> AddAccountInRoleModel(Guid id) {
            List<AccountInRoleModel> lists = new List<AccountInRoleModel>();
            foreach (var item in this.userRoless)
            {
                lists.Add(new AccountInRoleModel
                {
                    AccountId = id,
                    RolesId = item.RolesId,
                });
            }
            return lists;
        }
        public Account Update(Guid accountId,Account edit)
        {
            edit.Actived = this.Actived;
            edit.UserName = this.UserName;
            //edit.Password = this.Password;
            edit.FullName = this.FullName;
            edit.LastModifiedAccountId = accountId;
            edit.LastModifiedTime = DateTime.Now;
            edit.AccountInRoleModels = AccountInRoleModels();
            return edit;
        }

        public List<AccountInRoleModel> AccountInRoleModels() {
            List<AccountInRoleModel> lists = new List<AccountInRoleModel>();
            var roless = this.userRoless.Where(it => it.Selected == true).ToList();
            foreach (var item in roless)
            {
                lists.Add(new AccountInRoleModel
                {
                    RolesId = item.RolesId,
                });
            }
            return lists;
        }
    }

    public class UserRoles
    { 
        public Guid RolesId { get; set; }
        public string? RolesName { get; set; }
        public bool Selected { get; set; } = false;
    }
    public class UserSearchVM {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UserName")]
        public string? UserName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
    }
}
