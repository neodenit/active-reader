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

        public IEnumerable<Stat> GetExpressions(Article article)
        {
            IEnumerable<string> words = converterService.GetWords(article.Text);

            IEnumerable<KeyValuePair<string, string>> pairs = words.GetPairs(
                converterService.GetPrefix,
                converterService.GetSuffix,
                (word, prefix, suffix) =>
                    new KeyValuePair<string, string>(prefix, suffix),
                article.PrefixLength
            );

            var statDict = new Dictionary<KeyValuePair<string, string>, int>();

            foreach (var pair in pairs)
            {
                if (statDict.TryGetValue(pair, out var count))
                {
                    statDict[pair]++;
                }
                else
                {
                    statDict[pair] = 1;
                }
            }

            var result = statDict.Select(stat =>
                new Stat
                {
                    ArticleId = article.Id,
                    Prefix = stat.Key.Key,
                    Suffix = stat.Key.Value,
                    Count = stat.Value
                });

            return result;
        }

        public IEnumerable<Stat> GetWords(Article article)
        {
            IEnumerable<string> words = converterService.GetWords(article.Text);

            var normalizedWords = words.Select(w => converterService.NormalizeWord(w));

            var statDict = new Dictionary<string, int>();

            foreach (var word in normalizedWords)
            {
                if (statDict.TryGetValue(word, out var count))
                {
                    statDict[word]++;
                }
                else
                {
                    statDict[word] = 1;
                }
            }

            var result = statDict.Select(stat =>
                new Stat
                {
                    ArticleId = article.Id,
                    Suffix = stat.Key,
                    Count = stat.Value
                });

            return result;
        }
    }
}
