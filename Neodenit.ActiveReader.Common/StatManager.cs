using System.Collections.Generic;
using System.Linq;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.Models;

namespace Neodenit.ActiveReader.Common
{
    public class StatManager : IStatManager
    {
        private readonly IConverter converter;

        public StatManager(IConverter converter)
        {
            this.converter = converter;
        }

        public IEnumerable<Stat> GetExpressions(Article article)
        {
            var statDict = new Dictionary<KeyValuePair<string, string>, int>();
            var words = converter.GetWords(article.Text);

            var pairs = words.GetPairs(
                converter.GetPrefix,
                converter.GetSuffix,
                (word, prefix, suffix) =>
                    new KeyValuePair<string, string>(prefix, suffix)
            );

            foreach (var pair in pairs)
            {
                int count;

                if (statDict.TryGetValue(pair, out count))
                {
                    statDict[pair]++;
                }
                else
                {
                    statDict[pair] = 1;
                }
            }

            var result = statDict.Keys.Select((key, value) =>
                new Stat
                {
                    ArticleID = article.ID,
                    Prefix = key.Key,
                    Suffix = key.Value,
                    Count = value,
                });

            return result;
        }
    }
}
