﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IStatRepository : IRepository<Stat>
    {
        Task<IEnumerable<Stat>> GetByArticleAsync(int articleId);

        void DeleteFromArticle(int articleId);
    }
}
