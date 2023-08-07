
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.DataAccess
{
    public interface IHistoryModelRepository
    {
        void SaveUpdateHistory<T>(String id, Guid accountId,T obj, params Expression<Func<T, object>>[] propertiesToUpdate) where T : class;
    }
}
