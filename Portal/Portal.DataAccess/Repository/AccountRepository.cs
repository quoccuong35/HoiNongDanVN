using Portal.DataAccess.Repository.IRepository;
using Portal.Models;
using Portal.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.DataAccess.Repository
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
