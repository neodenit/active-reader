using System.Collections.Generic;
using System.Threading.Tasks;
using ActiveReader.Models.Models;

namespace ActiveReader.Interfaces
{
    public interface IStatRepository<T> : IRepository<T> where T : Stat
    {
        Task<T> GetByPrefixSuffixArticleAsync(string prefix, string suffix, int articleID);
        Task<IEnumerable<T>> GetByArticleAsync(int articleID);
    }
}
