using NetCoreApiBase.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCoreApiBase.Contracts
{
    public interface ICategoryRepository: IRepositoryBase<Category>
    {
        //posso tambem colocar metodos customizados.
        public Task<Category> GetCategoryByIdCustom(int Id);
        public Task<IEnumerable<Category>> FindAllAsync();
        public Task<IEnumerable<Category>> FindByConditionAsync(Expression<Func<Category, bool>> expression);
    }
}
