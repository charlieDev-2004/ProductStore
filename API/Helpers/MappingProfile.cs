using API.DTOs.Brand;
using API.DTOs.Category;
using API.DTOs.Order;
using API.DTOs.ProductDTOs;
using API.DTOs.Review;
using AutoMapper;
using Core.Models;

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
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Reviews != null && src.Reviews.Any()? src.Reviews.Average(r => r.Rating) : 0))
            .ForMember(dest => dest.Reviews, opt =>
                opt.MapFrom(src => src.Reviews
                    .Select(r => new ReviewDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        ProductId = r.ProductId,
                        Comment = r.Comment,
                        Rating = r.Rating,
                        CreatedAt = r.CreatedAt,
                    }).ToList()));

            CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.ProductsByOrder, opt =>
                opt.MapFrom(src => src.OrderProducts
                    .Select(op => new ProductByOrderDto
                    {
                        ProductId = op.ProductId,
                        Cuantity = op.ProductCuantity
                    }).ToList()));

            CreateMap<ReviewCreateDto, Review>();
            CreateMap<Review, ReviewDto>();
        }
    }
}