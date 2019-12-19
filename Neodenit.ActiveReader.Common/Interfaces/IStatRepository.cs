using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.Models;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IStatRepository<T> : IRepository<T> where T : Stat
    {
        Task<T> GetByPrefixSuffixArticleAsync(string prefix, string suffix, int articleID);

        Task<IEnumerable<T>> GetByArticleAsync(int articleID);
    }
}
