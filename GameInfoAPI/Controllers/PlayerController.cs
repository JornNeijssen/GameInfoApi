using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameInfoAPI.DTOs;
using GameInfoAPI.Entities;
using GameInfoAPI.Repositories;

[ApiController]
[Route("api/players")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerController(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayers()
    {
        var players = await _playerRepository.GetAllAsync();

        var playerDTOs = players.Select(player => new PlayerDTO
        {
            Id = player.Id,
            Name = player.Name
        }).ToList();

        return Ok(playerDTOs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerDTO>> GetPlayer(int id)
    {
        var player = await _playerRepository.GetByIdAsync(id);

        if (player == null)
        {
            return NotFound();
        }

        var playerDTO = new PlayerDTO
        {
            Id = player.Id,
            Name = player.Name
        };

        return playerDTO;
    }

    [HttpPost]
    public async Task<ActionResult<PlayerDTO>> CreatePlayer(PlayerDTO playerDTO)
    {
        var player = new Player
        {
            Name = playerDTO.Name
        };

        await _playerRepository.CreateAsync(player);

        var createdPlayerDTO = new PlayerDTO
        {
            Id = player.Id,
            Name = player.Name
        };

        return CreatedAtAction(nameof(GetPlayer), new { id = createdPlayerDTO.Id }, createdPlayerDTO);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer(int id, PlayerDTO playerDTO)
    {
        if (id != playerDTO.Id)
        {
            return BadRequest();
        }

        var existingPlayer = await _playerRepository.GetByIdAsync(id);

        if (existingPlayer == null)
        {
            return NotFound();
        }

        existingPlayer.Name = playerDTO.Name;

        await _playerRepository.UpdateAsync(existingPlayer);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer(int id)
    {
        var player = await _playerRepository.GetByIdAsync(id);

        if (player == null)
        {
            return NotFound();
        }

        await _playerRepository.DeleteAsync(player);

        return NoContent();
    }
}