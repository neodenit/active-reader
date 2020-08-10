using System;
using System.Collections.Generic;
using System.Linq;
using Ether.WeightedSelector;
using Neodenit.ActiveReader.Common;
using Neodenit.ActiveReader.Common.DataModels;
using Neodenit.ActiveReader.Common.Interfaces;

namespace Neodenit.ActiveReader.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IConverterService converterService;

        public StatisticsService(IConverterService converterService)
        {
            this.converterService = converterService;
        }

        public IEnumerable<Stat> GetExpressionStat(Article article)
        {
            IEnumerable<string> words = converterService.GetWords(article.Text, article.IgnorePunctuation);

            IEnumerable<KeyValuePair<string, string>> pairs = words.GetPairs(
                w => converterService.GetPrefix(w, article.IgnoreCase),
                w => converterService.GetSuffix(w, article.IgnoreCase),
                (word, prefix, suffix) =>
                    new KeyValuePair<string, string>(prefix, suffix),
                article.PrefixLength
            );

            var statDict = pairs.GroupBy(p => p).Select(p => new { Pair = p.Key, Count = p.Count() });

            var result = statDict.Select(stat =>
                new Stat
                {
                    ArticleId = article.Id,
                    Prefix = stat.Pair.Key,
                    Suffix = stat.Pair.Value,
                    Count = stat.Count
                });

            return result;
        }

        public IEnumerable<Stat> GetPrefixStat(Article article)
        {
            IEnumerable<string> words = converterService.GetWords(article.Text, article.IgnorePunctuation);

            IEnumerable<string> prefixes = words.GetPairs(
                w => converterService.GetPrefix(w, article.IgnoreCase),
                w => string.Empty,
                (word, prefix, suffix) => prefix,
                article.PrefixLength
            );

            var statDict = prefixes.GroupBy(p => p).Select(p => new { Prefix = p.Key, Count = p.Count() });

            var result = statDict.Select(stat =>
                new Stat
                {
                    ArticleId = article.Id,
                    Prefix = stat.Prefix,
                    Count = stat.Count
                });

            return result;
        }

        public IEnumerable<Stat> GetSuffixStat(Article article)
        {
            IEnumerable<string> words = converterService.GetWords(article.Text, article.IgnorePunctuation);

            IEnumerable<string> pairs = words.GetPairs(
                w => string.Empty,
                w => converterService.GetSuffix(w, article.IgnoreCase),
                (word, prefix, suffix) => suffix,
                article.PrefixLength
            );

            var statDict = pairs.GroupBy(p => p).Select(p => new { Suffix = p.Key, Count = p.Count() });

            var result = statDict.Select(stat =>
                new Stat
                {
                    ArticleId = article.Id,
                    Suffix = stat.Suffix,
                    Count = stat.Count
                });

            return result;
        }

        public string GetNextExpressionPrefix(Stat expression)
        {
            var words = expression.Prefix.Split(Constants.PrefixDelimiter).Skip(1).Append(expression.Suffix);
            var prefix = string.Join(Constants.PrefixDelimiter, words);
            return prefix;
        }

        public double GetProbability(IEnumerable<Stat> statistics, Stat stat)
        {
            double p1 = statistics.Single(s => s.Prefix == stat.Prefix && s.Suffix == stat.Suffix).Count;
            double p2 = statistics.Single(s => s.Prefix == stat.Prefix && string.IsNullOrEmpty(s.Suffix)).Count;
            var result = p1 / p2;
            return result;
        }

        public IEnumerable<string> GetWeightedChoices(IEnumerable<Stat> choices, int maxChoices, int answerLength)
        {
            var precision = Math.Pow(10, CoreSettings.Default.PrecisionOrder);

            var getWeight = answerLength > 1
               ? s => (int)(s.Probability * precision)
               : (Func<Stat, int>)(s => s.Count);

            var weightedSelector = new WeightedSelector<Stat>();

            var weightedStat = choices
                .Select(c => new WeightedItem<Stat>(c, getWeight(c)));

            weightedSelector.Add(weightedStat);

            if (answerLength > 1 && CoreSettings.Default.RandomizeFirstWord)
            {
                static IEnumerable<string> GetRandomChoices(WeightedSelector<Stat> selector)
                {
                    while (selector.ReadOnlyItems.Any())
                    {
                        var choice = selector.Select();

                        var exceptions = selector.ReadOnlyItems.Where(x => x.Value.SuffixFirstWord == choice.SuffixFirstWord).ToList();

                        foreach (var exception in exceptions)
                        {
                            selector.Remove(exception);
                        }

                        yield return choice.Suffix;
                    }
                }

                IEnumerable<string> randomChoices = GetRandomChoices(weightedSelector);
                var result = randomChoices.Take(maxChoices).ToList();
                return result;
            }
            else
            {
                var randomChoices = weightedSelector.SelectMultiple(maxChoices);
                var result = randomChoices.Select(c => c.Suffix);
                return result;
            }
        }
    }
}
