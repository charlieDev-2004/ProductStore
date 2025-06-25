
using Core.Models;

namespace Core.Specifications
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(string? userId, DateTime? date, int? pageSize, int? pageNumber)
        : base(o => (string.IsNullOrEmpty(userId) || o.UserId == userId) && (!date.HasValue || o.Date.Date == date.Value.Date))
        {
            PageSize = pageSize.HasValue ? pageSize.Value : 10;
            PageNumber = pageNumber.HasValue ? pageNumber.Value : 1;
            AddIncludes(o => o.OrderProducts);
        }
    }
}