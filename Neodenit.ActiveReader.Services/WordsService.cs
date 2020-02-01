using System.Collections.Generic;
using System.Threading.Tasks;
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
    }
}
