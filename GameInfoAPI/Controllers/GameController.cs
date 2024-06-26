using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameInfoAPI.DTOs;
using GameInfoAPI.Entities;
using GameInfoAPI.Repositories;

[ApiController]
[Route("api/games")]
public class GameController : ControllerBase
{
    private readonly IGameRepository _gameRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IPlayerRepository _playerRepository;

    public GameController(IGameRepository gameRepository, IAuthorRepository authorRepository, IPlayerRepository playerRepository)
    {
        _gameRepository = gameRepository;
        _authorRepository = authorRepository;
        _playerRepository = playerRepository;
    }

    // GET: api/games
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDTO>>> GetGames()
    {
        var games = await _gameRepository.GetAllAsync();

        var gameDTOs = games.Select(game => new GameDTO
        {
            Id = game.Id,
            Title = game.Title,
            Description = game.Description,
            ReleaseDate = game.ReleaseDate,
            Author = new AuthorDTO { Id = game.AuthorId, Name = game.Author.Name }, // Assuming AuthorId and Name for DTO
            BestPlayer = new PlayerDTO { Id = game.BestPlayerId, Name = game.BestPlayer.Name } // Assuming BestPlayerId and Name for DTO
        }).ToList();

        return Ok(gameDTOs);
    }

    // GET: api/games/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<GameDTO>> GetGame(int id)
    {
        var game = await _gameRepository.GetByIdAsync(id);

        if (game == null)
        {
            return NotFound();
        }

        var gameDTO = new GameDTO
        {
            Id = game.Id,
            Title = game.Title,
            Description = game.Description,
            ReleaseDate = game.ReleaseDate,
            Author = new AuthorDTO { Id = game.AuthorId, Name = game.Author.Name }, // Assuming AuthorId and Name for DTO
            BestPlayer = new PlayerDTO { Id = game.BestPlayerId, Name = game.BestPlayer.Name } // Assuming BestPlayerId and Name for DTO
        };

        return gameDTO;
    }

    // POST: api/games
    [HttpPost]
    public async Task<ActionResult<GameDTO>> CreateGame(GameDTO gameDTO)
    {
        var author = await _authorRepository.GetOrCreateAsync(gameDTO.Author.Id, gameDTO.Author.Name);
        var bestPlayer = await _playerRepository.GetOrCreateAsync(gameDTO.BestPlayer.Id, gameDTO.BestPlayer.Name);

        var game = new Game
        {
            Title = gameDTO.Title,
            Description = gameDTO.Description,
            ReleaseDate = gameDTO.ReleaseDate,
            AuthorId = author.Id,
            BestPlayerId = bestPlayer.Id
        };

        await _gameRepository.CreateAsync(game);

        var createdGameDTO = new GameDTO
        {
            Id = game.Id,
            Title = game.Title,
            Description = game.Description,
            ReleaseDate = game.ReleaseDate,
            Author = new AuthorDTO { Id = author.Id, Name = author.Name },
            BestPlayer = new PlayerDTO { Id = bestPlayer.Id, Name = bestPlayer.Name }
        };

        return CreatedAtAction(nameof(GetGame), new { id = createdGameDTO.Id }, createdGameDTO);
    }

    // PUT: api/games/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGame(int id, GameDTO gameDTO)
    {
        if (id != gameDTO.Id)
        {
            return BadRequest();
        }

        var existingGame = await _gameRepository.GetByIdAsync(id);

        if (existingGame == null)
        {
            return NotFound();
        }

        var author = await _authorRepository.GetOrCreateAsync(gameDTO.Author.Id, gameDTO.Author.Name);
        var bestPlayer = await _playerRepository.GetOrCreateAsync(gameDTO.BestPlayer.Id, gameDTO.BestPlayer.Name);

        existingGame.Title = gameDTO.Title;
        existingGame.Description = gameDTO.Description;
        existingGame.ReleaseDate = gameDTO.ReleaseDate;
        existingGame.AuthorId = author.Id;
        existingGame.BestPlayerId = bestPlayer.Id;

        await _gameRepository.UpdateAsync(existingGame);

        return NoContent();
    }

    // DELETE: api/games/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        var game = await _gameRepository.GetByIdAsync(id);

        if (game == null)
        {
            return NotFound();
        }

        await _gameRepository.DeleteAsync(game);

        return NoContent();
    }
}