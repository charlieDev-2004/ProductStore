using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }

        [Column(TypeName="varchar(150)")]
        public string Name { get; set; } = null!;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Column(TypeName="varchar(800)")]
        public string Description { get; set; } = null!;
        public int Stock { get; set; }

        [Column(TypeName="decimal(8,2)")]
        public double Price { get; set; }
        public ICollection<Picture> Pictures { get; set; } = new List<Picture>();
        public ICollection<OrderProduct> OrdersProduct { get; set; } = new List<OrderProduct>();

    }
}