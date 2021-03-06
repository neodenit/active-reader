﻿using System.Collections.Generic;
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

        public void DeleteFromArticle(int articleId) =>
            dbSet.RemoveRange(dbSet.Where(x => x.ArticleId == articleId));
    }
}
