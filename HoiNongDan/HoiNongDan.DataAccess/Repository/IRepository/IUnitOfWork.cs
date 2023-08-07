using HoiNongDan.DataAccess.Repository.IRepository;

namespace HoiNongDan.DataAccess
{
   
    public interface IUnitOfWork
    {
        IDepartmentRepository Department { get; }
        IAccountRepository Account { get; }
        IMenuModelRepository Menu { get; }
        void Save();
    }
}
