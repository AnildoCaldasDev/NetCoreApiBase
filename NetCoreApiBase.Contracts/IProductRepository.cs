using NetCoreApiBase.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCoreApiBase.Contracts
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        public Task<bool> ExistsProductsByCategoryId(int categoryId);
        public Task<IEnumerable<Product>> FindAllAsync();
        public Task<IEnumerable<Product>> FindByConditionAsync(Expression<Func<Product, bool>> expression);
    }
}
