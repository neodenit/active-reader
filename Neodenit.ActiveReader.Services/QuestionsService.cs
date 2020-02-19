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
        private readonly IWordRepository wordRepository;
        private readonly IArticlesRepository articlesRepository;
        private readonly IConverterService converterService;

        public QuestionsService(IStatRepository statRepository, IWordRepository wordRepository, IArticlesRepository articlesRepository, IConverterService converterService)
        {
            this.statRepository = statRepository ?? throw new ArgumentNullException(nameof(statRepository));
            this.wordRepository = wordRepository ?? throw new ArgumentNullException(nameof(wordRepository));
            this.articlesRepository = articlesRepository ?? throw new ArgumentNullException(nameof(articlesRepository));

            this.converterService = converterService ?? throw new ArgumentNullException(nameof(converterService));
        }

        public async Task<QuestionViewModel> GetQuestionAsync(int articleId, int position)
        {
            Article article = await articlesRepository.GetAsync(articleId);

            var lastPosition = Math.Max(position, article.Position);

            IEnumerable<Word> articleWords = await wordRepository.GetByArticleAsync(articleId);
            var orderedWords = articleWords.OrderBy(w => w.Position);

            IEnumerable<Stat> expressions = converterService.GetExpressions(orderedWords, article.PrefixLength)
                                       .Where(e => e.SuffixPosition >= lastPosition);

            IEnumerable<Stat> statistics = await statRepository.GetByArticleAsync(articleId);

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
                        .Take(article.MaxChoices)
                        .OrderBy(_ => Guid.NewGuid())
                        .Select(v => v.Suffix);

                    var wordsForText = orderedWords.Where(w => w.Position < expression.SuffixPosition);
                    string text = converterService.GetText(wordsForText);

                    var question = new QuestionViewModel
                    {
                        Answer = expression.Suffix,
                        AnswerPosition = expression.SuffixPosition,
                        ArticleId = expression.ArticleId,
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
                ArticleId = articleId,
                Variants = null,
                StartingWords = converterService.GetText(orderedWords)
            };
        }
    }
}
