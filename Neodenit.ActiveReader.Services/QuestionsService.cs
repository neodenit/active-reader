using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common;
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
        private readonly IAnswersService answersService;

        public QuestionsService(IStatRepository statRepository, IWordRepository wordRepository, IArticlesRepository articlesRepository, IConverterService converterService, IAnswersService answersService)
        {
            this.statRepository = statRepository ?? throw new ArgumentNullException(nameof(statRepository));
            this.wordRepository = wordRepository ?? throw new ArgumentNullException(nameof(wordRepository));
            this.articlesRepository = articlesRepository ?? throw new ArgumentNullException(nameof(articlesRepository));

            this.converterService = converterService ?? throw new ArgumentNullException(nameof(converterService));
            this.answersService = answersService ?? throw new ArgumentNullException(nameof(answersService));
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

            var linkedListExpressions = new LinkedList<Stat>(expressions);
            
            for (var item = linkedListExpressions.First; item != null; item = item.Next)
            {
                var expression = item.Value;

                if (expression.Prefix.ContainsSentenceBreak() || converterService.HasMultipleSentences(expression.Suffix))
                {
                    continue;
                }

                var correctAnswerExpressions = item.Take(article.AnswerLength);

                if (correctAnswerExpressions.Count() < article.AnswerLength)
                {
                    break;
                }

                Stat correctAnswer = answersService.GetMultiWordAnswer(correctAnswerExpressions);

                var correctAnswerFirstWord = expression.Suffix;

                IEnumerable<Stat> choices = answersService.GetMultiWordChoices(statistics, correctAnswer.Prefix, article.AnswerLength);

                IEnumerable<string> bestChoices = answersService.GetBestChoices(correctAnswer.Suffix, correctAnswerFirstWord, choices, article.MaxChoices, article.AnswerLength);

                var choicesCount = bestChoices.Count();

                if (choicesCount > 1)
                {
                    var correctedPosition = lastPosition - article.AnswerLength;

                    var startingWords = orderedWords.Where(w => w.Position < correctedPosition);
                    var newWords = orderedWords.Where(w => w.Position >= correctedPosition && w.Position < expression.SuffixPosition);

                    var lastWord = newWords.Last();
                    var nextWords = orderedWords.SkipWhile(w => w.Position < expression.SuffixPosition).Take(article.AnswerLength - 1);

                    if (!lastWord.NextSpace.ContainsSentenceBreak() && !nextWords.Any(w => w.NextSpace.ContainsSentenceBreak()))
                    {
                        string startingText = converterService.GetText(startingWords);
                        string newText = converterService.GetText(newWords);

                        var endPosition = orderedWords.Last().Position;

                        var question = new QuestionViewModel
                        {
                            CorrectAnswer = article.AnswerLength > 1 ? correctAnswer.Suffix : expression.Suffix,
                            AnswerPosition = expression.SuffixPosition,
                            LastPosition = lastPosition,
                            Progress = 100 * expression.SuffixPosition / endPosition,
                            ArticleId = expression.ArticleId,
                            Choices = bestChoices,
                            StartingText = startingText,
                            NewText = newText
                        };

                        return question;
                    }
                }
            }

            return new QuestionViewModel
            {
                AnswerPosition = lastPosition,
                LastPosition = lastPosition,
                Progress = 100,
                ArticleId = articleId,
                StartingText = converterService.GetText(orderedWords)
            };
        }
    }
}
