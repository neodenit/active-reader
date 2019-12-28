using System.Linq;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.Models;

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

        public async Task AddExpressionsFromArticle(Article article)
        {
            var expressions = statManagerService.GetExpressions(article);
            
            repository.Create(expressions.Cast<Stat>());

            await repository.SaveAsync();
        }
    }
}
