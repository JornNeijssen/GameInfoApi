using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameInfoAPI.Repositories
{

    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetOrCreateAsync(int id, string name); // Updated method signature
    }
}