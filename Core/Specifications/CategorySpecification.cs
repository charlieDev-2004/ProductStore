using Core.Models;

namespace Core.Specifications
{
    public class CategorySpecification : BaseSpecification<Category>
    {
        public CategorySpecification(string? name, int? pageNumber, int? pageSize)
        : base(c => string.IsNullOrEmpty(name) || c.Name == name)
        {
            PageSize = pageSize.HasValue ? pageSize.Value : 10;
            PageNumber = pageNumber.HasValue ? pageNumber.Value : 1;
        }

        public CategorySpecification(int id)
        : base(c => c.Id == id)
        {
          
        }
        
    }
}