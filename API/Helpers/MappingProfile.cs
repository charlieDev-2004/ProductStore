using API.DTOs.Category;
using API.DTOs.Product;
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
            CreateMap<ProductCreateDto, Product>();
            CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.PicturesPaths, opt => opt.MapFrom(src => src.Pictures.Select(p => p.Path).ToList()));
        }
    }
}