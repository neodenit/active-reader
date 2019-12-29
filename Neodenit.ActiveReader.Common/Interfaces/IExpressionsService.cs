using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IExpressionsService
    {
        Task AddExpressionsFromArticle(Article article);
    }
}