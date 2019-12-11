using System.Threading.Tasks;
using ActiveReader.Models.Models;

namespace ActiveReader.Interfaces
{
    public interface IWordsService
    {
        Task AddWordsFromArticle(Article article);
    }
}