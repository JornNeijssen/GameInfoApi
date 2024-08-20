using GameInfoAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameInfoAPI.Repositories
{
    public interface IGameRepository
    {
        Task<Game> GetByIdAsync(int id);
        Task<List<Game>> GetAllAsync();
        Task CreateAsync(Game game);
        Task UpdateAsync(Game game);
        Task DeleteAsync(Game game);
        Task<Game> GetOrCreateAsync(int id, string title);
    }
}