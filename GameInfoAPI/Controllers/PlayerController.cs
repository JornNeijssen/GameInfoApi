using GameInfoAPI.Data;
using GameInfoAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameInfoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly DataContext _context;

        public PlayerController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Player>>> GetAllPlayers()
        {
            var players = await _context.Players.ToListAsync();
            return Ok(players);
        }

        [HttpPost]
        public async Task<ActionResult<Player>> AddPlayer(Player player)
        {
            // Zorg ervoor dat PlayerId niet is ingesteld
            player.PlayerId = 0;

            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return Ok(player);
        }
    }
}