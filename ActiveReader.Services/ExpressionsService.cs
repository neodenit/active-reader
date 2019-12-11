using System.Linq;
using System.Threading.Tasks;
using ActiveReader.Interfaces;
using ActiveReader.Models.Models;

namespace ActiveReader.Services
{
    public class ExpressionsService : IExpressionsService
    {
        private readonly IRepository<Stat> repository;
        private readonly IStatManager statManager;

        public ExpressionsService(IRepository<Stat> repository, IStatManager statManager)
        {
            this.repository = repository;
            this.statManager = statManager;
        }

        public async Task AddExpressionsFromArticle(Article article)
        {
            var expressions = statManager.GetExpressions(article);
            
            repository.Create(expressions.Cast<Stat>());

            await repository.SaveAsync();
        }
    }
}
