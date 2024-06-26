using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameInfoAPI.DTOs;
using GameInfoAPI.Entities;
using GameInfoAPI.Repositories;

[ApiController]
[Route("api/authors")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorController(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    // GET: api/authors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors()
    {
        var authors = await _authorRepository.GetAllAsync();

        var authorDTOs = authors.Select(author => new AuthorDTO
        {
            Id = author.Id,
            Name = author.Name
            // Add other properties as needed
        }).ToList();

        return Ok(authorDTOs);
    }

    // GET: api/authors/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorDTO>> GetAuthor(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);

        if (author == null)
        {
            return NotFound();
        }

        var authorDTO = new AuthorDTO
        {
            Id = author.Id,
            Name = author.Name
            // Add other properties as needed
        };

        return authorDTO;
    }

    // POST: api/authors
    [HttpPost]
    public async Task<ActionResult<AuthorDTO>> CreateAuthor(AuthorDTO authorDTO)
    {
        var author = new Author
        {
            Name = authorDTO.Name
            // Set other properties as needed
        };

        await _authorRepository.CreateAsync(author);

        var createdAuthorDTO = new AuthorDTO
        {
            Id = author.Id,
            Name = author.Name
            // Add other properties as needed
        };

        return CreatedAtAction(nameof(GetAuthor), new { id = createdAuthorDTO.Id }, createdAuthorDTO);
    }

    // PUT: api/authors/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(int id, AuthorDTO authorDTO)
    {
        if (id != authorDTO.Id)
        {
            return BadRequest();
        }

        var existingAuthor = await _authorRepository.GetByIdAsync(id);

        if (existingAuthor == null)
        {
            return NotFound();
        }

        existingAuthor.Name = authorDTO.Name;
        // Update other properties as needed

        await _authorRepository.UpdateAsync(existingAuthor);

        return NoContent();
    }

    // DELETE: api/authors/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);

        if (author == null)
        {
            return NotFound();
        }

        await _authorRepository.DeleteAsync(author);

        return NoContent();
    }
}