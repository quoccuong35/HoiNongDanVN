using HoiNongDan.Models;

namespace HoiNongDan.DataAccess
{
    public interface IDepartmentRepository :IRepository<Department>
    {
        void Update(Department obj);
    }
}
