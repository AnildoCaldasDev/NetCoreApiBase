using AutoMapper;
using NetCoreApiBase.Domain.DTO;
using NetCoreApiBase.Domain.Models;

namespace NetCoreApiBase.Api.AutoMapper
{
    public class MappingProlile : Profile
    {
        public MappingProlile()
        {
            //Domain to Dto
            CreateMap<Category, CategoryDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<User, UserDto>();

            //Dto to Domain
            CreateMap<CategoryDto, Category>();
            CreateMap<ProductDto, Product>();
            CreateMap<UserDto, User>();
        }
    }
}
