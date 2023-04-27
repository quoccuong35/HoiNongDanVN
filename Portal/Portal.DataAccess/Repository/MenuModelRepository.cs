using Portal.Models;
using Portal.DataAccess;
using Portal.Models.Entitys;

namespace Portal.DataAccess
{
    public class MenuModelRepository : Repository<MenuModel>, IMenuModelRepository
    {
        private AppDbContext _db;
        public MenuModelRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MenuModel obj)
        {
            _db.MenuModels.Update(obj);
        }
    }
}
