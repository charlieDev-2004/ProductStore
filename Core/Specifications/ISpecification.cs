using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T, bool>> Filter { get; }
        int PageSize{ get; set; }
        int PageNumber{ get; set; }
        List<Expression<Func<T, object>>> Includes { get; }
    }
}