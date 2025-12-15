using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtTracker.Entities;

namespace DebtTracker.DataAccessLayer
{
    public interface IRepository<T> where T : IDomainObject
    {
        void Create(T entity);
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Update(T entity);
        void Delete(int id);
    }
}
