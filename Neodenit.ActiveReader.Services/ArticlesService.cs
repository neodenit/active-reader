using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Neodenit.ActiveReader.Common;
using Neodenit.ActiveReader.Common.DataModels;
using Neodenit.ActiveReader.Common.Enums;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.ViewModels;

namespace Neodenit.ActiveReader.Services
{
    public class ArticlesService : IArticlesService
    {
        private readonly IMapper mapper;
        private readonly IArticlesRepository repository;
        private readonly IWordsService wordsService;
        private readonly IExpressionsService expressionsService;
        private readonly ILogger<ArticlesService> logger;

        public ArticlesService(IMapper mapper, IArticlesRepository repository, IWordsService wordsService, IExpressionsService expressionsService, ILogger<ArticlesService> logger)
        {
            this.mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
            this.repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            this.wordsService = wordsService ?? throw new System.ArgumentNullException(nameof(wordsService));
            this.expressionsService = expressionsService ?? throw new System.ArgumentNullException(nameof(expressionsService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ArticleViewModel> CreateAsync(ArticleViewModel articleViewModel, string userName, CancellationToken token)
        {
            var article = mapper.Map<ArticleViewModel, Article>(
                articleViewModel,
                opt => opt.AfterMap((src, dest) =>
                {
                    dest.Owner = userName;
                    dest.Position = Constants.StartingPosition;
                    dest.State = ArticleState.Processing;
                }));

            await repository.CreateAsync(article);
            await repository.SaveAsync(token);

            try
            {
                await expressionsService.AddExpressionsFromArticleAsync(article);

                await wordsService.AddWordsFromArticleAsync(article);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Empty);

                article.State = ArticleState.Failed;
                await repository.SaveAsync(token);
                throw;
            }

            article.State = ArticleState.Processed;
            await repository.SaveAsync(token);

            var viewModel = mapper.Map<ArticleViewModel>(article);
            return viewModel;
        }

        public async Task DeleteAsync(int id)
        {
            Article article = await repository.GetAsync(id);

            if (article.State == ArticleState.Processing)
            {
                throw new InvalidOperationException();
            }

            repository.Delete(article);

            await repository.SaveAsync();
        }

        public async Task<IEnumerable<ArticleViewModel>> GetArticlesAsync(string userName)
        {
            IEnumerable<Article> articles = await repository.GetArticlesAsync(userName);
            var viewModel = mapper.Map<IEnumerable<ArticleViewModel>>(articles);
            return viewModel;
        }

        public ArticleViewModel Get(int id)
        {
            Article article = repository.Get(id);
            var viewModel = mapper.Map<ArticleViewModel>(article);
            return viewModel;
        }

        public async Task<ArticleViewModel> GetAsync(int id)
        {
            Article article = await repository.GetAsync(id);
            var viewModel = mapper.Map<ArticleViewModel>(article);
            return viewModel;
        }

        public async Task<int> NavigateAsync(NavigationViewModel model)
        {
            Article article = await repository.GetAsync(model.ArticleId);

            if (article.State != ArticleState.Processed)
            {
                throw new InvalidOperationException();
            }

            int newPosition = model.Target switch
            {
                NavigationTarget.Start => Constants.StartingPosition,
                NavigationTarget.Previous => await wordsService.GetPreviousPosition(model.ArticleId, article.Position),
                NavigationTarget.Next => await wordsService.GetNextPosition(model.ArticleId, article.Position),
                NavigationTarget.End => await wordsService.GetEndPosition(model.ArticleId, article.Position),
                _ => throw new NotImplementedException()
            };

            await UpdatePositionAsync(model.ArticleId, newPosition);

            return newPosition;
        }

        public async Task UpdatePositionAsync(int articleId, int position)
        {
            Article article = await repository.GetAsync(articleId);

            article.Position = position;

            await repository.SaveAsync();
        }

        public DefaultSettingsViewModel GetDefaultSettings() =>
            new DefaultSettingsViewModel
            {
                PrefixLength = CoreSettings.Default.PrefixLength,
                PrefixLengthMinOption = CoreSettings.Default.PrefixLengthMinOption,
                PrefixLengthMaxOption = CoreSettings.Default.PrefixLengthMaxOption,
                AnswerLength = CoreSettings.Default.AnswerLength,
                AnswerLengthMinOption = CoreSettings.Default.AnswerLengthMinOption,
                AnswerLengthMaxOption = CoreSettings.Default.AnswerLengthMaxOption,
                MaxChoices = CoreSettings.Default.MaxChoices,
                MaxChoicesMinOption = CoreSettings.Default.MaxChoicesMinOption,
                MaxChoicesMaxOption = CoreSettings.Default.MaxChoicesMaxOption
            };

        public async Task UpdateAsync(ArticleViewModel articleViewModel, string userName, CancellationToken token)
        {
            var article = mapper.Map<ArticleViewModel, Article>(
                articleViewModel,
                opt => opt.AfterMap((src, dest) =>
                {
                    dest.Owner = userName;
                    dest.Position = Constants.StartingPosition;
                }));

            Article dbArticle = repository.Get(article.Id);

            if (article.Text != dbArticle.Text ||
                article.PrefixLength != dbArticle.PrefixLength ||
                article.AnswerLength != dbArticle.AnswerLength ||
                article.IgnoreCase != dbArticle.IgnoreCase ||
                article.IgnorePunctuation != dbArticle.IgnorePunctuation)
            {
                article.State = ArticleState.Processing;
                await repository.UpdateAsync(article, article.Id);
                await repository.SaveAsync(token);

                try
                {
                    await expressionsService.DeleteExpressionsFromArticleAsync(article.Id);
                    await expressionsService.AddExpressionsFromArticleAsync(article);

                    await wordsService.DeleteWordsFromArticleAsync(article.Id);
                    await wordsService.AddWordsFromArticleAsync(article);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, string.Empty);

                    article.State = ArticleState.Failed;
                    await repository.UpdateAsync(article, article.Id);
                    await repository.SaveAsync(token);
                    throw;
                }
            }

            article.State = ArticleState.Processed;
            await repository.UpdateAsync(article, article.Id);
            await repository.SaveAsync(token);
        }

        public async Task RestartUpdateAsync(int id)
        {
            Article article = repository.Get(id);

            article.State = ArticleState.Processing;
            await repository.SaveAsync();

            try
            {
                await expressionsService.DeleteExpressionsFromArticleAsync(article.Id);
                await expressionsService.AddExpressionsFromArticleAsync(article);

                await wordsService.DeleteWordsFromArticleAsync(article.Id);
                await wordsService.AddWordsFromArticleAsync(article);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Empty);

                article.State = ArticleState.Failed;
                await repository.SaveAsync();
                throw;
            }

            article.State = ArticleState.Processed;
            await repository.SaveAsync();
        }

        public async Task Fail(int id)
        {
            Article article = repository.Get(id);

            article.State = ArticleState.Failed;

            await repository.SaveAsync();
        }
    }
}
