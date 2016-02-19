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
            var trimmedText = DropStartingSpace(text);

            var words = GetWords(trimmedText);

            var spaces = GetSpaces(trimmedText);

            var pairsCount = Math.Min(words.Count(), spaces.Count());

            var wordsSpaces = words
                .Zip(spaces, (word, space) =>
                    new { word, space })
                .Zip(Enumerable.Range(0, pairsCount), (wordSpace, i) =>
                    new { wordSpace.word, wordSpace.space, position = i });

            foreach (var wordSpace in wordsSpaces)
            {
                var word = new Word
                {
                    Position = wordSpace.position,
                    OriginalWord = wordSpace.word,
                    CorrectedWord = CorrectWord(wordSpace.word),
                    NextSpace = wordSpace.space,
                    ArticleID = articleID,
                };

                repository.Create(word);
            }
        }

        private static string[] GetSpaces(string correctedText)
        {
            return Regex.Split(correctedText, @"\w+").Skip(1);
        }

        private static IEnumerable<string> GetWords(string correctedText)
        {
            return Regex.Split(correctedText, @"\W+");
        }

        private string DropStartingSpace(string text)
        {
            return Regex.Match(text, @"\w.+", RegexOptions.Singleline).Value;
        }

        private string CorrectWord(string word)
        {
            return word.ToLowerInvariant();
        }
    }
}