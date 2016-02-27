using ActiveReader.Interfaces;
using ActiveReader.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveReader.Services
{
    public class WordsService : IWordsService
    {
        private readonly IRepository<Word> wordRepository;
        private readonly IConverter converter;

        public WordsService(IRepository<Article> articleRepository, IRepository<Stat> statRepository, IRepository<Word> wordRepository, IConverter converter)
        {
            this.wordRepository = wordRepository;

            this.converter = converter;
        }

        public async Task AddWordsFromArticle(IArticle article)
        {
            var words = converter.GetWords(article);

            wordRepository.Create(words.Cast<Word>());

            await wordRepository.SaveAsync();
        }
    }
}
