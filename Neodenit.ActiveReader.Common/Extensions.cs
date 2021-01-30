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
            Func<TSource, TPrefix, TSuffix, TResult> resultSelector,
            int prefixLength)
        {
            var prefixWords = new Queue<TSource>(source.Take(prefixLength));
            var restWords = source.Skip(prefixLength);

            foreach (var word in restWords)
            {
                var prefix = prefixSelector(prefixWords);
                var suffix = suffixSelector(word);

                yield return resultSelector(word, prefix, suffix);

                prefixWords.Enqueue(word);
                prefixWords.Dequeue();
            }
        }

        public static bool ContainsSentenceBreak(this string text) =>
            text.Any(x => Constants.SentenceBreaks.Contains(x));

        public static IEnumerable<T> Take<T>(this LinkedListNode<T> listNode, int n)
        {
            if (n > 0)
            {
                yield return listNode.Value;

                var rest = listNode.Next.Take(n - 1);

                foreach (var node in rest)
                {
                    yield return node;
                }
            }
        }
    }
}
