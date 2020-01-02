using System;
using System.Linq;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common;
using Neodenit.ActiveReader.Common.DataModels;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.ViewModels;

namespace Neodenit.ActiveReader.Services
{
    public class QuestionsService : IQuestionsService
    {
        private readonly IStatRepository statRepository;
        private readonly IRepository<Word> wordRepository;
        private readonly IConverterService converterService;

        public QuestionsService(IStatRepository statRepository, IRepository<Word> wordRepository, IConverterService converterService)
        {
            this.statRepository = statRepository;
            this.wordRepository = wordRepository;

            this.converterService = converterService;
        }

        public async Task<QuestionViewModel> GetQuestionAsync(int articleID, int lastAnswerPosition)
        {
            var words = wordRepository.GetAll()
                                      .Where(w => w.ArticleID == articleID)
                                      .OrderBy(w => w.Position);

            var expressions = converterService.GetExpressions(words)
                                       .Where(e => e.SuffixPosition > lastAnswerPosition);

            var statistics = await statRepository.GetByArticleAsync(articleID);

            foreach (var expression in expressions)
            {
                var variants = statistics.Where(s => s.Prefix == expression.Prefix);
                var variantsCount = variants.Count();

                if (variantsCount > 1)
                {
                    var bestVariants = variants
                        .OrderByDescending(x => x.Suffix == expression.Suffix)
                        .ThenByDescending(x => x.Count)
                        .ThenBy(_ => Guid.NewGuid())
                        .Take(CoreSettings.Default.MaxChoices)
                        .OrderBy(_ => Guid.NewGuid())
                        .Select(v => v.Suffix);

                    var wordsForText = words.Where(w => w.Position < expression.SuffixPosition);
                    var text = converterService.GetText(wordsForText);

                    var question = new QuestionViewModel
                    {
                        Answer = expression.Suffix,
                        AnswerPosition = expression.SuffixPosition,
                        ArticleID = expression.ArticleID,
                        Variants = bestVariants,
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
                StartingWords = converterService.GetText(words),
            };
        }
    }
}
