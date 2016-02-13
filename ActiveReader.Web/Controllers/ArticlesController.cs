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
using ActiveReader.Persistence.Models;

namespace ActiveReader.Web.Controllers
{
    public class ArticlesController : ApiController
    {
        private ActiveReaderDbContext db = new ActiveReaderDbContext();

        // GET: api/Articles
        public IQueryable<Article> GetArticles()
        {
            return db.Articles;
        }

        //GET: api/Articles/5
        [ResponseType(typeof(Article))]
        public async Task<IHttpActionResult> GetArticle(int id)
        {
            Article article = await db.Articles.FindAsync(id);
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

            db.Entry(article).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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

            db.Articles.Add(article);

            var words = System.Text.RegularExpressions.Regex.Split(article.Text, @"\W+");

            db.Statistics.Add(new Stat { Prefix = "", Suffix = "" });

            var prefixLenght = 2;

            var prefixExpression = new Queue<string>(words.Take(prefixLenght));

            var rest = words.Skip(2);

            var delimeter = " ";

            foreach (var word in rest)
            {
                var prefix = string.Join(delimeter, prefixExpression);
                var suffix = word;

                var oldStat = await db.Statistics.FirstOrDefaultAsync(x => x.Prefix == prefix && x.Suffix == suffix);

                var stat = oldStat == null ?
                    new Stat { Prefix = prefix, Suffix = suffix, Number = 1 } :
                    new Stat { Prefix = prefix, Suffix = suffix, Number = oldStat.Number + 1 };

                db.Statistics.Add(stat);

                prefixExpression.Enqueue(word);
                prefixExpression.Dequeue();
            }

            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = article.ID }, article);
        }

        // DELETE: api/Articles/5
        [ResponseType(typeof(Article))]
        public async Task<IHttpActionResult> DeleteArticle(int id)
        {
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            db.Articles.Remove(article);
            await db.SaveChangesAsync();

            return Ok(article);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArticleExists(int id)
        {
            return db.Articles.Count(e => e.ID == id) > 0;
        }
    }
}