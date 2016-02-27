using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveReader.Interfaces
{
    public interface IStatManager
    {
        IEnumerable<IStat> GetExpressions(IArticle article);
    }
}