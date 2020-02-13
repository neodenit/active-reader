using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.ViewModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IArticlesService
    {
        Task<IEnumerable<ArticleViewModel>> GetArticlesAsync(string userName);

        Task<ArticleViewModel> GetAsync(int id);

        Task<ArticleViewModel> CreateAsync(ArticleViewModel articleViewModel, string userName);

        Task DeleteAsync(int id);

        Task UpdatePositionAsync(int articleId, int position);

        Task<int> Navigate(NavigationViewModel model);
    }
}