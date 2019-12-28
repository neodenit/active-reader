using System.Collections.Generic;
using Neodenit.ActiveReader.Common.Models;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IStatManagerService
    {
        IEnumerable<Stat> GetExpressions(Article article);
    }
}