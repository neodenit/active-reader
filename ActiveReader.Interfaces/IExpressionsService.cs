using System.Threading.Tasks;
using ActiveReader.Models.Models;

namespace ActiveReader.Interfaces
{
    public interface IExpressionsService
    {
        Task AddExpressionsFromArticle(Article article);
    }
}