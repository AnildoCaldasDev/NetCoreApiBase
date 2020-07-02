using NetCoreApiBase.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCoreApiBase.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        public Task<IEnumerable<User>> FindAllAsync();
        public Task<IEnumerable<User>> FindByConditionAsync(Expression<Func<User, bool>> expression);
    }
}
