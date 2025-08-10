using API.DTOs.Brand;
using API.DTOs.Category;
using API.DTOs.Order;
using API.DTOs.ProductDTOs;
using API.DTOs.Review;
using AutoMapper;
using Core.Models;
using Core.Models.Pagination;

namespace API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var baseUrl = "http://localhost:5203/";

            CreateMap<CategoryCreateDto, Category>();
            CreateMap<Category, CategoryDto>();
            CreateMap<BrandCreateDto, Brand>();
            CreateMap<Brand, BrandDto>();
            CreateMap<ProductCreateDto, Product>()
            .ForMember(dest => dest.Pictures, opt => opt.Ignore());

            CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.PicturesPaths, opt => opt.MapFrom(src => src.Pictures.Select(p => baseUrl + p.Path.Replace("\\", "/")).ToList()))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Reviews != null && src.Reviews.Any() ? src.Reviews.Average(r => r.Rating) : 0))
            .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews));

            CreateMap<OrderProduct, ProductByOrderDto>()
            .ForMember(dest => dest.Cuantity, opt => opt.MapFrom(src => src.ProductCuantity));

            CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.ProductsByOrder, opt => opt.MapFrom(src => src.OrderProducts));

            CreateMap<ReviewCreateDto, Review>();
            CreateMap<Review, ReviewDto>();

            //Mapeos de paginación
            CreateMap<PagedResult<Brand>, PagedResult<BrandDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

             CreateMap<PagedResult<Category>, PagedResult<CategoryDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<PagedResult<Product>, PagedResult<ProductDto>>()
           .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            
             CreateMap<PagedResult<Order>, PagedResult<OrderDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}