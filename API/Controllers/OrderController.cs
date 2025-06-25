using System.Security.Claims;
using API.DTOs.Order;
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
    public class OrderController : ControllerBase
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public OrderController(IRepository<Order> orderRepository, IRepository<OrderProduct> ordersProductsRepository, IMapper mapper, IRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery]OrderQueryObject queryObject)
        {
            var spec = new OrderSpecification(queryObject.UserId, queryObject.Date, queryObject.PageSize, queryObject.PageNumber);
            var orderPagedResult = await _orderRepository.GetAll(spec);

            var pagedResult = new PagedResult<OrderDto>
            {
                CurrentPage = orderPagedResult.CurrentPage,
                PageSize = orderPagedResult.PageSize,
                TotalPages = orderPagedResult.TotalPages,
                Items = _mapper.Map<List<OrderDto>>(orderPagedResult.Items)
            };

            return Ok(pagedResult);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderCreateDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = new Order();
            double totalAmount = 0;
            List<OrderProduct> productsOrder = new List<OrderProduct>();

            foreach (var productByOrder in orderDto.ProductsByOrder)
            {
                var spec = new ProductSpecification(productByOrder.ProductId);
                var product = await _productRepository.GetById(spec);
                totalAmount += product.Price * productByOrder.Cuantity;
                product.Stock = product.Stock - productByOrder.Cuantity;
                productsOrder.Add(new OrderProduct { OrderId = order.Id, ProductId = productByOrder.ProductId, ProductCuantity = productByOrder.Cuantity });
                await _productRepository.Update(product);
            }

            order.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            order.Date = DateTime.Now;
            order.TotalAmount = totalAmount;
            order.OrderProducts = productsOrder;

            if (await _orderRepository.Create(order))
                return Ok(new { Message = "Orden creada correctamente." });

            return StatusCode(500, new { Message = "Ha ocurrido un error al realizar la operación." });
        }
        

        
    }
}