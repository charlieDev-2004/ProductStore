
namespace API.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime Date { get; set; }
        public double TotalAmount { get; set; }
        public List<ProductByOrderDto> ProductsByOrder{ get; set; } = new List<ProductByOrderDto>();
    }
}