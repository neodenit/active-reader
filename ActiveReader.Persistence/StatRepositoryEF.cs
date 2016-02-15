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
    public class StatRepositoryEF : IRepository<Stat>
    {
        private ActiveReaderDbContext db = new ActiveReaderDbContext();

        public IQueryable<Stat> Get()
        {
            return db.Statistics;
        }

        public Stat Get(int id)
        {
            return db.Statistics.Find(id);
        }

        public Task<Stat> GetAsync(int id)
        {
            return db.Statistics.FindAsync(id);
        }

        public void Create(Stat entity)
        {
            db.Statistics.Add(entity);
        }

        public void Update(Stat entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Stat entity)
        {
            db.Statistics.Remove(entity);
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
