﻿using System.Collections.Generic;
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
            var statDict = new Dictionary<KeyValuePair<string, string>, int>();
            var words = converterService.GetWords(article.Text);

            var pairs = words.GetPairs(
                converterService.GetPrefix,
                converterService.GetSuffix,
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
