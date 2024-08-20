using GameInfoAPI.Data;
using GameInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameInfoAPI.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly DataContext _context;

        public PlayerRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Player> GetByIdAsync(int id)
        {
            return await _context.Players.FindAsync(id);
        }

        public async Task<List<Player>> GetAllAsync()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task CreateAsync(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Player player)
        {
            _context.Players.Update(player);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Player player)
        {
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
        }

        public async Task<Player> GetOrCreateAsync(int id, string name)
        {
            var existingPlayer = await _context.Players.FindAsync(id);

            if (existingPlayer != null)
            {
                return existingPlayer;
            }
            else
            {
                var newPlayer = new Player { Id = id, Name = name };
                _context.Players.Add(newPlayer);
                await _context.SaveChangesAsync();
                return newPlayer;
            }
        }
    }
}