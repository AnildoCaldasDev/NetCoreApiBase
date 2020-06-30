using NetCoreApiBase.Domain.Models;

namespace NetCoreApiBase.Contracts
{
    public interface ICategoryRepository: IRepositoryBase<Category>
    {
        //posso tambem colocar metodos customizados.
        public Category GetCategoryByIdCustom(int Id);
    }
}
