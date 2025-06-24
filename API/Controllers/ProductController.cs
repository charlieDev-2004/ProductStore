using API.DTOs.Product;
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
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductQueryObject queryObject)
        {
            var spec = new ProductSpecification(queryObject.Name, queryObject.CategoryId, queryObject.Description, queryObject.Price, queryObject.PageSize, queryObject.PageNumber);
            var productsPagedResult = await _productRepository.GetAll(spec);

            var pagedResult = new PagedResult<ProductDto>
            {
                CurrentPage = productsPagedResult.CurrentPage,
                PageSize = productsPagedResult.PageSize,
                TotalPages = productsPagedResult.TotalPages,
                Items = _mapper.Map<List<ProductDto>>(productsPagedResult.Items)
            };

            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var spec = new ProductSpecification(id);
            var product = await _productRepository.GetById(spec);

            if (product == null)
                return NotFound(new { Message = $"El producto con el id {id} no existe." });

            return Ok(_mapper.Map<ProductDto>(product));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<Picture> pictureList = new List<Picture>();

            foreach (var path in productDto.PicturesPaths)
            {
                pictureList.Add(new Picture { Path = path });
            }

            var product = _mapper.Map<Product>(productDto);

            product.Pictures = pictureList;

            if (await _productRepository.Create(product))
                return Ok(new { Message = "Producto creado correctamente." });

            return StatusCode(500, new { Message = "Ha ocurrido un error al realizar la operación." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductCreateDto productDto)
        {
            var spec = new ProductSpecification(id);
            var product = await _productRepository.GetById(spec);

            if (product == null)
                return NotFound(new { Message = $"El producto con el id {id} no existe." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            product.Id = id;
            product.Name = productDto.Name;
            product.CategoryId = productDto.CategoryId;
            product.Description = productDto.Description;
            product.Stock = productDto.Stock;
            product.Price = productDto.Price;

            if (productDto.PicturesPaths.Any())
            {
                foreach (var path in productDto.PicturesPaths)
                {
                    product.Pictures.Add(new Picture { Path = path });
                }
            }

            if (await _productRepository.Update(product))
                return Ok(new { Message = "Producto editado correctamente" });

            return StatusCode(500, new { Message = "Ha ocurrido un error al realizar la operación." });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] int id)
        {
            var spec = new ProductSpecification(id);
            var product = await _productRepository.GetById(spec);

            if (product == null)
                return NotFound(new { Message = $"El producto con el id {id} no existe"});
            
            if (await _productRepository.Delete(product))
                return Ok("Se ha eliminado correctamente");
            
            return StatusCode(500, new { Message = "Ha ocurrido un error al realizar la operación." });
        }
    }
}