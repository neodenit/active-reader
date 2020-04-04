using System.Collections.Generic;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IStatManagerService
    {
        IEnumerable<Stat> GetExpressionStat(Article article);

        IEnumerable<Stat> GetPrefixStat(Article article);

        IEnumerable<Stat> GetSuffixStat(Article article);
    }
}