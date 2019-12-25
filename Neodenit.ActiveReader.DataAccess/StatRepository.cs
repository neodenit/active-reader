using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.Models;

namespace Neodenit.ActiveReader.DataAccess
{
    public class StatRepository : EFRepository<Stat>, IStatRepository
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
