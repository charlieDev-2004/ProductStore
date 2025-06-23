using API.DTOs;
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
        }
    }
}