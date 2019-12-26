using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.Models;

namespace Neodenit.ActiveReader.DataAccess
{
    public class ArticlesRepository : EFRepository<Article>, IArticlesRepository
    {
        public ArticlesRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Article>> GetArticlesAsync(string userName) =>
            await dbSet.Where(x => x.Owner == userName).ToListAsync();
    }
}
