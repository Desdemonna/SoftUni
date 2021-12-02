using AutoMapper;
using ProductShop.Dtos.Input;
using ProductShop.Dtos.Output;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserInputDto, User>();
            CreateMap<ProductInputDto, Product>();
            CreateMap<CategoryInputDto, Category>();
            CreateMap<CategoryProductsInputDto, CategoryProduct>();

            CreateMap<User, UserWithSoldProductDto>()
                .ForMember(dest => dest.SoldProducts, opt => opt.MapFrom(src => src.ProductsSold));

            CreateMap<Product, SoldProductDto>()
               .ForMember(x => x.BuyerFirstName, y => y.MapFrom(p => p.Buyer.FirstName))
               .ForMember(x => x.BuyerLastName, y => y.MapFrom(p => p.Buyer.LastName));


            CreateMap<Category, CategoryProductsDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProductsCount, opt => opt.MapFrom(src => src.CategoryProducts.Count))
                .ForMember(dest => dest.AveragePrice, opt => opt.MapFrom(src => $"{(src.CategoryProducts.Sum(cp => cp.Product.Price) / src.CategoryProducts.Count):F2}"))
                .ForMember(dest => dest.TotalRevenue, opt => opt.MapFrom(src => $"{src.CategoryProducts.Sum(c => c.Product.Price)}"));
        }
    }
}
