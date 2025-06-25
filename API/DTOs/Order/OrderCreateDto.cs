
namespace API.DTOs.Order
{
    public class OrderCreateDto
   {
        public List<ProductByOrderDto> ProductsByOrder{ get; set; } = new List<ProductByOrderDto>();
   }
}