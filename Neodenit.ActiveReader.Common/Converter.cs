using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.Models;

namespace Neodenit.ActiveReader.Common
{
    public class Converter : IConverter
    {
        public IEnumerable<Word> GetWords(Article article)
        {
            var words = GetWords(article.Text);
            var spaces = GetSpaces(article.Text);
            var pairsCount = words.Count();
            var positions = Enumerable.Range(1, pairsCount);

            var wordsSpaces = words
                .Zip(spaces, (word, space) =>
                    new { word, space })
                .Zip(positions, (wordSpace, i) =>
                    new { Word = wordSpace.word, Space = wordSpace.space, Position = i });

            var result = wordsSpaces.Select(ws => new Word
            {
                Position = ws.Position,
                OriginalWord = ws.Word,
                CorrectedWord = NormalizeWord(ws.Word),
                NextSpace = ws.Space,
                ArticleID = article.ID,
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

        public IEnumerable<Stat> GetExpressions(IEnumerable<Word> words)
        {
            return words.GetPairs(
                w => GetPrefix(w.Select(x => x.CorrectedWord)),
                w => GetSuffix(w.CorrectedWord),
                (word, prefix, suffix) => new Stat
                {
                    Prefix = prefix,
                    Suffix = suffix,
                    ArticleID = word.ArticleID,
                    SuffixPosition = word.Position
                });
        }

        public string GetPrefix(IEnumerable<string> words) =>
            string.Join(CoreSettings.Default.PrefixDelimiter, words.Select(NormalizeWord));

        public string GetSuffix(string word) =>
            NormalizeWord(word);

        public IEnumerable<string> GetSpaces(string text) =>
            Regex.Split(text, @"\w+").Skip(1);

        public IEnumerable<string> GetWords(string text) =>
            Regex.Split(text, @"\W+");

        public string NormalizeWord(string word) =>
            word.ToLowerInvariant();

        public IEnumerable<string> SplitPrefix(string prefix) =>
            prefix.Split(new string[] { CoreSettings.Default.PrefixDelimiter }, StringSplitOptions.None);
    }
}