using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        
    }
}