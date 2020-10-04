using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neodenit.ActiveReader.Common.Attributes;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.ViewModels;

namespace Neodenit.ActiveReader.Web.Angular.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticlesService articlesService;
        private readonly IImportService importService;

        private static Dictionary<string, CancellationTokenSource> tokenSources = new Dictionary<string, CancellationTokenSource>();

        public ArticlesController(IArticlesService articlesService, IImportService importService)
        {
            this.articlesService = articlesService ?? throw new ArgumentNullException(nameof(articlesService));
            this.importService = importService ?? throw new ArgumentNullException(nameof(importService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleViewModel>>> Get()
        {
            IEnumerable<ArticleViewModel> articles = await articlesService.GetArticlesAsync(User.Identity.Name);
            return Ok(articles);
        }

        [ValidateModel]
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleViewModel>> GetArticle([CheckOwner]int id)
        {
            ArticleViewModel article = await articlesService.GetAsync(id);
            return Ok(article);
        }

        [HttpGet("defaultsettings")]
        public ActionResult<DefaultSettingsViewModel> GetDefaultSettings() =>
            articlesService.GetDefaultSettings();

        [HttpGet("import")]
        public async Task<ActionResult<ImportArticleViewModel>> Import([FromQuery] Uri url) =>
            await importService.GetTextAndTitleAsync(url);

        [ValidateModel]
        [HttpPost]
        public async Task<ActionResult<ArticleViewModel>> Post(ArticleViewModel article)
        {
            _ = article ?? throw new ArgumentNullException(nameof(article));

            tokenSources[article.Title] = new CancellationTokenSource();

            ArticleViewModel newArticle = await articlesService.CreateAsync(article, User.Identity.Name, tokenSources[article.Title].Token);

            tokenSources.Remove(article.Title);

            return CreatedAtAction(nameof(GetArticle), new { id = newArticle.ID }, newArticle);
        }

        [ValidateModel]
        [HttpPost("{id}/position/{position}")]
        public async Task<ActionResult> Post([CheckOwner]int id, int position)
        {
            await articlesService.UpdatePositionAsync(id, position);

            return Ok();
        }

        [ValidateModel]
        [HttpPost("{id}/restart")]
        public async Task<ActionResult> Post([CheckOwner]int id)
        {
            await articlesService.RestartUpdateAsync(id);

            return Ok();
        }

        [ValidateModel]
        [HttpPut]
        public async Task<ActionResult> Put(ArticleViewModel article)
        {
            _ = article ?? throw new ArgumentNullException(nameof(article));

            tokenSources[article.Title] = new CancellationTokenSource();

            await articlesService.UpdateAsync(article, User.Identity.Name, tokenSources[article.Title].Token);

            tokenSources.Remove(article.Title);

            return Ok();
        }

        [ValidateModel]
        [HttpPost("navigate")]
        public async Task<ActionResult> Navigate(NavigationViewModel model)
        {
            int position = await articlesService.NavigateAsync(model);

            return Ok(position);
        }        
        
        [ValidateModel]
        [HttpPost("cancel")]
        public async Task<ActionResult> Cancel(ArticleViewModel article)
        {
            _ = article ?? throw new ArgumentNullException(nameof(article));

            if (tokenSources.ContainsKey(article.Title))
            {
                var tokenSource = tokenSources[article.Title];

                if (tokenSource.Token.CanBeCanceled)
                {
                    tokenSource.Cancel();
                }
            }

            if (article.ID.HasValue)
            {
                await articlesService.Fail(article.ID.Value);
            }

            return Ok();
        }

        [ValidateModel]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ArticleViewModel>> Delete([CheckOwner]int id)
        {
            await articlesService.DeleteAsync(id);

            return NoContent();
        }
    }
}