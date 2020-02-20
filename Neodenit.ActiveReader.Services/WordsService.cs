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
        private readonly IWordRepository wordRepository;
        private readonly IConverterService converterService;

        public WordsService(IWordRepository wordRepository, IConverterService converterService)
        {
            this.wordRepository = wordRepository;

            this.converterService = converterService;
        }

        public async Task AddWordsFromArticleAsync(Article article)
        {
            IEnumerable<Word> words = converterService.GetWords(article);

            await wordRepository.CreateAsync(words);

            await wordRepository.SaveAsync();
        }

        public async Task<int> GetPreviousPosition(int articleId, int position)
        {
            IEnumerable<Word> articleWords = await wordRepository.GetByArticleAsync(articleId);
            var orderedWords = articleWords.OrderBy(w => w.Position);

            var lineBreaks = orderedWords.Where(w => w.NextSpace.Contains(Constants.LineBreak) && w.Position <= position);

            var lastLineBreak = lineBreaks.Reverse().Skip(1).FirstOrDefault();
            var newPosition = lastLineBreak?.Position + 1 ?? Constants.StartingPosition;
            return newPosition;
        }

        public async Task<int> GetNextPosition(int articleId, int position)
        {
            IEnumerable<Word> articleWords = await wordRepository.GetByArticleAsync(articleId);
            var orderedWords = articleWords.OrderBy(w => w.Position);

            var lastLineBreak = orderedWords.FirstOrDefault(w => w.NextSpace.Contains(Constants.LineBreak) && w.Position > position);
            var newPosition = lastLineBreak == null ? orderedWords.Last().Position + 1 : lastLineBreak.Position + 1;
            return newPosition;
        }

        public async Task<int> GetEndPosition(int articleId, int position)
        {
            IEnumerable<Word> articleWords = await wordRepository.GetByArticleAsync(articleId);
            var orderedWords = articleWords.OrderBy(w => w.Position);

            var lastWord = orderedWords.Last();
            var newPosition = lastWord.Position + 1;
            return newPosition;
        }
    }
}
