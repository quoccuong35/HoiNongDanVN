using Portal.Models;
using Portal.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.DataAccess.Repository.IRepository
{
    public interface IAccountRepository: IRepository<Account>
    {
        void Update(Account obj);
    }
}
