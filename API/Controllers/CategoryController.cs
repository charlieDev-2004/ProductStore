using API.DTOs.Category;
using API.Helpers.QueryObjects;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Core.Models.Pagination;
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

         [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryQueryObject queryObject)
        {
            var spec = new CategorySpecification(queryObject.Name, queryObject.PageNumber, queryObject.PageSize);
            var pagedResult = _mapper.Map<PagedResult<CategoryDto>>(await _categoryRepository.GetAll(spec));
            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
             var spec = new CategorySpecification(id);
            var category = await _categoryRepository.GetById(spec);

            if (category == null)
                return NotFound(new { Message = $"La categoría con el id {id} no existe." });

            return Ok(category);
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

            return StatusCode(500, new { Message = "Ha ocuurido algún error al realizar la operación." });
        }
        
        [Authorize(Roles="Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCategory([FromRoute] int id, [FromBody] CategoryCreateDto categoryDto)
        {
            if (!await _categoryRepository.Exist(id))
                return NotFound(new { Message = $"La categoría con el id {id} no existe." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _mapper.Map<Category>(categoryDto);
            category.Id = id;

            if (await _categoryRepository.Update(category))
                return Ok(new { Message = "Categoría editada correctamente." });

            return StatusCode(500, new { Message = "Ha ocurrido un error al realizar la operación." });
        }
        
        [Authorize(Roles="Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var spec = new CategorySpecification(id);
            var category = await _categoryRepository.GetById(spec);

            if (category == null)
                return NotFound(new { Message = $"La categoría con el id {id} no existe." });

            if (await _categoryRepository.Delete(category))
                return Ok(new { Message = "Categoría eliminada correctamente." });
                 
            return StatusCode(500, new { Message = "Ha ocurrido un error al realizar la operación." });
        }

    }
}