using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Order")]
    public class Order
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string UserId { get; set; } = null!;
        public DateTime Date { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public double TotalAmount { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}