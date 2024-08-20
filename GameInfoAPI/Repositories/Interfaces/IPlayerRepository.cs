using GameInfoAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameInfoAPI.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player> GetByIdAsync(int id);
        Task<List<Player>> GetAllAsync();
        Task CreateAsync(Player player);
        Task UpdateAsync(Player player);
        Task DeleteAsync(Player player);
        Task<Player> GetOrCreateAsync(int id, string name);
    }
}