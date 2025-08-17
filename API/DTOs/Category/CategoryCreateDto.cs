
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Category
{
    public class CategoryCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}