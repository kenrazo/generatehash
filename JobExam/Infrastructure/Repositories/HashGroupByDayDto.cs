namespace Infrastructure.Repositories
{
    public partial class HashRepository
    {
        public class HashGroupByDayDto
        {
            public DateTime Day { get; set; }
            public int Count { get; set; }
        }
    }
}
