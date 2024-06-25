using GameInfoAPI.Data;
using GameInfoAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameInfoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly DataContext _context;
        public GameController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Game>>> GetAllGames()
        {
            var games = await _context.Games.Include(g => g.Players).Include(g => g.Author).ToListAsync();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.Games.Include(g => g.Players).Include(g => g.Author).FirstOrDefaultAsync(g => g.GameId == id);
            if (game == null)
            {
                return NotFound("Game Not Found.");
            }

            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<Game>> AddGame(Game game)
        {
            var author = await _context.Authors.FindAsync(game.Author.AuthorId);
            if (author == null)
            {
                return BadRequest("Invalid AuthorId.");
            }
            game.Author = author;

            var players = new List<Player>();
            foreach (var player in game.Players)
            {
                var dbPlayer = await _context.Players.FindAsync(player.PlayerId);
                if (dbPlayer == null)
                {
                    return BadRequest($"Invalid PlayerId: {player.PlayerId}");
                }
                players.Add(dbPlayer);
            }
            game.Players = players;

            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return Ok(game);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Game>> UpdateGame(int id, Game updatedGame)
        {
            var dbGame = await _context.Games.Include(g => g.Players).Include(g => g.Author).FirstOrDefaultAsync(g => g.GameId == id);
            if (dbGame == null)
            {
                return NotFound("Game Not Found.");
            }

            dbGame.GameTitle = updatedGame.GameTitle;
            dbGame.GameAgeRestriction = updatedGame.GameAgeRestriction;
            dbGame.GameDescription = updatedGame.GameDescription;

            dbGame.Author = await _context.Authors.FindAsync(updatedGame.Author.AuthorId);
            if (dbGame.Author == null)
            {
                return BadRequest("Invalid AuthorId.");
            }

            var players = new List<Player>();
            foreach (var player in updatedGame.Players)
            {
                var dbPlayer = await _context.Players.FindAsync(player.PlayerId);
                if (dbPlayer == null)
                {
                    return BadRequest($"Invalid PlayerId: {player.PlayerId}");
                }
                players.Add(dbPlayer);
            }

            dbGame.Players = players;

            await _context.SaveChangesAsync();
            return Ok(dbGame);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGame(int id)
        {
            var dbGame = await _context.Games.FindAsync(id);
            if (dbGame == null)
            {
                return NotFound("Game Not Found.");
            }

            _context.Games.Remove(dbGame);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}