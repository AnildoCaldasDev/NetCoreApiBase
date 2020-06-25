using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreApiBase.Contracts
{
    public interface IRepositoryWrapper
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IUserRepository User { get; }
        void Save();
    }
}
