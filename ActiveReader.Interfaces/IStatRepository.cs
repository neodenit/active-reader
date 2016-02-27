using ActiveReader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveReader.Interfaces
{
    public interface IStatRepository<T> : IRepository<T> where T : IStat
    {
        Task<T> GetByPrefixSuffixArticleAsync(string prefix, string suffix, int articleID);
        Task<IEnumerable<T>> GetByArticleAsync(int articleID);
    }
}
