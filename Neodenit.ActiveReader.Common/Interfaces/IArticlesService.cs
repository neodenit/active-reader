using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.ViewModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IArticlesService
    {
        Task<IEnumerable<ArticleViewModel>> GetArticlesAsync(string userName);

        Task<ArticleViewModel> GetAsync(int id);

        Task CreateAsync(ArticleViewModel articleViewModel);

        Task DeleteAsync(int id);
    }
}