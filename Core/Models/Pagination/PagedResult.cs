
namespace Core.Models.Pagination
{
    public class PagedResult<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public ICollection<T> Items { get; set; } = new List<T>();
    }
}