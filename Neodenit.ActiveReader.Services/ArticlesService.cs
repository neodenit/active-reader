﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Neodenit.ActiveReader.Common.DataModels;
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
                opt => opt.AfterMap((src, dest) => dest.Owner = userName));

            repository.Create(article);

            await repository.SaveAsync();

            await expressionsService.AddExpressionsFromArticle(article);

            await wordsService.AddWordsFromArticle(article);

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

        public async Task<ArticleViewModel> GetAsync(int id)
        {
            Article article = await repository.GetAsync(id);
            var viewModel = mapper.Map<ArticleViewModel>(article);
            return viewModel;
        }

        public async Task UpdatePositionAsync(int articleId, int position)
        {
            Article article = await repository.GetAsync(articleId);

            article.Position = position;

            await repository.SaveAsync();
        }
    }
}
