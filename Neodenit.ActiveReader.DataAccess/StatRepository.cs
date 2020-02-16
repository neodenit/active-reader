using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.DataAccess
{
    public class StatRepository : EFRepository<Stat>, IStatRepository
    {
        public StatRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Stat>> GetByArticleAsync(int articleId) =>
            await dbSet.Where(x => x.ArticleId == articleId).ToListAsync();

        public Task<Stat> GetByPrefixSuffixArticleAsync(string prefix, string suffix, int articleId) =>
            dbSet.SingleOrDefaultAsync(x => x.Prefix == prefix && x.Suffix == suffix && x.ArticleId == articleId);
    }
}
