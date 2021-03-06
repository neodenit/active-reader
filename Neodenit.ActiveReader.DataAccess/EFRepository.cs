﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neodenit.ActiveReader.Common.Interfaces;

namespace Neodenit.ActiveReader.DataAccess
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext dbContext;
        protected readonly DbSet<T> dbSet;

        public EFRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public IEnumerable<T> GetAll() => dbSet.ToList();

        public async Task<IEnumerable<T>> GetAllAsync() => await dbSet.ToListAsync();

        public T Get(int id) => dbSet.Find(id);

        public async Task<T> GetAsync(int id) => await dbSet.FindAsync(id);

        public void Create(T entity)
        {
            dbSet.Add(entity);
        }

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public void Create(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }

        public async Task CreateAsync(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public async Task UpdateAsync(T entity, int id)
        {
            var originalEntity = await dbSet.FindAsync(id);
            dbContext.Entry(originalEntity).CurrentValues.SetValues(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public async Task SaveAsync(CancellationToken token)
        {
            await dbContext.SaveChangesAsync(token);
        }
    }
}
