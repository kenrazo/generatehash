using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public partial class HashRepository : IHashRepository
    {
        private readonly HashDbContext _db;

        public HashRepository(HashDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task Add(Hash hash)
        {
            await _db.Hash.AddAsync(hash);
            await _db.SaveChangesAsync();
            
        }

        public async Task<HashesDto> Get()
        {
            var hashGroupByDate = await _db.Hash.GroupBy(h => h.Date.Date)
            .Select(g => new HashGroupByDayDto
            {
                Day = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

            return new HashesDto(hashGroupByDate);
        }
    }
}
