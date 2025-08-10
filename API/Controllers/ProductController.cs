using API.DTOs.ProductDTOs;
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
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IRepository<Product> productRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductQueryObject queryObject)
        {
            var spec = new ProductSpecification(queryObject.Name, queryObject.CategoryId, queryObject.BrandId, queryObject.Description, queryObject.Price, queryObject.PageSize, queryObject.PageNumber);
            var pagedResult = _mapper.Map<PagedResult<ProductDto>>(await _productRepository.GetAll(spec));
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
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<Picture> pictureList = new List<Picture>();

            foreach (var picture in productDto.Images)
            {
                if (picture != null && picture.Length > 0)
                {
                    var webRootPath = _webHostEnvironment.WebRootPath;
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);
                    var filePath = Path.Combine(webRootPath, "images", fileName);
                    var relativePath = Path.Combine("images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await picture.CopyToAsync(stream);
                    } 

                     pictureList.Add(new Picture { Path = relativePath });
                }
            }

            var product = _mapper.Map<Product>(productDto);

            product.Pictures = pictureList;

            if (await _productRepository.Create(product))
                return Ok(new { Message = "Producto creado correctamente." });

            return StatusCode(500, new { Message = "Ha ocurrido un error al realizar la operación." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromForm] ProductCreateDto productDto)
        {
            var spec = new ProductSpecification(id);
            var product = await _productRepository.GetById(spec);

            if (product == null)
                return NotFound(new { Message = $"El producto con el id {id} no existe." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            product.Name = productDto.Name;
            product.CategoryId = productDto.CategoryId;
            product.BrandId = productDto.BrandId;
            product.Description = productDto.Description;
            product.Stock = productDto.Stock;
            product.Price = productDto.Price;

            foreach (var picture in productDto.Images)
            {
                if (picture != null && picture.Length > 0)
                {
                    var webRootPath = _webHostEnvironment.WebRootPath;
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);
                    var filePath = Path.Combine(webRootPath, "images", fileName);
                    var relativePath = Path.Combine("images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await picture.CopyToAsync(stream);
                    }

                    product.Pictures.Add(new Picture { Path = relativePath });
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
                return NotFound(new { Message = $"El producto con el id {id} no existe" });

            var webRootPath = _webHostEnvironment.WebRootPath;

            foreach (var picture in product.Pictures)
            {
                if (System.IO.File.Exists(Path.Combine(webRootPath,picture.Path)))
                {
                    System.IO.File.Delete(Path.Combine(webRootPath,picture.Path));
                }
            }

            if (await _productRepository.Delete(product))
                    return Ok( new { Message = "Producto eliminado correctamente" });

            return StatusCode(500, new { Message = "Ha ocurrido un error al realizar la operación." });
        }

        [HttpGet("ProductsFromOrder/{orderId}")]
        public async Task<IActionResult> GetProductsFromOrder([FromRoute] int orderId, [FromQuery] int? PageSize, [FromQuery] int? PageNumber)
        {
            var spec = new ProductSpecification(orderId, PageSize, PageNumber);
            var productPagedResult = await _productRepository.GetAll(spec);

            var pagedResult = new PagedResult<ProductDto>
            {
                CurrentPage = productPagedResult.CurrentPage,
                PageSize = productPagedResult.PageSize,
                TotalPages = productPagedResult.TotalPages,
                Items = _mapper.Map<List<ProductDto>>(productPagedResult.Items)
            };

            return Ok(pagedResult);
        }
    }
}