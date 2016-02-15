using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ActiveReader.Web.Models;
using ActiveReader.Interfaces;
using ActiveReader.Models.Models;

namespace ActiveReader.Web.Controllers
{
    public class ArticlesController : ApiController
    {
        private readonly IRepository<Article> repository;
        private readonly IStatCollector statCollector;

        public ArticlesController(IRepository<Article> repository, IStatCollector statCollector)
        {
            this.repository = repository;
            this.statCollector = statCollector;
        }

        // GET: api/Articles
        public IQueryable<Article> GetArticles()
        {
            return repository.Get();
        }

        //GET: api/Articles/5
        [ResponseType(typeof(Article))]
        public async Task<IHttpActionResult> GetArticle(int id)
        {
            Article article = await repository.GetAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        // PUT: api/Articles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutArticle(int id, Article article)
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
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Articles
        [ResponseType(typeof(Article))]
        public async Task<IHttpActionResult> PostArticle(Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repository.Create(article);

            statCollector.Collect(article.Text);

            await repository.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = article.ID }, article);
        }

        // DELETE: api/Articles/5
        [ResponseType(typeof(Article))]
        public async Task<IHttpActionResult> DeleteArticle(int id)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArticleExists(int id)
        {
            return repository.Get().Count(e => e.ID == id) > 0;
        }
    }
}