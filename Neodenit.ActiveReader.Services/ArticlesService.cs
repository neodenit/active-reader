using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.Models;

namespace Neodenit.ActiveReader.Services
{
    public class ArticlesService : IArticlesService
    {
        private readonly IRepository<Article> repository;
        private readonly IWordsService wordsService;
        private readonly IExpressionsService expressionsService;

        public ArticlesService(IRepository<Article> repository, IWordsService wordsService, IExpressionsService expressionsService)
        {
            this.repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            this.wordsService = wordsService ?? throw new System.ArgumentNullException(nameof(wordsService));
            this.expressionsService = expressionsService ?? throw new System.ArgumentNullException(nameof(expressionsService));
        }

        public async Task CreateAsync(Article article)
        {
            repository.Create(article);

            await repository.SaveAsync();

            await expressionsService.AddExpressionsFromArticle(article);

            await wordsService.AddWordsFromArticle(article);
        }

        public async Task DeleteAsync(Article article)
        {
            repository.Delete(article);

            await repository.SaveAsync();
        }

        public IEnumerable<Article> GetArticlesAsync(string userName)
        {
            IEnumerable<Article> articles = repository.Get();
            return articles;
        }

        public Task<Article> GetAsync(int id) =>
            repository.GetAsync(id);
    }
}
