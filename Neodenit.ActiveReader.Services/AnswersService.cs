using System;
using System.Collections.Generic;
using System.Linq;
using Neodenit.ActiveReader.Common.DataModels;
using Neodenit.ActiveReader.Common.Interfaces;

namespace Neodenit.ActiveReader.Services
{
    public class AnswersService : IAnswersService
    {
        private readonly IStatManagerService statManagerService;

        public AnswersService(IStatManagerService statManagerService)
        {
            this.statManagerService = statManagerService ?? throw new ArgumentNullException(nameof(statManagerService));
        }

        public IEnumerable<Stat> GetSingleWordChoices(IEnumerable<Stat> statistics, string prefix) =>
            statistics.Where(s => s.Prefix == prefix && !string.IsNullOrEmpty(s.Suffix));

        public Stat GetTwoWordAnswer(Stat expression, Stat nextExpression)
        {
            var result = new Stat
            {
                Prefix = expression.Prefix,
                Suffix = nextExpression == null ? expression.Suffix : $"{expression.Suffix} {nextExpression.Suffix}"
            };
            return result;
        }

        public IEnumerable<Stat> GetDoubleWordChoices(IEnumerable<Stat> statistics, string prefix)
        {
            var firstWordChoices = statistics.Where(s => s.Prefix == prefix && !string.IsNullOrEmpty(s.Suffix));

            var pairs = firstWordChoices.SelectMany(first => GetSingleWordChoices(statistics, statManagerService.GetNextExpressionPrefix(first)).Select(second => new KeyValuePair<Stat, Stat>(first, second)));

            var result = pairs.Select(p => new Stat
            {
                Suffix = $"{p.Key.Suffix} {p.Value.Suffix}",
                Probability = statManagerService.GetProbability(statistics, p.Key) * statManagerService.GetProbability(statistics, p.Value)
            });

            return result;
        }

        public IEnumerable<string> GetBestChoices(string correctAnswer, IEnumerable<Stat> allChoices, int maxChoices, int answerLength)
        {
            if (allChoices.Count() <= maxChoices)
            {
                var result = allChoices.Select(c => c.Suffix).OrderBy(_ => Guid.NewGuid());
                return result;
            }
            else
            {
                var altChoicesCount = maxChoices - 1;
                var altChoices = statManagerService.GetWeightedChoices(correctAnswer, allChoices, altChoicesCount, answerLength);

                var result = altChoices.Append(correctAnswer).OrderBy(_ => Guid.NewGuid());
                return result;
            }
        }

        public IEnumerable<string> GetBestChoicesLegacy(string correctAnswer, IEnumerable<Stat> allChoices, int maxChoices) =>
            allChoices
            .OrderByDescending(x => x.Suffix == correctAnswer)
            .ThenByDescending(x => x.Count)
            .ThenBy(_ => Guid.NewGuid())
            .Take(maxChoices)
            .OrderBy(_ => Guid.NewGuid())
            .Select(c => c.Suffix);

    }
}
