using System;
using System.Linq;
using System.Linq.Expressions;

namespace Contracts
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> FindAll();

        IQueryable<T> Find(Expression<Func<T, bool>> expression);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
