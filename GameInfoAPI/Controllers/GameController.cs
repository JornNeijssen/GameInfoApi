using GameInfoAPI.DTOs;
using GameInfoAPI.Entities;
using GameInfoAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDTO>>> GetGames()
    {
        var games = await _gameRepository.GetAllAsync();
        var gameDTOs = new List<GameDTO>();

        foreach (var game in games)
        {
            var author = await _authorRepository.GetByIdAsync(game.AuthorId);
            var bestPlayer = await _playerRepository.GetByIdAsync(game.BestPlayerId);

            gameDTOs.Add(new GameDTO
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                ReleaseDate = game.ReleaseDate,
                AuthorId = game.AuthorId,
                BestPlayerId = game.BestPlayerId,
                Author = author == null ? null : new AuthorDTO { Id = author.Id, Name = author.Name },
                BestPlayer = bestPlayer == null ? null : new PlayerDTO { Id = bestPlayer.Id, Name = bestPlayer.Name }
            });
        }

        return Ok(gameDTOs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GameDTO>> GetGame(int id)
    {
        var game = await _gameRepository.GetByIdAsync(id);

        if (game == null)
        {
            return NotFound();
        }

        var author = await _authorRepository.GetByIdAsync(game.AuthorId);
        var bestPlayer = await _playerRepository.GetByIdAsync(game.BestPlayerId);

        var gameDTO = new GameDTO
        {
            Id = game.Id,
            Title = game.Title,
            Description = game.Description,
            ReleaseDate = game.ReleaseDate,
            AuthorId = game.AuthorId,
            BestPlayerId = game.BestPlayerId,
            Author = author == null ? null : new AuthorDTO { Id = author.Id, Name = author.Name },
            BestPlayer = bestPlayer == null ? null : new PlayerDTO { Id = bestPlayer.Id, Name = bestPlayer.Name }
        };

        return gameDTO;
    }

    [HttpPost]
    public async Task<ActionResult<GameDTO>> CreateGame(GameDTO gameDTO)
    {
        var author = await _authorRepository.GetByIdAsync(gameDTO.AuthorId);
        var bestPlayer = await _playerRepository.GetByIdAsync(gameDTO.BestPlayerId);

        if (author == null || bestPlayer == null)
        {
            return BadRequest("Author or BestPlayer does not exist.");
        }

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
            AuthorId = game.AuthorId,
            BestPlayerId = game.BestPlayerId,
            Author = new AuthorDTO { Id = author.Id, Name = author.Name },
            BestPlayer = new PlayerDTO { Id = bestPlayer.Id, Name = bestPlayer.Name }
        };

        return CreatedAtAction(nameof(GetGame), new { id = createdGameDTO.Id }, createdGameDTO);
    }

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

        var author = await _authorRepository.GetByIdAsync(gameDTO.AuthorId);
        var bestPlayer = await _playerRepository.GetByIdAsync(gameDTO.BestPlayerId);

        if (author == null || bestPlayer == null)
        {
            return BadRequest("Author or BestPlayer does not exist.");
        }

        existingGame.Title = gameDTO.Title;
        existingGame.Description = gameDTO.Description;
        existingGame.ReleaseDate = gameDTO.ReleaseDate;
        existingGame.AuthorId = author.Id;
        existingGame.BestPlayerId = bestPlayer.Id;

        await _gameRepository.UpdateAsync(existingGame);

        return NoContent();
    }

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