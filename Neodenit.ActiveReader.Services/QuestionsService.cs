using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ether.WeightedSelector;
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

            var linkedListExpressions = new LinkedList<Stat>(expressions);

            for (var item = linkedListExpressions.First; item != null; item = item.Next)
            {
                var expression = item.Value;

                Stat correctAnswer = article.AnswerLength > 1
                    ? GetTwoWordAnswer(expression, item.Next?.Value)
                    : expression;

                IEnumerable<Stat> choices = article.AnswerLength > 1
                    ? GetDoubleWordChoices(statistics, correctAnswer.Prefix)
                    : GetSingleWordChoices(statistics, correctAnswer.Prefix);

                var choicesCount = article.AnswerLength > 1 && CoreSettings.Default.RandomizeFirstWord
                    ? GetSingleWordChoices(statistics, correctAnswer.Prefix).Count()
                    : choices.Count();

                if (choicesCount > 1)
                {
                    var bestChoices = GetBestChoices(correctAnswer.Suffix, choices, article.MaxChoices, article.AnswerLength);

                    var correctedPosition = lastPosition - article.AnswerLength;

                    var startingWords = orderedWords.Where(w => w.Position < correctedPosition);
                    var newWords = orderedWords.Where(w => w.Position >= correctedPosition && w.Position < expression.SuffixPosition);

                    string startingText = converterService.GetText(startingWords);
                    string newText = converterService.GetText(newWords);

                    var question = new QuestionViewModel
                    {
                        CorrectAnswer = article.AnswerLength > 1 ? correctAnswer.Suffix : expression.Suffix,
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

        private IEnumerable<Stat> GetSingleWordChoices(IEnumerable<Stat> statistics, string prefix) =>
            statistics.Where(s => s.Prefix == prefix && !string.IsNullOrEmpty(s.Suffix));

        private string GetNextExpressionPrefix(Stat expression)
        {
            var words = expression.Prefix.Split(Constants.PrefixDelimiter).Skip(1).Append(expression.Suffix);
            var prefix = string.Join(Constants.PrefixDelimiter, words);
            return prefix;
        }

        private Stat GetTwoWordAnswer(Stat expression, Stat nextExpression)
        {
            var result = new Stat
            {
                Prefix = expression.Prefix,
                Suffix = nextExpression == null ? expression.Suffix : $"{expression.Suffix} {nextExpression.Suffix}"
            };
            return result;
        }

        private IEnumerable<Stat> GetDoubleWordChoices(IEnumerable<Stat> statistics, string prefix)
        {
            var firstWordChoices = statistics.Where(s => s.Prefix == prefix && !string.IsNullOrEmpty(s.Suffix));

            var pairs = firstWordChoices.SelectMany(first => GetSingleWordChoices(statistics, GetNextExpressionPrefix(first)).Select(second => new KeyValuePair<Stat, Stat>(first, second)));

            var result = pairs.Select(p => new Stat
            {
                Suffix = $"{p.Key.Suffix} {p.Value.Suffix}",
                Probability = GetProbability(statistics, p.Key) * GetProbability(statistics, p.Value)
            });

            return result;
        }

        private double GetProbability(IEnumerable<Stat> statistics, Stat stat)
        {
            double p1 = statistics.Single(s => s.Prefix == stat.Prefix && s.Suffix == stat.Suffix).Count;
            double p2 = statistics.Single(s => s.Prefix == stat.Prefix && string.IsNullOrEmpty(s.Suffix)).Count;
            var result = p1 / p2;
            return result;
        }

        private IEnumerable<string> GetWeightedChoices(string correctAnswer, IEnumerable<Stat> allChoices, int maxChoices, int answerLength)
        {
            var precision = Math.Pow(10, CoreSettings.Default.PrecisionOrder);

            var getWeight = answerLength > 1
               ? s => (int)(s.Probability * precision)
               : (Func<Stat, int>)(s => s.Count);

            var selector = new WeightedSelector<string>();

            var weightedStat = allChoices
                .Where(c => c.Suffix != correctAnswer)
                .Select(c => new WeightedItem<string>(c.Suffix, getWeight(c)));

            selector.Add(weightedStat);

            var result = selector.SelectMultiple(maxChoices);
            return result;
        }

        private IEnumerable<string> GetBestChoices(string correctAnswer, IEnumerable<Stat> allChoices, int maxChoices, int answerLength)
        {
            if (allChoices.Count() <= maxChoices)
            {
                var result = allChoices.Select(c => c.Suffix).OrderBy(_ => Guid.NewGuid());
                return result;
            }
            else
            {
                var altChoicesCount = maxChoices - 1;
                var altChoices = GetWeightedChoices(correctAnswer, allChoices, altChoicesCount, answerLength);

                var result = altChoices.Append(correctAnswer).OrderBy(_ => Guid.NewGuid());
                return result;
            }
        }

        private IEnumerable<string> GetBestChoicesLegacy(string correctAnswer, IEnumerable<Stat> allChoices, int maxChoices) =>
            allChoices
            .OrderByDescending(x => x.Suffix == correctAnswer)
            .ThenByDescending(x => x.Count)
            .ThenBy(_ => Guid.NewGuid())
            .Take(maxChoices)
            .OrderBy(_ => Guid.NewGuid())
            .Select(c => c.Suffix);

    }
}
