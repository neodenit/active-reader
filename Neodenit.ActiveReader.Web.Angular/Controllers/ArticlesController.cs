using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neodenit.ActiveReader.Common;
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

        public ArticlesController(IArticlesService articlesService)
        {
            this.articlesService = articlesService ?? throw new ArgumentNullException(nameof(articlesService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleViewModel>>> Get()
        {
            IEnumerable<ArticleViewModel> articles = await articlesService.GetArticlesAsync(User.Identity.Name);
            return Ok(articles);
        }

        [ValidateModel]
        [HttpGet("{id}", Name = "GetArticle")]
        public async Task<ActionResult<ArticleViewModel>> Get([CheckOwner]int id)
        {
            ArticleViewModel article = await articlesService.GetAsync(id);
            return Ok(article);
        }

        [HttpGet("defaultprefixlength")]
        public ActionResult<int> GetPrefixLenght()
        {
            return CoreSettings.Default.PrefixLength;
        }

        [ValidateModel]
        [HttpPost]
        public async Task<ActionResult<ArticleViewModel>> Post(ArticleViewModel article)
        {
            ArticleViewModel newArticle = await articlesService.CreateAsync(article, User.Identity.Name);

            return CreatedAtRoute("GetArticle", new { id = newArticle.ID }, newArticle);
        }

        [ValidateModel]
        [HttpPost("{id}/position/{position}")]
        public async Task<ActionResult> Post([CheckOwner]int id, int position)
        {
            await articlesService.UpdatePositionAsync(id, position);

            return Ok();
        }
        
        [ValidateModel]
        [HttpPost("navigate")]
        public async Task<ActionResult> Navigate(NavigationViewModel model)
        {
            int position = await articlesService.Navigate(model);

            return Ok(position);
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