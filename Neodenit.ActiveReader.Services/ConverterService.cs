using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Neodenit.ActiveReader.Common;
using Neodenit.ActiveReader.Common.DataModels;
using Neodenit.ActiveReader.Common.Interfaces;

namespace Neodenit.ActiveReader.Services
{
    public class ConverterService : IConverterService
    {
        public IEnumerable<Word> GetWords(Article article)
        {
            IEnumerable<string> words = GetWords(article.Text, article.IgnorePunctuation);
            IEnumerable<string> spaces = GetSpaces(article.Text, article.IgnorePunctuation);

            var pairsCount = words.Count();
            var positions = Enumerable.Range(Constants.StartingPosition, pairsCount);

            var wordsSpaces = words
                .Zip(spaces, (word, space) =>
                    new { word, space })
                .Zip(positions, (wordSpace, i) =>
                    new { Word = wordSpace.word, Space = wordSpace.space, Position = i });

            var result = wordsSpaces.Select(ws => new Word
            {
                Position = ws.Position,
                OriginalWord = ws.Word,
                CorrectedWord = NormalizeWord(ws.Word, article.IgnoreCase),
                NextSpace = ws.Space,
                ArticleId = article.Id
            });

            return result;
        }

        public string GetText(IEnumerable<Word> words) =>
            string.Join(string.Empty, words.Select(w => w.OriginalWord + w.NextSpace));

        public string GetTextAlt(IEnumerable<Word> words) =>
            string.Join(string.Empty, GetTextParts(words));

        public IEnumerable<string> GetTextParts(IEnumerable<Word> words)
        {
            foreach (var word in words)
            {
                yield return word.OriginalWord;
                yield return word.NextSpace;
            }
        }

        public IEnumerable<Stat> GetExpressions(IEnumerable<Word> words, int prefixLength, bool ignoreCase) =>
            words.GetPairs(
                w => GetPrefix(w.Select(x => x.CorrectedWord), ignoreCase),
                w => GetSuffix(w.CorrectedWord, ignoreCase),
                (word, prefix, suffix) => new Stat
                {
                    Prefix = prefix,
                    Suffix = suffix,
                    ArticleId = word.ArticleId,
                    SuffixPosition = word.Position
                },
                prefixLength);

        public string GetPrefix(IEnumerable<string> words, bool ignoreCase) =>
            string.Join(Constants.PrefixDelimiter, words.Select(w => NormalizeWord(w, ignoreCase)));

        public string GetSuffix(string word, bool ignoreCase) =>
            NormalizeWord(word, ignoreCase);

        public IEnumerable<string> GetSpaces(string text, bool ignorePunctuation) =>
            Regex.Split(text, ignorePunctuation ?  @"\w+" : @"\S+").Skip(1);

        public IEnumerable<string> GetWords(string text, bool ignorePunctuation) =>
            Regex.Split(text, ignorePunctuation ? @"\W+" : @"\s+");

        public string NormalizeWord(string word, bool ignoreCase) =>
            ignoreCase ? word.ToLowerInvariant() : word;

        public IEnumerable<string> SplitPrefix(string prefix) =>
            prefix.Split(Constants.PrefixDelimiter);
    }
}