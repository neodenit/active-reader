using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<ActionResult<ArticleViewModel>> Post(ArticleViewModel article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            article.Owner = user.UserName;

            await articlesService.CreateAsync(article);

            return CreatedAtRoute("GetArticle", new { id = article.ID }, article);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ArticleViewModel>> Delete(int id)
        {
            ArticleViewModel article = await articlesService.GetAsync(id);

            await articlesService.DeleteAsync(article);

            return Ok(article);
        }
    }
}