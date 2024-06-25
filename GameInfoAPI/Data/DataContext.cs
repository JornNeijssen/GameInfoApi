using GameInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameInfoAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}