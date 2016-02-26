using ActiveReader.Interfaces;
using ActiveReader.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ActiveReader.Core
{
    public class ArticleConverter : IArticleConverter
    {
        private readonly IRepository<Word> repository;

        public ArticleConverter(IRepository<Word> repository)
        {
            this.repository = repository;
        }

        public void SaveArticle(string text, int articleID)
        {
            var words = GetWords(text);

            var spaces = GetSpaces(text);

            var pairsCount = words.Count();

            var positions = Enumerable.Range(1, pairsCount);

            var wordsSpaces = words
                .Zip(spaces, (word, space) =>
                    new { word, space })
                .Zip(positions, (wordSpace, i) =>
                    new { Word = wordSpace.word, Space = wordSpace.space, Position = i });

            foreach (var wordSpace in wordsSpaces)
            {
                var word = new Word
                {
                    Position = wordSpace.Position,
                    OriginalWord = wordSpace.Word,
                    CorrectedWord = CorrectWord(wordSpace.Word),
                    NextSpace = wordSpace.Space,
                    ArticleID = articleID,
                };

                repository.Create(word);
            }

            repository.Save();
        }

        public string GetText(IEnumerable<IWord> words)
        {
            return string.Join(string.Empty, GetTextParts(words));
        }

        public IEnumerable<string> GetTextParts(IEnumerable<IWord> words)
        {
            foreach (var word in words)
            {
                yield return word.OriginalWord;
                yield return word.NextSpace;
            }
        }

        private static IEnumerable<string> GetSpaces(string correctedText)
        {
            return Regex.Split(correctedText, @"\w+").Skip(1);
        }

        private static IEnumerable<string> GetWords(string correctedText)
        {
            return Regex.Split(correctedText, @"\W+");
        }

        private string CorrectWord(string word)
        {
            return word.ToLowerInvariant();
        }
    }
}