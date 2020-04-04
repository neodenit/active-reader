using System.Collections.Generic;
using System.Linq;
using Neodenit.ActiveReader.Common;
using Neodenit.ActiveReader.Common.DataModels;
using Neodenit.ActiveReader.Common.Interfaces;

namespace Neodenit.ActiveReader.Services
{
    public class StatManagerService : IStatManagerService
    {
        private readonly IConverterService converterService;

        public StatManagerService(IConverterService converterService)
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

            var statDict = pairs.GroupBy(p => p).Select(p => new { Pair = p.Key, Count = p.Count()});

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
    }
}
