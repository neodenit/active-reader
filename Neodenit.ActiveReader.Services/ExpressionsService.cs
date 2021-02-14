using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common;
using Neodenit.ActiveReader.Common.DataModels;
using Neodenit.ActiveReader.Common.Interfaces;

namespace Neodenit.ActiveReader.Services
{
    public class ExpressionsService : IExpressionsService
    {
        private readonly IStatRepository repository;
        private readonly IStatisticsService statisticsService;

        public ExpressionsService(IStatRepository repository, IStatisticsService statisticsService)
        {
            this.repository = repository;
            this.statisticsService = statisticsService;
        }

        public async Task AddExpressionsFromArticleAsync(Article article, CancellationToken token)
        {
            IEnumerable<Stat> expressions = statisticsService.GetExpressionStat(article);
            await repository.CreateAsync(expressions);

            IEnumerable<Stat> prefixes = statisticsService.GetPrefixStat(article);
            await repository.CreateAsync(prefixes);

            if (CoreSettings.Default.CountSuffixes)
            {
                IEnumerable<Stat> suffixes = statisticsService.GetSuffixStat(article);
                await repository.CreateAsync(suffixes);
            }

            await repository.SaveAsync(token);
        }

        public async Task DeleteExpressionsFromArticleAsync(int articleId, CancellationToken token)
        {
            repository.DeleteFromArticle(articleId);

            await repository.SaveAsync(token);
        }
    }
}
