﻿using ActiveReader.Interfaces;
using ActiveReader.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveReader.Persistence
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext dbContext;
        private readonly DbSet<T> dbSet;

        public EFRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public IQueryable<T> Get()
        {
            return dbSet;
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public Task<T> GetAsync(int id)
        {
            return dbSet.FindAsync(id);
        }

        public void Create(T entity)
        {
            dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public Task SaveAsync()
        {
            return dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
