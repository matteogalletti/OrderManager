using System;
using System.Linq;
using System.Linq.Expressions;
using Contracts;
using Domain;

namespace DAL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected DomainContext DomainContext { get; set; }

        public BaseRepository(DomainContext domainContext)
        {
            this.DomainContext = domainContext;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return this.DomainContext.Set<T>()
                .Where(expression);
        }

        public void Create(T entity)
        {
            this.DomainContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.DomainContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.DomainContext.Set<T>().Remove(entity);
        }
    }
}
