using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery.Where(spec.Filter);

            if (spec.Includes.Any())
                query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}