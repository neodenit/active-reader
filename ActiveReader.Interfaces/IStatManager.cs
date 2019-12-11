using System.Collections.Generic;
using ActiveReader.Models.Models;

namespace ActiveReader.Interfaces
{
    public interface IStatManager
    {
        IEnumerable<Stat> GetExpressions(Article article);
    }
}