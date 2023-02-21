
namespace Domain.QueryParameters
{
    public class UserResultsParameters : PagingParameters
    {
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; } = DateTime.Now;
    }
}
