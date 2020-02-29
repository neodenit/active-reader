using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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

        public ArticlesService(IMapper mapper, IArticlesRepository repository, IWordsService wordsService, IExpressionsService expressionsService)
        {
            this.mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
            this.repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            this.wordsService = wordsService ?? throw new System.ArgumentNullException(nameof(wordsService));
            this.expressionsService = expressionsService ?? throw new System.ArgumentNullException(nameof(expressionsService));
        }

        public async Task<ArticleViewModel> CreateAsync(ArticleViewModel articleViewModel, string userName)
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
            await repository.SaveAsync();

            try
            {
                await expressionsService.AddExpressionsFromArticleAsync(article);

                await wordsService.AddWordsFromArticleAsync(article);
            }
            catch (Exception)
            {
                article.State = ArticleState.Failed;
                await repository.SaveAsync();
                throw;
            }

            article.State = ArticleState.Processed;
            await repository.SaveAsync();

            var viewModel = mapper.Map<ArticleViewModel>(article);
            return viewModel;

        }

        public async Task DeleteAsync(int id)
        {
            Article article = await repository.GetAsync(id);

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

        public async Task<int> Navigate(NavigationViewModel model)
        {
            Article article = await repository.GetAsync(model.ArticleId);

            int newPosition = model.Target switch
            {
                NavigationTarget.Start => Constants.StartingPosition,
                NavigationTarget.Previous => await wordsService.GetPreviousPosition(model.ArticleId, article.Position),
                NavigationTarget.Next => await wordsService.GetNextPosition(model.ArticleId, article.Position),
                NavigationTarget.End => await wordsService.GetEndPosition(model.ArticleId, article.Position),
                _ => throw new NotImplementedException()
            };

            article.Position = newPosition;

            await repository.SaveAsync();

            return newPosition;
        }

        public async Task UpdatePositionAsync(int articleId, int position)
        {
            Article article = await repository.GetAsync(articleId);

            article.Position = position;

            await repository.SaveAsync();
        }
    }
}
