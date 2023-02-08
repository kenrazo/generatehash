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
            var dbContextOptions = new DbContextOptionsBuilder<HashDbContext>()
                .UseSqlServer("Server=localhost, 1433;Initial Catalog=hash;Persist Security Info=False;User ID=sa;Password=8Waystop;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;")
                .Options;

            using (var context = new HashDbContext(dbContextOptions))
            {
                await context.Hash.AddAsync(hash);
                await context.SaveChangesAsync();
            }        
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
