using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IWordsService
    {
        Task AddWordsFromArticle(Article article);
    }
}