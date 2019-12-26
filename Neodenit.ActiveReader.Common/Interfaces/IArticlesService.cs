using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.Models;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IArticlesService
    {
        IEnumerable<Article> GetArticlesAsync(string userName);

        Task<Article> GetAsync(int id);

        Task CreateAsync(Article article);

        Task DeleteAsync(Article article);
    }
}