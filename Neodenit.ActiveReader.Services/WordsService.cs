using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common;
using Neodenit.ActiveReader.Common.DataModels;
using Neodenit.ActiveReader.Common.Interfaces;

namespace Neodenit.ActiveReader.Services
{
    public class WordsService : IWordsService
    {
        private readonly IRepository<Word> wordRepository;
        private readonly IConverterService converterService;

        public WordsService(IRepository<Word> wordRepository, IConverterService converterService)
        {
            this.wordRepository = wordRepository;

            this.converterService = converterService;
        }

        public async Task AddWordsFromArticle(Article article)
        {
            IEnumerable<Word> words = converterService.GetWords(article);

            wordRepository.Create(words);

            await wordRepository.SaveAsync();
        }

        public async Task<int> GetPreviousPosition(int articleId, int position)
        {
            IEnumerable<Word> allWords = await wordRepository.GetAllAsync();
            IEnumerable<Word> words = allWords
                .Where(w => w.ArticleID == articleId)
                .OrderBy(w => w.Position);

            var lineBreaks = words.Where(w => w.NextSpace.Contains(Constants.LineBreak) && w.Position <= position);

            var lastLineBreak = lineBreaks.Reverse().Skip(1).FirstOrDefault();
            var newPosition = lastLineBreak?.Position ?? Constants.StartingPosition;
            return newPosition;
        }

        public async Task<int> GetNextPosition(int articleId, int position)
        {
            IEnumerable<Word> allWords = await wordRepository.GetAllAsync();
            IEnumerable<Word> words = allWords
                .Where(w => w.ArticleID == articleId)
                .OrderBy(w => w.Position);

            var lastLineBreak = words.FirstOrDefault(w => w.NextSpace.Contains(Constants.LineBreak) && w.Position > position);
            var newPosition = lastLineBreak == null ? words.Last().Position + 1 : lastLineBreak.Position + 1;
            return newPosition;
        }

        public async Task<int> GetEndPosition(int articleId, int position)
        {
            IEnumerable<Word> allWords = await wordRepository.GetAllAsync();
            IEnumerable<Word> words = allWords
                .Where(w => w.ArticleID == articleId)
                .OrderBy(w => w.Position);

            var lastWord = words.Last();
            var newPosition = lastWord.Position + 1;
            return newPosition;
        }
    }
}
