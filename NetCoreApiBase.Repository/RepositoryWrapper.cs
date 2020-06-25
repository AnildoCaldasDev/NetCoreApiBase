using NetCoreApiBase.Contracts;
using NetCoreApiBase.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreApiBase.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repositoryContext;
        private ICategoryRepository _category;
        private IUserRepository _user;
        private IProductRepository _product;

        public ICategoryRepository Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new CategoryRepository(_repositoryContext);
                }
                return _category;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductRepository(_repositoryContext);
                }
                return _product;
            }
        }


        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repositoryContext);
                }
                return _user;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext) {
            _repositoryContext = repositoryContext;
        }

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }

    }
}
