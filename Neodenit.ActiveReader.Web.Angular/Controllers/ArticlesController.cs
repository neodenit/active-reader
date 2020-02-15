using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neodenit.ActiveReader.Common;
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
            this.articlesService = articlesService ?? throw new System.ArgumentNullException(nameof(articlesService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleViewModel>>> Get()
        {
            IEnumerable<ArticleViewModel> articles = await articlesService.GetArticlesAsync(User.Identity.Name);
            return Ok(articles);
        }

        [HttpGet("{id}", Name = "GetArticle")]
        public async Task<ActionResult<ArticleViewModel>> Get(int id)
        {
            ArticleViewModel article = await articlesService.GetAsync(id);
            return Ok(article);
        }

        [HttpGet("defaultprefixlength")]
        public ActionResult<int> GetPrefixLenght()
        {
            return CoreSettings.Default.PrefixLength;
        }

        [HttpPost]
        public async Task<ActionResult<ArticleViewModel>> Post(ArticleViewModel article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ArticleViewModel newArticle = await articlesService.CreateAsync(article, User.Identity.Name);

            return CreatedAtRoute("GetArticle", new { id = newArticle.ID }, newArticle);
        }

        [HttpPost("{id}/position/{position}")]
        public async Task<ActionResult> Post(int id, int position)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await articlesService.UpdatePositionAsync(id, position);

            return Ok();
        }
        
        [HttpPost("navigate")]
        public async Task<ActionResult> Navigate(NavigationViewModel model)
        {
            int position = await articlesService.Navigate(model);

            return Ok(position);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ArticleViewModel>> Delete(int id)
        {
            await articlesService.DeleteAsync(id);

            return NoContent();
        }
    }
}