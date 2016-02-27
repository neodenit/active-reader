using ActiveReader.Interfaces;
using ActiveReader.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveReader.Persistence
{
    public class StatRepository : EFRepository<Stat>, IStatRepository<Stat>
    {
        public StatRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Stat>> GetByArticleAsync(int articleID)
        {
            return await dbSet.Where(x => x.ArticleID == articleID).ToListAsync();
        }

        public Task<Stat> GetByPrefixSuffixArticleAsync(string prefix, string suffix, int articleID)
        {
            return dbSet.SingleOrDefaultAsync(x => x.Prefix == prefix && x.Suffix == suffix && x.ArticleID == articleID);
        }
    }
}
