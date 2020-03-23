using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IWordRepository : IRepository<Word>
    {
        Task<IEnumerable<Word>> GetByArticleAsync(int articleId);

        void DeleteFromArticle(int articleId);
    }
}
