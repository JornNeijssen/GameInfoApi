using GameInfoAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameInfoAPI.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author> GetByIdAsync(int id);
        Task<List<Author>> GetAllAsync();
        Task CreateAsync(Author author);
        Task UpdateAsync(Author author);
        Task DeleteAsync(Author author);
        Task<Author> GetOrCreateAsync(int id, string name);
    }
}