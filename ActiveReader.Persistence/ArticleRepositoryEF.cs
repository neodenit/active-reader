using ActiveReader.Interfaces;
using ActiveReader.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveReader.Persistence
{
    public class ArticleRepositoryEF : IRepository<Article>
    {
        private ActiveReaderDbContext db = new ActiveReaderDbContext();

        public IQueryable<Article> Get()
        {
            return db.Articles;
        }

        public Article Get(int id)
        {
            return db.Articles.Find(id);
        }

        public Task<Article> GetAsync(int id)
        {
            return db.Articles.FindAsync(id);
        }

        public void Create(Article entity)
        {
            db.Articles.Add(entity);
        }

        public void Update(Article entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Article entity)
        {
            db.Articles.Remove(entity);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public Task SaveAsync()
        {
            return db.SaveChangesAsync();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
