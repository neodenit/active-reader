using System.Threading.Tasks;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IImportService
    {
        Task<(string text, string title)> GetTextAndTitleAsync(string url);
    }
}