using System.Security.Claims;
using API.DTOs.Category;
using API.Helpers.QueryObjects;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _mapper.Map<Category>(categoryDto);

            if (await _categoryRepository.Create(category))
                return Ok(new { Message = "Categoría creada correctamente." });

            return StatusCode(500, new { Message = "A ocuurido algún error al realizar la operación." });
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryQueryObject queryObject)
        {
            var spec = new CategorySpecification(queryObject.Name, queryObject.PageNumber, queryObject.PageSize);
            var categories = _mapper.Map<List<CategoryDto>>(await _categoryRepository.GetAll(spec));
            return Ok(categories);
        }

    }
}