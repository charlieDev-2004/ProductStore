
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Brand")]
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}