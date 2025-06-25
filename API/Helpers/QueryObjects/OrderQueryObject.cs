
namespace API.Helpers.QueryObjects
{
    public class OrderQueryObject
    {
        public string? UserId { get; set; }
        public DateTime? Date { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber{ get; set; }
    }
}