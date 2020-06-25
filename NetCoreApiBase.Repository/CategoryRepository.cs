using Microsoft.EntityFrameworkCore;
using NetCoreApiBase.Contracts;
using NetCoreApiBase.Domain;
using NetCoreApiBase.Domain.Models;
using System.Linq;

namespace NetCoreApiBase.Repository
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public Category GetCategoryByIdCustom(int Id)
        {
            return base.FindByCondition(x => x.Id == Id).AsNoTracking().FirstOrDefault();
        }
    }
}
