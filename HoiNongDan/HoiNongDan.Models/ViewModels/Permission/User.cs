using HoiNongDan.Models.Entitys;
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

namespace HoiNongDan.Models
{
    public class UserVM
    {
        public UserVM() {
            userRoless = new List<UserRoles>();
            DiaBans = new List<AccountDiaBan>();
        }
        public Guid? AccountId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UserName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Password")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage ="Mật khẩu không hợp lệ")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string FullName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
        public Guid? EmployeeId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Roles")]
        public List<UserRoles> userRoless { get; set; }
        public List<AccountDiaBan> DiaBans { get; set; }
    }

    public class AccountEditVM
    {
        public AccountEditVM()
        {
            userRoless = new List<UserRoles>();
            DiaBans = new List<AccountDiaBan>();
        }
        public Guid? AccountId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UserName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Password")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Mật khẩu không hợp lệ")]
        [DataType(DataType.Password)]
        public string? PasswordNew { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string FullName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
        public Guid? EmployeeId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Roles")]
        public List<UserRoles> userRoless { get; set; }
        public List<AccountDiaBan> DiaBans { get; set; }
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
            account.EmployeeId = this.EmployeeId;
            account.CreatedAccountId = accountId;
            account.CreatedTime = DateTime.Now;
            account.AccountInRoleModels = AccountInRoleModels();
            account.PhamVis = AccountPhamVi(account.AccountId);

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
            edit.PhamVis = AccountPhamVi(edit.AccountId);
            edit.EmployeeId = this.EmployeeId;
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
        //edit pham vi
        public List<PhamVi> AccountPhamVi(Guid accountId) { 
            List<PhamVi> phamVis = this.DiaBans.Where(it=>it.Selected==true).Select(it=>new PhamVi { 
                MaDiabanHoatDong = it.MaDiaBanHoiVien,
                CreatedAccountId = accountId,
                CreatedTime = DateTime.Now
            }).ToList();
            return phamVis;
        }
    }
    public class AccountInfo {
        public Guid AccountId { get; set;}
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "UserName")]
        public string? UserName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FullName")]
        public string? FullName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PassWordOld")]
        public string PassWordOld { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PassWordNew")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Mật khẩu mới không hợp lệ")]
        public string PassWordNew { get; set; }
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
