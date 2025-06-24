
using Core.Models;

namespace Core.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(string? name, int? categoryId, string? description, double? rangePrice, int? pageSize, int? pageNumber)
        : base(p => (string.IsNullOrEmpty(name) || p.Name.ToLower().Contains(name.ToLower())) && (!categoryId.HasValue || p.CategoryId == categoryId) && (string.IsNullOrEmpty(description) || p.Name.ToLower().Contains(description.ToLower())) && (!rangePrice.HasValue || p.Price <= rangePrice.Value))
        {
            PageSize = pageSize.HasValue ? pageSize.Value : 10;
            PageNumber = pageNumber.HasValue ? pageNumber.Value : 1;
            AddIncludes(p => p.Pictures);
        }
        
        public ProductSpecification(int id)
        : base(p => p.Id == id)
        {
            AddIncludes(p => p.Pictures);
        }
    }
}