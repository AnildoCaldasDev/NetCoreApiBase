using System.Threading.Tasks;

namespace NetCoreApiBase.Contracts
{
    public interface IRepositoryWrapper
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IUserRepository User { get; }
        Task SaveAsync();
    }
}
