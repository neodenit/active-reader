using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.DataModels;
using Neodenit.ActiveReader.Common.Enums;
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

            if (article.State != ArticleState.Processed)
            {
                throw new InvalidOperationException();
            }

            var lastPosition = Math.Max(position, article.Position);

            IEnumerable<Word> articleWords = await wordRepository.GetByArticleAsync(articleId);
            var orderedWords = articleWords.OrderBy(w => w.Position);

            IEnumerable<Stat> expressions = converterService.GetExpressions(orderedWords, article.PrefixLength, article.IgnoreCase)
                                       .Where(e => e.SuffixPosition >= lastPosition);

            IEnumerable<Stat> statistics = await statRepository.GetByArticleAsync(articleId);

            foreach (var expression in expressions)
            {
                var choices = statistics.Where(s => s.Prefix == expression.Prefix);
                var choicesCount = choices.Count();

                if (choicesCount > 1)
                {
                    var bestChoices = choices
                        .OrderByDescending(x => x.Suffix == expression.Suffix)
                        .ThenByDescending(x => x.Count)
                        .ThenBy(_ => Guid.NewGuid())
                        .Take(article.MaxChoices)
                        .OrderBy(_ => Guid.NewGuid())
                        .Select(v => v.Suffix);

                    var correctedPosition = lastPosition - 1;

                    var startingWords = orderedWords.Where(w => w.Position < correctedPosition);
                    var newWords = orderedWords.Where(w => w.Position >= correctedPosition && w.Position < expression.SuffixPosition);

                    string startingText = converterService.GetText(startingWords);
                    string newText = converterService.GetText(newWords);
                    

                    var question = new QuestionViewModel
                    {
                        CorrectAnswer = expression.Suffix,
                        AnswerPosition = expression.SuffixPosition,
                        ArticleId = expression.ArticleId,
                        Choices = bestChoices,
                        StartingText = startingText,
                        NewText = newText
                    };

                    return question;
                }
            }

            return new QuestionViewModel
            {
                AnswerPosition = 0,
                ArticleId = articleId,
                StartingText = converterService.GetText(orderedWords)
            };
        }
    }
}
