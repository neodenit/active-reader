using System;
using System.Collections.Generic;
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
        private readonly IArticlesRepository articlesRepository;
        private readonly IConverterService converterService;

        public QuestionsService(IStatRepository statRepository, IRepository<Word> wordRepository, IArticlesRepository articlesRepository, IConverterService converterService)
        {
            this.statRepository = statRepository ?? throw new ArgumentNullException(nameof(statRepository));
            this.wordRepository = wordRepository ?? throw new ArgumentNullException(nameof(wordRepository));
            this.articlesRepository = articlesRepository ?? throw new ArgumentNullException(nameof(articlesRepository));

            this.converterService = converterService ?? throw new ArgumentNullException(nameof(converterService));
        }

        public async Task<QuestionViewModel> GetQuestionAsync(int articleID, int position)
        {
            Article article = await articlesRepository.GetAsync(articleID);

            var lastPosition = Math.Max(position, article.Position);

            IEnumerable<Word> words = wordRepository.GetAll()
                .Where(w => w.ArticleID == articleID)
                .OrderBy(w => w.Position);

            IEnumerable<Stat> expressions = converterService.GetExpressions(words, article.PrefixLength)
                                       .Where(e => e.SuffixPosition >= lastPosition);

            IEnumerable<Stat> statistics = await statRepository.GetByArticleAsync(articleID);

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
                    string text = converterService.GetText(wordsForText);

                    var question = new QuestionViewModel
                    {
                        Answer = expression.Suffix,
                        AnswerPosition = expression.SuffixPosition,
                        ArticleID = expression.ArticleID,
                        Variants = bestVariants,
                        StartingWords = text
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
                StartingWords = converterService.GetText(words)
            };
        }
    }
}
