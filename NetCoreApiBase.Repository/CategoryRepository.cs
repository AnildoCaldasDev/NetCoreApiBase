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
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<IEnumerable<Category>> FindAllAsync()
        {
            return await base.FindAll().ToListAsync();
        }

        public async Task<IEnumerable<Category>> FindByConditionAsync(Expression<Func<Category, bool>> expression)
        {
            return await base.FindByCondition(expression).ToListAsync();
        }

        public async Task<Category> GetCategoryByIdCustom(int Id)
        {
            return await base.FindByCondition(x => x.Id == Id).FirstOrDefaultAsync();
        }
    }
}
