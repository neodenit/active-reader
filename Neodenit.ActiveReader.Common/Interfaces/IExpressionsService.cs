using System.Threading;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IExpressionsService
    {
        Task AddExpressionsFromArticleAsync(Article article, CancellationToken token = default);

        Task DeleteExpressionsFromArticleAsync(int articleId, CancellationToken token = default);
    }
}