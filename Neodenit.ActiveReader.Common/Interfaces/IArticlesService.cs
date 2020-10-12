using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.ViewModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IArticlesService
    {
        Task<IEnumerable<ArticleViewModel>> GetArticlesAsync(string userName);

        ArticleViewModel Get(int id);

        Task<ArticleViewModel> GetAsync(int id);

        Task<ArticleViewModel> CreateAsync(ArticleViewModel articleViewModel, string userName, CancellationToken token = default);

        Task DeleteAsync(int id);

        Task UpdatePositionAsync(int articleId, int position);

        Task<int> NavigateAsync(NavigationViewModel model);

        DefaultSettingsViewModel GetDefaultSettings();

        Task UpdateAsync(ArticleViewModel article, string userName, CancellationToken token = default);

        Task RestartUpdateAsync(int id, CancellationToken token = default);

        Task Fail(int id);
    }
}