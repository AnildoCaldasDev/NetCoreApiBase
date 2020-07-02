using Microsoft.EntityFrameworkCore;
using NetCoreApiBase.Contracts;
using NetCoreApiBase.Domain;
using NetCoreApiBase.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCoreApiBase.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<IEnumerable<Product>> FindAllAsync()
        {
            return await base.FindAll().ToListAsync();
        }

        public async Task<IEnumerable<Product>> FindByConditionAsync(Expression<Func<Product, bool>> expression)
        {
            return await base.FindByCondition(expression).ToListAsync();
        }

       public async Task<bool> ExistsProductsByCategoryId(int categoryId)
        {
            return await base.FindByCondition(x => x.CategoryId == categoryId).AnyAsync();
        }
    }
}
