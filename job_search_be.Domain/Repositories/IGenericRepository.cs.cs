using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> GetAllData();
        T GetById(long id);
        void Create(T entity);
        void Update(T entity);
        void Delete(long id);
    }
}
