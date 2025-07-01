using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.ProductDTOs
{
    public class ProductDto
    {
        public int Id { get; set;}
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Stock { get; set; }
        public double Price { get; set; }
        public ICollection<string> PicturesPaths{ get; set; } = new List<string>();
    }
}