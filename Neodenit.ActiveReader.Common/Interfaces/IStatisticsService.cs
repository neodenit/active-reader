using System.Collections.Generic;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IStatisticsService
    {
        IEnumerable<Stat> GetExpressionStat(Article article);

        IEnumerable<Stat> GetPrefixStat(Article article);

        IEnumerable<Stat> GetSuffixStat(Article article);

        string GetNextExpressionPrefix(Stat expression);

        double GetProbability(IEnumerable<Stat> statistics, Stat stat);

        IEnumerable<string> GetWeightedChoices(IEnumerable<Stat> choices, int maxChoices, int answerLength);
    }
}