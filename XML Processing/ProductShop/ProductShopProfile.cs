using AutoMapper;
using ProductShop.DTO.Input;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<UsersInputModel, User>();

            this.CreateMap<ProductInputModel, Product>();

            this.CreateMap<CategoriesInputModel, Category>();

            this.CreateMap<CategoriesProductInputModel, CategoryProduct>();
        }
    }
}
