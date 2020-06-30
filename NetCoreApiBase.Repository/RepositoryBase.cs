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
        protected RepositoryContext _repositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this._repositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll()
        {
            return this._repositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this._repositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            this._repositoryContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this._repositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this._repositoryContext.Set<T>().Remove(entity);
        }
    }
}
