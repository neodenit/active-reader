using ActiveReader.Interfaces;
using ActiveReader.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveReader.Services
{
    public class QuestionsService : IQuestionsService
    {
        private const int prefixLenght = 2;

        private readonly IRepository<Article> articleRepository;
        private readonly IRepository<Stat> statRepository;
        private readonly IRepository<Word> wordRepository;
        private readonly IArticleConverter converter;

        public QuestionsService(IRepository<Article> articleRepository, IRepository<Stat> statRepository, IRepository<Word> wordRepository, IArticleConverter converter)
        {
            this.articleRepository = articleRepository;
            this.statRepository = statRepository;
            this.wordRepository = wordRepository;

            this.converter = converter;
        }

        public QuestionViewModel GetFirstQuestion(QuestionViewModel model)
        {
            return new QuestionViewModel
            {
                ArticleID = model.ArticleID
            };
        }

        public int GetPosition(int position)
        {
            var minPosition = Math.Max(position, prefixLenght);

            return minPosition;
        }

        public string GetStartingWords(int articleID, int position)
        {
            var allWords = wordRepository.GetAll();

            var words = allWords
                .Where(x => x.ArticleID == articleID)
                .Where(x => x.Position <= position)
                .OrderBy(x => x.Position);

            return converter.GetText(words);
        }

        public string GetStartingWords(QuestionViewModel model)
        {

            var position = Math.Max(model.Position, prefixLenght);

            var allWords = wordRepository.GetAll();

            var words = allWords
                .Where(x => x.Position <= position)
                .OrderBy(x => x.Position);

            return converter.GetText(words);
        }

    }
}
