namespace Infrastructure.Repositories
{
    public partial class HashRepository
    {
        public class HashGroupByDayDto
        {
            public DateTime Date { get; set; }
            public int Count { get; set; }
        }
    }
}
