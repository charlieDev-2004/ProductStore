using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Brand
{
    public class BrandCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}