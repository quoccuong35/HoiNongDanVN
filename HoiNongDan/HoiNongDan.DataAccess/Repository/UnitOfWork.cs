
using HoiNongDan.DataAccess;
using HoiNongDan.DataAccess.Repository;
using HoiNongDan.DataAccess.Repository.IRepository;
using HoiNongDan.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HoiNongDan.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _db;
        public IDepartmentRepository Department { get; private set; }
        public IAccountRepository Account { get; private set; }
        public IMenuModelRepository Menu { get; private set; }
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Department = new DepartmentRepository(_db);
            Account = new AccountRepository(_db);
            Menu = new MenuModelRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
