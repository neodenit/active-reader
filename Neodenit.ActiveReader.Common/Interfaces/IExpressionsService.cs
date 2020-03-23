using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IExpressionsService
    {
        Task AddExpressionsFromArticleAsync(Article article);

        Task DeleteExpressionsFromArticleAsync(int articleId);
    }
}