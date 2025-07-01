using API.DTOs.Brand;
using API.DTOs.Category;
using API.DTOs.Order;
using API.DTOs.ProductDTOs;
using AutoMapper;
using Core.Models;

namespace API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<Category, CategoryDto>();
            CreateMap<BrandCreateDto, Brand>();
            CreateMap<Brand, BrandDto>();
            CreateMap<ProductCreateDto, Product>();

            CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.PicturesPaths, opt => opt.MapFrom(src => src.Pictures.Select(p => p.Path).ToList()));

            CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.ProductsByOrder, opt =>
                opt.MapFrom(src => src.OrderProducts
                    .Select(op => new ProductByOrderDto
                    {
                        ProductId = op.ProductId,
                        Cuantity = op.ProductCuantity
                    }).ToList()));

        }
    }
}