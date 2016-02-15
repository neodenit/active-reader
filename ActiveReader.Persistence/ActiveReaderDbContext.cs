using ActiveReader.Models.Models;
using System.Data.Entity;

namespace ActiveReader.Persistence
{
    public class ActiveReaderDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Stat> Statistics { get; set; }
    }
}