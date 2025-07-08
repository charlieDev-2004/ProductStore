using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public BaseSpecification(Expression<Func<T, bool>> filter)
        {
            Filter = filter;
        }

        public Expression<Func<T, bool>> Filter { get; }
        public int PageSize { get; set; } 
        public int PageNumber { get; set; }
        public Expression<Func<T,object>>? OrderBy { get; set; }
        public Expression<Func<T,object>>? OrderByDescending { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        protected void AddIncludes(params Expression<Func<T, object>>[] includes)
        {
            foreach (var include in includes)
            {
                Includes.Add(include);
            }
        }

    }
}