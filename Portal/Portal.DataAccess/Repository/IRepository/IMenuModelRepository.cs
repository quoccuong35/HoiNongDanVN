using Portal.Models;
using Portal.Models.Entitys;

namespace Portal.DataAccess
{
    public interface IMenuModelRepository : IRepository<MenuModel>
    {
        void Update(MenuModel obj);
    }
}
