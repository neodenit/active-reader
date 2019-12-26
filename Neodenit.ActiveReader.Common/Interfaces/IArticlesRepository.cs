using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.Models;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IArticlesRepository
    {
        Task<IEnumerable<Article>> GetArticlesAsync(string userName);
    }
}
