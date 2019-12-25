using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.Models;
using Neodenit.ActiveReader.Web.Angular.Models;

namespace Neodenit.ActiveReader.Web.Angular.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IRepository<Article> repository;
        private readonly IWordsService wordsService;
        private readonly IExpressionsService expressionsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ArticlesController(IRepository<Article> repository, IWordsService wordsService, IExpressionsService expressionsService, UserManager<ApplicationUser> userManager)
        {
            this.repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            this.wordsService = wordsService ?? throw new System.ArgumentNullException(nameof(wordsService));
            this.expressionsService = expressionsService ?? throw new System.ArgumentNullException(nameof(expressionsService));
            this.userManager = userManager ?? throw new System.ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        public ActionResult<IEnumerable<Article>> Get()
        {
            IEnumerable<Article> articles = repository.Get();
            return Ok(articles);
        }

        [HttpGet("{id}", Name = "GetArticle")]
        public async Task<ActionResult<Article>> Get(int id)
        {
            Article article = await repository.GetAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != article.ID)
            {
                return BadRequest();
            }

            repository.Update(article);

            try
            {
                await repository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var articleExists = repository.Get().Any(e => e.ID == id);

                if (!articleExists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Article>> Post(Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            article.Owner = user.UserName;

            repository.Create(article);

            await repository.SaveAsync();

            await expressionsService.AddExpressionsFromArticle(article);

            await wordsService.AddWordsFromArticle(article);

            return CreatedAtRoute("GetArticle", new { id = article.ID }, article);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Article>> Delete(int id)
        {
            Article article = await repository.GetAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            repository.Delete(article);
            await repository.SaveAsync();

            return Ok(article);
        }
    }
}