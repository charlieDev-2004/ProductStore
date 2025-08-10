using System.Security.Claims;
using API.DTOs.Order;
using API.Helpers.QueryObjects;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Core.Models.Email;
using Core.Models.Pagination;
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
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public OrderController(IRepository<Order> orderRepository, IRepository<OrderProduct> ordersProductsRepository, IMapper mapper, IRepository<Product> productRepository, IEmailService emailService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _emailService = emailService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] OrderQueryObject queryObject)
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

        [HttpGet("OrdersFromUser")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetOrdersFromUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var spec = new OrderSpecification(userId);
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById([FromRoute] int id)
        {
            var spec = new OrderSpecification(id);
            var order = _mapper.Map<OrderDto>(await _orderRepository.GetById(spec));
            return Ok(order);
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

            order.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            order.Date = DateTime.Now;
            order.TotalAmount = totalAmount;
            order.OrderProducts = productsOrder;

            if (await _orderRepository.Create(order))
            {
                var email = new Email
                {
                    EmailReceiver = User.FindFirstValue(ClaimTypes.Email)!,
                    Subject = "Email de confirmación de compra.",
                    Message = "Su compra ha sido realizada con éxito. Gracias por elegirnos."
                };

                await _emailService.SendEmail(email);
                return Ok(new { Message = "Orden creada correctamente." });
            }
                
            return StatusCode(500, new { Message = "Ha ocurrido un error al realizar la operación." });
        }

        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            var spec = new OrderSpecification(id);
            var order = await _orderRepository.GetById(spec);

            if (order == null)
                return NotFound(new { Message = $"La orden con el id {id} no existe." });

            if (await _orderRepository.Delete(order))
                return Ok(new { Message = "Orden eliminada correctamente." });
            
            return StatusCode(500, new { Message = "Ha ocurrido un error al realizar la operación." });
        }
        
    }
}