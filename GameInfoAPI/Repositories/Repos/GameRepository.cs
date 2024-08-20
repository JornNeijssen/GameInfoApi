using GameInfoAPI.Data;
using GameInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameInfoAPI.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;

        public GameRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Game> GetByIdAsync(int id)
        {
            return await _context.Games
                .Include(g => g.Author)
                .Include(g => g.BestPlayer)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<List<Game>> GetAllAsync()
        {
            return await _context.Games
                .Include(g => g.Author)
                .Include(g => g.BestPlayer)
                .ToListAsync();
        }

        public async Task CreateAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Game game)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }

        public async Task<Game> GetOrCreateAsync(int id, string title)
        {
            var existingGame = await _context.Games.FindAsync(id);

            if (existingGame != null)
            {
                return existingGame;
            }
            else
            {
                var newGame = new Game { Id = id, Title = title };
                _context.Games.Add(newGame);
                await _context.SaveChangesAsync();
                return newGame;
            }
        }
    }
}