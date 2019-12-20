using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.Models;

namespace ActiveReader.Web.Controllers.API
{
    [ApiController]
    [Route("[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IRepository<Article> repository;
        private readonly IWordsService wordsService;
        private readonly IExpressionsService expressionsService;

        public ArticlesController(IRepository<Article> repository, IWordsService wordsService, IExpressionsService expressionsService)
        {
            this.repository = repository;
            this.wordsService = wordsService;
            this.expressionsService = expressionsService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Article>> Get()
        {
            IEnumerable<Article> articles = repository.Get();
            return Ok(articles);
        }

        [HttpGet("{id}")]
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

            repository.Create(article);

            await repository.SaveAsync();

            await expressionsService.AddExpressionsFromArticle(article);

            await wordsService.AddWordsFromArticle(article);

            return CreatedAtRoute("DefaultApi", new { id = article.ID }, article);
        }

        [HttpDelete]
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