using NetCoreApiBase.Contracts;
using NetCoreApiBase.Domain;
using NetCoreApiBase.Domain.Models;

namespace NetCoreApiBase.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
