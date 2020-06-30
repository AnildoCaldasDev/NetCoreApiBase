using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NetCoreApiBase.Contracts;
using NetCoreApiBase.Domain;
using NetCoreApiBase.Domain.Models;
using System.Linq;

namespace NetCoreApiBase.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public bool ExistsProductsByCategoryId(int categoryId)
        {
            return this.FindByCondition(x => x.CategoryId == categoryId).
                                    AsNoTracking().ToList().Any();

        }
    }
}
