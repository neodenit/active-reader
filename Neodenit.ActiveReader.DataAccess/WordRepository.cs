using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.DataAccess
{
    public class WordRepository : EFRepository<Word>, IWordRepository
    {
        public WordRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Word>> GetByArticleAsync(int articleId) =>
            await dbSet.Where(x => x.ArticleID == articleId).ToListAsync();
    }
}
