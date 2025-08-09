using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Review;

namespace API.DTOs.ProductDTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Stock { get; set; }
        public double Price { get; set; }
        public int Rating { get; set; }
        public ICollection<string> PicturesPaths { get; set; } = new List<string>();
        public ICollection<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    }
}