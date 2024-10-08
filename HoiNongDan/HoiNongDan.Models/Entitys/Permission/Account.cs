﻿
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public bool? Actived { get; set; }
        public Guid? CreatedAccountId { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public Guid? LastModifiedAccountId { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public Guid? EmployeeId { get; set; }
        public string? AccountIDParent { get; set; }
        public virtual ICollection<AccountInRoleModel> AccountInRoleModels { get; set; }
        public virtual ICollection<HoiVienHoiDap> HoiVienHoiDaps { get; set; }
        public virtual ICollection<PhamVi> PhamVis { get; set; }
        public virtual ICollection<Author_Token> Author_Tokens { get; set; }
        public virtual ICollection<UsageLog> UsageLogs { get; set; }
        public Account()
        {
            AccountInRoleModels = new List<AccountInRoleModel>();
            HoiVienHoiDaps = new List<HoiVienHoiDap>();
            PhamVis = new List<PhamVi>();
            Author_Tokens = new List<Author_Token>();
            UsageLogs = new List<UsageLog>();
        }
    }
}
