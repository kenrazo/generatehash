using static Infrastructure.Repositories.HashRepository;

namespace Infrastructure.Repositories
{
    public class HashesDto
    {
        public HashesDto(IEnumerable<HashGroupByDayDto> hashesGroup)
        {
            Hashes = hashesGroup;
        }

        public IEnumerable<HashGroupByDayDto> Hashes { get; set; }
    }
}

