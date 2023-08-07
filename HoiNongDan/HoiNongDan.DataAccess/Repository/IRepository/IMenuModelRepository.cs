using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;

namespace HoiNongDan.DataAccess
{
    public interface IMenuModelRepository : IRepository<MenuModel>
    {
        void Update(MenuModel obj);
    }
}
