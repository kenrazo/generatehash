namespace Infrastructure.RabbitMq
{
    public class HashDto
    {
        public HashDto(IEnumerable<string> hashes)
        {
            Hashes = hashes;
        }
        public IEnumerable<string> Hashes { get; set; }
    }
}
