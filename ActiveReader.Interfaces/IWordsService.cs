using System.Threading.Tasks;

namespace ActiveReader.Interfaces
{
    public interface IWordsService
    {
        Task AddWordsFromArticle(IArticle article);
    }
}