using System.Threading.Tasks;
using ActiveReader.Interfaces;

namespace ActiveReader.Interfaces
{
    public interface IExpressionsService
    {
        Task AddExpressionsFromArticle(IArticle article);
    }
}