using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Order
{
    public class ProductByOrderDto
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public int Cuantity { get; set; }
    }
}