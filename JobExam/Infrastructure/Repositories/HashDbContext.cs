using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class HashDbContext : DbContext
    {
        public DbSet<Hash> Hash { get; set; }

        public HashDbContext(DbContextOptions<HashDbContext> options) : base(options)
        {

        }
    }
}
