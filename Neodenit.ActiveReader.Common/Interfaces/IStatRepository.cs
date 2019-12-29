using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IStatRepository : IRepository<Stat>
    {
        Task<Stat> GetByPrefixSuffixArticleAsync(string prefix, string suffix, int articleID);

        Task<IEnumerable<Stat>> GetByArticleAsync(int articleID);
    }
}
