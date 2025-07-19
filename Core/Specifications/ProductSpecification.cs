
using Core.Models;

namespace Core.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(string? name, int? categoryId, int? brandId, string? description, double? rangePrice, int? pageSize, int? pageNumber)
        : base(p => (string.IsNullOrEmpty(name) || p.Name.ToLower().Contains(name.ToLower()))
                    && (!categoryId.HasValue || categoryId.Value == 0 || p.CategoryId == categoryId)
                    && (!brandId.HasValue || brandId.Value == 0 || p.BrandId == brandId)
                    && (string.IsNullOrEmpty(description) || p.Name.ToLower().Contains(description.ToLower()))
                    && (!rangePrice.HasValue || p.Price <= rangePrice.Value)
                    && (p.Stock > 0)
        )
        {
            PageSize = pageSize.HasValue ? pageSize.Value : 10;
            PageNumber = pageNumber.HasValue ? pageNumber.Value : 1;
            AddIncludes(p => p.Category, p => p.Brand, p => p.Pictures);
        }

        public ProductSpecification(int id)
        : base(p => p.Id == id)
        {
            AddIncludes(p => p.Category, p => p.Brand, p => p.Pictures);
        }

        public ProductSpecification(int orderId, int? pageSize, int? pageNumber)
        : base(p => p.OrdersProduct.Any(op => op.OrderId == orderId))
        {
            PageSize = pageSize.HasValue ? pageSize.Value : 20;
            PageNumber = pageNumber.HasValue ? pageNumber.Value : 1;
            AddIncludes(p => p.OrdersProduct, p => p.Pictures, p => p.Category, p => p.Brand);
        }
        
    }
}