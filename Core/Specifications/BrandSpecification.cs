using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Specifications
{
    public class BrandSpecification : BaseSpecification<Brand>
    {
        public BrandSpecification(string? name, int? pageSize, int? pageNumber)
        : base(b => string.IsNullOrEmpty(name) || b.Name.ToLower().Contains(name.ToLower()))
        {
            PageSize = pageSize.HasValue ? pageSize.Value : 10;
            PageNumber = pageNumber.HasValue ? pageNumber.Value : 1;
        }

        public BrandSpecification(int id) : base(b => b.Id == id)
        {}
    }
}