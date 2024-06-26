using GameInfoAPI.Data;
using GameInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GameInfoAPI.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public async Task<List<Author>> GetAllAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task CreateAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Author author)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task<Author> GetOrCreateAsync(int id, string name)
        {
            var existingAuthor = await _context.Authors.FindAsync(id);

            if (existingAuthor != null)
            {
                return existingAuthor;
            }
            else
            {
                var newAuthor = new Author { Id = id, Name = name };
                _context.Authors.Add(newAuthor);
                await _context.SaveChangesAsync();
                return newAuthor;
            }
        }
    }
}