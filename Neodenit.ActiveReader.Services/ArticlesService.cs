using System.Collections.Generic;
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
        private readonly IRepository<Article> repository;
        private readonly IWordsService wordsService;
        private readonly IExpressionsService expressionsService;

        public ArticlesService(IMapper mapper, IRepository<Article> repository, IWordsService wordsService, IExpressionsService expressionsService)
        {
            this.mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
            this.repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            this.wordsService = wordsService ?? throw new System.ArgumentNullException(nameof(wordsService));
            this.expressionsService = expressionsService ?? throw new System.ArgumentNullException(nameof(expressionsService));
        }

        public async Task CreateAsync(ArticleViewModel articleViewModel)
        {
            var article = mapper.Map<Article>(articleViewModel);

            repository.Create(article);

            await repository.SaveAsync();

            await expressionsService.AddExpressionsFromArticle(article);

            await wordsService.AddWordsFromArticle(article);
        }

        public async Task DeleteAsync(ArticleViewModel articleViewModel)
        {
            var article = mapper.Map<Article>(articleViewModel);

            repository.Delete(article);

            await repository.SaveAsync();
        }

        public IEnumerable<ArticleViewModel> GetArticlesAsync(string userName)
        {
            IEnumerable<Article> articles = repository.Get();
            var viewModel = mapper.Map<IEnumerable<ArticleViewModel>>(articles);
            return viewModel;
        }

        public async Task<ArticleViewModel> GetAsync(int id)
        {
            Article article = await repository.GetAsync(id);
            var viewModel = mapper.Map<ArticleViewModel>(article);
            return viewModel;
        }
    }
}
