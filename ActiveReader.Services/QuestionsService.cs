using System.Linq;
using System.Threading.Tasks;
using ActiveReader.Interfaces;
using ActiveReader.Models.Models;

namespace ActiveReader.Services
{
    public class QuestionsService : IQuestionsService
    {
        private readonly IStatRepository<Stat> statRepository;
        private readonly IRepository<Word> wordRepository;
        private readonly IConverter converter;

        public QuestionsService(IStatRepository<Stat> statRepository, IRepository<Word> wordRepository, IConverter converter)
        {
            this.statRepository = statRepository;
            this.wordRepository = wordRepository;

            this.converter = converter;
        }

        public async Task<QuestionViewModel> GetQuestionAsync(int articleID, int lastAnswerPosition)
        {
            var words = wordRepository.GetAll()
                                      .Where(w => w.ArticleID == articleID)
                                      .OrderBy(w => w.Position);

            var expressions = converter.GetExpressions(words)
                                       .Where(e => e.SuffixPosition > lastAnswerPosition);

            var statistics = await statRepository.GetByArticleAsync(articleID);

            foreach (var expression in expressions)
            {
                var variants = statistics.Where(s => s.Prefix == expression.Prefix);
                var variantsCount = variants.Count();

                if (variantsCount > 1)
                {
                    var wordsForText = words.Where(w => w.Position < expression.SuffixPosition);
                    var text = converter.GetText(wordsForText);

                    var question = new QuestionViewModel
                    {
                        Answer = expression.Suffix,
                        AnswerPosition = expression.SuffixPosition,
                        ArticleID = expression.ArticleID,
                        Variants = variants.Select(v => v.Suffix).OrderBy(v => v),
                        StartingWords = text,
                    };

                    return question;
                }
            }

            return new QuestionViewModel
            {
                Answer = null,
                AnswerPosition = 0,
                ArticleID = articleID,
                Variants = null,
                StartingWords = converter.GetText(words),
            };
        }
    }
}
