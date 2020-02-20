using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IWordsService
    {
        Task AddWordsFromArticleAsync(Article article);

        Task<int> GetPreviousPosition(int articleId, int position);

        Task<int> GetNextPosition(int articleId, int position);

        Task<int> GetEndPosition(int articleId, int position);
    }
}