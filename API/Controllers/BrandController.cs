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
        public async Task<IActionResult> GetAllBrands([FromQuery]BrandQueryObject queryObject)
        {
            var spec = new BrandSpecification(queryObject.Name, queryObject.PageSize, queryObject.PageNumber);
            var brandPagedResult = await _brandRepository.GetAll(spec);

            var pagedResult = new PagedResult<BrandDto>
            {
                CurrentPage = brandPagedResult.CurrentPage,
                PageSize = brandPagedResult.PageSize,
                TotalPages = brandPagedResult.TotalPages,
                Items = _mapper.Map<List<BrandDto>>(brandPagedResult.Items)
            };

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
    }
}