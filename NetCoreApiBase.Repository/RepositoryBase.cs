using Microsoft.EntityFrameworkCore;
using NetCoreApiBase.Contracts;
using NetCoreApiBase.Domain;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace NetCoreApiBase.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext repositoryContext { get; set; }

        public RepositoryBase(RepositoryContext _repositoryContext)
        {
            this.repositoryContext = _repositoryContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.repositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.repositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            this.repositoryContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.repositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.repositoryContext.Set<T>().Remove(entity);
        }
    }
}
