using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Neodenit.ActiveReader.Common;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.ViewModels;
using Neodenit.ActiveReader.Web.Angular.Models;

namespace Neodenit.ActiveReader.Web.Angular.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticlesService articlesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ArticlesController(IArticlesService articlesService, UserManager<ApplicationUser> userManager)
        {
            this.articlesService = articlesService ?? throw new System.ArgumentNullException(nameof(articlesService));
            this.userManager = userManager ?? throw new System.ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleViewModel>>> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            IEnumerable<ArticleViewModel> articles = await articlesService.GetArticlesAsync(user.UserName);
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

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            ArticleViewModel newArticle = await articlesService.CreateAsync(article, user.UserName);

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

        [HttpDelete("{id}")]
        public async Task<ActionResult<ArticleViewModel>> Delete(int id)
        {
            await articlesService.DeleteAsync(id);

            return NoContent();
        }
    }
}