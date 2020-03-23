using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common;
using Neodenit.ActiveReader.Common.DataModels;
using Neodenit.ActiveReader.Common.Interfaces;

namespace Neodenit.ActiveReader.Services
{
    public class ExpressionsService : IExpressionsService
    {
        private readonly IStatRepository repository;
        private readonly IStatManagerService statManagerService;

        public ExpressionsService(IStatRepository repository, IStatManagerService statManagerService)
        {
            this.repository = repository;
            this.statManagerService = statManagerService;
        }

        public async Task AddExpressionsFromArticleAsync(Article article)
        {
            IEnumerable<Stat> expressions = statManagerService.GetExpressions(article);
            await repository.CreateAsync(expressions);

            if (CoreSettings.Default.CountWords)
            {
                IEnumerable<Stat> words = statManagerService.GetWords(article);
                await repository.CreateAsync(words);
            }

            await repository.SaveAsync();
        }

        public async Task DeleteExpressionsFromArticleAsync(int articleId)
        {
            repository.DeleteFromArticle(articleId);

            await repository.SaveAsync();
        }
    }
}
