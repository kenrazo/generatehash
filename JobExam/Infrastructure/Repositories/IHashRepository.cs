using static Infrastructure.Repositories.HashRepository;

namespace Infrastructure.Repositories
{
    public interface IHashRepository
    {
        Task Add(Hash hash);

        Task<HashesDto> Get();
    }
}