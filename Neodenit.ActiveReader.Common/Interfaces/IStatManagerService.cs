using System.Collections.Generic;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IStatManagerService
    {
        IEnumerable<Stat> GetExpressions(Article article);

        IEnumerable<Stat> GetWords(Article article);
    }
}