using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.ProductDTOs
{
    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int BrandId { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public int Stock { get; set; }

        [Required]
        public double Price { get; set; }
        public ICollection<IFormFile> Images{ get; set; } = new List<IFormFile>();
    }
}