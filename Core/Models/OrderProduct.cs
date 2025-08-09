using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("OrderProduct")]
    public class OrderProduct
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int ProductCuantity{ get; set; }
    }
}