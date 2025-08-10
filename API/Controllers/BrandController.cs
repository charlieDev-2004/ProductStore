using API.DTOs.Brand;
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
    public class BrandController : ControllerBase
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;

        public BrandController(IRepository<Brand> brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBrands([FromQuery] BrandQueryObject queryObject)
        {
            var spec = new BrandSpecification(queryObject.Name, queryObject.PageSize, queryObject.PageNumber);
            var pagedResult = _mapper.Map<PagedResult<BrandDto>>(await _brandRepository.GetAll(spec));
            return Ok(pagedResult);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBrand(BrandCreateDto brandDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var brand = _mapper.Map<Brand>(brandDto);

            if (await _brandRepository.Create(brand))
                return Ok(new { Message = "Marca creada correctamente." });

            return StatusCode(500, new { Message = "Ha ocurrido un error al realizar la operación." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand([FromRoute] int id, BrandCreateDto brandDto)
        {
            if (!await _brandRepository.Exist(id))
                return NotFound(new { Message = $"La marca con el id {id} no existe." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var brand = _mapper.Map<Brand>(brandDto);
            brand.Id = id;

            if (await _brandRepository.Update(brand))
                return Ok(new { Message = "Marca actualizada correctamente." });

            return StatusCode(500, new{ Message = "Ha ocurrido un error interno en el servidor." });
        }
    }
}