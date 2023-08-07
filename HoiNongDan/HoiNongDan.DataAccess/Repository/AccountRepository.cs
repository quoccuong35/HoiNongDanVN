using HoiNongDan.DataAccess.Repository.IRepository;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.DataAccess.Repository
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        private AppDbContext _db;
        public AccountRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Account obj)
        {
            _db.Accounts.Update(obj);
        }
    }
}
