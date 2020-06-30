using NetCoreApiBase.Domain.Models;

namespace NetCoreApiBase.Contracts
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        bool ExistsProductsByCategoryId(int categoryId);
    }
}
