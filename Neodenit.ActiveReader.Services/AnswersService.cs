﻿using System;
using System.Collections.Generic;
using System.Linq;
using Neodenit.ActiveReader.Common;
using Neodenit.ActiveReader.Common.DataModels;
using Neodenit.ActiveReader.Common.Interfaces;

namespace Neodenit.ActiveReader.Services
{
    public class AnswersService : IAnswersService
    {
        private readonly IStatisticsService statisticsService;

        public AnswersService(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));
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

            var pairs = firstWordChoices.SelectMany(first =>
                GetSingleWordChoices(statistics, statisticsService.GetNextExpressionPrefix(first))
                    .Select(second => new KeyValuePair<Stat, Stat>(first, second)));

            var validPairs = pairs.Where(p => !p.Key.Suffix.ContainsSentenceBreak());

            var result = validPairs.Select(p => new Stat
            {
                SuffixFirstWord = p.Key.Suffix,
                Suffix = $"{p.Key.Suffix} {p.Value.Suffix}",
                Probability = statisticsService.GetProbability(statistics, p.Key) * statisticsService.GetProbability(statistics, p.Value)
            });

            return result;
        }

        public IEnumerable<string> GetBestChoices(string correctAnswer, string correctAnswerFirstWord, IEnumerable<Stat> allChoices, int maxChoices, int answerLength)
        {
            var altChoices = allChoices.Where(c => c.Suffix != correctAnswer && c.SuffixFirstWord != correctAnswerFirstWord);
            var maxAltChoicesCount = maxChoices - 1;

            if (altChoices.Count() <= maxAltChoicesCount)
            {
                var result = altChoices.Select(c => c.Suffix).Append(correctAnswer).OrderBy(_ => Guid.NewGuid());
                return result;
            }
            else
            {
                var bestAltChoices = statisticsService.GetWeightedChoices(altChoices, maxAltChoicesCount, answerLength);

                var result = bestAltChoices.Append(correctAnswer).OrderBy(_ => Guid.NewGuid());
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
