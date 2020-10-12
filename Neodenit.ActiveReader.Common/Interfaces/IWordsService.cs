using System.Threading;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IWordsService
    {
        Task AddWordsFromArticleAsync(Article article, CancellationToken token = default);

        Task<int> GetPreviousPosition(int articleId, int position);

        Task<int> GetNextPosition(int articleId, int position);

        Task<int> GetEndPosition(int articleId, int position);

        Task DeleteWordsFromArticleAsync(int articleId, CancellationToken token = default);
    }
}