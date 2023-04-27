using Portal.DataAccess.Repository.IRepository;

namespace Portal.DataAccess
{
   
    public interface IUnitOfWork
    {
        IDepartmentRepository Department { get; }
        IAccountRepository Account { get; }
        IMenuModelRepository Menu { get; }
        void Save();
    }
}
