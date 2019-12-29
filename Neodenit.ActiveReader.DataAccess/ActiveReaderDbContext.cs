using Microsoft.EntityFrameworkCore;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.DataAccess
{
    public class ActiveReaderDbContext : DbContext
    {
        public ActiveReaderDbContext(DbContextOptions<ActiveReaderDbContext> options) : base(options) { }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Stat> Statistics { get; set; }

        public DbSet<Word> Words { get; set; }
    }
}