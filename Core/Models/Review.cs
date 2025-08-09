using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Review")]
    public class Review
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string UserId { get; set; } = null!;

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Column(TypeName = "varchar(900)")]
        public string Comment { get; set; } = null!;

        [Column(TypeName = "tinyint")]
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } 

    }
}