using Portal.Models;

namespace Portal.DataAccess
{
    public interface IDepartmentRepository :IRepository<Department>
    {
        void Update(Department obj);
    }
}
