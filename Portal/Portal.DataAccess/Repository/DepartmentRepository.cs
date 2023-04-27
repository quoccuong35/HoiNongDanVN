﻿using Portal.Models;
using Portal.DataAccess;

namespace Portal.DataAccess
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private AppDbContext _db;
        public DepartmentRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Department obj)
        {
            _db.Departments.Update(obj);
        }
    }
}
