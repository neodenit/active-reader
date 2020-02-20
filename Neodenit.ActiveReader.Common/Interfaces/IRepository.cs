using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        T Get(int id);

        Task<T> GetAsync(int id);

        void Create(T entity);

        Task CreateAsync(T entity);

        void Create(IEnumerable<T> entities);

        Task CreateAsync(IEnumerable<T> entities);

        void Update(T entity);

        void Delete(T entity);

        void Save();

        Task SaveAsync();
    }
}
