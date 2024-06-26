using GameInfoAPI.Data;
using GameInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;
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
            return await _context.Games.FindAsync(id);
        }

        public async Task<List<Game>> GetAllAsync()
        {
            return await _context.Games.ToListAsync();
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

        public async Task<Game> GetOrCreateAsync(int id, string name)
        {
            var existingGame = await _context.Games.FindAsync(id);

            if (existingGame != null)
            {
                return existingGame;
            }
            else
            {
                var newGame = new Game { Id = id, Title = name };
                _context.Games.Add(newGame);
                await _context.SaveChangesAsync();
                return newGame;
            }
        }
    }
}