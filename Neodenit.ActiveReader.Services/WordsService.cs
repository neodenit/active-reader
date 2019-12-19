using System.Linq;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.Models;

namespace Neodenit.ActiveReader.Services
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

        public async Task AddWordsFromArticle(Article article)
        {
            var words = converter.GetWords(article);

            wordRepository.Create(words.Cast<Word>());

            await wordRepository.SaveAsync();
        }
    }
}
