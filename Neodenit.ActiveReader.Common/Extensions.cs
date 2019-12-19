using System;
using System.Collections.Generic;
using System.Linq;

namespace Neodenit.ActiveReader.Common
{
    public static class Extensions
    {
        public static IEnumerable<TResult> GetPairs<TSource, TResult, TPrefix, TSuffix>(
            this IEnumerable<TSource> source,
            Func<IEnumerable<TSource>, TPrefix> prefixSelector,
            Func<TSource, TSuffix> suffixSelector,
            Func<TSource, TPrefix, TSuffix, TResult> resultSelector)
        {
            var prefixExpression = new Queue<TSource>(source.Take(CoreSettings.Default.PrefixLength));
            var rest = source.Skip(CoreSettings.Default.PrefixLength);

            foreach (var word in rest)
            {
                var prefix = prefixSelector(prefixExpression);
                var suffix = suffixSelector(word);

                yield return resultSelector(word, prefix, suffix);

                prefixExpression.Enqueue(word);
                prefixExpression.Dequeue();
            }
        }
    }
}
