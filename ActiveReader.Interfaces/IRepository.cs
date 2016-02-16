﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveReader.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        IQueryable<T> Get();
        T Get(int id);
        Task<T> GetAsync(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
        Task SaveAsync();
    }
}