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
    public class AuthorController : ControllerBase
    {
        private readonly DataContext _context;
        public AuthorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAllAuthors()
        {
            var authors = await _context.Authors.ToListAsync();
            return Ok(authors);
        }

        [HttpPost]
        public async Task<ActionResult<Author>> AddAuthor([FromBody] Author author)
        {
            // Ensure AuthorId is not set
            author.AuthorId = 0;

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return Ok(author);
        }
    }
}