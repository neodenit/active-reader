using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.DataModels;
using Neodenit.ActiveReader.Common.Interfaces;

namespace Neodenit.ActiveReader.Services
{
    public class ExpressionsService : IExpressionsService
    {
        private readonly IRepository<Stat> repository;
        private readonly IStatManagerService statManagerService;

        public ExpressionsService(IRepository<Stat> repository, IStatManagerService statManagerService)
        {
            this.repository = repository;
            this.statManagerService = statManagerService;
        }

        public async Task AddExpressionsFromArticleAsync(Article article)
        {
            IEnumerable<Stat> expressions = statManagerService.GetExpressions(article);
            IEnumerable<Stat> words = statManagerService.GetWords(article);

            await repository.CreateAsync(expressions);
            await repository.CreateAsync(words);

            await repository.SaveAsync();
        }
    }
}
