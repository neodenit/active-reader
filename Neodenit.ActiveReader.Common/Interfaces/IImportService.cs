using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.ViewModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IImportService
    {
        Task<ImportArticleViewModel> GetTextAndTitleAsync(string escapedUrl);
    }
}