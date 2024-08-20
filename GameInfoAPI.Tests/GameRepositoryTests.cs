using GameInfoAPI.Data;
using GameInfoAPI.Entities;
using GameInfoAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GameInfoAPI.Tests
{
    public class GameRepositoryTests
    {
        private DbContextOptions<DataContext> CreateInMemoryDatabaseOptions()
        {
            return new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private async Task SeedData(DataContext context)
        {
            var author = new Author { Id = 1, Name = "Author 1" };
            var bestPlayer = new Player { Id = 1, Name = "Best Player" };
            await context.Authors.AddAsync(author);
            await context.Players.AddAsync(bestPlayer);
            await context.SaveChangesAsync();

            var games = new List<Game>
            {
                new Game { Id = 1, Title = "Game 1", AuthorId = author.Id, BestPlayerId = bestPlayer.Id },
                new Game { Id = 2, Title = "Game 2", AuthorId = author.Id, BestPlayerId = bestPlayer.Id }
            };

            await context.Games.AddRangeAsync(games);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsGame()
        {
            // Arrange
            var dbContextOptions = CreateInMemoryDatabaseOptions();

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                await SeedData(arrangeContext);
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new GameRepository(actContext);
                var result = await repository.GetByIdAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Game 1", result.Title);
                Assert.NotNull(result.Author);
                Assert.NotNull(result.BestPlayer);
                Assert.Equal("Author 1", result.Author.Name);
                Assert.Equal("Best Player", result.BestPlayer.Name);
            }
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var dbContextOptions = CreateInMemoryDatabaseOptions();

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                await SeedData(arrangeContext);
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new GameRepository(actContext);
                var result = await repository.GetByIdAsync(999); // Invalid ID

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllGames()
        {
            // Arrange
            var dbContextOptions = CreateInMemoryDatabaseOptions();

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                await SeedData(arrangeContext);
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new GameRepository(actContext);
                var games = await repository.GetAllAsync();

                // Assert
                Assert.Equal(2, games.Count);
                Assert.Contains(games, g => g.Title == "Game 1");
                Assert.Contains(games, g => g.Title == "Game 2");
            }
        }

        [Fact]
        public async Task CreateAsync_AddsGameToDatabase()
        {
            // Arrange
            var dbContextOptions = CreateInMemoryDatabaseOptions();

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                var author = new Author { Id = 1, Name = "Author 1" };
                var bestPlayer = new Player { Id = 1, Name = "Best Player" };
                await arrangeContext.Authors.AddAsync(author);
                await arrangeContext.Players.AddAsync(bestPlayer);
                await arrangeContext.SaveChangesAsync();
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new GameRepository(actContext);
                var newGame = new Game
                {
                    Id = 3,
                    Title = "New Game",
                    AuthorId = 1,
                    BestPlayerId = 1
                };
                await repository.CreateAsync(newGame);
                var result = await repository.GetByIdAsync(3);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("New Game", result.Title);
            }
        }

        [Fact]
        public async Task UpdateAsync_UpdatesGameInDatabase()
        {
            // Arrange
            var dbContextOptions = CreateInMemoryDatabaseOptions();

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                await SeedData(arrangeContext);
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new GameRepository(actContext);
                var gameToUpdate = await repository.GetByIdAsync(1);

                gameToUpdate.Title = "Updated Game";
                await repository.UpdateAsync(gameToUpdate);
                var result = await repository.GetByIdAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Updated Game", result.Title);
            }
        }

        [Fact]
        public async Task DeleteAsync_RemovesGameFromDatabase()
        {
            // Arrange
            var dbContextOptions = CreateInMemoryDatabaseOptions();

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                await SeedData(arrangeContext);
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new GameRepository(actContext);
                var gameToDelete = await repository.GetByIdAsync(1); // Haal het game-object op dat verwijderd moet worden

                await repository.DeleteAsync(gameToDelete); // Geef het object door aan de DeleteAsync-methode
                var result = await repository.GetByIdAsync(1);

                // Assert
                Assert.Null(result);
            }
        }
    }
}