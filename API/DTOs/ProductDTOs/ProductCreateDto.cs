using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.ProductDTOs
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string Description { get; set; } = null!;
        public int Stock { get; set; }
        public double Price { get; set; }
        public ICollection<string> PicturesPaths{ get; set; } = new List<string>();
    }
}