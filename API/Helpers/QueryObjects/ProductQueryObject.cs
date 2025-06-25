
namespace API.Helpers.QueryObjects
{
    public class ProductQueryObject
    {
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber{ get; set; }
    }
}